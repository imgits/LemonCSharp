using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LemonCSharp
{
    class Program 
    {
        static void Main(string[] args)
        {

            //for (char ch = (char)0; ch < 0x7f; ch++)
            //{
            //    if (ch < 0x20)
            //    {
            //        Console.WriteLine(string.Format("CHAR_0x{0:X2}=0x{0:X2},", (int)ch));
            //    }
            //    else if (char.IsLetterOrDigit(ch))
            //    {
            //        Console.WriteLine(string.Format("CHAR_{1}=0x{0:X},//{1}", (int)ch, ch));
            //    }
            //    else
            //    { 
            //        Console.WriteLine(string.Format("CHAR_0x{0:X}=0x{0:X},//{1}", (int)ch, ch));
            //    }
            //}
            string text = File.ReadAllText(@"E:\Lexer\LemonCSharp\calc.y");
            Tokenizer tokenizer = new Tokenizer(text);
            tokenizer.Parse();
        }
    }
}
