using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public async Task<ActionResult> GettAllBlogger()
        {
            try
            {
                return Ok(new { message = "Sikeres lekérdezés", result = await _blogContext.Bloggers.ToListAsync()});
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }

        [HttpGet("byid")]
        public async Task<ActionResult> GetBloggerById(int id)
        {
            try
            {
                var blogger = await _blogContext.Bloggers.FindAsync(id);

                if(blogger != null)
                {
                    return Ok(new { message = "Sikeres lekérdezés", result = blogger });
                }

                return StatusCode(404, new { message = "Sikertelen felvétel.", result = blogger });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBlogger([FromQuery]int id, [FromBody] UpdateBloggerDto updateBloggerDto)
        {
            try
            {
                var blogger = await _blogContext.Bloggers.FirstOrDefaultAsync(blogger => blogger.Id == id);

                if(blogger != null)
                {
                    blogger.Password = updateBloggerDto.Password;
                    blogger.Email = updateBloggerDto.Email;

                    _blogContext.Bloggers.Update(blogger);
                    await _blogContext.SaveChangesAsync();
                    return Ok(new { message = "Sikeres frissítés.", result = blogger });
                }

                return StatusCode(404, new { message = "Nincs találat.", result = blogger });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBlogger(int id)
        {
            try
            {
                var blogger = await _blogContext.Bloggers.FindAsync(id);

                if(blogger != null)
                {
                    _blogContext.Bloggers.Remove(blogger);
                    await _blogContext.SaveChangesAsync();
                    return Ok(new { message = "Sikeres törlés.", result = blogger });
                }

                return StatusCode(404, new { message = "Nincs találat.", result = blogger });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }

        [HttpGet("getAllBloggerData")]
        public async Task<ActionResult> GetAllBloggerData(int id)
        {
            try
            {
                var bloggerData = await _blogContext.Bloggers
                    .Include(x => x.Posts)
                    .Where(x => x.Id == id)
                    .ToListAsync();



                if(bloggerData != null)
                {
                    return Ok(new { message = "Sikeres lekérdezés", result = bloggerData });
                }

                return StatusCode(404, new { message = "Sikertelen felvétel.", result = bloggerData });

            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });

            }
        }

        [HttpGet("bloggerNameAndPostContent")]
        public async Task<ActionResult> BloggerNameAndPostContent(int id)
        {
            try
            {
                var bloggerData = await _blogContext.Bloggers
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        BloggerName = x.UserName,
                        Posts = x.Posts.Select(p => new { p.Content })
                    })
                    .FirstOrDefaultAsync();

                if (bloggerData != null)
                {
                    return Ok(new { message = "Sikeres lekérdezés", result = bloggerData });
                }

                return StatusCode(404, new { message = "Nincs találat.", result = bloggerData });

            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });

            }
        }
    }
}
