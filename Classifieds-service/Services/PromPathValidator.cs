using System.IO;
using System.Text.RegularExpressions;

namespace Classifieds_service.Services
{
    public class PromPathValidator
    {

        private const string Expression = @"^(?:/[^/]+)+$";

        public static  void CheckPath(string path)
        {
            if(!Regex.IsMatch(path, Expression) || string.IsNullOrEmpty(path)) throw new ValidationExeption($"Путь - {path} не соответствует валидации");
        }

        public static void CheckPaths(List<string> paths)
        {
            foreach (string path in paths)
            {
                if(!Regex.IsMatch(path, Expression)) throw new ValidationExeption($"Путь - {path} не соответствует валидации");
            }
        }
    }


    public class PromHeaderValidator
    {
        private const string Expression = @"^[\w.]+(?:\s+[\w.]+)*$";

        public static void CheckHeader(string header)
        {
            if (!Regex.IsMatch(header, Expression) || string.IsNullOrEmpty(header)) throw new ValidationExeption($"Заголовок - {header} не соответствует валидации");
        }

    }


    public class ValidationExeption : Exception
    {
            public ValidationExeption(string? message) : base(message) { }
    }











}
