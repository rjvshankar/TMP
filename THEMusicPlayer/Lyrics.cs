using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Net;
using Windows.Web.Http;
using HtmlAgilityPack;

namespace THEMusicPlayer
{
    public class Lyrics
    {
        public static async Task<string> Get(string artistName, string songName)
        {
            var httpClient = new HttpClient();
            var uriBuilder = new UriBuilder("http", "lyrics.wikia.com");
            var queryString = "artist=" + artistName + "&song=" + songName + "&fmt=xml";

            if (uriBuilder.Query != null && uriBuilder.Query.Length > 1)
            {
                uriBuilder.Query = uriBuilder.Query.Substring(1) + "&" + queryString;
            }
            else
            {
                uriBuilder.Query = queryString;
            }
            uriBuilder.Path = "/api.php";
            string xmlResponse = "";
            try
            {
                var response = await httpClient.GetAsync(uriBuilder.Uri);
                response.EnsureSuccessStatusCode();
                xmlResponse = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return "Error retrieving lyrics.";
            }
            
            string url;
            var settings = new XmlReaderSettings();
            settings.Async = true;
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlResponse), settings))
            {
                reader.ReadToFollowing("lyrics");
                var lyrics = await reader.ReadElementContentAsStringAsync();
                if (lyrics.Trim() == "Not found")
                {
                    return "Couldn't find lyrics for this track.";
                }

                reader.ReadToFollowing("url");
                url = await reader.ReadElementContentAsStringAsync();
            }

            string htmlResponse;
            try
            {
                var response = await httpClient.GetAsync(new Uri(url));
                //For some reason, lyrics.wikia.com occasionally returns a 404 along with
                //valid HTML (see the Thunderstruck unit test). Stupid website.
                htmlResponse = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
                return "Error retrieving lyrics.";
            }

            return LyricsFromHtml(htmlResponse);
        }

        private static string LyricsFromHtml(string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var lyricsNodes = document.DocumentNode.Descendants().Where(
                x => (x.Name == "div"
                && x.Attributes["class"] != null
                && x.Attributes["class"].Value == "lyricbox")).ToList();

            if (lyricsNodes.Count == 0)
            {
                return "Error retrieving lyrics.";
            }

            var node = lyricsNodes[0];
            var encodedLyrics = node.InnerHtml;

            //Remove whitespace and replace <br> with newlines.
            encodedLyrics = Regex.Replace(encodedLyrics, @"\s+", "");
            encodedLyrics = encodedLyrics.Replace("<br>", Environment.NewLine);

            //Remove the javascript and the divs.
            encodedLyrics = Regex.Replace(encodedLyrics, @"<script>.*?<\/script>", "", RegexOptions.Singleline);
            encodedLyrics = Regex.Replace(encodedLyrics, @"<div.*?>.*?<\/div>", "", RegexOptions.Singleline);
            //Then get rid of the comments.
            encodedLyrics = Regex.Replace(encodedLyrics, @"<!--(.|\s)*-->", "");
            //Decode the lyrics.
            var lyrics = WebUtility.HtmlDecode(encodedLyrics);

            return lyrics.Trim(); 
        }
    }
}
