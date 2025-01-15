using SQLite;
using System.Collections.Generic;
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
            _connection.CreateTablesAsync<HardwareResourcesData, CoreSoftDependencyData, TileSoftDependencyData>();
        }


        // given a group Id, return a dictionary that maps the table name a list of DBData objects from all tables
        public async Task<Dictionary<string, List<DbData>>> GetByGroupId(string groupId)
        {
            Dictionary<string, List<DbData>> batchData = new Dictionary<string, List<DbData>>{
                { "tileSoftDependencies", new List<DbData>() },
                { "coreSoftDependencies", new List<DbData>() },
                { "hardwareResources", new List<DbData>() }
            };
            List<TileSoftDependencyData> tileSoftDataList = await _connection.Table<TileSoftDependencyData>().Where(x => x.GroupId == groupId).ToListAsync();
            List<CoreSoftDependencyData> coreSoftDataList = await _connection.Table<CoreSoftDependencyData>().Where(x => x.GroupId == groupId).ToListAsync();
            List<HardwareResourcesData> hardwareDataList = await _connection.Table<HardwareResourcesData>().Where(x => x.GroupId == groupId).ToListAsync();

            foreach (TileSoftDependencyData entry in tileSoftDataList)
            {
                batchData["tileSoftDependencies"].Add(entry);
            }
            foreach (CoreSoftDependencyData entry in coreSoftDataList)
            {
                batchData["coreSoftDependencies"].Add(entry);
            }
            foreach (HardwareResourcesData entry in hardwareDataList)
            {
                batchData["hardwareResources"].Add(entry);
            }

            return batchData;
        }


        // Overloaded method to write to db----------------------------------------------------------------------------
        public async Task WriteToDb(TileSoftDependencyData softData)
        {
            await _connection.InsertAsync(softData);
        }
        public async Task WriteToDb(List<TileSoftDependencyData> softDataList)
        {
            Debug.WriteLine($"Wrote to Soft Depdency Table in DB at {Path.Combine(FileSystem.AppDataDirectory, DB_NAME)}");
            foreach(TileSoftDependencyData entry in softDataList)
            {
                await _connection.InsertAsync(entry);
            }
        }
        public async Task WriteToDb(CoreSoftDependencyData softData)
        {
            await _connection.InsertAsync(softData);
        }
        public async Task WriteToDb(List<CoreSoftDependencyData> softDataList)
        {
            Debug.WriteLine($"Wrote to Soft Depdency Table in DB at {Path.Combine(FileSystem.AppDataDirectory, DB_NAME)}");
            foreach (CoreSoftDependencyData entry in softDataList)
            {
                await _connection.InsertAsync(entry);
            }
        }
        public async Task WriteToDb(HardwareResourcesData datain)
        {
            await _connection.InsertAsync(datain);
        }
        public async Task WriteToDb(List<HardwareResourcesData> dataListIn)
        {
            Debug.WriteLine($"Wrote to Soft Depdency Table in DB at {Path.Combine(FileSystem.AppDataDirectory, DB_NAME)}");
            foreach (HardwareResourcesData entry in dataListIn)
            {
                await _connection.InsertAsync(entry);
            }
        }
        // ------------------------------------------------------------------------------------------------------------




        // Hardware Dependency Mehtods
        public async Task<List<HardwareResourcesData>> GetHardwareResourcesData()
        {
            return await _connection.Table<HardwareResourcesData>().ToListAsync();
        }

        public async Task<HardwareResourcesData> GetHardwareResourcesById(int id)
        {
            return await _connection.Table<HardwareResourcesData>().Where(x => x.Id == id).FirstOrDefaultAsync();
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
