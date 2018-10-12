using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ClothingStore.Data.Entities.Categories;
using ClothingStore.Repository.Interfaces;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ClothingStore.Service.Services {
    public class CategoryService : ICategoryService {
        private readonly IRepository<Category> m_catRepository;
        private readonly HttpContext m_context;
        private readonly IRepository<SubCategory> m_subRepository;

        public CategoryService(IRepository<SubCategory> subRepository, IRepository<Category> catRepository, IHttpContextAccessor accessor) {
            this.m_subRepository = subRepository;
            this.m_catRepository = catRepository;
            this.m_context = accessor?.HttpContext;
        }

        public async Task CreateCategoryAsync(CreateCategoryDTO category) {
            var createdBy = this.m_context.User.Claims.FirstOrDefault(u => u.Type == "Login")?.Value;

            var cat = new Category {
                Name = category.Name,
                RusName = category.RusName,
                CreatedBy = createdBy ?? "Admin",
                Active = true
            };

            await this.m_catRepository.CreateAsync(cat);
            if(cat.Id <= 0) throw new Exception("Creating error.");
        }

        public async Task CreateSubCategoryAsync(CreateSubCategoryDTO category) {
            var createdBy = this.m_context.User.Claims.FirstOrDefault(u => u.Type == "Login")?.Value;

            var catg =
                (await this.m_catRepository.GetAllAsync(new List<Expression<Func<Category, bool>>> { c => c.Id == category.CategoryId })).FirstOrDefault();
            if(catg == null) throw new Exception("Category not found.");
            var cat = new SubCategory {
                Name = category.Name,
                RusName = category.RusName,
                CreatedBy = createdBy ?? "Admin",
                CategoryId = category.CategoryId,
                Active = true
            };

            await this.m_subRepository.CreateAsync(cat);
            if(cat.Id <= 0) throw new Exception("Creating error.");
        }

        public async Task RemoveCategoryAsync(long id) {
            var cat = await this.m_catRepository.GetByIdAsync(id);

            if(cat ==null) throw new Exception("Category not found.");

            await this.m_catRepository.DeleteAsync(cat);
        }

        public async Task RemoveSubCategoryAsync(long id) {
            var subCat = await this.m_subRepository.GetByIdAsync(id);

            if (subCat == null) throw new Exception("Subcategory not found.");

            await this.m_subRepository.DeleteAsync(subCat);
        }

        public async Task<IList<CategoryDTO>> GetCategoriesAsync() {
            var result = await (await this.m_catRepository.GetAllAsync()).Select(c =>
                new CategoryDTO() {
                    Id = c.Id,
                    Name = c.Name,
                    Active = c.Active,
                    RusName = c.RusName
                }
            ).ToListAsync();
            return result;
        }

        public async Task<IList<SubCategoryDTO>> GetSubCategoriesAsync(long catId) {
            var result = await (await this.m_subRepository.GetAllAsync(new List<Expression<Func<SubCategory, bool>>> { c => c.CategoryId == catId }))
                                .Select(s=>new SubCategoryDTO() {
                                    Id = s.Id,
                                   Name = s.Name,
                                   RusName = s.RusName,
                                   Active = s.Active,
                                   CategoryId = s.CategoryId
                               })
                                .ToListAsync();
            return result;
        }

        public async Task<IList<SubCategoryDTO>> GetAllSubCategoriesAsync() {
            var result = await (await this.m_subRepository.GetAllAsync())
                               .Select(s => new SubCategoryDTO()
                               {
                                   Id = s.Id,
                                   Name = s.Name,
                                   RusName = s.RusName,
                                   Active = s.Active,
                                   CategoryId = s.CategoryId
                               })
                               .ToListAsync();
            return result;
        }
    }
}
