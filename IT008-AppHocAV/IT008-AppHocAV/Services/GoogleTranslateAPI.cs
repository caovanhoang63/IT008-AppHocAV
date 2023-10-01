using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace IT008_AppHocAV.Services
{
    public class GoogleTranslateApi
    {
        public static async Task<string> GoogleTranslate(string sl, string tl, string text)
        {
            try
            {
                string url = String.Format
                ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
                    sl, tl, Uri.EscapeUriString(text));
                HttpClient client = new HttpClient();
                var response = await client.GetStringAsync(url);
                int first = response.IndexOf('"');
                int second = response.Substring(first + 1).IndexOf('"');
                string result = response.Substring(first + 1, second);
                return result;
            }
            catch (HttpRequestException e)
            {
                return null;
            }
        }
    }
}