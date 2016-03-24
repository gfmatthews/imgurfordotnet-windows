using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImgShare.APISource.Data.ImgurResponseModels
{
    /// <summary>
    /// This model represents the data for albums 
    /// http://api.imgur.com/models/album
    /// </summary>
    public class ImgurAlbum
    {
        public String ID { get; set; }

        public String deletehash { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        [JsonProperty(PropertyName = "datetime")]
        public Int64 DateStamp { get; set; }

        [JsonProperty(PropertyName = "account_url")]
        public String AccountURL { get; set; }

        [JsonProperty(PropertyName = "cover")]
        public String CoverID { get; set; }

        public String privacy { get; set; }

        public String layout { get; set; }

        public Int64 views { get; set; }

        [JsonProperty(PropertyName = "images_count")]
        public int ImagesInAlbum { get; set; }

        [JsonProperty(PropertyName="Images")]
        public IEnumerable<ImgurImage> Images { get; set; }
    }

    internal class ImgurAlbumResponse : ImgurBasic
    {
        [JsonProperty(PropertyName ="data")]
        public ImgurAlbum album { get; set; }
    }
}
