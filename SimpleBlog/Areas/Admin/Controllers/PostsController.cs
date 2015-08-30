using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using SimpleBlog.Areas.Admin.ViewModels;
using SimpleBlog.Infrastructure;
using SimpleBlog.Models;

namespace SimpleBlog.Areas.Admin.Controllers
{

    //Set roles - Admin: not accessible to non-authorized nor non-admin users
    [Authorize(Roles = "admin")]
    [SelectedTab("Posts")]
    public class PostsController : Controller
    {
        private const int PostsPerPage = 5;


        // GET: Admin/Posts ... accept one param int and deault to one if not specifed => int page = 1
        public ActionResult Index(int page = 1)
        {
            var totalPostCount = Database.Session.Query<Post>().Count();

            var currentPostPage = Database.Session.Query<Post>()
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * PostsPerPage)
                .Take(PostsPerPage)
                .ToList();


            return View(new PostsIndex
            {
                Posts = new PagedData<Post>(currentPostPage, totalPostCount, page, PostsPerPage)
            });
        }

        public ActionResult New()
        {
            return View("Form", new PostsForm
            {
                IsNew = true
            });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Form(PostsForm form)
        {
            // if form id is null then it is new
            form.IsNew = form.PostId == null;

            if (!ModelState.IsValid)
            {
                return View(form);
            }

            Post post;
            if (form.IsNew) // if new post
            {
                post = new Post
                {
                    CreatedAt = DateTime.UtcNow,
                    User = Auth.User
                };
            }
            else // if existing post
            {
                post = Database.Session.Load<Post>(form.PostId);

                if (post == null) return HttpNotFound();

                post.UpdatedAt = DateTime.UtcNow;

            }

            post.Title = form.Title;
            post.Slug = form.Slug;
            post.Content = form.Content;

            // this SaveOrUpdate either insert or update
            Database.Session.SaveOrUpdate(post);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var post = Database.Session.Load<Post>(id);

            System.Diagnostics.Debug.WriteLine("test if nulll" + post.Title);

            if (post == null)
                return HttpNotFound();

            return View("Form", new PostsForm
            {
                IsNew = false,
                PostId = id,
                Content = post.Content,
                Slug = post.Slug,
                Title = post.Title
            });

        }

        
        public ActionResult Trash(int id)
        {
            var post = Database.Session.Load<Post>(id);

            System.Diagnostics.Debug.WriteLine("test trash : " + post.Title);

            if (post == null)
            {
                return HttpNotFound();
            }

            post.DeleteAt = DateTime.UtcNow;
            Database.Session.Update(post);
            return RedirectToAction("Index");
        }

        
        public ActionResult Delete(int id)
        {
            var post = Database.Session.Load<Post>(id);

            System.Diagnostics.Debug.WriteLine("test delete: " + post.Title);

            if (post == null)
            {
                return HttpNotFound();
            }

            Database.Session.Delete(post);
            return RedirectToAction("Index");
        }

        
        public ActionResult Restore(int id)
        {
            var post = Database.Session.Load<Post>(id);

            System.Diagnostics.Debug.WriteLine("test if Restore: " + post.Title);

            if (post == null)
            {
                return HttpNotFound();
            }

            post.DeleteAt = null;
            Database.Session.Update(post);
            return RedirectToAction("Index");
        }
    }
}