using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HTTPSoapProxy
{
    public class MessageSender
    {

        public static string Send(XDocument soapRequest, string apiUrl)
        {
            try
            {
                using (var client = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }) { Timeout = TimeSpan.FromMinutes(2) })
                {
                    var request = new HttpRequestMessage()
                    {
                        RequestUri = new Uri(apiUrl),
                        Method = HttpMethod.Post
                    };
                    
                    var content = soapRequest.ToString();
                    request.Content = new StringContent(content, Encoding.UTF8, "text/xml");

                    request.Headers.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                    request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                    request.Headers.Add("SOAPAction", "http://tempuri.org/Add");

                    HttpResponseMessage response = client.SendAsync(request).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }

                    Task<Stream> streamTask = response.Content.ReadAsStreamAsync();
                    Stream stream = streamTask.Result;
                    var sr = new StreamReader(stream);
                    var soapResponse = XDocument.Load(sr);
                    var text = soapResponse.ToString();
                    var result = soapResponse.Descendants(XName.Get("AddResponse", @"http://tempuri.org/")).FirstOrDefault().Value;

                    return result;
                }
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is TaskCanceledException)
                {
                    throw ex.InnerException;
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
