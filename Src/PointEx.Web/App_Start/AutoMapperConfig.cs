using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Framework.Common.Mapping;
using PointEx.Entities;
using PointEx.Service;
using PointEx.Web.Mapping;

namespace PointEx.Web
{
	public class AutoMapperConfig
	{
		public static void Config()
		{
			var typesReference = Assembly
                           .GetExecutingAssembly()
                           .GetReferencedAssemblies()
                           .Where(x => x.Name.StartsWith("PointEx"))
                           .Select(x => Assembly.Load(x))
                           .SelectMany(x => x.GetTypes()).ToArray();

            var typesCurrentAssembly = Assembly.GetExecutingAssembly().GetExportedTypes();

            var types = typesCurrentAssembly.Concat(typesReference).ToArray();

			LoadStandardMappings(types);

			LoadCustomMappings(types);

		    LoadProfileMapping();
		}

        private static void LoadProfileMapping()
        {
            Mapper.AddProfile<ShopMappingProfile>();
        }

		private static void LoadCustomMappings(IEnumerable<Type> types)
		{
			var maps = (from t in types
						from i in t.GetInterfaces()
						where typeof(IHaveCustomMappings).IsAssignableFrom(t) &&
							  !t.IsAbstract &&
							  !t.IsInterface
						select (IHaveCustomMappings)Activator.CreateInstance(t)).ToArray();

			foreach (var map in maps)
			{
				map.CreateMappings(Mapper.Configuration);
			}
		}

		private static void LoadStandardMappings(IEnumerable<Type> types)
		{
			var maps = (from t in types
						from i in t.GetInterfaces()
						where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
							  !t.IsAbstract &&
							  !t.IsInterface
						select new
						{
							Source = i.GetGenericArguments()[0], 
							Destination=t
						}).ToArray();

			foreach (var map in maps)
			{
				Mapper.CreateMap(map.Source, map.Destination);
                Mapper.CreateMap(map.Destination, map.Source);
			}
		}
	}
}