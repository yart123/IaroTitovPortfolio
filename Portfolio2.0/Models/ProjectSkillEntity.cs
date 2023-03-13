using Microsoft.Azure.Cosmos.Table;
using System;

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
