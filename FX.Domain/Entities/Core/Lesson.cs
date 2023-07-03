using FX.Domain.ReadOnly;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        //public Lesson(LessonDto lessonDto) 
        //{
        //    LessonId = Guid.NewGuid();
        //    LessonTitle= lessonDto.LessonTitle;
        //    VideoContent= lessonDto.VideoContent;
        //    TextContent= lessonDto.TextContent;
        //}
        [Key]
        public Guid LessonId { get; set; }
        public string LessonTitle { get; set; }
        public string VideoContent { get; set; }
        public string TextContent { get; set; }
    }
}
