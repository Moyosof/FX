using FX.Domain.ReadOnly;
using FX.DTO.WriteOnly.CoreDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Application.Contracts.Core
{
    public interface ICourseUpload
    {
        Task<List<CourseUploadDto>> GetAllCourses(CancellationToken cancellation);
        Task<CourseUploadDto> GetCourse(Guid id);
        Task<string> AddCourse(CourseUploadDTO courseUpload);
        Task<CourseUploadDto> UpdateCourse(CourseUploadDTO courseUpload, Guid id);
        Task<string> DeleteCourse(Guid id);
    }
}
