using FX.Domain.ReadOnly;
using FX.DTO.WriteOnly.CoreDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Domain.Entities.Core
{
    public class Lesson
    {
        public Lesson()
        {

        }
        public Lesson(LessonDto lessonDto)
        {
            LessonId = Guid.NewGuid();
            CourseId= lessonDto.CourseId;
            LessonTitle = lessonDto.LessonTitle;
            VideoContent = lessonDto.VideoContent;
            TextContent = lessonDto.TextContent;
        }

        public Lesson(LessonDTO lessonDTO, string videopath)
        {
            LessonId = Guid.NewGuid();
            CourseId = lessonDTO.CourseId; 
            LessonTitle = lessonDTO.LessonTitle;
            VideoContent = videopath;
            TextContent = lessonDTO.TextContent;

        }
        [Key]
        public Guid LessonId { get; set; }
        public Guid CourseId { get; set; }

        [ForeignKey("CourseId")]
        public virtual CourseUpload CourseUpload { get; set; }
        public string LessonTitle { get; set; }
        public string VideoContent { get; set; }
        public string TextContent { get; set; }
    }
}
