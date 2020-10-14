namespace MachineMaintenanceApp.Data.Common.Helpers
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public class CloudinaryHelper
    {
        public static async Task<string> UploadAsync(Cloudinary cloudinary, IFormFile file)
        {
            byte[] destinationImage;
            string url;

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                destinationImage = memoryStream.ToArray();
            }

            using (var destinationStream = new MemoryStream(destinationImage))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, destinationStream),
                };

                var result = await cloudinary.UploadAsync(uploadParams);
                url = result.Uri.AbsoluteUri.Split(new char[] { '/' }, 7).ToArray().LastOrDefault();
            }

            return url;
        }
    }
}
