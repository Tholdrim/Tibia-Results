using System.Text.Json;

namespace TibiaResults.Helpers
{
    internal static class FileHelper
    {
        public static TModel? DeserializeFromFile<TModel>(string fileName)
            where TModel : class
        {
            using var fileStream = GetFileStream(fileName);

            if (fileStream == null)
            {
                return null;
            }

            return JsonSerializer.Deserialize<TModel>(fileStream);
        }

        public static async Task<TModel?> DeserializeFromFileAsync<TModel>(string fileName)
            where TModel : class
        {
            using var fileStream = GetFileStream(fileName);

            if (fileStream == null)
            {
                return null;
            }

            return await JsonSerializer.DeserializeAsync<TModel>(fileStream);
        }

        private static FileStream? GetFileStream(string fileName)
        {
            var fileExists = File.Exists(fileName);

            if (!fileExists)
            {
                return null;
            }

            return File.OpenRead(fileName);
        }
    }
}
