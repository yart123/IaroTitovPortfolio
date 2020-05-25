using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Portfolio.Models
{
    public class SkillEntity : TableEntity
    {
        public SkillEntity()
        {

        }

        [IgnoreProperty]
        public int Id { get { return Int32.Parse(RowKey); } set { RowKey = value.ToString(); } }
        public string SkillName { get; set; }
        public int SkillGroup { get; set; }
        public double Proficiency { get; set; }
    }
}