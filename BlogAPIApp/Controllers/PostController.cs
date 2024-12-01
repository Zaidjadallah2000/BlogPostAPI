using Blogpp.data;
using Blogpp.Models;
using Blogpp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace BlogAPIApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;
        private readonly IHostingEnvironment host;
        private readonly ITopicService topicService;

        public PostController(IPostService _postService,
            IHostingEnvironment _host,
            
            ITopicService _topicService)
        {
            postService = _postService;
            host = _host;
            topicService = _topicService;
        }
        [HttpPost]
        [Route("insert")]
        public void insert(BlogPostDTO postDTO)
        {
            string fileName = string.Empty;
            if (postDTO.file != null)
            {
                string uploads = Path.Combine(host.WebRootPath, "Uploads");
                fileName = postDTO.file.FileName;
                string fullpath = Path.Combine(uploads, fileName);
                postDTO.file.CopyToAsync(new FileStream(fullpath, FileMode.Create));

            }
            var blog = postService.insert(postDTO);
            TopicDTO topicDTO = new TopicDTO();
            topicDTO.Name = postDTO.Topic;
            var Topic = topicService.add(topicDTO);
            foreach (var item in Topic)
            {
                BlogPostTopic blogPostTopic = new BlogPostTopic();
                blogPostTopic.blogpost = blog;
                blogPostTopic.topic = item;
                topicService.insertBlogTopic(blogPostTopic);
            }

        }
        
        
        [HttpGet]
        [Route("getAllPost")]
        public List<BlogPostDTO> getAllPost() {
            return postService.getPosts();
        }
        [HttpGet]
        [Route("search")]
        public List<BlogPostDTO> Search(string? searchQuery, string? filterTopic)
        {
            return postService.Search(searchQuery, filterTopic);
        }
        [HttpDelete]
        public void delete(int id)
        {
            postService.Delete(id);
        }
        [HttpGet]
        [Route("getToEdit")]
        public BlogPostDTO edit(int id)
        {
            return postService.Edit(id);
        }
        [HttpPut]
        public void update(BlogPostDTO blog)
        {
            string fileName = string.Empty;
            if (blog.file != null)
            {
                string uploads = Path.Combine(host.WebRootPath, "Uploads");
                fileName = blog.file.FileName;
                string fullpath = Path.Combine(uploads, fileName);
                blog.file.CopyToAsync(new FileStream(fullpath, FileMode.Create));

            }
            postService.update(blog);
        }
    }
}
