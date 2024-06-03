using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace api.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private ICommentRepository _commentRepo;

        public CommentController(ICommentRepository commentRepo)
        {
            _commentRepo = commentRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentRepo.GetAllCommentsAsync();
            var commentsDto = comments.Select(comment => comment.ToCommentDTO());

            return Ok(commentsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetCommentByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDTO());

        }

        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentRequestDTO createCommentDTO)
        {
            var commentModel = createCommentDTO.ToCommentFromCreateDTO();
            await _commentRepo.AddCommentAsync(commentModel);

            return CreatedAtAction(nameof(GetCommentById), new { Id = commentModel.Id }, commentModel.ToCommentDTO());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequestDTO updateCommentDTO)
        {
            var commentModel = await _commentRepo.UpdateCommentAsync(id, updateCommentDTO);

            if (commentModel == null)
            {
                return NotFound();
            }
            return Ok(commentModel.ToCommentDTO());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            var deletedComment = await _commentRepo.DeleteCommentAsync(id);

            if (deletedComment == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}