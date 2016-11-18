using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LemonCSharp
{
    public enum ActionType
    {
        SHIFT,
        ACCEPT,
        REDUCE,
        ERROR,
        SSCONFLICT, // A shift/shift conflict
        SRCONFLICT, // Was a reduce, but part of a conflict
        RRCONFLICT, // Was a reduce, but part of a conflict
        SH_RESOLVED, // Was a shift.  Precedence resolved conflict
        RD_RESOLVED, // Was reduce.  Precedence resolved conflict
        NOT_USED // Deleted by compression
    }

    public class Action
    {
        public Symbol sp; // The look-ahead symbol
        public ActionType type;
        //C++ TO C# CONVERTER TODO TASK: Unions are not supported in C#:
        //ORIGINAL LINE: union
        //C++ TO C# CONVERTER NOTE: Structs must be named in C#, so the following struct has been named AnonymousStruct:
        public struct AnonymousStruct
        {
            public State stp; // The new state, if a shift
            public Rule[] rp; // The rule, if a reduce
        }
        public AnonymousStruct x = new AnonymousStruct();
        public Action next; // Next action for this state
        public Action collide; // Next action with the same hash
    }


    public class State
    {
        //public config bp; // The basis configurations for this state
        //public config cfp; // All configurations in this set
        public int statenum; // Sequential number for this state
        public Action ap; // Array of actions for this state
        public int nTknAct; // Number of actions on terminals and nonterminals
        public int nNtAct;
        public int iTknOfst; // yy_action[] offset for terminals and nonterms
        public int iNtOfst;
        public int iDflt; // Default action
    }

    public partial class Lemon
    {
        public List<State> sorted= new List<State>(); // Table of states sorted by state number
        public Rule rule; // List of all rules
        public Rule startRule; // First rule
        public int nstate; // Number of states
        public int nxstate; // nstate with tail degenerate states removed
        public int nrule; // Number of rules
        public int nsymbol; // Number of terminal and nonterminal symbols
        public int nterminal; // Number of terminal symbols
        //public List<Symbol> symbols=new List<Symbol>(); // Sorted array of pointers to symbols
        public int errorcnt; // Number of errors
        public Symbol errsym; // The error symbol
        public Symbol wildcard; // Token that matches anything
        public string name; // Name of the generated parser
        public string arg; // Declaration of the 3th argument to parser
        public string tokentype; // Type of terminal symbols in the parser stack
        public string vartype; // The default type of non-terminal symbols
        public string start; // Name of the start symbol for the grammar
        public string stacksize; // Size of the parser stack
        public string include; // Code to put at the start of the C file
        public string error; // Code to execute when an error is seen
        public string overflow; // Code to execute on a stack overflow
        public string failure; // Code to execute on parser failure
        public string accept; // Code to execute when the parser excepts
        public string extracode; // Code appended to the generated file
        public string tokendest; // Code to execute to destroy token data
        public string vardest; // Code for the default non-terminal destructor
        public string filename; // Name of the input file
        public string outname; // Name of the current output file
        public string tokenprefix; // A prefix added to token names in the .h file
        public int nconflict; // Number of parsing conflicts
        public int nactiontab; // Number of entries in the yy_action[] table
        public int tablesize; // Total table size of all tables in bytes
        public int basisflag; // Print only basis configurations
        public int has_fallback; // True if any %fallback is seen in the grammar
        public int nolinenosflag; // True if #line statements should not be printed
        public string argv0; // Name of the program

        /********************重新定义************************/
        public Token CodeToken;
        public Token IncludeToken;
        public Token StartToken;

    }

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

    public class Lemon_Parser
    {
        public string filename; // Name of the input file
        public int tokenlineno; // Linenumber at which current token starts
        public int errorcnt; // Number of errors so far
        public string tokenstart; // Text of current token
        public Lemon gp=new Lemon(); // Global state vector
        public ParserState state; // The state of the parser
        public Symbol fallback; // The fallback token
        public Symbol tkclass; // Token class symbol
        public Symbol lhs; // Left-hand side of current rule
        public string lhsalias; // Alias for the LHS
        public int nrhs;//{ get { return rhs.Count; } } // Number of right-hand side symbols seen
        public List<Symbol> rhs = new List<Symbol>(); // RHS symbols
        public List<string> alias = new List<string>(); // Aliases for each RHS symbol (or NULL)
        public Rule prevrule; // Previous rule parsed
        public string declkeyword; // Keyword of a declaration
        public List<string> declargslot=new List<string>(); // Where the declaration argument should be put
        public int insertLineMacro; // Add #line before declaration insert
        public int[] decllinenoslot; // Where to write declaration line number
        public Assoc declassoc; // Assign this association to decl arguments
        public int preccounter; // Assign this precedence to decl arguments
        public Rule firstrule; // Pointer to first rule in the grammar
        public Rule lastrule; // Pointer to the most recently parsed rule
    }

    //public class RuleItem
    //{
    //    public Symbol symbol;
    //    public string alias;
    //    public RuleItem(Symbol symbol, string alias)
    //    {
    //        this.symbol = symbol;
    //        this.alias = alias;
    //    }
    //}

    public class Rule
    {
        public Symbol left;
        public string left_alias;

        public List<Symbol> right = new List<Symbol>();
        public List<string> right_alais = new List<string>();

        public Symbol precedence;

        //public RuleItem lhs; // Left-hand side of the rule
        public int lhsStart; // True if left-hand side is the start symbol
        public int ruleline; // Line number for the rule
        public int nrhs; // Number of RHS symbols
        //public List<RuleItem> rhs=new List<RuleItem>(); // The RHS symbols
        public int line; // Line number at which code begins
        public string code; // The code executed when this rule is reduced
        public string codePrefix; // Setup code before code[] above
        public string codeSuffix; // Breakdown code after code[] above
        public int noCode; // True if this rule has no associated C code
        public int codeEmitted; // True if the code has been emitted already
        public Symbol precsym; // Precedence symbol for this rule
        public int index; // An index number for this rule
        public int iRule; // Rule number as used in the generated tables
        public Boolean canReduce; // True if this rule is ever reduced
        public Boolean doesReduce; // Reduce actions occur after optimization
        public Rule nextlhs; // Next rule with the same LHS
        public Rule next; // Next rule in the global list
    }
     
    public enum SymbolType
    {
        TERMINAL,
        NONTERMINAL,
        MULTITERMINAL
    }
    public enum Assoc
    {
        LEFT=0,
        RIGHT,
        NONE,
        UNK
    }

    public class Symbol
    {
        public string name; // Name of the symbol
        public int index; // Index number for this symbol
        public SymbolType type; // Symbols are all either TERMINALS or NTs
        public Rule rule; // Linked list of rules of this (if an NT)
        public Symbol fallback; // fallback token in case this token doesn't parse
        public int prec; // Precedence if defined (-1 otherwise)
        public Assoc assoc; // Associativity if precedence is defined
        //public bool[] firstset; // First-set for all rules of this symbol
        public HashSet<Symbol> first= new HashSet<Symbol>();

        //public Boolean HasEmpty; // True if NT and can generate an empty string
        public bool HasEmpty { get; set; }
        public bool IsTerminal { get { return type == SymbolType.TERMINAL; } }
        public int useCnt; // Number of times used
        public string destructor; /* Code which executes whenever this symbol is
							 ** popped from the stack during error processing */
        public int destLineno; // Line number for start of destructor
        public string datatype; /* The data type of information held by this
							 ** object. Only used if type==NONTERMINAL */
        public int dtnum; /* The data type number.  In the parser, the value
							 ** stack is a union.  The .yy%d element of this
							 ** union is the correct data type for this object */
                          /* The following fields are used by MULTITERMINALs only */
        public int nsubsym; // Number of constituent symbols in the MULTI
        public List<Symbol> subsym; // Array of constituent symbols

        public Symbol(string text)
        {
            this.name = text;
            this.prec = -1;
            if (char.IsUpper(text[0]))
            {
                this.type = SymbolType.TERMINAL;
            }
            else this.type = SymbolType.NONTERMINAL;
        }
    }

}
