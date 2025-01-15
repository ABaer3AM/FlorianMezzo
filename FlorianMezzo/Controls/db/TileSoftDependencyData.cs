using SQLite;

namespace FlorianMezzo.Controls.db
{
    [Table("tileSoftwareDependencies")]
    public class TileSoftDependencyData : DbData
    {

        public TileSoftDependencyData()
            : base("", "", 0, "", "") { }
        public TileSoftDependencyData(string groupId, string title, int status, string feedback, string dateTime)
            : base(groupId, title, status, feedback, dateTime) { }
    }
}
