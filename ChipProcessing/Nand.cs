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
            : base("Nand",new List<string>() { "a", "b" }, new List<string>() { "out" })
        {


        }

        public override bool ProcessChip(Dictionary<string, string> inputValues)
        {
            if (_finishedProcessing)
                return false;

            wireInputs(inputValues);

            if (Inputs["a"] == "0")
            {
                if (Inputs["b"] == "0")
                    Outputs["out"] = "1";
                else
                    if (Inputs["b"] == "1")
                        Outputs["out"] = "1";
            }
            else if (Inputs["a"] == "1")
            {
                if (Inputs["b"] == "0")
                    Outputs["out"] = "1";
                else
                    if (Inputs["b"] == "1")
                        Outputs["out"] = "0";
            }
            else
            {
                return false;
            }

            _finishedProcessing = true;
            return true;
        }
    }
}
