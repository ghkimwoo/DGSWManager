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
            await DisplayAlert("����", "�ۼ����� �Ĵ� �Ǵ� [�Ĵܰ���Ȯ��] ó���� ���� ���� �Ĵ��� \n��ȸ���� ���� �� �ֽ��ϴ�.\n��ĥ �� �ٽ� �õ����ּ���.", "Ȯ��");
        }
        catch (Exception)
        {
            await DisplayAlert("����", "����� �б� ������ ������� �ʾҽ��ϴ�.\n���� �������ּ���.", "Ȯ��");
        }
    }
    private async void Button_Selected(object sender, EventArgs e)
    {
        try
        {
            SelectButton.IsEnabled = false;
            SelectButton.Text = "��Ʈ��ũ ������Դϴ�. ��ø� ��ٷ��ּ���.";
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
            SelectButton.Text = "���������� �ε��� �Ϸ�Ǿ����ϴ�.";
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