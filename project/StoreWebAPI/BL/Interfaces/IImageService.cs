using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.Interfaces
{
    public interface IImageService {
        Task<string> GetImagePathAsync(string image);

        void DeleteImage(string path);
        Task<string> GetBase64StringAsync(string path);
    } 
}
