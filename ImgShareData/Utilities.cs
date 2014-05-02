using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.Data
{
    static class Utilities
    {
        public static EnumType converToEnum<EnumType>(this String enumValue)
        {
            return (EnumType)Enum.Parse(typeof(EnumType), enumValue);
        }

        public static String convertToString(this Enum eff)
        {
            return Enum.GetName(eff.GetType(), eff);
        }

    }
}
