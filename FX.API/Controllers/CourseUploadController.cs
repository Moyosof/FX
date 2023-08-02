using FX.API.Helpers;
using FX.Application.Contracts.Core;
using FX.Domain.Entities.Core;
using FX.Domain.ReadOnly;
using FX.DTO.WriteOnly.CoreDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FX.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]

    public class CourseUploadController : BaseController
    {
        private readonly ICourseUpload _courseUpload;

        public CourseUploadController( ICourseUpload courseUpload)
        {
            _courseUpload = courseUpload;
        }

        /// <summary>
        /// This is to get all courses from the DB
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get_all_course")]
        [ProducesResponseType(typeof(JsonMessage<string>), 200)]

        public async Task<IActionResult> GetAllCourse(CancellationToken cancellationToken)
        {
            var result = await _courseUpload.GetAllCourses(cancellationToken);

            return Ok(new JsonMessage<CourseUploadDto>
            {
                Results2 = result.ToArray(),
                status = true,
                success_message = "Successful",
                status_code = (int)HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Get course by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("get_course_by_id")]
        [ProducesResponseType(typeof(JsonMessage<string>), 200)]

        public async Task<IActionResult> GetCourseById(Guid id)
        {
            var result = await _courseUpload.GetCourse(id);
            if(result != null)
            {
                return Ok(new JsonMessage<CourseUploadDto>()
                {
                    result = new List<CourseUploadDto>()
                    {
                        result
                    },
                    status = true,
                    success_message = "Course gotten by ID",
                    status_code = (int)HttpStatusCode.OK
                });
            }
            return Ok(new JsonMessage<CourseUploadDto>()
            {
                result = new List<CourseUploadDto>()
                {
                    result
                },
                status = false,
                error_message = "No course found by that particual Id",
                status_code = (int)HttpStatusCode.NotFound

            });
        }

        /// <summary>
        /// Add new courses
        /// </summary>
        /// <param name="courseUpload"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create_new_course")]
        [ProducesResponseType(typeof(JsonMessage<string>), 200)]

        public async Task<IActionResult> AddNewCourse([FromBody] CourseUploadDTO courseUpload)
        {
            var result = await _courseUpload.AddCourse(courseUpload);
            if (string.IsNullOrWhiteSpace(result))
            {

                return Ok(new JsonMessage<string>()
                {
                    status = true,
                    success_message = "Course created successfully",
                    status_code = (int)HttpStatusCode.OK

                });
            }
            return Ok(new JsonMessage<string>()
            {
                error_message = result,
                status = false,
                status_code = (int)HttpStatusCode.BadRequest

            });
        }

        /// <summary>
        /// This is to update a course
        /// </summary>
        /// <param name="courseUpload"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update_course")]
        [ProducesResponseType(typeof(JsonMessage<string>), 200)]

        public async Task<IActionResult> CourseUpdate(CourseUploadDTO courseUpload, Guid id)
        {
            var result = await _courseUpload.UpdateCourse(courseUpload, id);

            if (result is null)
            {
                return Ok(new JsonMessage<string>()
                {
                    error_message = "Course not Found",
                    status = false,
                    status_code = (int)HttpStatusCode.NotFound
                });
            }
            return Ok(new JsonMessage<string>()
            {
                status = true,
                success_message = "Course Updated successfully",
                status_code = (int)HttpStatusCode.OK
            });
        }

        /// <summary>
        /// This is to delete course by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete_course")]
        [ProducesResponseType(typeof(JsonMessage<string>), 200)]

        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var result = await _courseUpload.DeleteCourse(id);
            if (string.IsNullOrWhiteSpace(result))
            {
                return Ok(new JsonMessage<string>()
                {
                    status = true,
                    success_message = "Course Deleted  Successfully",
                    status_code = (int)HttpStatusCode.OK
                });
            }
            return Ok(new JsonMessage<string>()
            {
                error_message = result,
                status = false,
                status_code = (int)HttpStatusCode.NotFound
            });
        }
    }
}
