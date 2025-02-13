using SQLite;

namespace FlorianMezzo.Controls.db
{
    [Table("coreSoftwareDependencies")]
    public class CoreSoftDependencyData : DbData
    {

        public CoreSoftDependencyData()
            : base("", "", "", 0, "", "", false) { }
        public CoreSoftDependencyData(string groupId, string sessionId, string title, int status, string feedback, string dateTime, bool florianRunning)
            : base(groupId, sessionId, title, status, feedback, dateTime, true, florianRunning) { }
        public CoreSoftDependencyData(string groupId, string sessionId, string title, int status, string feedback, string dateTime, bool averagable, bool florianRunning)
            : base(groupId, sessionId, title, status, feedback, dateTime, averagable, florianRunning) { }
    }
}
