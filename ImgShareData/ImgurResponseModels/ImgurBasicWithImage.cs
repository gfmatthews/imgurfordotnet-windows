using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImgShare.APISource.Data.ImgurResponseModels
{
    /// <summary>
    /// A basic return type with some image data
    /// </summary>
    public class ImgurBasicWithImage : ImgurBasic
    {
        [JsonProperty(PropertyName = "data")]
        public ImgurImage Image { get; set; }
    }
}
