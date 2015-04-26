using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipProcessing
{
    public class Chip
    {

        private Dictionary<string, string> _inputs, _outputs;
        private Dictionary<Chip,Dictionary<string, string>> _partList;


        public Chip(List<string> inputs, List<string> outputs)
        {
            _inputs = new Dictionary<string, string>();
            _outputs = new Dictionary<string, string>();
            _partList = new Dictionary<Chip, Dictionary<string, string>>();

            foreach (var i in inputs)
                _inputs[i] = null;

            foreach (var o in outputs)
                _outputs[o] = null;


        }

        public Dictionary<string, string> Inputs { get { return _inputs; } }

        public Dictionary<string, string> Outputs { get { return _outputs; } }



        public virtual void ProcessChip()
        {
            // this only works for single level chips for now i think.

            // go through each chip wiring up inputs
            foreach(var c in _partList.Keys)
            {
                var workingWiring = _partList[c];
                // each input in a chip needs its value updated
                var tempKeys = new List<string>(c.Inputs.Keys);
                foreach(var i in tempKeys)
                {
                    var outerValue = workingWiring[i];
                    c.Inputs[i] = Inputs[outerValue];
                }

            }

            // Process all chips...
            foreach(var c in _partList.Keys)
            {
                c.ProcessChip();
            }

            // Get output
            foreach(var c in _partList.Keys)
            {
                var workingWiring = _partList[c];

                var tempKeys = new List<string>(Outputs.Keys);
                
                foreach(var o in tempKeys)
                {
                    if(workingWiring.ContainsValue(o))
                    {
                        var outKey = workingWiring.FirstOrDefault(x => x.Value == o).Key;
                        Outputs[o] = c.Outputs[outKey];
                    }
                }
                
            }

        }



        public void AddChip(Chip c, string p)
        {
            Dictionary<string,string> wiring = new Dictionary<string,string>();
            var separateWires = p.Split(',').ToList<string>();
            
            foreach(var w in separateWires)
            {
                var split = w.Split('=');
                wiring[split[0]] = split[1];
            }

            _partList.Add(c,wiring);
        }

    }
}
