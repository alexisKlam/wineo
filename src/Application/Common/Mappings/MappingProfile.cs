using System.Reflection;
using AutoMapper;
using wineo.Application.Wines.Queries.GetWinePrices;
using wineo.Application.Wines.Queries.SearchWine.Models;
using wineo.Domain.Entities;

namespace wineo.Application.Common.Mappings;



public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        CreateMap<Wine, WineListItemDto>()
            .ForMember(dest => dest.CurrentPrice,
                opt => opt.MapFrom(src => src.Prices.MaxBy(p => p.Date)))
            .ForMember(dest => dest.Score,
                opt => opt.MapFrom(src => src.Evaluations.Count == 0 ? default(double?) : Math.Round(src.Evaluations!.Average(e => e.CalculateOverallScore()), 1)))
            .ForMember(dest => dest.Taste,
                opt => opt.MapFrom(src => src.Evaluations.Count == 0 ? default(double?) : Math.Round(src.Evaluations!.Average(e => e.Taste), 1)))
            .ForMember(dest => dest.Aroma,
                opt => opt.MapFrom(src => src.Evaluations.Count == 0 ? default(double?) : Math.Round(src.Evaluations!.Average(e => e.Aroma), 1)))
            .ForMember(dest => dest.Appearance,
                opt => opt.MapFrom(src => src.Evaluations.Count == 0 ? default(double?) : Math.Round(src.Evaluations!.Average(e => e.Appearance), 1)))
            ;
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var mapFromType = typeof(IMapFrom<>);
        
        var mappingMethodName = nameof(IMapFrom<object>.Mapping);

        bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;
        
        var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();
        
        var argumentTypes = new Type[] { typeof(Profile) };

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            
            var methodInfo = type.GetMethod(mappingMethodName);

            if (methodInfo != null)
            {
                methodInfo.Invoke(instance, new object[] { this });
            }
            else
            {
                var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

                if (interfaces.Count > 0)
                {
                    foreach (var @interface in interfaces)
                    {
                        var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

                        interfaceMethodInfo?.Invoke(instance, new object[] { this });
                    }
                }
            }
        }
    }
}
