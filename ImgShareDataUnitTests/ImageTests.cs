using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using ImgShare.APISource.Data.ImgurResponseModels;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Net;
using System.Threading;
using ImgShare.APISource.UnitTests.TestBase;
using ImgShare.APISource.Data;

namespace ImgShare.APISource.UnitTests.Image
{
    [TestClass]
    public class ImageTests
    {
        // Picture of cat in cereal is used for testing upload.  We download the file then put it back
        private String TestImage = @"http://i.imgur.com/AsOXOMT.jpg";

        // This is the byte array used to hold the test image
        Byte[] ImageBytes;

        // A list of images that we'll attempt deletion on after we're done
        List<ImgurImage> imagesToDelete;

        [TestInitialize]
        public void StartupTests()
        {
            // Ensure that we have setup the client API keys correctly
            Utilities.InitializeImgurAPISource();

            // Download the test image and store it in the byte array
            WebRequest request = WebRequest.Create(TestImage);
            WebResponse response = request.GetResponseAsync().Result;
            Stream stream = response.GetResponseStream();

            ImageBytes = new Byte[response.ContentLength];
            response.GetResponseStream().Read(ImageBytes, 0, (int)response.ContentLength);

            // Setup our deletion list for later
            imagesToDelete = new List<ImgurImage>();
        }

        /// <summary>
        /// Posts the image by bytearray to Imgur, checks that the deletehash exists, then attempts a 
        /// get details call on the uploaded image to check that the other properties we requested were
        /// filled in correctly.
        /// </summary>
        [TestMethod]
        public void UploadImageWithFile()
        {
            
            ImgurImage test = ImgurApiSource.Instance.ImageUploadAsync(ImageBytes, "Hello", "DescriptionTest").Result;
            
            Assert.IsNotNull(test.Deletehash);
            
            // Queue this particular image for deletion
            imagesToDelete.Add(test);

            test = ImgurApiSource.Instance.ImageDetailsAsync(test.ID).Result;
            Assert.AreEqual("Hello", test.Title);
            Assert.AreEqual("DescriptionTest", test.Description);

        }

        /// <summary>
        /// Posts the image by URL to Imgur, checks that the deletehash exists, then attempts a 
        /// get details call on the uploaded image to check that the other properties we requested were
        /// filled in correctly.
        /// </summary>
        [TestMethod]
        public void UploadImageWithURL()
        {
            ImgurImage test = ImgurApiSource.Instance.ImageUploadAsync(TestImage, "Hello", "DescriptionTest").Result;

            Assert.IsNotNull(test.Deletehash);

            // Queue this particular image for deletion
            imagesToDelete.Add(test);

            test = ImgurApiSource.Instance.ImageDetailsAsync(test.ID).Result;
            Assert.AreEqual("Hello", test.Title);
            Assert.AreEqual("DescriptionTest", test.Description);
        }

        /// <summary>
        /// Uploads an Image to Imgur by ByteArray then attempts a delete on the uploaded Image.
        /// </summary>
        [TestMethod]
        public void DeleteUploadedImageByDeleteHash()
        {
            ImgurImage test = ImgurApiSource.Instance.ImageUploadAsync(ImageBytes, "Hello", "DescriptionTest").Result;

            System.Threading.SpinWait.SpinUntil(() => 0 == 1, TimeSpan.FromSeconds(4));

            Assert.IsNotNull(test.Deletehash);

            ImgurBasic result = ImgurApiSource.Instance.ImageDeleteAsync(test.Deletehash).Result;
            Assert.IsTrue(result.success);
        }

        /// <summary>
        /// Uploads an Image to Imgur by ByteArray then attempts to update the metadata
        /// </summary>
        [TestMethod]
        public void UpdateImageByDeleteHash()
        {
            ImgurImage test = ImgurApiSource.Instance.ImageUploadAsync(ImageBytes, "Hello", "DescriptionTest").Result;

            // pause for 4 seconds while the server updates 
            System.Threading.SpinWait.SpinUntil(() => 0 == 1, TimeSpan.FromSeconds(4));

            Assert.IsNotNull(test.Deletehash);

            ImgurBasic result = ImgurApiSource.Instance.ImageUpdateAsync(test.Deletehash, "NewTitle", "NewDescription").Result;

            // pause for 4 more seconds while the server updates
            System.Threading.SpinWait.SpinUntil(() => 0 == 1, TimeSpan.FromSeconds(4));

            ImgurImage image = ImgurApiSource.Instance.ImageDetailsAsync(test.ID).Result;
            Assert.AreEqual(image.Title, "NewTitle");
            Assert.AreEqual(image.Description, "NewDescription");

        }

        [TestCleanup]
        public void RemoveImages()
        {
            // Cleanup any images we might have uploaded to Imgur during testing
            foreach(ImgurImage i in imagesToDelete)
            {
                ImgurBasic result = ImgurApiSource.Instance.ImageDeleteAsync(i.Deletehash).Result;
                if (!result.success)
                {
                    throw new Exception("Unable to delete uploaded test images");
                }
            }
        }
    }
}
