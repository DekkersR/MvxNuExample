using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvxNuExample.Models
{
    public class News
    {
        public int id { get; set; }
        public string title { get; set; }
        public string excerpt { get; set; }
        public string copyright { get; set; }
        public string created_by { get; set; }
        public string body { get; set; }
        public string media_id { get; set; }
        public string mediaUrl => String.Format("http://media.nu.nl/m/{0}_wd640.jpg", media_id);
    }
}
