using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Input;

namespace ReadBook
{
    public partial class MainWindow : Window
    {
        private bool hide = false;

        public MainWindow()
        {
            InitializeComponent();
            ContentFrame.Navigate(new ReadBook.Pages.Recommended());
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ExitImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void RollupImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ExitAccImg_MouseDown(object sender, MouseButtonEventArgs e)
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

        private void MyLibraryImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentFrame.Navigate(new Pages.MyLibrary());
        }

        private void RecomImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentFrame.Navigate(new Pages.Recommended());
        }

        private void PolygonMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!hide)
            {
                HideMenu.Width = new GridLength(1000);
                hide = true;
                
            }
            else
            {
                HideMenu.Width = new GridLength(800);
                hide = false;
            }
        }

        private void SearchImg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContentFrame.Navigate(new Pages.Search());
        }
    }
}
