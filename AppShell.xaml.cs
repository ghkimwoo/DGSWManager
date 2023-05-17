using DGSWManager.Views.company;

namespace DGSWManager;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        Routing.RegisterRoute(nameof(CompanyRegister), typeof(CompanyRegister));
    }
}
