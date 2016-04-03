using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImgShare.APISource.Data.ImgurResponseModels
{
    /// <summary>
    /// The data model formatted for gallery images. 
    /// http://api.imgur.com/models/gallery_image
    /// </summary>
    public class ImgurGalleryImage : ImgurImage
    {
        public bool Vote { get; set; }

        [JsonProperty(PropertyName="account_url")]
        public String AccountURL { get; set; }

        public Int32 Ups { get; set; }

        public Int32 Downs { get; set; }

        public Int32 Score { get; set; }

        [JsonProperty(PropertyName="is_album")]
        public bool isAlbum { get; set; }

    }

    /// <summary>
    /// The data type representing a response with a list of images
    /// </summary>
    public class ImgurGalleryImageList
    {
        [JsonProperty(PropertyName = "data")]
        public IEnumerable<ImgurGalleryImage> Images { get; set; }
    }
}
