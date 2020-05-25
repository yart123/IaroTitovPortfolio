using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models
{
    public class ProjectLinkEntity : TableEntity
    {
        public ProjectLinkEntity()
        {

        }

        [IgnoreProperty]

        public int ProjectId { get { return Int32.Parse(RowKey); } set { RowKey = value.ToString(); } }
        [IgnoreProperty]
        public string Title { get { return PartitionKey; } set { PartitionKey = value; } }
        public string Link { get; set; }
    }
}
