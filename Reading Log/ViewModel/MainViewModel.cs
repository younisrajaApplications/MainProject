using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Reading_Log.ViewModel;
public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        Books = new ObservableCollection<string>();
        string filePath = "C:\\Books\\Books.csv";
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

    [RelayCommand]
    void Add()
    {
        if (string.IsNullOrWhiteSpace(Book) || string.IsNullOrWhiteSpace(Author))
        {
            Book = string.Empty;
            Author = string.Empty;
            return;
        }
        if (!Books.Contains(Book + " by " + Author))
        {
            string filePath = "C:\\Books\\Books.csv";
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
                    bookStatus = "Finished";
                } else
                {
                    bookStatus = "Reading";
                }
                if (!string.IsNullOrWhiteSpace(content)) writer.WriteLine(content);
                writer.Write(Book + "," + Author + "," + Summary + "," + bookStatus);
                writer.Close();
            }
            Books.Add(Book + " by " + Author);
            Book = string.Empty;
            Author = string.Empty;
            Summary = string.Empty;
            BookFinished = false;
        } else
        {
            Book = "Already read!";
            Author = string.Empty;
        }
    }

    [RelayCommand]
    void Delete(string bookDetail)
    {
        if (Books.Contains(bookDetail))
        {
            Books.Remove(bookDetail);
        }
    }

    [RelayCommand]
    void DeleteAll()
    {
        Books.Clear();
        string filePath = "C:\\Books\\Books.csv";
        System.IO.File.Create(filePath).Close();
    }
}
