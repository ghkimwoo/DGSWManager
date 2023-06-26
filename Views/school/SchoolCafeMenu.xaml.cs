using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Text;

namespace DGSWManager.Views.school;

public partial class SchoolCafeMenu : ContentPage
{
    private static string mainDir = FileSystem.Current.AppDataDirectory;
    private static string fileName = "SchoolInfo.json";
    private static string filePath = System.IO.Path.Combine(mainDir, fileName);
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
    private async Task LoadCafeMenu()
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
            var config = new ConfigurationBuilder()
            .AddUserSecrets<SchoolCafeMenu>()
            .Build();

            string Token = config["NeisApiKey"];
            string url = $"https://open.neis.go.kr/hub/mealServiceDietInfo?KEY={Token}&Type=json&ATPT_OFCDC_SC_CODE={eduOfficeCode}" + 
                $"&SD_SCHUL_CODE={schoolCode}&MLSV_YMD={DateTime.Today.ToString("yyyyMMdd")}";


            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(url);
            var content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage response = httpClient.PostAsync(url, content).Result;


            string body = await response.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(body);
            _schoolCafeMenu = obj["mealServiceDietInfo"][1]["row"].ToString();
        }
        catch (NullReferenceException)
        {
            await DisplayAlert("오류", "작성중인 식단 또는 [식단공개확정] 처리가 되지 않은 식단은 \n조회되지 않을 수 있습니다.\n며칠 후 다시 시도해주세요.", "확인");
        }
        catch (Exception)
        {
            await DisplayAlert("오류", "사용자 학교 정보가 저장되지 않았습니다.\n새로 설정해주세요.", "확인");
        }
    }
    private async void Button_Selected(object sender, EventArgs e)
    {
        try
        {
            SelectButton.IsEnabled = false;
            SelectButton.Text = "네트워크 통신중입니다. 잠시만 기다려주세요.";
            await LoadCafeMenu();
            var pObj = JsonConvert.DeserializeObject<List<Menu>>(_schoolCafeMenu);
            contents.Clear();
            foreach (var item in pObj)
            {
                string replaceResult = item.DDISH_NM.Replace("<br/>", "\n");
                contents.Add(new Content() { ContentName = item.MMEAL_SC_NM, ContentDescription = replaceResult });
            }
            collectionView.ItemsSource = contents;
            SelectButton.IsEnabled = true;
            SelectButton.Text = "정상적으로 로딩이 완료되었습니다.";
        }
        catch (Exception)
        {
            Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
        }
        
    }
    public SchoolCafeMenu()
    {
        InitializeComponent();
    }
}