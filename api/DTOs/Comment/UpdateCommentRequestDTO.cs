using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static api.Models.Stock;

namespace api.DTOs.Comment
{
    public class UpdateCommentRequestDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? StockId { get; set; }

    }
}