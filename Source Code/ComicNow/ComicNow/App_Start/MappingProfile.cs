using AutoMapper;
using ComicNow.DTOs;
using ComicNow.DTOs.Account;
using ComicNow.DTOs.Comic;
using ComicNow.Models;

namespace ComicNow
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Account, LoginAccountDto>().ReverseMap();

            Mapper.CreateMap<Account, AccountDto>().ReverseMap();

            Mapper.CreateMap<Chapter, ChapterDto>().ReverseMap();

            Mapper.CreateMap<Publisher, PublisherDto>().ReverseMap();

            Mapper.CreateMap<Author, AuthorDto>().ReverseMap();

            Mapper.CreateMap<Tag, TagDto>().ReverseMap();

            Mapper.CreateMap<Comic, ComicDto>().ReverseMap();

            Mapper.CreateMap<Comic, ComicThumbnailDto>().ReverseMap();
        }
    }
}