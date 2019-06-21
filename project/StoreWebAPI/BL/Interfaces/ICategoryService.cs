using System.Collections.Generic;
using System.Threading.Tasks;
using ClothingStore.Service.Models.Categories;

namespace ClothingStore.Service.Interfaces
{
    public interface ICategoryService {
        Task CreateCategoryAsync(CreateCategoryDTO category);
        Task CreateSubCategoryAsync(CreateSubCategoryDTO category);
        Task RemoveCategoryAsync(long id);
        Task RemoveSubCategoryAsync(long id);
        Task<IList<CategoryDTO>> GetCategoriesAsync();
        Task<IList<SubCategoryDTO>> GetSubCategoriesAsync(long catId);

        Task<IList<SubCategoryDTO>> GetAllSubCategoriesAsync();
    }
}
