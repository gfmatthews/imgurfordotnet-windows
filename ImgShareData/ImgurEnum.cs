using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShare.APISource.Data
{
    /// <summary>
    /// Global parameters used in multiple Imgur calls
    /// </summary>
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

    /// <summary>
    /// Gallery section parameters
    /// </summary>
    public enum GallerySection
    {
        hot,
        top,
        user
    };

    /// <summary>
    /// Gallery sort parameters
    /// </summary>
    public enum GallerySort
    {
        viral,
        time
    };

    /// <summary>
    /// Used by the ImgurApiSource object to denote free or paid endpoint types
    /// </summary>
    public enum ImgurEndpointUseType
    {
        free,
        paid
    };

    /// <summary>
    /// Gallery search parameters for controlling how far back to search
    /// </summary>
    public enum GallerySearchWindow
    {
        day,
        week,
        month,
        year,
        all
    };

    /// <summary>
    /// Privacy parameter used to control privacy settings on album and image posting
    /// </summary>
    /// <remarks>
    /// The real imgur API calls the most open level of albums "public" but that clearly wouldn't work here since its a reserved name so it
    /// is called "open" here instead.
    /// </remarks>
    public enum Privacy
    {
        open,
        hidden,
        secret,
        ignore
    };

    /// <summary>
    /// Layout parameters used in album creation
    /// </summary>
    public enum Layout
    {
        blog,
        grid,
        horizontal,
        vertical,
        ignore
    };
}
