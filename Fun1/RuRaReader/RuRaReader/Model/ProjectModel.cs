using Java.Lang;

namespace RuRaReader.Model
{
    public class ProjectModel : Object
    {
        public string Title { get; set; }
        public string Status { get; set; }
        public int Id { get; set; }

        public ProjectModel(dynamic des)
        {
            Title = des.title;
            Id = (int)des.projectId;
        }
    }
}