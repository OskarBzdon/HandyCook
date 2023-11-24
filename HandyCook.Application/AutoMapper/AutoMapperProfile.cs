using AutoMapper;
using HandyCook.Application.Data;
using HandyCook.Application.VOs;

namespace HandyCook.Application.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Recipe, RecipeVo>();
            CreateMap<RecipeVo, Recipe>();
        }
    }
}
