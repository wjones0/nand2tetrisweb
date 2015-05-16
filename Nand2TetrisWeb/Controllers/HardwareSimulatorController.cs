using ChipProcessing;
using Contracts;
using Nand2TetrisWeb.DAL;
using Nand2TetrisWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Nand2TetrisWeb.Controllers
{
    public class HardwareSimulatorController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HardwareSimulator
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult ParseFile(int id)
        {
            
            var ch = CreateChip(id);

            if (ch == null)
                return Json(new List<string>());

            var pf = new ProcessedFile();
            pf.inputs = ch.Inputs.Keys.ToList<string>();
            pf.outputs = ch.Outputs.Keys.ToList<string>();


            return Json(pf);
        }


        public JsonResult FindFileByName(string fileName)
        {
            var un = GetUserID();
            var tun = GetTestingUserID();

            var file = (from f in db.SourceFiles
                            where (f.FileName == fileName && (f.userid == un || f.userid == tun))
                            select f).FirstOrDefault();
            if (file == null)
                return Json(HttpNotFound());

            return Json (new { id = file.id });
        }


        public JsonResult ProcessChip(string fileid, string[] inputIDs, string[] inputVals)
        {
            var ch = CreateChip(Int32.Parse(fileid));

            if(ch==null)
                return Json(new List<string>());

            var inputDict = new Dictionary<string, string>();

            for (int i = 0; i < inputIDs.Length; i++)
            {
                var inp = Regex.Replace(inputIDs[i], @"\[[\d.]+\]", ""); 
                ch.Inputs[inp] = inputVals[i];
                ch.Intermediates[inp] = inputVals[i];
                inputDict[inp] = inputVals[i];
            }

            ch.ProcessChip(inputDict);

            return Json(JsonConvert.SerializeObject( ch.Outputs ));
        }



        private Chip CreateChip(int id)
        {
            var file = (from f in db.SourceFiles
                        where f.id == id
                        select f).FirstOrDefault();

            if (file == null || file.userid != GetUserID())
                return null;

            var sourcefiles = new ChipFileFetcher(GetUserID());

            return ChipParser.Parse(file.FileBody, sourcefiles.FetchFiles());
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