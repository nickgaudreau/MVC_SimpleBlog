using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace SimpleBlog.Migrations
{
    [Migration(1)]
    public class _001_UserAndRoles : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("username").AsString(128)
                .WithColumn("email").AsString(256)
                .WithColumn("password_hash").AsString(128);

            Create.Table("roles")
                .WithColumn("id").AsInt32().Identity().PrimaryKey()
                .WithColumn("name").AsString(128);

            // Foreign Key make suer that the id exists in users. OnDelete so if an id form users get deleted the corresponding role_users get deleted
            Create.Table("role_users")
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id").OnDelete(Rule.Cascade)
                .WithColumn("role_id").AsInt32().ForeignKey("roles", "id").OnDelete(Rule.Cascade);
            // Foreign Key make sure that the id exists in roles. OnDelete so if an id of roles get deleted the correspondings role_users get deleted
        }

        public override void Down()
        {
            Delete.Table("role_users"); // must be delete first since it has foreign key constraint!
            Delete.Table("roles");
            Delete.Table("users");
        }
    }
}