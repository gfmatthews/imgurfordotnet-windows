using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ImgShare.Data;
using ImgShare.Data.ImgurResponseModels;
using ImgurAPIUnitTests.TestBase;

namespace ImgurAPIUnitTests.GalleryEndpoints
{
    [TestClass]
    public class GalleryEndpointTests
    {
        [TestInitialize]
        public void StartupTests()
        {
            Utilities.InitializeImgurAPISource();
        }

        /// <summary>
        /// Checks the top images by viral sorting on the main page.  Looks to see that we got some reasonable number of images back.
        /// </summary>
        [TestMethod]
        public void TopImagesReturnsImages()
        {
            ImgurGalleryImageList list = ImgurApiSource.Instance.GalleryDetails(GallerySection.top, GallerySort.viral, 0).Result;
            // Check that some images are in the list
            Assert.IsFalse((list.Images.Count() < 5));
        }

        /// <summary>
        /// Checsk top and hot images on two different pages.
        /// </summary>
        [TestMethod]
        public void GetImagesReturnsImagesOnDifferentPages()
        {
            ImgurGalleryImageList list = ImgurApiSource.Instance.GalleryDetails(GallerySection.top, GallerySort.viral, 1).Result;
            // Check that some images are in the list
            Assert.IsFalse((list.Images.Count() < 5));

            ImgurGalleryImageList newlist = ImgurApiSource.Instance.GalleryDetails(GallerySection.hot, GallerySort.viral, 4).Result;
            // Check that some images are in the list
            Assert.IsFalse((newlist.Images.Count() < 5));
        }

        /// <summary>
        /// Checks the hot images section to see if we get stuff back.
        /// </summary>
        [TestMethod]
        public void HotImagesReturnsImages()
        {
            ImgurGalleryImageList list = ImgurApiSource.Instance.GalleryDetails(GallerySection.hot, GallerySort.time, 0).Result;
            // Check that some images are in the list
            Assert.IsFalse((list.Images.Count() < 5));
        }

        /// <summary>
        /// Check that the search actually returns a few images if you look for cats
        /// </summary>
        [TestMethod]
        public void GallerySearchReturnsImages()
        {
            ImgurGalleryImageList list = ImgurApiSource.Instance.GallerySearch(GallerySort.time, GallerySearchWindow.all, 0, "cats").Result;

            // Check that some images are in the list
            Assert.IsFalse((list.Images.Count() < 5));
        }
    }


}
