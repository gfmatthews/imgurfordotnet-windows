using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImgShare.Data.ImgurResponseModels
{
    public class ImgurBasic
    {
        public bool success { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int httpStatus { get; set; }
    }
}
