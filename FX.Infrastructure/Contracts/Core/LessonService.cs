using FX.Application.Contracts.Core;
using FX.Application.FileService;
using FX.DataAccess.UnitOfWork.Interface;
using FX.Domain.Entities.Core;
using FX.Domain.ReadOnly;
using FX.DTO.WriteOnly.CoreDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Infrastructure.Contracts.Core
{
    public class LessonService : ILessonService
    {
        private readonly IFileService _fileService;
        private readonly IUnitOfWork<Lesson> _unitOfWork;

        public LessonService(IUnitOfWork<Lesson> unitOfWork, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _fileService = fileService;
        }

        public async Task<string> AddLesson(LessonDTO lessonDTO)
        {
            string videopath = null;
            if(lessonDTO.VideoContent != null)
            {
                try
                {
                    videopath = await _fileService.UploadVideo(lessonDTO.VideoContent);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            Lesson lesson = new Lesson(lessonDTO, videopath);
            //{
            //    CourseId= lessonDTO.CourseId,
            //    LessonTitle = lessonDTO.LessonTitle,
            //    VideoContent = videopath,
            //    TextContent = lessonDTO.TextContent
            //};
            try
            {
                await _unitOfWork.Repository.Add(lesson);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        public async Task<string> DeleteLesson(Guid id)
        {
            var lesson = await _unitOfWork.Repository.ReadSingle(id);
            if (lesson == null)
            {
                return string.Empty;
            }
            try
            {
                await _unitOfWork.Repository.Delete(id);
                //await _unitOfWork.SaveAsync();
                if (!string.IsNullOrEmpty(lesson.VideoContent))
                {
                    await _fileService.DeleteImage(lesson.VideoContent);
                }
                await _unitOfWork.SaveAsync();
                return "Lesson deleted succussfully";
            }
            catch { return string.Empty; }

        }

        public async Task<List<LessonDto>> GetAllLessons(CancellationToken cancellationToken)
        {
            var lesson = await _unitOfWork.Repository.ReadAllQuery().Select(c => new Lesson
            {
                LessonId= c.LessonId,
                CourseId= c.CourseId,
                LessonTitle= c.LessonTitle,
                VideoContent= c.VideoContent,
                TextContent= c.TextContent,
            }).ToListAsync(cancellationToken);

            List<LessonDto> result = new List<LessonDto>();
            foreach(var items in lesson)
            {
                result.Add(new LessonDto(items));
            }
            return result;
        }

        public async Task<LessonDto> GetLessons(Guid id)
        {
            var lesson = await _unitOfWork.Repository.ReadSingle(id);
            LessonDto lessonDto = new LessonDto(lesson);
            return lessonDto;
        }

        public async Task<LessonDto> UpdateLesson(LessonDTO lessonDTO, Guid id)
        {
            var lesson = await _unitOfWork.Repository.ReadSingle(id);
            if(id != lesson.LessonId)
            {
                return null;
            }
            try
            {
                lesson.CourseId = lessonDTO.CourseId;
                lesson.LessonTitle = lessonDTO.LessonTitle;
                lesson.TextContent = lessonDTO.TextContent;
                if(lessonDTO.TextContent != null)
                {
                    await _fileService.UploadVideo(lessonDTO.VideoContent);
                }
                _unitOfWork.Repository.Update(lesson);
                await _unitOfWork.SaveAsync();
                LessonDto lessons = new LessonDto(lesson);
                return lessons;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
