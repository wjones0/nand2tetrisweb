using ChipProcessing;
using Contracts;
using Nand2TetrisWeb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var file = (from f in db.SourceFiles
                        where f.id == id
                        select f).FirstOrDefault();

            if (file==null || file.userid != GetUserID())
                return Json(new List<string>());

            var ch = ChipParser.Parse(file.FileBody);
            if (ch == null)
                return Json(new List<string>());

            var pf = new ProcessedFile();
            pf.inputs = ch.Inputs.Keys.ToList<string>();
            pf.outputs = ch.Outputs.Keys.ToList<string>();


            return Json(pf);
        }



        public JsonResult ProcessChip(string fileid, string[] inputIDs, string[] inputVals)
        {
            var file = (from f in db.SourceFiles
                        where f.id.ToString() == fileid
                        select f).FirstOrDefault();

            if (file == null || file.userid != GetUserID())
                return Json(new List<string>());

            var ch = ChipParser.Parse(file.FileBody);

            if(ch==null)
                return Json(new List<string>());

            for (int i = 0; i < inputIDs.Length; i++)
            {
                ch.Inputs[inputIDs[i]] = inputVals[i];
            }

            ch.ProcessChip();

            return Json(JsonConvert.SerializeObject( ch.Outputs ));
        }




        private Guid GetUserID()
        {
            var userId = (from u in db.Users
                          where u.UserName == User.Identity.Name
                          select u.Id).FirstOrDefault();
            return new Guid(userId);
        }



    }
}