using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;

namespace Reading_Log.ViewModel;

[QueryProperty("BookName", "BookName")]
[QueryProperty("Author","Author")]
[QueryProperty("Summary", "Summary")]
[QueryProperty("BookStatus", "BookStatus")]

public partial class BookDetailsViewModel : ObservableObject
{
    private bool isSummaryInitialized = false;

    [ObservableProperty]
    string bookName;

    [ObservableProperty]
    string author;

    [ObservableProperty]
    string summary;

    [ObservableProperty]
    string bookStatus;

    [ObservableProperty]
    string newSummary;

    [RelayCommand]
    async Task GoBack()
    {
        await Shell.Current.GoToAsync("..");
    }



    protected override void OnPropertyChanged(PropertyChangedEventArgs args)
    {
        base.OnPropertyChanged(args);

        if (args.PropertyName == nameof(Summary) && !isSummaryInitialized)
        {
            NewSummary = Summary;
            isSummaryInitialized = true; // Ensure this only happens once
        }
    }
}

