using System;
using System.IO;
using System.Threading.Tasks;
using BL.Interfaces;

namespace BL.Services {
    public class ImageService : IImageService {
        public async Task<string> GetImagePathAsync(string image) {
            //var type = this.GetTypeOfImage(image);

           // var base64Str = image.Substring(image.IndexOf(',') + 1);
            //var bytes = Convert.FromBase64String(image);
            var name = "../ImageStore/" + DateTime.Now.ToString("yyyyMMddhhmmss");// +"."+ type;

            //await File.WriteAllBytesAsync(name, bytes);
            await File.WriteAllTextAsync(name,image);
            return name;
        }

        public async Task<string> GetBase64StringAsync(string path) {
            //var bytes = await File.ReadAllBytesAsync(path);
            var s = await File.ReadAllTextAsync(path);
            return s;
            //var base64string = Convert.ToBase64String(bytes);
            //return base64string;
        }

        private string GetTypeOfImage(string image) {
            var st = image.IndexOf("/", StringComparison.Ordinal);
            var res = image.Substring(st + 1);

            var end = res.IndexOf(';');
            res = res.Remove(end);
            if(res != "png" && res!="jpeg" && res!="gif" && res!="pjpeg") {
                throw new Exception("Incorrect image format.");
            }
            return res;
        }
    }
}
