using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.APISource.Data.ImgurResponseModels
{
    /// <summary>
    /// The base model for a vote. 
    /// http://api.imgur.com/models/vote
    /// </summary>
    public class ImgurVote
    {
        public int ups { get; set; }
        public int downs { get; set; }
    }
}
