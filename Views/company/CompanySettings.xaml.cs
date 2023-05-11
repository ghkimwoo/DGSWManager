using System.Net;
using System.Xml;
using Ionic.Zip;
using DGSWManager.Models;
using DGSWManager.Data;
using Microsoft.Extensions.Configuration;

namespace DGSWManager.Views.company;

public partial class CompanySettings : ContentPage
{
	string txtSaveFolder = "./xml";
	string xmlfile = "./xml/CORPCODE.xml";

    public CompanySettings()
	{
		InitializeComponent();
	}
    private async void RenewalButton_Clicked(object sender, EventArgs e)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<CompanySettings>()
            .Build();
        string Token = config["OpenDartKey"];
        string ZipResult = callWebClientZipSave("https://opendart.fss.or.kr/api/corpCode.xml?crtfc_key=" + Token);

		ExtractZip(ZipResult);
		SaveXMLData(xmlfile);
    }

	protected string callWebClientZipSave(string targetURL)
	{
		string result = string.Empty;
		Byte[] bytes = null;
		try
		{
			WebClient client = new WebClient();

			//헤더값 추가
			client.Headers.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.3; WOW64; Trident/7.0)");

			using (Stream data = client.OpenRead(targetURL))
			{
				byte[] buffer = new byte[16 * 1024];
				using (MemoryStream ms = new MemoryStream())
				{
					int read;
					while((read = data.Read(buffer, 0, buffer.Length)) > 0)
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
                //파일 정보 정리
                FileStream file = new FileStream(txtSaveFolder + "\\" + zipFileName, FileMode.Create, FileAccess.Write);
				ms.WriteTo(file);

				file.Close();
				ms.Close();
			}

			result = txtSaveFolder + "\\" + zipFileName;
		} catch (Exception e) { 
			Console.WriteLine(e.ToString());
		}

		return result;
	}

	protected void ExtractZip(string zipPath)
	{
		using (ZipFile zip = new ZipFile(zipPath))
		{
			zip.ExtractAll(txtSaveFolder);
		}
	}

	protected async void SaveXMLData(string filePath)
	{
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.Load(xmlfile);
		XmlNodeList XmlList = xmlDoc.GetElementsByTagName("list");
		foreach(XmlNode list in XmlList)
		{
            OpendartData dartlist = new OpendartData();
			await dartlist.SaveItemAsync(new CorpDartListModel
			{
				Corp_code = Convert.ToInt32(list["corp_code"].InnerText),
				Corp_name = list["corp_name"].InnerText,
				Stock_code = Convert.ToInt32(list["stock_code"].InnerText),
				Modify_date = Convert.ToInt32(list["modify_date"].InnerText)
			});
        }
    }
}