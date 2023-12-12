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

    [RelayCommand]
    void SaveChanges()
    {
        if (NewSummary != Summary)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(documentsPath, "Books.csv");
            using (StreamReader reader = new(filePath))
            {
                string book = reader.ReadLine();
                System.IO.File.Create(Path.Combine(documentsPath, "NewBooks.csv")).Close();
                using (StreamWriter writer = new(Path.Combine(documentsPath, "NewBooks.csv")))
                {
                    while (!string.IsNullOrEmpty(book))
                    {
                        string[] savedBook = book.Split(",");
                        string savedBookName = savedBook[0];
                        string savedBookAuthor = savedBook[1];
                        if (!BookName.Equals(savedBookName) || !Author.Equals(savedBookAuthor))
                        {
                            writer.WriteLine(book);
                        } else
                        {
                            writer.WriteLine(BookName + "," + Author + "," + NewSummary + "," + BookStatus);
                        }
                        book = reader.ReadLine();
                    }
                    writer.Close();
                }
                reader.Close();
                System.IO.File.Delete(Path.Combine(documentsPath, "Books.csv"));
                System.IO.File.Move(Path.Combine(documentsPath, "NewBooks.csv"), Path.Combine(documentsPath, "Books.csv"));
            }
        }
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

