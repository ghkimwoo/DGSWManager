namespace DGSWManager.Views.school;

public partial class SchoolInfoPage : ContentPage
{
	public SchoolInfoPage()
	{
		InitializeComponent();
	}

	private async void CafeMenuButton_Clicked(Object sender, EventArgs e)
	{
        await Navigation.PushAsync(new school.SchoolCafeMenu());
    }
	private async void TimeButton_Clicked(Object sender, EventArgs e)
	{
		await Navigation.PushAsync(new school.SchoolTimePage());
	}

	private async void SettingButton_Clicked(Object sender, EventArgs e)
	{
		await Navigation.PushAsync(new school.SchoolSettings());
	}
}

