using FX.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Domain.ReadOnly
{
    public class LessonDto
    {
        public LessonDto(Lesson lesson) 
        {
            LessonId = lesson.LessonId;
            LessonTitle= lesson.LessonTitle;
            VideoContent= lesson.VideoContent;
            TextContent = lesson.TextContent;
        }
        public Guid LessonId { get; set; }
        public string LessonTitle { get; set; }
        public string VideoContent { get; set; }
        public string TextContent { get; set; }
    }
}
