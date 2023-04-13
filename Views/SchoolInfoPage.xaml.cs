namespace DGSWManager.Views;

public partial class SchoolInfoPage : ContentPage
{
	public SchoolInfoPage()
	{
		InitializeComponent();
	}

	private async void testButton_Clicked(Object sender, EventArgs e)
	{
        await Navigation.PushAsync(new school.SchoolCafeMenu());
    }
}

