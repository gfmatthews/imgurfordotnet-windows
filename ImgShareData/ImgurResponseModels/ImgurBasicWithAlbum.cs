using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.APISource.Data.ImgurResponseModels
{
    /// <summary>
    /// Basic response tailored for an album
    /// </summary>
    public class ImgurBasicWithAlbum : ImgurBasic
    {
        // Responses sometimes include a creation response if this response is from creating an object (like an album or image)
        public ImgurAlbumCreationResponse data { get; set; }
    }

    /// <summary>
    /// The standard response type returned when creating an album
    /// </summary>
    public class ImgurAlbumCreationResponse
    {
        public string id { get; set; }
        public string deletehash { get; set; }
    }
}
