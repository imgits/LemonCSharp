using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LemonCSharp
{
    enum TokenType
    {
        NULL=0,
        CHAR_0x00 = 0x00,
        CHAR_0x01 = 0x01,
        CHAR_0x02 = 0x02,
        CHAR_0x03 = 0x03,
        CHAR_0x04 = 0x04,
        CHAR_0x05 = 0x05,
        CHAR_0x06 = 0x06,
        CHAR_0x07 = 0x07,
        CHAR_0x08 = 0x08,
        CHAR_0x09 = 0x09,
        CHAR_0x0A = 0x0A,
        CHAR_0x0B = 0x0B,
        CHAR_0x0C = 0x0C,
        CHAR_0x0D = 0x0D,
        CHAR_0x0E = 0x0E,
        CHAR_0x0F = 0x0F,
        CHAR_0x10 = 0x10,
        CHAR_0x11 = 0x11,
        CHAR_0x12 = 0x12,
        CHAR_0x13 = 0x13,
        CHAR_0x14 = 0x14,
        CHAR_0x15 = 0x15,
        CHAR_0x16 = 0x16,
        CHAR_0x17 = 0x17,
        CHAR_0x18 = 0x18,
        CHAR_0x19 = 0x19,
        CHAR_0x1A = 0x1A,
        CHAR_0x1B = 0x1B,
        CHAR_0x1C = 0x1C,
        CHAR_0x1D = 0x1D,
        CHAR_0x1E = 0x1E,
        CHAR_0x1F = 0x1F,
        CHAR_0x20 = 0x20,//
        CHAR_0x21 = 0x21, NOT=0x21,EXCL=0x21,//!     exclamation mark 惊叹号
        CHAR_0x22 = 0x22, DQUOT=0x22,//"    double quotation marks 双引号
        CHAR_0x23 = 0x23, SHARP =0x23,//#
        CHAR_0x24 = 0x24, DOLLAR=0x24,//$
        CHAR_0x25 = 0x25, PERCENT=0x25,MOD=0x25,//%
        CHAR_0x26 = 0x26, AND=0x26,//&
        CHAR_0x27 = 0x27, SQUOT = 0x27,  //'    single quotation marks 单引号
        CHAR_0x28 = 0x28, LPAR=0x28,//( parenthesis or round brackets 圆括号
        CHAR_0x29 = 0x29, RPAR=0x29,//) parenthesis or round brackets 圆括号
        CHAR_0x2A = 0x2A, MUL=0x2A, STAR= 0x2A,//*
        CHAR_0x2B = 0x2B, ADD=0x2B, PLUS= 0x2B,//+
        CHAR_0x2C = 0x2C, COMMA=0x2C,//,
        CHAR_0x2D = 0x2D, SUB = 0x2D, MIMUS = 0x2D,//-
        CHAR_0x2E = 0x2E, DOT = 0x2E, PERIOD =0x2E,//. period or full stop 句号
        CHAR_0x2F = 0x2F, DIV=0x2F, DIVIDE=0x2F,SLASH=0x2F,/// divided slash or virgule or diagonal mark 斜线号
        CHAR_0 = 0x30,//0
        CHAR_1 = 0x31,//1
        CHAR_2 = 0x32,//2
        CHAR_3 = 0x33,//3
        CHAR_4 = 0x34,//4
        CHAR_5 = 0x35,//5
        CHAR_6 = 0x36,//6
        CHAR_7 = 0x37,//7
        CHAR_8 = 0x38,//8
        CHAR_9 = 0x39,//9
        CHAR_0x3A = 0x3A, COLON=0x3A,//: colon 冒号
        CHAR_0x3B = 0x3B, SEMI=0x3B,//; semicolon 分号
        CHAR_0x3C = 0x3C, LE=0x3C, LANGLE=0x3C,//< Angle brackets 尖括号
        CHAR_0x3D = 0x3D, EQ=0x3D,//=
        CHAR_0x3E = 0x3E, GE=0x3E, RANGLE = 0x3E,//>
        CHAR_0x3F = 0x3F, ASK=0x3F, QUESTION=0x3F,//?
        CHAR_0x40 = 0x40, AT=0x40,//@
        CHAR_A = 0x41,//A
        CHAR_B = 0x42,//B
        CHAR_C = 0x43,//C
        CHAR_D = 0x44,//D
        CHAR_E = 0x45,//E
        CHAR_F = 0x46,//F
        CHAR_G = 0x47,//G
        CHAR_H = 0x48,//H
        CHAR_I = 0x49,//I
        CHAR_J = 0x4A,//J
        CHAR_K = 0x4B,//K
        CHAR_L = 0x4C,//L
        CHAR_M = 0x4D,//M
        CHAR_N = 0x4E,//N
        CHAR_O = 0x4F,//O
        CHAR_P = 0x50,//P
        CHAR_Q = 0x51,//Q
        CHAR_R = 0x52,//R
        CHAR_S = 0x53,//S
        CHAR_T = 0x54,//T
        CHAR_U = 0x55,//U
        CHAR_V = 0x56,//V
        CHAR_W = 0x57,//W
        CHAR_X = 0x58,//X
        CHAR_Y = 0x59,//Y
        CHAR_Z = 0x5A,//Z
        CHAR_0x5B = 0x5B, LSQUARE=0x5B,//[ square brackets 方括号
        CHAR_0x5C = 0x5C, BSLASH=0x5C,//\ backslash; trailing slash; bslash 反斜杠
        CHAR_0x5D = 0x5D, RSQUARE = 0x5D,//]
        CHAR_0x5E = 0x5E, POWER=0x5E,//^
        CHAR_0x5F = 0x5F, UNDERLINE=0x5F,//_ underline
        CHAR_0x60 = 0x60, //`
        CHAR_a = 0x61,//a
        CHAR_b = 0x62,//b
        CHAR_c = 0x63,//c
        CHAR_d = 0x64,//d
        CHAR_e = 0x65,//e
        CHAR_f = 0x66,//f
        CHAR_g = 0x67,//g
        CHAR_h = 0x68,//h
        CHAR_i = 0x69,//i
        CHAR_j = 0x6A,//j
        CHAR_k = 0x6B,//k
        CHAR_l = 0x6C,//l
        CHAR_m = 0x6D,//m
        CHAR_n = 0x6E,//n
        CHAR_o = 0x6F,//o
        CHAR_p = 0x70,//p
        CHAR_q = 0x71,//q
        CHAR_r = 0x72,//r
        CHAR_s = 0x73,//s
        CHAR_t = 0x74,//t
        CHAR_u = 0x75,//u
        CHAR_v = 0x76,//v
        CHAR_w = 0x77,//w
        CHAR_x = 0x78,//x
        CHAR_y = 0x79,//y
        CHAR_z = 0x7A,//z
        CHAR_0x7B = 0x7B, LBRACES=0x7B,//{ curly brackets or braces 大括号
        CHAR_0x7C = 0x7C, OR=0x7C,//|
        CHAR_0x7D = 0x7D, RBRACES = 0x7D,//} curly brackets or braces 大括号
        CHAR_0x7E = 0x7E, NEG=0x7E,//~

        NUM = 256, FLOAT, KEY, ID, STRING, SPECIAL, ERROR,

        Set,Decl,
        CodeBlock,
        SingleLineComment, MultiLineComment,
    };

    public enum ParserState
    {
        INITIALIZE,
        WAITING_FOR_DECL_OR_RULE,
        WAITING_FOR_DECL_KEYWORD,
        WAITING_FOR_DECL_ARG,
        WAITING_FOR_PRECEDENCE_SYMBOL,
        WAITING_FOR_ARROW,
        IN_RHS,
        LHS_ALIAS_1,
        LHS_ALIAS_2,
        LHS_ALIAS_3,
        RHS_ALIAS_1,
        RHS_ALIAS_2,
        PRECEDENCE_MARK_1,
        PRECEDENCE_MARK_2,
        RESYNC_AFTER_RULE_ERROR,
        RESYNC_AFTER_DECL_ERROR,
        WAITING_FOR_DESTRUCTOR_SYMBOL,
        WAITING_FOR_DATATYPE_SYMBOL,
        WAITING_FOR_FALLBACK_ID,
        WAITING_FOR_WILDCARD_ID,
        WAITING_FOR_CLASS_ID,
        WAITING_FOR_CLASS_TOKEN
    } 
     
    class Parser
    {
        public ParserState state;
        public Symbol lhs;
        public int nrhs;
        public int lhsalias;
    }
       
    class Rule
    {
        public Symbol lhs;
        public string lhsalias;
        public int ruleline;
        public List<Symbol> rhs;
        public List<string> rhsalias;
        public int line;
        public string code;
        public Symbol precsym;
        int index;
        bool canReduce;
        public Rule next_lhs;
        public Rule next;
    }

    class Symbol
    {
        public Symbol(string text)
        {

        }
    }

    class Token
    {
        public int start_line;
        public int start_column;
        public int start_offset;

        public int end_line;
        public int end_column;
        public int end_offset;

        public string text;
        public TokenType type;
        public Token(int line, int column, int offset)
        {
            start_line = end_line = line;
            start_column = end_column = column;
            start_offset = end_offset = offset;
            type = TokenType.NULL;
        }

        public void End(int line, int column, int offset)
        {
            end_line = line;
            end_column = column;
            end_offset = offset;
        }
    }

    class Tokenizer
    {
        string Text;
        int scan_line;
        int scan_column;
        int scan_offset;

        int last_line_length;

        Token cur_token;
        const char EOF = (char)0;


        public Tokenizer(string text)
        {
            this.Text = text;
            scan_line = 0;
            scan_column = 0;
            scan_offset = 0;
            cur_token = null;

            last_line_length = -1;
        }

        char read_char()
        {//移动读指针
            if (scan_offset >= Text.Length) return EOF;
            char ch = Text[scan_offset++];
            if (ch == '\n')
            {
                last_line_length = scan_column;
                scan_line++;
                scan_column = 0;
            }
            else scan_column++;
            return ch;
        }

        char unread_char()
        {
            scan_offset--;
            if (scan_offset >= Text.Length) return EOF;
            char ch = Text[scan_offset];
            if (ch == '\n')
            {
                scan_line--;
                scan_column = last_line_length;
                last_line_length =  - 1;
            }
            else scan_column--;
            return ch;
        }

        char look_ahead(int step)
        {//不移动读指针
            return ((step <1) || scan_offset + (step-1) >= Text.Length) ? EOF : Text[scan_offset + (step-1)];
        }

        char next_char1()
        {//不移动读指针
            return (scan_offset + 0 >= Text.Length) ? EOF : Text[scan_offset + 0];
        }

        char next_char2()
        {//不移动读指针
            return (scan_offset + 1 >= Text.Length) ? EOF : Text[scan_offset + 1];
        }

        char next_char3()
        {//不移动读指针
            return (scan_offset + 2 >= Text.Length) ? EOF : Text[scan_offset + 2];
        }

        const string white_space_chars = " \t\r\n";
        void skip_white_space()
        {//返回WhiteSpace之后的第一个字符
            for (char ch = look_ahead(1); ch!= EOF; read_char(), ch = look_ahead(1))
            {
                if (!is_white_space(ch)) break;
            }
        }

        bool is_white_space(char ch)
        {
            return white_space_chars.IndexOf(ch) >= 0;
        }

        bool start_with(string str)
        {
            if (scan_offset + str.Length >= Text.Length) return false;
            return str.Equals(Text.Substring(scan_offset, str.Length), StringComparison.Ordinal);
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
        {// 移动读指针，跟踪处理行、列和偏移量
            for (char ch = look_ahead(1); ch != EOF ; read_char(), ch = look_ahead(1))
            {
                if (ch == end_ch) return true;
            }
            return false;
        }

        bool skip_until(string end_str)
        {// 移动读指针，跟踪处理行、列和偏移量
            char ch0 = end_str[0];
            for (char ch = look_ahead(1); ch != EOF; read_char(), ch = look_ahead(1))
            {
                if (ch == ch0 && end_str.Equals(Text.Substring(scan_offset - 1, end_str.Length), StringComparison.Ordinal))
                {
                    skip_chars(end_str.Length);
                    return true;
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
            cur_token.End(scan_line, scan_column, scan_offset);
            cur_token.text = Text.Substring(cur_token.start_offset, cur_token.end_offset - cur_token.start_offset);
            if (tktype != TokenType.NULL) cur_token.type = tktype;
            if (cur_token.text.Length == 0) cur_token.type = TokenType.ERROR;
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

        public void parse()
        {
            for (;;)
            {
                Token token = next_token();
                if (token.type == TokenType.ERROR) break;
                Console.WriteLine(string.Format("[{0},{1} {2}:{3}", token.start_line, token.start_column, token.type, token.text));
                switch(token.type)
                {
                    case TokenType.NULL:
                        break;
                    case TokenType.SingleLineComment:
                    case TokenType.MultiLineComment:
                        break;
                    
                    default:
                        break;
                }
            }
        }

        Token next_token()
        {
            skip_white_space();
            cur_token = new Token(scan_line, scan_column, scan_offset);
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

        Token Parse()
        {
            skip_white_space();
            for (char cur_ch = read_char(); cur_ch != EOF; skip_white_space(), cur_ch = read_char())
            {
                cur_token = new Token(scan_line, scan_column, scan_offset);
                char next_ch = next_char1();
                if (cur_ch=='/' && next_ch == '/')
                {
                    skip_line();
                    continue;
                    //return end_token(TokenType.SingleLineComment);
                }
                else if (cur_ch == '/' && next_ch == '*')
                {
                    skip_until("*/");
                    continue;
                    //return end_token(TokenType.MultiLineComment);
                }

                if (cur_ch =='"')
                {
                    skip_until('"');
                }
                else if (cur_ch=='{')
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
                        else if (ch == '\'' || ch == '"') skip_string(ch, true);
                    }
                }
                else  if (char.IsLetterOrDigit(cur_ch) || cur_ch=='_')
                {
                    id_token();
                }
                else if (cur_ch==':' && next_ch==':' && next_char2()=='=')
                {
                    skip_chars(3);
                }
                else if ( (cur_ch=='/' || cur_ch=='|') && char.IsLetterOrDigit(next_ch))
                {

                }
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
                            continue;
                            //return end_token(TokenType.SingleLineComment);
                        }
                        else if (next_ch == '*')
                        {
                            skip_until("*/");
                            continue;
                            //return end_token(TokenType.MultiLineComment);
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
                        if (cur_ch == '0')
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
            }
            return end_token((TokenType)cur_ch);
        }

        void ErrorMsg(string errmsg=null)
        {
            Console.WriteLine(errmsg);
        }

        void ParserToken(Parser parser)
        {

            string x = cur_token.text;
            switch(parser.state)
            {
                case ParserState.INITIALIZE:
                case ParserState.WAITING_FOR_DECL_OR_RULE:
                    if (x[0]=='%')
                    {//定义
                        parser.state = ParserState.WAITING_FOR_DECL_KEYWORD;
                    }
                    else if (char.IsLower(x[0]))
                    {//非终结符
                        parser.state = ParserState.WAITING_FOR_ARROW;
                    }
                    else if (x[0] == '{')
                    {//c#代码块

                    }
                    else if (x[0] == '[')
                    {//伪终结符开始
                        parser.state = ParserState.PRECEDENCE_MARK_1;
                    }
                    else
                    {
                        ErrorMsg();
                    }
                    break;
                
                    
                case ParserState.PRECEDENCE_MARK_1://伪终结符开始
                    if (!char.IsUpper(x[0]))
                    {//伪终结符首字母必须大写
                        ErrorMsg("The precedence symbol must be a terminal.");
                    }
                    break;

                case ParserState.PRECEDENCE_MARK_2://伪终结符结束
                    if (x[0] != ']')
                    {
                        ErrorMsg("Missing \"]\" on precedence mark.");
                        //psp->errorcnt++;
                    }
                    //伪终结符结束
                    parser.state = ParserState.WAITING_FOR_DECL_OR_RULE;
                    break;

                case ParserState.WAITING_FOR_ARROW:
                    if (x[0] == ':' && x[1] == ':' && x[2] == '=')
                    {//产生式定义
                        parser.state = ParserState.IN_RHS;
                    }
                    else if (x[0] == '(')
                    {
                        parser.state = ParserState.LHS_ALIAS_1;
                    }
                    else
                    {
                        ErrorMsg("Expected to see a \":\" following the LHS symbol \"%s\".");
                        //psp->errorcnt++;
                        parser.state = ParserState.RESYNC_AFTER_RULE_ERROR;
                    }
                    break;
            }

        }
    }
}
