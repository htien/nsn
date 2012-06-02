using System;

namespace NSN.Common.Utilities
{
    public class SiteInfo
    {
        public Uri Address { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public string Content { get; set; }

        public string[] ImageUrls { get; set; }
    }
}
