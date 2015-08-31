using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;
using SimpleBlog.Areas.Admin.ViewModels;
using SimpleBlog.Infrastructure;
using SimpleBlog.Infrastructure.Extensions;
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
                IsNew = true,
                Tags = Database.Session.Query<Tag>().Select(tag => new TagCheckBox
                {
                   Id = tag.Id,
                   Name = tag.Name,
                   IsChecked = false
                }).ToList()

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

            var selectedTags = ReconsileTags(form.Tags).ToList();

            Post post;
            if (form.IsNew) // if new post
            {
                post = new Post
                {
                    CreatedAt = DateTime.UtcNow,
                    User = Auth.User
                };

                foreach (var tag in selectedTags)
                {
                    post.Tags.Add(tag);
                }
            }
            else // if existing post
            {
                post = Database.Session.Load<Post>(form.PostId);

                if (post == null) return HttpNotFound();

                post.UpdatedAt = DateTime.UtcNow;

                foreach (var toAdd in selectedTags.Where(t => !post.Tags.Contains(t)))
                {
                    post.Tags.Add(toAdd);
                }

                foreach (var toRemove in post.Tags.Where(t => !selectedTags.Contains(t)).ToList())
                {
                    post.Tags.Add(toRemove);
                }

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
                Title = post.Title,
                Tags = Database.Session.Query<Tag>().Select(tag => new TagCheckBox
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    IsChecked = post.Tags.Contains(tag)
                }).ToList()
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

        /// <summary>
        /// Loop through tags selected. Tags with Ids return them from DB
        /// Tags no Ids check if that tags already exist...if yes return it form DB
        /// If the tags has no ID and not exists will be created , added to DB, then returned
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        private IEnumerable<Tag> ReconsileTags(IEnumerable<TagCheckBox> tags)
        {
            foreach (var tag in tags.Where(t => t.IsChecked))
            {
                if (tag.Id != null)
                {
                    yield return Database.Session.Load<Tag>(tag.Id);
                    continue;
                }

                var existingTag = Database.Session.Query<Tag>().FirstOrDefault(t => t.Name == tag.Name);

                // existingTag is found in DB so use the one in DB
                if (existingTag != null)
                {
                    yield return existingTag;
                    continue;
                }

                var newTag = new Tag
                {
                    Name = tag.Name,
                    Slug = tag.Name.Slugify()
                };

                Database.Session.Save(newTag);
                yield return newTag;

            }            
        }
    }
}