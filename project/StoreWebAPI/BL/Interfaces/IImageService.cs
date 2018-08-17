using System.Threading.Tasks;

namespace ClothingStore.Service.Interfaces {
    public interface IImageService {
        Task<string> GetImagePathAsync(string image);

        void DeleteImage(string path);

        Task<string> GetBase64StringAsync(string path);
    }
}
