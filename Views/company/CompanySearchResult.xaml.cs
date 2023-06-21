// File: C:\Users\w1ipg\source\repos\DGSWManager\Views\company\CompanySearchResult.xaml.cs
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace DGSWManager.Views.company;

public partial class CompanySearchResult : ContentPage
{
    private string _companyName;
    private string _companyNumber;
    private static string mainDir = FileSystem.Current.AppDataDirectory;
    private static string fileName = "CORPCODE.xml";
    private static string filePath = System.IO.Path.Combine(mainDir, fileName);
    public CompanySearchResult(string bizNo, string bizName)
    {
        InitializeComponent();
        _companyName = bizNo;
        CompanyNumber.Text = bizNo;

        _companyNumber = bizName;
        CompanyName.Text = bizName;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        SearchButton.IsEnabled = false;
        SearchButton.Text = "�˻����Դϴ�. ��ø� ��ٷ��ּ���.";
        var config = new ConfigurationBuilder()
            .AddUserSecrets<CompanySearchResult>()
            .Build();
        string Token = config["OpenDartKey"];
        string corp_code = "";
        corp_code = FindCorpCodeByCorpName(_companyName);
        if (corp_code == "�������� �ʽ��ϴ�.")
        {
            await DisplayAlert("����", "������ȣ�� �������� �ʽ��ϴ�.\n������ȣ�� ���� �� �ٽ� �õ����ּ���", "Ȯ��");
            Device.BeginInvokeOnMainThread(async () => await Navigation.PopAsync());
            return;
        }

        string bgn_De = DateTime.Now.AddDays(-90).ToString("yyyyMMdd");
        try
        {
            string url = $"https://opendart.fss.or.kr/api/list.json?crtfc_key={Token}&corp_code={corp_code}" +
            $"&bgn_de={bgn_De}&last_report_at=Y&pblntf_ty=A&sort_mth=asc";
            HttpClient client = new HttpClient();
            //����� �߰�
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.3; WOW64; Trident/7.0)");
            HttpResponseMessage response = await client.GetAsync(url);
            HttpContent content = response.Content;
            var body = await content.ReadAsStringAsync();
            JObject obj = JObject.Parse(body);

            string report_Name = obj["list"][0]["report_nm"].ToString();
            string reprt_code = "";
            string reprt_year = report_Name.Substring(7, 4);

            switch (report_Name.Substring(0, 2))
            {
                case "���":
                    reprt_code = "11011";
                    break;
                case "�ݱ�":
                    reprt_code = "11012";
                    break;
                case "�б�":
                    if (report_Name.Substring(12, 2) == "03")
                    {
                        reprt_code = "11013";
                    }
                    else
                    {
                        reprt_code = "11014";
                    }
                    break;
            }
            double netIncome = 0;
            double operatingProfit = 0;
            string stability = "";

            url = $"https://opendart.fss.or.kr/api/fnlttSinglAcnt.json?crtfc_key={Token}" +
                    $"&corp_code={corp_code}&bsns_year={reprt_year}&reprt_code={reprt_code}&fs_div=OFS";

            HttpResponseMessage response2 = await client.GetAsync(url);
            string body2 = await response2.Content.ReadAsStringAsync();
            JObject obj2 = JObject.Parse(body2);
            foreach (JObject item in obj2["list"])
            {
                if(item["sj_nm"].ToString() == "���Ͱ�꼭" && item["account_nm"].ToString() == "��������")
                {
                    netIncome = item["thstrm_amount"].Value<double>();
                    netIncomeEntry.Text = netIncome.ToString();
                    
                }
                if (item["sj_nm"].ToString() == "���Ͱ�꼭" && item["account_nm"].ToString() == "��������")
                {
                    operatingProfit = item["thstrm_amount"].Value<double>();
                    operatingProfitEntry.Text = operatingProfit.ToString();
                    break;
                }
            }

            if (operatingProfit > 0 && netIncome > 0)
            {
                stability = "�������Դϴ�.";
            }
            else if (operatingProfit < 0 && netIncome < 0)
            {
                stability = "�Ҿ����մϴ�.";
            }
            else
            {
                stability = "ȸ�� �������� �Һи��մϴ�.";
            }
            netIncomeEntry.Text = netIncome.ToString();
            operatingProfitEntry.Text = operatingProfit.ToString();
            stabilityEntry.Text = stability;
        }
        catch (Exception)
        {
            SearchButton.Text = "Error";
            netIncomeEntry.IsReadOnly = false;
            operatingProfitEntry.IsReadOnly = false;
            await DisplayAlert("����", "���� ������ ã�� �� �����ϴ�.\n���� �������� ��ȯ�մϴ�.", "Ȯ��");
            await Launcher.OpenAsync("https://sminfo.mss.go.kr/cm/sv/CSV001R0.do");
        }
        

        

    }
    public string FindCorpCodeByCorpName(string corpName)
    {
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(filePath);
        Debug.WriteLine(filePath);
        var targetNode = xmlDoc.SelectSingleNode($"/result/list[corp_name='�Ｚ����']");
        if (targetNode != null)
        {
            return targetNode.SelectSingleNode("corp_code").InnerText;
        }
        return "�������� �ʽ��ϴ�.";
    }
}
