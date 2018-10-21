
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Seez
{
  public static class ParseReceipt
  {
    [FunctionName("ParseReceipt")]
    public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req, TraceWriter log)
    {

      String apiKey = Environment.GetEnvironmentVariable("APIKey");
      String apiRoot = Environment.GetEnvironmentVariable("APIRoot");
      try
      {
        var form = await req.ReadFormAsync();
        var pictureFile = form.Files.GetFile("receipt");
        var stream = pictureFile.OpenReadStream();
        ComputerVisionClient computerVision = new ComputerVisionClient(
                 new ApiKeyServiceClientCredentials(apiKey),
                 new System.Net.Http.DelegatingHandler[] { });
        computerVision.Endpoint = apiRoot;
        
        OcrResult result = await computerVision.RecognizePrintedTextInStreamAsync(true, stream, OcrLanguages.En);

        log.Info("C# HTTP trigger function processed a request.");
        var list = ReceiptEngine.GroupIntoRows(result);
        return (ActionResult)new OkObjectResult(Newtonsoft.Json.JsonConvert.SerializeObject(new object[] { list, result.Regions }));
      }
      catch (Exception e)
      {
        log.Error("Could not Process", e);
        return new BadRequestObjectResult($"Could not process result: {e.Message}");
      }
    }
  }
}
