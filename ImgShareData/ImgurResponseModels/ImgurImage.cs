using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace ImgShare.Data.ImgurResponseModels
{
    public class ImgurImage
    {
        public String ID { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        [JsonProperty(PropertyName="datetime")]
        public Int64 DateStamp { get; set; }

        public String Type { get; set; }

        [JsonProperty(PropertyName="animated")]
        public bool isAnimated { get; set; }

        public Int32 Width { get; set; }

        public Int32 Height { get; set; }

        public Int64 Size { get; set; }

        public Int64 Views { get; set; }

        public Int64 Bandwidth { get; set; }

        public String Deletehash { get; set; }

        public String Link { get; set; }

        public String LinkThumbnail160x160
        {
            get
            {
                return GetThumbnailStringForLink(ThumbnailType.Small);
            }
        }

        public String LinkThumbnail320x320
        {
            get
            {
                return GetThumbnailStringForLink(ThumbnailType.Medium);
            }
        }

        public String LinkThumbnail640x640
        {
            get
            {
                return GetThumbnailStringForLink(ThumbnailType.Large);
            }
        }

        public String LinkThumbnail1024x1024
        {
            get
            {
                return GetThumbnailStringForLink(ThumbnailType.Huge);
            }
        }

        private String GetThumbnailStringForLink(ThumbnailType type)
        {
            Uri returnLink = new Uri(Link);

            String fileName = returnLink.Segments.Last();
            String newFileName;
            switch(type)
            {
                case ThumbnailType.Small:
                    newFileName = fileName.Replace(".", "t.");
                    break;
                case ThumbnailType.Medium:
                    newFileName = fileName.Replace(".", "m.");
                    break;
                case ThumbnailType.Large:
                    newFileName = fileName.Replace(".", "l.");
                    break;
                case ThumbnailType.Huge:
                    newFileName = fileName.Replace(".", "h.");
                    break;
                default:
                    // use huge if type isn't specified
                    newFileName = fileName.Replace(".", "h.");
                    break;
            }

            UriBuilder builder = new UriBuilder();
            builder.Host = returnLink.Host;
            builder.Path = newFileName;
            return builder.Uri.ToString();
        }

    }

    enum ThumbnailType
    {
        Small,
        Medium,
        Large,
        Huge
    };
}
