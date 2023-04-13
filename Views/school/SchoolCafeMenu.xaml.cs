using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text;

namespace DGSWManager.Views.school;

public partial class SchoolCafeMenu : ContentPage
{
    private string _schoolCafeMenu;
    public class Menu
    {
        public string MMEAL_SC_NM;
        public string DDISH_NM;
    }
    public class Content
    {
        public string ContentName { get; set; }
        public string ContentDescription { get; set; }
    }
    ObservableCollection<Content> contents = new ObservableCollection<Content>();
    public ObservableCollection<Content> Contents { get { return contents; } }
    private async void LoadCafeMenu()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<SchoolCafeMenu>()
            .Build();
        string Token = config["NeisApiKey"];
        string url = "https://open.neis.go.kr/hub/mealServiceDietInfo?KEY=" + Token + "&Type=json&ATPT_OFCDC_SC_CODE="+ "D10" + "&SD_SCHUL_CODE=" + "7240454" + "&MLSV_YMD=" + DateTime.Now.ToString("yyyyMMdd");


        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(url);
        var content = new StringContent("", Encoding.UTF8, "application/json");
        HttpResponseMessage response = httpClient.PostAsync(url, content).Result;


        string body = await response.Content.ReadAsStringAsync();
        JObject obj = JObject.Parse(body);
        _schoolCafeMenu = obj["mealServiceDietInfo"][1]["row"].ToString();
    }
    public SchoolCafeMenu()
    {
        InitializeComponent();
        LoadCafeMenu();
        var pObj = JsonConvert.DeserializeObject<List<Menu>>(_schoolCafeMenu);
        foreach (var item in pObj)
        {
            string replaceResult = item.DDISH_NM.Replace("<br/>", " ");
            contents.Add(new Content() { ContentName = item.MMEAL_SC_NM, ContentDescription = replaceResult });
        }
        collectionView.ItemsSource = contents;
    }
}