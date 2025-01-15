using SQLite;

namespace FlorianMezzo.Controls.db
{
    [Table("coreSoftwareDependencies")]
    public class CoreSoftDependencyData : DbData
    {

        public CoreSoftDependencyData()
            : base("", "", 0, "", "") { }
        public CoreSoftDependencyData(string groupId, string title, int status, string feedback, string dateTime)
            : base(groupId, title, status, feedback, dateTime) { }
    }
}
