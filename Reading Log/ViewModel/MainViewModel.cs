using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Reading_Log.ViewModel;
public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        Books = new ObservableCollection<string>();
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(documentsPath, "Books.csv");
        if (System.IO.File.Exists(filePath))
        {
            using StreamReader reader = new(filePath);
            string line = reader.ReadLine();
            while (line != null)
            {
                string[] bookDetails = line.Split(",");
                Books.Add(bookDetails[0] + " by " + bookDetails[1]);
                line = reader.ReadLine();
            }
            reader.Close();
        }
    }

    [ObservableProperty]
    ObservableCollection<string> books;

    [ObservableProperty]
    string book;

    [ObservableProperty]
    string author;

    [ObservableProperty]
    bool bookFinished;

    [ObservableProperty]
    string summary;

    [ObservableProperty]
    string startDate;

    [ObservableProperty]
    string finishDate;

    [RelayCommand]
    void Add()
    {
        if (string.IsNullOrWhiteSpace(Book) || string.IsNullOrWhiteSpace(Author))
        {
            Book = string.Empty;
            Author = string.Empty;
            Summary = string.Empty;
            BookFinished = false;
            StartDate = string.Empty;
            FinishDate = string.Empty;
            return;
        }
        if (!Books.Contains(Book + " by " + Author))
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(documentsPath, "Books.csv");
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath).Close();  
            }
            string content;
            using (StreamReader reader = new(filePath))
            {
                content = reader.ReadToEnd();
                reader.Close();
            }
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                string bookStatus = "";
                if (BookFinished) 
                {
                    StartDate = DateTime.Today.ToString("d");
                    FinishDate = DateTime.Today.ToString("d");
                    bookStatus = "Finished";
                } else
                {
                    StartDate = DateTime.Today.ToString("d");
                    FinishDate = string.Empty;
                    bookStatus = "Reading";
                }
                if (!string.IsNullOrWhiteSpace(content)) writer.WriteLine(content);
                writer.Write(Book + "," + Author + "," + Summary + "," + StartDate + "," + FinishDate + "," + bookStatus);
                writer.Close();
            }
            Books.Add(Book + " by " + Author);
            Book = string.Empty;
            Author = string.Empty;
            Summary = string.Empty;
            BookFinished = false;
            StartDate = string.Empty;
            FinishDate = string.Empty;
        } else
        {
            Book = "Already read!";
            Author = string.Empty;
        }
    }

    [RelayCommand]
    void Delete(string bookDetail)
    {
        string[] bookDetails = bookDetail.Split(" by ");
        string bookName = bookDetails[0];
        string authorName = bookDetails[1];
        if (Books.Contains(bookDetail))
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filePath = Path.Combine(documentsPath, "Books.csv");
            using (StreamReader reader = new(filePath))
            {
                string book = reader.ReadLine();
                System.IO.File.Create(Path.Combine(documentsPath,"NewBooks.csv")).Close();
                using (StreamWriter writer = new(Path.Combine(documentsPath, "NewBooks.csv")))
                {
                    while (!string.IsNullOrEmpty(book))
                    {
                        string[] savedBook = book.Split(",");
                        string savedBookName = savedBook[0];
                        string savedBookAuthor = savedBook[1];
                        if (!bookName.Equals(savedBookName) || !authorName.Equals(savedBookAuthor))
                        {
                            writer.WriteLine(book);
                        }
                        book = reader.ReadLine();
                    }
                    writer.Close();
                }
                reader.Close();
                System.IO.File.Delete(Path.Combine(documentsPath, "Books.csv"));
                System.IO.File.Move(Path.Combine(documentsPath, "NewBooks.csv"), Path.Combine(documentsPath, "Books.csv"));
            }
            Books.Remove(bookDetail);
        }
    }

    [RelayCommand]
    void DeleteAll()
    {
        Books.Clear();
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(documentsPath, "Books.csv");
        System.IO.File.Create(filePath).Close();
    }

    [RelayCommand]
    async Task Tap(string book)
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string filePath = Path.Combine(documentsPath, "Books.csv");

        string bookName = "";
        string author = "";
        string summary = "";
        string startDate ="";
        string finishDate = "";
        string bookStatus = "";

        if (System.IO.File.Exists(filePath))
        {
            using StreamReader reader = new(filePath);
            string line = reader.ReadLine();
            while (line != null)
            {
                string[] bookDetails = line.Split(",");
                if (book.Equals(bookDetails[0] + " by " + bookDetails[1]))
                {
                    bookName = bookDetails[0];
                    author = bookDetails[1];
                    summary = bookDetails[2];
                    startDate = bookDetails[3];
                    finishDate = bookDetails[4];
                    bookStatus = bookDetails[5];
                }
                line = reader.ReadLine();
            }
            reader.Close();
        }
        await Shell.Current.GoToAsync($"{nameof(BookDetailsPage)}?BookName={bookName}&Author={author}&Summary={summary}&StartDate={startDate}&FinishDate={finishDate}&BookStatus={bookStatus}");
    }
}
