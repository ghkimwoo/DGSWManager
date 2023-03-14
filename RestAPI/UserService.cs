using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGSWManager.RestAPI;
public interface IUserService
{
    Task<string> Execute(string bizNo);
}

public class UserService : IUserService
{
    public async Task<string> Execute(string bizNo)
    {
        using HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("type", "POST");
        httpClient.DefaultRequestHeaders.Add("b_no", bizNo);
        var response = await httpClient.GetAsync("https://api.odcloud.kr/api/nts-businessman/v1/status?serviceKey=" + await SecureStorage.Default.GetAsync("NtsStatusKey"));

        return await response.Content.ReadAsStringAsync();
    }
}


public class User
{
    public int bizNo { get; set; }
    public string taxType { get; set; }
}
