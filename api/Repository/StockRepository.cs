using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDbContext _ctx;
        public StockRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;

        }

        public async Task<Stock> AddStockAsync(Stock stockModel)
        {
            await _ctx.Stocks.AddAsync(stockModel);
            await _ctx.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteStockAsync(int id)
        {
            var stockModel = await _ctx.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);
            if (stockModel == null)
            {
                return null;
            }
            _ctx.Stocks.Remove(stockModel);
            await _ctx.SaveChangesAsync();
            return stockModel;

        }

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _ctx.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _ctx.Stocks.FindAsync(id);
        }

        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDTO updateStockDto)
        {
            var existingStock = await _ctx.Stocks.FirstOrDefaultAsync(stock => stock.Id == id);

            if (existingStock == null)
            {
                return null;
            }

            existingStock.Symbol = updateStockDto.Symbol;
            existingStock.Purchase = updateStockDto.Purchase;
            existingStock.CompanyName = updateStockDto.CompanyName;
            existingStock.LastDiv = updateStockDto.LastDiv;
            existingStock.Industry = updateStockDto.Industry;
            existingStock.MarketCap = updateStockDto.MarketCap;

            await _ctx.SaveChangesAsync();

            return existingStock;
        }
    }
}