using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Text;

namespace DGSWManager.Views.school;

public partial class SchoolTimePage : ContentPage
{
    private static string mainDir = FileSystem.Current.AppDataDirectory;
    private static string fileName = "SchoolInfo.json";
    private static string filePath = System.IO.Path.Combine(mainDir, fileName);
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
        try
        {
            // Read the JSON file into a string
            var jsonString = File.ReadAllText(filePath);

            // Deserialize the string into a JSON object
            var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonString);

            // Store the values in variables
            string eduOfficeCode = jsonObject.EduOfficeCode;
            string schoolCode = jsonObject.SchoolCode;
            string schoolName = jsonObject.SchoolName;
            int schoolGrade = jsonObject.SchoolGrade;
            int schoolClass = jsonObject.SchoolClass;

            var config = new ConfigurationBuilder()
                .AddUserSecrets<SchoolCafeMenu>()
                .Build();
            string Token = config["NeisApiKey"];
            string url = "https://open.neis.go.kr/hub/hisTimetable?KEY=" + Token + "&Type=json&ATPT_OFCDC_SC_CODE=" + eduOfficeCode +
                "&SD_SCHUL_CODE=" + schoolCode + "&ALL_TI_YMD=" + DateTime.Now.ToString("yyyyMMdd") + "&GRADE=" + schoolGrade + "&CLASS_NM=" + schoolClass;

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync(url, content).Result;


            string body = await response.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(body);
            _schoolTime = obj["hisTimetable"][1]["row"].ToString();
        }
        catch (JsonException)
        {
            await DisplayAlert("오류", "사용자 학교 정보가 저장되지 않았습니다.\n새로 설정해주세요.", "확인");
        }
        
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