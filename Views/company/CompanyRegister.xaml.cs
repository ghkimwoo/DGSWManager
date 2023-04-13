using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Specialized;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;
using DGSWManager.Data;
using DGSWManager.Models;
using System.Security.Cryptography.X509Certificates;

namespace DGSWManager.Views.company;



public partial class CompanyRegister : ContentPage
{
    public class BizJson
    {
		public string[] b_no { get; set; }
    }
    public CompanyRegister()
	{
		InitializeComponent();
	}

	private async void OkButton_Clicked(object sender, EventArgs e)
	{
		string bizNo = BizNoEntry.Text;
        var config = new ConfigurationBuilder()
            .AddUserSecrets<CompanyRegister>()
            .Build();
        string Token = config["NtsStatusKey"];
		string url = "https://api.odcloud.kr/api/nts-businessman/v1/status?serviceKey="+ Token;
        var bizJson = new BizJson
        {
            b_no = new string[] { bizNo }
        };

        string jsonString = JsonConvert.SerializeObject(bizJson);

        var httpClient = new HttpClient();
		httpClient.BaseAddress = new Uri(url);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        HttpResponseMessage response = httpClient.PostAsync(url, content).Result;


        string body = await response.Content.ReadAsStringAsync();
        JObject obj = JObject.Parse(body);
        string status = obj["data"][0]["tax_type"].ToString();


        bool answer = await DisplayAlert("조회 결과", status+"입니다."+"\n"+"저장하시겠습니까?", "저장", "취소");
        if (answer)
        {
            CorprationDatabase corprationDatabase = new CorprationDatabase();
            await corprationDatabase.SaveItemAsync(new CorprationInfoModel
            {
                CorprationId = Convert.ToInt32(BizNoEntry.Text),
                Taxtype = status,
                CorprationName = CorpNameEntry.Text,
            });
            await DisplayAlert("저장 완료", "저장되었습니다.", "확인");
        }
    }
}