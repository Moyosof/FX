using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.DTO.WriteOnly.CoreDTO
{
    public class LessonDTO
    {
        public string LessonTitle { get; set; }
        public IFormFile VideoContent { get; set; }
        public IFormFile TextContent { get; set; } 
    }
}
