using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace textmvc.Models
{
    /// <summary>
    /// 處理發送Request的Handler
    /// </summary>
    public class TextHandler
    {
        private string filepath;
        public TextHandler(string _filepath)
        {
            filepath = _filepath;
        }

        //please replace your subscribe key here
        string key = "{key}";

        //language parameter "unk" means auto detect image text
        string requestParamter = "?language=unk&detectOrientation=true";

        //Computer Vision API ocr uri
        string uriBase = "https://southeastasia.api.cognitive.microsoft.com/vision/v1.0/ocr";

        //send requect to ocr api
        public async Task<TextJson> MakeORCRequest()
        {
            HttpResponseMessage response;
            TextJson jsonData = null;

            HttpClient client = new HttpClient();

            //add subscribe key into header "Ocp-Apim-Subscription-Key"
            client.DefaultRequestHeaders.Add(
                   "Ocp-Apim-Subscription-Key", key);

            //complete uri
            string uri = uriBase + requestParamter;
                        
            byte[] byteData = GetImageAsByteArray(filepath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {               
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");
               
                //send post request
                response = await client.PostAsync(uri, content);
            }

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string data = await response.Content.ReadAsStringAsync();

                //Deserialize to TextJson object
                jsonData = JsonConvert.DeserializeObject<TextJson>(data);
            }

            return jsonData;
        }

        /// <summary>
        /// 將圖片轉成ByteArray
        /// </summary>
        /// <param name="imageFilePath">圖檔路徑</param>
        /// <returns></returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            // Open a read-only file stream for the specified file.
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                // Read the file's contents into a byte array.
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }
    }
}