using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using ComicNow.DTOs;
using ComicNow.DTOs.Comic;
using ComicNow.DTOs.Comment;
using ComicNow.Models;

namespace ComicNow.Controllers.Api
{
    public class CommentsController : ApiController
    {
        public ComicNowEntities Context;

        public CommentsController()
        {
            Context = new ComicNowEntities();
        }

        //GET /api/comments/comicId
        //Get a list of the a comic book
        [HttpGet]
        public IHttpActionResult GetComments(int comicId)
        {
            var comic = Context.Comics.SingleOrDefault(c => c.Id == comicId);

            if (comic == null || comic.Comments.Count <= 0)
            {
                return NotFound();
            }

            return Ok(Mapper.Map<Comic, ComicWithCommentListDto>(comic));
        }

        //POST /api/comments/
        //Post a new comment
        [HttpPost]
        public IHttpActionResult PostComment(PostCommentDto postCommentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var comment = Mapper.Map<PostCommentDto, Comment>(postCommentDto);
                comment.CreatedDate = DateTime.Now;
                comment.LastUpdate = comment.CreatedDate;
                Context.Comments.Add(comment);
                Context.SaveChanges();
                return Created(new Uri(Request.RequestUri + "/" + comment.Id), Mapper.Map<Comment, ComicDto>(comment));
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        //PUT /api/comments/changeCommentStatus/commentId
        //Activate/Deactivate a comment
        [HttpPut]
        [Route("api/comments/changeCommentStatus/{commentId}")]
        public IHttpActionResult ChangeCommentStatus(int commentId)
        {
            var comment = Context.Comments.SingleOrDefault(c => c.ComicId == commentId);

            if (comment == null)
            {
                return Conflict();
            }

            comment.IsActive = !comment.IsActive;
            Context.SaveChanges();
            return Ok();
        }

        //PUT /api/comments
        //Edit a comment
        [HttpPut]
        public IHttpActionResult EditComment(CommentDto commentDto)
        {
            var comment = Context.Comments.SingleOrDefault(c => c.Id == commentDto.Id && c.IsActive);

            if (comment == null)
            {
                return NotFound();
            }

            comment.Text = commentDto.Text;
            comment.LastUpdate = DateTime.Now;
            commentDto.CreatedDate = comment.CreatedDate;
            commentDto.CreatedDate = comment.LastUpdate;
            Context.SaveChanges();

            return Ok(commentDto);

        }
    }
}
