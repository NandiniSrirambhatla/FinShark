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

        public static Comment ToCommentFromCreateDTO(this CreateCommentRequestDTO createCommentDTO){
            return new Comment{
                Title = createCommentDTO.Title,
                Content = createCommentDTO.Content,
                CreatedOn = createCommentDTO.CreatedOn,
                StockId = createCommentDTO.StockId,
            };
        }
    }
}