using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMapper
    {
        public static StockDto ToStockDto(this Stock stockModel){
            return new StockDto{
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                Purchase = stockModel.Purchase,
                CompanyName = stockModel.CompanyName,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c=>c.ToCommentDTO()).ToList(),
            };
        }

        public static Stock ToStockFromCreateDTO(this CreateStockRequestDTO createStockDto){
            return new Stock{
                Symbol = createStockDto.Symbol,
                Purchase = createStockDto.Purchase,
                CompanyName = createStockDto.CompanyName,
                LastDiv = createStockDto.LastDiv,
                Industry = createStockDto.Industry,
                MarketCap = createStockDto.MarketCap
            };
        }

        public static Stock ToStockFromUpdateDTO (this UpdateStockRequestDTO updateStockDTO){
             return new Stock{
                Symbol = updateStockDTO.Symbol,
                Purchase = updateStockDTO.Purchase,
                CompanyName = updateStockDTO.CompanyName,
                LastDiv = updateStockDTO.LastDiv,
                Industry = updateStockDTO.Industry,
                MarketCap = updateStockDTO.MarketCap
            };
        }
    }
}