using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    [Route("blogger")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        private readonly BlogContext _blogContext;

        public BloggerController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        [HttpPost]
        public async Task<ActionResult> AddNewBlogger(AddBloggerDto addBloggerDto)
        {
            try
            {
                var blogger = new Blogger
                {
                    UserName = addBloggerDto.UserName,
                    Password = addBloggerDto.Password,
                    Email = addBloggerDto.Email 
                };

                if(blogger != null)
                {
                    await _blogContext.Bloggers.AddAsync(blogger);
                    await _blogContext.SaveChangesAsync();

                    return StatusCode(201, new {message = "Sikeres felvétel.", result= blogger});
                }

                return StatusCode(404, new { message = "Sikertelen felvétel.", result = blogger });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }
    }
}
