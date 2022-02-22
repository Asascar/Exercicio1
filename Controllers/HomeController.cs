using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Exercicio1.Services;
using Exercicio1.Interfaces;
using Microsoft.AspNetCore.Http;
using Exercicio1.Models;

namespace Exercicio1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        IProgamService progamService = new ProgamService();

        [HttpPost]
        [Route("ReceberArquivo")]
        public async Task<ActionResult> ReceberArquivo(IFormFile file)
        {


            if (file != null)
            {

                var filePath = Path.GetTempFileName();
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                var model = new MediasModel
                {
                    Medias = await progamService.ReadCSV(filePath)
                };


                try
                {

                    string medias = JsonConvert.SerializeObject(model);


                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://zeit-endpoint.brmaeji.now.sh/api/avg");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(medias);
                    }

                    var retorno = (HttpWebResponse)httpWebRequest.GetResponse();
                    using StreamReader streamReader = new StreamReader(retorno.GetResponseStream());

                    return Ok(streamReader.ReadToEnd());

                }
                catch (WebException wex)
                {
                    using StreamReader streamReader = new StreamReader(wex.Response.GetResponseStream());
                    string jretorno = streamReader.ReadToEnd();
                    string retorno = JsonConvert.DeserializeObject<JToken>(jretorno).ToString();

                    return BadRequest(retorno);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Arquivo não informado");
        }
    }
}
