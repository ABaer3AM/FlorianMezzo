using SQLite;
using System.Diagnostics;

namespace FlorianMezzo.Controls.db
{
    public class LocalDbService
    {
        private const string DB_NAME = "mezzo_localdb.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTablesAsync<HardwareResourcesData, SoftDependencyData>();
        }

        // Software Dependency Methods
        public async Task<List<SoftDependencyData>> GetSoftwareDependencyData()
        {
            return await _connection.Table<SoftDependencyData>().ToListAsync();
        }

        public async Task<SoftDependencyData> GetSoftwareDependencyById(int id)
        {
            return await _connection.Table<SoftDependencyData>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<SoftDependencyData> GetSoftwareDependencyByGroupId(string groupId)
        {
            return await _connection.Table<SoftDependencyData>().Where(x => x.GroupId == groupId).FirstOrDefaultAsync();
        }

        public async Task WriteToSoftwareDependency(SoftDependencyData softData)
        {
            await _connection.InsertAsync(softData);
        }
        public async Task WriteToSoftwareDependency(List<SoftDependencyData> softDataList)
        {
            Debug.WriteLine($"Wrote to Soft Depdency Table in DB at {Path.Combine(FileSystem.AppDataDirectory, DB_NAME)}");
            foreach(SoftDependencyData entry in softDataList)
            {
                await _connection.InsertAsync(entry);
            }
        }

        public async Task UpdateSomeSoftwareDependency(SoftDependencyData softData)
        {
            await _connection.UpdateAsync(softData);
        }

        public async Task RemoveASoftwareDependency(SoftDependencyData softData)
        {
            await _connection.DeleteAsync(softData);
        }



        // Hardware Dependency Mehtods
        public async Task<List<HardwareResourcesData>> GetHardwareResourcesData()
        {
            return await _connection.Table<HardwareResourcesData>().ToListAsync();
        }

        public async Task<HardwareResourcesData> GetHardwareResourcesById(int id)
        {
            return await _connection.Table<HardwareResourcesData>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<HardwareResourcesData> GetHardwareResourcesByGroupId(string groupId)
        {
            return await _connection.Table<HardwareResourcesData>().Where(x => x.GroupId == groupId).FirstOrDefaultAsync();
        }

        public async Task WriteToHardwareResources(HardwareResourcesData hardData)
        {
            await _connection.InsertAsync(hardData);
        }
        public async Task WriteToHardwareResources(List<HardwareResourcesData> hardDataList)
        {
            Debug.WriteLine($"Wrote to Hardware Resources Table in DB at {Path.Combine(FileSystem.AppDataDirectory, DB_NAME)}");
            foreach (HardwareResourcesData entry in hardDataList)
            {
                await _connection.InsertAsync(entry);
            }
        }

        public async Task UpdateSomeHardwareResources(HardwareResourcesData hardData)
        {
            await _connection.UpdateAsync(hardData);
        }

        public async Task RemoveAHardwareResources(HardwareResourcesData hardData)
        {
            await _connection.DeleteAsync(hardData);
        }
    }
}
