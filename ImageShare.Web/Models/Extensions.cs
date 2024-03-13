using ImageShare.Web.Controllers;
using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ImageShare.Web.Models
{
    public static class Extensions 
    {
        public static string Upload(this IFormFile source, string folderName)
        {
            var fileName = $"{Guid.NewGuid()} - {source.FileName}";
            using var fileStream = new FileStream(Path.Combine(Path.GetFullPath("wwwroot"), folderName, fileName), FileMode.Create);
            source.CopyTo(fileStream);

            return fileName;
        }
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default :
                JsonSerializer.Deserialize<T>(value);
        }
    }
}
