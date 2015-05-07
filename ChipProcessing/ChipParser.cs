using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChipProcessing
{
    public class ChipParser
    {

        public static Chip Parse(string chipText, Dictionary<string,string> otherChips)
        {
            if (String.IsNullOrEmpty(chipText)) return null;

            chipText = RemoveCommentsAndWhiteSpace(chipText);

            var firstLevelRegex = new Regex(@"CHIP(\w+)\{IN([\w,]+);OUT([\w,]+);PARTS:([\w,()=\[\];]+)\}");   

            var matched = firstLevelRegex.Match(chipText);
            if (matched.Success)
            {
                var chipName = matched.Groups[1].Value;
                var inputs = matched.Groups[2].Value;
                var outputs = matched.Groups[3].Value;
                var parts = matched.Groups[4].Value;



                Chip c = new Chip(chipName,inputs.Split(',').ToList<string>(),outputs.Split(',').ToList<string>());


                var partListRegex = new Regex(@"(\w+)\(([\w+=\w+,]+)\);");
                var partList = partListRegex.Match(parts);
                var match = partList;
                
                do
                {
                    if (match.Success)
                    {
                        if (match.Groups[1].Value == "Nand")
                        {
                            c.AddChip(new Nand(), match.Groups[2].Value);
                        }
                        else
                        {
                            c.AddChip(ChipParser.Parse(otherChips[match.Groups[1].Value],otherChips),match.Groups[2].Value);
                        }
                    }
                    match = match.NextMatch();
                } while (match.Success);

                return c; 
            }
            else
                return null;
        }

        private static string RemoveCommentsAndWhiteSpace(string chipText)
        {

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


            //   http://stackoverflow.com/questions/6219454/efficient-way-to-remove-all-whitespace-from-string/14591148#14591148
            return new string(noComments.ToCharArray()
               .Where(c => !Char.IsWhiteSpace(c))
               .ToArray());
        }
    }
}
