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
        List<Symbol> Symbols = new List<Symbol>();
        List<Rule> Rules = new List<Rule>();
        Symbol StartSymbol;
        Parser parser;
        Lexer lexer = new Lexer(null);
        public Lemon()
        {
            StartSymbol = null;
            parser = new Parser(this);
            AddSymbol("$");
            AddSymbol("error");
        }

        public void ParseFile(string filename)
        {
            string text = File.ReadAllText(filename);
            ParseText(text);
        }

        void Show(string format, params object[] args)
        {
            string str = string.Format(format, args);
            Console.Write(str);
        }

        void ShowLine(string format, params object[] args)
        {
            string str = string.Format(format, args);
            Console.WriteLine(str);
        }

        public void ParseText(string text)
        {
            parser.parse(text);
            FindRulePrecedences();
            ShowSymbols();
            SortSymbols();
            ShowSymbols();
            ShowRules();

            FindFirstSets();
            ShowFirstSets();
            return;
        }
         
        void ShowSymbols()
        {
            int index = 0;
            ShowLine("Symbols:");
            foreach (Symbol symbol in Symbols)
            {
                ShowLine("{0} {1}", index++, symbol.name);
            }
            return;
        }

        void ShowRules()
        {
            int index = 0;
            ShowLine("Rules:");
            foreach (Rule rule in Rules)
            {
                if (rule.left_alias != null)
                {
                    Show("{0} {1}({2}) ::= ", index, rule.left.name, rule.left_alias);
                }
                else Show("{0} {1} ::= ", index, rule.left.name);

                for(int i = 0; i < rule.right.Count;i++)
                {
                    if (rule.right_alais[i] == null) Show("{0} ", rule.right[i].name);
                    else Show("{0}({1}) ", rule.right[i].name, rule.right_alais[i]);
                }
                if (rule.precsym !=null)
                {
                    ShowLine("PREC:{0}", rule.precsym.prec);
                }
                else Show("\n");
                index++;
                
            }
        }

        void ShowFirstSets()
        {
            HashSet<Symbol> left_symbols = new HashSet<Symbol>();
            foreach(Rule rule in Rules)
            {
                if (!left_symbols.Add(rule.left)) continue;
                Show("{0} ==> {{", rule.left.name);
                foreach(Symbol symbol in rule.left.first)
                {
                    Show("{0},", symbol.name);
                }
                if (rule.left.HasEmpty) ShowLine(" empty}}");
                else ShowLine(" }}");
            }
        }

        public Symbol AddSymbol(string name)
        {
            Symbol symbol = Symbols.Find( sym =>
            {
                return sym.name == name;
            });

            if (symbol==null)
            {
                symbol = new Symbol(name);
                Symbols.Add(symbol);
            }
            return symbol;
        }

        public Rule AddRule()
        {
            Rule rule = new Rule();
            Rules.Add(rule);
            return rule;
        }

        void SortSymbols()
        {
            int index = 0;
            foreach (Symbol symbol in Symbols)
            {
                symbol.index = index++;
            }

            Symbols.Sort((x, y) => 
            {
                int i1 = x.type == SymbolType.MULTITERMINAL ? 3 : x.name[0] > 'Z' ? 2 : 1;
                int i2 = y.type ==  SymbolType.MULTITERMINAL ? 3 : y.name[0] > 'Z' ? 2 : 1;
                return i1 == i2 ? x.index - y.index : i1 - i2;
            });

            index = 0;
            nsymbol = Symbols.Count;
            nterminal = 0;
            foreach(Symbol symbol in Symbols)
            {
                symbol.index = index++;
                if (nterminal == 0 && char.IsLower(symbol.name[0]))
                {//终结符数量
                    nterminal = index-1;
                }
            }
        }

        void FindRulePrecedences()
        {
            foreach (Rule rule in Rules)
            {
                if (rule.precedence != null) continue;
                foreach (Symbol symbol in rule.right)
                {
                    if (symbol.prec >= 0) rule.precedence = symbol;
                }
            }
        }

        void FindFirstSets()
        {
            foreach(Symbol symbol in Symbols)
            {
                symbol.HasEmpty = false;
                if (symbol.IsTerminal) symbol.first.Add(symbol);
            }

            foreach (Rule rule in Rules)
            {
                if (rule.right.Count == 0) rule.left.HasEmpty = true;
            }

            bool progress = false;

            /* First compute all lambdas */
            do
            {
                progress = false;
                foreach (Rule rule in Rules)
                {
                    if (rule.left.HasEmpty) continue;
                    //假设右侧符号全都可以为空
                    rule.left.HasEmpty = true;
                    foreach (Symbol symbol in rule.right)
                    {
                        if (!symbol.HasEmpty)
                        {
                            rule.left.HasEmpty = false;
                            break;
                        }
                    }
                    if (rule.left.HasEmpty) progress = true;
                }
            }while (progress);

            /* Now compute all first sets */
            do
            {
                Symbol s1, s2;
                progress = false;
                foreach (Rule rule in Rules)
                {
                    s1 = rule.left;
                    foreach (Symbol symbol in rule.right)
                    {
                        s2 = symbol;
                        if (s2.type == SymbolType.TERMINAL)
                        {
                            if (SetAdd(s1,s2)) progress=true;
                        }
                        //else if (s2.type == SymbolType.MULTITERMINAL){}
                        else if (s1 == s2)
                        {
                            if (!s1.HasEmpty) break;
                        }
                        else
                        {
                            if (SetUnion(s1, s2)) progress = true;
                            if (!s2.HasEmpty) break;
                        }
                    }
                }
            }while (progress);
        }

        bool SetAdd(Symbol s1, Symbol s2)
        {
            return s1.first.Add(s2);
        }

        bool SetUnion(Symbol s1, Symbol s2)
        {
            bool progress = false;
            foreach(Symbol symbol in s2.first)
            {
                if (s1.first.Add(symbol)) progress = true;
            }
            return progress;
        }
    }
}
