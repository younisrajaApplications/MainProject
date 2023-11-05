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
                Books.Add(line);
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

    [RelayCommand]
    void Add()
    {
        if (string.IsNullOrWhiteSpace(Book) || string.IsNullOrWhiteSpace(Author))
        {
            Book = string.Empty;
            Author = string.Empty;
            return;
        }
        if (!Books.Contains(Book + " : " + Author))
        {
            string filePath = "C:\\Books\\Books.csv";
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.Create(filePath);  
            }
            string content;
            using (StreamReader reader = new(filePath))
            {
                content = reader.ReadToEnd();
                reader.Close();
            }
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                if (!string.IsNullOrWhiteSpace(content)) writer.WriteLine(content);
                writer.Write(Book + " : " + Author);
                writer.Close();
            }
            Books.Add(Book + " : " + Author);
            Book = string.Empty;
            Author = string.Empty;
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
}
