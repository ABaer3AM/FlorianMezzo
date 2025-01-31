using SQLite;

namespace FlorianMezzo.Controls.db
{
    [Table("tileSoftwareDependencies")]
    public class TileSoftDependencyData : DbData
    {

        public TileSoftDependencyData() : base("", "", "", 0, "", "") { }
        public TileSoftDependencyData(string groupId, string sessionId, string title, int status, string feedback, string dateTime)
            : base(groupId, sessionId, title, status, feedback, dateTime, true) { }
        public TileSoftDependencyData(string groupId, string sessionId, string title, int status, string feedback, string dateTime, bool averagable)
            : base(groupId, sessionId, title, status, feedback, dateTime, averagable) { }
    }
}
