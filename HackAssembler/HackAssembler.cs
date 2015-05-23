using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assembler
{
    public class HackAssembler
    {

        private string _asmText;
        private string _hackText;
        private string _errors;

        private Dictionary<string, string> _cBits;
        private Dictionary<string, string> _dBits;
        private Dictionary<string, string> _jBits;

        private Dictionary<string, string> _symbolTable;

        public string AsmText { get { return _asmText; } }
        public string HackText { get { return _hackText; } }
        public string Errors { get { return _errors; } }
        public Dictionary<string, string> SymbolTalbe { get { return _symbolTable; } }


        public HackAssembler(string asmText)
        {
            _asmText = asmText;
            _hackText = "";
            _errors = "";

            InitializeDataStructures();
            AssembleFile();

        }

        private void AssembleFile()
        {
            var workingText = RemoveAllComments(_asmText);
            var asmLines = workingText.Split('\n');
            string[] hackLines = new string[asmLines.Length];

            BuildSymbolTable(asmLines);

            var hackLine = 0;
            for (int i = 0; i < asmLines.Length; i++)
            {
                var labelRegEx = new Regex(@"\((.+)\)");
                var match = labelRegEx.Match(asmLines[i].Trim());
                if(!String.IsNullOrEmpty(asmLines[i]) && !match.Success)
                {
                    hackLines[hackLine] = TranslateLine(asmLines[i]);
                    hackLine++;
                }
            }

            _hackText = String.Join("\n", hackLines);
        }

        private void BuildSymbolTable(string[] asmLines)
        {
            var actualCodeLine = 0;

            for (int i = 0; i < asmLines.Length; i++)
            {
                var labelRegEx = new Regex(@"\((.+)\)");
                var match = labelRegEx.Match(asmLines[i].Trim());

                if(match.Success)
                {
                    _symbolTable[match.Groups[1].Value] = (actualCodeLine).ToString();
                }
                else
                {
                    actualCodeLine++;
                }
            }

            var memCounter = 16;

            for (int i = 0; i < asmLines.Length; i++)
            {
                var labelRegEx = new Regex(@"@\D+");
                var match = labelRegEx.Match(asmLines[i].Trim());

                if (match.Success)
                {
                    if (!_symbolTable.ContainsKey(asmLines[i].Trim().Substring(1)))
                    {
                        _symbolTable[asmLines[i].Trim().Substring(1)] = (memCounter).ToString();
                        memCounter++;
                    }
                }
            }

        }

        private string TranslateLine(string line)
        {
            line = line.Trim();
            if(line[0] == '@')
            {
                return CreateAInstruction(line);
            }
            else
            {
                return CreateCInstruction(line);
            }
        }

        private string CreateCInstruction(string line)
        {
            var l1 = line.Split('=');
            string splitStr;
            string cstr, dstr, jstr;
            
            if(l1.Length>1)
            {
                dstr = l1[0];
                splitStr = l1[1];
            }
            else
            {
                dstr = "null";
                splitStr = l1[0];
            }

            l1 = splitStr.Split(';');

            if(l1.Length>1)
            {
                cstr = l1[0];
                jstr = l1[1];
            }
            else
            {
                cstr = l1[0];
                jstr = "null";
            }

            return "111" + CompBits(cstr) + DestBits(dstr) + JumpBits(jstr);
        }

        private string CompBits(string cstr)
        {
            return _cBits[cstr];
        }

        private object DestBits(string dstr)
        {
            return _dBits[dstr];
        }

        private object JumpBits(string jstr)
        {
            return _jBits[jstr];
        }

        private string CreateAInstruction(string line)
        {
            int n;
            if (_symbolTable.ContainsKey(line.Substring(1)))
                n = Int32.Parse(_symbolTable[line.Substring(1)]);
            else
                n = Int32.Parse(line.Substring(1));
            return Convert.ToString(n, 2).PadLeft(16, '0');
        }

        private void InitializeDataStructures()
        {
            InitBitTranslations();
            InitSymbolTable();

        }

        private void InitSymbolTable()
        {
            _symbolTable = new Dictionary<string, string>();

            for (int i = 0; i < 16; i++ )
                _symbolTable["R"+i.ToString()] = i.ToString();

            _symbolTable["SCREEN"] = "16384";
            _symbolTable["KBD"] = "24576";
            _symbolTable["SP"] = "0";
            _symbolTable["LCL"] = "1";
            _symbolTable["ARG"] = "2";
            _symbolTable["THIS"] = "3";
            _symbolTable["THAT"] = "4";

        }

        private void InitBitTranslations()
        {
            _cBits = new Dictionary<string, string>();
            _cBits["0"] = "0101010";
            _cBits["1"] = "0111111";
            _cBits["-1"] = "0111010";
            _cBits["D"] = "0001100";
            _cBits["A"] = "0110000";
            _cBits["!D"] = "0001101";
            _cBits["!A"] = "0110001";
            _cBits["-D"] = "0001111";
            _cBits["-A"] = "0110011";
            _cBits["D+1"] = "0011111";
            _cBits["A+1"] = "0110111";
            _cBits["D-1"] = "0001110";
            _cBits["A-1"] = "0110010";
            _cBits["D+A"] = "0000010";
            _cBits["D-A"] = "0010011";
            _cBits["A-D"] = "0000111";
            _cBits["D&A"] = "0000000";
            _cBits["D|A"] = "0010101";
            _cBits["M"] = "1110000";
            _cBits["!M"] = "1110001";
            _cBits["-M"] = "1110011";
            _cBits["M+1"] = "1110111";
            _cBits["M-1"] = "1110010";
            _cBits["D+M"] = "1000010";
            _cBits["D-M"] = "1010011";
            _cBits["M-D"] = "1000111";
            _cBits["D&M"] = "1000000";
            _cBits["D|M"] = "1010101";



            _dBits = new Dictionary<string, string>();
            _dBits["null"] = "000";
            _dBits["M"] = "001";
            _dBits["D"] = "010";
            _dBits["MD"] = "011";
            _dBits["A"] = "100";
            _dBits["AM"] = "101";
            _dBits["AD"] = "110";
            _dBits["AMD"] = "111";



            _jBits = new Dictionary<string, string>();
            _jBits["null"] = "000";
            _jBits["JGT"] = "001";
            _jBits["JEQ"] = "010";
            _jBits["JGE"] = "011";
            _jBits["JLT"] = "100";
            _jBits["JNE"] = "101";
            _jBits["JLE"] = "110";
            _jBits["JMP"] = "111";
        }


        public static string RemoveAllComments(string chipText)
        {
            if (String.IsNullOrEmpty(chipText))
                return null;

            return RemoveBlankLines(RemoveComments(chipText));
        }

        private static string RemoveBlankLines(string subjectString)
        {
            if (String.IsNullOrEmpty(subjectString))
                return null;

            return Regex.Replace(subjectString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
        }

        private static string RemoveComments(string chipText)
        {
            if (String.IsNullOrEmpty(chipText))
                return null;

            //   http://stackoverflow.com/questions/3524317/regex-to-strip-line-comments-from-c-sharp/3524689#3524689
            var blockComments = @"/\*(.*?)\*/";
            var lineComments = @"//(.*?)\r?\n";
            var strings = @"""((\\[^\n]|[^""\n])*)""";
            var verbatimStrings = @"@(""[^""]*"")+";

            string noComments = Regex.Replace(chipText,
                blockComments + "|" + lineComments + "|" + strings + "|" + verbatimStrings,
                me =>
                {
                    if (me.Value.StartsWith("/*") || me.Value.StartsWith("//"))
                        return me.Value.StartsWith("//") ? Environment.NewLine : "";
                    // Keep the literal strings
                    return me.Value;
                },
                RegexOptions.Singleline);
            return noComments;
        }
    }
}
