using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shopbridge_base.ActionFilters;
using Shopbridge_base.Data.Repository;
using Shopbridge_base.Domain.Models;
using Shopbridge_base.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_base.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly ILogger<ProductsController> logger;
        private readonly IRepository repository;
        public ProductsController(IProductService _productService, IRepository _repository)
        {
            this.productService = _productService;
            this.repository = _repository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            try
            {
                return Ok(await repository.Get<Product>());
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            
        }


        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Product>))]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            try
            {
                var IdFound =  await repository.GetProductbyId(id);
                return Ok(IdFound);


            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPut("{id}")]
        [ServiceFilter(typeof(UpdateValidationAttributeClass<Product>))]        
        public async Task<IActionResult> PutProduct(int id, Product _product)
        {                                      
            var result =repository.InsertProduct(id,_product);
            if(result.Result.ToString()=="Success")
                return Ok("Product Updated");

            return NotFound(result);
        }


        [HttpPost]        
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                var addedproduct = repository.AddProduct(product);                              
                if(addedproduct.Result=="Success")
                return Ok("New Product Added");                
                    return NotFound(addedproduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }


        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEntityExistsAttribute<Product>))]
        public async Task<IActionResult> DeleteProduct(int id)
        {            
            try
            {
                    await repository.DeleteProductbyId(id);
                return Ok("Product deleted successfully... ");


            }
            catch(Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<Product>> Login(SignInModel signInModel)
        {
            try
            {

                var result = await repository.Login(signInModel);

                if(result!=null)
                {
                    return Ok(result);
                }

                return Unauthorized();
                //var addedproduct = repository.AddProduct(product);
                //if (addedproduct.Result == "Success")
                //    return Ok("New Product Added");
                //return NotFound(addedproduct);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
