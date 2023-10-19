using Newtonsoft.Json;
using RestSharp;
using static GatewayApiProject.Models.ImaggaResult;

namespace GatewayApiProject.Models
{
    /// <summary>
    /// Represents the JSON response from the Imagga API as a deserialized object.
    /// </summary>
    public class ImaggaResult
    {
        /// <summary>
        /// A list of tags associated with the analyzed image.
        /// </summary>
        public List<Tag2> tags { get; set; }

        /// <summary>
        /// Represents the root object containing the result and status of the Imagga analysis.
        /// </summary>
        public class RootImageResult
        {
            /// <summary>
            /// The ImaggaResult object containing the analyzed image's tags.
            /// </summary>
            public ImaggaResult result { get; set; }
            /// <summary>
            /// The status of the Imagga analysis.
            /// </summary>
            public Status status { get; set; }
        }
        /// <summary>
        /// Represents the status information of the Imagga analysis.
        /// </summary>
        public class Status
        {
            /// <summary>
            /// The textual status information.
            /// </summary>
            public string text { get; set; }
            /// <summary>
            /// The type of status (e.g., "success" or other values).
            /// </summary>
            public string type { get; set; }
        }

        /// <summary>
        /// Represents a tag associated with the analyzed image, including confidence level and tag information.
        /// </summary>
        public class Tag2
        {
            /// <summary>
            /// The confidence level associated with the tag.
            /// </summary>
            public double confidence { get; set; }
            /// <summary>
            /// The tag (classification) information.
            /// </summary>
            public Tag tag { get; set; }
        }
        /// <summary>
        /// Represents a tag (classification), including its text in English.
        /// </summary>
        public class Tag
        {
            /// <summary>
            /// The English text of the tag- the classification.
            /// </summary>
            public string en { get; set; }
        }



        // This examples is using RestSharp as a REST client - http://restsharp.org
        /// <summary>
        /// this function gets an image url, and returns Immaga response for that image as deserialized object
        /// </summary>
        /// <param name="imageUrl">image url to send to Immaga for check</param>
        /// <returns>Immaga (Json) response as deserialized object</returns>
        public RootImageResult  CheckImage(string imageUrl)
            {
                string apiKey = "acc_400d45350deded3";
                string apiSecret = "504cbaae38a9e6fc064acc80abd37e3d";

                string basicAuthValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(String.Format("{0}:{1}", apiKey, apiSecret)));

                var client = new RestClient("https://api.imagga.com/v2/tags");

            var request = new RestRequest();
                request.AddParameter("image_url", imageUrl);
                request.AddHeader("Authorization", String.Format("Basic {0}", basicAuthValue));

            RestResponse response = client.Execute(request);
            RootImageResult DeserializedRootImaggaResult = JsonConvert.DeserializeObject<RootImageResult>(response.Content);

            return DeserializedRootImaggaResult;
            }
        }

    
   


}