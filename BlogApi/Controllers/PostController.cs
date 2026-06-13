using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers
{
    [Route("post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly BlogContext _blogContext;

        public PostController(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        [HttpPost]
        public async Task<ActionResult> AddNewPost(AddPostDto addPostDto)
        {
            try
            {
                var post = new Post
                {
                   Title = addPostDto.Title,
                   Content = addPostDto.Content,
                   BlogId = addPostDto.BlogId,
                   CreatedTime = DateTime.Now
                };

                if (post != null)
                {
                    await _blogContext.Posts.AddAsync(post);
                    await _blogContext.SaveChangesAsync();

                    return StatusCode(201, new { message = "Sikeres felvétel.", result = post });
                }

                return StatusCode(404, new { message = "Sikertelen felvétel.", result = post });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult> GettAllPost()
        {
            try
            {
                return Ok(new { message = "Sikeres lekérdezés", result = await _blogContext.Posts.ToListAsync() });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }

        [HttpGet("byid")]
        public async Task<ActionResult> GetPostById(int id)
        {
            try
            {
                var post = await _blogContext.Posts.FindAsync(id);

                if (post != null)
                {
                    return Ok(new { message = "Sikeres lekérdezés", result = post });
                }

                return StatusCode(404, new { message = "Sikertelen lekérdezés.", result = post });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdatePost([FromQuery] int id, [FromBody] UpdatePostDto updatePostDto)
        {
            try
            {
                var post = await _blogContext.Posts.FirstOrDefaultAsync(post => post.Id == id);

                if (post != null)
                {
                    post.Title = updatePostDto.Title;
                    post.Content = updatePostDto.Content;

                    _blogContext.Posts.Update(post);
                    await _blogContext.SaveChangesAsync();
                    return Ok(new { message = "Sikeres frissítés.", result = post });
                }

                return StatusCode(404, new { message = "Nincs találat.", result = post });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePost(int id)
        {
            try
            {
                var post = await _blogContext.Posts.FindAsync(id);

                if (post != null)
                {
                    _blogContext.Posts.Remove(post);
                    await _blogContext.SaveChangesAsync();
                    return Ok(new { message = "Sikeres törlés.", result = post });
                }

                return StatusCode(404, new { message = "Nincs találat.", result = post });
            }
            catch (Exception ex)
            {
                return StatusCode(400, new { message = ex.Message });
            }
        }
    }
}
