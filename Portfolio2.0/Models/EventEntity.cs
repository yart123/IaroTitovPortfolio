using Microsoft.Azure.Cosmos.Table;
using System;

namespace Portfolio.Models
{
    public class EventEntity : TableEntity
    {
        public EventEntity()
        {

        }

        public int Id { get { return Int32.Parse(RowKey); } set { RowKey = value.ToString(); } }

        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        [IgnoreProperty]
        public bool Right { get; set; }
    }
}
