using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Text;

namespace DGSWManager.Views.school;

public partial class SchoolTimePage : ContentPage
{
	private string _schoolTime;
    public class Menu
    {
        public string PERIO;
        public string ITRT_CNTNT;
    }
    public class Content
    {
        public string ContentName { get; set; }
        public string ContentDescription { get; set; }
    }
    ObservableCollection<Content> contents = new ObservableCollection<Content>();
    public ObservableCollection<Content> Contents { get { return contents; } }
    private async void LoadSchoolTime()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<SchoolCafeMenu>()
            .Build();
        string Token = config["NeisApiKey"];
        string url = "https://open.neis.go.kr/hub/hisTimetable?KEY=" + Token + "&Type=json&ATPT_OFCDC_SC_CODE=" + "D10" + "&SD_SCHUL_CODE=" + "7240454" + "&ALL_TI_YMD=" + DateTime.Now.ToString("yyyyMMdd") + "&GRADE=" + "3" + "&CLASS_NM=" + "1";


        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(url);
        var content = new StringContent("", Encoding.UTF8, "application/json");
        HttpResponseMessage response = httpClient.PostAsync(url, content).Result;


        string body = await response.Content.ReadAsStringAsync();
        JObject obj = JObject.Parse(body);
        _schoolTime = obj["hisTimetable"][1]["row"].ToString();
    }
    public SchoolTimePage()
	{
		InitializeComponent();
        LoadSchoolTime();
        var pObj = JsonConvert.DeserializeObject<List<Menu>>(_schoolTime);
        foreach (var item in pObj)
        {
            contents.Add(new Content() { ContentName = item.PERIO, ContentDescription = item.ITRT_CNTNT });
        }
        collectionView.ItemsSource = contents;
    }
}