using AutoMapper;
using Services.HorseAPI.Domain.DTO;
using Services.HorseAPI.Domain;

namespace Services.HorseAPI.Infrastructure.Mapping
{
    public class HorseProfile : Profile
    {
        public HorseProfile()
        {
            CreateMap<Horse, GetHorseDTO>();
            CreateMap<CreateHorseDTO, Horse>();
        }
    }
}
