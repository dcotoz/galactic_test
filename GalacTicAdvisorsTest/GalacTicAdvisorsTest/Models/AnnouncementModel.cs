using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalacTicAdvisorsTest.Models
{
    public class AnnouncementModel
    {
        public int UID { get; set; }
        public string Author { get; set; }
        public DateTime Date { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
