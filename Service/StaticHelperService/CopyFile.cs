using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Service.StaticHelperService
{
    public static class CopyFile
    {

        public static async Task CopyFileLocation(string fileName, string NameFolder, IFormFile formFile, IWebHostEnvironment hosting)
        {
            string uploads = Path.Combine(hosting.WebRootPath, NameFolder);
            string fullPath = Path.Combine(uploads, fileName);

            if (!File.Exists(fullPath))
            {
                await formFile.CopyToAsync(new FileStream(fullPath, FileMode.Create));
            }
        }

        public static async Task<Stream> GetFileStreamAsync(IWebHostEnvironment hosting, string NameFolder, string fileName)
        {
            string uploads = Path.Combine(hosting.WebRootPath, NameFolder);
            string fullPath = Path.Combine(uploads, fileName);

            var stream = new MemoryStream();

            using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                await fileStream.CopyToAsync(stream);
            }

            // Reset the stream position to the beginning
            stream.Position = 0;
            return stream;
        }
    }
}
