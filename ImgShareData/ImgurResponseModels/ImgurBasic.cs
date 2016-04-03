using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImgShare.APISource.Data.ImgurResponseModels
{
    /// <summary>
    /// This is the basic response for requests that do not return data. If the POST request has a Basic model it will return the id.
    /// http://api.imgur.com/models/basic
    /// </summary>
    public class ImgurBasic
    {
        public bool success { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int httpStatus { get; set; }
    }



}
