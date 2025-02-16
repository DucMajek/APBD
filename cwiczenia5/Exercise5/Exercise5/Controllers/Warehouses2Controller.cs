using Exercise5.Models;
using Exercise5.Models.DTOs;
using Exercise5.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Transactions;

namespace Exercise5.Controllers
{
    [Route("/warehouses2")]
    [ApiController]
    public class Warehouses2Controller : ControllerBase
    {

        
        private readonly IProductWarehousesRepository _productWarehousesRepository;

        public Warehouses2Controller(IProductWarehousesRepository productWarehousesRepository)
        {  
            _productWarehousesRepository = productWarehousesRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(RegisterNewProductDTO dto)
        {
           
            _productWarehousesRepository.CreateWithProcedure(dto.IdProduct, dto.IdWarehouse, dto.Amount, dto.CreatedAt);

            var id = await _productWarehousesRepository.LastAdded(dto, dto.IdWarehouse, dto.IdProduct, dto.Amount);

            return Created("", "Record inserted with ID = " + id);
        }
    }
}
