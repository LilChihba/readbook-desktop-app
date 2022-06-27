using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;


namespace ReadBook
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
            StreamWriter srWriter = new StreamWriter(new IsolatedStorageFileStream("storage", FileMode.Create, isolatedStorage));
            App.Current.Properties[0] = null;
            App.Current.Properties[1] = null;
            App.Current.Properties[2] = null;
            srWriter.WriteLine(App.Current.Properties[0]);
            srWriter.WriteLine(App.Current.Properties[1]);
            srWriter.WriteLine(App.Current.Properties[2]);

            srWriter.Flush();
            srWriter.Close();

            Auth window = new Auth();
            window.Show();
            this.Close();
        }
    }
}
