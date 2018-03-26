using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComicNow.DTOs.Comment;
using ComicNow.Models;

namespace ComicNow.Services
{
    public class CommentServices
    {
        public ComicNowEntities Context;

        public CommentServices()
        {
            Context = new ComicNowEntities();
        }

        public List<Comment> GetAllActiveCommentOfComic(Comic comic)
        {
            return (from comment in comic.Comments where comment.IsActive select comment).ToList();
        }

        public Comment GetActiveCommentById(int commentId)
        {
            return Context.Comments.SingleOrDefault(comment => comment.IsActive && comment.Id == commentId);
        }

        public Comment GetCommentById(int commentId)
        {
            return Context.Comments.SingleOrDefault(comment => comment.Id == commentId);
        }

        public static CommentDto MapComment_CommentDto(Comment comment)
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

        public Comment CreateComment(PostCommentDto postCommentDto)
        {
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
                return comment;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int GetCommentId(Comment comment)
        {
            return comment.Id;
        }

        public Comment ChangeCommentStatus(Comment comment)
        {
            try
            {
                comment.IsActive = !comment.IsActive;
                Context.SaveChanges();
                return comment;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Comment EditComment(Comment comment, string text)
        {
            try
            {
                comment.Text = text;
                comment.LastUpdate = DateTime.Now;
                Context.SaveChanges();
                return comment;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}