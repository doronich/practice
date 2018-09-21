using System.Threading.Tasks;

namespace ClothingStore.Service.Interfaces {
    public interface IImageService {
        Task<string> GetImagePathAsync(string image);

        void DeleteImage(string path);

        Task<string> GetBase64StringAsync(string path);

        string GetBase64String(string path);

        Task<string> GetMinImagePathAsync(string path);
    }
}
