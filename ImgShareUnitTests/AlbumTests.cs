using ImgShare.APISource.Data;
using ImgShare.APISource.Data.ImgurResponseModels;
using ImgShare.APISource.UnitTests.TestBase;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ImgShare.APISource.UnitTests.Album
{
    [TestClass]
    public class AlbumEndpointTests
    {
        // KNOWN ALBUM that works
        string testAlbum = "WET8E";

        [TestInitialize]
        public void StartupTests()
        {
            Utilities.InitializeImgurAPISource();
        }

        [TestCategory("album"), TestMethod]
        public void GetAlbumDetailsTest()
        {
            ImgurAlbum album = ImgurApiSource.Instance.AlbumDetailsAsync(testAlbum.ToString()).Result;

            // TODO: Fill in better test details
            Assert.IsNotNull(album);
        }

        [TestCategory("album"), TestMethod]
        public void GetAlbumImagesTest()
        {
            List<ImgurImage> listOfImages = new List<ImgurImage>(ImgurApiSource.Instance.AlbumImagesAsync(testAlbum).Result);
            Assert.AreNotEqual(0, listOfImages.Count);
        }

        [TestCategory("album"), TestMethod]
        public void AlbumCRUD()
        {
            string albumTitle = "K1tt3ns!";
            string descriptor = "An album created to talk about cats";
            string albumTitleUpdate = "dogsarecool";
            string albumDescUpdate = "album desc update";


            // grab some images (from another album) to use in our test album
            List<ImgurImage> listOfImages = new List<ImgurImage>(ImgurApiSource.Instance.AlbumImagesAsync(testAlbum).Result);

            // create the album
            ImgurAlbum album = ImgurApiSource.Instance.AlbumCreationAsync(listOfImages, listOfImages[0], albumTitle, descriptor, Privacy.ignore, Layout.ignore).Result;

            Assert.AreEqual(album.Title, albumTitle);
            Assert.AreEqual(album.Description, descriptor);
            Assert.AreEqual(album.ImagesInAlbum, listOfImages.Count);


            // update information
            List<string> stringList = new List<string>();
            ImgurBasic responseBasic = ImgurApiSource.Instance.AlbumUpdateAsync(album.deletehash, stringList, null, albumTitleUpdate, albumDescUpdate, Privacy.hidden, Layout.blog).Result;
            Assert.IsTrue(responseBasic.success);

            // destroy
            ImgurBasic deleteResponse = ImgurApiSource.Instance.AlbumDeletionAsync(album.deletehash).Result;

            Assert.IsTrue(deleteResponse.success);

        }
    }
}
