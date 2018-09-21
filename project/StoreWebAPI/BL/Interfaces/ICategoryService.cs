using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ClothingStore.Data.Entities.Categories;
using ClothingStore.Service.Models.Categories;

namespace ClothingStore.Service.Interfaces
{
    public interface ICategoryService {
        Task CreateCategoryAsync(CreateCategoryDTO category);
        Task CreateSubCategoryAsync(CreateSubCategoryDTO category);
        Task RemoveCategoryAsync(long id);
        Task RemoveSubCategoryAsync(long id);
        Task<IList<Category>> GetCategoriesAsync();
        Task<IList<SubCategory>> GetSubCategoriesAsync(long catId);
    }
}
