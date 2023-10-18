using Newtonsoft.Json;
using RestSharp;

namespace GatewayApiProject.Models
{
    public class ImaggaResponse
    {
        // This examples is using RestSharp as a REST client - http://restsharp.org

            public Root  CheckImage(string imageUrl)
            {
                string apiKey = "acc_400d45350deded3";
                string apiSecret = "504cbaae38a9e6fc064acc80abd37e3d";

                string basicAuthValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", apiKey, apiSecret)));

                var client = new RestClient("https://api.imagga.com/v2/tags");
            //  client.Timeout = -1;

            var request = new RestRequest();
                request.AddParameter("image_url", imageUrl);
                request.AddHeader("Authorization", String.Format("Basic {0}", basicAuthValue));

            RestResponse response = client.Execute(request);
            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(response.Content);

            //Console.WriteLine(response.Content);
            //Console.ReadLine();
            return myDeserializedClass;
            }
        }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class ImaggaResult
    {
        public List<Tag2> tags{ get; set; }
    }

    public class Root
    {
        public ImaggaResult result { get; set; }
        public Status status { get; set; }
    }

    public class Status
    {
        public string text { get; set; }
        public string type { get; set; }
    }

    public class Tag2
    {
        public double confidence { get; set; }
        public Tag tag { get; set; }
    }

    public class Tag
    {
        public string en { get; set; }
    }


}