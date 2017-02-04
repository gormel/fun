using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Android.App;
using HtmlAgilityPack;
using SuperJson;
using SuperJson.Parser;
using Object = Java.Lang.Object;

namespace RuRaReader.Model
{
    public class SaveDataManager : Object
    {
        private static readonly Lazy<SaveDataManager> mInstance = new Lazy<SaveDataManager>(() => new SaveDataManager());
        public static SaveDataManager Instance => mInstance.Value;

        [DoNotSerialise]
        const string FileName = "Data.json";
        [DoNotSerialise]
        private List<ProjectModel> mProjectsCashe;
        [DoNotSerialise]
        private readonly ConcurrentDictionary<int, List<VolumeModel>> mVolumeCashe = new ConcurrentDictionary<int, List<VolumeModel>>();
        [DoNotSerialise]
        private readonly ConcurrentDictionary<int, List<ChapterModel>> mChaptersCache = new ConcurrentDictionary<int, List<ChapterModel>>();
        [DoNotSerialise]
        private readonly ConcurrentDictionary<int, TextModel> mTextCace = new ConcurrentDictionary<int, TextModel>();
        [DoNotSerialise]
        private int mLastChapterId;
        [DoNotSerialise]
        public bool Loaded { get; private set; }

        public List<string> ReadChapterUrls { get; private set; } = new List<string>();
        public Dictionary<string, int> ChapterSctolls { get; private set; } = new Dictionary<string, int>();

        public void Load(string dir)
        {
            string readData = "";
            var fullFileName = Path.Combine(dir, FileName);

            if (File.Exists(fullFileName))
                readData = File.ReadAllText(fullFileName);

            if (string.IsNullOrEmpty(readData))
                return;

            var des = new SuperJsonSerializer();
            var data = (SaveDataManager)des.Deserialize(readData);

            ReadChapterUrls = data.ReadChapterUrls;
            ChapterSctolls = data.ChapterSctolls;
            Loaded = true;
        }

        public void Save(string dir)
        {
            var ser = new SuperJsonSerializer();
            var serData = ser.Serialize(this);
            File.WriteAllText(Path.Combine(dir, FileName), serData);
            Loaded = true;
        }

        private async Task<string> ReadUrl(string url)
        {
            var client = new HttpClient();
            var result = await client.GetAsync(url);
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<IReadOnlyList<ProjectModel>> GetProjects()
        {
            if (mProjectsCashe == null)
            {
                var source = await ReadUrl("http://ruranobe.ru/api/projects");
                dynamic des = new SuperJsonSerializer().Deserialize(source);

                mProjectsCashe = new List<ProjectModel>();
                foreach (var project in des)
                {
                    mProjectsCashe.Add(new ProjectModel(project));
                }
            }
            return mProjectsCashe;
        }

        public async Task<ProjectModel> GetProject(int id)
        {
            foreach (var model in await GetProjects())
            {
                if (model.Id == id)
                    return model;
            }
            return null;
        }

        public async Task<IReadOnlyList<VolumeModel>> GetVolumes(int projectId)
        {
            if (!mVolumeCashe.ContainsKey(projectId))
            {
                var source = await ReadUrl($"http://ruranobe.ru/api/projects/{projectId}/volumes");
                dynamic des = new SuperJsonSerializer().Deserialize(source);
                mVolumeCashe[projectId] = new List<VolumeModel>();
                foreach (var vol in des)
                {
                    var volumeModel = new VolumeModel(vol);
                    volumeModel.Project = await GetProject(projectId);
                    mVolumeCashe[projectId].Add(volumeModel);
                }
            }
            return mVolumeCashe[projectId];
        }

        public async Task<VolumeModel> GetVolume(int volumeId)
        {
            foreach (var volume in mVolumeCashe.Values.SelectMany(c => c))
            {
                if (volume.Id == volumeId)
                    return volume;
            }

            foreach (var project in await GetProjects())
            {
                foreach (var volume in await GetVolumes(project.Id))
                {
                    if (volume.Id == volumeId)
                        return volume;
                }
            }
            return null;
        }

        public async Task<IReadOnlyList<ChapterModel>> GetChapters(int volumeId)
        {
            if (!mChaptersCache.ContainsKey(volumeId))
            {
                var volume = await GetVolume(volumeId);
                var page = await ReadUrl($"http://ruranobe.ru/r/{volume.Url}");

                var pageDoc = new HtmlDocument();
                pageDoc.LoadHtml(page);
                var chapters = pageDoc.DocumentNode.Descendants("div")
                    .FirstOrDefault(d => d.GetAttributeValue("class", "<NULL>") == "read");
                mChaptersCache[volumeId] = new List<ChapterModel>();
                if (chapters != null)
                {
                    ChapterModel prevChapter = null;
                    foreach (var node in chapters.ChildNodes)
                    {
                        var chapterUrl = node.GetAttributeValue("href", ".");
                        var chapterName = node.ChildNodes[0].InnerText;

                        dynamic ch = new ExpandoObject();
                        ch.chapterTitle = $"{chapterName}";
                        ch.url = $"{volume.Url}/.{chapterUrl}";
                        lock(this)
                            ch.chapterId = mLastChapterId++;

                        var model = new ChapterModel(ch);
                        model.PrevModel = prevChapter;
                        if (prevChapter != null)
                            prevChapter.NextModel = model;
                        prevChapter = model;
                        model.Volume = volume;

                        mChaptersCache[volumeId].Add(model);
                    }
                }
            }
            return mChaptersCache[volumeId];
        }

        public async Task<ChapterModel> GetChapter(int chapterId)
        {
            foreach (var chapter in mChaptersCache.Values.SelectMany(c => c))
            {
                if (chapter.Id == chapterId)
                    return chapter;
            }

            foreach (var project in await GetProjects())
            {
                foreach (var volume in await GetVolumes(project.Id))
                {
                    foreach (var chapter in await GetChapters(volume.Id))
                    {
                        if (chapter.Id == chapterId)
                            return chapter;
                    }
                }
            }
            return null;
        }

        public async Task<TextModel> GetText(int chapterId)
        {
            if (!mTextCace.ContainsKey(chapterId))
            {
                var chapter = await GetChapter(chapterId);
                var raw = await ReadUrl($"http://ruranobe.ru/r/{chapter.Url}");

                var htDoc = new HtmlDocument();
                htDoc.LoadHtml(raw);
                var textContainer =
                    htDoc.DocumentNode.Descendants("div")
                        .FirstOrDefault(d => d.GetAttributeValue("class", null) == "text");
                if (textContainer == null)
                    return null;

                dynamic dyn = new ExpandoObject();
                dyn.textTitle = textContainer.Descendants("h2").FirstOrDefault()?.InnerText;
                var lines = textContainer.Descendants("p").Select(d => d.InnerText).ToList();
                dyn.text = lines.Any() ? lines.Aggregate((a, b) => $"{a}\n\r\t{b}") : "";

                var model = new TextModel(dyn);
                model.Chapter = chapter;
                mTextCace[chapterId] = model;
            }
            return mTextCace[chapterId];
        }
    }
}