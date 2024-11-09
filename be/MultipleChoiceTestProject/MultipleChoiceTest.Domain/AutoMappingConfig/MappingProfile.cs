using AutoMapper;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;

namespace MultipleChoiceTest.Domain.AutoMappingConfig
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CUSubject, Subject>();
            CreateMap<Subject, CUSubject>();
        }
    }
}
