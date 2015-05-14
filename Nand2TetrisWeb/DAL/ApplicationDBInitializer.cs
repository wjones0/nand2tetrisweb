using Contracts;
using Microsoft.AspNet.Identity;
using Nand2TetrisWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Nand2TetrisWeb.DAL
{
    public class ApplicationDBInitializer : DropCreateDatabaseAlways<ApplicationDbContext> //CreateDatabaseIfNotExists<ApplicationDbContext>  //
    {
        protected override void Seed(ApplicationDbContext context)
        {

            /******************************************************* users ***************************************/

            // testing user
            var passwordHash = new PasswordHasher();
            string password = passwordHash.HashPassword(ConfigurationManager.AppSettings["TestingUserPW"]);
            context.Users.Add(new ApplicationUser()
            {
                UserName = ConfigurationManager.AppSettings["TestingUserName"],
                PasswordHash = password,
                Email = ConfigurationManager.AppSettings["TestingUserName"],
                SecurityStamp = Guid.NewGuid().ToString()
            });

            context.SaveChanges();


            var tun = ConfigurationManager.AppSettings["TestingUserName"];
            var testinguser  = (from u in context.Users
                                where u.Email == tun
                       select u).FirstOrDefault();

            //  bob
            password = passwordHash.HashPassword("bobobo");
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


            /************************************************** hdl files *************************************/
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



            // Not16
            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Not16.hdl",
                FileBody = @"// This file is part of www.nand2tetris.org
// and the book The Elements of Computing Systems
// by Nisan and Schocken, MIT Press.
// File name: projects/01/Not16.hdl

/**
 * 16-bit Not:
 * for i=0..15: out[i] = not in[i]
 */

CHIP Not16 {
    IN in[16];
    OUT out[16];

    PARTS:
    Not (in=in[0],out=out[0]);
    Not (in=in[1],out=out[1]);
    Not (in=in[2],out=out[2]);
    Not (in=in[3],out=out[3]);
    Not (in=in[4],out=out[4]);
    Not (in=in[5],out=out[5]);
    Not (in=in[6],out=out[6]);
    Not (in=in[7],out=out[7]);
    Not (in=in[8],out=out[8]);
    Not (in=in[9],out=out[9]);
    Not (in=in[10],out=out[10]);
    Not (in=in[11],out=out[11]);
    Not (in=in[12],out=out[12]);
    Not (in=in[13],out=out[13]);
    Not (in=in[14],out=out[14]);
    Not (in=in[15],out=out[15]);
}
",
                userid = new Guid(bob.Id)
            });


/************************************** test file seed **********************************/
            // Not
            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Not.tst",
                FileBody = @"// This file is part of www.nand2tetris.org
// and the book The Elements of Computing Systems
// by Nisan and Schocken, MIT Press.
// File name: projects/01/Not.tst

load Not.hdl,
output-file Not.out,
compare-to Not.cmp,
output-list in%B3.1.3 out%B3.1.3;

set in 0,
eval,
output;

set in 1,
eval,
output;

",
                userid = new Guid(testinguser.Id)
            });


            // And

            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "And.tst",
                FileBody = @"// This file is part of www.nand2tetris.org
// and the book The Elements of Computing Systems
// by Nisan and Schocken, MIT Press.
// File name: projects/01/And.tst

load And.hdl,
output-file And.out,
compare-to And.cmp,
output-list a%B3.1.3 b%B3.1.3 out%B3.1.3;

set a 0,
set b 0,
eval,
output;

set a 0,
set b 1,
eval,
output;

set a 1,
set b 0,
eval,
output;

set a 1,
set b 1,
eval,
output;

",
                userid = new Guid(testinguser.Id)
            });


            // Or

            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Or.tst",
                FileBody = @"// This file is part of www.nand2tetris.org
// and the book The Elements of Computing Systems
// by Nisan and Schocken, MIT Press.
// File name: projects/01/Or.tst

load Or.hdl,
output-file Or.out,
compare-to Or.cmp,
output-list a%B3.1.3 b%B3.1.3 out%B3.1.3;

set a 0,
set b 0,
eval,
output;

set a 0,
set b 1,
eval,
output;

set a 1,
set b 0,
eval,
output;

set a 1,
set b 1,
eval,
output;

",
                userid = new Guid(testinguser.Id)
            });




            // Xor

            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Xor.tst",
                FileBody = @"// This file is part of www.nand2tetris.org
// and the book The Elements of Computing Systems
// by Nisan and Schocken, MIT Press.
// File name: projects/01/Xor.tst

load Xor.hdl,
output-file Xor.out,
compare-to Xor.cmp,
output-list a%B3.1.3 b%B3.1.3 out%B3.1.3;

set a 0,
set b 0,
eval,
output;

set a 0,
set b 1,
eval,
output;

set a 1,
set b 0,
eval,
output;

set a 1,
set b 1,
eval,
output;

",
                userid = new Guid(testinguser.Id)
            });



            // Mux

            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Mux.tst",
                FileBody = @"// This file is part of www.nand2tetris.org
// and the book The Elements of Computing Systems
// by Nisan and Schocken, MIT Press.
// File name: projects/01/Mux.tst

load Mux.hdl,
output-file Mux.out,
compare-to Mux.cmp,
output-list a%B3.1.3 b%B3.1.3 sel%B3.1.3 out%B3.1.3;

set a 0,
set b 0,
set sel 0,
eval,
output;

set sel 1,
eval,
output;

set a 0,
set b 1,
set sel 0,
eval,
output;

set sel 1,
eval,
output;

set a 1,
set b 0,
set sel 0,
eval,
output;

set sel 1,
eval,
output;

set a 1,
set b 1,
set sel 0,
eval,
output;

set sel 1,
eval,
output;

",
                userid = new Guid(testinguser.Id)
            });


            // DMux

            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "DMux.tst",
                FileBody = @"// This file is part of www.nand2tetris.org
// and the book The Elements of Computing Systems
// by Nisan and Schocken, MIT Press.
// File name: projects/01/DMux.tst

load DMux.hdl,
output-file DMux.out,
compare-to DMux.cmp,
output-list in%B3.1.3 sel%B3.1.3 a%B3.1.3 b%B3.1.3;

set in 0,
set sel 0,
eval,
output;

set sel 1,
eval,
output;

set in 1,
set sel 0,
eval,
output;

set sel 1,
eval,
output;

",
                userid = new Guid(testinguser.Id)
            });


            // Not16

            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Not16.tst",
                FileBody = @"// This file is part of www.nand2tetris.org
// and the book The Elements of Computing Systems
// by Nisan and Schocken, MIT Press.
// File name: projects/01/Not16.tst

load Not16.hdl,
output-file Not16.out,
compare-to Not16.cmp,
output-list in%B1.16.1 out%B1.16.1;

set in %B0000000000000000,
eval,
output;

set in %B1111111111111111,
eval,
output;

set in %B1010101010101010,
eval,
output;

set in %B0011110011000011,
eval,
output;

set in %B0001001000110100,
eval,
output;
",
                userid = new Guid(testinguser.Id)
            });



            /************************************** compare file seed **********************************/
            // Not
            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Not.cmp",
                FileBody = @"|  in   |  out  |
|   0   |   1   |
|   1   |   0   |

",
                userid = new Guid(testinguser.Id)
            });


            // And

            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "And.cmp",
                FileBody = @"|   a   |   b   |  out  |
|   0   |   0   |   0   |
|   0   |   1   |   0   |
|   1   |   0   |   0   |
|   1   |   1   |   1   |

",
                userid = new Guid(testinguser.Id)
            });


            // Or

            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Or.cmp",
                FileBody = @"|   a   |   b   |  out  |
|   0   |   0   |   0   |
|   0   |   1   |   1   |
|   1   |   0   |   1   |
|   1   |   1   |   1   |

",
                userid = new Guid(testinguser.Id)
            });




            // Xor

            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Xor.cmp",
                FileBody = @"|   a   |   b   |  out  |
|   0   |   0   |   0   |
|   0   |   1   |   1   |
|   1   |   0   |   1   |
|   1   |   1   |   0   |

",
                userid = new Guid(testinguser.Id)
            });



            // Mux

            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Mux.cmp",
                FileBody = @"|   a   |   b   |  sel  |  out  |
|   0   |   0   |   0   |   0   |
|   0   |   0   |   1   |   0   |
|   0   |   1   |   0   |   0   |
|   0   |   1   |   1   |   1   |
|   1   |   0   |   0   |   1   |
|   1   |   0   |   1   |   0   |
|   1   |   1   |   0   |   1   |
|   1   |   1   |   1   |   1   |

",
                userid = new Guid(testinguser.Id)
            });


            // DMux

            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "DMux.cmp",
                FileBody = @"|  in   |  sel  |   a   |   b   |
|   0   |   0   |   0   |   0   |
|   0   |   1   |   0   |   0   |
|   1   |   0   |   1   |   0   |
|   1   |   1   |   0   |   1   |

",
                userid = new Guid(testinguser.Id)
            });



            // Not16

            context.SourceFiles.Add(new SourceFile()
            {
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now,
                FileName = "Not16.cmp",
                FileBody = @"|        in        |       out        |
| 0000000000000000 | 1111111111111111 |
| 1111111111111111 | 0000000000000000 |
| 1010101010101010 | 0101010101010101 |
| 0011110011000011 | 1100001100111100 |
| 0001001000110100 | 1110110111001011 |

",
                userid = new Guid(testinguser.Id)
            });

        
        }








    }
}