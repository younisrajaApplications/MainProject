using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Reading_Log.ViewModel;

[QueryProperty("BookName", "BookName")]
[QueryProperty("Author","Author")]
[QueryProperty("Summary", "Summary")]
[QueryProperty("BookStatus", "BookStatus")]

public partial class BookDetailsViewModel : ObservableObject
{
    [ObservableProperty]
    string bookName;

    [ObservableProperty]
    string author;

    [ObservableProperty]
    string summary;

    [ObservableProperty]
    string bookStatus;

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}

