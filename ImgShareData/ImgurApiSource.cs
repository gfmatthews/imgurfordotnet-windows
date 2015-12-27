using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ImgShare.APISource.Data.ImgurResponseModels;
using Newtonsoft.Json;

namespace ImgShare.APISource.Data
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

        #region [API] Album Endpoint
        /// <summary>
        /// Get information about a specific album
        /// </summary>
        /// <param name="albumID">The requested album ID</param>
        /// <returns>A filled in album object</returns>
        public async Task<ImgurAlbum> AlbumDetailsAsync(string albumID)
        {
            string responseString = await GetAnonymousImgurDataAsync(ImgurEndpoints.Album(albumID));
            ImgurAlbumResponse x = await Task.Run(() => JsonConvert.DeserializeObject<ImgurAlbumResponse>(responseString, _defaultSerializerSettings));
            return x.album;
        }

        /// <summary>
        /// Gets a list of the images in an album
        /// </summary>
        /// <param name="albumID">The requested album ID</param>
        /// <returns>The list of images in the album</returns>
        public async Task<IEnumerable<ImgurImage>> AlbumImagesAsync(string albumID)
        {
            string responseString = await GetAnonymousImgurDataAsync(ImgurEndpoints.AlbumImages(albumID));
            ImgurGalleryImageList listBase = await Task.Run(() => JsonConvert.DeserializeObject<ImgurGalleryImageList>(responseString, _defaultSerializerSettings));
            return listBase.Images;
        }

        /// <summary>
        /// Create a new album.
        /// </summary>
        /// <param name="imageIDs">The image ids that you want to be included in the album.</param>
        /// <param name="coverImageId">The id of the image you want to set as the cover.  If the coverImageID isn't in the list you specified in the first parameter, this is ignored.</param>
        /// <param name="title"> The title of the album </param>
        /// <param name="description">The description of the album</param>
        /// <param name="albumPrivacy">Sets the privacy level of the album. Values are : public | hidden | secret. Defaults to user's privacy settings for logged in users.</param>
        /// <param name="albumLayout">Sets the layout to display the album. Values are : blog | grid | horizontal | vertical</param>
        /// <returns></returns>
        public async Task<ImgurAlbum> AlbumCreationAsync(List<string> imageIDs, string coverImageId, String title = "", String description = "", Privacy albumPrivacy = Privacy.ignore, Layout albumLayout = Layout.ignore)
        {
            MultipartFormDataContent content = new MultipartFormDataContent(BoundaryGuid.ToString());
            if (imageIDs.Count != 0 || imageIDs == null)
            {
                string serializedImageList = await Task.Run(() => JsonConvert.SerializeObject(imageIDs));
                content.Add(new StringContent(serializedImageList), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.ids]);
            }
            if (imageIDs.Contains(coverImageId))
            {
                content.Add(new StringContent(coverImageId), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.cover]);
            }
            if (title != "")
            {
                content.Add(new StringContent(title), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.title]);
            }
            if (description != "")
            {
                content.Add(new StringContent(description), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.description]);
            }
            if (albumPrivacy != Privacy.ignore)
            {
                content.Add(new StringContent(Utilities.convertToString(albumPrivacy)), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.privacy]);
            }
            if (albumLayout != Layout.ignore)
            {
                content.Add(new StringContent(Utilities.convertToString(albumLayout)), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.layout]);
            }
            String responseString = await PostAnonymousImgurDataAsync(ImgurEndpoints.AlbumCreation(), content);
            ImgurBasicWithAlbum status = await Task.Run(() => JsonConvert.DeserializeObject<ImgurBasicWithAlbum>(responseString, _defaultSerializerSettings));

            // By default, we only get the deletehash in the basic response but this should really include the deletehash
            // so we make another call to fill in the rest of the details about the object 
            ImgurAlbum album = await this.AlbumDetailsAsync(status.data.id);
            album.deletehash = status.data.deletehash;

            return album;
        }

        /// <summary>
        /// Create a new album.
        /// </summary>
        /// <param name="imageList">The images that you want to be included in the album.</param>
        /// <param name="coverImage">The image you want to set as the cover.  If the coverImage isn't in the list you specified in the first parameter, this is ignored.</param>
        /// <param name="title"> The title of the album </param>
        /// <param name="description">The description of the album</param>
        /// <param name="albumPrivacy">Sets the privacy level of the album. Values are : public | hidden | secret. Defaults to user's privacy settings for logged in users.</param>
        /// <param name="albumLayout">Sets the layout to display the album. Values are : blog | grid | horizontal | vertical</param>
        public async Task<ImgurAlbum> AlbumCreationAsync(List<ImgurImage> imageList, ImgurImage coverImage, String title = "", String description = "", Privacy albumPrivacy = Privacy.ignore, Layout albumLayout = Layout.ignore)
        {
            List<string> imageIDList = new List<string>();
            foreach (ImgurImage i in imageList)
            {
                imageIDList.Add(i.ID);
            }

            return await AlbumCreationAsync(imageIDList, coverImage.ID, title, description, albumPrivacy, albumLayout);
        }

        /// <summary>
        /// Update the information of an album. For anonymous albums, {album} should be the deletehash that is returned at creation. 
        /// </summary>
        /// <param name="albumID">The album ID to update</param>
        /// <param name="imageList">The images that you want to be included in the album.</param>
        /// <param name="coverImage">The image you want to set as the cover.</param>
        /// <param name="title"> The title of the album </param>
        /// <param name="description">The description of the album</param>
        /// <param name="albumPrivacy">Sets the privacy level of the album. Values are : public | hidden | secret. Defaults to user's privacy settings for logged in users.</param>
        /// <param name="albumLayout">Sets the layout to display the album. Values are : blog | grid | horizontal | vertical</param>
        /// <returns></returns>
        public async Task<ImgurAlbum> AlbumUpdateAsync(string albumID, List<ImgurImage> imageList, ImgurImage coverImage, String title = "", String description = "", Privacy albumPrivacy = Privacy.ignore, Layout albumLayout = Layout.ignore)
        {
            List<string> imageIDList = new List<string>();
            if (imageList != null)
            {

                foreach (ImgurImage i in imageList)
                {
                    imageIDList.Add(i.ID);
                }
            }
            return await AlbumUpdateAsync(albumID, imageIDList, coverImage.ID, title, description, albumPrivacy, albumLayout);
        }

        /// <summary>
        /// Update the information of an album. For anonymous albums, {album} should be the deletehash that is returned at creation. 
        /// </summary>
        /// <param name="albumID">The album ID (only if logged in) or deletehash of the album to update</param>
        /// <param name="imageIDs">The image ids that you want to be included in the album.</param>
        /// <param name="coverImageId">The id of the image you want to set as the cover.</param>
        /// <param name="title"> The title of the album </param>
        /// <param name="description">The description of the album</param>
        /// <param name="albumPrivacy">Sets the privacy level of the album. Values are : public | hidden | secret. Defaults to user's privacy settings for logged in users.</param>
        /// <param name="albumLayout">Sets the layout to display the album. Values are : blog | grid | horizontal | vertical</param>
        /// <returns></returns>
        public async Task<ImgurAlbum> AlbumUpdateAsync(string albumID, List<string> imageIDs = null, string coverImageId = "", String title = "", String description = "", Privacy albumPrivacy = Privacy.ignore, Layout albumLayout = Layout.ignore)
        {
            if (albumID == null)
            {
                throw new ArgumentNullException("albumID");
            }

            MultipartFormDataContent content = new MultipartFormDataContent(BoundaryGuid.ToString());
            if (imageIDs.Count != 0 || imageIDs == null)
            {
                string serializedImageList = await Task.Run(() => JsonConvert.SerializeObject(imageIDs));
                content.Add(new StringContent(serializedImageList), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.ids]);
            }
            if (coverImageId != String.Empty)
            {
                content.Add(new StringContent(coverImageId), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.cover]);
            }
            if (title != "")
            {
                content.Add(new StringContent(title), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.title]);
            }
            if (description != "")
            {
                content.Add(new StringContent(title), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.description]);
            }
            if (albumPrivacy != Privacy.ignore)
            {
                content.Add(new StringContent(Utilities.convertToString(albumPrivacy)), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.privacy]);
            }
            if (albumLayout != Layout.ignore)
            {
                content.Add(new StringContent(Utilities.convertToString(albumLayout)), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.layout]);
            }
            String responseString = await PostAnonymousImgurDataAsync(ImgurEndpoints.AlbumUpdate(albumID), content);
            ImgurBasicWithAlbum status = await Task.Run(() => JsonConvert.DeserializeObject<ImgurBasicWithAlbum>(responseString, _defaultSerializerSettings));

            // By default, we only get the deletehash in the basic response but this should really include the deletehash
            // so we make another call to fill in the rest of the details about the object 
            ImgurAlbum album = await this.AlbumDetailsAsync(status.data.id);

            // if the user passed in the deletehash, fill that back in the object we're about to send back
            if (album.ID.ToLower() != albumID.ToLower())
            {
                album.deletehash = status.data.deletehash;
            }

            return album;
        }

        /// <summary>
        /// Deletes an album
        /// </summary>
        /// <param name="albumID">The delete hash or album ID (only if logged in) you want to delete.</param>
        /// <returns></returns>
        public async Task<ImgurBasic> AlbumDeletionAsync(string albumID)
        {
            String responseString = await DeleteImgurDataAsync(ImgurEndpoints.AlbumDeletion(albumID));
            return await Task.Run(() => JsonConvert.DeserializeObject<ImgurBasic>(responseString, _defaultSerializerSettings));
        }


        #endregion

        #region [API] Gallery Endpoint
        /// <summary>
        /// Returns the list of images in the Main gallery in the given section and with the given sort method
        /// </summary>
        /// <param name="section">The section to get the images from.</param>
        /// <param name="sorting">The sorting method to use to sort the returned images.  Default is the viral sorting method.</param>
        /// <param name="page">The page number to return images from.  Default is the first page (0).</param>
        /// <returns>The list of images in the chosen gallery</returns>
        public async Task<ImgurGalleryImageList> GalleryDetailsAsync(GallerySection section, GallerySort sorting = GallerySort.viral, int page = 0)
        {
            string responseString = await GetAnonymousImgurDataAsync(ImgurEndpoints.Gallery(section, sorting, page));
            return await Task.Run(() => JsonConvert.DeserializeObject<ImgurGalleryImageList>(responseString, _defaultSerializerSettings));
        }

        /// <summary>
        /// Search the gallery with a given query string.
        /// </summary>
        /// <param name="sorting">time | viral | top - defaults to time</param>
        /// <param name="window">Change the date range of the request if the sort is 'top', day | week | month | year | all, defaults to all.</param>
        /// <param name="page">the data paging number</param>
        /// <param name="query">Query string. This parameter also supports boolean operators (AND, OR, NOT) and indices (tag: user: title: ext: subreddit: album: meme:). An example compound query would be 'title: cats AND dogs ext: gif'</param>
        /// <returns></returns>
        public async Task<ImgurGalleryImageList> GallerySearchAsync(GallerySort sorting = GallerySort.time, GallerySearchWindow window = GallerySearchWindow.all, int page = 0, string query = "")
        {
            String responseString = await GetAnonymousImgurDataAsync(ImgurEndpoints.GallerySearch(sorting, window, page, query));
            return await Task.Run(() => JsonConvert.DeserializeObject<ImgurGalleryImageList>(responseString, _defaultSerializerSettings));
        }
        #endregion

        #region [API] Image Endpoint
        /// <summary>
        /// Fills in an ImgurImage object with details for a specific image.  Requires an ImageID.
        /// </summary>
        /// <param name="imageID">The Imgur image ID of the image you want details for.</param>
        /// <returns>A nicely filled in ImgurImage object.  All for you.</returns>
        public async Task<ImgurImage> ImageDetailsAsync(String imageID)
        {
            String responseString = await GetAnonymousImgurDataAsync(ImgurEndpoints.Image(imageID));
            return (await Task.Run(() => JsonConvert.DeserializeObject<ImgurBasicWithImage>(responseString))).Image;
        }

        /// <summary>
        /// Upload a new image.
        /// </summary>
        /// <param name="ImageToUploadFileData">A Byte array representing the image</param>
        /// <param name="Title">The title of the image</param>
        /// <param name="Description"> The description of the image</param>
        /// <param name="Album">The id of the album you want to add the image to. For anonymous albums, {album} should be the deletehash that is returned at creation.</param>
        /// <returns>A ImgurImage object that represents the image just uploaded</returns>
        public async Task<ImgurImage> ImageUploadAsync(Byte[] ImageToUploadFileData, String Title = "", String Description = "", String Album = "")
        {
            // TODO: Make all the strings used in form content constants somewhere
            MultipartFormDataContent content = new MultipartFormDataContent(BoundaryGuid.ToString());
            content.Add(new ByteArrayContent(ImageToUploadFileData), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.image]);
            if (Title != "")
            {
                content.Add(new StringContent(Title), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.title]);
            }
            if (Description != "")
            {
                content.Add(new StringContent(Description), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.description]);
            }
            if (Album != "")
            {
                content.Add(new StringContent(Album), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.album]);
            }

            String responseString = await PostAnonymousImgurDataAsync(ImgurEndpoints.ImageUpload(), content);
            ImgurBasicWithImage returnedImage = await Task.Run(() => JsonConvert.DeserializeObject<ImgurBasicWithImage>(responseString, _defaultSerializerSettings));

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
        public async Task<ImgurImage> ImageUploadAsync(String url, String Title = "", String Description = "", String Album = "")
        {
            MultipartFormDataContent content = new MultipartFormDataContent(BoundaryGuid.ToString());
            content.Add(new StringContent(url), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.image]);
            if (Title != "")
            {
                content.Add(new StringContent(Title), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.title]);
            }
            if (Description != "")
            {
                content.Add(new StringContent(Description), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.description]);
            }
            if (Album != "")
            {
                content.Add(new StringContent(Album), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.album]);
            }

            String responseString = await PostAnonymousImgurDataAsync(ImgurEndpoints.ImageUpload(), content);
            ImgurBasicWithImage returnedImage = await Task.Run(() => JsonConvert.DeserializeObject<ImgurBasicWithImage>(responseString, _defaultSerializerSettings));

            return returnedImage.Image;
        }

        /// <summary>
        /// Deletes an image
        /// </summary>
        /// <param name="deleteID">If this image belongs to the account, the ID, if not, the deletehash</param>
        /// <returns>An ImgurBasic response</returns>
        public async Task<ImgurBasic> ImageDeleteAsync(String deleteID)
        {
            String responseString = await DeleteImgurDataAsync(ImgurEndpoints.ImageDeletion(deleteID));
            return await Task.Run(() => JsonConvert.DeserializeObject<ImgurBasic>(responseString, _defaultSerializerSettings));
        }

        /// <summary>
        /// Updates the title or description of an image. You can only update an image you own and is associated with your account. For an anonymous image, {id} must be the image's deletehash.
        /// </summary>
        /// <param name="deleteHashOrImageID">The deletehash or ID of an image (ID ONLY WORKS IF LOGGED IN!)</param>
        /// <param name="Title">The title of the image.</param>
        /// <param name="Description">The description of the image.</param>
        /// <returns></returns>
        public async Task<ImgurBasic> ImageUpdateAsync(String deleteHashOrImageID, String Title = "", String Description = "")
        {
            MultipartFormDataContent content = new MultipartFormDataContent(BoundaryGuid.ToString());
            if (Title != "")
            {
                content.Add(new StringContent(Title), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.title]);
            }
            if (Description != "")
            {
                content.Add(new StringContent(Description), ImgurEndpoints.ImageEndpointParameterLookup[ImgurParameters.description]);
            }
            String responseString = await PostAnonymousImgurDataAsync(ImgurEndpoints.ImageUpdate(deleteHashOrImageID), content);
            ImgurBasic status = await Task.Run(() => JsonConvert.DeserializeObject<ImgurBasic>(responseString, _defaultSerializerSettings));

            return status;
        }
        #endregion

    }
}
