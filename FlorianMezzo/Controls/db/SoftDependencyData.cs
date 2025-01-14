using SQLite;

namespace FlorianMezzo.Controls.db
{
    [Table("softwareDependencies")]
    public class SoftDependencyData
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

        [Column("isTile")]
        public bool IsTile { get; set; }

        [Column("datetime")]
        public string DateTime { get; set; }

        public SoftDependencyData() { }
        public SoftDependencyData(string groupId, string title, int status, string feedback, bool isTitle, string dateTime)
        {
            GroupId = groupId;
            Title = title;
            Status = status;
            Feedback = feedback;
            IsTile = isTitle;
            DateTime = dateTime;
        }
    }
}
