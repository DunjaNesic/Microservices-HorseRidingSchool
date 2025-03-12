using AutoMapper;
using Services.TrainerAPI.Domain.DTO;
using Services.TrainerAPI.Domain;

namespace Services.TrainerAPI.Infrastructure.Mapping
{
    public class TrainerProfile : Profile
    {
        public TrainerProfile()
        {
            CreateMap<Trainer, GetTrainerDTO>();
            CreateMap<CreateTrainerDTO, Trainer>();
        }
    }
}
