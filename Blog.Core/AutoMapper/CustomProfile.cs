using AutoMapper;
using Blog.Core.Model.Models;
using Blog.Core.Model.ViewModels;

namespace Blog.Core.AutoMapper
{
    public class CustomProfile:Profile
    {
        public CustomProfile()
        {
            CreateMap<BlogArticle,BlogViewModels>();
            CreateMap<BlogViewModels,BlogArticle>();
        }   
    }
}