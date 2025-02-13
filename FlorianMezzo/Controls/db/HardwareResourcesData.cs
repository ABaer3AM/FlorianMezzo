using SQLite;

namespace FlorianMezzo.Controls.db
{
    [Table("hardwareResources")]
    public class HardwareResourcesData : DbData
    {
        public HardwareResourcesData()
            : base("", "", "", 0, "", "", false) { }
        public HardwareResourcesData(string groupId, string sessionId, string title, int status, string feedback, string dateTime, bool florianRunning)
            : base(groupId, sessionId, title, status, feedback, dateTime, true, florianRunning) {  }
        public HardwareResourcesData(string groupId, string sessionId, string title, int status, string feedback, string dateTime, bool averagable, bool florianRunning)
            : base(groupId, sessionId, title, status, feedback, dateTime, averagable, florianRunning) { }
    }
}
