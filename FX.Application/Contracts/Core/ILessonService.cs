using FX.Domain.ReadOnly;
using FX.DTO.WriteOnly.CoreDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Application.Contracts.Core
{
    public interface ILessonService
    {
        Task<List<LessonDto>> GetAllLessons(CancellationToken cancellationToken);
        Task<LessonDto> GetLessons(Guid id);
        Task<string> AddLesson(LessonDTO lessonDTO);
        Task<LessonDto> UpdateLesson(LessonDTO lessonDTO, Guid id);
        Task<string> DeleteLesson(Guid id);
    }
}
