using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repository;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace api.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comments = await _commentRepo.GetAllCommentsAsync();
            var commentsDto = comments.Select(comment => comment.ToCommentDTO());

            return Ok(commentsDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCommentById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentRepo.GetCommentByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDTO());

        }

        [HttpPost("{stockId:int}")]

        public async Task<IActionResult> AddComment([FromRoute] int stockId, [FromBody] CreateCommentRequestDTO createCommentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _stockRepo.StockExists(stockId))
            {
                return BadRequest("Stock does not exist");
            }

            var commentModel = createCommentDTO.ToCommentFromCreateDTO(stockId);
            await _commentRepo.AddCommentAsync(commentModel);

            return CreatedAtAction(nameof(GetCommentById), new { Id = commentModel.Id }, commentModel.ToCommentDTO());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequestDTO updateCommentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentRepo.UpdateCommentAsync(id, updateCommentDTO.ToCommentFromUpdateDTO());

            if (comment == null)
            {
                return NotFound("Comment not found");
            }
            return Ok(comment.ToCommentDTO());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var deletedComment = await _commentRepo.DeleteCommentAsync(id);

            if (deletedComment == null)
            {
                return NotFound("Comment not found");
            }

            return NoContent();
        }
    }
}