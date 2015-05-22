﻿using ImgShare.APISource.Data;
using ImgShare.APISource.Data.ImgurResponseModels;
using ImgShare.APISource.UnitTests.TestBase;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.APISource.UnitTests.Album
{
    [TestClass]
    public class AlbumEndpointTests
    {
        [TestInitialize]
        public void StartupTests()
        {
            Utilities.InitializeImgurAPISource();
        }

        [TestMethod]
        public void GetAlbumDetailsTest()
        {
            // KNOWN ALBUM that works
            string testAlbum = "WET8E";

            ImgurAlbum album = ImgurApiSource.Instance.AlbumDetailsAsync(testAlbum.ToString()).Result;

            // TODO: Fill in better test details
            Assert.IsNotNull(album);
        }
    }
}
