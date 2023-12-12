using Reading_Log.ViewModel;

namespace Reading_Log;

public partial class BookDetailsPage : ContentPage
{
	public BookDetailsPage(BookDetailsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}