namespace Reading_Log;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(BookDetailsPage), typeof(BookDetailsPage));
	}
}
