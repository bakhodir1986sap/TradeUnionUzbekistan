using KasabaKIAT.Core.DTOs.Reference;
using KasabaKIAT.Core.Models;
using Mapster;

namespace KasabaKIAT.Core.Mappings;

public static class MapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<SpBranch, SpBranchDto>.NewConfig()
            .Map(dest => dest.Id, src => src.id)
            .Map(dest => dest.NameRu, src => src.name_ru)
            .Map(dest => dest.NameUz, src => src.name_uz)
            .Map(dest => dest.NameUzl, src => src.name_uzl)
            .Map(dest => dest.Status, src => src.s_status)
            .Map(dest => dest.CreateDate, src => src.s_create);
    }
}
