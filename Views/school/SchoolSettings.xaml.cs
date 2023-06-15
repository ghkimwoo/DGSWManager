using DGSWManager.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace DGSWManager.Views.school;

public partial class SchoolSettings : ContentPage
{
    private static string mainDir = FileSystem.Current.AppDataDirectory;
    private static string fileName = "SchoolInfo.json";
    private static string filePath = System.IO.Path.Combine(mainDir, fileName);
    private string _schoolInfo;

    public SchoolSettings()
	{
		InitializeComponent();
	}
    public class Error
    {
        public string MESSAGE { get; set; }
    }
    public class Info
    {
        public string SD_SCHUL_CODE;
    }

	private async void SaveButton_Clicked(object sender, EventArgs e)
	{
        if(!File.Exists(filePath))
		{
			File.Create(filePath);
		}
		await InputJsonAsync();
	}
    private async Task InputJsonAsync()
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<SchoolSettings>()
            .Build();
        string eduOfficeCode = ((ComboBoxModel)EduOfficePicker.SelectedItem).EduOfficeCode;
        string Token = config["NeisApiKey"];
        string url = "https://open.neis.go.kr/hub/schoolInfo?KEY=" + Token + "&Type=json&ATPT_OFCDC_SC_CODE=" + eduOfficeCode + "&SCHUL_NM=" + SchoolName.Text;


        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(url);
        var content = new StringContent("", Encoding.UTF8, "application/json");
        HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
        string body = await response.Content.ReadAsStringAsync();
        
        try
        {
            JObject obj = JObject.Parse(body);

            _schoolInfo = obj["schoolInfo"][1]["row"].ToString();
            var pInfo = JsonConvert.DeserializeObject<List<Info>>(_schoolInfo);
            string SchoolCode = "";
            foreach (var info in pInfo)
            {
                SchoolCode = info.SD_SCHUL_CODE;
            }
            JObject jObject = new JObject(
                new JProperty("EduOfficeCode", ((ComboBoxModel)EduOfficePicker.SelectedItem).EduOfficeCode),
                new JProperty("SchoolName", SchoolName.Text),
                new JProperty("SchoolCode", SchoolCode),
                new JProperty("SchoolGrade", Convert.ToInt32(SchoolGrade.Text)),
                new JProperty("SchoolClass", Convert.ToInt32(SchoolClass.Text)));
            TextWriter tw = new StreamWriter(filePath);
            tw.WriteLine(jObject.ToString());
            tw.Close();
            await DisplayAlert("완료", "설정이 완료되었습니다.", "확인");
            Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
        }
        catch (JsonException)
        {
            await DisplayAlert("오류 발생", "학교 정보가 존재하지 않습니다.\n학교 이름이나 소속 교육청을 확인 후 다시 시도해주세요.", "확인");
            return;
        }
        
    }

}