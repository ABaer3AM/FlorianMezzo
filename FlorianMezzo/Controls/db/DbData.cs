using SQLite;

namespace FlorianMezzo.Controls.db
{
    public class DbData
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("groupId")]
        public string GroupId { get; set; }

        [Column("sessionId")]
        public string SessionId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("status")]
        public int Status { get; set; }

        [Column("feedback")]
        public string Feedback { get; set; }

        [Column("datetime")]
        public string DateTime { get; set; }

        [Column("averagable")]
        public bool Averageable { get; set; }

        [Column("florianRunning")]
        public bool FlorianRunning { get; set; }

        public DbData(string groupId, string sessionId, string title, int status, string feedback, string dateTime, bool florianRunning)
        {
            GroupId = groupId;
            SessionId = sessionId;
            Title = title;
            Status = status;
            Feedback = feedback;
            DateTime = dateTime;
            Averageable = true;
            FlorianRunning = florianRunning;
        }
        public DbData(string groupId, string sessionId, string title, int status, string feedback, string dateTime, bool averageable, bool florianRunning)
        {
            GroupId = groupId;
            SessionId = sessionId;
            Title = title;
            Status = status;
            Feedback = feedback;
            DateTime = dateTime;
            Averageable = averageable;
            FlorianRunning = florianRunning;
        }
        public override string ToString()
        {
            return $"{GroupId}, {SessionId}, {Title}, {Status}, {Feedback}, {DateTime}, {Averageable}, {FlorianRunning}";
        }
    }
}
