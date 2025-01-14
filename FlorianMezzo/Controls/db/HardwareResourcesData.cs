using SQLite;

namespace FlorianMezzo.Controls.db
{
    [Table("hardwareResources")]
    public class HardwareResourcesData
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("groupId")]
        public string GroupId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("feedback")]
        public string Feedback { get; set; }

        [Column("datetime")]
        public string DateTime { get; set; }

        public HardwareResourcesData() { }
        public HardwareResourcesData(string groupId, string title, int status, string feedback, string dateTime)
        {
            GroupId = groupId;
            Title = title;
            Status = status;
            Feedback = feedback;
            DateTime = dateTime;
        }
    }
}
