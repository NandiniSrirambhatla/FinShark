using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Helpers;
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

        public async Task<List<Stock>> GetAllStocksAsync(QueryObject query)
        {

            var stocks = _ctx.Stocks.Include(c => c.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(stock => stock.CompanyName.Contains(query.CompanyName));
            }
            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(Stock => Stock.Symbol.Contains(query.Symbol));
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDesc ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _ctx.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(stock => stock.Id == id);
        }

        public async Task<bool> StockExists(int id)
        {
            return await _ctx.Stocks.AnyAsync(Stock => Stock.Id == id);
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