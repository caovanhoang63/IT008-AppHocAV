using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DictionaryEntry = IT008_AppHocAV.Models.DictionaryEntry;

namespace IT008_AppHocAV.Services
{
    public class DictionaryApi
    {
        public static async Task<List<DictionaryEntry>> SearchDictionary(string query)
        {

            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.dictionaryapi.dev/api/v2/entries/en/{query}";
                try
                {
                    var response = await client.GetStringAsync(url);
                    List<DictionaryEntry> result = JsonSerializer.Deserialize<List<DictionaryEntry>>(response);
                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}