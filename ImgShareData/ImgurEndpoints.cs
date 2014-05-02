using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.Data
{
    static public class ImgurEndpoints
    {
        static internal String APIBase
        {
            get
            {
                switch (_useType)
                {
                    case ImgurEndpointUseType.free:
                        return _freeEndpoint;
                    case ImgurEndpointUseType.paid:
                        return _paidEndpoint;
                    default:
                        return _freeEndpoint;
                }
            }
        }

        static private String _freeEndpoint = "https://api.imgur.com/3/{0}";
        static private String _paidEndpoint = "https://imgur-apiv3.p.mashape.com/";

        static private String _maingallery = String.Format(APIBase, "gallery/{0}/{1}/{2}.json");
        static private String _image = String.Format(APIBase, "image/{0}");
        static private String _imageUpload = String.Format(APIBase, "upload");
        static internal ImgurEndpointUseType _useType = ImgurEndpointUseType.free;

        static public String GetMainGallery(MainGallerySection section, MainGallerySort sort, int page)
        {
            return String.Format(_maingallery,
                                 Utilities.convertToString(section),
                                 Utilities.convertToString(sort),
                                 page);
        }

        static public String Image(String imageID)
        {
            return string.Format(_image,
                                 imageID);
        }

        static public String Image()
        {
            return String.Format(_image,
                                 String.Empty);
        }

        static public Dictionary<ImageEndpointParameters, String> ImageEndpointParameterLookup = new Dictionary<ImageEndpointParameters, string>()
        {
            {ImageEndpointParameters.image, "image"},
            {ImageEndpointParameters.album, "album"},
            {ImageEndpointParameters.description, "description"},
            {ImageEndpointParameters.title, "title"}
        };
    }

    public enum MainGallerySection
    {
        hot,
        top,
        user
    };

    public enum MainGallerySort
    {
        viral,
        time
    };

    public enum ImageEndpointParameters
    {
        image,
        title,
        description,
        album
    };

    public enum ImgurEndpointUseType
    {
        free,
        paid
    };
}
