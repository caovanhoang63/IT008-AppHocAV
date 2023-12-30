using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


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
                string result = "";
                List<object> data = JArray.Parse(response).ToObject<List<object>>();
                List<object> data2 = JArray.Parse(data[0].ToString()).ToObject<List<object>>();

                for (int i = 0; i < data2.Count; i++)
                {
                    List<object> data3 = JArray.Parse(data2[i].ToString()).ToObject<List<object>>();
                    result += data3[0].ToString();
                }

                return result;
            }
            catch (HttpRequestException e)
            {
                return null;
            }
        }
    }
}