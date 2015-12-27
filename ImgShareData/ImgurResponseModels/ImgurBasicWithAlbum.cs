using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.APISource.Data.ImgurResponseModels
{
    class ImgurBasicWithAlbum : ImgurBasic
    {
        // Responses sometimes include a creation response if this response is from creating an object (like an album or image)
        public ImgurAlbumCreationResponse data { get; set; }
    }

    public class ImgurAlbumCreationResponse
    {
        public string id { get; set; }
        public string deletehash { get; set; }
    }
}
