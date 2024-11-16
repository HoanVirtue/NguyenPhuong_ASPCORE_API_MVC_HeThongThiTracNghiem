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

            CreateMap<CULesson, Lesson>();
            CreateMap<Lesson, CULesson>();
            CreateMap<Lesson, LessonItem>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.SubjectName));

            CreateMap<CUQuestion, Question>();
            CreateMap<Question, CUQuestion>();
        }
    }
}
