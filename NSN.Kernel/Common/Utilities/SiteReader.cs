using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace NSN.Common.Utilities
{
    public class SiteReader
    {
        public SiteInfo SiteInfo { get; private set; }

        public SiteReader(string url)
        {
            if (String.IsNullOrWhiteSpace(url))
            {
                throw new Exception("Invalid url.");
            }
            this.SiteInfo = new SiteInfo();
            this.SiteInfo.Content = GetContent(url);
            TryParseMeta(SiteInfo.Content, SiteInfo);
            this.SiteInfo.ImageUrls = ParseImages(SiteInfo.Content);
        }

        private string GetContent(string url)
        {
            HttpWebRequest request;
            HttpWebResponse response = null;
            StreamReader reader = null;
            StringBuilder sbSource = null;

            try
            {
                if (!Regex.IsMatch(url, @"^([^<]*://)"))
                {
                    url = "http://" + url;
                }
                Uri uri = new Uri(url);
                request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.KeepAlive = false;
                request.Timeout = 10 * 1000;

                this.SiteInfo.Address = request.Address;
                response = (HttpWebResponse)request.GetResponse();

                if (request.HaveResponse && response != null)
                {
                    reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                    sbSource = new StringBuilder(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                throw new Exception("Cannot get content from this url.", e);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (response != null)
                    response.Close();
            }
            return sbSource.ToString();
        }

        private bool TryParseMeta(string strIn, SiteInfo siteInfo)
        {
            try
            {
                // Parse the title
                Match titleMatch = Regex.Match(strIn, @"<title>([^<]*)</title>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                siteInfo.MetaTitle = titleMatch.Groups[1].Value;

                // Parse the meta keywords
                Match keywordMatch = Regex.Match(strIn, @"<meta\s*name=""keywords""\s*content=""([^<]*)\""\s*/?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                siteInfo.MetaKeywords = keywordMatch.Groups[1].Value;

                // Parse the meta description
                Match descriptionMatch = Regex.Match(strIn, @"<meta\s*name=""description""\s*content=""([^<]*)""\s*/?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                siteInfo.MetaDescription = descriptionMatch.Groups[1].Value;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string[] ParseImages(string strIn)
        {
            MatchCollection imagesMatch = Regex.Matches(strIn, @"<img\s+[^<]*src=""([^<]+(.jpe?g|.gif|.png))""[^<]*/?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string[] images = new string[imagesMatch.Count];

            for (int i = 0; i < imagesMatch.Count; i++)
            {
                Match imageMatch = imagesMatch[i];
                images[i] = imageMatch.Groups[1].Value;
            }
            return images;
        }
    }
}
