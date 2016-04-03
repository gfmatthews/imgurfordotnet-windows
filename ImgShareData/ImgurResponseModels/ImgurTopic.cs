using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.APISource.Data.ImgurResponseModels
{
    /// <summary>
    /// The data model used in the list of topics
    /// </summary>
    public class ImgurTopicDetail
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string css { get; set; }
        public bool ephemeral { get; set; }
    }

    /// <summary>
    /// The base model for a topic. 
    /// </summary>
    public class ImgurTopicBase
    {
        public List<ImgurTopicDetail> data { get; set; }
        public bool success { get; set; }
        public int status { get; set; }
    }
}
