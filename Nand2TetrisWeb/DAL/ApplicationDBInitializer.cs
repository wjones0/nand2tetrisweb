using Contracts;
using Microsoft.AspNet.Identity;
using Nand2TetrisWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Nand2TetrisWeb.DAL
{
    public class ApplicationDBInitializer : DropCreateDatabaseAlways<ApplicationDbContext> //CreateDatabaseIfNotExists<ApplicationDbContext>  //
    {
        protected override void Seed(ApplicationDbContext context)
        {

            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword("bobobo");
            context.Users.Add(new ApplicationUser()
                {
                    UserName = "bob@bob.com",
                    PasswordHash = password,
                    Email = "bob@bob.com",
                    SecurityStamp = Guid.NewGuid().ToString()
                });

            context.SaveChanges();

            var bob = (from u in context.Users
                           where u.Email == "bob@bob.com"
                           select u).FirstOrDefault();

            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Nand.hdl",
                FileBody = "/* Nand file \n */",
                userid = new Guid(bob.Id)
            });
            


        }
    }
}