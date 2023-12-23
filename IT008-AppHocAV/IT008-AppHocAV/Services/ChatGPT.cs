using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IT008_AppHocAV.Models.GPT;
using System.Text.Json;
using System.Windows.Controls.Primitives;

namespace IT008_AppHocAV.Services
{
    public class ChatGpt
    {
        private const string ApiUrl = "https://api.openai.com/v1/chat/completions";
        
        private static string _apiKey = Properties.Settings.Default.ChatGptApiKey;

  
        
        
        private static async Task<string> Request(GptRequest request)
        {
            string requestData = JsonSerializer.Serialize(request);
            StringContent content = new StringContent(requestData, Encoding.UTF8, "application/json");

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(ApiUrl, content);
                
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                    GptResponse response = JsonSerializer.Deserialize<GptResponse>(responseString);
                    return response.Choices[0].Message.Content;
                }
                return $"Error: {httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}";
            }
        }

        
        public static Task<string> WritingHelp(Func func,string topic,string answer)
        {
            string userMessage;
            switch (func)
            {
                case Func.Ideas:
                    userMessage = "Suggest some ideas to write an essay about this topic: " + topic;
                    break;
                case Func.OutLine:
                    userMessage = "Suggest an outline for this topic: " + topic;
                    break;
                case Func.Lexical:
                    userMessage = "suggest some lexical items to write an essay about this topic: " + topic;
                    break;
                case Func.Sample:
                    userMessage = "Write an academic ielst essay (250 - 350 words long) about the following topic: "+ topic;
                    break;
                case Func.Enhance:
                    userMessage =  "Thank, here are some regulations for our conversations on scoring IELTS writing test:" +
                                   "\n I'll provide the information for the IELTS writing, including writing task, topic [topics],, and the writing answer [answer];" +
                                   "\n You act as IELTS examiner to score the writing task. " +
                                   "And then give the feedbacks and suggestions to improve the writing for the following points:" +
                                   "\n+) Task achievement;" +
                                   "\n+) Vocabulary and collocations;" +
                                   "\n+) Grammar and sentence structure;" +
                                   "\n+) Coherence and Cohesion;" +
                                   "\nLet start with the following variable values:" +
                                   "\n[exam] = IELTS;" +
                                   "\n[topics] =   " + topic + ";" +
                                   "\n[answer] = "+ answer + ";\n";
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(func), func, null);
            }
            Console.WriteLine(topic);
            
            GptRequest request = new GptRequest();
            
            request.Messages = new List<RequestMessage>();
            
            request.Messages.Add(new RequestMessage()
            {
                Role = "system",
                Content = "You are a helpful assistant."
            });
            
            
            request.Messages.Add(new RequestMessage()
            {
                Role =  "user",
                Content = userMessage,
            });
            
            
            
            string result = Task.Run(async() => await Request(request)).Result;
            
            
            return Task.FromResult(result);
        }
        
        
        
    }

    public enum Func
    {
        Ideas,
        OutLine,
        Lexical,
        Sample,
        Enhance,
    }
    
}