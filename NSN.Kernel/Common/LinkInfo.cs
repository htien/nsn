using System;

namespace NSN.Common
{
    [Serializable]
    public class LinkInfo
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] ImageUrls { get; set; }

        public LinkInfo() { }
    }
}
