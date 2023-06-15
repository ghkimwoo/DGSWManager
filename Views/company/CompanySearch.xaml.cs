using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using DGSWManager.Models;
using System.Xml;
using Ionic.Zip;
using System.Net;

namespace DGSWManager.Views.company;

public partial class CompanySearch : ContentPage
{
    private static string mainDir = FileSystem.Current.AppDataDirectory;
    private static string fileName = "CORPCODE.xml";
    private static string filePath = System.IO.Path.Combine(mainDir, fileName);
    static readonly HttpClient httpClient = new HttpClient();

    public class BizJson
    {
		public string[] b_no { get; set; }
    }
    public CompanySearch()
	{
		InitializeComponent();
	}

    private async void OkButton_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(BizNoEntry.Text))
        {
            await DisplayAlert("����ڵ�Ϲ�ȣ �ʿ�", "����ڵ�Ϲ�ȣ�� �Է����ּ���.", "Ȯ��");
            return;
        }
        string bizNo = BizNoEntry.Text;
        string bizName = CorpNameEntry.Text;
        var config = new ConfigurationBuilder()
            .AddUserSecrets<CompanySearch>()
            .Build();
        string Token = config["NtsStatusKey"];
        string url = "https://api.odcloud.kr/api/nts-businessman/v1/status?serviceKey=" + Token;
        var bizJson = new BizJson
        {
            b_no = new string[] { bizNo }
        };

        string jsonString = JsonConvert.SerializeObject(bizJson);
        if (httpClient.BaseAddress == null)
        {
            httpClient.BaseAddress = new Uri(url);
        }
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        HttpResponseMessage response = httpClient.PostAsync(url, content).Result;


        string body = await response.Content.ReadAsStringAsync();
        JObject obj = JObject.Parse(body);
        string status = obj["data"][0]["tax_type"].ToString();

        

        bool answer = await DisplayAlert("��ȸ ���", "���� : " + status + "\n" + "���� ��ȸ�� �Ͻðڽ��ϱ�?", "����", "���");
        if (answer)
        {
            // New Code
            await Navigation.PushAsync(new CompanySearchResult(bizNo, bizName));
        }
    }
    private async void RenewalButton_Clicked(object sender, EventArgs e)
    {
        renewButton.IsEnabled = false;
        renewButton.Text = "�������Դϴ�. ��ٷ��ּ���...";
        var config = new ConfigurationBuilder()
            .AddUserSecrets<CompanySearch>()
            .Build();
        string Token = config["OpenDartKey"];
        string ZipResult = await callWebClientZipSave("https://opendart.fss.or.kr/api/corpCode.xml?crtfc_key=" + Token);
        ExtractZip(ZipResult);
        renewButton.Text = "������ �Ϸ�Ǿ����ϴ�.";
    }

    protected async Task<string> callWebClientZipSave(string targetURL)
    {
        string result = string.Empty;
        Byte[] bytes = null;
        try
        {
            HttpClient client = new HttpClient();

            //����� �߰�
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.3; WOW64; Trident/7.0)");

            using (HttpResponseMessage response = await client.GetAsync(targetURL))
            {
                using (HttpContent content = response.Content)
                {
                    bytes = await content.ReadAsByteArrayAsync();
                    string zipFileName = "corpCode_" + DateTime.Now.ToShortDateString() + ".zip";

                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        //���� ���� ����
                        FileStream file = new FileStream(mainDir + "\\" + zipFileName, FileMode.Create, FileAccess.Write);
                        ms.WriteTo(file);

                        file.Close();
                        ms.Close();
                    }

                    result = mainDir + "\\" + zipFileName;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        return result;
    }

    protected void ExtractZip(string zipPath)
    {
        using (ZipFile zip = new ZipFile(zipPath))
        {
            try
            {
                zip.ExtractAll(mainDir);
            }
            catch (ZipException)
            {
                File.Delete(filePath);
                zip.ExtractAll(mainDir);
            }
        }
    }

    
}