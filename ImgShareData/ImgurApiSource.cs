using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ImgShare.Data.ImgurResponseModels;
using Newtonsoft.Json;

namespace ImgShare.Data
{
    public class ImgurApiSource
    {
        #region Imgur API Fields
        /// <summary>
        /// You, yeah you.  You need to set these fields to something before you run off
        /// and try to use this object.  You can grab API keys and the secret values by creating an API
        /// account at http://api.imgur.com If you don't, most all the stuff don't work.
        /// Actually, none of it will work so you should go make an account if you haven't already.
        /// 
        /// I'm serious.
        /// </summary>
        internal string _clientid = String.Empty;
        public String ClientID
        {
            set
            {
                this._clientid = value;
            }
        }

        internal string _clientsecret = String.Empty;
        public String ClientSecret
        {
            set
            {
                this._clientsecret = value;
            }
        }

        internal ImgurEndpointUseType _useType = ImgurEndpointUseType.free;

        public ImgurEndpointUseType UseType
        {
            set
            {
                ImgurEndpoints._useType = value;
            }
            get
            {
                return ImgurEndpoints._useType;
            }
        }

        #endregion

        #region Singleton Setup
        private static ImgurApiSource _instance;
        public static ImgurApiSource Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ImgurApiSource();
                }
                return _instance;
            }
        }

        ImgurApiSource()
        {
            _defaultSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", this._clientid);
        }
        #endregion

        #region Private Fields
        /// <summary>
        /// Used for the multipart form data for processing imgur posts
        /// </summary>
        Guid BoundaryGuid = Guid.NewGuid();

        internal JsonSerializerSettings _defaultSerializerSettings = new JsonSerializerSettings();
        private HttpClient _client = new HttpClient();

        /// <summary>
        /// The following field can be set to "free" or "paid" depending on which version of the API
        /// you're using.  Set once in this process' lifetime and forget.  Defaults to free
        /// </summary>
        public static ImgurEndpointUseType APIUseType
        {
            set
            {
                ImgurEndpoints._useType = value;
            }
        }
        #endregion

        #region API Calls
        /// <summary>
        /// Returns the list of images in the Main gallery in the given section and with the given sort method
        /// </summary>
        /// <param name="section">The section to get the images from.</param>
        /// <param name="sorting">The sorting method to use to sort the returned images.  Default is the viral sorting method.</param>
        /// <param name="page">The page number to return images from.  Default is the first page (0).</param>
        /// <returns>The list of images in the chosen gallery</returns>
        public async Task<ImgurGalleryImageList> GetMainGalleryImagesAsync(MainGallerySection section, MainGallerySort sorting=MainGallerySort.viral, int page=0)
        {
            string responseString = await GetAnonymousImgurDataAsync(ImgurEndpoints.GetMainGallery(section, sorting, page));
            return await Task.Run( () => JsonConvert.DeserializeObject<ImgurGalleryImageList>(responseString, _defaultSerializerSettings));
        }

        /// <summary>
        /// Search the gallery with a given query string.
        /// </summary>
        /// <param name="sorting">time | viral | top - defaults to time</param>
        /// <param name="window">Change the date range of the request if the sort is 'top', day | week | month | year | all, defaults to all.</param>
        /// <param name="page">the data paging number</param>
        /// <param name="query">Query string. This parameter also supports boolean operators (AND, OR, NOT) and indices (tag: user: title: ext: subreddit: album: meme:). An example compound query would be 'title: cats AND dogs ext: gif'</param>
        /// <returns></returns>
        public async Task<ImgurGalleryImageList> SearchMainGalleryImagesAsync(MainGallerySort sorting=MainGallerySort.time, GalleryWindow window=GalleryWindow.all, int page=0, string query ="")
        {
            String responseString = await GetAnonymousImgurDataAsync(ImgurEndpoints.GallerySearch(sorting, window, page, query));
            return await Task.Run( () => JsonConvert.DeserializeObject<ImgurGalleryImageList>(responseString, _defaultSerializerSettings));
        }

        /// <summary>
        /// Fills in an ImgurImage object with details for a specific image.  Requires an ImageID.
        /// </summary>
        /// <param name="imageID">The Imgur image ID of the image you want details for.</param>
        /// <returns>A nicely filled in ImgurImage object.  All for you.</returns>
        public async Task<ImgurImage> GetImageDetailsAsync(String imageID)
        {
            String responseString = await GetAnonymousImgurDataAsync(ImgurEndpoints.Image(imageID));
            return (await Task.Run( () => JsonConvert.DeserializeObject<ImgurBasicWithImage>(responseString))).Image;
        }

        /// <summary>
        /// Upload a new image.
        /// </summary>
        /// <param name="ImageToUploadFileData">A Byte array representing the image</param>
        /// <param name="Title">The title of the image</param>
        /// <param name="Description"> The description of the image</param>
        /// <param name="Album">The id of the album you want to add the image to. For anonymous albums, {album} should be the deletehash that is returned at creation.</param>
        /// <returns>A ImgurImage object that represents the image just uploaded</returns>
        public async Task<ImgurImage> PostImageAnonymousAsync(Byte[] ImageToUploadFileData, String Title="", String Description="", String Album="")
        {
            // TODO: Make all the strings used in form content constants somewhere
            MultipartFormDataContent content = new MultipartFormDataContent(BoundaryGuid.ToString());
            content.Add(new ByteArrayContent(ImageToUploadFileData), ImgurEndpoints.ImageEndpointParameterLookup[ImageEndpointParameters.image]);
            if (Title != "")
            {
                content.Add(new StringContent(Title), ImgurEndpoints.ImageEndpointParameterLookup[ImageEndpointParameters.title]);
            }
            if (Description != "")
            {
                content.Add(new StringContent(Description), ImgurEndpoints.ImageEndpointParameterLookup[ImageEndpointParameters.description]);
            }
            if (Album != "")
            {
                content.Add(new StringContent(Album), ImgurEndpoints.ImageEndpointParameterLookup[ImageEndpointParameters.album]);
            }
     
            String responseString = await PostAnonymousImgurDataAsync(ImgurEndpoints.Image(), content);
            ImgurBasicWithImage returnedImage = await Task.Run( () => JsonConvert.DeserializeObject<ImgurBasicWithImage>(responseString, _defaultSerializerSettings));

            return returnedImage.Image;
        }

        /// <summary>
        /// Upload a new image.
        /// </summary>
        /// <param name="url">The url to an image to upload</param>
        /// <param name="Title">The title of the image</param>
        /// <param name="Description"> The description of the image</param>
        /// <param name="Album">The id of the album you want to add the image to. For anonymous albums, {album} should be the deletehash that is returned at creation.</param>
        /// <returns>A ImgurImage object that represents the image just uploaded</returns>
        public async Task<ImgurImage> PostImageAnonymousAsync(String url, String Title="", String Description="", String Album="")
        {
            MultipartFormDataContent content = new MultipartFormDataContent(BoundaryGuid.ToString());
            content.Add(new StringContent(url), ImgurEndpoints.ImageEndpointParameterLookup[ImageEndpointParameters.image]);
            if (Title != "")
            {
                content.Add(new StringContent(Title), ImgurEndpoints.ImageEndpointParameterLookup[ImageEndpointParameters.title]);
            }
            if (Description != "")
            {
                content.Add(new StringContent(Description), ImgurEndpoints.ImageEndpointParameterLookup[ImageEndpointParameters.description]);
            }
            if (Album != "")
            {
                content.Add(new StringContent(Album), ImgurEndpoints.ImageEndpointParameterLookup[ImageEndpointParameters.album]);
            }

            String responseString = await PostAnonymousImgurDataAsync(ImgurEndpoints.Image(), content);
            ImgurBasicWithImage returnedImage = await Task.Run( () => JsonConvert.DeserializeObject<ImgurBasicWithImage>(responseString, _defaultSerializerSettings));

            return returnedImage.Image;
        }

        /// <summary>
        /// Deletes an image
        /// </summary>
        /// <param name="deleteID">If this image belongs to the account, the ID, if not, the deletehash</param>
        /// <returns>An ImgurBasic response</returns>
        public async Task<ImgurBasic> DeleteImageAsync(String deleteID)
        {
            String responseString = await DeleteImgurDataAsync(ImgurEndpoints.Image(deleteID));
            return await Task.Run( () => JsonConvert.DeserializeObject<ImgurBasic>(responseString, _defaultSerializerSettings));
        }

        /// <summary>
        /// Updates the title or description of an image. You can only update an image you own and is associated with your account. For an anonymous image, {id} must be the image's deletehash.
        /// </summary>
        /// <param name="deleteHashOrImageID">The deletehash or ID of an image (ID ONLY WORKS IF LOGGED IN!)</param>
        /// <param name="Title">The title of the image.</param>
        /// <param name="Description">The description of the image.</param>
        /// <returns></returns>
        public async Task<ImgurBasic> UpdateImageInformationAsync(String deleteHashOrImageID, String Title="", String Description="")
        {
            MultipartFormDataContent content = new MultipartFormDataContent(BoundaryGuid.ToString());
            if (Title != "")
            {
                content.Add(new StringContent(Title), ImgurEndpoints.ImageEndpointParameterLookup[ImageEndpointParameters.title]);
            }
            if (Description != "")
            {
                content.Add(new StringContent(Description), ImgurEndpoints.ImageEndpointParameterLookup[ImageEndpointParameters.description]);
            }
            String responseString = await PostAnonymousImgurDataAsync(ImgurEndpoints.Image(deleteHashOrImageID), content);
            ImgurBasic status = await Task.Run(() => JsonConvert.DeserializeObject<ImgurBasic>(responseString, _defaultSerializerSettings));

            return status;
        }


        #endregion

        #region Internal Methods to Fetch and Add Data
        /// <summary>
        /// Fetches data asynchronously (and anonymously) for a given Imgur API endpoint.  Used by other calls in this 
        /// object.
        /// </summary>
        /// <param name="endpoint">The Imgur endpoint to run the HTTP get operation on</param>
        /// <returns>A string formatted response.  This will need to be parsed before it returns useful Imgur data.</returns>
        protected internal async Task<String> GetAnonymousImgurDataAsync(String endpoint)
        {
            EnsureClientHasAuthorizationHeaders();
            HttpResponseMessage response = await _client.GetAsync(endpoint);
            return await ProcessResponseAsync(response);
        }

        /// <summary>
        /// Posts the content data to the endpoint specified.
        /// </summary>
        /// <param name="endpoint">The Imgur endpoint to run the HTTP post operation on</param>
        /// <param name="content">The HttpContent object that contains the data to post</param>
        /// <returns>A string formatted response.  This will need to be parsed before it returns useful Imgur data.</returns>
        protected internal async Task<String> PostAnonymousImgurDataAsync(String endpoint, HttpContent content)
        {
            EnsureClientHasAuthorizationHeaders();
            HttpResponseMessage response = await _client.PostAsync(endpoint, content);
            return await ProcessResponseAsync(response);
        }

        /// <summary>
        /// Runs a delete operation on the endpoint specified.
        /// </summary>
        /// <param name="endpoint">The Imgur endpoint to run the delete operation on</param>
        /// <returns>The response</returns>
        protected internal async Task<String> DeleteImgurDataAsync(String endpoint)
        {
            EnsureClientHasAuthorizationHeaders();
            HttpResponseMessage response = await _client.DeleteAsync(endpoint);
            return await ProcessResponseAsync(response);
        }

        /// <summary>
        /// Runs a put operation on the endpoint specified.
        /// </summary>
        /// <param name="endpoint">The Imgur endpoint to run the put operation on</param>
        /// <param name="content">Content to be included with the put operation</param>
        /// <returns>The response as a string</returns>
        protected internal async Task<String> PutAnonymousImgurDataAsync(String endpoint, HttpContent content)
        {
            EnsureClientHasAuthorizationHeaders();
            HttpResponseMessage response = await _client.PutAsync(endpoint, content);
            return await ProcessResponseAsync(response);
        }

        /// <summary>
        /// Processes the response string from an HttpResponseMessage
        /// </summary>
        /// <param name="response">The HttpResponseMessage to process</param>
        /// <returns>The response message as a string, throws an exception if there's not a valid status code</returns>
        protected internal async Task<string> ProcessResponseAsync(HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            String responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        /// <summary>
        /// Double checks that the client has the authorization header attached before continuing.
        /// </summary>
        protected internal void EnsureClientHasAuthorizationHeaders()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", this._clientid);
        }
        #endregion
    }
}
