using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models
{
    public class SkillGroupEntity : TableEntity
    {
        public SkillGroupEntity()
        {

        }

        [IgnoreProperty]
        public string SkillGroupName { get { return RowKey; } set { RowKey = value; } }

        public int Id { get; set; }

        public List<SkillEntity> skills = new List<SkillEntity>();
    }
}
