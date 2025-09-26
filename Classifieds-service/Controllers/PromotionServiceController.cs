using Classifieds_service.Data;
using Classifieds_service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Classifieds_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdServiceControlller : ControllerBase
    {
        private readonly PromotionData data;


        public AdServiceControlller(PromotionData data)
        {
            this.data = data;
        }


        [HttpPut("/locations/upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {


                var parseTxt = new ParseTxtFile(file);

                await parseTxt.Update(data.pathsLocations);


                return Ok("Файл успешно загружен");

            }
            catch(ParseExeption ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationExeption ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Возникла неизвестная ошибка");
            }

        }


        [HttpGet("/locations")]
        public IActionResult GetLocations([FromQuery] string region)
        {
            try
            {

                List<string> result = new();

                PromPathValidator.CheckPath(region);

                string[] index = [.. region.Split("/", StringSplitOptions.RemoveEmptyEntries).Select(e => "/" + e)];

                string value = string.Empty;
          
                for (int i = 0; i < index.Length; i++)
                {

                    value += index[i];
                    if (data.pathsLocations.ContainsKey(value))
                    {

                        result.AddRange(data.pathsLocations[value]);
                    }
                }
                if(result.Count == 0) return NotFound("Путь не найден");


                return Ok(new {  promotion_playgrounds = result });
            }
            catch (ValidationExeption ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {

                return BadRequest("Возникла неизвестная ошибка");
            }
        }


    }
}
