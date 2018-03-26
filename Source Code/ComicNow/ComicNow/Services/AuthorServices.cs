using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComicNow.Models;

namespace ComicNow.Services
{
    public class AuthorServices
    {
        public ComicNowEntities Context;

        public AuthorServices()
        {
                Context = new ComicNowEntities();
        }

        public List<Author> GetAllAuthors()
        {
            return Context.Authors.ToList();
        }

        public Author GetAuthorById(int authorId)
        {
            return Context.Authors.SingleOrDefault(author => author.Id == authorId);
        }

        

        //Create new author based on the name
        public Author CreateNewAuthor(string authorName)
        {
            try
            {
                var newAuthor = new Author()
                {
                    Name = authorName
                };
                Context.Authors.Add(newAuthor);
                Context.SaveChanges();

                return newAuthor;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}