using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImgShare.APISource.Data.ImgurResponseModels
{
    public class ImgurAccountStatistics
    {
        public int ID { get; set; }

        [JsonProperty(PropertyName="total_image")]
        public int TotalImages { get; set; }

        [JsonProperty(PropertyName="total_albums")]
        public int TotalAlbums { get; set; }

        [JsonProperty(PropertyName="disk_used")]
        public string DiskUsed { get; set; }

        [JsonProperty(PropertyName = "bandwidth_used")]
        public string BandwidthUsed { get; set; }

        [JsonProperty(PropertyName = "top_images")]
        public IEnumerable<ImgurImage> TopImages {get; set;}

        [JsonProperty(PropertyName = "top_albums")]
        public IEnumerable<ImgurAlbum> TopAlbums { get; set; }

        [JsonProperty(PropertyName = "top_gallery_comments")]
        public IEnumerable<ImgurComment> TopComments { get; set; }
    }
}
