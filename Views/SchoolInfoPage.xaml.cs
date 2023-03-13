namespace DGSWManager.Views;

public partial class SchoolInfoPage : ContentPage
{
	public SchoolInfoPage()
	{
		InitializeComponent();
	}

	private async void testButton_Clicked(Object sender, EventArgs e)
	{

		string oauthToken = await SecureStorage.Default.GetAsync("NeisApiKey");

		if( oauthToken != null )
		{
			await DisplayAlert("INFOMATION", oauthToken, "확인");
		}
    }
}

