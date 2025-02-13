using SQLite;

namespace FlorianMezzo.Controls.db
{
    [Table("tileSoftwareDependencies")]
    public class TileSoftDependencyData : DbData
    {

        public TileSoftDependencyData() : base("", "", "", 0, "", "", false) { }
        public TileSoftDependencyData(string groupId, string sessionId, string title, int status, string feedback, string dateTime, bool florianRunning)
            : base(groupId, sessionId, title, status, feedback, dateTime, true, florianRunning) { }
        public TileSoftDependencyData(string groupId, string sessionId, string title, int status, string feedback, string dateTime, bool averagable, bool florianRunning)
            : base(groupId, sessionId, title, status, feedback, dateTime, averagable, florianRunning) { }
    }
}
