using SQLite;
using DGSWManager.Models;

namespace DGSWManager.Data;

public class CorprationDatabase
{
    SQLiteAsyncConnection Database;
    public CorprationDatabase()
    {
    }
    async Task Init()
    {
        if (Database is not null)
            return;

        Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        var result = await Database.CreateTableAsync<CorprationInfoModel>();
    }


    public async Task<List<CorprationInfoModel>> GetItemsAsync()
    {
        await Init();
        return await Database.QueryAsync<CorprationInfoModel>("SELECT * FROM [CorprationInfoModel]");
    }
    public async Task<CorprationInfoModel> GetItemAsync(int id)
    {
        await Init();
        return await Database.Table<CorprationInfoModel>().Where(i => i.CorprationId == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveItemAsync(CorprationInfoModel item)
    {
        await Init();
        if (item.CorprationId != 0)
        {
            return await Database.UpdateAsync(item);
        }
        else
        {
            return await Database.InsertAsync(item);
        }
    }

    public async Task<int> DeleteItemAsync(CorprationInfoModel item)
    {
        await Init();
        return await Database.DeleteAsync(item);
    }

}
