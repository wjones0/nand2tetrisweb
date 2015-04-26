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


        public Chip(List<string> inputs, List<string> outputs)
        {
            _inputs = new Dictionary<string, string>();
            _outputs = new Dictionary<string, string>();

            foreach (var i in inputs)
                _inputs[i] = null;

            foreach (var o in outputs)
                _outputs[o] = null;


        }

        public Dictionary<string, string> Inputs { get { return _inputs; } }

        public Dictionary<string, string> Outputs { get { return _outputs; } }

        public virtual void ProcessChip()
        {

        }


    }
}
