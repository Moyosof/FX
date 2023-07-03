using FX.Domain.ReadOnly;
using FX.DTO.WriteOnly.CoreDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Domain.Entities.Core
{
    public class CourseUpload
    {
        public CourseUpload()
        {
            Lessons = new HashSet<Lesson>();
        }
        public CourseUpload(CourseUploadDto courseUploadDto)
        {
            CourseId = Guid.NewGuid();
            CourseTitle = courseUploadDto.CourseTitle;
            CourseDescription = courseUploadDto.CourseDescription;
        }
        [Key]
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
