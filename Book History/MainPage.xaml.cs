namespace Book_History
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnHistoryClicked(object sender, EventArgs e)
        {
            HistoryBtn.Text = "Showing History";
            SemanticScreenReader.Announce(HistoryBtn.Text);
        }

        private void OnReadClicked(Object sender, EventArgs e)
        {
            ReadBookBtn.Text = "Reading new book!";
            SemanticScreenReader.Announce(ReadBookBtn.Text);
        }

        private void OnQuitClicked(Object sender, EventArgs e)
        {
            
        }
    }
}