using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using SimpleBlog.Infrastructure;
using SimpleBlog.Models;
using SimpleBlog.ViewModels;

namespace SimpleBlog.Controllers
{
    public class PostsController : Controller
    {
        private const int PostsPerPage = 8;

        // GET: Posts
        public ActionResult Index(int page = 1)
        {

            var totalPostCount =
                Database.Session.Query<Post>()
                    .Where(t => t.DeleteAt == null)
                    .OrderByDescending(t => t.CreatedAt)
                    .Count();

            // return an array of post ids
            var postIds = Database.Session.Query<Post>()
                .Where(t => t.DeleteAt == null)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1)*PostsPerPage)
                .Take(PostsPerPage)
                .Select(p => p.Id)
                .ToArray();

            var currentPostPage = Database.Session.Query<Post>()
                .Where(t => t.DeleteAt == null)
                .Where(p => postIds.Contains(p.Id))
                .OrderByDescending(c => c.CreatedAt)
                .FetchMany(f => f.Tags)
                .Fetch(f => f.User)
                .ToList();


            // we are now sending that data to our view which we will strongly map to this with : @model SimpleBlog.ViewModels.PostsIndex
            return View(new PostsIndex
            {
                Posts = new PagedData<Post>(currentPostPage, totalPostCount, page, PostsPerPage)
            });
        }

        public ActionResult Tag(string idAndSlug, int page = 1)
        {
            // parts is a Tuple<Item1, Item2>
            var parts = SeparateIdAndSlug(idAndSlug);

            if (parts == null) return HttpNotFound();

            var tag = Database.Session.Load<Tag>(parts.Item1);
            if (tag == null)
            {
                return HttpNotFound();
            }

            // if invalid slug
            if (!tag.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
                return RedirectToRoutePermanent("Tag", new { id = parts.Item1, slug = tag.Slug });

            var totalPostCount = tag.Posts.Count();

            // return an array of post ids
            var postIds = tag.Posts
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * PostsPerPage)
                .Take(PostsPerPage)
                .Select(p => p.Id)
                .ToArray();

            var posts = Database.Session.Query<Post>()
                .Where(p => postIds.Contains(p.Id))
                .OrderByDescending(c => c.CreatedAt)
                .FetchMany(f => f.Tags)
                .Fetch(f => f.User)
                .ToList();

            
            return View(new PostsTag
            {
                Tag = tag,
                Posts = new PagedData<Post>(posts, totalPostCount, page, PostsPerPage)
            });
        }

        public ActionResult Show(string idAndSlug)
        {
            // parts is a Tuple<Item1, Item2>
            var parts = SeparateIdAndSlug(idAndSlug);

            if (parts == null) return HttpNotFound();

            var post = Database.Session.Load<Post>(parts.Item1);
            if (post == null || post.IsDeleted)
            {
                return HttpNotFound();
            }

            // if invalid slug
            if (!post.Slug.Equals(parts.Item2, StringComparison.CurrentCultureIgnoreCase))
                return RedirectToRoutePermanent("Post", new {id = parts.Item1, slug = post.Slug});

            // good for SEO RedirectToRoutePermanent. Tells google only one way and instance to get there

            return View(new PostsShow
            {
                Post = post
            });

        }

        private System.Tuple<int, string> SeparateIdAndSlug(string idAndSlug)
        {
            // Regex: from start of string: 0 to many digits, then dash zero to many times
            var matches = Regex.Match(idAndSlug, @"^(\d+)\-(.*)?$");
            if (!matches.Success)
            {
                return null;

            }
            var id = int.Parse(matches.Result("$1")); // get first group
            var slug = matches.Result("$2"); // get 2nd...

            return Tuple.Create(id, slug);
        }
    }
}