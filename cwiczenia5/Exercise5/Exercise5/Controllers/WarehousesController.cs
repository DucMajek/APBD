using Exercise5.Models;
using Exercise5.Models.DTOs;
using Exercise5.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

namespace Exercise5.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IProductRepository _productsRepository;
        private readonly IWarehousesRepository _warehousesRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IProductWarehousesRepository _productWarehousesRepository;
        public WarehousesController(IProductRepository productsRepository, IWarehousesRepository warehousesRepository, IOrdersRepository ordersRepository, IProductWarehousesRepository productWarehousesRepository)
        {
            _productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
            _warehousesRepository = warehousesRepository ?? throw new ArgumentNullException(nameof(warehousesRepository));
            _ordersRepository = ordersRepository ?? throw new ArgumentNullException(nameof(ordersRepository));
            _productWarehousesRepository = productWarehousesRepository ?? throw new ArgumentNullException(nameof(productWarehousesRepository));
        }

        [HttpPost("/warehouse")]
        //[Route("api/warehouses")]
        public async Task <IActionResult> RegisterNewProduct(RegisterNewProductDTO dto)
        {
            var product = await _productsRepository.ProductIsExist(dto.IdProduct);
            var warehouse = await _warehousesRepository.WarehouseIsExist(dto.IdWarehouse);

            if (dto.Amount <= 0)
            {
                return BadRequest("Amount should be greater than 0");
            }

            if (!product) 
            {
                return NotFound($"IdProduct {dto.IdProduct} does not exist");
            }


            if (!warehouse)
            {
                return NotFound($"IdWarehouse {dto.IdWarehouse} does not exist");
            }

            var order = await _ordersRepository.checkProductInOrder(dto.IdProduct);
            var amount = await _ordersRepository.checkAmountInOrder(dto.Amount);
            var fulfilled = await _ordersRepository.Fulfill(dto.IdProduct, dto.Amount);
            if (!order && !fulfilled && !amount)
            {
                return NotFound($"Product {dto.IdProduct} does not exist in order");
            }


            await _ordersRepository.updateFulFilled(dto.IdProduct);

            await _productWarehousesRepository.AddProducts(dto, dto.IdWarehouse, dto.IdProduct, dto.Amount);

            var id = await _productWarehousesRepository.LastAdded(dto, dto.IdWarehouse, dto.IdProduct, dto.Amount);

            return Ok("An order has been placed " + id);
           
        }

    }
}
