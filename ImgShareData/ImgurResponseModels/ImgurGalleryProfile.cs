using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.APISource.Data.ImgurResponseModels
{
    /// <summary>
    /// The data type representing a trophy in a user's gallery information
    /// </summary>
    public class Trophy
    {
        public int id { get; set; }
        public string name { get; set; }
        public string name_clean { get; set; }
        public string description { get; set; }
        public object data { get; set; }
        public object data_link { get; set; }
        public int datetime { get; set; }
        public string image { get; set; }
    }

    /// <summary>
    /// The totals for a user's gallery information. 
    /// http://api.imgur.com/models/gallery_profile
    /// </summary>
    public class ImgurGalleryProfile
    {
        public int total_gallery_comments { get; set; }
        public int total_gallery_likes { get; set; }
        public int total_gallery_submissions { get; set; }
        public List<Trophy> trophies { get; set; }
    }
}
