using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO.IsolatedStorage;
using System.IO;

namespace ReadBook
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
                StreamWriter srWriter = new StreamWriter(new IsolatedStorageFileStream("storage", FileMode.Create, isolatedStorage));
                if (App.Current.Properties[0] != null || App.Current.Properties[1] != null)
                {
                    srWriter.WriteLine(App.Current.Properties[0].ToString());
                    srWriter.WriteLine(App.Current.Properties[1].ToString());
                }
                else
                {
                    srWriter.WriteLine(App.Current.Properties[0].ToString());
                    srWriter.WriteLine(App.Current.Properties[1].ToString());
                }

                srWriter.Flush();
                srWriter.Close();
            }
            catch (System.Security.SecurityException sx)
            {
                MessageBox.Show(sx.Message);
                throw;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
                StreamReader srReader = new StreamReader(new IsolatedStorageFileStream("storage", FileMode.OpenOrCreate, isolatedStorage));
                int count = 0;


                if (srReader != null)
                {
                    while (!srReader.EndOfStream)
                    {
                        string item = srReader.ReadLine();
                        App.Current.Properties[count] = item;
                        count++;
                    }
                }
                srReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
    }
}
