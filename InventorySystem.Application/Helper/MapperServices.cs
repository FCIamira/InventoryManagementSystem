using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

using AutoMapper;
namespace InventorySystem.Application.Mapping
{

    namespace HandmadeMarket.Services
    {
        public static class MapperServices
        {
            public static IMapper Mapper { get; set; }

            public static IQueryable<TDestination> ProjectTo<TDestination>(this IQueryable source)
            {
                return source.ProjectTo<TDestination>(Mapper.ConfigurationProvider);
            }

            public static IEnumerable<TDestination> ProjectEnumrableTo<TSource, TDestination>(this IEnumerable<TSource> source)
            {
                return source.AsQueryable().ProjectTo<TDestination>(Mapper.ConfigurationProvider);
            }

            public static TDestination Map<TDestination>(this object source)
            {
                return Mapper.Map<TDestination>(source);
            }
        }

    }

}
