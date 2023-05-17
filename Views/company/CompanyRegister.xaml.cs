using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using DGSWManager.Data;
using DGSWManager.Models;
using System.Xml;
using Ionic.Zip;
using System.Net;

namespace DGSWManager.Views.company;

[QueryProperty("Item", "Item")]
public partial class CompanyRegister : ContentPage
{
    private static string mainDir = FileSystem.Current.AppDataDirectory;
    private static string fileName = "CORPCODE.xml";
    private static string filePath = System.IO.Path.Combine(mainDir, fileName);
    public CorprationInfoModel Item
    {
        get => BindingContext as CorprationInfoModel;
        set => BindingContext = value;
    }
    CorprationDatabase database;
    public class BizJson
    {
		public string[] b_no { get; set; }
    }
    public CompanyRegister(CorprationDatabase corprationDatabase)
	{
		InitializeComponent();
        database = corprationDatabase;
	}

	private async void OkButton_Clicked(object sender, EventArgs e)
	{
        if (string.IsNullOrWhiteSpace(BizNoEntry.Text))
        {
            await DisplayAlert("����ڵ�Ϲ�ȣ �ʿ�", "����ڵ�Ϲ�ȣ�� �Է����ּ���.", "Ȯ��");
            return;
        }
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


        bool answer = await DisplayAlert("��ȸ ���", status+"�Դϴ�."+"\n"+"�����Ͻðڽ��ϱ�?", "����", "���");
        if (answer)
        {
            await database.SaveItemAsync(Item);
            await DisplayAlert("���� �Ϸ�", "����Ǿ����ϴ�.", "Ȯ��");
        }
        await Shell.Current.GoToAsync("..");
    }
    private void RenewalButton_Clicked(object sender, EventArgs e)
    {
        renewButton.IsEnabled = false;
        renewButton.Text = "�������Դϴ�. ��ٷ��ּ���...";
        var config = new ConfigurationBuilder()
            .AddUserSecrets<CompanyRegister>()
            .Build();
        string Token = config["OpenDartKey"];
        string ZipResult = callWebClientZipSave("https://opendart.fss.or.kr/api/corpCode.xml?crtfc_key=" + Token);

        ExtractZip(ZipResult);
        SaveXMLData(filePath);
    }

    protected string callWebClientZipSave(string targetURL)
    {
        string result = string.Empty;
        Byte[] bytes = null;
        try
        {
            WebClient client = new WebClient();

            //����� �߰�
            client.Headers.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.3; WOW64; Trident/7.0)");

            using (Stream data = client.OpenRead(targetURL))
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = data.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    bytes = ms.ToArray();
                }
                data.Close();
            }

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

    protected async void SaveXMLData(string filePath)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);
        XmlNodeList XmlList = xmlDoc.GetElementsByTagName("list");

        OpendartData dartlist = new OpendartData();
        int i = 0;
        foreach (XmlNode list in XmlList)
        {
            i++;
            await dartlist.SaveItemAsync(new CorpDartListModel
            {
                Corp_code = Convert.ToInt32(list["corp_code"].InnerText),
                Corp_name = Convert.ToString(list["corp_name"].InnerText),
                Modify_date = Convert.ToInt32(list["modify_date"].InnerText)
            });


            renewButton.Text = "�������Դϴ�. " + Math.Round(Convert.ToDouble(i * 100 / XmlList.Count), 2) + "% �Ϸ�Ǿ����ϴ�.";
        }
        await DisplayAlert("���� �Ϸ�", "���ŵǾ����ϴ�.", "Ȯ��");
        renewButton.Text = "������ �Ϸ�Ǿ����ϴ�.";

    }
}