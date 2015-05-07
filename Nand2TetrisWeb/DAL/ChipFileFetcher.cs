using ChipProcessing;
using Nand2TetrisWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nand2TetrisWeb.DAL
{
    public class ChipFileFetcher
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Guid _userID;       

        public ChipFileFetcher(Guid UserID)
        {
            _userID = UserID;
        }

        public Dictionary<string,string> FetchFiles()
        {
            var files = (from f in db.SourceFiles
                        where ((f.userid == _userID) && (f.FileName.Contains(".hdl")))
                        select f).ToList();

            var fileContents = new Dictionary<string, string>();

            foreach(var f in files)
            {
                fileContents.Add(f.FileName.Replace(".hdl",""), f.FileBody);
            }



            return fileContents;
            
        }
    }
}