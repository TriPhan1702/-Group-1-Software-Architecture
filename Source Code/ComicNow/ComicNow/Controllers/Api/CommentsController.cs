using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using ComicNow.DTOs.Comment;
using ComicNow.Models;
using ComicNow.Services;

namespace ComicNow.Controllers.Api
{
    [AllowCrossSiteJson]
    public class CommentsController : ApiController
    {
            public CommentServices CommentServices;
            public ComicServices ComicServices;

            public CommentsController()
            {
                CommentServices = new CommentServices();
                ComicServices = new ComicServices();
            }

            //GET /api/comments/{comicId}
            //Get comment list of a comic book
            [HttpGet]
            [Route("api/comments/{comicId}")]
            public IHttpActionResult GetComments(int comicId)
            {
                var comic = ComicServices.GetActiveComicById(comicId);

                if (comic == null)
                {
                    return NotFound();
                }

                var comments = CommentServices.GetAllActiveCommentOfComic(comic);

                if (!comments.Any())
                {
                    return NotFound();
                }

                return Ok(comments.Select(CommentServices.MapComment_CommentDto));
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

                var comment = CommentServices.CreateComment(postCommentDto);

                if (comment == null)
                {
                    return NotFound();
                }

                return Created(new Uri(Request.RequestUri + "/" + CommentServices.GetCommentId(comment)), CommentServices.MapComment_CommentDto(comment));
            }

            //PUT /api/comments/changeCommentStatus/commentId
            //Activate/Deactivate a comment
            [HttpPut]
            [Route("api/comments/changeCommentStatus/{commentId}")]
            public IHttpActionResult ChangeCommentStatus(int commentId)
            {
                var comment = CommentServices.GetCommentById(commentId);

                if (comment == null)
                {
                    return NotFound();
                }

                comment = CommentServices.ChangeCommentStatus(comment);

                if (comment == null)
                {
                    return Conflict();
                }

                return Ok(CommentServices.MapComment_CommentDto(comment));
            }

            //PUT /api/comments/edit
            //Edit a comment
            [HttpPut]
            [Route("api/comments/edit")]
            public IHttpActionResult EditComment(EditCommentDto commentDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var comment = CommentServices.GetActiveCommentById(commentDto.Id);

                if (comment == null)
                {
                    return NotFound();
                }

                comment = new Comment();

                return Ok(CommentServices.MapComment_CommentDto(comment));
            }
        
    }
}
