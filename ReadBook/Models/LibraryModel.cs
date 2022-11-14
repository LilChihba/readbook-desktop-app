using System;
using System.Windows.Media.Imaging;

namespace ReadBook
{
    public class LibraryModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string DTime { get; set; }
        public BitmapImage Img { get; set; }
    }
}
