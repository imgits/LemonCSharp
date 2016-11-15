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
                if (rule.lhs.alias != null)
                {
                    Show("{0} {1}({2}) ::= ", index, rule.lhs.symbol.name, rule.lhs.alias);
                }
                else Show("{0} {1} ::= ", index, rule.lhs.symbol.name);

                foreach (RuleItem ri in rule.rhs)
                {
                    if (ri.alias == null) Show("{0} ", ri.symbol.name);
                    else Show("{0}({1}) ", ri.symbol.name, ri.alias);
                }
                if (rule.precsym !=null)
                {
                    ShowLine("PREC:{0}", rule.precsym.prec);
                }
                else Show("\n");
                index++;
                
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
                if (rule.precsym != null) continue;
                foreach (RuleItem ri in rule.rhs)
                {
                    if (ri.symbol.prec >= 0) rule.precsym = ri.symbol;
                }
            }
        }

        void FindFirstSets()
        {
            foreach(Symbol symbol in Symbols)
            {
                symbol.lambda = false;
            }

            for (int i = nterminal; i < Symbols.Count;i++)
            {
                Symbols[i].firstset = new bool[nterminal + 1];
            }

            bool progress = false;
            /* First compute all lambdas */
            do
            {
                progress = false;
                foreach (Rule rule in Rules)
                {
                    if (rule.lhs.symbol.lambda) continue;
                    //假设右侧符号全都可以为空
                    progress = true;
                    rule.lhs.symbol.lambda = true;
                    foreach (RuleItem ri in rule.rhs)
                    {
                        Symbol symbol = ri.symbol;
                        if (!ri.symbol.lambda)
                        {
                            progress = false;
                            rule.lhs.symbol.lambda = false;
                            break;
                        }
                    }
                }
            }while (progress);

            /* Now compute all first sets */
            do
            {
                Symbol s1, s2;
                progress = false;
                foreach (Rule rule in Rules)
                {
                    s1 = rule.lhs.symbol;
                    foreach (RuleItem ri in rule.rhs)
                    {
                        s2 = ri.symbol;
                        if (s2.type == SymbolType.TERMINAL)
                        {
                            progress = SetAdd(s1, s2);
                        }
                        else if (s2.type == SymbolType.MULTITERMINAL)
                        {

                        }
                        else if (s1 == s2)
                        {
                            if (!s1.lambda) break;
                        }
                        else
                        {
                            progress = SetUnion(s1,s2);
                            if (!s2.lambda) break;
                        }
                    }
                }
            }while (progress);
        }

        bool SetAdd(Symbol s1, Symbol s2)
        {
            bool rv = s1.firstset[s2.index];
            s1.firstset[s2.index] = true;
            return !rv;
        }

        bool SetUnion(Symbol s1, Symbol s2)
        {
            bool progress = false;
            for (int i = 0; i < nterminal + 1; i++)
            {
                if (!s2.firstset[i]) continue;
                if (!s1.firstset[i])
                {
                    s1.firstset[i] = true;
                    progress = true;
                }
            }
            return progress;
        }
    }
}
