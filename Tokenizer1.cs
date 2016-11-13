using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LemonCSharp
{
    class Tokenizer
    {
        string Text;
        int row;
        int col;
        int offset;

        int last_line_length;
        Token cur_token;
        const char EOF = (char)0;


        public Tokenizer(string text)
        {
            this.Text = text;
            row = 0;
            col = 0;
            offset = 0;
            cur_token = null;

        }

        char read_char()
        {//先移动指针再读
            if (offset >= Text.Length) return EOF;
            char ch = Text[offset++];
            if (ch == '\n')
            {
                last_line_length = col;
                row++;
                col = 0;
            }
            else col++;
            return ch;
        }

        bool unread_char()
        {
            offset--;
            if (offset <0 || offset >= Text.Length) return false;
            char ch = Text[offset];
            if (ch == '\n')
            {
                row--;
                col = last_line_length;
                last_line_length =  - 1;
            }
            else col--;
            return true;
        }

        char peek_char()
        {
            return (offset >= Text.Length) ? EOF : Text[offset];
        }

        char look_ahead(int step)
        {//不移动读指针
            return ((step <1) || offset + (step-1) >= Text.Length) ? EOF : Text[offset + (step-1)];
        }

        char next_char1()
        {//不移动读指针
            return (offset + 1 >= Text.Length) ? EOF : Text[offset + 1];
        }

        char next_char2()
        {//不移动读指针
            return (offset + 2 >= Text.Length) ? EOF : Text[offset + 2];
        }

        const string white_space_chars = " \t\r\n";
        void skip_white_space()
        {//返回WhiteSpace之后的第一个字符
            for (char ch = read_char(); ch!= EOF; ch = read_char())
            {
                if (!is_white_space(ch))
                {
                    unread_char();
                    break;
                }
            }
        }

        bool is_white_space(char ch)
        {
            return white_space_chars.IndexOf(ch) >= 0;
        }

        bool start_with(string str)
        {
            if (offset + str.Length >= Text.Length) return false;
            return str.Equals(Text.Substring(offset, str.Length), StringComparison.Ordinal);
        }

        bool skip_chars(int count=1)
        {//移动读指针，跟踪处理行、列和偏移量
            if (count < 0) return false;
            for (int i = 0; i < count; i++)
            {
                if (read_char()== EOF) return false;
            }
            return true;
        }

        bool skip_until(char end_ch)
        {//移动读指针，跟踪处理行、列和偏移量
            for (char ch = read_char(); ch != EOF ; ch = read_char())
            {
                if (ch == end_ch) return true;
            }
            return false;
        }

        bool skip_until(string end_str)
        {// 移动读指针，跟踪处理行、列和偏移量
            char ch0 = end_str[0];
            for (char ch = read_char(); ch != EOF; ch = read_char())
            {
                if (ch == ch0)
                {
                    string substr = Text.Substring(offset, end_str.Length);
                    if (end_str.Equals(substr, StringComparison.Ordinal))
                    {
                        skip_chars(end_str.Length);
                        return true;
                    }
                }
            }
            return false;
        }

        void skip_line()
        {
            skip_until('\n');
        }

        bool skip_string(char start_ch, bool process_escape=true)
        {//处理C语言转义字符
            char prev_ch = EOF;
            for (char ch = read_char(); (ch != EOF); ch = read_char())
            {
                if (ch == start_ch)
                {
                    if (!process_escape || prev_ch != '\\')
                    {
                        return true;
                    }
                }
                if (prev_ch == '\\') prev_ch = EOF;
                else prev_ch = ch;
            }
            return false;
        }

        Token end_token(TokenType tktype = TokenType.NULL)
        {
            cur_token.End(row, col, offset);
            cur_token.text = Text.Substring(cur_token.start_offset, cur_token.end_offset - cur_token.start_offset);
            if (tktype != TokenType.NULL) cur_token.type = tktype;
            if (cur_token.text.Length == 0) cur_token.type = TokenType.ERROR;
            string tk = string.Format("[{0},{1}] {2} : {3}", cur_token.start_line, cur_token.start_column, cur_token.type, cur_token.text);
            Console.WriteLine(tk);
            return cur_token;
        }

        Token bad_token()
        {
            return end_token(TokenType.ERROR);
        }

        Token codeblock_token()
        {//代码块 {}
            char ch, next_ch;
            int level = 1;
            for (ch = read_char(); ch!= EOF; ch = read_char())
            {
                next_ch = next_char1();
                if (ch == '{')
                {
                    level++;
                }
                else if (ch == '}')
                {
                    if (--level == 0) return end_token(TokenType.CodeBlock);
                }
                else if (ch == '/' && next_ch == '/')
                {
                    skip_until('\n');
                }
                else if (ch == '/' && next_ch == '*')
                {
                    skip_until("*/");
                }
                else if (ch == '\'') skip_string('\'');
                else if (ch == '"') skip_string('"');
            }
            return end_token(TokenType.ERROR);
        }

        Token string_token(char start_ch, bool process_escape=true)
        {
            return skip_string(start_ch, process_escape) ? end_token(TokenType.STRING) : end_token(TokenType.ERROR);
        }

        Token hex_token()
        {
            for (char ch = look_ahead(1); ch != EOF; read_char(), ch = look_ahead(1))
            {
                if (char.IsDigit(ch) || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F'))
                {
                    continue;
                }
                else break;
            }
            return end_token(TokenType.NUM);
        }

        Token number_token()
        {
            for (char ch = look_ahead(1); ch != EOF; read_char(), ch = look_ahead(1))
            {
                if (ch == '.')
                {
                    read_char();
                    return float_token();
                }
                if (!char.IsDigit(ch)) break;
            }
            return end_token(TokenType.NUM);
        }

        Token float_token()
        {
            for (char ch = look_ahead(1); ch != EOF; read_char(), ch = look_ahead(1))
            {
                if (!char.IsDigit(ch)) break;
            }
            return end_token(TokenType.FLOAT);
        }

        Token comment_token()
        {
            return end_token(TokenType.STRING);
        }

        Token id_token()
        {
            for (char ch = look_ahead(1); ch != EOF; read_char(), ch = look_ahead(1))
            {
                if (ch != '_' && !char.IsLetterOrDigit(ch)) break;
            }
            return end_token(TokenType.ID);
        }

        Token decl_token()
        {
            for (char ch = read_char(); ch != EOF; ch = read_char())
            {
                if ( ch == ' ' || ch =='\t')  break;
            }
            return end_token(TokenType.Decl);
        }

 
        public Token next_token()
        {
            skip_white_space();
            cur_token = new Token(row, col, offset);
            char cur_ch = read_char();
            char next_ch = next_char1();
            switch (cur_ch)
            {
                case '!'://0x21 exclamation mark 惊叹号
                    break;
                case '"'://0x22  double quotation marks 双引号
                    return string_token(cur_ch);
                case '#'://0x23  sharp
                case '$'://0x24  dollar 
                    break;
                case '%'://0x25  percent
                case '&'://0x26  and
                case '\''://0x27  single quotation marks 单引号
                case '('://0x28   parenthesis or round brackets 圆括号
                case ')'://0x29   parenthesis or round brackets 圆括号
                case '*'://0x2A   multiplied asterisk 星号
                case '+'://0x2B   plus
                case ','://0x2C   comma 逗号
                case '-'://0x2D   minus 
                case '.'://0x2E   period or full stop 句号
                    break;
                case '/'://0x2F divided slash or virgule or diagonal mark 斜线号
                    if (next_ch == '/')
                    {
                        skip_line();
                        return end_token(TokenType.SingleLineComment);
                    }
                    else if (next_ch == '*')
                    {
                        skip_until("*/");
                        return end_token(TokenType.MultiLineComment);
                    }
                    break;
                case ':'://0x3A colon 冒号
                case ';'://0x3B semicolon 分号
                case '<'://0x3C Angle brackets 尖括号
                case '='://0x3D  is equal to 等于号
                case '>'://0x3E Angle brackets 尖括号
                case '?'://0x3F question mark 问号
                case '@'://0x40  at 在
                case '['://0x5B square brackets 方括号
                case '\\'://0x5C backslash; trailing slash; bslash 反斜杠
                case ']'://0x5D square brackets 方括号
                case '^'://0x5E
                case '_'://0x5F
                case '`'://0x60
                case '{'://0x7B curly brackets or braces 大括号
                case '|'://0x7C
                case '}'://0x7D curly brackets or braces 大括号
                case '~'://0x7E
                case ''://0x7F
                    break;
                default:
                    if (cur_ch=='0')
                    {
                        if (next_ch == 'x' || next_ch == 'X')
                        {
                            return hex_token();
                        }
                        else return number_token();
                    }
                    else if (char.IsDigit(cur_ch))
                    {
                        return number_token();
                    }
                    if (cur_ch == '_' || char.IsLetter(cur_ch))
                    {
                        return id_token();
                    }
                    break;
            }
            return end_token((TokenType)cur_ch);
        }

        public void Parse()
        {
            ErrorMsg("arg0");
            ErrorMsg("arg1 ={0}",1);
            ErrorMsg("arg1 ={0} arg2 ={1}", 1,"two");
            Parser1 parser = new Parser1();
            parser.state = ParserState.INITIALIZE;

            skip_white_space();
            for (char cur_ch = read_char(); cur_ch != EOF; skip_white_space(), cur_ch = read_char())
            {
                cur_token = new Token(row, col, offset);
                char next_ch = next_char1();
                if (cur_ch=='/' && next_ch == '/')
                {
                    skip_chars(2);
                    skip_line();
                    end_token(TokenType.SingleLineComment);
                    continue;
                }
                else if (cur_ch == '/' && next_ch == '*')
                {
                    skip_chars(2);
                    skip_until("*/");
                    end_token(TokenType.MultiLineComment);
                    continue;
                }

                if (cur_ch == '"')
                {
                    skip_until('"');
                }
                else if (cur_ch == '{')
                {//潜入c#代码块
                    int level = 1;
                    for (char ch = read_char(); ch != EOF; ch = read_char())
                    {
                        next_ch = next_char1();
                        if (ch == '{')
                        {
                            level++;
                        }
                        else if (ch == '}')
                        {
                            if (--level == 0) break;// return end_token(TokenType.CodeBlock);
                        }
                        else if (ch == '/' && next_ch == '/')
                        {
                            skip_until('\n');
                        }
                        else if (ch == '/' && next_ch == '*')
                        {
                            skip_until("*/");
                        }
                        else if (ch == '\'' || ch == '"') skip_string(ch, true);
                    }
                    end_token(TokenType.CodeBlock);
                }
                else if (char.IsLetterOrDigit(cur_ch) || cur_ch == '_')
                {
                    id_token();
                }
                else if (cur_ch == ':' && next_ch == ':' && next_char2() == '=')
                {
                    skip_chars(3);
                    end_token(TokenType.ARROW);
                }
                else if ((cur_ch == '/' || cur_ch == '|') && char.IsLetterOrDigit(next_ch))
                {

                }
                else
                {
                    end_token((TokenType)cur_ch);
                }
                ParserToken(parser);
            }
            return;
        }

        void ErrorMsg(string format, params object[] args)
        {
            string errmsg = string.Format(format, args);
            Console.WriteLine(errmsg);
        }

        Dictionary<string, Symbol> x2a = new Dictionary<string, Symbol>();

        Symbol Symbol_new(string key)
        {
            Symbol symbol = null;
            if (!x2a.ContainsKey(key))
            {
                symbol = new Symbol(key);
            }
            else symbol = x2a[key];
            return symbol;
        }

        void ParserToken(Parser1 parser)
        {
            string x = cur_token.text;
            switch (parser.state)
            {
                case ParserState.INITIALIZE:
                case ParserState.WAITING_FOR_DECL_OR_RULE:
                    if (parser.state == ParserState.INITIALIZE)
                    {
                        parser.prevrule = null;
                        parser.preccounter = 0;
                        parser.firstrule = parser.lastrule = null;
                        parser.gp.nrule = 0;
                    }
                    if (x[0] == '%')
                    {//定义
                        parser.state = ParserState.WAITING_FOR_DECL_KEYWORD;
                    }
                    else if (char.IsLower(x[0]))
                    {//非终结符
                        parser.lhs = Symbol_new(x);
                        parser.nrhs = 0;
                        parser.lhsalias = null;
                        parser.state = ParserState.WAITING_FOR_ARROW;
                    }
                    else if (x[0] == '{')
                    {//c#代码块
                        if (parser.prevrule == null)
                        {
                            ErrorMsg("There is no prior rule upon which to attach the code fragment which begins on this line.");
                            parser.errorcnt++;
                        }
                        else if (parser.prevrule.code != null)
                        {
                            ErrorMsg("Code fragment beginning on this line is not the first to follow the previous rule.");
                            parser.errorcnt++;
                        }
                        else
                        {
                            parser.prevrule.line = parser.tokenlineno;
                            parser.prevrule.code = x;
                            parser.prevrule.noCode = 0;
                        }
                    }
                    else if (x[0] == '[')
                    {//伪终结符开始
                        parser.state = ParserState.PRECEDENCE_MARK_1;
                    }
                    else
                    {
                        ErrorMsg("Token \"{0}\" should be either \"%%\" or a nonterminal name.", x);
                        parser.errorcnt++;
                    }
                    break;


                case ParserState.PRECEDENCE_MARK_1://伪终结符开始
                    if (!char.IsUpper(x[0]))
                    {//伪终结符首字母必须大写
                        ErrorMsg("The precedence symbol must be a terminal.");
                        parser.errorcnt++;
                    }
                    else if (parser.prevrule == null)
                    {
                        ErrorMsg("There is no prior rule to assign precedence \"[%s]\".", x);
                        parser.errorcnt++;
                    }
                    else if (parser.prevrule.precsym != null)
                    {
                        ErrorMsg("Precedence mark on this line is not the first to follow the previous rule.");
                        parser.errorcnt++;
                    }
                    else
                    {
                        parser.prevrule.precsym = Symbol_new(x);
                    }
                    parser.state = ParserState.PRECEDENCE_MARK_2;
                    break;

                case ParserState.PRECEDENCE_MARK_2://伪终结符结束
                    if (x[0] != ']')
                    {
                        ErrorMsg("Missing \"]\" on precedence mark.");
                        parser.errorcnt++;
                    }
                    parser.state = ParserState.WAITING_FOR_DECL_OR_RULE;
                    break;

                case ParserState.WAITING_FOR_ARROW://产生式定义
                    if (x[0] == ':' && x[1] == ':' && x[2] == '=')
                    {
                        parser.state = ParserState.IN_RHS;
                    }
                    else if (x[0] == '(')
                    {
                        parser.state = ParserState.LHS_ALIAS_1;
                    }
                    else
                    {
                        ErrorMsg("Expected to see a \":\" following the LHS symbol \"{0}\".", parser.lhs.name);
                        parser.errorcnt++;
                        parser.state = ParserState.RESYNC_AFTER_RULE_ERROR;
                    }
                    break;

                case ParserState.LHS_ALIAS_1://左非终结符别名开始 Lhs (
                    if (char.IsLetter(x[0]))
                    {// Lhs (xxxxx
                        parser.lhsalias = x;
                        parser.state = ParserState.LHS_ALIAS_2;
                    }
                    else
                    {
                        ErrorMsg("\"{0}\" is not a valid alias for the LHS \"{1}\"\n", x, parser.lhs.name);
                        parser.errorcnt++;
                        parser.state = ParserState.RESYNC_AFTER_RULE_ERROR;
                    }
                    break;
                case ParserState.LHS_ALIAS_2:
                    if (x[0] == ')')
                    {//左非终结符别名结束 Lhs (xxxxx) 
                        parser.state = ParserState.LHS_ALIAS_3;
                    }
                    else
                    {
                        ErrorMsg("Missing \")\" following LHS alias name \"{0}\".", parser.lhsalias);
                        parser.errorcnt++;
                        parser.state = ParserState.RESYNC_AFTER_RULE_ERROR;
                    }
                    break;
                case ParserState.LHS_ALIAS_3:
                    if (x[0] == ':' && x[1] == ':' && x[2] == '=')
                    {
                        parser.state = ParserState.IN_RHS;
                    }
                    else
                    {
                        ErrorMsg("Missing \"->\" following: \"{0}({1})\".", parser.lhs.name, parser.lhsalias);
                        parser.errorcnt++;
                        parser.state = ParserState.RESYNC_AFTER_RULE_ERROR;
                    }
                    break;
                //以上已将产生式左边处理完毕
                //以下开始处理产生式右边内容
                case ParserState.IN_RHS:
                    if (x[0] == '.')
                    {//一条产生式处理完毕

                    }
                    break;
            }

        }
    }
}
