using SQLite;
using DGSWManager.Models;

namespace DGSWManager.Data
{
    public class OpendartData
    {
        SQLiteAsyncConnection Database;
        public OpendartData()
        {
        }
        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<CorpDartListModel>();
        }

        public async Task<List<CorpDartListModel>> GetItemsAsync()
        {
            await Init();
            return await Database.Table<CorpDartListModel>().ToListAsync();
        }

        public async Task<int> SaveItemAsync(CorpDartListModel item)
        {
            await Init();
            if (item.Corp_code != 0)
            {
                return await Database.UpdateAsync(item);
            }
            else
            {
                return await Database.InsertAsync(item);
            }
        }

        public async Task<int> DeleteItemAsync(CorpDartListModel item)
        {
            await Init();
            return await Database.DeleteAsync(item);
        }

    }
}