using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TattooProject
{
    public static class CommonExtensions
    {
        public static T2 MapTo<T2>(this object source)
        {
            var sourceType = source.GetType();
            var targetType = typeof(T2);

            return (T2)Convert.ChangeType(MapperConfig.Mapper.Map(source, sourceType, targetType), targetType);
        }
    }
}