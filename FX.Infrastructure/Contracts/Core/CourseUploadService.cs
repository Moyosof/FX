using FX.Application.Contracts.Core;
using FX.DataAccess.UnitOfWork.Interface;
using FX.Domain.Entities.Core;
using FX.Domain.ReadOnly;
using FX.DTO.WriteOnly.CoreDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Infrastructure.Contracts.Core
{
    public class CourseUploadService : ICourseUpload
    {
        private readonly IUnitOfWork<CourseUpload> _unitOfWork;

        public CourseUploadService(IUnitOfWork<CourseUpload> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> AddCourse(CourseUploadDTO courseUpload)
        {
            CourseUpload course = new CourseUpload (courseUpload);
            try
            {
                await _unitOfWork.Repository.Add(course);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        public async Task<string> DeleteCourse(Guid id)
        {
            var course = await _unitOfWork.Repository.ReadSingle(id);
            if (course == null)
            {
                return "Course Not Found";
            }

            try
            {
                await _unitOfWork.Repository.Delete(id);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }

        public async Task<List<CourseUploadDto>> GetAllCourses(CancellationToken cancellation)
        {
            var course = await _unitOfWork.Repository.ReadAllQuery().Select(x => new CourseUpload
            {
                CourseId= x.CourseId,
                CourseTitle = x.CourseTitle,
                CourseDescription = x.CourseDescription,
                Lessons= x.Lessons,
            }).ToListAsync(cancellation);

            List<CourseUploadDto> result = new List<CourseUploadDto>();
            foreach (var courseUpload in course)
            {
                result.Add(new CourseUploadDto(courseUpload));
            }
            return result;
        }

        public async Task<CourseUploadDto> GetCourse(Guid id)
        {
            var course = await _unitOfWork.Repository.ReadAllQuery().Where(x => x.CourseId == id).Include(x => x.Lessons).FirstOrDefaultAsync();
            CourseUploadDto courseUpload = new CourseUploadDto (course);
            return courseUpload;
        }

        public async Task<CourseUploadDto> UpdateCourse(CourseUploadDTO courseUpload, Guid id)
        {
            var course = await _unitOfWork.Repository.ReadSingle(id);
            if(id != course.CourseId)
            {
                return null;
            }

            course.CourseTitle = courseUpload.CourseTitle;
            course.CourseDescription = courseUpload.CourseDescription;

            _unitOfWork.Repository.Update(course);
            await _unitOfWork.SaveAsync();
            CourseUploadDto result = new(course);
            return result;
        }
    }
}
