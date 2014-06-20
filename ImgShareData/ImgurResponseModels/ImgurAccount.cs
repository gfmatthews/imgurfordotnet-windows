using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ImgShare.Data.ImgurResponseModels
{
    /// <summary>
    /// A imgur object that represents an account.
    /// </summary>
    public class ImgurAccount
    {
        public int ID { get; set; }

        public String URL { get; set; }

        public String Bio { get; set; }

        public float Reputation { get; set; }

        [JsonProperty(PropertyName = "created")]
        public String DateCreated { get; set; }
    }
}
