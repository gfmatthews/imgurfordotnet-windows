using ImgShare.APISource.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.APISource.UnitTests.TestBase
{
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
