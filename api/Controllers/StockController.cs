using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stocks")]
    [ApiController]
    public class StockController :ControllerBase
    {
        private readonly ApplicationDbContext _ctx;
        public StockController(ApplicationDbContext context)
        {
            _ctx=context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStocks(){
            var stocks = await _ctx.Stocks.ToListAsync();
            
            var stockDto = stocks.Select(stock=>stock.ToStockDto());
            return Ok(stocks);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetStockById( [FromRoute] int id){
            var stock = await _ctx.Stocks.FindAsync(id);
            if(stock == null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }

        [HttpPost]

        public async Task<IActionResult> AddStock([FromBody] CreateStockRequestDTO createStockDto){
            var stockModel = createStockDto.ToStockFromCreateDTO();
            await _ctx.Stocks.AddAsync(stockModel);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStockById), new{id = stockModel.Id});
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateStock([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateStockDto){
            var stockModel = await _ctx.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);

            if(stockModel == null){
                return NotFound();
            }
            
            // stockModel =  updateStockDto.ToStockFromUpdateDTO();
            // _ctx.Stocks.Update(stockModel);

            stockModel.Symbol = updateStockDto.Symbol;
            stockModel.Purchase = updateStockDto.Purchase;
            stockModel.CompanyName = updateStockDto.CompanyName;
            stockModel.LastDiv = updateStockDto.LastDiv;
            stockModel.Industry = updateStockDto.Industry;
            stockModel.MarketCap = updateStockDto.MarketCap;
            await _ctx.SaveChangesAsync();

            return Ok(stockModel.ToStockDto());

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteStock([FromRoute] int id){
            var stockModel = await _ctx.Stocks.FirstOrDefaultAsync(stock=>stock.Id==id);

            if(stockModel == null){
                return NotFound();
            }

            _ctx.Stocks.Remove(stockModel);
            await _ctx.SaveChangesAsync();

            return NoContent();
        }
    }
}