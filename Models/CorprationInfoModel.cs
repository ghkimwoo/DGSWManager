using SQLite;

namespace DGSWManager.Models;
public class CorprationInfoModel
{
    [PrimaryKey]
    public int CorprationId { get; set; }
    public string Taxtype { get; set; }
    public string CorprationName { get; set; }
    public long CorpRevenue { get; set; }
    public long OperatingIncome { get; set; }
    public long NetIncome { get; set; }
}
