using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChipProcessing
{
    public class Chip
    {

        private Dictionary<string, string> _inputs, _outputs, _intermediates;
        //private Dictionary<Chip, Dictionary<string, string>> _partList;
        private List<Chip> _partList;
        private Dictionary<string, string> _wiring = new Dictionary<string, string>();

        private string _chipName;
        protected bool _finishedProcessing;


        public Chip(string ChipName, List<string> inputs, List<string> outputs)
        {
            _chipName = ChipName;
            _inputs = new Dictionary<string, string>();
            _outputs = new Dictionary<string, string>();
            _intermediates = new Dictionary<string, string>();
            _partList = new List<Chip>();  // new Dictionary<Chip, Dictionary<string, string>>();

            foreach (var i in inputs)
                _inputs[Regex.Replace(i, @"\[[\d.]+\]", "")] = null;

            foreach (var o in outputs)
                _outputs[Regex.Replace(o, @"\[[\d.]+\]", "")] = null;

            _finishedProcessing = false;

        }

        public Dictionary<string, string> Inputs { get { return _inputs; } }

        public Dictionary<string, string> Outputs { get { return _outputs; } }

        public Dictionary<string, string> Intermediates { get { return _intermediates; } }

        public Dictionary<string, string> Wiring { get { return _wiring; } }



        public virtual bool ProcessChip(Dictionary<string, string> inputValues)
        {

            if (_finishedProcessing)
                return false;

            // go through each chip wiring up inputs
            wireInputs(inputValues);

            bool processed;

            do
            {
                processed = false;

                // Process all chips...
                foreach (var c in _partList)
                {
                    bool pro = c.ProcessChip(Intermediates);
                    if (pro)
                    {
                        foreach (var o in c.Outputs.Keys)
                        {
                            var w = c.Wiring[o];
                            if (!Intermediates.ContainsKey(w))
                                Intermediates[w] = c.Outputs[o];
                        }
                    }
                    processed = pro || processed;
                }

                // Get output
                wireOutputs();

            } while (!outputsDefined() && processed);

            _finishedProcessing = outputsDefined();
            return outputsDefined();
        }



        private bool outputsDefined()
        {
            return !Outputs.ContainsValue(null);
        }

        private void wireOutputs()
        {
            foreach (var i in Intermediates.Keys)
            {
                if (Outputs.ContainsKey(i))
                    Outputs[i] = Intermediates[i];
            }

        }

        protected void wireInputs(Dictionary<string, string> inputValues)
        {
            foreach (var i in inputValues.Keys)
            {
                var keys = (from w in Wiring
                            where Regex.Replace(w.Value, @"\[[\d.]+\]", "") == i
                            select w.Key).ToList();
                foreach (var key in keys)
                {
                    if (Wiring[key].Contains('['))
                    {
                        var subPins = new Regex(@"(\w+)\[(\d+)[\.\.]*(\d*)\]");
                        var matches = subPins.Match(Wiring[key]);
                        if(matches.Success)
                        {
                            int start;
                            int len;
                            if (Wiring[key].Contains('.'))
                            {
                                start = Invert(Int32.Parse(matches.Groups[3].ToString()), inputValues[key].Length);
                                var endPos = Invert(Int32.Parse(matches.Groups[2].ToString()), inputValues[key].Length);
                                len = endPos - start;
                            }
                            else
                            {
                                start = Invert(Int32.Parse(matches.Groups[2].ToString()), inputValues[key].Length);
                                len = 1;
                            }

                            var val = inputValues[key].Substring(start,len);
                            Inputs[key] = val;
                            Intermediates[key] = val;
                        }
                    }
                    else
                    {
                        Inputs[key] = inputValues[i];
                        Intermediates[key] = inputValues[i];
                    }
                }
            }

        }



        public void AddChip(Chip c, string p)
        {

            var separateWires = p.Split(',').ToList<string>();

            foreach (var w in separateWires)
            {
                var split = w.Split('=');
                c.Wiring[split[0]] = split[1];
            }

            _partList.Add(c);
        }


        private int Invert(int val, int length)
        {
            return length - val -1;
        }


    }
}
