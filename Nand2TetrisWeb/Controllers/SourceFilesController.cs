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
//using System.Web.Mvc;

namespace Nand2TetrisWeb.Controllers
{
    public class SourceFilesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SourceFiles
        public IHttpActionResult GetSourceFiles() 
        {
            return Json(db.SourceFiles);
        }

        // GET: api/SourceFiles/5
        [ResponseType(typeof(SourceFile))]
        public IHttpActionResult GetSourceFile(int id)
        {
            SourceFile sourceFile = db.SourceFiles.Find(id);
            if (sourceFile == null)
            {
                return NotFound();
            }

            return Json(sourceFile);
        }
        /*
        // PUT: api/SourceFiles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSourceFile(int id, SourceFile sourceFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sourceFile.id)
            {
                return BadRequest();
            }

            db.Entry(sourceFile).State = EntityState.Modified;

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
        public IHttpActionResult PostSourceFile(SourceFile sourceFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SourceFiles.Add(sourceFile);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = sourceFile.id }, sourceFile);
        }

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
    }
}