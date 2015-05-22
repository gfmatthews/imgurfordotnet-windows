using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImgShare.APISource.Data.ImgurResponseModels
{
    public class ImgurAccountSettings
    {
        public String Email { get; set; }

        [JsonProperty(PropertyName = "high_quality")]
        public bool canUploadHighQuality { get; set; }

        [JsonProperty(PropertyName = "public_images")]
        public bool AutomaticallyAllowAllImagesPublic { get; set; }

        [JsonProperty(PropertyName = "album_privacy")]
        public String AlbumPrivacy { get; set; }

        [JsonProperty(PropertyName="pro_expiration")]
        public String ProExpirationDate {get; set;}

        public bool IsProAccount
        {
            get
            {
                if (String.Compare(ProExpirationDate, "false") == 0)
                {   return false;  }
                else { return true; }
            }
        }

        [JsonProperty(PropertyName = "accepted_gallery_terms")]
        public bool didAcceptGalleryTerms { get; set; }

        [JsonProperty(PropertyName = "active_emails")]
        public IEnumerable<String> ActiveEmails { get; set; }

        [JsonProperty(PropertyName = "messaging_enabled")]
        public bool areIncomingMessagesEnabled { get; set; }

    }
}
