using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChipProcessing
{
    public class Chip
    {

        private Dictionary<string, string> _inputs, _outputs, _intermediates;
        private Dictionary<Chip, Dictionary<string, string>> _partList;
        protected bool _finishedProcessing;


        public Chip(List<string> inputs, List<string> outputs)
        {
            _inputs = new Dictionary<string, string>();
            _outputs = new Dictionary<string, string>();
            _intermediates = new Dictionary<string, string>();
            _partList = new Dictionary<Chip, Dictionary<string, string>>();

            foreach (var i in inputs)
                _inputs[i] = null;

            foreach (var o in outputs)
                _outputs[o] = null;

            _finishedProcessing = false;

        }

        public Dictionary<string, string> Inputs { get { return _inputs; } }

        public Dictionary<string, string> Outputs { get { return _outputs; } }

        public Dictionary<string, string> Intermediates { get { return _intermediates; } }



        public virtual bool ProcessChip()
        {

            if (_finishedProcessing)
                return false;

            // go through each chip wiring up inputs
            wireInputs();

            bool processed;

            do
            {
                processed = false;

                wireIntermediates();

                // Process all chips...
                foreach (var c in _partList.Keys)
                {
                    processed = c.ProcessChip() || processed;
                }

                // Get output
                wireOutputs();

            } while (!outputsDefined() && processed);


            return outputsDefined();
        }

        private void wireIntermediates()
        {
            foreach (var c in _partList.Keys)
            {
                var workingWiring = _partList[c];
                // each input in a chip needs its value updated
                var tempKeys = (from tk in new List<string>(c.Inputs.Keys)
                                where c.Inputs[tk] == null
                                select tk).ToList();
                foreach (var i in tempKeys)
                {
                    var outerValue = workingWiring[i];
                    if (Intermediates.ContainsKey(outerValue))
                        c.Inputs[i] = Intermediates[outerValue];
                }

            }
        }

        private bool outputsDefined()
        {
            return !Outputs.ContainsValue(null);
        }

        private void wireOutputs()
        {
            foreach (var c in _partList.Keys)
            {
                var workingWiring = _partList[c];

                var tempKeys = new List<string>(c.Outputs.Keys);

                foreach (var o in tempKeys)
                {
                    var inValue = workingWiring[o];
                    if (Outputs.ContainsKey(inValue))
                    {
                        Outputs[inValue] = c.Outputs[o];
                    }
                    Intermediates[inValue] = c.Outputs[o];
                }
            }
        }

        private void wireInputs()
        {
            foreach (var c in _partList.Keys)
            {
                var workingWiring = _partList[c];
                // each input in a chip needs its value updated
                var tempKeys = new List<string>(c.Inputs.Keys);
                foreach (var i in tempKeys)
                {
                    var outerValue = workingWiring[i];
                    if(Inputs.ContainsKey(outerValue))
                        c.Inputs[i] = Inputs[outerValue];
                }

            }
        }



        public void AddChip(Chip c, string p)
        {
            Dictionary<string, string> wiring = new Dictionary<string, string>();
            var separateWires = p.Split(',').ToList<string>();

            foreach (var w in separateWires)
            {
                var split = w.Split('=');
                wiring[split[0]] = split[1];
            }

            _partList.Add(c, wiring);
        }

    }
}
