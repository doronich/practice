using System;
using System.IO;
using System.Threading.Tasks;
using ClothingStore.Service.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


namespace ClothingStore.Service.Services {
    public class ImageService : IImageService {
        private const string PATH_S = "../ImageStore/";
        public async Task<string> GetImagePathAsync(string image) {
            var type = GetTypeOfImage(image);
            
            var base64Str = image.Substring(image.IndexOf(',') + 1);
            var bytes = Convert.FromBase64String(base64Str);
            var name = PATH_S + DateTime.UtcNow.ToString("yyyyMMddhhmmss") +"R" + new Random().Next(1000)+ "." + type;

            await File.WriteAllBytesAsync(name, bytes);

            return name;
        }
        public async Task<string> GetMinImagePathAsync(string path) {
            return await Task.Run(() => {
                var type = GetType(path);
                string fileName;
                const string prefix = "mini";
                using(var inputStream = File.OpenRead(path)) {
                    using(var image = Image.Load(inputStream)) {
                        var w = Convert.ToInt32(image.Width / 2.5);
                        var h = Convert.ToInt32(image.Height / 2.5);
                        image.Mutate(x => x.Resize(w, h));
                        fileName = PATH_S + prefix + DateTime.UtcNow.ToString("yyyyMMddhhmmss") + "R" + new Random().Next(1000) + "." + type;
                        image.Save(fileName);
                    }
                }

                return fileName;
            });

        }

        public string GetBase64String(string path) {

            var type = GetType(path);
            var bytes = File.ReadAllBytes(path);
            return "data:image/" + type + ";base64," + Convert.ToBase64String(bytes);
        }

        public async Task<string> GetBase64StringAsync(string path) {

            var bytes = await File.ReadAllBytesAsync(path);

            return "data:image/" + GetType(path) + ";base64," + Convert.ToBase64String(bytes);
        }

        public void DeleteImage(string path) {            
            if(path != PATH_S+"default.png" && !string.IsNullOrEmpty(path)) File.Delete(path);
        }

        private static string GetTypeOfImage(string image) {
            var st = image.IndexOf("/", StringComparison.Ordinal);
            var res = image.Substring(st + 1);

            var end = res.IndexOf(';');
            res = res.Remove(end);
            if(res != "png" && res != "jpeg" && res != "gif") throw new Exception("Incorrect image format.");
            return res;
        }

        private static string GetType(string path)
        {
            var type = string.Empty;
            var temp = path.Substring(path.LastIndexOf('.') + 1);
            switch (temp)
            {
                case "jpeg":
                case "jpg":
                    type = "jpeg";
                    break;
                case "png":
                    type = "png";
                    break;
                case "gif":
                    type = "gif";
                    break;
            }

            return type;
        }
    }
}
