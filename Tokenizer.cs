using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LemonCSharp
{
    public delegate bool CharIsHandler(char ch);
    public delegate Token TokenHandler(char ch);

    public enum TokenType : int
    {
        NULL = 0,
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
        CHAR_0x21 = 0x21, NOT = 0x21, EXCL = 0x21,//!     exclamation mark 惊叹号
        CHAR_0x22 = 0x22, DQUOT = 0x22,//"    double quotation marks 双引号
        CHAR_0x23 = 0x23, SHARP = 0x23,//#
        CHAR_0x24 = 0x24, DOLLAR = 0x24,//$
        CHAR_0x25 = 0x25, PERCENT = 0x25, MOD = 0x25,//%
        CHAR_0x26 = 0x26, AND = 0x26,//&
        CHAR_0x27 = 0x27, SQUOT = 0x27,  //'    single quotation marks 单引号
        CHAR_0x28 = 0x28, LPAR = 0x28,//( parenthesis or round brackets 圆括号
        CHAR_0x29 = 0x29, RPAR = 0x29,//) parenthesis or round brackets 圆括号
        CHAR_0x2A = 0x2A, MUL = 0x2A, STAR = 0x2A,//*
        CHAR_0x2B = 0x2B, ADD = 0x2B, PLUS = 0x2B,//+
        CHAR_0x2C = 0x2C, COMMA = 0x2C,//,
        CHAR_0x2D = 0x2D, SUB = 0x2D, MIMUS = 0x2D,//-
        CHAR_0x2E = 0x2E, DOT = 0x2E, PERIOD = 0x2E,//. period or full stop 句号
        CHAR_0x2F = 0x2F, DIV = 0x2F, DIVIDE = 0x2F, SLASH = 0x2F,/// divided slash or virgule or diagonal mark 斜线号
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
        CHAR_0x3A = 0x3A, COLON = 0x3A,//: colon 冒号
        CHAR_0x3B = 0x3B, SEMI = 0x3B,//; semicolon 分号
        CHAR_0x3C = 0x3C, LE = 0x3C, LANGLE = 0x3C,//< Angle brackets 尖括号
        CHAR_0x3D = 0x3D, EQ = 0x3D,//=
        CHAR_0x3E = 0x3E, GE = 0x3E, RANGLE = 0x3E,//>
        CHAR_0x3F = 0x3F, ASK = 0x3F, QUESTION = 0x3F,//?
        CHAR_0x40 = 0x40, AT = 0x40,//@
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
        CHAR_0x5B = 0x5B, LSQUARE = 0x5B,//[ square brackets 方括号
        CHAR_0x5C = 0x5C, BSLASH = 0x5C,//\ backslash; trailing slash; bslash 反斜杠
        CHAR_0x5D = 0x5D, RSQUARE = 0x5D,//]
        CHAR_0x5E = 0x5E, POWER = 0x5E,//^
        CHAR_0x5F = 0x5F, UNDERLINE = 0x5F,//_ underline
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
        CHAR_0x7B = 0x7B, LBRACES = 0x7B,//{ curly brackets or braces 大括号
        CHAR_0x7C = 0x7C, OR = 0x7C,//|
        CHAR_0x7D = 0x7D, RBRACES = 0x7D,//} curly brackets or braces 大括号
        CHAR_0x7E = 0x7E, NEG = 0x7E,//~

        NUM = 0x100, INT = NUM, FLOAT,  ID, KEYWORD, STRING, 
        SINGLE_LINE_COMMENT, MULTI_LINE_COMMENT,
        ERROR,

        CODE_BLOCK = 0x10000,
        DEFINE,
        ARROW,

        LAST_ONE =ERROR
    };

    public class Token
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

        public bool IsType(char ch)
        {
            return ((int)type == (int)ch);
        }

        public bool IsType(TokenType ttype)
        {
            return (this.type == ttype);
        }
    }

    public class Tokenizer
    {
        string Text;
        int row;
        int col;
        int offset;
        bool IgnoreSpaceToken { get; set; }
        Token cur_token;
        public const char EOF = (char)0;
        Dictionary<char, TokenHandler> TokenHandlers= new Dictionary<char, TokenHandler>();
        public TokenHandler DefaultTokenHandler { get; set; }

        public string DelimitationChars = " \t\r\n([{.";
        public string WhiteSpaceChars = " \t\r\n";

        public Tokenizer(string text)
        {
            this.Text = text;
            row = 0;
            col = 0;
            offset = 0;
            cur_token = null;
            IgnoreSpaceToken = true;
            SetTokenHandler('/',  Comment_Token);
            SetTokenHandler('\'', String_Token);
            SetTokenHandler('\"', String_Token);
            DefaultTokenHandler = Default_Token_handler;
        }

        public virtual string TokenName(TokenType TkType)
        {
            if (TkType < TokenType.LAST_ONE)
            {
                return Enum.GetName(typeof(TokenType), TkType);
            }
            else
            {
                return ((uint)TkType).ToString();
            }
        }

        public void SetTokenHandler(char ch, TokenHandler handler)
        {
            if (handler == null && TokenHandlers.ContainsKey(ch))
            {
                TokenHandlers.Remove(ch);
            }
            else TokenHandlers[ch] = handler;
        }

        public char read_char()
        {//先移动指针再读
            if (offset >= Text.Length) return EOF;
            char ch = Text[offset++];
            if (ch == '\n')
            {
                row++;
                col = 0;
            }
            else col++;
            return ch;
        }

        public char peek_char()
        {
            return (offset >= Text.Length) ? EOF : Text[offset];
        }

        public char next_char1()
        {//不移动读指针
            return (offset + 0 >= Text.Length) ? EOF : Text[offset + 0];
        }

        public char next_char2()
        {//不移动读指针
            return (offset + 1 >= Text.Length) ? EOF : Text[offset + 1];
        }

        public char next_charX(int x)
        {//不移动读指针
            if (x <= 0) return EOF;
            x--;
            return (offset + x >= Text.Length) ? EOF : Text[offset + x];
        }

        public void skip_white_space()
        {//返回WhiteSpace之后的第一个字符
            for (char ch = peek_char(); ch != EOF; read_char(),ch = peek_char())
            {
                if (!is_white_space(ch))
                {
                    break;
                }
            }
        }

        public virtual bool is_white_space(char ch)
        {
            return WhiteSpaceChars.IndexOf(ch) >= 0;
        }

        public virtual bool is_delimitation_char(char ch)
        {
            return DelimitationChars.IndexOf(ch) >= 0;
        }

        public bool start_with(char start_ch)
        {
            if (offset >= Text.Length) return false;
            return start_ch == Text[offset];
        }

        public bool start_with(char start_ch1, char start_ch2)
        {
            if (offset + 1>= Text.Length) return false;
            return (start_ch1 == Text[offset]) && (start_ch2 == Text[offset+1]);
        }

        public bool start_with(char start_ch1, char start_ch2, char start_ch3)
        {
            if (offset + 2 >= Text.Length) return false;
            return (start_ch1 == Text[offset]) &&
                (start_ch2 == Text[offset + 1]) &&
                (start_ch3 == Text[offset + 2]);
        }

        bool start_with(string str)
        {
            if (offset + str.Length >= Text.Length) return false;
            string substr = Text.Substring(offset, str.Length);
            return str == substr;
        }

        public bool skip_chars(int count = 1)
        {//移动读指针，跟踪处理行、列和偏移量
            if (count <= 0) return false;
            for (int i = 0;  i < count; i++)
            {
                if (offset >= Text.Length)  return false;
                char ch = Text[offset++];
                if (ch == '\n')
                {
                    row++;
                    col = 0;
                }
                else col++;
            }
            return true;
        }

        public bool skip_line()
        {//移动读指针，跟踪处理行、列和偏移量
            for (char ch = peek_char(); ch != EOF; read_char(), ch = peek_char())
            {
                if (ch == '\n')
                {
                    read_char();
                    return true;
                }
            }
            return false;
        }

        public bool skip_until(char end_ch)
        {//移动读指针，跟踪处理行、列和偏移量
            for (char ch = read_char(); ch != EOF; ch=read_char())
            {
                if (ch == end_ch)
                {
                    return true;
                }
            }
            return false;
        }

        public bool skip_until(string end_str)
        {// 移动读指针，跟踪处理行、列和偏移量
            char ch0 = end_str[0];
            for (char ch = peek_char(); ch != EOF; read_char(), ch = peek_char())
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

        public bool skip_if(CharIsHandler char_is)
        {
            for (char ch = peek_char(); ch != EOF; read_char(), ch = peek_char())
            {
                if (!char_is(ch))
                {
                    return true;
                }
            }
            return false;
        }

        public Token end_token(TokenType tktype = TokenType.NULL)
        {
            cur_token.End(row, col, offset);
            cur_token.text = Text.Substring(cur_token.start_offset, cur_token.end_offset - cur_token.start_offset);
            if (tktype != TokenType.NULL) cur_token.type = tktype;
            if (cur_token.text.Length == 0) cur_token.type = TokenType.ERROR;
            string tk = string.Format("[{0},{1}] {2} : {3}", 
                cur_token.start_line+1, 
                cur_token.start_column+1, 
                TokenName(cur_token.type), 
                cur_token.text);
            Console.WriteLine(tk);
            return cur_token;
        }

        public Token bad_token()
        {
            return end_token(TokenType.ERROR);
        }

        protected virtual Token Default_Token_handler(char ch)
        {
            if (char.IsDigit(ch))
            {
                if (ch == '0')
                {
                    char next_ch = next_char1();
                    if (next_ch == 'x' || next_ch == 'X')
                    {
                        ch = read_char();//跳过x|X
                        return Hex_Token(ch);
                    }
                }
                return Number_Token(ch);
            }
            else if (char.IsLetter(ch) || ch == '_')
            {
                return Identifier_Token(ch);
            }
            return null;
        }

        protected virtual Token Comment_Token(char ch)
        {
            char next_ch = next_char1();
            if (next_ch == '/')
            {
                skip_until('\n');
                return end_token(TokenType.SINGLE_LINE_COMMENT);
            }
            else if (next_ch == '*')
            {
                skip_until("*/");
                return end_token(TokenType.MULTI_LINE_COMMENT);
            }
            return null;
        }

        protected virtual Token String_Token(char start_ch)
        {
            //处理C语言转义字符
            char prev_ch = EOF;
            for (char ch = peek_char(); ch != EOF; read_char(), ch = peek_char())
            {
                if (ch == start_ch && prev_ch != '\\')
                {
                    read_char();
                    return end_token(TokenType.STRING);
                }
                if (prev_ch == '\\') prev_ch = EOF;
                else prev_ch = ch;
            }
            return end_token(TokenType.ERROR);
        }

        protected virtual Token Identifier_Token(char cur_ch)
        {
            for (char ch = peek_char(); ch != EOF; read_char(), ch = peek_char())
            {
                if (ch != '_' && !char.IsLetterOrDigit(ch))
                {
                    //if (is_delimitation_char(ch)) 
                    break;
                }
            }
            return end_token(TokenType.ID);
        }

        protected virtual Token Hex_Token(char cur_ch)
        {
            //if (cur_ch != 'x' && cur_ch != 'X') return null;
            for (char ch = peek_char(); ch != EOF; read_char(), ch = peek_char())
            {
                if (char.IsDigit(ch) || (ch >= 'a' && ch <= 'f') || (ch >= 'A' && ch <= 'F'))
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            return end_token(TokenType.NUM);
        }

        protected virtual Token Number_Token(char cur_ch)
        {
            for (char ch = peek_char(); ch != EOF; read_char(), ch = peek_char())
            {
                if (ch == '.')
                {
                    ch = read_char();
                    return Float_Token(ch);
                }
                if (!char.IsDigit(ch))
                {
                    break;
                }
            }
            return end_token(TokenType.NUM);
        }

        protected virtual Token Float_Token(char cur_ch)
        {
            //if (cur_ch != '.') return null;
            for (char ch = peek_char(); ch != EOF; read_char(), ch = peek_char())
            {
                if (!char.IsDigit(ch))
                {
                    break;
                }
            }
            return end_token(TokenType.FLOAT);
        }

        protected virtual void Log(string msg)
        {
            Console.WriteLine(msg);
        }

        protected void Log(Exception ex)
        {
            Log(ex.Message);
        }

        void Error(string errmsg)
        {
            string str = string.Format("[{0},{1}]:{2}", row + 1, col + 1, errmsg);
            throw new ObjectDisposedException(str);
        }

        public Token next_token()
        {
            try
            {
                if (IgnoreSpaceToken) skip_white_space();
                cur_token = new Token(row, col, offset);
                char cur_ch = read_char();
                if (cur_ch == EOF) return null;
                char next_ch = next_char1();
                if (TokenHandlers.ContainsKey(cur_ch))
                {
                    TokenHandler handler = TokenHandlers[cur_ch];
                    Token tk = handler(cur_ch);
                    if (tk != null) return tk;
                }
                if (DefaultTokenHandler != null)
                {
                    Token tk = DefaultTokenHandler(cur_ch);
                    if (tk != null) return tk;
                }
                return end_token((TokenType)cur_ch);
            }
            catch(Exception ex)
            {
                Log(ex);
            }
            return null;
        }

        static public void EnumTokenType()
        {
            for (char ch = (char)0; ch < 0x7f; ch++)
            {
                if (ch < 0x20)
                {
                    Console.WriteLine(string.Format("CHAR_0x{0:X2}=0x{0:X2},", (int)ch));
                }
                else if (char.IsLetterOrDigit(ch))
                {
                    Console.WriteLine(string.Format("CHAR_{1}=0x{0:X},//{1}", (int)ch, ch));
                }
                else
                {
                    Console.WriteLine(string.Format("CHAR_0x{0:X}=0x{0:X},//{1}", (int)ch, ch));
                }
            }
        }
    }
}
