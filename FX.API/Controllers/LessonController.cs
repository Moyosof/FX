using FX.API.Helpers;
using FX.Application.Contracts.Core;
using FX.Domain.Entities.Core;
using FX.Domain.ReadOnly;
using FX.DTO;
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
    public class LessonController : BaseController
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService) 
        {
            _lessonService = lessonService;
        }

        /// <summary>
        /// Get all lesson upload
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("get_all_lesson")]
        [ProducesResponseType(typeof(JsonMessage<string>), 200)]

        public async Task<IActionResult> GetAllLesson(CancellationToken cancellationToken)
        {
            var lesson = await _lessonService.GetAllLessons(cancellationToken);
            return Ok(new JsonMessage<LessonDto>
            {
                Results2 = lesson.ToArray(),
                status = true,
                success_message = "Successful",
                status_code = (int)HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Get lesson by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("get_lesson_by_id")]
        [ProducesResponseType(typeof(JsonMessage<string>), 200)]

        public async Task<IActionResult> GetCourseById(Guid id)
        {
            var result = await _lessonService.GetLessons(id);
            if (result != null)
            {
                return Ok(new JsonMessage<LessonDto>()
                {
                    result = new List<LessonDto>()
                    {
                        result
                    },
                    status = true,
                    success_message = "lesson gotten by ID",
                    status_code = (int)HttpStatusCode.OK
                });
            }
            return Ok(new JsonMessage<LessonDto>()
            {
                result = new List<LessonDto>()
                {
                    result
                },
                status = false,
                error_message = "No lesson found by that particual Id",
                status_code = (int)HttpStatusCode.NotFound

            });
        }

        /// <summary>
        /// Add new lesson
        /// </summary>
        /// <param name="lesson"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create_new_lesson")]
        [ProducesResponseType(typeof(JsonMessage<string>), 200)]

        public async Task<IActionResult> AddNewLesson([FromForm] LessonDTO lesson)
        {
            var result = await _lessonService.AddLesson(lesson);
            if (string.IsNullOrWhiteSpace(result))
            {

                return Ok(new JsonMessage<string>()
                {
                    status = true,
                    success_message = "Lesson created successfully",
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
       /// Update lesson 
       /// </summary>
       /// <param name="lesson"></param>
       /// <param name="id"></param>
       /// <returns></returns>
        [HttpPut]
        [Route("update_lessn")]
        [ProducesResponseType(typeof(JsonMessage<string>), 200)]

        public async Task<IActionResult> LessonUpdate([FromForm] LessonDTO lesson, Guid id)
        {
            var result = await _lessonService.UpdateLesson(lesson, id);

            if (result is null)
            {
                return Ok(new JsonMessage<string>()
                {
                    error_message = "lesson not Found",
                    status = false,
                    status_code = (int)HttpStatusCode.NotFound
                });
            }
            return Ok(new JsonMessage<string>()
            {
                status = true,
                success_message = "Lesson Updated successfully",
                status_code = (int)HttpStatusCode.OK
            });
        }


        /// <summary>
        /// This is to delete lesson by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete_lesson")]
        [ProducesResponseType(typeof(JsonMessage<string>), 200)]

        public async Task<IActionResult> DeleteLesson(Guid id)
        {
            var result = await _lessonService.DeleteLesson(id);
            if (string.IsNullOrWhiteSpace(result))
            {
                return Ok(new JsonMessage<string>()
                {
                    status = true,
                    success_message = "Lesson Deleted Successfully",
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
