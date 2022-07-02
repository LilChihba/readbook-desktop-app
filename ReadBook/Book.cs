using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ReadBook
{
    public class Book
    {
        public string ISBN { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int YearPublic { get; set; }
        public int Pages { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public BitmapImage Img { get; set; }
    }
}
