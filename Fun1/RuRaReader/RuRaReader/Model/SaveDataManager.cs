using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RuRaReader.Model.SerializeCustomers;
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
        private readonly HttpClient mClient = new HttpClient();
        [DoNotSerialise]
        private readonly SuperJsonSerializer mSerializer = new SuperJsonSerializer();
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
        public Dictionary<int, int> ProjectOrders { get; private set; } = new Dictionary<int, int>();

        public SaveDataManager()
        {
            mSerializer.SerializeCustomers.Add(new IntToIntDictionarySerializeCustomer());
            mSerializer.DeserializeCustomers.Add(new IntToIntDictionaryDeserializeCustomer());
        }

        public void Load(string dir)
        {
            string readData = "";
            var fullFileName = Path.Combine(dir, FileName);

            if (File.Exists(fullFileName))
                readData = File.ReadAllText(fullFileName);

            if (string.IsNullOrEmpty(readData))
                return;
            
            var data = (SaveDataManager)mSerializer.Deserialize(readData);

            ReadChapterUrls = data.ReadChapterUrls;
            ChapterSctolls = data.ChapterSctolls;
            ProjectOrders = data.ProjectOrders ?? ProjectOrders;
            Loaded = true;
        }

        public void Save(string dir)
        {
            var serData = mSerializer.Serialize(this);
            File.WriteAllText(Path.Combine(dir, FileName), serData);
            Loaded = true;
        }

        private async Task<string> ReadUrl(string url)
        {
            try
            {
                var result = await mClient.GetAsync(url);
                var strResult = await result.Content.ReadAsStringAsync();
                return strResult;

            }
            catch (Exception)
            {
                return null;
            }
            //var regex = new Regex(@"\\[uU]([0-9A-Fa-f]{4})");
            //return regex.Replace(strResult, m => ((char)int.Parse(m.Value.Substring(2), NumberStyles.HexNumber)).ToString());
        }

        public async Task<IReadOnlyList<ProjectModel>> GetProjects()
        {
            if (mProjectsCashe == null)
            {
                var source = await ReadUrl("http://ruranobe.ru/api/projects");
                if (source == null)
                    return new List<ProjectModel>();
                dynamic des = mSerializer.Deserialize(source);

                mProjectsCashe = new List<ProjectModel>();
                foreach (var project in des)
                {
                    var model = new ProjectModel(project);
                    mProjectsCashe.Add(model);
                }
            }
            return mProjectsCashe;
        }

        public void TopProject(int id)
        {
            if (!ProjectOrders.ContainsKey(id))
                ProjectOrders[id] = 0;

            var max = ProjectOrders.Values.Max();
            ProjectOrders[id] = max + 1;
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
                if (source == null)
                    return new List<VolumeModel>();
                dynamic des = mSerializer.Deserialize(source);
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
                if (volume == null)
                    return new List<ChapterModel>();
                var page = await ReadUrl($"http://ruranobe.ru/r/{volume.Url}");
                if (page == null)
                    return new List<ChapterModel>();

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
                        if (chapterUrl.EndsWith("/i"))
                            continue;
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

        private bool IsHeaderTag(string name) => name.Length == 2 && name[0] == 'h' && char.IsDigit(name[1]);

        private bool IsIllustration(HtmlNode node) => node.Name == "div" && node.GetAttributeValue("class", "<NULL>").EndsWith("illustration");

        private bool IsSubtitle(HtmlNode node) => node.Name == "div" && node.GetAttributeValue("class", "<NULL>").EndsWith("subtitle");

        public async Task<TextModel> GetText(int chapterId)
        {
            if (!mTextCace.ContainsKey(chapterId))
            {
                var chapter = await GetChapter(chapterId);
                var raw = await ReadUrl($"http://ruranobe.ru/r/{chapter.Url}");
                if (raw == null)
                    return null;

                var htDoc = new HtmlDocument();
                htDoc.LoadHtml(raw);
                var textContainer =
                    htDoc.DocumentNode.Descendants("div")
                        .FirstOrDefault(d => d.GetAttributeValue("class", null) == "text");
                if (textContainer == null)
                    return null;

                var lst = new List<dynamic>();
                var text = textContainer.ChildNodes.Where(n => n.GetAttributeValue("id", null) != "i" && n.GetAttributeValue("id", null) != "footnotes");
                List<RowModel> constr = null;
                foreach (var node in text)
                {
                    if (IsHeaderTag(node.Name))
                    {
                        constr = new List<RowModel>();
                        constr.Add(new HeaderRowModel(node.InnerText));
                        lst.Add(constr);
                    }
                    if (node.Name == "p")
                    {
                        constr?.Add(new TextRowModel(node.InnerText));
                    }
                    if (IsIllustration(node))
                    {
                        constr?.Add(new ImageRowModel("http:" + node.ChildNodes[0].GetAttributeValue("href", null)));
                    }
                    if (IsSubtitle(node))
                    {
                        constr?.Add(new SubtitleRowModel(node.InnerText));
                    }
                }

                var model = new TextModel(lst);
                model.Chapter = chapter;
                mTextCace[chapterId] = model;
            }
            return mTextCace[chapterId];
        }
    }
}