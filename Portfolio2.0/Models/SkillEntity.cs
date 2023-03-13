using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System;

namespace Portfolio.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SkillEntity : TableEntity
    {
        public SkillEntity()
        {

        }

        [JsonProperty]
        public int Id { get { return Int32.Parse(RowKey); } set { RowKey = value.ToString(); } }
        [JsonProperty]
        public string SkillName { get; set; }
        [JsonProperty]
        public int SkillGroup { get; set; }
        [JsonProperty]
        public double Proficiency { get; set; }
    }
}