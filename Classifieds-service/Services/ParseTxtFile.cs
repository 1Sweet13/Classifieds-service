using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.Tasks;

namespace Classifieds_service.Services
{


    public class ParseTxtFile
    {
        private readonly string[] extensions = new[] { ".txt" };

        private readonly IFormFile file;


        public ParseTxtFile(IFormFile file)
        {
            if(file is null ) throw new ParseExeption($"Файл не может быть пустым");

            string ext = Path.GetExtension(file.FileName);

            if (!extensions.Contains(ext)) throw new ParseExeption($"Файл {file.FileName} - не соответствует расширению");

            this.file = file;
        }

        public async Task Update(Dictionary<string, List<string>> dictionary)
        {
            var allData = await ReadAllData(file);

            foreach (string data in allData)
            {
                int length = data.Length;

                int index = data.IndexOf(':');

                if (index == -1) throw new ParseExeption("Невалидный формат");

                var VALUE = data.Substring(0, index);

                PromHeaderValidator.CheckHeader(VALUE);


                index++;

                var KEYS = data.Substring(index, length - index).Split(",").Where(e => !string.IsNullOrEmpty(e)).ToList();

                PromPathValidator.CheckPaths(KEYS);

                if(KEYS.Count == 0)
                {
                    throw new ParseExeption($"{VALUE} -  пустые локации");
                }
             
                
                foreach (string key in KEYS)
                {

                    if (dictionary.ContainsKey(key))
                    {
                        if (!dictionary[key].Contains(VALUE)) dictionary[key].Add(VALUE);
                    }

                    else
                    {
                        dictionary.Add(key, new List<string> { VALUE });
                    }
                }
            }
        }


        public async Task<string[]> ReadAllData(IFormFile file)
        {
            using (StreamReader reader = new StreamReader(file.OpenReadStream()))
            {
                var read = await reader.ReadToEndAsync();
                return read.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            }

        }
    }

    public class ParseExeption : Exception
    { 
        public ParseExeption(string message) : base(message)  { }
    }

}
