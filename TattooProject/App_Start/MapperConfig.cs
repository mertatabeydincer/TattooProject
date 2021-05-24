using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TattooProject.Models;
using TattooProject.ViewModels;

namespace TattooProject
{
    public class MapperConfig
    {
        public static IMapper Mapper;
        
        public static void Register()
        {
            var config = new MapperConfiguration(c =>
            {
                CreateMapViceVerca<Artist, ArtistModel>(c);
                CreateMapViceVerca<User, UserModel>(c);
            });

            Mapper = config.CreateMapper();
        }

        private static void CreateMapViceVerca<T1, T2>(IMapperConfigurationExpression expression)
        {
            expression.CreateMap<T1, T2>();
            expression.CreateMap<T2, T1>();
        }

    }
}