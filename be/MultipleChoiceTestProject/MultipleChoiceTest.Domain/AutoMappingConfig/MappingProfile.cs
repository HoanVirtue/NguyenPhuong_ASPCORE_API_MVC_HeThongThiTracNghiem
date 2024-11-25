﻿using AutoMapper;
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

            CreateMap<CUQuestion, Question>(); // CUQuestion là nguồn, Question là đích, chuyển từ nguồn thành đích, e viết trương tự như usẻ đi
            CreateMap<Question, CUQuestion>();


            // chưa mapp đối tượng, đây là chuyển đổi từ CUUSER -> USER
            CreateMap<CUUser, User>();
            CreateMap<User, CUUser>();
            CreateMap<User, UserItem>();

            CreateMap<ExamResult, CUExamResult>();
            CreateMap<ExamResult,ResultItem>().ForMember(dest=>dest.UserName,opt=>opt.MapFrom(src=>src.User.UserName))
                                                .ForMember(dest=>dest.ExamName,opt=>opt.MapFrom(src=>src.Exam.ExamName));
            CreateMap<Exam, CUExam>();
            CreateMap<CUExam, Exam>();
            CreateMap<Exam, ExamItem>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.SubjectName))
                .ForMember(dest => dest.LessonName, opt => opt.MapFrom(src => src.Lesson.LessonName));
        }
    }
}
