using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LemonCSharp
{
    enum TokenTypeEx : int
    {
        CODE_BLOCK = 0x10000,
        DEFINE,
        ARROW
    }

    class Lexer : Tokenizer
    {
        public Lexer(string text) : base(text)
        {
            SetTokenHandler('{', CodeBlock_Token);
            SetTokenHandler('%', Define_Token);
            SetTokenHandler(':', Arrow_Token);
            TokenType tt = TokenType.ADD;
             
            switch((int)tt)
            {
                case (int)TokenType.ASK:
                    break;
                case (int)TokenTypeEx.CODE_BLOCK:
                    break;
            }
        }

        public override string TokenName(TokenType TkType)
        {
            if (TkType < TokenType.LAST_ONE)
            {
                return Enum.GetName(typeof(TokenType), TkType);
            }
            else
            {
                return Enum.GetName(typeof(TokenTypeEx), TkType);
            }
        }

        Token Define_Token(char cur_ch)
        {//
            char first_ch = peek_char();
            if (first_ch != '_' && !char.IsLetter(first_ch))
            {
                return null;
            }
            read_char();
            for (char ch = peek_char(); ch != EOF; read_char(), ch = peek_char())
            {
                if (ch != '_' && !char.IsLetterOrDigit(ch))
                {
                    break;
                }
            }
            return end_token((TokenType)TokenTypeEx.DEFINE);
        }

        Token Arrow_Token(char cur_ch)
        {//产生式定义 ::=
            if (next_char1()==':' && next_char2() == '=')
            {
                skip_chars(2);
                return end_token((TokenType)TokenTypeEx.ARROW);
            }
            return null;
        }

        Token CodeBlock_Token(char cur_ch)
        { //代码块 {}
            //if (cur_ch != '{') return null;
            char ch, next_ch;
            int level = 1;
            for (ch = read_char(); ch != EOF; ch = read_char())
            {
                next_ch = next_char1();
                if (ch == '{')
                {
                    level++;
                }
                else if (ch == '}')
                {
                    if (--level == 0)
                    {
                        return end_token((TokenType)TokenTypeEx.CODE_BLOCK);
                    }
                }
                else if (ch == '/' && next_ch == '/')
                {
                    skip_until('\n');
                }
                else if (ch == '/' && next_ch == '*')
                {
                    skip_until("*/");
                }
                else if (ch == '\'' || ch == '"') skip_string(ch);
            }
            return end_token(TokenType.ERROR);
        }

        bool skip_string(char start_ch)
        {//处理C语言转义字符
            char prev_ch = EOF;
            for (char ch = read_char(); (ch != EOF); ch = read_char())
            {
                if (ch == start_ch && prev_ch != '\\')
                {
                    return true;
                }
                if (prev_ch == '\\') prev_ch = EOF;
                else prev_ch = ch;
            }
            return false;
        }

    }
}
