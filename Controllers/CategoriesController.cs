﻿using AutoMapper;
using ElectronicsStore.Domain.Models;
using ElectronicsStore.Domain.Services;
using ElectronicsStore.Domain.Services.Communication;
using ElectronicsStore.Extensions;
using ElectronicsStore.Resources;
using ElectronicsStore.Resources.Errors;
using ElectronicsStore.Resources.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Controllers {

    [ApiController, Route("api/v1/[controller]")]
    public class CategoriesController : ControllerBase {

        private readonly ICategoriesService categoriesService;
        private readonly IMapper mapper;

        public CategoriesController(ICategoriesService categoriesService, IMapper mapper) {
            this.categoriesService = categoriesService;
            this.mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllAsync() {
            IEnumerable<Category> categories = await categoriesService.GetAllAsync();
            if (categories != null)
                return Ok(mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResponse>>(categories));
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult> GetByIdAsync([FromQuery] CategoryIdRequest request) {
            Category category = await categoriesService.FindByIdAsync(request.Id);
            if (category != null)
                return Ok(mapper.Map<Category, CategoryResponse>(category));
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromQuery] CategorySaveRequest request) {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse { Error = ModelState.GetErrorMessages(), Status = false });
            Category category = mapper.Map<CategorySaveRequest, Category>(request);
            CategoryStatusResponse response = await categoriesService.SaveCategoryAsync(category);
            if (response.Status)
                return Ok(mapper.Map<Category, CategoryResponse>(response.Resource));
            return BadRequest(new ErrorResponse { Error = response.Message, Status = response.Status });
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync([FromQuery] CategoryUpdateRequest request) {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse { Error = ModelState.GetErrorMessages(), Status = false });
            CategoryStatusResponse response = await categoriesService.UpdateAsync(request);
            if (response.Status)
                return Ok(mapper.Map<Category, CategoryResponse>(response.Resource));
            return BadRequest(new ErrorResponse { Error = response.Message, Status = response.Status });
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync([FromQuery] CategoryIdRequest request) {
            bool status = await categoriesService.DeleteAsync(request.Id);
            if (status)
                return Ok();
            return NotFound();
        }
    }
}