using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComicNow.Models;

namespace ComicNow.Services
{
    public class TagServices
    {
        public ComicNowEntities Context;

        public TagServices()
        {
            Context = new ComicNowEntities();
        }

        public List<Tag> GetAllTags()
        {
            return Context.Tags.ToList();
        }

        public Tag GetTagById(int id)
        {
            return Context.Tags.SingleOrDefault(tag => tag.Id == id);
        }

        public List<Tag> FindTagsOfAComic(Comic comic)
        {
            return comic.Tags.ToList();
        }

        public List<Tag> FindTagByString(string searchValue)
        {
            return (from tag in Context.Tags where tag.Name.Contains(searchValue) select tag).ToList();
        }

        public Tag CreateTag(string tagName)
        {
            try
            {
                var newTag = new Tag()
                {
                    Name = tagName
                };
                Context.Tags.Add(newTag);
                Context.SaveChanges();

                return newTag;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int GetTagId(Tag tag)
        {
            return tag.Id;
        }
    }
}