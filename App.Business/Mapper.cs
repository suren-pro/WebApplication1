using App.Business.Dto;
using App.Data.Models;
using AutoMapper;
namespace App.Business
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<PostDto, Post>().ReverseMap();
        }
    }
}
