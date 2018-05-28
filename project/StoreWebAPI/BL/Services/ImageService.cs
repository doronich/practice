using System;
using System.IO;
using System.Threading.Tasks;
using BL.Interfaces;

namespace BL.Services {
    public class ImageService : IImageService {
        public async Task<string> GetImagePathAsync(string image) {
            var type = this.GetTypeOfImage(image);

            var base64Str = image.Substring(image.IndexOf(',') + 1);
            var bytes = Convert.FromBase64String(base64Str);
            var name = "../../store/" + DateTime.Now.ToString("yyyyMMddhhmmss") + type;

            await File.WriteAllBytesAsync(name, bytes);
            return name;
        }

        private string GetTypeOfImage(string image) {
            var st = image.IndexOf("/", StringComparison.Ordinal);
            var res = image.Substring(st + 1);

            var end = res.IndexOf(';');
            res = res.Remove(end);

            return res;
        }
    }
}
