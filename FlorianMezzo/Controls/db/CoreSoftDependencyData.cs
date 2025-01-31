using SQLite;

namespace FlorianMezzo.Controls.db
{
    [Table("coreSoftwareDependencies")]
    public class CoreSoftDependencyData : DbData
    {

        public CoreSoftDependencyData()
            : base("", "", "", 0, "", "") { }
        public CoreSoftDependencyData(string groupId, string sessionId, string title, int status, string feedback, string dateTime)
            : base(groupId, sessionId, title, status, feedback, dateTime, true) { }
        public CoreSoftDependencyData(string groupId, string sessionId, string title, int status, string feedback, string dateTime, bool averagable)
            : base(groupId, sessionId, title, status, feedback, dateTime, averagable) { }
    }
}
