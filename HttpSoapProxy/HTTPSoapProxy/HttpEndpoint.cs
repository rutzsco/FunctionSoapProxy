using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HTTPSoapProxy
{
    public static class HttpEndpoint
    {
        [FunctionName("HttpEndpoint")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var value1 = req.Query["value1"];
            var value2 = req.Query["value2"];

            var message = MessageBuilder.BuildMessage(Convert.ToInt32(value1), Convert.ToInt32(value2));
            var result = MessageSender.Send(message, "http://www.dneonline.com/calculator.asmx");

            return new OkObjectResult(result);
        }
    }
}
