using FX.Domain.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FX.Domain.ReadOnly
{
    public class CourseUploadDto
    {
        public CourseUploadDto() 
        {
            Lessons = new HashSet<Lesson>();
        }
        public CourseUploadDto(CourseUpload courseUpload) 
        {
            CourseId= courseUpload.CourseId;
            CourseTitle= courseUpload.CourseTitle;
            CourseDescription= courseUpload.CourseDescription;
            Lessons = courseUpload.Lessons;
        }
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
    }
}
