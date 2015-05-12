using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Contracts;
using Nand2TetrisWeb.Models;
using System.Configuration;

namespace Nand2TetrisWeb.Controllers
{
    public class SourceFilesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SourceFiles
        public IHttpActionResult GetSourceFiles()
        {
            var userid = GetUserID();
            var testingUserID = GetTestingUserID();
            return Json((from f in db.SourceFiles
                             where ((f.userid == userid) || (f.userid == testingUserID))
                             select f).ToList());
        }

        // GET: api/SourceFiles/5
        [ResponseType(typeof(SourceFile))]
        public IHttpActionResult GetSourceFile(int id)
        {
            SourceFile sourceFile = db.SourceFiles.Find(id);
            if (sourceFile == null || (sourceFile.userid != GetUserID() && sourceFile.userid != GetTestingUserID()))
            {
                return NotFound();
            }

            return Json(new SourceFile() {CreateDate = sourceFile.CreateDate, FileBody = sourceFile.FileBody, minFileBody = ChipProcessing.ChipParser.RemoveAllComments(sourceFile.FileBody), 
                                                FileName = sourceFile.FileName, id = sourceFile.id, ModifyDate = sourceFile.ModifyDate, userid = sourceFile.userid});
        }
        
        // PUT: api/SourceFiles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSourceFile(int id, [FromBody] SourceFile sourceFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sf = (from f in db.SourceFiles
                      where f.id == id
                      select f).FirstOrDefault();

            if (id != sourceFile.id || sf.userid != GetUserID())
            {
                return BadRequest();
            }

            
            sf.FileBody = sourceFile.FileBody;
            sf.ModifyDate = DateTime.Now;

            db.Entry(sf).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SourceFileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/SourceFiles
        [ResponseType(typeof(SourceFile))]
        [HttpPost]
        public IHttpActionResult PostSourceFile(SourceFile sourceFile)
        {

            sourceFile.CreateDate = DateTime.Now;
            sourceFile.ModifyDate = DateTime.Now;
            sourceFile.userid = GetUserID();


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            db.SourceFiles.Add(sourceFile);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sourceFile.id }, sourceFile);
        }
        /*
        // DELETE: api/SourceFiles/5
        [ResponseType(typeof(SourceFile))]
        public IHttpActionResult DeleteSourceFile(int id)
        {
            SourceFile sourceFile = db.SourceFiles.Find(id);
            if (sourceFile == null)
            {
                return NotFound();
            }

            db.SourceFiles.Remove(sourceFile);
            db.SaveChanges();

            return Ok(sourceFile);
        }
        */
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SourceFileExists(int id)
        {
            return db.SourceFiles.Count(e => e.id == id) > 0;
        }

        private Guid GetUserID()
        {
            var userId = (from u in db.Users
                          where u.UserName == User.Identity.Name
                          select u.Id).FirstOrDefault();
            return new Guid(userId);
        }

        private Guid GetTestingUserID()
        {
            var un = ConfigurationManager.AppSettings["TestingUserName"];
            var userId = (from u in db.Users
                          where u.UserName == un
                          select u.Id).FirstOrDefault();
            return new Guid(userId);
        }
    }
}