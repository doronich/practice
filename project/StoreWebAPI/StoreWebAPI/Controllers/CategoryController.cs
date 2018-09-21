﻿using System;
using System.Threading.Tasks;
using ClothingStore.Filters;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Models.Categories;
using Microsoft.AspNetCore.Mvc;

namespace ClothingStore.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase {
        private readonly ICategoryService m_categoryService;
        private readonly IItemService m_itemService;

        public CategoryController(IItemService itemService, ICategoryService categoryService) {
            this.m_itemService = itemService;
            this.m_categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories() {
            try {
                var result = await this.m_categoryService.GetCategoriesAsync();
                return this.Ok(result);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [HttpGet("sub/{id}")]
        public async Task<IActionResult> GetSubCategories(long id) {
            try {
                var result = await this.m_categoryService.GetSubCategoriesAsync(id);
                return this.Ok(result);
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO category) {
            try {
                await this.m_categoryService.CreateCategoryAsync(category);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }

        [HttpPost("sub")]
        [ValidateModel]
        public async Task<IActionResult> CreateSubCategory(CreateSubCategoryDTO category) {
            try {
                await this.m_categoryService.CreateSubCategoryAsync(category);
                return this.Ok();
            } catch(Exception exception) {
                return this.BadRequest(exception.Message);
            }
        }
    }
}
