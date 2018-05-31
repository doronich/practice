﻿using System;
using System.IO;
using System.Threading.Tasks;
using BL.Interfaces;

namespace BL.Services {
    public class ImageService : IImageService {
        public async Task<string> GetImagePathAsync(string image) {
            var type = this.GetTypeOfImage(image);

            var base64Str = image.Substring(image.IndexOf(',') + 1);
            var bytes = Convert.FromBase64String(base64Str);
            var name = "../ImageStore/" + DateTime.Now.ToString("yyyyMMddhhmmss") +"."+ type;

            await File.WriteAllBytesAsync(name, bytes);

            return name;
        }

        public async Task<string> GetBase64StringAsync(string path) {
            string type=String.Empty;
            string temp = path.Substring(path.LastIndexOf('.')+1);
            switch (temp) {
                case "jpeg":
                case "jpg": type = "jpeg";
                    break;
                case "png": type = "png";
                    break;
                case "gif": type = "gif";
                    break;
            }
            var bytes = await File.ReadAllBytesAsync(path);

            return "data:image/"+type+";base64," + Convert.ToBase64String(bytes);

        }

        private string GetTypeOfImage(string image) {
            var st = image.IndexOf("/", StringComparison.Ordinal);
            var res = image.Substring(st + 1);

            var end = res.IndexOf(';');
            res = res.Remove(end);
            if(res != "png" && res != "jpeg" && res != "gif" && res != "pjpeg") throw new Exception("Incorrect image format.");
            return res;
        }

        public void DeleteImage(string path) {
            File.Delete(path);
        }
    }
}
