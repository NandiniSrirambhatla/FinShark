using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private ApplicationDbContext _ctx;

        public CommentRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<Comment> AddCommentAsync(Comment commentModel)
        {
            await _ctx.Comments.AddAsync(commentModel);
            await _ctx.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteCommentAsync(int id)
        {
            var comment = _ctx.Comments.Find(id);
            if (comment == null)
            {
                return null;
            }

            _ctx.Comments.Remove(comment);
            await _ctx.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            var comments = await _ctx.Comments.ToListAsync();

            return comments;
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            var comment = await _ctx.Comments.FirstOrDefaultAsync(comment=>comment.Id==id);
            if(comment == null){
                return null;
            }
            return comment;
        }

        public async Task<Comment?> UpdateCommentAsync(int id, UpdateCommentRequestDTO updatedComment)
        {
            var existingComment = await _ctx.Comments.FirstOrDefaultAsync(comment => comment.Id == id);
            if(existingComment == null){
                return null;
            }
            existingComment.Content = updatedComment.Content;
            existingComment.Title = updatedComment.Title;
            existingComment.CreatedOn = updatedComment.CreatedOn;
            existingComment.StockId = updatedComment.StockId;

            await _ctx.SaveChangesAsync();

            return existingComment;

        }
    }
}