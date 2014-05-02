using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ImgShare.Data;
using ImgShare.Data.ImgurResponseModels;

namespace ImgurAPIUnitTests
{
    [TestClass]
    public class GetGalleryImageTests
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
            ImgurGalleryImageList list = ImgurApiSource.Instance.GetMainGalleryImagesAsync(MainGallerySection.top, MainGallerySort.viral, 0).Result;
            // Check that some images are in the list
            Assert.IsFalse((list.Images.Count() < 5));
        }

        /// <summary>
        /// Checsk top and hot images on two different pages.
        /// </summary>
        [TestMethod]
        public void GetImagesReturnsImagesOnDifferentPages()
        {
            ImgurGalleryImageList list = ImgurApiSource.Instance.GetMainGalleryImagesAsync(MainGallerySection.top, MainGallerySort.viral, 1).Result;
            // Check that some images are in the list
            Assert.IsFalse((list.Images.Count() < 5));

            ImgurGalleryImageList newlist = ImgurApiSource.Instance.GetMainGalleryImagesAsync(MainGallerySection.hot, MainGallerySort.viral, 4).Result;
            // Check that some images are in the list
            Assert.IsFalse((newlist.Images.Count() < 5));
        }

        /// <summary>
        /// Checks the hot images section to see if we get stuff back.
        /// </summary>
        [TestMethod]
        public void HotImagesReturnsImages()
        {
            ImgurGalleryImageList list = ImgurApiSource.Instance.GetMainGalleryImagesAsync(MainGallerySection.hot, MainGallerySort.time, 0).Result;
            // Check that some images are in the list
            Assert.IsFalse((list.Images.Count() < 5));
        }

        /// <summary>
        /// Fills in image details for a single image returned from the hot images section.
        /// </summary>
        [TestMethod]
        public void ImageDetailsFilledInForImage()
        {
            ImgurGalleryImageList testImageList = ImgurApiSource.Instance.GetMainGalleryImagesAsync(MainGallerySection.hot, MainGallerySort.viral, 0).Result;
            ImgurImage testImage = testImageList.Images.ElementAt(3);

            ImgurImage newtestImage = ImgurApiSource.Instance.GetImageDetailsAsync(testImage.ID).Result;

            Assert.IsNotNull(newtestImage.isAnimated);
            Assert.IsNotNull(newtestImage.Height);
            Assert.IsNotNull(newtestImage.Bandwidth);
        }
    }

    public static class Utilities
    {
        public static String ClientID = "204544071ed584d";
        public static String ClientSecret = "884884ea1098066ca3e20cd7500a6f86a2663c8b";

        public static void InitializeImgurAPISource()
        {
            ImgurApiSource.Instance.ClientID = Utilities.ClientID;
            ImgurApiSource.Instance.ClientSecret = Utilities.ClientSecret;
        }
    }
}
