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
            string text = File.ReadAllText(@"E:\Lexer\LemonCSharp\calc.y");
            Lemon lemon = new Lemon();
            lemon.ParseText(text);
            return;
        }
    }

    public partial class Lemon
    {
        //Dictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();
        Parser parser;
        Lexer lexer = new Lexer(null);
        public Lemon()
        {
            parser = new Parser(this);
        }

        public void ParseFile(string filename)
        {
            string text = File.ReadAllText(filename);
            ParseText(text);
        }

        public void ParseText(string text)
        {
            parser.parse(text);
        }
    }
}
