using System;
using System.Collections.Generic;
using NHibernate.Mapping;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SimpleBlog.Models
{
    public class Post
    {

        public virtual int Id { get; set; }
        public virtual User User { get; set; } // map post to user

        public virtual string Title { get; set; }
        public virtual string Slug { get; set; } // specialize title to use with url
        public virtual string Content { get; set; }

        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        /// <summary>
        /// soft deletion ... mark it as deleted but we actually keep it in the DB for possible future reference
        /// </summary>
        public virtual DateTime? DeleteAt { get; set; } 

        public virtual bool IsDeleted {
            get { return DeleteAt != null; }
        }

        public virtual IList<Tag> Tags { get; set; } 
        
    }

    public class PostMap : ClassMapping<Post>
    {
        public PostMap()
        {
            Table("posts");

            Id(x => x.Id, x => x.Generator(Generators.Identity));

            // Map user:  many to 1 between post and user
            ManyToOne(x => x.User, x =>
            {
                x.Column("user_id");
                x.NotNullable(true);
            });

            Property(x => x.Title, x => x.NotNullable(true));
            Property(x => x.Slug, x => x.NotNullable(true));
            Property(x => x.Content, x => x.NotNullable(true));

            Property(x => x.CreatedAt, x =>
            {
                x.Column("created_at");
                x.NotNullable(true);
            });

            Property(x => x.UpdatedAt, x => x.Column("updated_at"));
            Property(x => x.DeleteAt, x => x.Column("deleted_at"));

            // link IList tags to our posts
            Bag(x => x.Tags, x =>
            {
                x.Key(y => y.Column("post_id"));
                x.Table("post_tags");

            }, x => x.ManyToMany(y => y.Column("tag_id")));
        }
        
    }
}