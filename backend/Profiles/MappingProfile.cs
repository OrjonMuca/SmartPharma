using AutoMapper;
using Backend_myMeds.DTOs;
using Backend_myMeds.Models;

namespace Backend_myMeds.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Medicine -> MedicineResponseDto
            CreateMap<Medicine, MedicineResponseDto>();

            // CreateMedicineDto -> Medicine
            CreateMap<CreateMedicineDto, Medicine>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // UpdateMedicineDto -> Medicine
            CreateMap<UpdateMedicineDto, Medicine>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}