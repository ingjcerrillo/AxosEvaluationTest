using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using com.ToysAndGames.Server.WebAPI.DAL.Models;
using com.ToysAndGames.Server.WebAPI.DAL.Repositories.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace com.ToysAndGames.Server.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericRepository<Product> genericRepository;
        private readonly IUnitOfWork unitOfWork;
        public ProductController(IGenericRepository<Product> genericRepository, IUnitOfWork unitOfWork)
        {
            this.genericRepository = genericRepository;
            this.unitOfWork = unitOfWork;
        }

        // GET: /Api/Product/GetAll/
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await genericRepository.GetAllAsync();
            return Ok(
                result
                );
        }

        // GET: /Api/Product/GetById/
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetByIdProduct(int id)
        {
            if(id < 0)
            {
                BadRequest();
            }

            var result = await genericRepository.GetByIdAsync(id);

            if(result == null)
            {
                return NotFound();
            }

            return Ok(
                result
                );
        }

        // POST: /Api/Product/Create
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> NewProduct([FromBody]Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await genericRepository.CreateAsync(product);
            unitOfWork.Commit();

            return Ok(true);
        }

        // POST: /Api/Product/Update
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> UpdateProduct([FromBody]Product product)
        {
            if (product.Id <= 0)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notFound = !await genericRepository.UpdateAsync(product.Id, product);
            unitOfWork.Commit();

            if (notFound)
            {
                return NotFound();
            }

            return Ok(true);
        }

        // POST: /Api/Product/Delete
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if(id <= 0)
            {
                return BadRequest();
            }

            var notFound = !await genericRepository.DeleteAsync(id);
            unitOfWork.Commit();

            if (notFound)
            {
                return NotFound();
            }

            return Ok(true);
        }
    }
}