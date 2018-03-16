using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using ComicNow.DTOs.Comment;
using ComicNow.Models;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class CommentsController : ApiController
    {
            public ComicNowEntities Context;

            public CommentsController()
            {
                Context = new ComicNowEntities();
            }

            //GET /api/comments/{comicId}
            //Get comment list of a comic book
            [HttpGet]
            [Route("api/comments/{comicId}")]
            public IHttpActionResult GetComments(int comicId)
            {
                var comic = Context.Comics.SingleOrDefault(c => c.Id == comicId);

                if (comic == null)
                {
                    return NotFound();
                }

                if (!comic.Comments.Any())
                {
                    return NotFound();
                }

                return Ok(comic.Comments.Select(MapComment_CommentDto)
                    .ToList());
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
                var comment = new Comment
                {
                    AccountId = postCommentDto.AccountId,
                    ComicId = postCommentDto.ComicId,
                    Text = postCommentDto.Text,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                };
                comment.LastUpdate = comment.CreatedDate;

                try
                {
                    Context.Comments.Add(comment);
                    Context.SaveChanges();
                    return Created(new Uri(Request.RequestUri + "/" + comment.Id), MapComment_CommentDto(comment));
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
                    return NotFound();
                }

                comment.IsActive = !comment.IsActive;
                Context.SaveChanges();
                return Ok(MapComment_CommentDto(comment));
            }

            //PUT /api/comments
            //Edit a comment
            [HttpPut]
            public IHttpActionResult EditComment(EditCommentDto commentDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var comment = Context.Comments.SingleOrDefault(c => c.Id == commentDto.Id && c.IsActive);

                if (comment == null)
                {
                    return NotFound();
                }

                try
                {
                    comment.Text = commentDto.Text;
                    comment.LastUpdate = DateTime.Now;
                    Context.SaveChanges();
                    return Ok(MapComment_CommentDto(comment));
            }
                catch (Exception)
                {
                    return Conflict();
                }
                

            }

        private static CommentDto MapComment_CommentDto(Comment comment)
        {
            return new CommentDto()
            {
                Id = comment.Id,
                ComicId = comment.ComicId,
                AccountId = comment.AccountId,
                AccountName = comment.Account.Username,
                CreatedDate = comment.CreatedDate,
                LastUpdate = comment.LastUpdate,
                Text = comment.Text
            };
        }
        
    }
}
