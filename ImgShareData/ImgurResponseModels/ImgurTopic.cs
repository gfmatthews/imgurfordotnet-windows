using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.Data.ImgurResponseModels
{
    public class Datum
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string css { get; set; }
        public bool ephemeral { get; set; }
    }

    public class ImgurTopicBase
    {
        public List<Datum> data { get; set; }
        public bool success { get; set; }
        public int status { get; set; }
    }
}
