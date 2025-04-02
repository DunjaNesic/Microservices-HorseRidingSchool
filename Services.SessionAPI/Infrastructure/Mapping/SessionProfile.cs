using AutoMapper;
using Services.SessionAPI.Domain.DTO;
using Services.SessionAPI.Domain;

namespace Services.HorseAPI.Infrastructure.Mapping
{
    public class SessionProfile : Profile
    {
        public SessionProfile()
        {
            CreateMap<SessionDetails, SessionDetailsDTO>()
                .ForMember(dest => dest.SessionDetailsID, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<SessionAssigned, SessionAssignedDTO>().ReverseMap();
            CreateMap<SessionDTO, SessionPublishDTO>().ReverseMap();

        }
    }
}
