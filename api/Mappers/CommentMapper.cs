using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Models;

namespace api.Mappers
{
    public static class CommentMapper
    {
        public static CommentDTO ToCommentDTO(this Comment comment)
        {
            return new CommentDTO
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                StockId = comment.StockId,
            };
        }

        public static Comment ToCommentFromCreateDTO(this CreateCommentRequestDTO createCommentDTO, int stockId){
            return new Comment{
                Title = createCommentDTO.Title,
                Content = createCommentDTO.Content,
                StockId = stockId,
            };
        }

        public static Comment ToCommentFromUpdateDTO(this UpdateCommentRequestDTO updateCommentDTO){
            return new Comment{
                Title = updateCommentDTO.Title,
                Content = updateCommentDTO.Content,
            };
        }
    }
}