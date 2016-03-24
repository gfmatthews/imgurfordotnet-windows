using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImgShare.APISource.Data.ImgurResponseModels
{
    /// <summary>
    /// This data model represents an images comment. 
    /// http://api.imgur.com/models/comment
    /// </summary>
    public class ImgurComment
    {
        public String ID { get; set; }

        [JsonProperty(PropertyName = "image_id")]
        public String ImageID { get; set; }

        public String Caption { get; set; }

        public String Author { get; set; }

        [JsonProperty(PropertyName="author_id")]
        public int AuthorID { get; set; }

        [JsonProperty(PropertyName = "on_album")]
        public bool isCommentOnAlbum { get; set; }

        [JsonProperty(PropertyName = "album_cover")]
        public String AlbumCoverID { get; set; }

        public Int64 ups { get; set; }

        public Int64 downs { get; set; }

        public float points { get; set; }

        [JsonProperty(PropertyName = "datetime")]
        public Int64 DateStamp { get; set; }

        [JsonProperty(PropertyName = "parent_id")]
        public int ParentID { get; set; }

        public bool Deleted { get; set; }

        public IEnumerable<ImgurComment> children { get; set; }
    }
}
