using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipProcessing
{
    public class Nand : Chip
    {

        public Nand()
            : base(new List<string>() { "a", "b" }, new List<string>() { "out" })
        {


        }

        public override void ProcessChip()
        {
            if (Inputs["a"] == "0")
            {
                if (Inputs["b"] == "0")
                    Outputs["out"] = "1";
                else
                    if (Inputs["b"] == "1")
                        Outputs["out"] = "0";
                    else
                        Outputs["out"] = "error";
            }
            else if (Inputs["a"] == "1")
            {
                if (Inputs["b"] == "0")
                    Outputs["out"] = "0";
                else
                    if (Inputs["b"] == "1")
                        Outputs["out"] = "0";
                    else
                        Outputs["out"] = "error";
            }
            else
                this.Outputs["out"] = "error";
        }
    }
}
