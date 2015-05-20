using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Contracts;
using Assembler;

namespace Nand2TetrisWeb.Controllers
{
    public class AssemblerController : Controller
    {
        // GET: Assembler
        public ActionResult Index()
        {
            return View();
        }



        public JsonResult Assemble(string asmText)
        {
            HackAssembler hack = new HackAssembler(asmText);

            return Json(new AssembledFile() { asmText = hack.AsmText, hackText = hack.HackText });
        }
    }
}