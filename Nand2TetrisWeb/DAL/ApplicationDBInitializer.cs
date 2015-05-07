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


            //  NOT
            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Not.hdl",
                FileBody = @"/**
 * Not gate:
 * out = not in
 */

CHIP Not {
    IN in;
    OUT out;

    PARTS:
    Nand (a=in,b=in,out=out);
}",
                userid = new Guid(bob.Id)
            });



            // AND
            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "And.hdl",
                FileBody = @"// This file is part of www.nand2tetris.org
// and the book The Elements of Computing Systems
// by Nisan and Schocken, MIT Press.
// File name: projects/01/And.hdl

/**
 * And gate: 
 * out = 1 if (a == 1 and b == 1)
 *       0 otherwise
 */

CHIP And {
    IN a, b;
    OUT out;

    PARTS:
    Nand (a=a,b=b,out=aAndb);
	Not(in=aAndb,out=out);
}
",
                userid = new Guid(bob.Id)
            });



            // OR
            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Or.hdl",
                FileBody = @"// This file is part of www.nand2tetris.org
// and the book The Elements of Computing Systems
// by Nisan and Schocken, MIT Press.
// File name: projects/01/Or.hdl

 /**
 * Or gate:
 * out = 1 if (a == 1 or b == 1)
 *       0 otherwise
 */

CHIP Or {
    IN a, b;
    OUT out;

    PARTS:
    Not (in=a,out=notA);
	Not (in=b,out=notB);
	Nand (a=notA,b=notB,out=out);
	//Not (in=notAandnotB,out=out);
}
",
                userid = new Guid(bob.Id)
            });



            //  XOR
            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Xor.hdl",
                FileBody = @"// This file is part of www.nand2tetris.org
// and the book The Elements of Computing Systems
// by Nisan and Schocken, MIT Press.
// File name: projects/01/Xor.hdl

/**
 * Exclusive-or gate:
 * out = not (a == b)
 */

CHIP Xor {
    IN a, b;
    OUT out;

    PARTS:
    And (a=a,b=b,out=aAndb);
	Not (in=a,out=notA);
	Not (in=b,out=notB);
	And (a=notA,b=notB,out=notAandnotB);
	Or (a=aAndb,b=notAandnotB,out=aAndbornotAandnotB);
	Not (in=aAndbornotAandnotB,out=out);
}",
                userid = new Guid(bob.Id)
            });



            //   Mux
            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Mux.hdl",
                FileBody = @"// This file is part of www.nand2tetris.org
// and the book The Elements of Computing Systems
// by Nisan and Schocken, MIT Press.
// File name: projects/01/Mux.hdl

/** 
 * Multiplexor:
 * out = a if sel == 0
 *       b otherwise
 */

CHIP Mux {
    IN a, b, sel;
    OUT out;

    PARTS:
    Not (in=sel,out=notSel);
	And (a=a,b=notSel,out=AnotSel);
	And (a=b,b=sel,out=BandSel);
	Or (a=AnotSel,b=BandSel,out=out);
}",
                userid = new Guid(bob.Id)
            });



            // DMux
            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "DMux.hdl",
                FileBody = @"// This file is part of www.nand2tetris.org
// and the book The Elements of Computing Systems
// by Nisan and Schocken, MIT Press.
// File name: projects/01/DMux.hdl

/**
 * Demultiplexor:
 * {a, b} = {in, 0} if sel == 0
 *          {0, in} if sel == 1
 */

CHIP DMux {
    IN in, sel;
    OUT a, b;

    PARTS:
    Not (in=sel,out=notSel);
	And (a=in,b=notSel,out=a);
	And (a=in,b=sel,out=b);
}
",
                userid = new Guid(bob.Id)
            });

        }
    }
}