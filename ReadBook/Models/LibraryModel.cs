using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadBook
{
    public class LibraryModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ISBN { get; set; }
        public string Name { get; set; }
        public DateTime DTime { get; set; }
    }
}
