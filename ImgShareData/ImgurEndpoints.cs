using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.APISource.Data
{
    static public class ImgurEndpoints
    {
        #region API Base
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
        static internal ImgurEndpointUseType _useType = ImgurEndpointUseType.free;
        #endregion

        #region Album Routes
        static private String _album = String.Format(APIBase, "album/{0}");
        static private String _albumImages = String.Format(APIBase, "album/{0}/images");
        static private String _albumImage = String.Format(APIBase, "album/{0}/image/{1}");
        static private String _albumCreation = String.Format(APIBase, "album");
        static private String _albumUpdate = String.Format(APIBase, "album/{0}");
        static private String _albumDeletion = String.Format(APIBase, "album/{0}");
        static private String _albumFavorite = String.Format(APIBase, "album/{0}/favorite");
        static private String _albumSetImages = String.Format(APIBase, "album/{0}");
        static private String _albumAddImages = String.Format(APIBase, "album/{0}/add");
        static private String _albumRemoveImages = String.Format(APIBase, "album/{0}/remove_images");

        static public string Album(string albumID)
        {
            return String.Format(_album, albumID);
        }

        static public string AlbumImages(string albumID)
        {
            return String.Format(_albumImages, albumID);
        }

        static public string AlbumImage(string albumID, string imageID)
        {
            return String.Format(_albumImage, albumID, imageID);
        }

        static public string AlbumCreation()
        {
            return String.Format(_albumCreation);
        }

        static public string AlbumUpdate(string albumID)
        {
            return String.Format(_albumUpdate, albumID);
        }

        static public string AlbumDeletion(string albumID)
        {
            return String.Format(_albumDeletion, albumID);
        }

        static public string AlbumFavorite(string albumID)
        {
            return String.Format(_albumFavorite, albumID);
        }

        static public string AlbumSetImages(string albumID)
        {
            return String.Format(_albumSetImages, albumID);
        }

        static public string AlbumAddImages(string albumID)
        {
            return String.Format(_albumAddImages, albumID);
        }

        static public string AlbumRemoveImages(string albumID)
        {
            return String.Format(_albumRemoveImages, albumID);
        }
        #endregion

        #region Image Routes
        static private String _image = String.Format(APIBase, "image/{0}");
        static private String _imageUpload = String.Format(APIBase, "image");
        static private String _imageDeletion = String.Format(APIBase, "image/{0}");
        static private String _imageUpdate = String.Format(APIBase, "image/{0}");

        static public String Image(String imageID)
        {
            return string.Format(_image,
                                 imageID);
        }

        static public String ImageUpload()
        {
            return String.Format(_imageUpload);
        }

        static public String ImageDeletion(string imageID)
        {
            return String.Format(_imageDeletion, imageID);
        }

        static public String ImageUpdate (string imageID)
        {
            return String.Format(_imageUpdate, imageID);
        }
        #endregion

        #region Gallery Routes
        static private String _gallery = String.Format(APIBase, "gallery/{0}/{1}/{2}.json");
        static private String _gallerySearch = String.Format(APIBase, "gallery/search/{0}/{1}/{2}");
        
        static public String Gallery(GallerySection section, GallerySort sort, int page)
        {
            return String.Format(_gallery,
                                 Utilities.convertToString(section),
                                 Utilities.convertToString(sort),
                                 page);
        }
        
        static public String GallerySearch(GallerySort sort, GallerySearchWindow window, int page, String queryString="")
        {
            String queryBase = String.Format(_gallerySearch,
                                         Utilities.convertToString(sort),
                                         Utilities.convertToString(window),
                                         page
                                         );
            if (queryString == "")
                return queryBase;
            else
            {
                UriBuilder builder = new UriBuilder(queryBase);
                if (queryString.StartsWith("q="))
                    builder.Query = queryString;
                else
                    builder.Query = "q=" + queryString;
                return builder.ToString();
            }

        }
        #endregion

        static public Dictionary<ImgurParameters, String> ImageEndpointParameterLookup = new Dictionary<ImgurParameters, string>()
        {
            {ImgurParameters.image, "image"},
            {ImgurParameters.album, "album"},
            {ImgurParameters.description, "description"},
            {ImgurParameters.title, "title"},
            {ImgurParameters.ids, "ids" },
            {ImgurParameters.layout, "layout" },
            {ImgurParameters.privacy, "privacy" },
            {ImgurParameters.cover, "cover" }
        };
    }


}
