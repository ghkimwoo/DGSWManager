namespace DGSWManager.Views.company;

public partial class CompanyRegister : ContentPage
{
	public CompanyRegister()
	{
		InitializeComponent();
	}

    private void OkButton_Clicked(object sender, EventArgs e)
    {
		int bizNo = int.Parse(BizNoEntry.Text);

    }
}