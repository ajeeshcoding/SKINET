
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase 
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productbrandRepo;
        private readonly IGenericRepository<ProductType> _producttypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo, 
            IGenericRepository<ProductBrand> productbrandRepo,IGenericRepository<ProductType> producttypeRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _productbrandRepo = productbrandRepo;
            _producttypeRepo = producttypeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec =new ProductsWithTypesAndBrandsSpecification();

            var Products = await _productRepo.ListAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDto>>(Products));
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int Id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(Id);
            var product= await _productRepo.GetEntityWithSpec(spec);
            return _mapper.Map<Product, ProductToReturnDto>(product);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrand()
        {
            return Ok(await _productbrandRepo.ListAllAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductType()
        {
            return Ok(await _producttypeRepo.ListAllAsync());
        }
    }
}
