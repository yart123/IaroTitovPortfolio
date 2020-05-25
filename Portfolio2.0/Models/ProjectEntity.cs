using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models
{
    public class ProjectEntity : TableEntity
    {
        public ProjectEntity()
        {

        }

        [IgnoreProperty]
        public int Id { get { return Int32.Parse(RowKey); } set { RowKey = value.ToString(); } }

        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string ImageFolder { get; set; }
        public DateTime ReleaseDate { get; set; }

        public List<ProjectVideoEntity> videos = new List<ProjectVideoEntity>();
        public List<ProjectLinkEntity> links = new List<ProjectLinkEntity>();
        public List<SkillEntity> skills = new List<SkillEntity>();
        public List<string> images = new List<string>();
    }
}
