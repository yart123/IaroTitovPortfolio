using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Portfolio.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SkillGroupEntity : TableEntity
    {
        public SkillGroupEntity()
        {

        }

        [JsonProperty]
        public string SkillGroupName { get { return RowKey; } set { RowKey = value; } }
        [JsonProperty]
        public int Id { get; set; }

        [JsonProperty]
        public List<SkillEntity> skills = new List<SkillEntity>();
    }
}
