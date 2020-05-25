using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models
{
    public class ProjectSkillEntity : TableEntity
    {
        public ProjectSkillEntity()
        {

        }

        [IgnoreProperty]
        public int ProjectId { get { return Int32.Parse(PartitionKey); } set { PartitionKey = value.ToString(); } }
        [IgnoreProperty]
        public int SkillId { get { return Int32.Parse(RowKey); } set { RowKey = value.ToString(); } }
    }
}
