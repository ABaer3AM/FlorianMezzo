using SQLite;

namespace FlorianMezzo.Controls.db
{
    [Table("hardwareResources")]
    public class HardwareResourcesData : DbData
    {
        public HardwareResourcesData()
            : base("", "", 0, "", "") { }
        public HardwareResourcesData(string groupId, string title, int status, string feedback, string dateTime)
            : base(groupId, title, status, feedback, dateTime) {  }
    }
}
