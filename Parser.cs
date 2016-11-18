using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LemonCSharp
{
    class Parser
    {
        Lemon lemon;
        Lexer lexer;
        int Precedence = 0;
        Dictionary<string, Symbol> Symbols = new Dictionary<string, Symbol>();
        public Parser(Lemon lemon)
        {
            this.lemon = lemon;
        }

        public void parse(string text)
        {
            lexer = new Lexer(text);
            for (Token token = lexer.next_token(); token != null;)
            {
                switch (token.type)
                {
                    case TokenType.DEFINE:
                        token = Define(token);
                        break;
                    case TokenType.ID:
                        token = Production(token);
                        break;
                    default:
                        token = null;
                        break;
                }
                if (token == null)
                {
                    token = lexer.next_token();
                }
            }
        }

        public void Error(Token token, string errmsg)
        {
            throw new ObjectDisposedException(errmsg);
        }

        void  CheckToken(Token token, TokenType ttype)
        {
            if (!token.IsType(ttype))
            {
                Error(token, "TokenType must be " + lexer.TokenName(ttype));
            }
        }

        void CheckToken(Token token, char ch)
        {
            if (!token.IsType(ch))
            {
                Error(token, "TokenType must be '" + ch + "'");
            }
        }
 
        Token Define(Token define_token)
        {
            Token ret_token = null;
            Token token = lexer.next_token();
            if (token==null)
            {
                Error(token, "Need more token");
            }
            switch (define_token.text)
            {
                case "%code":
                    CheckToken(token, TokenType.CODE_BLOCK);
                    lemon.CodeToken = token;
                    break;
                case "%default_destructor":
                    break;
                case "%default_type":
                    break;
                case "%destructor":
                    break;
                case "%extra_argument":
                    break;
                case "%include":
                    CheckToken(token, TokenType.CODE_BLOCK);
                    lemon.IncludeToken = token;
                    break;
                case "%left":
                    PrecedenceAndAssoc(token,++Precedence, Assoc.LEFT);
                    break;
                case "%name":
                    lemon.name = token.text;
                    break;
                case "%nonassoc":
                    PrecedenceAndAssoc(token, ++Precedence, Assoc.NONE);
                    break;
                case "%parse_accept":
                    break;
                case "%parse_failure":
                    break;
                case "%right":
                    PrecedenceAndAssoc(token, ++Precedence, Assoc.RIGHT);
                    break;
                case "%stack_overflow":
                    break;
                case "%stack_size":
                    break;
                case "%start_symbol":
                    CheckToken(token, TokenType.ID);
                    lemon.StartToken = token;
                    break;
                case "%syntax_error":
                    break;
                case "%token_destructor":
                    break;
                case "%token_prefix":
                    break;
                case "%token_type":
                    break;
                case "%type":
                    break;
            }
            return ret_token;
        }

        void PrecedenceAndAssoc(Token token, int precedence, Assoc assoc)
        {
            for (; token != null; token = lexer.next_token())
            {
                if (token.type == TokenType.DOT) break;
                else if (token.type == TokenType.ID && char.IsUpper(token.text[0]))
                {//首字符大写，视为终结符
                    Symbol symbol = lemon.AddSymbol(token.text);
                    if (symbol.prec >= 0)
                    {
                        Error(token, "Symbol has already be given a precedence.");
                    }
                    else
                    {
                        symbol.prec = precedence;
                        symbol.assoc = assoc;
                    }
                }
                else Error(token, "This TokenType must be ID and first char is upper case");
            }
        }

        Token Production(Token id_token)
        {//产生式
            Token ret_token = null;
            Token token = lexer.next_token();
            if (token == null)
            {
                Error(token, "Need more token");
            }
            Rule rule = lemon.AddRule();
            //产生式左部
            Symbol symbol = lemon.AddSymbol(id_token.text);
            string alias = null;
            if (token.IsType('('))
            {//产生式左部别名
                alias = SymbolAlias();
                token = lexer.next_token();
            }
            rule.left = symbol;
            rule.left_alias= alias;

            //::=
            CheckToken(token, TokenType.ARROW);
            
            //产生式右部
            for (token = lexer.next_token();token!=null; )
            {
                if (token.type == TokenType.DOT) break;

                CheckToken(token, TokenType.ID);
                symbol = lemon.AddSymbol(token.text);

                token = lexer.next_token();
                if (token.IsType('('))
                {
                    alias = SymbolAlias();
                    token = lexer.next_token();
                }
                rule.right.Add(symbol);
                rule.right_alais.Add(alias);
            }

            token = lexer.next_token();
            if (token == null)
            {
                return null;
            }
            if (token.IsType('['))
            {//伪优先级处理
                token = lexer.next_token();
                CheckToken(token, TokenType.ID);
                token = lexer.next_token();
                CheckToken(token, ']');
            }
            else if (token.IsType(TokenType.CODE_BLOCK))
            {//规则代码处理

            }
            else ret_token = token;

            return ret_token;
        }

        string SymbolAlias()
        {
            Token token = lexer.next_token();
            if (token ==null)
            {
                Error(null, "Need token Alias");
            }
            if (token.type != TokenType.ID || !char.IsUpper(token.text[0]))
            {
                Error(token, "This token must be an Alias");
            }
            string alias = token.text;
            token = lexer.next_token();
            CheckToken(token, ')');
            return alias;
        }
    }
}
