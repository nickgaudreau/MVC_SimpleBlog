using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SimpleBlog.Models
{
    /// <summary>
    /// C# version of our MsSQL user Tbale
    /// </summary>
    public class User
    {
        // MUST BE VIRTUAL KEYWORDED
        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string PasswordHash { get; set; }

        public virtual void SetPassword(string password)
        {
            PasswordHash = "Filler for now..";
        }
    }

    // loads of way to do the mapping here is 1....

    public class UserMap : ClassMapping<User>
    {
        public UserMap()
        {
            Table("users");

            Id(x => x.Id, x => x.Generator(Generators.Identity));
            
            Property(x => x.Username, x => x.NotNullable(true));
            Property(x => x.Email, x => x.NotNullable(true));
            // lamba use {for multiple properties}
            Property(x => x.PasswordHash, x =>
            {
                // here must point exact db column name
                x.Column("password_hash");
                x.NotNullable(true);

            });
        }
    }
}