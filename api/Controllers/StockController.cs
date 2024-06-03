using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStocks()
        {
            var stocks = await _stockRepo.GetAllStocksAsync();
            var stockDto = stocks.Select(stock => stock.ToStockDto());
            return Ok(stockDto);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetStockById([FromRoute] int id)
        {
            var stock = await _stockRepo.GetStockByIdAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]

        public async Task<IActionResult> AddStock([FromBody] CreateStockRequestDTO createStockDto)
        {
            var stockModel = createStockDto.ToStockFromCreateDTO();
            await _stockRepo.AddStockAsync(stockModel);

            return CreatedAtAction(nameof(GetStockById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateStockDto)
        {
            var stockModel = await _stockRepo.UpdateStockAsync(id, updateStockDto);

            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id)
        {
            var stockModel = await _stockRepo.DeleteStockAsync(id);

            if (stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}