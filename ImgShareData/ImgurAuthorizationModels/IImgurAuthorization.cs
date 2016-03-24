using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.APISource.Data.ImgurAuthorization
{
    public interface IImgurAuthorization
    {
        String AccessToken
        {
            get;
        }

        String RefreshToken
        {
            get;
        }

        String AuthorizationToken
        {
            get;
        }

        String AuthorizationStateParameter
        {
            get;
            set;
        }

        ImgurAuthorizationGrantType GrantType
        {
            get;
            set;
        }

    }

    public enum ImgurAuthorizationGrantType
    {
        PIN,
        Token,
        Code
    };
}
