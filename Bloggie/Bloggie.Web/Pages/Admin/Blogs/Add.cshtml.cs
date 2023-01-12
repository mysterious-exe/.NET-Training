using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bloggie.Web.Pages.Admin.Blogs
{
    public class AddModel : PageModel
    {
        private readonly BloggieDbContext bloggieDbContext;

        [BindProperty]
        public AddBlogPost AddBlogpostRequest { get; set; }

        public AddModel(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost() 
        {
            var blogPost = new BlogPost()
            {
                Heading = AddBlogpostRequest.Heading,
                PageTitle = AddBlogpostRequest.PageTitle,
                Content = AddBlogpostRequest.Content,
                ShortDescription = AddBlogpostRequest.ShortDescription,
                FeaturedImageUrl = AddBlogpostRequest.FeaturedImageUrl,
                UrlHandle = AddBlogpostRequest.UrlHandle,
                PublishedDate = AddBlogpostRequest.PublishedDate,
                Author = AddBlogpostRequest.Author,
                Visible = AddBlogpostRequest.Visible,
            };
            await bloggieDbContext.BlogPosts.AddAsync(blogPost);
            await bloggieDbContext.SaveChangesAsync();

            return RedirectToPage("/admin/Blogs/List");
        }
    }

}
