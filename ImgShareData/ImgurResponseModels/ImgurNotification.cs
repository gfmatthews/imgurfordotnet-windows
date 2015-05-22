using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.APISource.Data.ImgurResponseModels
{
    public class ImgurReplyDetails
    {
        public object album_cover { get; set; }
        public string author { get; set; }
        public int author_id { get; set; }
        public List<ImgurComment> children { get; set; }
        public string comment { get; set; }
        public int datetime { get; set; }
        public bool deleted { get; set; }
        public int downs { get; set; }
        public int id { get; set; }
        public string image_id { get; set; }
        public bool on_album { get; set; }
        public int parent_id { get; set; }
        public int points { get; set; }
        public int ups { get; set; }
        public object vote { get; set; }
    }

    public class Reply
    {
        public int id { get; set; }
        public int account_id { get; set; }
        public bool viewed { get; set; }
        public ImgurReplyDetails content { get; set; }
    }

    public class ImgurMessage
    {
        public string id { get; set; }
        public string from { get; set; }
        public string account_id { get; set; }
        public string with_account { get; set; }
        public string last_message { get; set; }
        public string message_num { get; set; }
        public int datetime { get; set; }
    }

    public class ImgurNotificationMessage
    {
        public int id { get; set; }
        public int account_id { get; set; }
        public bool viewed { get; set; }
        public ImgurMessage content { get; set; }
    }

    public class ImgurNotification
    {
        public List<Reply> replies { get; set; }
        public List<ImgurNotificationMessage> messages { get; set; }
    }
}
