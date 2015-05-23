using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.APISource.Data
{
    public enum ImgurParameters
    {
        image,
        title,
        description,
        album,
        ids,
        privacy,
        layout,
        cover
    };


    public enum GallerySection
    {
        hot,
        top,
        user
    };

    public enum GallerySort
    {
        viral,
        time
    };

    public enum ImgurEndpointUseType
    {
        free,
        paid
    };

    public enum GallerySearchWindow
    {
        day,
        week,
        month,
        year,
        all
    };

    /// <summary>
    /// The real imgur API calls the most open level of albums "public" but that clearly wouldn't work here since its a reserved name so it
    /// is called "open" here instead.
    /// </summary>
    public enum Privacy
    {
        open,
        hidden,
        secret,
        ignore
    };

    public enum Layout
    {
        blog,
        grid,
        horizontal,
        vertical,
        ignore
    };
}
