using Contracts;
using Nand2TetrisWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Nand2TetrisWeb.DAL
{
    public class ApplicationDBInitializer :  DropCreateDatabaseAlways<ApplicationDbContext> //CreateDatabaseIfNotExists<ApplicationDbContext>  //
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var srcFile = new SourceFile() { FileName="test", FileBody="testBody", CreateDate=DateTime.Now, ModifyDate=DateTime.Now };
            context.SourceFiles.Add(srcFile);
            context.SaveChanges();
        }
    }
}