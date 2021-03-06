﻿using System;
using System.Diagnostics;

/*
** This file contains all sources (including headers) to the LEMON
** LALR(1) parser generator.  The sources have been combined into a
** single file to make it easy to include LEMON in the source tree
** and Makefile of another program.
**
** The author of this program disclaims copyright.
*/

//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define ISSPACE(X) isspace((unsigned char)(X))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define ISDIGIT(X) isdigit((unsigned char)(X))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define ISALNUM(X) isalnum((unsigned char)(X))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define ISALPHA(X) isalpha((unsigned char)(X))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define ISUPPER(X) isupper((unsigned char)(X))
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define ISLOWER(X) islower((unsigned char)(X))


#if ! __WIN32__
#if _WIN32 || WIN32
#define __WIN32__
#endif
#endif

#if __WIN32__
#if __cplusplus
#endif
#endif

/****** From the file "option.h" ******************************************/
public enum option_type
{
    OPT_FLAG = 1,
    OPT_INT,
    OPT_DBL,
    OPT_STR,
    OPT_FFLAG,
    OPT_FINT,
    OPT_FDBL,
    OPT_FSTR
}
public class s_options
{
    public option_type type;
    public readonly string label;
    public string arg;
    public readonly string message;
}
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define SetFind(X,Y) (X[Y])

/********** From the file "struct.h" *************************************/
/*
** Principal data structures for the LEMON parser generator.
*/

public enum Boolean
{
    LEMON_FALSE = 0,
    LEMON_TRUE
}

/* Symbols (terminals and nonterminals) of the grammar are stored
** in the following: */
public enum symbol_type
{
    TERMINAL,
    NONTERMINAL,
    MULTITERMINAL
}
public enum e_assoc
{
    LEFT,
    RIGHT,
    NONE,
    UNK
}
public class symbol
{
    public readonly string name; // Name of the symbol
    public int index; // Index number for this symbol
    public symbol_type type; // Symbols are all either TERMINALS or NTs
    public rule rule; // Linked list of rules of this (if an NT)
    public symbol fallback; // fallback token in case this token doesn't parse
    public int prec; // Precedence if defined (-1 otherwise)
    public e_assoc assoc; // Associativity if precedence is defined
    public string firstset; // First-set for all rules of this symbol
    public Boolean lambda; // True if NT and can generate an empty string
    public int useCnt; // Number of times used
    public string destructor; /* Code which executes whenever this symbol is
							 ** popped from the stack during error processing */
    public int destLineno; /* Line number for start of destructor.  Set to
							 ** -1 for duplicate destructors. */
    public string datatype; /* The data type of information held by this
							 ** object. Only used if type==NONTERMINAL */
    public int dtnum; /* The data type number.  In the parser, the value
							 ** stack is a union.  The .yy%d element of this
							 ** union is the correct data type for this object */
                      /* The following fields are used by MULTITERMINALs only */
    public int nsubsym; // Number of constituent symbols in the MULTI
    public symbol[] subsym; // Array of constituent symbols
}

/* Each production rule in the grammar is stored in the following
** structure.  */
public class rule
{
    public symbol lhs; // Left-hand side of the rule
    public readonly string lhsalias; // Alias for the LHS (NULL if none)
    public int lhsStart; // True if left-hand side is the start symbol
    public int ruleline; // Line number for the rule
    public int nrhs; // Number of RHS symbols
    public symbol[] rhs; // The RHS symbols
    public readonly string[] rhsalias; // An alias for each RHS symbol (NULL if none)
    public int line; // Line number at which code begins
    public readonly string code; // The code executed when this rule is reduced
    public readonly string codePrefix; // Setup code before code[] above
    public readonly string codeSuffix; // Breakdown code after code[] above
    public int noCode; // True if this rule has no associated C code
    public int codeEmitted; // True if the code has been emitted already
    public symbol precsym; // Precedence symbol for this rule
    public int index; // An index number for this rule
    public int iRule; // Rule number as used in the generated tables
    public Boolean canReduce; // True if this rule is ever reduced
    public Boolean doesReduce; // Reduce actions occur after optimization
    public rule nextlhs; // Next rule with the same LHS
    public rule next; // Next rule in the global list
}

/* A configuration is a production rule of the grammar together with
** a mark (dot) showing how much of that rule has been processed so far.
** Configurations also contain a follow-set which is a list of terminal
** symbols which are allowed to immediately follow the end of the rule.
** Every configuration is recorded as an instance of the following: */
public enum cfgstatus
{
    COMPLETE,
    INCOMPLETE
}
public class config
{
    public rule[] rp; // The rule upon which the configuration is based
    public int dot; // The parse point
    public string fws; // Follow-set for this configuration only
    public plink fplp; // Follow-set forward propagation links
    public plink bplp; // Follow-set backwards propagation links
    public state stp; // Pointer to state which contains this
    public cfgstatus status; // used during followset and shift computations
    public config next; // Next configuration in the state
    public config bp; // The next basis configuration
}

public enum e_action
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
    NOT_USED, // Deleted by compression
    SHIFTREDUCE // Shift first, then reduce
}

/* Every shift or reduce operation is stored as one of the following */
public class action
{
    public symbol sp; // The look-ahead symbol
    public e_action type;
    //C++ TO C# CONVERTER TODO TASK: Unions are not supported in C#:
    //ORIGINAL LINE: union
    //C++ TO C# CONVERTER NOTE: Structs must be named in C#, so the following struct has been named AnonymousStruct:
    public struct AnonymousStruct
    {
        public state stp; // The new state, if a shift
        public rule[] rp; // The rule, if a reduce
    }
    public AnonymousStruct x = new AnonymousStruct();
    public symbol spOpt; // SHIFTREDUCE optimization to this symbol
    public action next; // Next action for this state
    public action collide; // Next action with the same hash
}

/* Each state of the generated parser's finite state machine
** is encoded as an instance of the following structure. */
public class state
{
    public config bp; // The basis configurations for this state
    public config cfp; // All configurations in this set
    public int statenum; // Sequential number for this state
    public action ap; // List of actions for this state
    public int nTknAct; // Number of actions on terminals and nonterminals
    public int nNtAct;
    public int iTknOfst; // yy_action[] offset for terminals and nonterms
    public int iNtOfst;
    public int iDfltReduce; // Default action is to REDUCE by this rule
    public rule pDfltReduce; // The default REDUCE rule.
    public int autoReduce; // True if this is an auto-reduce state
}

/* A followset propagation link indicates that the contents of one
** configuration followset should be propagated to another whenever
** the first changes. */
public class plink
{
    public config cfp; // The configuration to which linked
    public plink next; // The next propagate link
}

/* The state vector for the entire parser generator is recorded as
** follows.  (LEMON uses no global variables and makes little use of
** static variables.  Fields in the following structure can be thought
** of as begin global variables in the program.) */
public class lemon
{
    public state[] sorted; // Table of states sorted by state number
    public rule rule; // List of all rules
    public rule startRule; // First rule
    public int nstate; // Number of states
    public int nxstate; // nstate with tail degenerate states removed
    public int nrule; // Number of rules
    public int nsymbol; // Number of terminal and nonterminal symbols
    public int nterminal; // Number of terminal symbols
    public symbol[] symbols; // Sorted array of pointers to symbols
    public int errorcnt; // Number of errors
    public symbol errsym; // The error symbol
    public symbol wildcard; // Token that matches anything
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
}
/********************** New code to implement the "acttab" module ***********/
/*
** This module implements routines use to construct the yy_action[] table.
*/

/*
** The state of the yy_action table under construction is an instance of
** the following structure.
**
** The yy_action table maps the pair (state_number, lookahead) into an
** action_number.  The table is an array of integers pairs.  The state_number
** determines an initial offset into the yy_action array.  The lookahead
** value is then added to this initial offset to get an index X into the
** yy_action array. If the aAction[X].lookahead equals the value of the
** of the lookahead input, then the value of the action_number output is
** aAction[X].action.  If the lookaheads do not match then the
** default action for the state_number is returned.
**
** All actions associated with a single state_number are first entered
** into aLookahead[] using multiple calls to acttab_action().  Then the
** actions for that single state_number are placed into the aAction[]
** array with a single call to acttab_insert().  The acttab_insert() call
** also resets the aLookahead[] array in preparation for the next
** state number.
*/
public class lookahead_action
{
    public int lookahead; // Value of the lookahead token
    public int action; // Action to take on the given lookahead
}
public class acttab
{
    public int nAction; // Number of used slots in aAction[]
    public int nActionAlloc; // Slots allocated for aAction[]
    public lookahead_action[] aAction; // A single new transaction set -  The yy_action[] table under construction
    public lookahead_action[] aLookahead;
    public int mnLookahead; // Minimum aLookahead[].lookahead
    public int mnAction; // Action associated with mnLookahead
    public int mxLookahead; // Maximum aLookahead[].lookahead
    public int nLookahead; // Used slots in aLookahead[]
    public int nLookaheadAlloc; // Slots allocated in aLookahead[]
}
/*********************** From the file "parse.c" ****************************/
/*
** Input file parser for the LEMON parser generator.
*/

/* The state of the parser */
public enum e_state
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
public class pstate
{
    public string filename; // Name of the input file
    public int tokenlineno; // Linenumber at which current token starts
    public int errorcnt; // Number of errors so far
    public string tokenstart; // Text of current token
    public lemon gp; // Global state vector
    public e_state state; // The state of the parser
    public symbol fallback; // The fallback token
    public symbol tkclass; // Token class symbol
    public symbol lhs; // Left-hand side of current rule
    public readonly string lhsalias; // Alias for the LHS
    public int nrhs; // Number of right-hand side symbols seen
    public symbol[] rhs = Arrays.InitializeWithDefaultInstances<symbol>(DefineConstants.MAXRHS); // RHS symbols
    public readonly string[] alias = new string[DefineConstants.MAXRHS]; // Aliases for each RHS symbol (or NULL)
    public rule prevrule; // Previous rule parsed
    public readonly string declkeyword; // Keyword of a declaration
    public string[] declargslot; // Where the declaration argument should be put
    public int insertLineMacro; // Add #line before declaration insert
    public int[] decllinenoslot; // Where to write declaration line number
    public e_assoc declassoc; // Assign this association to decl arguments
    public int preccounter; // Assign this precedence to decl arguments
    public rule firstrule; // Pointer to first rule in the grammar
    public rule lastrule; // Pointer to the most recently parsed rule
}

/*
** Each state contains a set of token transaction and a set of
** nonterminal transactions.  Each of these sets makes an instance
** of the following structure.  An array of these structures is used
** to order the creation of entries in the yy_action[] table.
*/
public class axset
{
    public state stp; // A pointer to a state
    public int isTkn; // True to use tokens.  False for non-terminals
    public int nAction; // Number of actions
    public int iOrder; // Original order of action sets
}

/* There is one instance of the following structure for each
** associative array of type "x1".
*/
public class s_x1
{
    public int size; // The number of available slots.
                     /*   Must be a power of 2 greater than or */
                     /*   equal to 1 */
    public int count; // Number of currently slots filled
    public s_x1node tbl; // The data stored here
    public s_x1node[] ht; // Hash table for lookups
}

/* There is one instance of this structure for every data element
** in an associative array of type "x1".
*/
public class s_x1node
{
    public readonly string data; // The data
    public s_x1node next; // Next entry with the same hash
    public s_x1node[] from; // Previous link
}

/* There is one instance of the following structure for each
** associative array of type "x2".
*/
public class s_x2
{
    public int size; // The number of available slots.
                     /*   Must be a power of 2 greater than or */
                     /*   equal to 1 */
    public int count; // Number of currently slots filled
    public s_x2node tbl; // The data stored here
    public s_x2node[] ht; // Hash table for lookups
}

/* There is one instance of this structure for every data element
** in an associative array of type "x2".
*/
public class s_x2node
{
    public symbol data; // The data
    public readonly string key; // The key
    public s_x2node next; // Next entry with the same hash
    public s_x2node[] from; // Previous link
}

/* There is one instance of the following structure for each
** associative array of type "x3".
*/
public class s_x3
{
    public int size; // The number of available slots.
                     /*   Must be a power of 2 greater than or */
                     /*   equal to 1 */
    public int count; // Number of currently slots filled
    public s_x3node tbl; // The data stored here
    public s_x3node[] ht; // Hash table for lookups
}

/* There is one instance of this structure for every data element
** in an associative array of type "x3".
*/
public class s_x3node
{
    public state data; // The data
    public config key; // The key
    public s_x3node next; // Next entry with the same hash
    public s_x3node[] from; // Previous link
}

/* There is one instance of the following structure for each
** associative array of type "x4".
*/
public class s_x4
{
    public int size; // The number of available slots.
                     /*   Must be a power of 2 greater than or */
                     /*   equal to 1 */
    public int count; // Number of currently slots filled
    public s_x4node tbl; // The data stored here
    public s_x4node[] ht; // Hash table for lookups
}

/* There is one instance of this structure for every data element
** in an associative array of type "x4".
*/
public class s_x4node
{
    public config data; // The data
    public s_x4node next; // Next entry with the same hash
    public s_x4node[] from; // Previous link
}

public static class GlobalMembers
{
    //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
    //	int access(string path, int mode);
#if __cplusplus
#endif
#else

    /* #define PRIVATE static */

#if TEST
#define MAXRHS
#else
#define MAXRHS
#endif

    internal static int showPrecedenceConflict = 0;
    private delegate int cmpDelegate(string NamelessParameter1, string NamelessParameter2);
    internal static string msort(ref string list, string[] next, cmpDelegate cmp)
    {
        uint offset;
        string ep;
        string[] set = new string[DefineConstants.LISTSIZE];
        int i;
        offset = (uint)((string)next - (string)list);
        for (i = 0; i < DefineConstants.LISTSIZE; i++)
        {
            set[i] = 0;
        }
        while (list != 0)
        {
            ep = list;
            list = ((string)(((string)list) + offset));
            ((string)(((string)ep) + offset)) = 0;
            for (i = 0; i < DefineConstants.LISTSIZE - 1 && set[i] != 0; i++)
            {
                ep = merge(ref ep, ref set[i], cmp, offset);
                set[i] = 0;
            }
            set[i] = ep;
        }
        ep = 0;
        for (i = 0; i < DefineConstants.LISTSIZE; i++)
        {
            if (set[i] != 0)
            {
                ep = merge(ref set[i], ref ep, cmp, offset);
            }
        }
        return ep;
    }

    /*
	** Compilers are getting increasingly pedantic about type conversions
	** as C evolves ever closer to Ada....  To work around the latest problems
	** we have to define the following variant of strlen().
	*/
    //C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
    //ORIGINAL LINE: #define lemonStrlen(X) ((int)strlen(X))

    /*
	** Compilers are starting to complain about the use of sprintf() and strcpy(),
	** saying they are unsafe.  So we define our own versions of those routines too.
	**
	** There are three routines here:  lemon_sprintf(), lemon_vsprintf(), and
	** lemon_addtext(). The first two are replacements for sprintf() and vsprintf().
	** The third is a helper routine for vsnprintf() that adds texts to the end of a
	** buffer, making sure the buffer is always zero-terminated.
	**
	** The string formatter is a minimal subset of stdlib sprintf() supporting only
	** a few simply conversions:
	**
	**   %d
	**   %s
	**   %.*s
	**
	*/
    //C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on the parameter 'pnUsed', so pointers on this parameter are left unchanged:
    internal static void lemon_addtext(ref string zBuf, int* pnUsed, string zIn, int nIn, int iWidth)
    { // Field width.  Negative to left justify -  Bytes of text to add.  -1 to use strlen() -  Text to add -  Slots of the buffer used so far -  The buffer to which text is added
        if (nIn < 0)
        {
            for (nIn = 0; zIn[nIn]; nIn++)
            {
            }
        }
        while (iWidth > nIn)
        {
            zBuf[(*pnUsed)++] = ' ';
            iWidth--;
        }
        if (nIn == 0)
        {
            return;
        }
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
        memcpy(zBuf[*pnUsed], zIn, nIn);
        *pnUsed += nIn;
        while ((-iWidth) > nIn)
        {
            zBuf[(*pnUsed)++] = ' ';
            iWidth++;
        }
        zBuf = zBuf.Substring(0, *pnUsed);
    }
    internal static int lemon_vsprintf(ref string str, string zFormat, va_list ap)
    {
        int i;
        int j;
        int k;
        int c;
        int nUsed = 0;
        string z;
        string zTemp = new string(new char[50]);
        str = null;
        for (i = j = 0; (c = zFormat[i]) != 0; i++)
        {
            if (c == '%')
            {
                int iWidth = 0;
                lemon_addtext(ref str, nUsed, zFormat[j], i - j, 0);
                c = zFormat[++i];
                if (char.IsDigit((byte)(c)) || (c == '-' && char.IsDigit((byte)(zFormat[i + 1]))))
                {
                    if (c == '-')
                    {
                        i++;
                    }
                    while (char.IsDigit((byte)(zFormat[i])))
                    {
                        iWidth = iWidth * 10 + zFormat[i++] - '0';
                    }
                    if (c == '-')
                    {
                        iWidth = -iWidth;
                    }
                    c = zFormat[i];
                }
                if (c == 'd')
                {
                    int v = va_arg(ap, int);
                    if (v < 0)
                    {
                        lemon_addtext(ref str, nUsed, "-", 1, iWidth);
                        v = -v;
                    }
                    else if (v == 0)
                    {
                        lemon_addtext(ref str, nUsed, "0", 1, iWidth);
                    }
                    k = 0;
                    while (v > 0)
                    {
                        k++;
                        zTemp = StringFunctions.ChangeCharacter(zTemp, sizeof(sbyte) - k, (v % 10) + '0');
                        v /= 10;
                    }
                    lemon_addtext(ref str, nUsed, zTemp[sizeof(sbyte) - k], k, iWidth);
                }
                else if (c == 's')
                {
                    z = va_arg(ap, const sbyte*);
        lemon_addtext(ref str, nUsed, z, -1, iWidth);
    }
//C++ TO C# CONVERTER TODO TASK: The memory management function 'memcmp' has no equivalent in C#:
				else if (c == '.' && memcmp(zFormat[i], ".*s", 3) == 0)
				{
					i += 2;
					k = va_arg(ap, int);
    z = va_arg(ap, const sbyte*);

                    lemon_addtext(ref str, nUsed, z, k, iWidth);
}
				else if (c == '%')
				{

                    lemon_addtext(ref str, nUsed, "%", 1, 0);
				}
				else
				{
					Console.Error.Write("illegal format\n");
					Environment.Exit(1);
				}
				j = i + 1;
			}
		}

        lemon_addtext(ref str, nUsed, zFormat[j], i - j, 0);
		return nUsed;
	}
	internal static int lemon_sprintf(ref string str, string format, params object[] LegacyParamArray)
{
    //	va_list ap;
    int rc;
    int ParamCount = -1;
    //	va_start(ap, format);
    rc = lemon_vsprintf(ref str, format, ap);
    //	va_end(ap);
    return rc;
}
//C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on the parameter 'dest', so pointers on this parameter are left unchanged:
//C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on the parameter 'src', so pointers on this parameter are left unchanged:
internal static void lemon_strcpy(sbyte* dest, sbyte* src)
{
    while ((*(dest++) = *(src++)) != 0)
    {
    }
}
//C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on the parameter 'dest', so pointers on this parameter are left unchanged:
internal static void lemon_strcat(sbyte* dest, string src)
{
    while (*dest != 0)
    {
        dest = dest.Substring(1);
    }
    lemon_strcpy(dest, src);
}


/* a few forward declarations... */
//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//struct rule;
//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//struct lemon;
//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//struct action;

/****************** From the file "action.c" *******************************/
/*
** Routines processing parser actions in the LEMON parser generator.
*/

/* Allocate a new parser action */
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static Action_new_action freelist = 0;

internal static action Action_new()
{
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static struct action *freelist = 0;
    Action_new_action newaction;

    if (freelist == 0)
    {
        int i;
        int amt = 100;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
        freelist = (Action_new_action)calloc(amt, sizeof(Action_new_action));
        if (freelist == 0)
        {
            Console.Error.Write("Unable to allocate memory for a new parser action.");
            Environment.Exit(1);
        }
        for (i = 0; i < amt - 1; i++)
        {
            freelist[i].next = freelist[i + 1];
        }
        freelist[amt - 1].next = 0;
    }
    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
    //ORIGINAL LINE: newaction = freelist;
    newaction = freelist;
    freelist = freelist.next;
    return newaction;
}

/* Sort parser actions */
internal static action Action_sort(action ap)
{
    ap = (action)msort(ref (string)ap, (string)ap.next, actioncmp);
    return ap;
}

/********** From the file "build.h" ************************************/
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//void FindRulePrecedences();
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//void FindFirstSets();
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//void FindStates();
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//void FindLinks();
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//void FindFollowSets();
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//void FindActions();

/* Initialized the configuration list builder */

/********* From the file "configlist.h" *********************************/
public static void Configlist_init()
{
    current = 0;
    currentend = current;
    basis = 0;
    basisend = basis;
    Configtable_init();
    return;
}

/* Add another configuration to the configuration list */
public static config Configlist_add(rule rp, int dot)
{ // Index into the RHS of the rule where the dot goes -  The rule
    config cfp;
    config model = new config();

    Debug.Assert(currentend != 0);
    model.rp = rp;
    model.dot = dot;
    cfp = Configtable_find(model);
    if (cfp == 0)
    {
        cfp = newconfig();
        cfp.rp = rp;
        cfp.dot = dot;
        cfp.fws = SetNew();
        cfp.stp = 0;
        cfp.fplp = cfp.bplp = 0;
        cfp.next = 0;
        cfp.bp = 0;
        currentend = cfp;
        currentend = cfp.next;
        Configtable_insert(cfp);
    }
    return cfp;
}

/* Add a basis configuration to the configuration list */
public static config Configlist_addbasis(rule rp, int dot)
{
    config cfp;
    config model = new config();

    Debug.Assert(basisend != 0);
    Debug.Assert(currentend != 0);
    model.rp = rp;
    model.dot = dot;
    cfp = Configtable_find(model);
    if (cfp == 0)
    {
        cfp = newconfig();
        cfp.rp = rp;
        cfp.dot = dot;
        cfp.fws = SetNew();
        cfp.stp = 0;
        cfp.fplp = cfp.bplp = 0;
        cfp.next = 0;
        cfp.bp = 0;
        currentend = cfp;
        currentend = cfp.next;
        basisend = cfp;
        basisend = cfp.bp;
        Configtable_insert(cfp);
    }
    return cfp;
}

/* Compute the closure of the configuration list */
public static void Configlist_closure(lemon lemp)
{
    config cfp;
    config newcfp;
    rule rp;
    rule newrp;
    symbol sp;
    symbol xsp;
    int i;
    int dot;

    Debug.Assert(currentend != 0);
    for (cfp = current; cfp != null; cfp = cfp.next)
    {
        rp = cfp.rp;
        dot = cfp.dot;
        if (dot >= rp.nrhs)
        {
            continue;
        }
        sp = rp.rhs[dot];
        if (sp.type == symbol_type.NONTERMINAL)
        {
            if (sp.rule == 0 && sp != lemp.errsym)
            {
                ErrorMsg(lemp.filename, rp.line, "Nonterminal \"%s\" has no rules.", sp.name);
                lemp.errorcnt++;
            }
            for (newrp = sp.rule; newrp != null; newrp = newrp.nextlhs)
            {
                newcfp = Configlist_add(newrp, 0);
                for (i = dot + 1; i < rp.nrhs; i++)
                {
                    xsp = rp.rhs[i];
                    if (xsp.type == symbol_type.TERMINAL)
                    {
                        SetAdd(ref newcfp.fws, xsp.index);
                        break;
                    }
                    else if (xsp.type == symbol_type.MULTITERMINAL)
                    {
                        int k;
                        for (k = 0; k < xsp.nsubsym; k++)
                        {
                            SetAdd(ref newcfp.fws, xsp.subsym[k].index);
                        }
                        break;
                    }
                    else
                    {
                        SetUnion(ref newcfp.fws, ref xsp.firstset);
                        if (xsp.lambda == Boolean.LEMON_FALSE)
                        {
                            break;
                        }
                    }
                }
                if (i == rp.nrhs)
                {
                    Plink_add(cfp.fplp, newcfp);
                }
            }
        }
    }
    return;
}

/* Sort the configuration list */
public static void Configlist_sort()
{
    current = (config)msort(ref (string)current, (string)(current.next), Configcmp);
    currentend = 0;
    return;
}

/* Sort the basis configuration list */
public static void Configlist_sortbasis()
{
    basis = (config)msort(ref (string)current, (string)(current.bp), Configcmp);
    basisend = 0;
    return;
}

/* Return a pointer to the head of the configuration list and
** reset the list */
public static config Configlist_return()
{
    config old;
    old = current;
    current = 0;
    currentend = 0;
    return old;
}

/* Return a pointer to the head of the configuration list and
** reset the list */
public static config Configlist_basis()
{
    config old;
    old = basis;
    basis = 0;
    basisend = 0;
    return old;
}

/* Free all elements of the given configuration list */
public static void Configlist_eat(config cfp)
{
    config nextcfp;
    for (; cfp != null; cfp = nextcfp)
    {
        nextcfp = cfp.next;
        Debug.Assert(cfp.fplp == 0);
        Debug.Assert(cfp.bplp == 0);
        if (cfp.fws != 0)
        {
            SetFree(ref cfp.fws);
        }
        deleteconfig(cfp);
    }
    return;
}

/* Initialized the configuration list builder */
public static void Configlist_reset()
{
    current = 0;
    currentend = current;
    basis = 0;
    basisend = basis;
    Configtable_clear(0);
    return;
}
/***************** From the file "error.c" *********************************/
/*
** Code for printing error message.
*/


/********* From the file "error.h" ***************************************/
public static void ErrorMsg(string filename, int lineno, string format, params object[] LegacyParamArray)
{
    //	va_list ap;
    Console.Error.Write("{0}:{1:D}: ", filename, lineno);
    int ParamCount = -1;
    //	va_start(ap, format);
    vfprintf(stderr, format, ap);
    //	va_end(ap);
    Console.Error.Write("\n");
}
public static int OptInit(string[] a, s_options o, FILE err)
{
    int errcnt = 0;
    argv = a;
    op = o;
    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
    //ORIGINAL LINE: errstream = err;
    errstream = err;
    if (argv && argv && op)
    {
        int i;
        for (i = 1; argv[i] != 0; i++)
        {
            if (argv[i][0] == '+' || argv[i][0] == '-')
            {
                errcnt += handleflags(i, err);
            }
            else if (StringFunctions.StrChr(argv[i], '='))
            {
                errcnt += handleswitch(i, err);
            }
        }
    }
    if (errcnt > 0)
    {
        fprintf(err, "Valid command line options for \"%s\" are:\n", a);
        OptPrint();
        Environment.Exit(1);
    }
    return 0;
}
public static int OptNArgs()
{
    int cnt = 0;
    int dashdash = 0;
    int i;
    if (argv != 0 && argv[0] != 0)
    {
        for (i = 1; argv[i] != 0; i++)
        {
            if (dashdash != 0 || !((argv[i])[0] == '-' || (argv[i])[0] == '+' || StringFunctions.StrChr((argv[i]), '=') != 0))
            {
                cnt++;
            }
            if (string.Compare(argv[i], "--") == 0)
            {
                dashdash = 1;
            }
        }
    }
    return cnt;
}
public static string OptArg(int n)
{
    int i;
    i = argindex(n);
    return i >= 0 ? argv[i] : 0;
}
public static void OptErr(int n)
{
    int i;
    i = argindex(n);
    if (i >= 0)
    {
        errline(i, 0, errstream);
    }
}
public static void OptPrint()
{
    int i;
    int max;
    int len;
    max = 0;
    for (i = 0; op[i].label != 0; i++)
    {
        len = ((int)Convert.ToString(op[i].label).Length) + 1;
        switch (op[i].type)
        {
            case option_type.OPT_FLAG:
            case option_type.OPT_FFLAG:
                break;
            case option_type.OPT_INT:
            case option_type.OPT_FINT:
                len += 9; // length of "<integer>"
                break;
            case option_type.OPT_DBL:
            case option_type.OPT_FDBL:
                len += 6; // length of "<real>"
                break;
            case option_type.OPT_STR:
            case option_type.OPT_FSTR:
                len += 8; // length of "<string>"
                break;
        }
        if (len > max)
        {
            max = len;
        }
    }
    for (i = 0; op[i].label != 0; i++)
    {
        switch (op[i].type)
        {
            case option_type.OPT_FLAG:
            case option_type.OPT_FFLAG:
                fprintf(errstream, "  -%-*s  %s\n", max, op[i].label, op[i].message);
                break;
            case option_type.OPT_INT:
            case option_type.OPT_FINT:
                fprintf(errstream, "  -%s<integer>%*s  %s\n", op[i].label, (int)(max - ((int)Convert.ToString(op[i].label).Length) - 9), "", op[i].message);
                break;
            case option_type.OPT_DBL:
            case option_type.OPT_FDBL:
                fprintf(errstream, "  -%s<real>%*s  %s\n", op[i].label, (int)(max - ((int)Convert.ToString(op[i].label).Length) - 6), "", op[i].message);
                break;
            case option_type.OPT_STR:
            case option_type.OPT_FSTR:
                fprintf(errstream, "  -%s<string>%*s  %s\n", op[i].label, (int)(max - ((int)Convert.ToString(op[i].label).Length) - 8), "", op[i].message);
                break;
        }
    }
}

/* In spite of its name, this function is really a scanner.  It read
** in the entire input file (all at once) then tokenizes it.  Each
** token is passed to the function "parseonetoken" which builds all
** the appropriate data structures in the global state vector "gp".
*/

/******** From the file "parse.h" *****************************************/
public static void Parse(lemon gp)
{
    pstate ps = new pstate();
    FILE fp;
    string filebuf;
    uint filesize;
    int lineno;
    int c;
    string cp;
    string nextcp;
    int startline = 0;

    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
    memset(ps, '\0', sizeof(pstate));
    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
    //ORIGINAL LINE: ps.gp = gp;
    ps.gp = gp;
    ps.filename = gp.filename;
    ps.errorcnt = 0;
    ps.state = e_state.INITIALIZE;

    /* Begin by reading the input file */
    fp = fopen(ps.filename, "rb");
    if (fp == 0)
    {
        ErrorMsg(ps.filename, 0, "Can't open this file for reading.");
        gp.errorcnt++;
        return;
    }
    fseek(fp, 0, 2);
    filesize = ftell(fp);
    rewind(fp);
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
    filebuf = (string)malloc(filesize + 1);
    if (filesize > 100000000 || filebuf == 0)
    {
        ErrorMsg(ps.filename, 0, "Input file too large.");
        gp.errorcnt++;
        fclose(fp);
        return;
    }
    if (fread(filebuf, 1, filesize, fp) != filesize)
    {
        ErrorMsg(ps.filename, 0, "Can't read in all %d bytes of this file.", filesize);
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
        free(filebuf);
        gp.errorcnt++;
        fclose(fp);
        return;
    }
    fclose(fp);
    filebuf = filebuf.Substring(0, filesize);

    /* Make an initial pass through the file to handle %ifdef and %ifndef */
    preprocess_input(ref filebuf);

    /* Now scan the text of the input file */
    lineno = 1;
    for (cp = filebuf; (c = cp) != 0;)
    {
        if (c == '\n')
        {
            lineno++; // Keep track of the line number
        }
        if (char.IsWhiteSpace((byte)(c)))
        {
            cp = cp.Substring(1);
            continue;
        } // Skip all white space
        if (c == '/' && cp[1] == '/')
        { // Skip C++ style comments
            cp += 2;
            while ((c = cp) != 0 && c != '\n')
            {
                cp = cp.Substring(1);
            }
            continue;
        }
        if (c == '/' && cp[1] == '*')
        { // Skip C style comments
            cp += 2;
            while ((c = cp) != 0 && (c != '/' || cp[-1] != '*'))
            {
                if (c == '\n')
                {
                    lineno++;
                }
                cp = cp.Substring(1);
            }
            if (c != 0)
            {
                cp = cp.Substring(1);
            }
            continue;
        }
        ps.tokenstart = cp; // Mark the beginning of the token
        ps.tokenlineno = lineno; // Linenumber on which token begins
        if (c == '\"')
        { // String literals
            cp = cp.Substring(1);
            while ((c = cp) != 0 && c != '\"')
            {
                if (c == '\n')
                {
                    lineno++;
                }
                cp = cp.Substring(1);
            }
            if (c == 0)
            {
                ErrorMsg(ps.filename, startline, "String starting on this line is not terminated before the end of the file.");
                ps.errorcnt++;
                nextcp = cp;
            }
            else
            {
                nextcp = cp.Substring(1);
            }
        }
        else if (c == '{')
        { // A block of C code
            int level;
            cp = cp.Substring(1);
            for (level = 1; (c = cp) != 0 && (level > 1 || c != '}'); cp++)
            {
                if (c == '\n')
                {
                    lineno++;
                }
                else if (c == '{')
                {
                    level++;
                }
                else if (c == '}')
                {
                    level--;
                }
                else if (c == '/' && cp[1] == '*')
                { // Skip comments
                    int prevc;
                    cp = cp[2];
                    prevc = 0;
                    while ((c = cp) != 0 && (c != '/' || prevc != '*'))
                    {
                        if (c == '\n')
                        {
                            lineno++;
                        }
                        prevc = c;
                        cp = cp.Substring(1);
                    }
                }
                else if (c == '/' && cp[1] == '/')
                { // Skip C++ style comments too
                    cp = cp[2];
                    while ((c = cp) != 0 && c != '\n')
                    {
                        cp = cp.Substring(1);
                    }
                    if (c != 0)
                    {
                        lineno++;
                    }
                }
                else if (c == '\'' || c == '\"')
                { // String a character literals
                    int startchar;
                    int prevc;
                    startchar = c;
                    prevc = 0;
                    for (cp++; (c = cp) != 0 && (c != startchar || prevc == '\\'); cp++)
                    {
                        if (c == '\n')
                        {
                            lineno++;
                        }
                        if (prevc == '\\')
                        {
                            prevc = 0;
                        }
                        else
                        {
                            prevc = c;
                        }
                    }
                }
            }
            if (c == 0)
            {
                ErrorMsg(ps.filename, ps.tokenlineno, "C code starting on this line is not terminated before the end of the file.");
                ps.errorcnt++;
                nextcp = cp;
            }
            else
            {
                nextcp = cp.Substring(1);
            }
        }
        else if (char.IsLetterOrDigit((byte)(c)))
        { // Identifiers
            while ((c = cp) != 0 && (char.IsLetterOrDigit((byte)(c)) || c == '_'))
            {
                cp = cp.Substring(1);
            }
            nextcp = cp;
        }
        else if (c == ':' && cp[1] == ':' && cp[2] == '=')
        { // The operator "::="
            cp += 3;
            nextcp = cp;
        }
        else if ((c == '/' || c == '|') && char.IsLetter((byte)(cp[1])))
        {
            cp += 2;
            while ((c = cp) != 0 && (char.IsLetterOrDigit((byte)(c)) || c == '_'))
            {
                cp = cp.Substring(1);
            }
            nextcp = cp;
        }
        else
        { // All other (one character) operators
            cp = cp.Substring(1);
            nextcp = cp;
        }
        c = cp;
        cp = 0; // Null terminate the token
        parseonetoken(ps); // Parse the token
        cp = (sbyte)c; // Restore the buffer
        cp = nextcp;
    }
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
    free(filebuf); // Release the buffer after parsing
    gp.rule = ps.firstrule;
    gp.errorcnt = ps.errorcnt;
}

/* Allocate a new plink */

/********* From the file "plink.h" ***************************************/
public static plink Plink_new()
{
    plink newlink;

    if (plink_freelist == 0)
    {
        int i;
        int amt = 100;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
        plink_freelist = (plink)calloc(amt, sizeof(plink));
        if (plink_freelist == 0)
        {
            Console.Error.Write("Unable to allocate memory for a new follow-set propagation link.\n");
            Environment.Exit(1);
        }
        for (i = 0; i < amt - 1; i++)
        {
            plink_freelist[i].next = plink_freelist[i + 1];
        }
        plink_freelist[amt - 1].next = 0;
    }
    newlink = plink_freelist;
    plink_freelist = plink_freelist.next;
    return newlink;
}

/* Add a plink to a plink list */
public static void Plink_add(plink[] plpp, config cfp)
{
    plink newlink;
    newlink = Plink_new();
    newlink.next = plpp;
    plpp = newlink;
    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
    //ORIGINAL LINE: newlink->cfp = cfp;
    newlink.cfp = cfp;
}

/* Transfer every plink on the list "from" to the list "to" */
public static void Plink_copy(plink[] to, plink from)
{
    plink nextpl;
    while (from != null)
    {
        nextpl = from.next;
        from.next = to;
        to = from;
        //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
        //ORIGINAL LINE: from = nextpl;
        from = nextpl;
    }
}

/* Delete every plink on the list */
public static void Plink_delete(plink plp)
{
    plink nextpl;

    while (plp != null)
    {
        nextpl = plp.next;
        plp.next = plink_freelist;
        //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
        //ORIGINAL LINE: plink_freelist = plp;
        plink_freelist = plp;
        //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
        //ORIGINAL LINE: plp = nextpl;
        plp = nextpl;
    }
}

/* Duplicate the input file without comments and without actions
** on rules */

/********** From the file "report.h" *************************************/
public static void Reprint(lemon lemp)
{
    rule rp;
    symbol sp;
    int i;
    int j;
    int maxlen;
    int len;
    int ncolumns;
    int skip;
    Console.Write("// Reprint of input file \"{0}\".\n// Symbols:\n", lemp.filename);
    maxlen = 10;
    for (i = 0; i < lemp.nsymbol; i++)
    {
        sp = lemp.symbols[i];
        len = ((int)sp.name.Length);
        if (len > maxlen)
        {
            maxlen = len;
        }
    }
    ncolumns = 76 / (maxlen + 5);
    if (ncolumns < 1)
    {
        ncolumns = 1;
    }
    skip = (lemp.nsymbol + ncolumns - 1) / ncolumns;
    for (i = 0; i < skip; i++)
    {
        Console.Write("//");
        for (j = i; j < lemp.nsymbol; j += skip)
        {
            sp = lemp.symbols[j];
            Debug.Assert(sp.index == j);
            //C++ TO C# CONVERTER TODO TASK: The following line has a C format specifier which cannot be directly translated to C#:
            //ORIGINAL LINE: printf(" %3d %-*.*s", j, maxlen, maxlen, sp->name);
            Console.Write(" {0,3:D} %-*.*s", j, maxlen, maxlen, sp.name);
        }
        Console.Write("\n");
    }
    for (rp = lemp.rule; rp != null; rp = rp.next)
    {
        Console.Write("{0}", rp.lhs.name);
        /*    if( rp->lhsalias ) printf("(%s)",rp->lhsalias); */
        Console.Write(" ::=");
        for (i = 0; i < rp.nrhs; i++)
        {
            sp = rp.rhs[i];
            if (sp.type == symbol_type.MULTITERMINAL)
            {
                Console.Write(" {0}", sp.subsym[0].name);
                for (j = 1; j < sp.nsubsym; j++)
                {
                    Console.Write("|{0}", sp.subsym[j].name);
                }
            }
            else
            {
                Console.Write(" {0}", sp.name);
            }
            /* if( rp->rhsalias[i] ) printf("(%s)",rp->rhsalias[i]); */
        }
        Console.Write(".");
        if (rp.precsym != null)
        {
            Console.Write(" [{0}]", rp.precsym.name);
        }
        /* if( rp->code ) printf("\n    %s",rp->code); */
        Console.Write("\n");
    }
}

/* Generate the "*.out" log file */
public static void ReportOutput(lemon lemp)
{
    int i;
    state stp;
    config cfp;
    action ap;
    FILE fp;

    fp = file_open(lemp, ".out", "wb");
    if (fp == 0)
    {
        return;
    }
    for (i = 0; i < lemp.nxstate; i++)
    {
        stp = lemp.sorted[i];
        fprintf(fp, "State %d:\n", stp.statenum);
        if (lemp.basisflag != 0)
        {
            cfp = stp.bp;
        }
        else
        {
            cfp = stp.cfp;
        }
        while (cfp != null)
        {
            string buf = new string(new char[20]);
            if (cfp.dot == cfp.rp.nrhs)
            {
                lemon_sprintf(ref buf, "(%d)", cfp.rp.iRule);
                fprintf(fp, "    %5s ", buf);
            }
            else
            {
                fprintf(fp, "          ");
            }
            ConfigPrint(fp, cfp);
            fprintf(fp, "\n");
#if false
	//			SetPrint(fp, cfp->fws, lemp);
	//			PlinkPrint(fp, cfp->fplp, "To  ");
	//			PlinkPrint(fp, cfp->bplp, "From");
#endif
            if (lemp.basisflag != 0)
            {
                cfp = cfp.bp;
            }
            else
            {
                cfp = cfp.next;
            }
        }
        fprintf(fp, "\n");
        for (ap = stp.ap; ap != null; ap = ap.next)
        {
            if (PrintAction(ap, fp, 30) != 0)
            {
                fprintf(fp, "\n");
            }
        }
        fprintf(fp, "\n");
    }
    fprintf(fp, "----------------------------------------------------\n");
    fprintf(fp, "Symbols:\n");
    for (i = 0; i < lemp.nsymbol; i++)
    {
        int j;
        symbol sp;

        sp = lemp.symbols[i];
        fprintf(fp, "  %3d: %s", i, sp.name);
        if (sp.type == symbol_type.NONTERMINAL)
        {
            fprintf(fp, ":");
            if (sp.lambda)
            {
                fprintf(fp, " <lambda>");
            }
            for (j = 0; j < lemp.nterminal; j++)
            {
                if (sp.firstset != 0 && (sp.firstset[j]))
                {
                    fprintf(fp, " %s", lemp.symbols[j].name);
                }
            }
        }
        fprintf(fp, "\n");
    }
    fclose(fp);
    return;
}

/* Generate C source code for the parser */
public static void ReportTable(lemon lemp, int mhflag)
{ // Output in makeheaders format if true
    FILE @out;
    FILE in;
    string line = new string(new char[DefineConstants.LINESIZE]);
    int lineno;
    state stp;
    action ap;
    rule rp;
    acttab pActtab;
    int i;
    int j;
    int n;
    int sz;
    int szActionType; // sizeof(YYACTIONTYPE)
    int szCodeType; // sizeof(YYCODETYPE)
    string name;
    int mnTknOfst;
    int mxTknOfst;
    int mnNtOfst;
    int mxNtOfst;
    axset ax;

		in = tplt_open(lemp);
    if (in == 0)
		{
        return;
    }
    @out = file_open(lemp, ".c", "wb");
    if (@out == 0)
    {
        fclose(in);
        return;
    }
    lineno = 1;
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Generate the include code, if any */
    tplt_print(@out, lemp, lemp.include, ref lineno);
    if (mhflag != 0)
    {
        //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
        //ORIGINAL LINE: sbyte *incName = file_makename(lemp, ".h");
        sbyte incName = file_makename(lemp, ".h");
        fprintf(@out, "#include \"%s\"\n", incName);
        lineno++;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
        free(incName);
    }
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Generate #defines for all tokens */
    if (mhflag != 0)
    {
        string prefix;
        fprintf(@out, "#if INTERFACE\n");
        lineno++;
        if (lemp.tokenprefix != 0)
        {
            prefix = lemp.tokenprefix;
        }
        else
        {
            prefix = "";
        }
        for (i = 1; i < lemp.nterminal; i++)
        {
            fprintf(@out, "#define %s%-30s %2d\n", prefix, lemp.symbols[i].name, i);
            lineno++;
        }
        fprintf(@out, "#endif\n");
        lineno++;
    }
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Generate the defines */
    fprintf(@out, "#define YYCODETYPE %s\n", minimum_size_type(0, lemp.nsymbol + 1, ref szCodeType));
    lineno++;
    fprintf(@out, "#define YYNOCODE %d\n", lemp.nsymbol + 1);
    lineno++;
    fprintf(@out, "#define YYACTIONTYPE %s\n", minimum_size_type(0, lemp.nstate + lemp.nrule * 2 + 5, ref szActionType));
    lineno++;
    if (lemp.wildcard != null)
    {
        fprintf(@out, "#define YYWILDCARD %d\n", lemp.wildcard.index);
        lineno++;
    }
    print_stack_union(@out, lemp, ref lineno, mhflag);
    fprintf(@out, "#ifndef YYSTACKDEPTH\n");
    lineno++;
    if (lemp.stacksize != 0)
    {
        fprintf(@out, "#define YYSTACKDEPTH %s\n", lemp.stacksize);
        lineno++;
    }
    else
    {
        fprintf(@out, "#define YYSTACKDEPTH 100\n");
        lineno++;
    }
    fprintf(@out, "#endif\n");
    lineno++;
    if (mhflag != 0)
    {
        fprintf(@out, "#if INTERFACE\n");
        lineno++;
    }
    name = lemp.name != 0 ? lemp.name : "Parse";
    if (lemp.arg != 0 && lemp.arg[0])
    {
        i = ((int)lemp.arg.Length);
        while (i >= 1 && char.IsWhiteSpace((byte)(lemp.arg[i - 1])))
        {
            i--;
        }
        while (i >= 1 && (char.IsLetterOrDigit((byte)(lemp.arg[i - 1])) || lemp.arg[i - 1] == '_'))
        {
            i--;
        }
        fprintf(@out, "#define %sARG_SDECL %s;\n", name, lemp.arg);
        lineno++;
        fprintf(@out, "#define %sARG_PDECL ,%s\n", name, lemp.arg);
        lineno++;
        fprintf(@out, "#define %sARG_FETCH %s = yypParser->%s\n", name, lemp.arg, lemp.arg[i]);
        lineno++;
        fprintf(@out, "#define %sARG_STORE yypParser->%s = %s\n", name, lemp.arg[i], lemp.arg[i]);
        lineno++;
    }
    else
    {
        fprintf(@out, "#define %sARG_SDECL\n", name);
        lineno++;
        fprintf(@out, "#define %sARG_PDECL\n", name);
        lineno++;
        fprintf(@out, "#define %sARG_FETCH\n", name);
        lineno++;
        fprintf(@out, "#define %sARG_STORE\n", name);
        lineno++;
    }
    if (mhflag != 0)
    {
        fprintf(@out, "#endif\n");
        lineno++;
    }
    if (lemp.errsym.useCnt != 0)
    {
        fprintf(@out, "#define YYERRORSYMBOL %d\n", lemp.errsym.index);
        lineno++;
        fprintf(@out, "#define YYERRSYMDT yy%d\n", lemp.errsym.dtnum);
        lineno++;
    }
    if (lemp.has_fallback != 0)
    {
        fprintf(@out, "#define YYFALLBACK 1\n");
        lineno++;
    }

    /* Compute the action table, but do not output it yet.  The action
    ** table must be computed before generating the YYNSTATE macro because
    ** we need to know how many states can be eliminated.
    */
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
    ax = (axset)calloc(lemp.nxstate * 2, sizeof(ax[0]));
    if (ax == 0)
    {
        Console.Error.Write("malloc failed\n");
        Environment.Exit(1);
    }
    for (i = 0; i < lemp.nxstate; i++)
    {
        stp = lemp.sorted[i];
        ax[i * 2].stp = stp;
        ax[i * 2].isTkn = 1;
        ax[i * 2].nAction = stp.nTknAct;
        ax[i * 2 + 1].stp = stp;
        ax[i * 2 + 1].isTkn = 0;
        ax[i * 2 + 1].nAction = stp.nNtAct;
    }
    mxTknOfst = mnTknOfst = 0;
    mxNtOfst = mnNtOfst = 0;
    /* In an effort to minimize the action table size, use the heuristic
    ** of placing the largest action sets first */
    for (i = 0; i < lemp.nxstate * 2; i++)
    {
        ax[i].iOrder = i;
    }
    qsort(ax, lemp.nxstate * 2, sizeof(ax[0]), axset_compare);
    pActtab = acttab_alloc();
    for (i = 0; i < lemp.nxstate * 2 && ax[i].nAction > 0; i++)
    {
        stp = ax[i].stp;
        if (ax[i].isTkn)
        {
            for (ap = stp.ap; ap != null; ap = ap.next)
            {
                int action;
                if (ap.sp.index >= lemp.nterminal)
                {
                    continue;
                }
                action = compute_action(lemp, ap);
                if (action < 0)
                {
                    continue;
                }
                acttab_action(pActtab, ap.sp.index, action);
            }
            stp.iTknOfst = acttab_insert(pActtab);
            if (stp.iTknOfst < mnTknOfst)
            {
                mnTknOfst = stp.iTknOfst;
            }
            if (stp.iTknOfst > mxTknOfst)
            {
                mxTknOfst = stp.iTknOfst;
            }
        }
        else
        {
            for (ap = stp.ap; ap != null; ap = ap.next)
            {
                int action;
                if (ap.sp.index < lemp.nterminal)
                {
                    continue;
                }
                if (ap.sp.index == lemp.nsymbol)
                {
                    continue;
                }
                action = compute_action(lemp, ap);
                if (action < 0)
                {
                    continue;
                }
                acttab_action(pActtab, ap.sp.index, action);
            }
            stp.iNtOfst = acttab_insert(pActtab);
            if (stp.iNtOfst < mnNtOfst)
            {
                mnNtOfst = stp.iNtOfst;
            }
            if (stp.iNtOfst > mxNtOfst)
            {
                mxNtOfst = stp.iNtOfst;
            }
        }
#if false
	//		{ int jj, nn;
	//		for (jj = nn = 0; jj<pActtab->nAction; jj++) {
	//			if (pActtab->aAction[jj].action<0) nn++;
	//		}
	//		printf("%4d: State %3d %s n: %2d size: %5d freespace: %d\n",
	//			i, stp->statenum, ax[i].isTkn ? "Token" : "Var  ",
	//			ax[i].nAction, pActtab->nAction, nn);
	//		}
#endif
    }
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
    free(ax);

    /* Mark rules that are actually used for reduce actions after all
    ** optimizations have been applied
    */
    for (rp = lemp.rule; rp != null; rp = rp.next)
    {
        rp.doesReduce = Boolean.LEMON_FALSE;
    }
    for (i = 0; i < lemp.nxstate; i++)
    {
        action ap;
        for (ap = lemp.sorted[i].ap; ap != null; ap = ap.next)
        {
            if (ap.type == e_action.REDUCE || ap.type == e_action.SHIFTREDUCE)
            {
                ap.x.rp.doesReduce = i;
            }
        }
    }

    /* Finish rendering the constants now that the action table has
    ** been computed */
    fprintf(@out, "#define YYNSTATE             %d\n", lemp.nxstate);
    lineno++;
    fprintf(@out, "#define YYNRULE              %d\n", lemp.nrule);
    lineno++;
    fprintf(@out, "#define YY_MAX_SHIFT         %d\n", lemp.nxstate - 1);
    lineno++;
    fprintf(@out, "#define YY_MIN_SHIFTREDUCE   %d\n", lemp.nstate);
    lineno++;
    i = lemp.nstate + lemp.nrule;
    fprintf(@out, "#define YY_MAX_SHIFTREDUCE   %d\n", i - 1);
    lineno++;
    fprintf(@out, "#define YY_MIN_REDUCE        %d\n", i);
    lineno++;
    i = lemp.nstate + lemp.nrule * 2;
    fprintf(@out, "#define YY_MAX_REDUCE        %d\n", i - 1);
    lineno++;
    fprintf(@out, "#define YY_ERROR_ACTION      %d\n", i);
    lineno++;
    fprintf(@out, "#define YY_ACCEPT_ACTION     %d\n", i + 1);
    lineno++;
    fprintf(@out, "#define YY_NO_ACTION         %d\n", i + 2);
    lineno++;
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Now output the action table and its associates:
    **
    **  yy_action[]        A single table containing all actions.
    **  yy_lookahead[]     A table containing the lookahead for each entry in
    **                     yy_action.  Used to detect hash collisions.
    **  yy_shift_ofst[]    For each state, the offset into yy_action for
    **                     shifting terminals.
    **  yy_reduce_ofst[]   For each state, the offset into yy_action for
    **                     shifting non-terminals after a reduce.
    **  yy_default[]       Default action for each state.
    */

    /* Output the yy_action table */
    lemp.nactiontab = n = ((pActtab).nAction);
    lemp.tablesize += n * szActionType;
    fprintf(@out, "#define YY_ACTTAB_COUNT (%d)\n", n);
    lineno++;
    fprintf(@out, "static const YYACTIONTYPE yy_action[] = {\n");
    lineno++;
    for (i = j = 0; i < n; i++)
    {
        int action = ((pActtab).aAction[i].action);
        if (action < 0)
        {
            action = lemp.nstate + lemp.nrule + 2;
        }
        if (j == 0)
        {
            fprintf(@out, " /* %5d */ ", i);
        }
        fprintf(@out, " %4d,", action);
        if (j == 9 || i == n - 1)
        {
            fprintf(@out, "\n");
            lineno++;
            j = 0;
        }
        else
        {
            j++;
        }
    }
    fprintf(@out, "};\n");
    lineno++;

    /* Output the yy_lookahead table */
    lemp.tablesize += n * szCodeType;
    fprintf(@out, "static const YYCODETYPE yy_lookahead[] = {\n");
    lineno++;
    for (i = j = 0; i < n; i++)
    {
        int la = ((pActtab).aAction[i].lookahead);
        if (la < 0)
        {
            la = lemp.nsymbol;
        }
        if (j == 0)
        {
            fprintf(@out, " /* %5d */ ", i);
        }
        fprintf(@out, " %4d,", la);
        if (j == 9 || i == n - 1)
        {
            fprintf(@out, "\n");
            lineno++;
            j = 0;
        }
        else
        {
            j++;
        }
    }
    fprintf(@out, "};\n");
    lineno++;

    /* Output the yy_shift_ofst[] table */
    n = lemp.nxstate;
    while (n > 0 && lemp.sorted[n - 1].iTknOfst == DefineConstants.NO_OFFSET)
    {
        n--;
    }
    fprintf(@out, "#define YY_SHIFT_USE_DFLT (%d)\n", lemp.nactiontab);
    lineno++;
    fprintf(@out, "#define YY_SHIFT_COUNT    (%d)\n", n - 1);
    lineno++;
    fprintf(@out, "#define YY_SHIFT_MIN      (%d)\n", mnTknOfst);
    lineno++;
    fprintf(@out, "#define YY_SHIFT_MAX      (%d)\n", mxTknOfst);
    lineno++;
    fprintf(@out, "static const %s yy_shift_ofst[] = {\n", minimum_size_type(mnTknOfst, lemp.nterminal + lemp.nactiontab, ref sz));
    lineno++;
    lemp.tablesize += n * sz;
    for (i = j = 0; i < n; i++)
    {
        int ofst;
        stp = lemp.sorted[i];
        ofst = stp.iTknOfst;
        if (ofst == DefineConstants.NO_OFFSET)
        {
            ofst = lemp.nactiontab;
        }
        if (j == 0)
        {
            fprintf(@out, " /* %5d */ ", i);
        }
        fprintf(@out, " %4d,", ofst);
        if (j == 9 || i == n - 1)
        {
            fprintf(@out, "\n");
            lineno++;
            j = 0;
        }
        else
        {
            j++;
        }
    }
    fprintf(@out, "};\n");
    lineno++;

    /* Output the yy_reduce_ofst[] table */
    fprintf(@out, "#define YY_REDUCE_USE_DFLT (%d)\n", mnNtOfst - 1);
    lineno++;
    n = lemp.nxstate;
    while (n > 0 && lemp.sorted[n - 1].iNtOfst == DefineConstants.NO_OFFSET)
    {
        n--;
    }
    fprintf(@out, "#define YY_REDUCE_COUNT (%d)\n", n - 1);
    lineno++;
    fprintf(@out, "#define YY_REDUCE_MIN   (%d)\n", mnNtOfst);
    lineno++;
    fprintf(@out, "#define YY_REDUCE_MAX   (%d)\n", mxNtOfst);
    lineno++;
    fprintf(@out, "static const %s yy_reduce_ofst[] = {\n", minimum_size_type(mnNtOfst - 1, mxNtOfst, ref sz));
    lineno++;
    lemp.tablesize += n * sz;
    for (i = j = 0; i < n; i++)
    {
        int ofst;
        stp = lemp.sorted[i];
        ofst = stp.iNtOfst;
        if (ofst == DefineConstants.NO_OFFSET)
        {
            ofst = mnNtOfst - 1;
        }
        if (j == 0)
        {
            fprintf(@out, " /* %5d */ ", i);
        }
        fprintf(@out, " %4d,", ofst);
        if (j == 9 || i == n - 1)
        {
            fprintf(@out, "\n");
            lineno++;
            j = 0;
        }
        else
        {
            j++;
        }
    }
    fprintf(@out, "};\n");
    lineno++;

    /* Output the default action table */
    fprintf(@out, "static const YYACTIONTYPE yy_default[] = {\n");
    lineno++;
    n = lemp.nxstate;
    lemp.tablesize += n * szActionType;
    for (i = j = 0; i < n; i++)
    {
        stp = lemp.sorted[i];
        if (j == 0)
        {
            fprintf(@out, " /* %5d */ ", i);
        }
        fprintf(@out, " %4d,", stp.iDfltReduce + lemp.nstate + lemp.nrule);
        if (j == 9 || i == n - 1)
        {
            fprintf(@out, "\n");
            lineno++;
            j = 0;
        }
        else
        {
            j++;
        }
    }
    fprintf(@out, "};\n");
    lineno++;
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Generate the table of fallback tokens.
    */
    if (lemp.has_fallback != 0)
    {
        int mx = lemp.nterminal - 1;
        while (mx > 0 && lemp.symbols[mx].fallback == 0)
        {
            mx--;
        }
        lemp.tablesize += (mx + 1) * szCodeType;
        for (i = 0; i <= mx; i++)
        {
            symbol[] p = lemp.symbols[i];
            if (p.fallback == 0)
            {
                fprintf(@out, "    0,  /* %10s => nothing */\n", p.name);
            }
            else
            {
                fprintf(@out, "  %3d,  /* %10s => %s */\n", p.fallback.index, p.name, p.fallback.name);
            }
            lineno++;
        }
    }
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Generate a table containing the symbolic name of every symbol
    */
    for (i = 0; i < lemp.nsymbol; i++)
    {
        lemon_sprintf(ref line, "\"%s\",", lemp.symbols[i].name);
        fprintf(@out, "  %-15s", line);
        if ((i & 3) == 3)
        {
            fprintf(@out, "\n");
            lineno++;
        }
    }
    if ((i & 3) != 0)
    {
        fprintf(@out, "\n");
        lineno++;
    }
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Generate a table containing a text string that describes every
    ** rule in the rule set of the grammar.  This information is used
    ** when tracing REDUCE actions.
    */
    for (i = 0, rp = lemp.rule; rp != null; rp = rp.next, i++)
    {
        Debug.Assert(rp.iRule == i);
        fprintf(@out, " /* %3d */ \"", i);
        writeRuleText(@out, rp);
        fprintf(@out, "\",\n");
        lineno++;
    }
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Generate code which executes every time a symbol is popped from
    ** the stack while processing errors or while destroying the parser.
    ** (In other words, generate the %destructor actions)
    */
    if (lemp.tokendest != 0)
    {
        int once = 1;
        for (i = 0; i < lemp.nsymbol; i++)
        {
            symbol[] sp = lemp.symbols[i];
            if (sp == 0 || sp.type != symbol_type.TERMINAL)
            {
                continue;
            }
            if (once != 0)
            {
                fprintf(@out, "      /* TERMINAL Destructor */\n");
                lineno++;
                once = 0;
            }
            fprintf(@out, "    case %d: /* %s */\n", sp.index, sp.name);
            lineno++;
        }
        for (i = 0; i < lemp.nsymbol && lemp.symbols[i].type != symbol_type.TERMINAL; i++)
        {
            ;
        }
        if (i < lemp.nsymbol)
        {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
            //ORIGINAL LINE: emit_destructor_code(out, lemp->symbols[i], lemp, &lineno);
            emit_destructor_code(@out, new symbol(lemp.symbols[i]), lemp, ref lineno);
            fprintf(@out, "      break;\n");
            lineno++;
        }
    }
    if (lemp.vardest != 0)
    {
        symbol dflt_sp = 0;
        int once = 1;
        for (i = 0; i < lemp.nsymbol; i++)
        {
            symbol[] sp = lemp.symbols[i];
            if (sp == 0 || sp.type == symbol_type.TERMINAL || sp.index <= 0 || sp.destructor != 0)
            {
                continue;
            }
            if (once != 0)
            {
                fprintf(@out, "      /* Default NON-TERMINAL Destructor */\n");
                lineno++;
                once = 0;
            }
            fprintf(@out, "    case %d: /* %s */\n", sp.index, sp.name);
            lineno++;
            //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
            //ORIGINAL LINE: dflt_sp = sp;
            dflt_sp = sp;
        }
        if (dflt_sp != 0)
        {
            emit_destructor_code(@out, dflt_sp, lemp, ref lineno);
        }
        fprintf(@out, "      break;\n");
        lineno++;
    }
    for (i = 0; i < lemp.nsymbol; i++)
    {
        symbol[] sp = lemp.symbols[i];
        if (sp == 0 || sp.type == symbol_type.TERMINAL || sp.destructor == 0)
        {
            continue;
        }
        if (sp.destLineno < 0)
        {
            continue; // Already emitted
        }
        fprintf(@out, "    case %d: /* %s */\n", sp.index, sp.name);
        lineno++;

        /* Combine duplicate destructors into a single case */
        for (j = i + 1; j < lemp.nsymbol; j++)
        {
            symbol[] sp2 = lemp.symbols[j];
            if (sp2 && sp2.type != symbol_type.TERMINAL && sp2.destructor && sp2.dtnum == sp.dtnum && string.Compare(sp.destructor, sp2.destructor) == 0)
            {
                fprintf(@out, "    case %d: /* %s */\n", sp2.index, sp2.name);
                lineno++;
                sp2.destLineno = -1; // Avoid emitting this destructor again
            }
        }

        //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
        //ORIGINAL LINE: emit_destructor_code(out, lemp->symbols[i], lemp, &lineno);
        emit_destructor_code(@out, new symbol(lemp.symbols[i]), lemp, ref lineno);
        fprintf(@out, "      break;\n");
        lineno++;
    }
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Generate code which executes whenever the parser stack overflows */
    tplt_print(@out, lemp, lemp.overflow, ref lineno);
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Generate the table of rule information
    **
    ** Note: This code depends on the fact that rules are number
    ** sequentually beginning with 0.
    */
    for (rp = lemp.rule; rp != null; rp = rp.next)
    {
        fprintf(@out, "  { %d, %d },\n", rp.lhs.index, rp.nrhs);
        lineno++;
    }
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Generate code which execution during each REDUCE action */
    i = 0;
    for (rp = lemp.rule; rp != null; rp = rp.next)
    {
        i += translate_code(lemp, rp);
    }
    if (i != 0)
    {
        fprintf(@out, "        YYMINORTYPE yylhsminor;\n");
        lineno++;
    }
    /* First output rules other than the default: rule */
    for (rp = lemp.rule; rp != null; rp = rp.next)
    {
        rule rp2; // Other rules with the same action
        if (rp.codeEmitted != 0)
        {
            continue;
        }
        if (rp.noCode != 0)
        {
            /* No C code actions, so this will be part of the "default:" rule */
            continue;
        }
        fprintf(@out, "      case %d: /* ", rp.iRule);
        writeRuleText(@out, rp);
        fprintf(@out, " */\n");
        lineno++;
        for (rp2 = rp.next; rp2 != null; rp2 = rp2.next)
        {
            if (rp2.code == rp.code && rp2.codePrefix == rp.codePrefix && rp2.codeSuffix == rp.codeSuffix)
            {
                fprintf(@out, "      case %d: /* ", rp2.iRule);
                writeRuleText(@out, rp2);
                fprintf(@out, " */ yytestcase(yyruleno==%d);\n", rp2.iRule);
                lineno++;
                rp2.codeEmitted = 1;
            }
        }
        emit_code(@out, rp, lemp, ref lineno);
        fprintf(@out, "        break;\n");
        lineno++;
        rp.codeEmitted = 1;
    }
    /* Finally, output the default: rule.  We choose as the default: all
    ** empty actions. */
    fprintf(@out, "      default:\n");
    lineno++;
    for (rp = lemp.rule; rp != null; rp = rp.next)
    {
        if (rp.codeEmitted != 0)
        {
            continue;
        }
        Debug.Assert(rp.noCode);
        fprintf(@out, "      /* (%d) ", rp.iRule);
        writeRuleText(@out, rp);
        if (rp.doesReduce)
        {
            fprintf(@out, " */ yytestcase(yyruleno==%d);\n", rp.iRule);
            lineno++;
        }
        else
        {
            fprintf(@out, " (OPTIMIZED OUT) */ assert(yyruleno!=%d);\n", rp.iRule);
            lineno++;
        }
    }
    fprintf(@out, "        break;\n");
    lineno++;
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Generate code which executes if a parse fails */
    tplt_print(@out, lemp, lemp.failure, ref lineno);
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Generate code which executes when a syntax error occurs */
    tplt_print(@out, lemp, lemp.error, ref lineno);
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Generate code which executes when the parser accepts its input */
    tplt_print(@out, lemp, lemp.accept, ref lineno);
    tplt_xfer(ref lemp.name, in, @out, ref lineno);

    /* Append any addition code the user desires */
    tplt_print(@out, lemp, lemp.extracode, ref lineno);

    fclose(in);
    fclose(@out);
    return;
}

/* Generate a header file for the parser */
public static void ReportHeader(lemon lemp)
{
    FILE @out;
    FILE in;
    string prefix;
    string line = new string(new char[DefineConstants.LINESIZE]);
    string pattern = new string(new char[DefineConstants.LINESIZE]);
    int i;

    if (lemp.tokenprefix != 0)
    {
        prefix = lemp.tokenprefix;
    }
    else
    {
        prefix = "";
    }
		in = file_open(lemp, ".h", "rb");
    if (in != null)
		{
        int nextChar;
        for (i = 1; i < lemp.nterminal && fgets(line, DefineConstants.LINESIZE, in); i++)
        {
            lemon_sprintf(pattern, "#define %s%-30s %3d\n", prefix, lemp.symbols[i].name, i);
            if (string.Compare(line, pattern))
            {
                break;
            }
        }
        nextChar = fgetc(in);
        fclose(in);
        if (i == lemp.nterminal && nextChar == EOF)
        {
            /* No change in the file.  Don't rewrite it. */
            return;
        }
    }
    @out = file_open(lemp, ".h", "wb");
    if (@out != null)
    {
        for (i = 1; i < lemp.nterminal; i++)
        {
            fprintf(@out, "#define %s%-30s %3d\n", prefix, lemp.symbols[i].name, i);
        }
        fclose(@out);
    }
    return;
}

/* Reduce the size of the action tables, if possible, by making use
** of defaults.
**
** In this version, we take the most frequent REDUCE action and make
** it the default.  Except, there is no default if the wildcard token
** is a possible look-ahead.
*/
public static void CompressTables(lemon lemp)
{
    state stp;
    action ap;
    action ap2;
    action nextap;
    rule rp;
    rule rp2;
    rule rbest;
    int nbest;
    int n;
    int i;
    int usesWildcard;

    for (i = 0; i < lemp.nstate; i++)
    {
        stp = lemp.sorted[i];
        nbest = 0;
        rbest = 0;
        usesWildcard = 0;

        for (ap = stp.ap; ap != null; ap = ap.next)
        {
            if (ap.type == e_action.SHIFT && ap.sp == lemp.wildcard)
            {
                usesWildcard = 1;
            }
            if (ap.type != e_action.REDUCE)
            {
                continue;
            }
            rp = ap.x.rp;
            if (rp.lhsStart != 0)
            {
                continue;
            }
            if (rp == rbest)
            {
                continue;
            }
            n = 1;
            for (ap2 = ap.next; ap2 != null; ap2 = ap2.next)
            {
                if (ap2.type != e_action.REDUCE)
                {
                    continue;
                }
                rp2 = ap2.x.rp;
                if (rp2 == rbest)
                {
                    continue;
                }
                if (rp2 == rp)
                {
                    n++;
                }
            }
            if (n > nbest)
            {
                nbest = n;
                //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
                //ORIGINAL LINE: rbest = rp;
                rbest = rp;
            }
        }

        /* Do not make a default if the number of rules to default
        ** is not at least 1 or if the wildcard token is a possible
        ** lookahead.
        */
        if (nbest < 1 || usesWildcard != 0)
        {
            continue;
        }


        /* Combine matching REDUCE actions into a single default */
        for (ap = stp.ap; ap != null; ap = ap.next)
        {
            if (ap.type == e_action.REDUCE && ap.x.rp == rbest)
            {
                break;
            }
        }
        Debug.Assert(ap);
        ap.sp = Symbol_new("{default}");
        for (ap = ap.next; ap != null; ap = ap.next)
        {
            if (ap.type == e_action.REDUCE && ap.x.rp == rbest)
            {
                ap.type = e_action.NOT_USED;
            }
        }
        stp.ap = Action_sort(stp.ap);

        for (ap = stp.ap; ap != null; ap = ap.next)
        {
            if (ap.type == e_action.SHIFT)
            {
                break;
            }
            if (ap.type == e_action.REDUCE && ap.x.rp != rbest)
            {
                break;
            }
        }
        if (ap == 0)
        {
            stp.autoReduce = 1;
            //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
            //ORIGINAL LINE: stp->pDfltReduce = rbest;
            stp.pDfltReduce = rbest;
        }
    }

    /* Make a second pass over all states and actions.  Convert
    ** every action that is a SHIFT to an autoReduce state into
    ** a SHIFTREDUCE action.
    */
    for (i = 0; i < lemp.nstate; i++)
    {
        stp = lemp.sorted[i];
        for (ap = stp.ap; ap != null; ap = ap.next)
        {
            state pNextState;
            if (ap.type != e_action.SHIFT)
            {
                continue;
            }
            pNextState = ap.x.stp;
            if (pNextState.autoReduce != 0 && pNextState.pDfltReduce != 0)
            {
                ap.type = e_action.SHIFTREDUCE;
                ap.x.rp = pNextState.pDfltReduce;
            }
        }
    }

    /* If a SHIFTREDUCE action specifies a rule that has a single RHS term
    ** (meaning that the SHIFTREDUCE will land back in the state where it
    ** started) and if there is no C-code associated with the reduce action,
    ** then we can go ahead and convert the action to be the same as the
    ** action for the RHS of the rule.
    */
    for (i = 0; i < lemp.nstate; i++)
    {
        stp = lemp.sorted[i];
        for (ap = stp.ap; ap != null; ap = nextap)
        {
            nextap = ap.next;
            if (ap.type != e_action.SHIFTREDUCE)
            {
                continue;
            }
            rp = ap.x.rp;
            if (rp.noCode == 0)
            {
                continue;
            }
            if (rp.nrhs != 1)
            {
                continue;
            }
#if 1
				/* Only apply this optimization to non-terminals.  It would be OK to
				** apply it to terminal symbols too, but that makes the parser tables
				** larger. */
				if (ap.sp.index < lemp.nterminal)
				{
					continue;
				}
#endif
            /* If we reach this point, it means the optimization can be applied */
            //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
            //ORIGINAL LINE: nextap = ap;
            nextap = ap;
            for (ap2 = stp.ap; ap2 && (ap2 == ap || ap2.sp != rp.lhs); ap2 = ap2.next)
            {
            }
            Debug.Assert(ap2 != 0);
            ap.spOpt = ap2.sp;
            ap.type = ap2.type;
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: ap->x = ap2->x;
            ap.x.CopyFrom(ap2.x);
        }
    }
}

/*
** Renumber and resort states so that states with fewer choices
** occur at the end.  Except, keep state 0 as the first state.
*/
public static void ResortStates(lemon lemp)
{
    int i;
    state stp;
    action ap;

    for (i = 0; i < lemp.nstate; i++)
    {
        stp = lemp.sorted[i];
        stp.nTknAct = stp.nNtAct = 0;
        stp.iDfltReduce = lemp.nrule; // Init dflt action to "syntax error"
        stp.iTknOfst = DefineConstants.NO_OFFSET;
        stp.iNtOfst = DefineConstants.NO_OFFSET;
        for (ap = stp.ap; ap != null; ap = ap.next)
        {
            int iAction = compute_action(lemp, ap);
            if (iAction >= 0)
            {
                if (ap.sp.index < lemp.nterminal)
                {
                    stp.nTknAct++;
                }
                else if (ap.sp.index < lemp.nsymbol)
                {
                    stp.nNtAct++;
                }
                else
                {
                    Debug.Assert(stp.autoReduce == 0 || stp.pDfltReduce == ap.x.rp);
                    stp.iDfltReduce = iAction - lemp.nstate - lemp.nrule;
                }
            }
        }
    }
    qsort(lemp.sorted[1], lemp.nstate - 1, sizeof(state), stateResortCompare);
    for (i = 0; i < lemp.nstate; i++)
    {
        lemp.sorted[i].statenum = i;
    }
    lemp.nxstate = lemp.nstate;
    while (lemp.nxstate > 1 && lemp.sorted[lemp.nxstate - 1].autoReduce != 0)
    {
        lemp.nxstate--;
    }
}

/* Set the set size */

/********** From the file "set.h" ****************************************/
public static void SetSize(int n)
{
    size = n + 1;
}

/* Allocate a new set */
public static string SetNew()
{
    string s;
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
    s = (string)calloc(size, 1);
    if (s == 0)
    {
        //C++ TO C# CONVERTER NOTE: 'extern' variable declarations are not required in C#:
        //		extern void memory_error();
        memory_error();
    }
    return s;
}

/* Deallocate a set */
public static void SetFree(ref string s)
{
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
    free(s);
}

/* Add a new element to the set.  Return TRUE if the element was added
** and FALSE if it was already there. */
public static int SetAdd(ref string s, int e)
{
    int rv;
    Debug.Assert(e >= 0 && e < size);
    rv = s[e];
    s[e] = 1;
    return rv == 0;
}

/* Add every element of s2 to s1.  Return TRUE if s1 changes. */
public static int SetUnion(ref string s1, ref string s2)
{
    int i;
    int progress;
    progress = 0;
    for (i = 0; i < size; i++)
    {
        if (s2[i] == 0)
        {
            continue;
        }
        if (s1[i] == 0)
        {
            progress = 1;
            s1[i] = 1;
        }
    }
    return progress;
}

/* Works like strdup, sort of.  Save a string in malloced memory, but
** keep strings in a table so that the same string is not in more
** than one place.
*/

//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define MemoryCheck(X) if((X)==0){ extern void memory_error(); memory_error(); }

/**************** From the file "table.h" *********************************/
/*
** All code in this file has been automatically generated
** from a specification in the file
**              "table.q"
** by the associative array code building program "aagen".
** Do not edit this file!  Instead, edit the specification
** file, then rerun aagen.
*/
/*
** Code for processing tables in the LEMON parser generator.
*/
/* Routines for handling a strings */

public static string Strsafe(string y)
{
    string z;
    string cpy;

    if (y == 0)
    {
        return 0;
    }
    z = Strsafe_find(y);
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
    if (z == 0 && (cpy = (string)malloc(((int)y.Length) + 1)) != 0)
    {
        lemon_strcpy(cpy, y);
        z = cpy;
        Strsafe_insert(z);
    }
    if ((z) == 0)
    {
        //C++ TO C# CONVERTER NOTE: 'extern' variable declarations are not required in C#:
        //		extern void memory_error();
        memory_error();
    };
    return z;
}

/* Allocate a new associative array */

public static void Strsafe_init()
{
    if (x1a != null)
    {
        return;
    }
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
    x1a = (s_x1)malloc(sizeof(s_x1));
    if (x1a != null)
    {
        x1a.size = 1024;
        x1a.count = 0;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
        x1a.tbl = (s_x1node)calloc(1024, sizeof(s_x1node) + sizeof(s_x1node));
        if (x1a.tbl == 0)
        {
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(x1a);
            x1a = 0;
        }
        else
        {
            int i;
            x1a.ht = (s_x1node) & (x1a.tbl[1024]);
            for (i = 0; i < 1024; i++)
            {
                x1a.ht[i] = 0;
            }
        }
    }
}
/* Insert a new record into the array.  Return TRUE if successful.
** Prior data with the same key is NOT overwritten */
public static int Strsafe_insert(string data)
{
    s_x1node np;
    uint h;
    uint ph;

    if (x1a == 0)
    {
        return 0;
    }
    ph = strhash(data);
    h = ph & (x1a.size - 1);
    np = x1a.ht[h];
    while (np != null)
    {
        if (string.Compare(np.data, data) == 0)
        {
            /* An existing entry with the same key is found. */
            /* Fail because overwrite is not allows. */
            return 0;
        }
        np = np.next;
    }
    if (x1a.count >= x1a.size)
    {
        /* Need to make the hash table bigger */
        int i;
        int arrSize;
        s_x1 array = new s_x1();
        array.size = arrSize = x1a.size * 2;
        array.count = x1a.count;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
        array.tbl = (s_x1node)calloc(arrSize, sizeof(s_x1node) + sizeof(s_x1node));
        if (array.tbl == 0)
        {
            return 0; // Fail due to malloc failure
        }
        array.ht = (s_x1node) & (array.tbl[arrSize]);
        for (i = 0; i < arrSize; i++)
        {
            array.ht[i] = 0;
        }
        for (i = 0; i < x1a.count; i++)
        {
            s_x1node oldnp;
            s_x1node newnp;
            oldnp = (x1a.tbl[i]);
            h = strhash(oldnp.data) & (arrSize - 1);
            newnp = (array.tbl[i]);
            if (array.ht[h] != null)
            {
                array.ht[h].from = (newnp.next);
            }
            newnp.next = array.ht[h];
            newnp.data = oldnp.data;
            newnp.from = &(array.ht[h]);
            array.ht[h] = newnp;
        }
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
        free(x1a.tbl);
        //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
        //ORIGINAL LINE: *x1a = array;
        //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
        x1a.CopyFrom(array);
    }
    /* Insert the new data */
    h = ph & (x1a.size - 1);
    np = (x1a.tbl[x1a.count++]);
    np.data = data;
    if (x1a.ht[h] != null)
    {
        x1a.ht[h].from = (np.next);
    }
    np.next = x1a.ht[h];
    x1a.ht[h] = np;
    np.from = &(x1a.ht[h]);
    return 1;
}

/* Return a pointer to data assigned to the given key.  Return NULL
** if no such key. */
public static string Strsafe_find(string key)
{
    uint h;
    s_x1node np;

    if (x1a == 0)
    {
        return 0;
    }
    h = strhash(key) & (x1a.size - 1);
    np = x1a.ht[h];
    while (np != null)
    {
        if (string.Compare(np.data, key) == 0)
        {
            break;
        }
        np = np.next;
    }
    return np != null ? np.data : 0;
}

/* Return a pointer to the (terminal or nonterminal) symbol "x".
** Create a new symbol if this is the first time "x" has been seen.
*/

/* Routines for handling symbols of the grammar */

public static symbol Symbol_new(string x)
{
    symbol sp;

    sp = Symbol_find(x);
    if (sp == 0)
    {
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
        sp = (symbol)calloc(1, sizeof(symbol));
        if ((sp) == 0)
        {
            //C++ TO C# CONVERTER NOTE: 'extern' variable declarations are not required in C#:
            //			extern void memory_error();
            memory_error();
        };
        sp.name = Strsafe(x);
        sp.type = char.IsUpper((byte)(x)) ? symbol_type.TERMINAL : symbol_type.NONTERMINAL;
        sp.rule = 0;
        sp.fallback = 0;
        sp.prec = -1;
        sp.assoc = e_assoc.UNK;
        sp.firstset = 0;
        sp.lambda = Boolean.LEMON_FALSE;
        sp.destructor = 0;
        sp.destLineno = 0;
        sp.datatype = 0;
        sp.useCnt = 0;
        Symbol_insert(sp, sp.name);
    }
    sp.useCnt++;
    return sp;
}

/* Compare two symbols for sorting purposes.  Return negative,
** zero, or positive if a is less then, equal to, or greater
** than b.
**
** Symbols that begin with upper case letters (terminals or tokens)
** must sort before symbols that begin with lower case letters
** (non-terminals).  And MULTITERMINAL symbols (created using the
** %token_class directive) must sort at the very end. Other than
** that, the order does not matter.
**
** We find experimentally that leaving the symbols in their original
** order (the order they appeared in the grammar file) gives the
** smallest parser tables in SQLite.
*/
public static int Symbolcmpp(object _a, object _b)
{
    symbol a = *(const struct symbol **) _a;
		symbol b = *(const struct symbol **) _b;
		int i1 = (int)a.type == (int)((int)symbol_type.MULTITERMINAL) != 0 ? 3 : a.name[0] > 'Z' ? 2 : 1;
int i2 = (int)b.type == (int)((int)symbol_type.MULTITERMINAL) != 0 ? 3 : b.name[0] > 'Z' ? 2 : 1;
		return i1 == i2 != 0 ? a.index - b.index : i1 - i2;
	}

/* Allocate a new associative array */
	public static void Symbol_init()
{
    if (x2a != null)
    {
        return;
    }
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
    x2a = (s_x2)malloc(sizeof(s_x2));
    if (x2a != null)
    {
        x2a.size = 128;
        x2a.count = 0;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
        x2a.tbl = (s_x2node)calloc(128, sizeof(s_x2node) + sizeof(s_x2node));
        if (x2a.tbl == 0)
        {
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(x2a);
            x2a = 0;
        }
        else
        {
            int i;
            x2a.ht = (s_x2node) & (x2a.tbl[128]);
            for (i = 0; i < 128; i++)
            {
                x2a.ht[i] = 0;
            }
        }
    }
}
/* Insert a new record into the array.  Return TRUE if successful.
** Prior data with the same key is NOT overwritten */
public static int Symbol_insert(symbol data, string key)
{
    s_x2node np;
    uint h;
    uint ph;

    if (x2a == 0)
    {
        return 0;
    }
    ph = strhash(key);
    h = ph & (x2a.size - 1);
    np = x2a.ht[h];
    while (np != null)
    {
        if (string.Compare(np.key, key) == 0)
        {
            /* An existing entry with the same key is found. */
            /* Fail because overwrite is not allows. */
            return 0;
        }
        np = np.next;
    }
    if (x2a.count >= x2a.size)
    {
        /* Need to make the hash table bigger */
        int i;
        int arrSize;
        s_x2 array = new s_x2();
        array.size = arrSize = x2a.size * 2;
        array.count = x2a.count;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
        array.tbl = (s_x2node)calloc(arrSize, sizeof(s_x2node) + sizeof(s_x2node));
        if (array.tbl == 0)
        {
            return 0; // Fail due to malloc failure
        }
        array.ht = (s_x2node) & (array.tbl[arrSize]);
        for (i = 0; i < arrSize; i++)
        {
            array.ht[i] = 0;
        }
        for (i = 0; i < x2a.count; i++)
        {
            s_x2node oldnp;
            s_x2node newnp;
            oldnp = (x2a.tbl[i]);
            h = strhash(oldnp.key) & (arrSize - 1);
            newnp = (array.tbl[i]);
            if (array.ht[h] != null)
            {
                array.ht[h].from = (newnp.next);
            }
            newnp.next = array.ht[h];
            newnp.key = oldnp.key;
            newnp.data = oldnp.data;
            newnp.from = &(array.ht[h]);
            array.ht[h] = newnp;
        }
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
        free(x2a.tbl);
        //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
        //ORIGINAL LINE: *x2a = array;
        //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
        x2a.CopyFrom(array);
    }
    /* Insert the new data */
    h = ph & (x2a.size - 1);
    np = (x2a.tbl[x2a.count++]);
    np.key = key;
    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
    //ORIGINAL LINE: np->data = data;
    np.data = data;
    if (x2a.ht[h] != null)
    {
        x2a.ht[h].from = (np.next);
    }
    np.next = x2a.ht[h];
    x2a.ht[h] = np;
    np.from = &(x2a.ht[h]);
    return 1;
}

/* Return a pointer to data assigned to the given key.  Return NULL
** if no such key. */
public static symbol Symbol_find(string key)
{
    uint h;
    s_x2node np;

    if (x2a == 0)
    {
        return 0;
    }
    h = strhash(key) & (x2a.size - 1);
    np = x2a.ht[h];
    while (np != null)
    {
        if (string.Compare(np.key, key) == 0)
        {
            break;
        }
        np = np.next;
    }
    return np != null ? np.data : 0;
}

/* Return the n-th data.  Return NULL if n is out of range. */
public static symbol Symbol_Nth(int n)
{
    symbol data;
    if (x2a != null && n > 0 && n <= x2a.count)
    {
        data = x2a.tbl[n - 1].data;
    }
    else
    {
        data = 0;
    }
    return data;
}

/* Return the size of the array */
public static int Symbol_count()
{
    return x2a != null ? x2a.count : 0;
}

/* Return an array of pointers to all data in the table.
** The array is obtained from malloc.  Return NULL if memory allocation
** problems, or if the array is empty. */
public static symbol[] Symbol_arrayof()
{
    symbol[] array;
    int i;
    int arrSize;
    if (x2a == 0)
    {
        return 0;
    }
    arrSize = x2a.count;
    array = Arrays.InitializeWithDefaultInstances<symbol>(arrSize);
    if (array != null)
    {
        for (i = 0; i < arrSize; i++)
        {
            array[i] = x2a.tbl[i].data;
        }
    }
    return array;
}

/* Compare two configurations */

/* Routines to manage the state table */

public static int Configcmp(string _a, string _b)
{
    config a = (config)_a;
    config b = (config)_b;
    int x;
    x = a.rp.index - b.rp.index;
    if (x == 0)
    {
        x = a.dot - b.dot;
    }
    return x;
}

/* Allocate a new state structure */
public static state State_new()
{
    state newstate;
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
    newstate = (state)calloc(1, sizeof(state));
    if ((newstate) == 0)
    {
        //C++ TO C# CONVERTER NOTE: 'extern' variable declarations are not required in C#:
        //		extern void memory_error();
        memory_error();
    };
    return newstate;
}

/* Allocate a new associative array */
public static void State_init()
{
    if (x3a != null)
    {
        return;
    }
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
    x3a = (s_x3)malloc(sizeof(s_x3));
    if (x3a != null)
    {
        x3a.size = 128;
        x3a.count = 0;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
        x3a.tbl = (s_x3node)calloc(128, sizeof(s_x3node) + sizeof(s_x3node));
        if (x3a.tbl == 0)
        {
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(x3a);
            x3a = 0;
        }
        else
        {
            int i;
            x3a.ht = (s_x3node) & (x3a.tbl[128]);
            for (i = 0; i < 128; i++)
            {
                x3a.ht[i] = 0;
            }
        }
    }
}
/* Insert a new record into the array.  Return TRUE if successful.
** Prior data with the same key is NOT overwritten */
public static int State_insert(state data, config key)
{
    s_x3node np;
    uint h;
    uint ph;

    if (x3a == 0)
    {
        return 0;
    }
    ph = statehash(key);
    h = ph & (x3a.size - 1);
    np = x3a.ht[h];
    while (np != null)
    {
        if (statecmp(np.key, key) == 0)
        {
            /* An existing entry with the same key is found. */
            /* Fail because overwrite is not allows. */
            return 0;
        }
        np = np.next;
    }
    if (x3a.count >= x3a.size)
    {
        /* Need to make the hash table bigger */
        int i;
        int arrSize;
        s_x3 array = new s_x3();
        array.size = arrSize = x3a.size * 2;
        array.count = x3a.count;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
        array.tbl = (s_x3node)calloc(arrSize, sizeof(s_x3node) + sizeof(s_x3node));
        if (array.tbl == 0)
        {
            return 0; // Fail due to malloc failure
        }
        array.ht = (s_x3node) & (array.tbl[arrSize]);
        for (i = 0; i < arrSize; i++)
        {
            array.ht[i] = 0;
        }
        for (i = 0; i < x3a.count; i++)
        {
            s_x3node oldnp;
            s_x3node newnp;
            oldnp = (x3a.tbl[i]);
            h = statehash(oldnp.key) & (arrSize - 1);
            newnp = (array.tbl[i]);
            if (array.ht[h] != null)
            {
                array.ht[h].from = (newnp.next);
            }
            newnp.next = array.ht[h];
            newnp.key = oldnp.key;
            newnp.data = oldnp.data;
            newnp.from = &(array.ht[h]);
            array.ht[h] = newnp;
        }
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
        free(x3a.tbl);
        //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
        //ORIGINAL LINE: *x3a = array;
        //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
        x3a.CopyFrom(array);
    }
    /* Insert the new data */
    h = ph & (x3a.size - 1);
    np = (x3a.tbl[x3a.count++]);
    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
    //ORIGINAL LINE: np->key = key;
    np.key = key;
    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
    //ORIGINAL LINE: np->data = data;
    np.data = data;
    if (x3a.ht[h] != null)
    {
        x3a.ht[h].from = (np.next);
    }
    np.next = x3a.ht[h];
    x3a.ht[h] = np;
    np.from = &(x3a.ht[h]);
    return 1;
}

/* Return a pointer to data assigned to the given key.  Return NULL
** if no such key. */
public static state State_find(config key)
{
    uint h;
    s_x3node np;

    if (x3a == 0)
    {
        return 0;
    }
    h = statehash(key) & (x3a.size - 1);
    np = x3a.ht[h];
    while (np != null)
    {
        if (statecmp(np.key, key) == 0)
        {
            break;
        }
        np = np.next;
    }
    return np != null ? np.data : 0;
}

/* Return an array of pointers to all data in the table.
** The array is obtained from malloc.  Return NULL if memory allocation
** problems, or if the array is empty. */
public static state[] State_arrayof()
{
    state[] array;
    int i;
    int arrSize;
    if (x3a == 0)
    {
        return 0;
    }
    arrSize = x3a.count;
    array = Arrays.InitializeWithDefaultInstances<state>(arrSize);
    if (array != null)
    {
        for (i = 0; i < arrSize; i++)
        {
            array[i] = x3a.tbl[i].data;
        }
    }
    return array;
}

/* Allocate a new associative array */

/* Routines used for efficiency in Configlist_add */

public static void Configtable_init()
{
    if (x4a != null)
    {
        return;
    }
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
    x4a = (s_x4)malloc(sizeof(s_x4));
    if (x4a != null)
    {
        x4a.size = 64;
        x4a.count = 0;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
        x4a.tbl = (s_x4node)calloc(64, sizeof(s_x4node) + sizeof(s_x4node));
        if (x4a.tbl == 0)
        {
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(x4a);
            x4a = 0;
        }
        else
        {
            int i;
            x4a.ht = (s_x4node) & (x4a.tbl[64]);
            for (i = 0; i < 64; i++)
            {
                x4a.ht[i] = 0;
            }
        }
    }
}
/* Insert a new record into the array.  Return TRUE if successful.
** Prior data with the same key is NOT overwritten */
public static int Configtable_insert(config data)
{
    s_x4node np;
    uint h;
    uint ph;

    if (x4a == 0)
    {
        return 0;
    }
    ph = confighash(data);
    h = ph & (x4a.size - 1);
    np = x4a.ht[h];
    while (np != null)
    {
        if (Configcmp((string)np.data, (string)data) == 0)
        {
            /* An existing entry with the same key is found. */
            /* Fail because overwrite is not allows. */
            return 0;
        }
        np = np.next;
    }
    if (x4a.count >= x4a.size)
    {
        /* Need to make the hash table bigger */
        int i;
        int arrSize;
        s_x4 array = new s_x4();
        array.size = arrSize = x4a.size * 2;
        array.count = x4a.count;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
        array.tbl = (s_x4node)calloc(arrSize, sizeof(s_x4node) + sizeof(s_x4node));
        if (array.tbl == 0)
        {
            return 0; // Fail due to malloc failure
        }
        array.ht = (s_x4node) & (array.tbl[arrSize]);
        for (i = 0; i < arrSize; i++)
        {
            array.ht[i] = 0;
        }
        for (i = 0; i < x4a.count; i++)
        {
            s_x4node oldnp;
            s_x4node newnp;
            oldnp = (x4a.tbl[i]);
            h = confighash(oldnp.data) & (arrSize - 1);
            newnp = (array.tbl[i]);
            if (array.ht[h] != null)
            {
                array.ht[h].from = (newnp.next);
            }
            newnp.next = array.ht[h];
            newnp.data = oldnp.data;
            newnp.from = &(array.ht[h]);
            array.ht[h] = newnp;
        }
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
        free(x4a.tbl);
        //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
        //ORIGINAL LINE: *x4a = array;
        //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
        x4a.CopyFrom(array);
    }
    /* Insert the new data */
    h = ph & (x4a.size - 1);
    np = (x4a.tbl[x4a.count++]);
    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
    //ORIGINAL LINE: np->data = data;
    np.data = data;
    if (x4a.ht[h] != null)
    {
        x4a.ht[h].from = (np.next);
    }
    np.next = x4a.ht[h];
    x4a.ht[h] = np;
    np.from = &(x4a.ht[h]);
    return 1;
}

/* Return a pointer to data assigned to the given key.  Return NULL
** if no such key. */
public static config Configtable_find(config key)
{
    int h;
    s_x4node np;

    if (x4a == 0)
    {
        return 0;
    }
    h = confighash(key) & (x4a.size - 1);
    np = x4a.ht[h];
    while (np != null)
    {
        if (Configcmp((string)np.data, (string)key) == 0)
        {
            break;
        }
        np = np.next;
    }
    return np != null ? np.data : 0;
}

/* Remove all data from the table.  Pass each data to the function "f"
** as it is removed.  ("f" may be null to avoid this step.) */
private delegate int fDelegate(config UnnamedParameter1);
public static void Configtable_clear(fDelegate f)
{
    int i;
    if (x4a == 0 || x4a.count == 0)
    {
        return;
    }
    if (f != null)
    {
        for (i = 0; i < x4a.count; i++)
        {
            f(x4a.tbl[i].data);
        }
    }
    for (i = 0; i < x4a.size; i++)
    {
        x4a.ht[i] = 0;
    }
    x4a.count = 0;
    return;
}

/* Compare two actions for sorting purposes.  Return negative, zero, or
** positive if the first action is less than, equal to, or greater than
** the first
*/
internal static int actioncmp(action ap1, action ap2)
{
    int rc;
    rc = ap1.sp.index - ap2.sp.index;
    if (rc == 0)
    {
        rc = (int)ap1.type - (int)ap2.type;
    }
    if (rc == 0 && (ap1.type == e_action.REDUCE || ap1.type == e_action.SHIFTREDUCE))
    {
        rc = ap1.x.rp.index - ap2.x.rp.index;
    }
    if (rc == 0)
    {
        rc = (int)(ap2 - ap1);
    }
    return rc;
}

public static void Action_add(action[] app, e_action type, symbol sp, ref string arg)
{
    action newaction;
    newaction = Action_new();
    newaction.next = app;
    app = newaction;
    newaction.type = type;
    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
    //ORIGINAL LINE: newaction->sp = sp;
    newaction.sp = sp;
    newaction.spOpt = 0;
    if (type == e_action.SHIFT)
    {
        newaction.x.stp = (state)arg;
    }
    else
    {
        newaction.x.rp = (rule)arg;
    }
}

/* Return the number of entries in the yy_action table */
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define acttab_size(X) ((X)->nAction)

/* The value for the N-th entry in yy_action */
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define acttab_yyaction(X,N) ((X)->aAction[N].action)

/* The value for the N-th entry in yy_lookahead */
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define acttab_yylookahead(X,N) ((X)->aAction[N].lookahead)

/* Free all memory associated with the given acttab */
public static void acttab_free(acttab p)
{
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
    free(p.aAction);
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
    free(p.aLookahead);
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
    free(p);
}

/* Allocate a new acttab structure */
public static acttab acttab_alloc()
{
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
    acttab p = (acttab)calloc(1, sizeof(acttab));
    if (p == 0)
    {
        Console.Error.Write("Unable to allocate memory for a new acttab.");
        Environment.Exit(1);
    }
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
    memset(p, 0, sizeof(acttab));
    return p;
}

/* Add a new action to the current transaction set.
**
** This routine is called once for each lookahead for a particular
** state.
*/
public static void acttab_action(acttab p, int lookahead, int action)
{
    if (p.nLookahead >= p.nLookaheadAlloc)
    {
        p.nLookaheadAlloc += 25;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'realloc' has no equivalent in C#:
        p.aLookahead = (lookahead_action)realloc(p.aLookahead, sizeof(lookahead_action) * p.nLookaheadAlloc);
        if (p.aLookahead == 0)
        {
            Console.Error.Write("malloc failed\n");
            Environment.Exit(1);
        }
    }
    if (p.nLookahead == 0)
    {
        p.mxLookahead = lookahead;
        p.mnLookahead = lookahead;
        p.mnAction = action;
    }
    else
    {
        if (p.mxLookahead < lookahead)
        {
            p.mxLookahead = lookahead;
        }
        if (p.mnLookahead > lookahead)
        {
            p.mnLookahead = lookahead;
            p.mnAction = action;
        }
    }
    p.aLookahead[p.nLookahead].lookahead = lookahead;
    p.aLookahead[p.nLookahead].action = action;
    p.nLookahead++;
}

/*
** Add the transaction set built up with prior calls to acttab_action()
** into the current action table.  Then reset the transaction set back
** to an empty set in preparation for a new round of acttab_action() calls.
**
** Return the offset into the action table of the new transaction.
*/
public static int acttab_insert(acttab p)
{
    int i;
    int j;
    int k;
    int n;
    Debug.Assert(p.nLookahead > 0);

    /* Make sure we have enough space to hold the expanded action table
    ** in the worst case.  The worst case occurs if the transaction set
    ** must be appended to the current action table
    */
    n = p.mxLookahead + 1;
    if (p.nAction + n >= p.nActionAlloc)
    {
        int oldAlloc = p.nActionAlloc;
        p.nActionAlloc = p.nAction + n + p.nActionAlloc + 20;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'realloc' has no equivalent in C#:
        p.aAction = (lookahead_action)realloc(p.aAction, sizeof(lookahead_action) * p.nActionAlloc);
        if (p.aAction == 0)
        {
            Console.Error.Write("malloc failed\n");
            Environment.Exit(1);
        }
        for (i = oldAlloc; i < p.nActionAlloc; i++)
        {
            p.aAction[i].lookahead = -1;
            p.aAction[i].action = -1;
        }
    }

    /* Scan the existing action table looking for an offset that is a
    ** duplicate of the current transaction set.  Fall out of the loop
    ** if and when the duplicate is found.
    **
    ** i is the index in p->aAction[] where p->mnLookahead is inserted.
    */
    for (i = p.nAction - 1; i >= 0; i--)
    {
        if (p.aAction[i].lookahead == p.mnLookahead)
        {
            /* All lookaheads and actions in the aLookahead[] transaction
            ** must match against the candidate aAction[i] entry. */
            if (p.aAction[i].action != p.mnAction)
            {
                continue;
            }
            for (j = 0; j < p.nLookahead; j++)
            {
                k = p.aLookahead[j].lookahead - p.mnLookahead + i;
                if (k < 0 || k >= p.nAction)
                {
                    break;
                }
                if (p.aLookahead[j].lookahead != p.aAction[k].lookahead)
                {
                    break;
                }
                if (p.aLookahead[j].action != p.aAction[k].action)
                {
                    break;
                }
            }
            if (j < p.nLookahead)
            {
                continue;
            }

            /* No possible lookahead value that is not in the aLookahead[]
            ** transaction is allowed to match aAction[i] */
            n = 0;
            for (j = 0; j < p.nAction; j++)
            {
                if (p.aAction[j].lookahead < 0)
                {
                    continue;
                }
                if (p.aAction[j].lookahead == j + p.mnLookahead - i)
                {
                    n++;
                }
            }
            if (n == p.nLookahead)
            {
                break; // An exact match is found at offset i
            }
        }
    }

    /* If no existing offsets exactly match the current transaction, find an
    ** an empty offset in the aAction[] table in which we can add the
    ** aLookahead[] transaction.
    */
    if (i < 0)
    {
        /* Look for holes in the aAction[] table that fit the current
        ** aLookahead[] transaction.  Leave i set to the offset of the hole.
        ** If no holes are found, i is left at p->nAction, which means the
        ** transaction will be appended. */
        for (i = 0; i < p.nActionAlloc - p.mxLookahead; i++)
        {
            if (p.aAction[i].lookahead < 0)
            {
                for (j = 0; j < p.nLookahead; j++)
                {
                    k = p.aLookahead[j].lookahead - p.mnLookahead + i;
                    if (k < 0)
                    {
                        break;
                    }
                    if (p.aAction[k].lookahead >= 0)
                    {
                        break;
                    }
                }
                if (j < p.nLookahead)
                {
                    continue;
                }
                for (j = 0; j < p.nAction; j++)
                {
                    if (p.aAction[j].lookahead == j + p.mnLookahead - i)
                    {
                        break;
                    }
                }
                if (j == p.nAction)
                {
                    break; // Fits in empty slots
                }
            }
        }
    }
    /* Insert transaction set at index i. */
    for (j = 0; j < p.nLookahead; j++)
    {
        k = p.aLookahead[j].lookahead - p.mnLookahead + i;
        p.aAction[k] = p.aLookahead[j];
        if (k >= p.nAction)
        {
            p.nAction = k + 1;
        }
    }
    p.nLookahead = 0;

    /* Return the offset that is added to the lookahead in order to get the
    ** index into yy_action of the action */
    return i - p.mnLookahead;
}

/********************** From the file "build.c" *****************************/
/*
** Routines to construction the finite state machine for the LEMON
** parser generator.
*/

/* Find a precedence symbol of every rule in the grammar.
**
** Those rules which have a precedence symbol coded in the input
** grammar using the "[symbol]" construct will already have the
** rp->precsym field filled.  Other rules take as their precedence
** symbol the first RHS symbol with a defined precedence.  If there
** are not RHS symbols with a defined precedence, the precedence
** symbol field is left blank.
*/
public static void FindRulePrecedences(lemon xp)
{
    rule rp;
    for (rp = xp.rule; rp != null; rp = rp.next)
    {
        if (rp.precsym == 0)
        {
            int i;
            int j;
            for (i = 0; i < rp.nrhs && rp.precsym == 0; i++)
            {
                symbol[] sp = rp.rhs[i];
                if (sp.type == symbol_type.MULTITERMINAL)
                {
                    for (j = 0; j < sp.nsubsym; j++)
                    {
                        if (sp.subsym[j].prec >= 0)
                        {
                            rp.precsym = sp.subsym[j];
                            break;
                        }
                    }
                }
                else if (sp.prec >= 0)
                {
                    rp.precsym = rp.rhs[i];
                }
            }
        }
    }
    return;
}

/* Find all nonterminals which will generate the empty string.
** Then go back and compute the first sets of every nonterminal.
** The first set is the set of all terminal symbols which can begin
** a string generated by that nonterminal.
*/
public static void FindFirstSets(lemon lemp)
{
    int i;
    int j;
    rule rp;
    int progress;

    for (i = 0; i < lemp.nsymbol; i++)
    {
        lemp.symbols[i].lambda = Boolean.LEMON_FALSE;
    }
    for (i = lemp.nterminal; i < lemp.nsymbol; i++)
    {
        lemp.symbols[i].firstset = SetNew();
    }

    /* First compute all lambdas */
    do
    {
        progress = 0;
        for (rp = lemp.rule; rp != null; rp = rp.next)
        {
            if (rp.lhs.lambda)
            {
                continue;
            }
            for (i = 0; i < rp.nrhs; i++)
            {
                symbol[] sp = rp.rhs[i];
                Debug.Assert(sp.type == symbol_type.NONTERMINAL || sp.lambda == Boolean.LEMON_FALSE);
                if (sp.lambda == Boolean.LEMON_FALSE)
                {
                    break;
                }
            }
            if (i == rp.nrhs)
            {
                rp.lhs.lambda = Boolean.LEMON_TRUE;
                progress = 1;
            }
        }
    } while (progress != 0);

    /* Now compute all first sets */
    do
    {
        symbol s1;
        symbol s2;
        progress = 0;
        for (rp = lemp.rule; rp != null; rp = rp.next)
        {
            s1 = rp.lhs;
            for (i = 0; i < rp.nrhs; i++)
            {
                s2 = rp.rhs[i];
                if (s2.type == symbol_type.TERMINAL)
                {
                    progress += SetAdd(ref s1.firstset, s2.index);
                    break;
                }
                else if (s2.type == symbol_type.MULTITERMINAL)
                {
                    for (j = 0; j < s2.nsubsym; j++)
                    {
                        progress += SetAdd(ref s1.firstset, s2.subsym[j].index);
                    }
                    break;
                }
                else if (s1 == s2)
                {
                    if (s1.lambda == Boolean.LEMON_FALSE)
                    {
                        break;
                    }
                }
                else
                {
                    progress += SetUnion(ref s1.firstset, ref s2.firstset);
                    if (s2.lambda == Boolean.LEMON_FALSE)
                    {
                        break;
                    }
                }
            }
        }
    } while (progress != 0);
    return;
}

/* Compute all LR(0) states for the grammar.  Links
** are added to between some states so that the LR(1) follow sets
** can be computed later.
*/
public static state getstate(lemon lemp)
{
    config cfp;
    config bp;
    state stp;

    /* Extract the sorted basis of the new state.  The basis was constructed
    ** by prior calls to "Configlist_addbasis()". */
    Configlist_sortbasis();
    bp = Configlist_basis();

    /* Get a state with the same basis */
    stp = State_find(bp);
    if (stp != null)
    {
        /* A state with the same basis already exists!  Copy all the follow-set
        ** propagation links from the state under construction into the
        ** preexisting state, then return a pointer to the preexisting state */
        config x;
        config y;
        for (x = bp, y = stp.bp; x && y != null; x = x.bp, y = y.bp)
        {
            Plink_copy(y.bplp, x.bplp);
            Plink_delete(x.fplp);
            x.fplp = x.bplp = 0;
        }
        cfp = Configlist_return();
        Configlist_eat(cfp);
    }
    else
    {
        /* This really is a new state.  Construct all the details */
        Configlist_closure(lemp); // Compute the configuration closure
        Configlist_sort(); // Sort the configuration closure
        cfp = Configlist_return(); // Get a pointer to the config list
        stp = State_new(); // A new state structure
        if ((stp) == 0)
        {
            //C++ TO C# CONVERTER NOTE: 'extern' variable declarations are not required in C#:
            //			extern void memory_error();
            memory_error();
        };
        //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
        //ORIGINAL LINE: stp->bp = bp;
        stp.bp = bp; // Remember the configuration basis
                     //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
                     //ORIGINAL LINE: stp->cfp = cfp;
        stp.cfp = cfp; // Remember the configuration closure
        stp.statenum = lemp.nstate++; // Every state gets a sequence number
        stp.ap = 0; // No actions, yet.
        State_insert(stp, stp.bp); // Add to the state table
        buildshifts(lemp, stp); // Recursively compute successor states
    }
    return stp;
}
public static void FindStates(lemon lemp)
{
    symbol sp;
    rule rp;

    Configlist_init();

    /* Find the start symbol */
    if (lemp.start != 0)
    {
        sp = Symbol_find(lemp.start);
        if (sp == 0)
        {
            ErrorMsg(lemp.filename, 0, "The specified start symbol \"%s\" is not in a nonterminal of the grammar.  \"%s\" will be used as the start symbol instead.", lemp.start, lemp.startRule.lhs.name);
            lemp.errorcnt++;
            sp = lemp.startRule.lhs;
        }
    }
    else
    {
        sp = lemp.startRule.lhs;
    }

    /* Make sure the start symbol doesn't occur on the right-hand side of
    ** any rule.  Report an error if it does.  (YACC would generate a new
    ** start symbol in this case.) */
    for (rp = lemp.rule; rp != null; rp = rp.next)
    {
        int i;
        for (i = 0; i < rp.nrhs; i++)
        {
            if (rp.rhs[i] == sp)
            { // FIX ME:  Deal with multiterminals
                ErrorMsg(lemp.filename, 0, "The start symbol \"%s\" occurs on the right-hand side of a rule. This will result in a parser which does not work properly.", sp.name);
                lemp.errorcnt++;
            }
        }
    }

    /* The basis configuration set for the first state
    ** is all rules which have the start symbol as their
    ** left-hand side */
    for (rp = sp.rule; rp != null; rp = rp.nextlhs)
    {
        config newcfp;
        rp.lhsStart = 1;
        newcfp = Configlist_addbasis(rp, 0);
        SetAdd(ref newcfp.fws, 0);
    }

    /* Compute the first state.  All other states will be
    ** computed automatically during the computation of the first one.
    ** The returned pointer to the first state is not used. */
    getstate(lemp);
    return;
}

/* Construct all successor states to the given state.  A "successor"
** state is any state which can be reached by a shift action.
*/

/* Return a pointer to a state which is described by the configuration
** list which has been built from calls to Configlist_add.
*/
public static void buildshifts(lemon lemp, state stp)
{
    config cfp; // For looping thru the config closure of "stp"
    config bcfp; // For the inner loop on config closure of "stp"
    config newcfg;
    symbol sp; // Symbol following the dot in configuration "cfp"
    symbol bsp; // Symbol following the dot in configuration "bcfp"
    state newstp; // A pointer to a successor state

    /* Each configuration becomes complete after it contibutes to a successor
    ** state.  Initially, all configurations are incomplete */
    for (cfp = stp.cfp; cfp != null; cfp = cfp.next)
    {
        cfp.status = cfgstatus.INCOMPLETE;
    }

    /* Loop through all configurations of the state "stp" */
    for (cfp = stp.cfp; cfp != null; cfp = cfp.next)
    {
        if (cfp.status == cfgstatus.COMPLETE)
        {
            continue; // Already used by inner loop
        }
        if (cfp.dot >= cfp.rp.nrhs)
        {
            continue; // Can't shift this config
        }
        Configlist_reset(); // Reset the new config set
        sp = cfp.rp.rhs[cfp.dot]; // Symbol after the dot

        /* For every configuration in the state "stp" which has the symbol "sp"
        ** following its dot, add the same configuration to the basis set under
        ** construction but with the dot shifted one symbol to the right. */
        for (bcfp = cfp; bcfp != null; bcfp = bcfp.next)
        {
            if (bcfp.status == cfgstatus.COMPLETE)
            {
                continue; // Already used
            }
            if (bcfp.dot >= bcfp.rp.nrhs)
            {
                continue; // Can't shift this one
            }
            bsp = bcfp.rp.rhs[bcfp.dot]; // Get symbol after dot
            if (same_symbol(bsp, sp) == 0)
            {
                continue; // Must be same as for "cfp"
            }
            bcfp.status = cfgstatus.COMPLETE; // Mark this config as used
                                              //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                                              //ORIGINAL LINE: newcfg = Configlist_addbasis(bcfp->rp, bcfp->dot + 1);
            newcfg = Configlist_addbasis(new rule(bcfp.rp), bcfp.dot + 1);
            Plink_add(newcfg.bplp, bcfp);
        }

        /* Get a pointer to the state described by the basis configuration set
        ** constructed in the preceding loop */
        newstp = getstate(lemp);

        /* The state "newstp" is reached from the state "stp" by a shift action
        ** on the symbol "sp" */
        if (sp.type == symbol_type.MULTITERMINAL)
        {
            int i;
            for (i = 0; i < sp.nsubsym; i++)
            {
                //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                //ORIGINAL LINE: Action_add(&stp->ap, SHIFT, sp->subsym[i], (sbyte*)newstp);
                Action_add(stp.ap, e_action.SHIFT, new symbol(sp.subsym[i]), ref (string)newstp);
            }
        }
        else
        {
            Action_add(stp.ap, e_action.SHIFT, sp, ref (string)newstp);
        }
    }
}

/*
** Return true if two symbols are the same.
*/
public static int same_symbol(symbol a, symbol b)
{
    int i;
    if (a == b)
    {
        return 1;
    }
    if (a.type != symbol_type.MULTITERMINAL)
    {
        return 0;
    }
    if (b.type != symbol_type.MULTITERMINAL)
    {
        return 0;
    }
    if (a.nsubsym != b.nsubsym)
    {
        return 0;
    }
    for (i = 0; i < a.nsubsym; i++)
    {
        if (a.subsym[i] != b.subsym[i])
        {
            return 0;
        }
    }
    return 1;
}

/*
** Construct the propagation links
*/
public static void FindLinks(lemon lemp)
{
    int i;
    config cfp;
    config other;
    state stp;
    plink plp;

    /* Housekeeping detail:
    ** Add to every propagate link a pointer back to the state to
    ** which the link is attached. */
    for (i = 0; i < lemp.nstate; i++)
    {
        stp = lemp.sorted[i];
        for (cfp = stp.cfp; cfp != null; cfp = cfp.next)
        {
            //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
            //ORIGINAL LINE: cfp->stp = stp;
            cfp.stp = stp;
        }
    }

    /* Convert all backlinks into forward links.  Only the forward
    ** links are used in the follow-set computation. */
    for (i = 0; i < lemp.nstate; i++)
    {
        stp = lemp.sorted[i];
        for (cfp = stp.cfp; cfp != null; cfp = cfp.next)
        {
            for (plp = cfp.bplp; plp != null; plp = plp.next)
            {
                other = plp.cfp;
                Plink_add(other.fplp, cfp);
            }
        }
    }
}

/* Compute all followsets.
**
** A followset is the set of all symbols which can come immediately
** after a configuration.
*/
public static void FindFollowSets(lemon lemp)
{
    int i;
    config cfp;
    plink plp;
    int progress;
    int change;

    for (i = 0; i < lemp.nstate; i++)
    {
        for (cfp = lemp.sorted[i].cfp; cfp != null; cfp = cfp.next)
        {
            cfp.status = cfgstatus.INCOMPLETE;
        }
    }

    do
    {
        progress = 0;
        for (i = 0; i < lemp.nstate; i++)
        {
            for (cfp = lemp.sorted[i].cfp; cfp != null; cfp = cfp.next)
            {
                if (cfp.status == cfgstatus.COMPLETE)
                {
                    continue;
                }
                for (plp = cfp.fplp; plp != null; plp = plp.next)
                {
                    change = SetUnion(ref plp.cfp.fws, ref cfp.fws);
                    if (change != 0)
                    {
                        plp.cfp.status = cfgstatus.INCOMPLETE;
                        progress = 1;
                    }
                }
                cfp.status = cfgstatus.COMPLETE;
            }
        }
    } while (progress != 0);
}

/* Resolve a conflict between the two given actions.  If the
** conflict can't be resolved, return non-zero.
**
** NO LONGER TRUE:
**   To resolve a conflict, first look to see if either action
**   is on an error rule.  In that case, take the action which
**   is not associated with the error rule.  If neither or both
**   actions are associated with an error rule, then try to
**   use precedence to resolve the conflict.
**
** If either action is a SHIFT, then it must be apx.  This
** function won't work if apx->type==REDUCE and apy->type==SHIFT.
*/

internal static int resolve_conflict(action apx, action apy)
{
    symbol spx;
    symbol spy;
    int errcnt = 0;
    Debug.Assert(apx.sp == apy.sp); // Otherwise there would be no conflict
    if (apx.type == e_action.SHIFT && apy.type == e_action.SHIFT)
    {
        apy.type = e_action.SSCONFLICT;
        errcnt++;
    }
    if (apx.type == e_action.SHIFT && apy.type == e_action.REDUCE)
    {
        spx = apx.sp;
        spy = apy.x.rp.precsym;
        if (spy == 0 || spx.prec < 0 || spy.prec < 0)
        {
            /* Not enough precedence information. */
            apy.type = e_action.SRCONFLICT;
            errcnt++;
        }
        else if (spx.prec > spy.prec)
        { // higher precedence wins
            apy.type = e_action.RD_RESOLVED;
        }
        else if (spx.prec < spy.prec)
        {
            apx.type = e_action.SH_RESOLVED;
        }
        else if (spx.prec == spy.prec && spx.assoc == e_assoc.RIGHT)
        { // Use operator
            apy.type = e_action.RD_RESOLVED; // associativity
        }
        else if (spx.prec == spy.prec && spx.assoc == e_assoc.LEFT)
        { // to break tie
            apx.type = e_action.SH_RESOLVED;
        }
        else
        {
            Debug.Assert(spx.prec == spy.prec && spx.assoc == e_assoc.NONE);
            apx.type = e_action.ERROR;
        }
    }
    else if (apx.type == e_action.REDUCE && apy.type == e_action.REDUCE)
    {
        spx = apx.x.rp.precsym;
        spy = apy.x.rp.precsym;
        if (spx == 0 || spy == 0 || spx.prec < 0 || spy.prec < 0 || spx.prec == spy.prec)
        {
            apy.type = e_action.RRCONFLICT;
            errcnt++;
        }
        else if (spx.prec > spy.prec)
        {
            apy.type = e_action.RD_RESOLVED;
        }
        else if (spx.prec < spy.prec)
        {
            apx.type = e_action.RD_RESOLVED;
        }
    }
    else
    {
        Debug.Assert(apx.type == e_action.SH_RESOLVED || apx.type == e_action.RD_RESOLVED || apx.type == e_action.SSCONFLICT || apx.type == e_action.SRCONFLICT || apx.type == e_action.RRCONFLICT || apy.type == e_action.SH_RESOLVED || apy.type == e_action.RD_RESOLVED || apy.type == e_action.SSCONFLICT || apy.type == e_action.SRCONFLICT || apy.type == e_action.RRCONFLICT);
        /* The REDUCE/SHIFT case cannot happen because SHIFTs come before
        ** REDUCEs on the list.  If we reach this point it must be because
        ** the parser conflict had already been resolved. */
    }
    return errcnt;
}

/* Compute the reduce actions, and resolve conflicts.
*/
public static void FindActions(lemon lemp)
{
    int i;
    int j;
    config cfp;
    state stp;
    symbol sp;
    rule rp;

    /* Add all of the reduce actions
    ** A reduce action is added for each element of the followset of
    ** a configuration which has its dot at the extreme right.
    */
    for (i = 0; i < lemp.nstate; i++)
    { // Loop over all states
        stp = lemp.sorted[i];
        for (cfp = stp.cfp; cfp != null; cfp = cfp.next)
        { // Loop over all configurations
            if (cfp.rp.nrhs == cfp.dot)
            { // Is dot at extreme right?
                for (j = 0; j < lemp.nterminal; j++)
                {
                    if ((cfp.fws[j]))
                    {
                        /* Add a reduce action to the state "stp" which will reduce by the
                        ** rule "cfp->rp" if the lookahead symbol is "lemp->symbols[j]" */
                        //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                        //ORIGINAL LINE: Action_add(&stp->ap, REDUCE, lemp->symbols[j], (sbyte *)cfp->rp);
                        Action_add(stp.ap, e_action.REDUCE, new symbol(lemp.symbols[j]), ref (string)cfp.rp);
                    }
                }
            }
        }
    }

    /* Add the accepting token */
    if (lemp.start != 0)
    {
        sp = Symbol_find(lemp.start);
        if (sp == 0)
        {
            sp = lemp.startRule.lhs;
        }
    }
    else
    {
        sp = lemp.startRule.lhs;
    }
    /* Add to the first state (which is always the starting state of the
    ** finite state machine) an action to ACCEPT if the lookahead is the
    ** start nonterminal.  */
    Action_add(lemp.sorted[0].ap, e_action.ACCEPT, sp, 0);

    /* Resolve conflicts */
    for (i = 0; i < lemp.nstate; i++)
    {
        action ap;
        action nap;
        stp = lemp.sorted[i];
        /* assert( stp->ap ); */
        stp.ap = Action_sort(stp.ap);
        for (ap = stp.ap; ap && ap.next != null; ap = ap.next)
        {
            for (nap = ap.next; nap && nap.sp == ap.sp; nap = nap.next)
            {
                /* The two actions "ap" and "nap" have the same lookahead.
                ** Figure out which one should be used */
                lemp.nconflict += resolve_conflict(ap, nap);
            }
        }
    }

    /* Report an error for each rule that can never be reduced. */
    for (rp = lemp.rule; rp != null; rp = rp.next)
    {
        rp.canReduce = Boolean.LEMON_FALSE;
    }
    for (i = 0; i < lemp.nstate; i++)
    {
        action ap;
        for (ap = lemp.sorted[i].ap; ap != null; ap = ap.next)
        {
            if (ap.type == e_action.REDUCE)
            {
                ap.x.rp.canReduce = Boolean.LEMON_TRUE;
            }
        }
    }
    for (rp = lemp.rule; rp != null; rp = rp.next)
    {
        if (rp.canReduce)
        {
            continue;
        }
        ErrorMsg(lemp.filename, rp.ruleline, "This rule can not be reduced.\n");
        lemp.errorcnt++;
    }
}
/********************* From the file "configlist.c" *************************/
/*
** Routines to processing a configuration list and building a state
** in the LEMON parser generator.
*/

internal static config[] freelist = 0; // List of free configurations
internal static config current = 0; // Top of list of configurations
internal static config[] currentend = 0; // Last on list of configs
internal static config basis = 0; // Top of list of basis configs
internal static config[] basisend = 0; // End of list of basis configs

/* Return a pointer to a new configuration */
public static config newconfig()
{
    config newcfg;
    if (freelist == 0)
    {
        int i;
        int amt = 3;
        freelist = Arrays.InitializeWithDefaultInstances<config>(amt);
        if (freelist == 0)
        {
            Console.Error.Write("Unable to allocate memory for a new configuration.");
            Environment.Exit(1);
        }
        for (i = 0; i < amt - 1; i++)
        {
            freelist[i].next = freelist[i + 1];
        }
        freelist[amt - 1].next = 0;
    }
    newcfg = freelist;
    freelist = freelist.next;
    return newcfg;
}

/* The configuration "old" is no longer used */
public static void deleteconfig(config old)
{
    old.next = freelist;
    freelist = old;
}
/**************** From the file "main.c" ************************************/
/*
** Main program file for the LEMON parser generator.
*/

/* Report an out-of-memory condition and abort.  This function
** is used mostly by the "MemoryCheck" macro in struct.h
*/
public static void memory_error()
{
    Console.Error.Write("Out of memory.  Aborting...\n");
    Environment.Exit(1);
}

internal static int nDefine = 0; // Number of -D options on the command line
internal static string[] azDefine = 0; // Name of the -D macros

/* This routine is called with the argument to each -D command-line option.
** Add the macro defined to the azDefine array.
*/
//C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on the parameter 'z', so pointers on this parameter are left unchanged:
internal static void handle_D_option(sbyte* z)
{
    string[] paz;
    nDefine++;
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'realloc' has no equivalent in C#:
    azDefine = (string)realloc(azDefine, sizeof(sbyte) * nDefine);
    if (azDefine == 0)
    {
        Console.Error.Write("out of memory\n");
        Environment.Exit(1);
    }
    paz = azDefine[nDefine - 1];
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
    paz = (string)malloc(((int)z.Length) + 1);
    if (paz == 0)
    {
        Console.Error.Write("out of memory\n");
        Environment.Exit(1);
    }
    lemon_strcpy(paz, z);
    for (z = paz; *z && *z != '='; z++)
    {
    }
    *z = 0;
}

internal static string user_templatename = null;
internal static void handle_T_option(ref string z)
{
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
    user_templatename = (string)malloc(((int)z.Length) + 1);
    if (user_templatename == 0)
    {
        memory_error();
    }
    lemon_strcpy(user_templatename, z);
}

/* Merge together to lists of rules ordered by rule.iRule */
internal static rule Rule_merge(rule pA, rule pB)
{
    rule pFirst = 0;
    rule[] ppPrev = pFirst;
    while (pA != null && pB != null)
    {
        if (pA.iRule < pB.iRule)
        {
            ppPrev = pA;
            ppPrev = pA.next;
            pA = pA.next;
        }
        else
        {
            ppPrev = pB;
            ppPrev = pB.next;
            pB = pB.next;
        }
    }
    if (pA != null)
    {
        ppPrev = pA;
    }
    else
    {
        ppPrev = pB;
    }
    return pFirst;
}

/*
** Sort a list of rules in order of increasing iRule value
*/
internal static rule Rule_sort(rule rp)
{
    int i;
    rule pNext;
    rule[] x = Arrays.InitializeWithDefaultInstances<rule>(32);
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
    memset(x, 0, sizeof(rule));
    while (rp != null)
    {
        pNext = rp.next;
        rp.next = 0;
        //C++ TO C# CONVERTER WARNING: This 'sizeof' ratio was replaced with a direct reference to the array length:
        //ORIGINAL LINE: for (i = 0; i<sizeof(x) / sizeof(x[0]) && x[i]; i++)
        for (i = 0; i < x.Length && x[i] != null; i++)
        {
            rp = Rule_merge(x[i], rp);
            x[i] = 0;
        }
        x[i] = rp;
        //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
        //ORIGINAL LINE: rp = pNext;
        rp = pNext;
    }
    rp = 0;
    //C++ TO C# CONVERTER WARNING: This 'sizeof' ratio was replaced with a direct reference to the array length:
    //ORIGINAL LINE: for (i = 0; i<sizeof(x) / sizeof(x[0]); i++)
    for (i = 0; i < x.Length; i++)
    {
        rp = Rule_merge(x[i], rp);
    }
    return rp;
}

/*
** Return the name of a C datatype able to represent values between
** lwr and upr, inclusive.  If pnByte!=NULL then also write the sizeof
** for that type (1, 2, or 4) into *pnByte.
*/

/* forward reference */
internal static string minimum_size_type(int lwr, int upr, ref int pnByte)
{
    string zType = "int";
    int nByte = 4;
    if (lwr >= 0)
    {
        if (upr <= 255)
        {
            zType = "unsigned char";
            nByte = 1;
        }
        else if (upr < 65535)
        {
            zType = "unsigned short int";
            nByte = 2;
        }
        else
        {
            zType = "unsigned int";
            nByte = 4;
        }
    }
    else if (lwr >= -127 && upr <= 127)
    {
        zType = "signed char";
        nByte = 1;
    }
    else if (lwr >= -32767 && upr < 32767)
    {
        zType = "short";
        nByte = 2;
    }
    if (pnByte != 0)
    {
        pnByte = nByte;
    }
    return zType;
}

/* Print a single line of the "Parser Stats" output
*/
internal static void stats_line(string zLabel, int iValue)
{
    int nLabel = ((int)zLabel.Length);
    //C++ TO C# CONVERTER TODO TASK: The following line has a C format specifier which cannot be directly translated to C#:
    //ORIGINAL LINE: printf("  %s%.*s %5d\n", zLabel, 35 - nLabel, "................................", iValue);
    Console.Write("  {0}%.*s {2,5:D}\n", zLabel, 35 - nLabel, "................................", iValue);
}
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static int Main_version = 0;
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static int Main_rpflag = 0;
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static int Main_basisflag = 0;
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static int Main_compress = 0;
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static int Main_quiet = 0;
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static int Main_statistics = 0;
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static int Main_mhflag = 0;
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static int Main_nolinenosflag = 0;
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static int Main_noResort = 0;
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static Main_s_options[] options =
{
    new Main_s_options(option_type.OPT_FLAG, "b", (string) & basisflag, "Print only the basis in report."),
    new Main_s_options(option_type.OPT_FLAG, "c", (string) & compress, "Don't compress the action table."),
    new Main_s_options(option_type.OPT_FSTR, "D", (string)handle_D_option, "Define an %ifdef macro."),
    new Main_s_options(option_type.OPT_FSTR, "f", 0, "Ignored.  (Placeholder for -f compiler options.)"),
    new Main_s_options(option_type.OPT_FLAG, "g", (string) & rpflag, "Print grammar without actions."),
    new Main_s_options(option_type.OPT_FSTR, "I", 0, "Ignored.  (Placeholder for '-I' compiler options.)"),
    new Main_s_options(option_type.OPT_FLAG, "m", (string) & mhflag, "Output a makeheaders compatible file."),
    new Main_s_options(option_type.OPT_FLAG, "l", (string) & nolinenosflag, "Do not print #line statements."),
    new Main_s_options(option_type.OPT_FSTR, "O", 0, "Ignored.  (Placeholder for '-O' compiler options.)"),
    new Main_s_options(option_type.OPT_FLAG, "p", (string) & showPrecedenceConflict, "Show conflicts resolved by precedence rules"),
    new Main_s_options(option_type.OPT_FLAG, "q", (string) & quiet, "(Quiet) Don't print the report file."),
    new Main_s_options(option_type.OPT_FLAG, "r", (string) & noResort, "Do not sort or renumber states"),
    new Main_s_options(option_type.OPT_FLAG, "s", (string) & statistics, "Print parser stats to standard output."),
    new Main_s_options(option_type.OPT_FLAG, "x", (string) & version, "Print the version number."),
    new Main_s_options(option_type.OPT_FSTR, "T", (string)handle_T_option, "Specify a template file."),
    new Main_s_options(option_type.OPT_FSTR, "W", 0, "Ignored.  (Placeholder for '-W' compiler options.)"),
    new Main_s_options(option_type.OPT_FLAG, 0, 0, 0)
};

/* The main program.  Parse the command line and do it... */
static int Main(string[] args)
{
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static int version = 0;
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static int rpflag = 0;
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static int basisflag = 0;
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static int compress = 0;
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static int quiet = 0;
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static int statistics = 0;
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static int mhflag = 0;
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static int nolinenosflag = 0;
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static int noResort = 0;
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static struct s_options options[] = { { OPT_FLAG, "b", (sbyte*)&basisflag, "Print only the basis in report." }, { OPT_FLAG, "c", (sbyte*)&compress, "Don't compress the action table." }, { OPT_FSTR, "D", (sbyte*)handle_D_option, "Define an %ifdef macro." }, { OPT_FSTR, "f", 0, "Ignored.  (Placeholder for -f compiler options.)" }, { OPT_FLAG, "g", (sbyte*)&rpflag, "Print grammar without actions." }, { OPT_FSTR, "I", 0, "Ignored.  (Placeholder for '-I' compiler options.)" }, { OPT_FLAG, "m", (sbyte*)&mhflag, "Output a makeheaders compatible file." }, { OPT_FLAG, "l", (sbyte*)&nolinenosflag, "Do not print #line statements." }, { OPT_FSTR, "O", 0, "Ignored.  (Placeholder for '-O' compiler options.)" }, { OPT_FLAG, "p", (sbyte*)&showPrecedenceConflict, "Show conflicts resolved by precedence rules" }, { OPT_FLAG, "q", (sbyte*)&quiet, "(Quiet) Don't print the report file." }, { OPT_FLAG, "r", (sbyte*)&noResort, "Do not sort or renumber states" }, { OPT_FLAG, "s", (sbyte*)&statistics, "Print parser stats to standard output." }, { OPT_FLAG, "x", (sbyte*)&version, "Print the version number." }, { OPT_FSTR, "T", (sbyte*)handle_T_option, "Specify a template file." }, { OPT_FSTR, "W", 0, "Ignored.  (Placeholder for '-W' compiler options.)" }, { OPT_FLAG,0,0,0 } };
    int i;
    int exitcode;
    lemon lem = new lemon();
    rule rp;

    //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
    //ORIGINAL LINE: OptInit(args, options, stderr);
    OptInit(args, new s_options(options), stderr);
    if (Main_version)
    {
        Console.Write("Lemon version 1.0\n");
        Environment.Exit(0);
    }
    if (OptNArgs() != 1)
    {
        Console.Error.Write("Exactly one filename argument is required.\n");
        Environment.Exit(1);
    }
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
    memset(lem, 0, sizeof(lemon));
    lem.errorcnt = 0;

    /* Initialize the machine */
    Strsafe_init();
    Symbol_init();
    State_init();
    lem.argv0 = args[0];
    lem.filename = OptArg(0);
    lem.basisflag = Main_basisflag;
    lem.nolinenosflag = Main_nolinenosflag;
    Symbol_new("$");
    lem.errsym = Symbol_new("error");
    lem.errsym.useCnt = 0;

    /* Parse the input file */
    Parse(lem);
    if (lem.errorcnt != 0)
    {
        Environment.Exit(lem.errorcnt);
    }
    if (lem.nrule == 0)
    {
        Console.Error.Write("Empty grammar.\n");
        Environment.Exit(1);
    }

    /* Count and index the symbols of the grammar */
    Symbol_new("{default}");
    lem.nsymbol = Symbol_count();
    lem.symbols = Symbol_arrayof();
    for (i = 0; i < lem.nsymbol; i++)
    {
        lem.symbols[i].index = i;
    }
    qsort(lem.symbols, lem.nsymbol, sizeof(symbol), Symbolcmpp);
    for (i = 0; i < lem.nsymbol; i++)
    {
        lem.symbols[i].index = i;
    }
    while (lem.symbols[i - 1].type == symbol_type.MULTITERMINAL)
    {
        i--;
    }
    Debug.Assert(string.Compare(lem.symbols[i - 1].name, "{default}") == 0);
    lem.nsymbol = i - 1;
    for (i = 1; char.IsUpper((byte)(lem.symbols[i].name[0])); i++)
    {
        ;
    }
    lem.nterminal = i;

    /* Assign sequential rule numbers.  Start with 0.  Put rules that have no
    ** reduce action C-code associated with them last, so that the switch()
    ** statement that selects reduction actions will have a smaller jump table.
    */
    for (i = 0, rp = lem.rule; rp != null; rp = rp.next)
    {
        rp.iRule = rp.code != 0 ? i++ : -1;
    }
    for (rp = lem.rule; rp != null; rp = rp.next)
    {
        if (rp.iRule < 0)
        {
            rp.iRule = i++;
        }
    }
    lem.startRule = lem.rule;
    lem.rule = Rule_sort(lem.rule);

    /* Generate a reprint of the grammar, if requested on the command line */
    if (Main_rpflag)
    {
        Reprint(lem);
    }
    else
    {
        /* Initialize the size for all follow and first sets */
        SetSize(lem.nterminal + 1);

        /* Find the precedence for every production rule (that has one) */
        FindRulePrecedences(lem);

        /* Compute the lambda-nonterminals and the first-sets for every
        ** nonterminal */
        FindFirstSets(lem);

        /* Compute all LR(0) states.  Also record follow-set propagation
        ** links so that the follow-set can be computed later */
        lem.nstate = 0;
        FindStates(lem);
        lem.sorted = State_arrayof();

        /* Tie up loose ends on the propagation links */
        FindLinks(lem);

        /* Compute the follow set of every reducible configuration */
        FindFollowSets(lem);

        /* Compute the action tables */
        FindActions(lem);

        /* Compress the action tables */
        if (Main_compress == 0)
        {
            CompressTables(lem);
        }

        /* Reorder and renumber the states so that states with fewer choices
        ** occur at the end.  This is an optimization that helps make the
        ** generated parser tables smaller. */
        if (Main_noResort == 0)
        {
            ResortStates(lem);
        }

        /* Generate a report of the parser generated.  (the "y.output" file) */
        if (Main_quiet == 0)
        {
            ReportOutput(lem);
        }

        /* Generate the source code for the parser */
        ReportTable(lem, Main_mhflag);

        /* Produce a header file for use by the scanner.  (This step is
        ** omitted if the "-m" option is used because makeheaders will
        ** generate the file for us.) */
        if (Main_mhflag == 0)
        {
            ReportHeader(lem);
        }
    }
    if (Main_statistics)
    {
        Console.Write("Parser statistics:\n");
        stats_line("terminal symbols", lem.nterminal);
        stats_line("non-terminal symbols", lem.nsymbol - lem.nterminal);
        stats_line("total symbols", lem.nsymbol);
        stats_line("rules", lem.nrule);
        stats_line("states", lem.nxstate);
        stats_line("conflicts", lem.nconflict);
        stats_line("action table entries", lem.nactiontab);
        stats_line("total table size (bytes)", lem.tablesize);
    }
    if (lem.nconflict > 0)
    {
        Console.Error.Write("{0:D} parsing conflicts.\n", lem.nconflict);
    }

    /* return 0 on success, 1 on failure. */
    exitcode = ((lem.errorcnt > 0) || (lem.nconflict > 0)) ? 1 : 0;
    Environment.Exit(exitcode);
    return (exitcode);
}
private delegate int cmpDelegate(string NamelessParameter1, string NamelessParameter2);
/******************** From the file "msort.c" *******************************/
/*
** A generic merge-sort program.
**
** USAGE:
** Let "ptr" be a pointer to some structure which is at the head of
** a null-terminated list.  Then to sort the list call:
**
**     ptr = msort(ptr,&(ptr->next),cmpfnc);
**
** In the above, "cmpfnc" is a pointer to a function which compares
** two instances of the structure and returns an integer, as in
** strcmp.  The second argument is a pointer to the pointer to the
** second element of the linked list.  This address is used to compute
** the offset to the "next" field within the structure.  The offset to
** the "next" field must be constant for all structures in the list.
**
** The function returns a new pointer which is the head of the list
** after sorting.
**
** ALGORITHM:
** Merge-sort.
*/

/*
** Return a pointer to the next structure in the linked list.
*/
//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define NEXT(A) (*(char**)(((char*)A)+offset))

/*
** Inputs:
**   a:       A sorted, null-terminated linked list.  (May be null).
**   b:       A sorted, null-terminated linked list.  (May be null).
**   cmp:     A pointer to the comparison function.
**   offset:  Offset in the structure to the "next" field.
**
** Return Value:
**   A pointer to the head of a sorted list containing the elements
**   of both a and b.
**
** Side effects:
**   The "next" pointers for elements in the lists a and b are
**   changed.
*/
internal static string merge(ref string a, ref string b, cmpDelegate cmp, int offset)
{
    string ptr;
    string head;

    if (a == 0)
    {
        head = b;
    }
    else if (b == 0)
    {
        head = a;
    }
    else
    {
        if (cmp(a, b) <= 0)
        {
            ptr = a;
            a = ((string)(((string)a) + offset));
        }
        else
        {
            ptr = b;
            b = ((string)(((string)b) + offset));
        }
        head = ptr;
        while (a != 0 && b != 0)
        {
            if (cmp(a, b) <= 0)
            {
                ((string)(((string)ptr) + offset)) = a;
                ptr = a;
                a = ((string)(((string)a) + offset));
            }
            else
            {
                ((string)(((string)ptr) + offset)) = b;
                ptr = b;
                b = ((string)(((string)b) + offset));
            }
        }
        if (a != 0)
        {
            ((string)(((string)ptr) + offset)) = a;
        }
        else
        {
            ((string)(((string)ptr) + offset)) = b;
        }
    }
    return head;
}

/*
** Inputs:
**   list:      Pointer to a singly-linked list of structures.
**   next:      Pointer to pointer to the second element of the list.
**   cmp:       A comparison function.
**
** Return Value:
**   A pointer to the head of a sorted list containing the elements
**   orginally in list.
**
** Side effects:
**   The "next" pointers for elements in list are changed.
*/
/************************ From the file "option.c" **************************/
internal static string[] argv;
internal static s_options[] op;
internal static FILE errstream;

//C++ TO C# CONVERTER NOTE: The following #define macro was replaced in-line:
//ORIGINAL LINE: #define ISOPT(X) ((X)[0]=='-'||(X)[0]=='+'||strchr((X),'=')!=0)

/*
** Print the command line with a carrot pointing to the k-th character
** of the n-th field.
*/
internal static void errline(int n, int k, FILE err)
{
    int spcnt;
    int i;
    if (argv[0] != 0)
    {
        fprintf(err, "%s", argv[0]);
    }
    spcnt = ((int)Convert.ToString(argv[0]).Length) + 1;
    for (i = 1; i < n && argv[i] != 0; i++)
    {
        fprintf(err, " %s", argv[i]);
        spcnt += ((int)Convert.ToString(argv[i]).Length) + 1;
    }
    spcnt += k;
    for (; argv[i] != 0; i++)
    {
        fprintf(err, " %s", argv[i]);
    }
    if (spcnt < 20)
    {
        fprintf(err, "\n%*s^-- here\n", spcnt, "");
    }
    else
    {
        fprintf(err, "\n%*shere --^\n", spcnt - 7, "");
    }
}

/*
** Return the index of the N-th non-switch argument.  Return -1
** if N is out of range.
*/
internal static int argindex(int n)
{
    int i;
    int dashdash = 0;
    if (argv != 0 && argv != 0)
    {
        for (i = 1; argv[i] != 0; i++)
        {
            if (dashdash != 0 || !((argv[i])[0] == '-' || (argv[i])[0] == '+' || StringFunctions.StrChr((argv[i]), '=') != 0))
            {
                if (n == 0)
                {
                    return i;
                }
                n--;
            }
            if (string.Compare(argv[i], "--") == 0)
            {
                dashdash = 1;
            }
        }
    }
    return -1;
}

internal static string emsg = "Command line syntax error: ";

/*
** Process a flag command line argument.
*/
internal static int handleflags(int i, FILE err)
{
    int v;
    int errcnt = 0;
    int j;
    for (j = 0; op[j].label != 0; j++)
    {
        if (string.Compare(argv[i][1], 0, op[j].label, 0, ((int)Convert.ToString(op[j].label).Length)) == 0)
        {
            break;
        }
    }
    v = argv[i][0] == '-' ? 1 : 0;
    if (op[j].label == 0)
    {
        if (err != null)
        {
            fprintf(err, "%sundefined option.\n", emsg);
            errline(i, 1, err);
        }
        errcnt++;
    }
    else if (op[j].arg == 0)
    {
        /* Ignore this option */
    }
    else if (op[j].type == option_type.OPT_FLAG)
    {
        (int)op[j].arg = v;
    }
    else if (op[j].type == option_type.OPT_FFLAG)
    {
        (void(*)(int))(op[j].arg)(v);
    }
    else if (op[j].type == option_type.OPT_FSTR)
    {
        (void(*)(string))(op[j].arg)(argv[i][2]);
    }
    else
    {
        if (err != null)
        {
            fprintf(err, "%smissing argument on switch.\n", emsg);
            errline(i, 1, err);
        }
        errcnt++;
    }
    return errcnt;
}

/*
** Process a command line switch which has an argument.
*/
internal static int handleswitch(int i, FILE err)
{
    int lv = 0;
    double dv = 0.0;
    string sv = 0;
    string end;
    string cp;
    int j;
    int errcnt = 0;
    cp = StringFunctions.StrChr(argv[i], '=');
    Debug.Assert(cp != 0);
    cp = 0;
    for (j = 0; op[j].label != 0; j++)
    {
        if (string.Compare(argv[i], op[j].label) == 0)
        {
            break;
        }
    }
    cp = (sbyte)'=';
    if (op[j].label == 0)
    {
        if (err != null)
        {
            fprintf(err, "%sundefined option.\n", emsg);
            errline(i, 0, err);
        }
        errcnt++;
    }
    else
    {
        cp = cp.Substring(1);
        switch (op[j].type)
        {
            case option_type.OPT_FLAG:
            case option_type.OPT_FFLAG:
                if (err != null)
                {
                    fprintf(err, "%soption requires an argument.\n", emsg);
                    errline(i, 0, err);
                }
                errcnt++;
                break;
            case option_type.OPT_DBL:
            case option_type.OPT_FDBL:
                dv = strtod(cp, end);
                if (end != 0)
                {
                    if (err != null)
                    {
                        fprintf(err, "%sillegal character in floating-point argument.\n", emsg);
                        errline(i, (int)((string)end - (string)argv[i]), err);
                    }
                    errcnt++;
                }
                break;
            case option_type.OPT_INT:
            case option_type.OPT_FINT:
                lv = strtol(cp, end, 0);
                if (end != 0)
                {
                    if (err != null)
                    {
                        fprintf(err, "%sillegal character in integer argument.\n", emsg);
                        errline(i, (int)((string)end - (string)argv[i]), err);
                    }
                    errcnt++;
                }
                break;
            case option_type.OPT_STR:
            case option_type.OPT_FSTR:
                sv = cp;
                break;
        }
        switch (op[j].type)
        {
            case option_type.OPT_FLAG:
            case option_type.OPT_FFLAG:
                break;
            case option_type.OPT_DBL:
                (double)(op[j].arg) = dv;
                break;
            case option_type.OPT_FDBL:
                (void(*)(double))(op[j].arg)(dv);
                break;
            case option_type.OPT_INT:
                (int)(op[j].arg) = lv;
                break;
            case option_type.OPT_FINT:
                (void(*)(int))(op[j].arg)((int)lv);
                break;
            case option_type.OPT_STR:
                (string)(op[j].arg) = sv;
                break;
            case option_type.OPT_FSTR:
                (void(*)(string))(op[j].arg)(sv);
                break;
        }
    }
    return errcnt;
}

/* Parse a single token */
internal static void parseonetoken(pstate psp)
{
    string x;
    x = Strsafe(psp.tokenstart); // Save the token permanently
#if false
	//	printf("%s:%d: Token=[%s] state=%d\n", psp->filename, psp->tokenlineno,
	//		x, psp->state);
#endif
    switch (psp.state)
    {
        case e_state.INITIALIZE:
            psp.prevrule = 0;
            psp.preccounter = 0;
            psp.firstrule = psp.lastrule = 0;
            psp.gp.nrule = 0;
        /* Fall thru to next case */
        //C++ TO C# CONVERTER TODO TASK: C# does not allow fall-through from a non-empty 'case':
        case e_state.WAITING_FOR_DECL_OR_RULE:
            if (x[0] == '%')
            {
                psp.state = e_state.WAITING_FOR_DECL_KEYWORD;
            }
            else if (char.IsLower((byte)(x[0])))
            {
                psp.lhs = Symbol_new(x);
                psp.nrhs = 0;
                psp.lhsalias = 0;
                psp.state = e_state.WAITING_FOR_ARROW;
            }
            else if (x[0] == '{')
            {
                if (psp.prevrule == 0)
                {
                    ErrorMsg(psp.filename, psp.tokenlineno, "There is no prior rule upon which to attach the code fragment which begins on this line.");
                    psp.errorcnt++;
                }
                else if (psp.prevrule.code != 0)
                {
                    ErrorMsg(psp.filename, psp.tokenlineno, "Code fragment beginning on this line is not the first to follow the previous rule.");
                    psp.errorcnt++;
                }
                else
                {
                    psp.prevrule.line = psp.tokenlineno;
                    psp.prevrule.code = x[1];
                    psp.prevrule.noCode = 0;
                }
            }
            else if (x[0] == '[')
            {
                psp.state = e_state.PRECEDENCE_MARK_1;
            }
            else
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Token \"%s\" should be either \"%%\" or a nonterminal name.", x);
                psp.errorcnt++;
            }
            break;
        case e_state.PRECEDENCE_MARK_1:
            if (!char.IsUpper((byte)(x[0])))
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "The precedence symbol must be a terminal.");
                psp.errorcnt++;
            }
            else if (psp.prevrule == 0)
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "There is no prior rule to assign precedence \"[%s]\".", x);
                psp.errorcnt++;
            }
            else if (psp.prevrule.precsym != 0)
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Precedence mark on this line is not the first to follow the previous rule.");
                psp.errorcnt++;
            }
            else
            {
                psp.prevrule.precsym = Symbol_new(x);
            }
            psp.state = e_state.PRECEDENCE_MARK_2;
            break;
        case e_state.PRECEDENCE_MARK_2:
            if (x[0] != ']')
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Missing \"]\" on precedence mark.");
                psp.errorcnt++;
            }
            psp.state = e_state.WAITING_FOR_DECL_OR_RULE;
            break;
        case e_state.WAITING_FOR_ARROW:
            if (x[0] == ':' && x[1] == ':' && x[2] == '=')
            {
                psp.state = e_state.IN_RHS;
            }
            else if (x[0] == '(')
            {
                psp.state = e_state.LHS_ALIAS_1;
            }
            else
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Expected to see a \":\" following the LHS symbol \"%s\".", psp.lhs.name);
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_RULE_ERROR;
            }
            break;
        case e_state.LHS_ALIAS_1:
            if (char.IsLetter((byte)(x[0])))
            {
                psp.lhsalias = x;
                psp.state = e_state.LHS_ALIAS_2;
            }
            else
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "\"%s\" is not a valid alias for the LHS \"%s\"\n", x, psp.lhs.name);
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_RULE_ERROR;
            }
            break;
        case e_state.LHS_ALIAS_2:
            if (x[0] == ')')
            {
                psp.state = e_state.LHS_ALIAS_3;
            }
            else
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Missing \")\" following LHS alias name \"%s\".", psp.lhsalias);
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_RULE_ERROR;
            }
            break;
        case e_state.LHS_ALIAS_3:
            if (x[0] == ':' && x[1] == ':' && x[2] == '=')
            {
                psp.state = e_state.IN_RHS;
            }
            else
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Missing \"->\" following: \"%s(%s)\".", psp.lhs.name, psp.lhsalias);
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_RULE_ERROR;
            }
            break;
			
			//以上已将产生式左边处理完毕
			//以下开始处理产生式右边内容
        case e_state.IN_RHS:
            if (x[0] == '.')
            {//一条产生式处理完毕
                rule rp = new rule();
                //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
                rp = (rule)calloc(sizeof(rule) + sizeof(symbol) * psp.nrhs + sizeof(string) * psp.nrhs, 1);
                if (rp == 0)
                {
                    ErrorMsg(psp.filename, psp.tokenlineno, "Can't allocate enough memory for this rule.");
                    psp.errorcnt++;
                    psp.prevrule = 0;
                }
                else
                {
                    int i;
                    rp.ruleline = psp.tokenlineno;
                    rp.rhs = (symbol)rp[1];
                    rp.rhsalias = (string)&(rp.rhs[psp.nrhs]);
                    for (i = 0; i < psp.nrhs; i++)
                    {
                        rp.rhs[i] = psp.rhs[i];
                        rp.rhsalias[i] = psp.alias[i];
                    }
                    rp.lhs = psp.lhs;
                    rp.lhsalias = psp.lhsalias;
                    rp.nrhs = psp.nrhs;
                    rp.code = 0;
                    rp.noCode = 1;
                    rp.precsym = 0;
                    rp.index = psp.gp.nrule++;
                    rp.nextlhs = rp.lhs.rule;
                    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
                    //ORIGINAL LINE: rp->lhs->rule = rp;
                    rp.lhs.rule = rp;
                    rp.next = 0;
                    if (psp.firstrule == 0)
                    {
                        psp.firstrule = psp.lastrule = rp;
                    }
                    else
                    {
                        //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
                        //ORIGINAL LINE: psp->lastrule->next = rp;
                        psp.lastrule.next = rp;
                        //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
                        //ORIGINAL LINE: psp->lastrule = rp;
                        psp.lastrule = rp;
                    }
                    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
                    //ORIGINAL LINE: psp->prevrule = rp;
                    psp.prevrule = rp;
                }
                psp.state = e_state.WAITING_FOR_DECL_OR_RULE;
            }
            else if (char.IsLetter((byte)(x[0])))
            {
                if (psp.nrhs >= DefineConstants.MAXRHS)
                {
                    ErrorMsg(psp.filename, psp.tokenlineno, "Too many symbols on RHS of rule beginning at \"%s\".", x);
                    psp.errorcnt++;
                    psp.state = e_state.RESYNC_AFTER_RULE_ERROR;
                }
                else
                {
                    psp.rhs[psp.nrhs] = Symbol_new(x);
                    psp.alias[psp.nrhs] = 0;
                    psp.nrhs++;
                }
            }
            else if ((x[0] == '|' || x[0] == '/') && psp.nrhs > 0)
            {
                symbol[] msp = psp.rhs[psp.nrhs - 1];
                if (msp.type != symbol_type.MULTITERMINAL)
                {
                    //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                    //ORIGINAL LINE: struct symbol *origsp = msp;
                    symbol[] origsp = new symbol(msp);
                    msp = Arrays.InitializeWithDefaultInstances<symbol>(1);
                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memset' has no equivalent in C#:
                    memset(msp, 0, sizeof(symbol));
                    msp.type = symbol_type.MULTITERMINAL;
                    msp.nsubsym = 1;
                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
                    msp.subsym = (symbol)calloc(1, sizeof(symbol));
                    msp.subsym[0] = origsp;
                    msp.name = origsp.name;
                    psp.rhs[psp.nrhs - 1] = msp;
                }
                msp.nsubsym++;
                //C++ TO C# CONVERTER TODO TASK: The memory management function 'realloc' has no equivalent in C#:
                msp.subsym = (symbol)realloc(msp.subsym, sizeof(symbol) * msp.nsubsym);
                msp.subsym[msp.nsubsym - 1] = Symbol_new(x[1]);
                if (char.IsLower((byte)(x[1])) || char.IsLower((byte)(msp.subsym[0].name[0])))
                {
                    ErrorMsg(psp.filename, psp.tokenlineno, "Cannot form a compound containing a non-terminal");
                    psp.errorcnt++;
                }
            }
            else if (x[0] == '(' && psp.nrhs > 0)
            {
                psp.state = e_state.RHS_ALIAS_1;
            }
            else
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Illegal character on RHS of rule: \"%s\".", x);
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_RULE_ERROR;
            }
            break;
        case e_state.RHS_ALIAS_1:
            if (char.IsLetter((byte)(x[0])))
            {
                psp.alias[psp.nrhs - 1] = x;
                psp.state = e_state.RHS_ALIAS_2;
            }
            else
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "\"%s\" is not a valid alias for the RHS symbol \"%s\"\n", x, psp.rhs[psp.nrhs - 1].name);
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_RULE_ERROR;
            }
            break;
        case e_state.RHS_ALIAS_2:
            if (x[0] == ')')
            {
                psp.state = e_state.IN_RHS;
            }
            else
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Missing \")\" following LHS alias name \"%s\".", psp.lhsalias);
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_RULE_ERROR;
            }
            break;
        case e_state.WAITING_FOR_DECL_KEYWORD:
            if (char.IsLetter((byte)(x[0])))
            {
                psp.declkeyword = x;
                psp.declargslot = 0;
                psp.decllinenoslot = 0;
                psp.insertLineMacro = 1;
                psp.state = e_state.WAITING_FOR_DECL_ARG;
                if (string.Compare(x, "name") == 0)
                {
                    psp.declargslot = (psp.gp.name);
                    psp.insertLineMacro = 0;
                }
                else if (string.Compare(x, "include") == 0)
                {
                    psp.declargslot = (psp.gp.include);
                }
                else if (string.Compare(x, "code") == 0)
                {
                    psp.declargslot = (psp.gp.extracode);
                }
                else if (string.Compare(x, "token_destructor") == 0)
                {
                    psp.declargslot = psp.gp.tokendest;
                }
                else if (string.Compare(x, "default_destructor") == 0)
                {
                    psp.declargslot = psp.gp.vardest;
                }
                else if (string.Compare(x, "token_prefix") == 0)
                {
                    psp.declargslot = psp.gp.tokenprefix;
                    psp.insertLineMacro = 0;
                }
                else if (string.Compare(x, "syntax_error") == 0)
                {
                    psp.declargslot = (psp.gp.error);
                }
                else if (string.Compare(x, "parse_accept") == 0)
                {
                    psp.declargslot = (psp.gp.accept);
                }
                else if (string.Compare(x, "parse_failure") == 0)
                {
                    psp.declargslot = (psp.gp.failure);
                }
                else if (string.Compare(x, "stack_overflow") == 0)
                {
                    psp.declargslot = (psp.gp.overflow);
                }
                else if (string.Compare(x, "extra_argument") == 0)
                {
                    psp.declargslot = (psp.gp.arg);
                    psp.insertLineMacro = 0;
                }
                else if (string.Compare(x, "token_type") == 0)
                {
                    psp.declargslot = (psp.gp.tokentype);
                    psp.insertLineMacro = 0;
                }
                else if (string.Compare(x, "default_type") == 0)
                {
                    psp.declargslot = (psp.gp.vartype);
                    psp.insertLineMacro = 0;
                }
                else if (string.Compare(x, "stack_size") == 0)
                {
                    psp.declargslot = (psp.gp.stacksize);
                    psp.insertLineMacro = 0;
                }
                else if (string.Compare(x, "start_symbol") == 0)
                {
                    psp.declargslot = (psp.gp.start);
                    psp.insertLineMacro = 0;
                }
                else if (string.Compare(x, "left") == 0)
                {
                    psp.preccounter++;
                    psp.declassoc = e_assoc.LEFT;
                    psp.state = e_state.WAITING_FOR_PRECEDENCE_SYMBOL;
                }
                else if (string.Compare(x, "right") == 0)
                {
                    psp.preccounter++;
                    psp.declassoc = e_assoc.RIGHT;
                    psp.state = e_state.WAITING_FOR_PRECEDENCE_SYMBOL;
                }
                else if (string.Compare(x, "nonassoc") == 0)
                {
                    psp.preccounter++;
                    psp.declassoc = e_assoc.NONE;
                    psp.state = e_state.WAITING_FOR_PRECEDENCE_SYMBOL;
                }
                else if (string.Compare(x, "destructor") == 0)
                {
                    psp.state = e_state.WAITING_FOR_DESTRUCTOR_SYMBOL;
                }
                else if (string.Compare(x, "type") == 0)
                {
                    psp.state = e_state.WAITING_FOR_DATATYPE_SYMBOL;
                }
                else if (string.Compare(x, "fallback") == 0)
                {
                    psp.fallback = 0;
                    psp.state = e_state.WAITING_FOR_FALLBACK_ID;
                }
                else if (string.Compare(x, "wildcard") == 0)
                {
                    psp.state = e_state.WAITING_FOR_WILDCARD_ID;
                }
                else if (string.Compare(x, "token_class") == 0)
                {
                    psp.state = e_state.WAITING_FOR_CLASS_ID;
                }
                else
                {
                    ErrorMsg(psp.filename, psp.tokenlineno, "Unknown declaration keyword: \"%%%s\".", x);
                    psp.errorcnt++;
                    psp.state = e_state.RESYNC_AFTER_DECL_ERROR;
                }
            }
            else
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Illegal declaration keyword: \"%s\".", x);
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_DECL_ERROR;
            }
            break;
        case e_state.WAITING_FOR_DESTRUCTOR_SYMBOL:
            if (!char.IsLetter((byte)(x[0])))
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Symbol name missing after %%destructor keyword");
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_DECL_ERROR;
            }
            else
            {
                symbol sp = Symbol_new(x);
                psp.declargslot = sp.destructor;
                psp.decllinenoslot = sp.destLineno;
                psp.insertLineMacro = 1;
                psp.state = e_state.WAITING_FOR_DECL_ARG;
            }
            break;
        case e_state.WAITING_FOR_DATATYPE_SYMBOL:
            if (!char.IsLetter((byte)(x[0])))
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Symbol name missing after %%type keyword");
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_DECL_ERROR;
            }
            else
            {
                symbol sp = Symbol_find(x);
                if ((sp) != null && (sp.datatype) != 0)
                {
                    ErrorMsg(psp.filename, psp.tokenlineno, "Symbol %%type \"%s\" already defined", x);
                    psp.errorcnt++;
                    psp.state = e_state.RESYNC_AFTER_DECL_ERROR;
                }
                else
                {
                    if (sp == null)
                    {
                        sp = Symbol_new(x);
                    }
                    psp.declargslot = sp.datatype;
                    psp.insertLineMacro = 0;
                    psp.state = e_state.WAITING_FOR_DECL_ARG;
                }
            }
            break;
        case e_state.WAITING_FOR_PRECEDENCE_SYMBOL:
            if (x[0] == '.')
            {
                psp.state = e_state.WAITING_FOR_DECL_OR_RULE;
            }
            else if (char.IsUpper((byte)(x[0])))
            {
                symbol sp;
                sp = Symbol_new(x);
                if (sp.prec >= 0)
                {
                    ErrorMsg(psp.filename, psp.tokenlineno, "Symbol \"%s\" has already be given a precedence.", x);
                    psp.errorcnt++;
                }
                else
                {
                    sp.prec = psp.preccounter;
                    sp.assoc = psp.declassoc;
                }
            }
            else
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Can't assign a precedence to \"%s\".", x);
                psp.errorcnt++;
            }
            break;
        case e_state.WAITING_FOR_DECL_ARG:
            if (x[0] == '{' || x[0] == '\"' || char.IsLetterOrDigit((byte)(x[0])))
            {
                string zOld;
                string zNew;
                string zBuf;
                string z;
                int nOld;
                int n;
                int nLine = 0;
                int nNew;
                int nBack;
                int addLineMacro;
                string zLine = new string(new char[50]);
                zNew = x;
                if (zNew[0] == '"' || zNew[0] == '{')
                {
                    zNew = zNew.Substring(1);
                }
                nNew = ((int)zNew.Length);
                if (psp.declargslot != null)
                {
                    zOld = psp.declargslot;
                }
                else
                {
                    zOld = "";
                }
                nOld = ((int)zOld.Length);
                n = nOld + nNew + 20;
                addLineMacro = psp.gp.nolinenosflag == 0 && psp.insertLineMacro != 0 && (psp.decllinenoslot == 0 || psp.decllinenoslot[0] != 0);
                if (addLineMacro != 0)
                {
                    for (z = psp.filename, nBack = 0; z != 0; z++)
                    {
                        if (z == '\\')
                        {
                            nBack++;
                        }
                    }
                    lemon_sprintf(ref zLine, "#line %d ", psp.tokenlineno);
                    nLine = ((int)zLine.Length);
                    n += nLine + ((int)psp.filename.Length) + nBack;
                }
                //C++ TO C# CONVERTER TODO TASK: The memory management function 'realloc' has no equivalent in C#:
                psp.declargslot = (string)realloc(psp.declargslot, n);
                zBuf = psp.declargslot + nOld;
                if (addLineMacro != 0)
                {
                    if (nOld != 0 && zBuf[-1] != '\n')
                    {
                        *(zBuf++) = '\n';
                    }
                    //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
                    memcpy(zBuf, zLine, nLine);
                    zBuf += nLine;
                    *(zBuf++) = '"';
                    for (z = psp.filename; z != 0; z++)
                    {
                        if (z == '\\')
                        {
                            *(zBuf++) = '\\';
                        }
                        *(zBuf++) = z;
                    }
                    *(zBuf++) = '"';
                    *(zBuf++) = '\n';
                }
                if (psp.decllinenoslot && psp.decllinenoslot[0] == 0)
                {
                    psp.decllinenoslot[0] = psp.tokenlineno;
                }
                //C++ TO C# CONVERTER TODO TASK: The memory management function 'memcpy' has no equivalent in C#:
                memcpy(zBuf, zNew, nNew);
                zBuf += nNew;
                zBuf = 0;
                psp.state = e_state.WAITING_FOR_DECL_OR_RULE;
            }
            else
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Illegal argument to %%%s: %s", psp.declkeyword, x);
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_DECL_ERROR;
            }
            break;
        case e_state.WAITING_FOR_FALLBACK_ID:
            if (x[0] == '.')
            {
                psp.state = e_state.WAITING_FOR_DECL_OR_RULE;
            }
            else if (!char.IsUpper((byte)(x[0])))
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "%%fallback argument \"%s\" should be a token", x);
                psp.errorcnt++;
            }
            else
            {
                symbol sp = Symbol_new(x);
                if (psp.fallback == 0)
                {
                    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
                    //ORIGINAL LINE: psp->fallback = sp;
                    psp.fallback = sp;
                }
                else if (sp.fallback)
                {
                    ErrorMsg(psp.filename, psp.tokenlineno, "More than one fallback assigned to token %s", x);
                    psp.errorcnt++;
                }
                else
                {
                    sp.fallback = psp.fallback;
                    psp.gp.has_fallback = 1;
                }
            }
            break;
        case e_state.WAITING_FOR_WILDCARD_ID:
            if (x[0] == '.')
            {
                psp.state = e_state.WAITING_FOR_DECL_OR_RULE;
            }
            else if (!char.IsUpper((byte)(x[0])))
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "%%wildcard argument \"%s\" should be a token", x);
                psp.errorcnt++;
            }
            else
            {
                symbol sp = Symbol_new(x);
                if (psp.gp.wildcard == 0)
                {
                    //C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to variables (in C#, the variable no longer points to the original when the original variable is re-assigned):
                    //ORIGINAL LINE: psp->gp->wildcard = sp;
                    psp.gp.wildcard = sp;
                }
                else
                {
                    ErrorMsg(psp.filename, psp.tokenlineno, "Extra wildcard to token: %s", x);
                    psp.errorcnt++;
                }
            }
            break;
        case e_state.WAITING_FOR_CLASS_ID:
            if (!char.IsLower((byte)(x[0])))
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "%%token_class must be followed by an identifier: ", x);
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_DECL_ERROR;
            }
            else if (Symbol_find(x))
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "Symbol \"%s\" already used", x);
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_DECL_ERROR;
            }
            else
            {
                psp.tkclass = Symbol_new(x);
                psp.tkclass.type = symbol_type.MULTITERMINAL;
                psp.state = e_state.WAITING_FOR_CLASS_TOKEN;
            }
            break;
        case e_state.WAITING_FOR_CLASS_TOKEN:
            if (x[0] == '.')
            {
                psp.state = e_state.WAITING_FOR_DECL_OR_RULE;
            }
            else if (char.IsUpper((byte)(x[0])) || ((x[0] == '|' || x[0] == '/') && char.IsUpper((byte)(x[1]))))
            {
                symbol msp = psp.tkclass;
                msp.nsubsym++;
                //C++ TO C# CONVERTER TODO TASK: The memory management function 'realloc' has no equivalent in C#:
                msp.subsym = (symbol)realloc(msp.subsym, sizeof(symbol) * msp.nsubsym);
                if (!char.IsUpper((byte)(x[0])))
                {
                    x = x.Substring(1);
                }
                msp.subsym[msp.nsubsym - 1] = Symbol_new(x);
            }
            else
            {
                ErrorMsg(psp.filename, psp.tokenlineno, "%%token_class argument \"%s\" should be a token", x);
                psp.errorcnt++;
                psp.state = e_state.RESYNC_AFTER_DECL_ERROR;
            }
            break;
        case e_state.RESYNC_AFTER_RULE_ERROR:
        /*      if( x[0]=='.' ) psp->state = WAITING_FOR_DECL_OR_RULE;
        **      break; */
        case e_state.RESYNC_AFTER_DECL_ERROR:
            if (x[0] == '.')
            {
                psp.state = e_state.WAITING_FOR_DECL_OR_RULE;
            }
            if (x[0] == '%')
            {
                psp.state = e_state.WAITING_FOR_DECL_KEYWORD;
            }
            break;
    }
}

/* Run the preprocessor over the input file text.  The global variables
** azDefine[0] through azDefine[nDefine-1] contains the names of all defined
** macros.  This routine looks for "%ifdef" and "%ifndef" and "%endif" and
** comments them out.  Text in between is also commented out as appropriate.
*/
internal static void preprocess_input(ref string z)
{
    int i;
    int j;
    int k;
    int n;
    int exclude = 0;
    int start = 0;
    int lineno = 1;
    int start_lineno = 1;
    for (i = 0; z[i]; i++)
    {
        if (z[i] == '\n')
        {
            lineno++;
        }
        if (z[i] != '%' || (i > 0 && z[i - 1] != '\n'))
        {
            continue;
        }
        if (string.Compare(z[i], 0, "%endif", 0, 6) == 0 && char.IsWhiteSpace((byte)(z[i + 6])))
        {
            if (exclude != 0)
            {
                exclude--;
                if (exclude == 0)
                {
                    for (j = start; j < i; j++)
                    {
                        if (z[j] != '\n')
                        {
                            z[j] = ' ';
                        }
                    }
                }
            }
            for (j = i; z[j] && z[j] != '\n'; j++)
            {
                z[j] = ' ';
            }
        }
        else if ((string.Compare(z[i], 0, "%ifdef", 0, 6) == 0 && char.IsWhiteSpace((byte)(z[i + 6]))) || (string.Compare(z[i], 0, "%ifndef", 0, 7) == 0 && char.IsWhiteSpace((byte)(z[i + 7]))))
        {
            if (exclude != 0)
            {
                exclude++;
            }
            else
            {
                for (j = i + 7; char.IsWhiteSpace((byte)(z[j])); j++)
                {
                }
                for (n = 0; z[j + n] && !char.IsWhiteSpace((byte)(z[j + n])); n++)
                {
                }
                exclude = 1;
                for (k = 0; k < nDefine; k++)
                {
                    if (string.Compare(azDefine[k], 0, z[j], 0, n) == 0 && ((int)Convert.ToString(azDefine[k]).Length) == n)
                    {
                        exclude = 0;
                        break;
                    }
                }
                if (z[i + 3] == 'n')
                {
                    exclude = exclude == 0;
                }
                if (exclude != 0)
                {
                    start = i;
                    start_lineno = lineno;
                }
            }
            for (j = i; z[j] && z[j] != '\n'; j++)
            {
                z[j] = ' ';
            }
        }
    }
    if (exclude != 0)
    {
        Console.Error.Write("unterminated %ifdef starting on line {0:D}\n", start_lineno);
        Environment.Exit(1);
    }
}
/*************************** From the file "plink.c" *********************/
/*
** Routines processing configuration follow-set propagation links
** in the LEMON parser generator.
*/
internal static plink plink_freelist = 0;
/*********************** From the file "report.c" **************************/
/*
** Procedures for generating reports and tables in the LEMON parser generator.
*/

/* Generate a filename with the given suffix.  Space to hold the
** name comes from malloc() and must be freed by the calling
** function.
*/
public static string file_makename(lemon lemp, string suffix)
{
    string name;
    string cp;

    //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
    name = (string)malloc(((int)lemp.filename.Length) + ((int)suffix.Length) + 5);
    if (name == 0)
    {
        Console.Error.Write("Can't allocate space for a filename.\n");
        Environment.Exit(1);
    }
    lemon_strcpy(name, lemp.filename);
    cp = StringFunctions.StrRChr(name, '.');
    if (cp != 0)
    {
        cp = 0;
    }
    lemon_strcat(name, suffix);
    return name;
}

/* Open a file with a name based on the name of the input file,
** but with a different (specified) suffix, and return a pointer
** to the stream */
public static FILE file_open(lemon lemp, string suffix, string mode)
{
    FILE fp;

    if (lemp.outname != 0)
    {
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
        free(lemp.outname);
    }
    lemp.outname = file_makename(lemp, suffix);
    fp = fopen(lemp.outname, mode);
    if (fp == 0 && mode == 'w')
    {
        Console.Error.Write("Can't open file \"{0}\".\n", lemp.outname);
        lemp.errorcnt++;
        return 0;
    }
    return fp;
}

/* Print a single rule.
*/
public static void RulePrint(FILE fp, rule rp, int iCursor)
{
    symbol sp;
    int i;
    int j;
    fprintf(fp, "%s ::=", rp.lhs.name);
    for (i = 0; i <= rp.nrhs; i++)
    {
        if (i == iCursor)
        {
            fprintf(fp, " *");
        }
        if (i == rp.nrhs)
        {
            break;
        }
        sp = rp.rhs[i];
        if (sp.type == symbol_type.MULTITERMINAL)
        {
            fprintf(fp, " %s", sp.subsym[0].name);
            for (j = 1; j < sp.nsubsym; j++)
            {
                fprintf(fp, "|%s", sp.subsym[j].name);
            }
        }
        else
        {
            fprintf(fp, " %s", sp.name);
        }
    }
}

/* Print the rule for a configuration.
*/
public static void ConfigPrint(FILE fp, config cfp)
{
    //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
    //ORIGINAL LINE: RulePrint(fp, cfp->rp, cfp->dot);
    RulePrint(fp, new rule(cfp.rp), cfp.dot);
}

/* #define TEST */
#if false
	// /* Print a set */
	//PRIVATE void SetPrint(out, set, lemp)
	//FILE *out;
	//char *set;
	//struct lemon *lemp;
	//{
	//	int i;
	//	char *spacer;
	//	spacer = "";
	//	fprintf(out, "%12s[", "");
	//	for (i = 0; i<lemp->nterminal; i++) {
	//		if (SetFind(set, i)) {
	//			fprintf(out, "%s%s", spacer, lemp->symbols[i]->name);
	//			spacer = " ";
	//		}
	//	}
	//	fprintf(out, "]\n");
	//}
	//
	// /* Print a plink chain */
	//PRIVATE void PlinkPrint(out, plp, tag)
	//FILE *out;
	//struct plink *plp;
	//char *tag;
	//{
	//	while (plp) {
	//		fprintf(out, "%12s%s (state %2d) ", "", tag, plp->cfp->stp->statenum);
	//		ConfigPrint(out, plp->cfp);
	//		fprintf(out, "\n");
	//		plp = plp->next;
	//	}
	//}
#endif

/* Print an action to the given file descriptor.  Return FALSE if
** nothing was actually printed.
*/
public static int PrintAction(action ap, FILE fp, int indent)
{ // Indent by this amount -  Print the action here -  The action to print
    int result = 1;
    switch (ap.type)
    {
        case e_action.SHIFT:
            {
                state stp = ap.x.stp;
                fprintf(fp, "%*s shift        %-7d", indent, ap.sp.name, stp.statenum);
                break;
            }
        case e_action.REDUCE:
            {
                //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                //ORIGINAL LINE: struct rule *rp = ap->x.rp;
                rule[] rp = new rule(ap.x.rp);
                fprintf(fp, "%*s reduce       %-7d", indent, ap.sp.name, rp.iRule);
                //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                //ORIGINAL LINE: RulePrint(fp, rp, -1);
                RulePrint(fp, new rule(rp), -1);
                break;
            }
        case e_action.SHIFTREDUCE:
            {
                //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                //ORIGINAL LINE: struct rule *rp = ap->x.rp;
                rule[] rp = new rule(ap.x.rp);
                fprintf(fp, "%*s shift-reduce %-7d", indent, ap.sp.name, rp.iRule);
                //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                //ORIGINAL LINE: RulePrint(fp, rp, -1);
                RulePrint(fp, new rule(rp), -1);
                break;
            }
        case e_action.ACCEPT:
            fprintf(fp, "%*s accept", indent, ap.sp.name);
            break;
        case e_action.ERROR:
            fprintf(fp, "%*s error", indent, ap.sp.name);
            break;
        case e_action.SRCONFLICT:
        case e_action.RRCONFLICT:
            fprintf(fp, "%*s reduce       %-7d ** Parsing conflict **", indent, ap.sp.name, ap.x.rp.iRule);
            break;
        case e_action.SSCONFLICT:
            fprintf(fp, "%*s shift        %-7d ** Parsing conflict **", indent, ap.sp.name, ap.x.stp.statenum);
            break;
        case e_action.SH_RESOLVED:
            if (showPrecedenceConflict != 0)
            {
                fprintf(fp, "%*s shift        %-7d -- dropped by precedence", indent, ap.sp.name, ap.x.stp.statenum);
            }
            else
            {
                result = 0;
            }
            break;
        case e_action.RD_RESOLVED:
            if (showPrecedenceConflict != 0)
            {
                fprintf(fp, "%*s reduce %-7d -- dropped by precedence", indent, ap.sp.name, ap.x.rp.iRule);
            }
            else
            {
                result = 0;
            }
            break;
        case e_action.NOT_USED:
            result = 0;
            break;
    }
    if (result != 0 && ap.spOpt != null)
    {
        fprintf(fp, "  /* because %s==%s */", ap.sp.name, ap.spOpt.name);
    }
    return result;
}

/* Search for the file "name" which is in the same directory as
** the exacutable */
public static string pathsearch(ref string argv0, ref string name, int modemask)
{
    string pathlist;
    string pathbufptr;
    string pathbuf;
    string path;
    string cp;
    sbyte c;

#if __WIN32__
		cp = StringFunctions.StrRChr(argv0, '\\');
#else
    cp = StringFunctions.StrRChr(argv0, '/');
#endif
    if (cp != 0)
    {
        c = cp;
        cp = 0;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
        path = (string)malloc(((int)argv0.Length) + ((int)name.Length) + 2);
        if (path != 0)
        {
            lemon_sprintf(path, "%s/%s", argv0, name);
        }
        cp = c;
    }
    else
    {
        pathlist = getenv("PATH");
        if (pathlist == 0)
        {
            pathlist = ".:/bin:/usr/bin";
        }
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
        pathbuf = (string)malloc(((int)pathlist.Length) + 1);
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
        path = (string)malloc(((int)pathlist.Length) + ((int)name.Length) + 2);
        if ((pathbuf != 0) && (path != 0))
        {
            pathbufptr = pathbuf;
            lemon_strcpy(pathbuf, pathlist);
            while (pathbuf != 0)
            {
                cp = StringFunctions.StrChr(pathbuf, ':');
                if (cp == 0)
                {
                    cp = pathbuf[((int)pathbuf.Length)];
                }
                c = cp;
                cp = 0;
                lemon_sprintf(path, "%s/%s", pathbuf, name);
                cp = c;
                if (c == 0)
                {
                    pathbuf = null;
                }
                else
                {
                    pathbuf = cp[1];
                }
                if (access(path, modemask) == 0)
                {
                    break;
                }
            }
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
            free(pathbufptr);
        }
    }
    return path;
}

/* Given an action, compute the integer value for that action
** which is to be put in the action table of the generated machine.
** Return negative if no action should be generated.
*/
public static int compute_action(lemon lemp, action ap)
{
    int act;
    switch (ap.type)
    {
        case e_action.SHIFT:
            act = ap.x.stp.statenum;
            break;
        case e_action.SHIFTREDUCE:
            act = ap.x.rp.iRule + lemp.nstate;
            break;
        case e_action.REDUCE:
            act = ap.x.rp.iRule + lemp.nstate + lemp.nrule;
            break;
        case e_action.ERROR:
            act = lemp.nstate + lemp.nrule * 2;
            break;
        case e_action.ACCEPT:
            act = lemp.nstate + lemp.nrule * 2 + 1;
            break;
        default:
            act = -1;
            break;
    }
    return act;
}

/* The next cluster of routines are for reading the template file
** and writing the results to the generated parser */
/* The first function transfers data from "in" to "out" until
** a line is seen which begins with "%%".  The line number is
** tracked.
**
** if name!=0, then any word that begin with "Parse" is changed to
** begin with *name instead.
*/
public static void tplt_xfer(ref string name, FILE in, FILE @out, ref int lineno)
{
    int i;
    int iStart;
    string line = new string(new char[DefineConstants.LINESIZE]);
    while (fgets(line, DefineConstants.LINESIZE, in) && (line[0] != '%' || line[1] != '%'))
    {
        lineno++;
        iStart = 0;
        if (name != 0)
        {
            for (i = 0; line[i]; i++)
            {
                if (line[i] == 'P' && string.Compare(line[i], 0, "Parse", 0, 5) == 0 && (i == 0 || !char.IsLetter((byte)(line[i - 1]))))
                {
                    if (i > iStart)
                    {
                        fprintf(@out, "%.*s", i - iStart, line[iStart]);
                    }
                    fprintf(@out, "%s", name);
                    i += 4;
                    iStart = i + 1;
                }
            }
        }
        fprintf(@out, "%s", line[iStart]);
    }
}
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static string tplt_open_templatename = "lempar.c";

/* The next function finds the template file and opens it, returning
** a pointer to the opened file. */
public static FILE tplt_open(lemon lemp)
{
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static sbyte templatename[] = "lempar.c";
    string buf = new string(new char[1000]);
    FILE in;
    string tpltname;
    string cp;

    /* first, see if user specified a template filename on the command line. */
    if (user_templatename != 0)
    {
        if (access(user_templatename, 0x4) == -1)
        {
            Console.Error.Write("Can't find the parser driver template file \"{0}\".\n", user_templatename);
            lemp.errorcnt++;
            return 0;
        }
			in = fopen(user_templatename, "rb");
        if (in == 0)
			{
            Console.Error.Write("Can't open the template file \"{0}\".\n", user_templatename);
            lemp.errorcnt++;
            return 0;
        }
        return in;
    }

    cp = StringFunctions.StrRChr(lemp.filename, '.');
    if (cp != 0)
    {
        lemon_sprintf(buf, "%.*s.lt", (int)(cp - lemp.filename), lemp.filename);
    }
    else
    {
        lemon_sprintf(ref buf, "%s.lt", lemp.filename);
    }
    if (access(buf, 0x4) == 0)
    {
        tpltname = buf;
    }
    else if (access(tplt_open_templatename, 0x4) == 0)
    {
        tpltname = tplt_open_templatename;
    }
    else
    {
        tpltname = pathsearch(ref lemp.argv0, ref tplt_open_templatename, 0);
    }
    if (tpltname == 0)
    {
        Console.Error.Write("Can't find the parser driver template file \"{0}\".\n", tplt_open_templatename);
        lemp.errorcnt++;
        return 0;
    }
		in = fopen(tpltname, "rb");
    if (in == 0)
		{
        Console.Error.Write("Can't open the template file \"{0}\".\n", tplt_open_templatename);
        lemp.errorcnt++;
        return 0;
    }
    return in;
}

/* Print a #line directive line to the output file. */
//C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on the parameter 'filename', so pointers on this parameter are left unchanged:
public static void tplt_linedir(FILE @out, int lineno, sbyte* filename)
{
    fprintf(@out, "#line %d \"", lineno);
    while (*filename != 0)
    {
        if (*filename == '\\')
        {
            putc('\\', @out);
        }
        putc(*filename, @out);
        filename = filename.Substring(1);
    }
    fprintf(@out, "\"\n");
}

/* Print a string to the file and keep the linenumber up to date */
//C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on the parameter 'str', so pointers on this parameter are left unchanged:
public static void tplt_print(FILE @out, lemon lemp, sbyte* str, ref int lineno)
{
    if (str == 0)
    {
        return;
    }
    while (*str != 0)
    {
        putc(*str, @out);
        if (*str == '\n')
        {
            lineno++;
        }
        str = str.Substring(1);
    }
    if (str[-1] != '\n')
    {
        putc('\n', @out);
        lineno++;
    }
    if (lemp.nolinenosflag == 0)
    {
        lineno++;
        tplt_linedir(@out, lineno, lemp.outname);
    }
    return;
}

/*
** The following routine emits code for the destructor for the
** symbol sp
*/
public static void emit_destructor_code(FILE @out, symbol sp, lemon lemp, ref int lineno)
{
    string cp = 0;

    if (sp.type == symbol_type.TERMINAL)
    {
        cp = lemp.tokendest;
        if (cp == 0)
        {
            return;
        }
        fprintf(@out, "{\n");
        lineno++;
    }
    else if (sp.destructor)
    {
        cp = sp.destructor;
        fprintf(@out, "{\n");
        lineno++;
        if (lemp.nolinenosflag == 0)
        {
            lineno++;
            tplt_linedir(@out, sp.destLineno, lemp.filename);
        }
    }
    else if (lemp.vardest)
    {
        cp = lemp.vardest;
        if (cp == 0)
        {
            return;
        }
        fprintf(@out, "{\n");
        lineno++;
    }
    else
    {
        Debug.Assert(0); // Cannot happen
    }
    for (; cp != 0; cp++)
    {
        if (cp == '$' && cp[1] == '$')
        {
            fprintf(@out, "(yypminor->yy%d)", sp.dtnum);
            cp = cp.Substring(1);
            continue;
        }
        if (cp == '\n')
        {
            lineno++;
        }
        fputc(cp, @out);
    }
    fprintf(@out, "\n");
    lineno++;
    if (lemp.nolinenosflag == 0)
    {
        lineno++;
        tplt_linedir(@out, lineno, lemp.outname);
    }
    fprintf(@out, "}\n");
    lineno++;
    return;
}

/*
** Return TRUE (non-zero) if the given symbol has a destructor.
*/
public static int has_destructor(symbol sp, lemon lemp)
{
    int ret;
    if (sp.type == symbol_type.TERMINAL)
    {
        ret = lemp.tokendest != 0;
    }
    else
    {
        ret = lemp.vardest != 0 || sp.destructor != 0;
    }
    return ret;
}
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static sbyte[] append_str_empty = { 0 };
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static string append_str_z = 0;
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static int append_str_alloced = 0;
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static int append_str_used = 0;

/*
** Append text to a dynamically allocated string.  If zText is 0 then
** reset the string to be empty again.  Always return the complete text
** of the string (which is overwritten with each call).
**
** n bytes of zText are stored.  If n==0 then all of zText up to the first
** \000 terminator is stored.  zText can contain up to two instances of
** %d.  The values of p1 and p2 are written into the first and second
** %d.
**
** If n==-1, then the previous character is overwritten.
*/
//C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on the parameter 'zText', so pointers on this parameter are left unchanged:
public static string append_str(sbyte* zText, int n, int p1, int p2)
{
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static sbyte empty[1] = { 0 };
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static sbyte *z = 0;
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static int alloced = 0;
    //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
    //	static int used = 0;
    int c;
    string zInt = new string(new char[40]);
    if (zText == 0)
    {
        if (append_str_used == 0 && append_str_z != 0)
        {
            append_str_z = null;
        }
        append_str_used = 0;
        return append_str_z;
    }
    if (n <= 0)
    {
        if (n < 0)
        {
            append_str_used += n;
            Debug.Assert(append_str_used >= 0);
        }
        n = ((int)zText.Length);
    }
    if ((int)(n + sizeof(sbyte) * 2 + append_str_used) >= append_str_alloced)
    {
        append_str_alloced = n + sizeof(sbyte) * 2 + append_str_used + 200;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'realloc' has no equivalent in C#:
        append_str_z = (string)realloc(append_str_z, append_str_alloced);
    }
    if (append_str_z == 0)
    {
        return append_str_empty;
    }
    while (n-- > 0)
    {
        c = (zText++);
        if (c == '%' && n > 0 && zText[0] == 'd')
        {
            lemon_sprintf(ref zInt, "%d", p1);
            p1 = p2;
            lemon_strcpy(append_str_z[append_str_used], zInt);
            append_str_used += ((int)Convert.ToString(append_str_z[append_str_used]).Length);
            zText = zText.Substring(1);
            n--;
        }
        else
        {
            append_str_z[append_str_used++] = (sbyte)c;
        }
    }
    append_str_z = append_str_z.Substring(0, append_str_used);
    return append_str_z;
}
//C++ TO C# CONVERTER NOTE: This was formerly a static local variable declaration (not allowed in C#):
private static string translate_code_newlinestr = "\n";

/*
** Write and transform the rp->code string so that symbols are expanded.
** Populate the rp->codePrefix and rp->codeSuffix strings, as appropriate.
**
** Return 1 if the expanded code requires that "yylhsminor" local variable
** to be defined.
*/
public static int translate_code(lemon lemp, rule rp)
{
    string cp;
    string xp;
    int i;
    int rc = 0; // True if yylhsminor is used
    int dontUseRhs0 = 0; // If true, use of left-most RHS label is illegal
    string zSkip = 0; // The zOvwrt comment within rp->code, or NULL
    sbyte lhsused = 0; // True if the LHS element has been used
    sbyte lhsdirect; // True if LHS writes directly into stack
    string used = new string(new char[DefineConstants.MAXRHS]); // True for each RHS element which is used
    string zLhs = new string(new char[50]); // Convert the LHS symbol into this string
    string zOvwrt = new string(new char[900]); // Comment that to allow LHS to overwrite RHS

    for (i = 0; i < rp.nrhs; i++)
    {
        used = used.Substring(0, i);
    }
    lhsused = 0;

    if (rp.code == 0)
    {
        //C++ TO C# CONVERTER NOTE: This static local variable declaration (not allowed in C#) has been moved just prior to the method:
        //		static sbyte newlinestr[2] = { '\n', '\0' };
        rp.code = translate_code_newlinestr;
        rp.line = rp.ruleline;
        rp.noCode = 1;
    }
    else
    {
        rp.noCode = 0;
    }


    if (rp.nrhs == 0)
    {
        /* If there are no RHS symbols, then writing directly to the LHS is ok */
        lhsdirect = 1;
    }
    else if (rp.rhsalias[0] == 0)
    {
        /* The left-most RHS symbol has no value.  LHS direct is ok.  But
        ** we have to call the distructor on the RHS symbol first. */
        lhsdirect = 1;
        //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
        //ORIGINAL LINE: if (has_destructor(rp->rhs[0], lemp))
        if (has_destructor(new symbol(rp.rhs[0]), lemp) != 0)
        {
            append_str(0, 0, 0, 0);
            append_str("  yy_destructor(yypParser,%d,&yymsp[%d].minor);\n", 0, rp.rhs[0].index, 1 - rp.nrhs);
            rp.codePrefix = Strsafe(append_str(0, 0, 0, 0));
            rp.noCode = 0;
        }
    }
    else if (rp.lhsalias == 0)
    {
        /* There is no LHS value symbol. */
        lhsdirect = 1;
    }
    else if (string.Compare(rp.lhsalias, rp.rhsalias[0]) == 0)
    {
        /* The LHS symbol and the left-most RHS symbol are the same, so
        ** direct writing is allowed */
        lhsdirect = 1;
        lhsused = 1;
        used = StringFunctions.ChangeCharacter(used, 0, 1);
        if (rp.lhs.dtnum != rp.rhs[0].dtnum)
        {
            ErrorMsg(lemp.filename, rp.ruleline, "%s(%s) and %s(%s) share the same label but have " + "different datatypes.", rp.lhs.name, rp.lhsalias, rp.rhs[0].name, rp.rhsalias[0]);
            lemp.errorcnt++;
        }
    }
    else
    {
        lemon_sprintf(zOvwrt, "/*%s-overwrites-%s*/", rp.lhsalias, rp.rhsalias[0]);
        zSkip = StringFunctions.StrStr(rp.code, zOvwrt);
        if (zSkip != 0)
        {
            /* The code contains a special comment that indicates that it is safe
            ** for the LHS label to overwrite left-most RHS label. */
            lhsdirect = 1;
        }
        else
        {
            lhsdirect = 0;
        }
    }
    if (lhsdirect != 0)
    {
        zLhs = string.Format("yymsp[{0:D}].minor.yy{1:D}", 1 - rp.nrhs, rp.lhs.dtnum);
    }
    else
    {
        rc = 1;
        zLhs = string.Format("yylhsminor.yy{0:D}", rp.lhs.dtnum);
    }

    append_str(0, 0, 0, 0);

    /* This const cast is wrong but harmless, if we're careful. */
    for (cp = (string)rp.code; cp != 0; cp++)
    {
        if (cp == zSkip)
        {
            append_str(zOvwrt, 0, 0, 0);
            cp += ((int)zOvwrt.Length) - 1;
            dontUseRhs0 = 1;
            continue;
        }
        if (char.IsLetter((byte)(cp)) && (cp == rp.code || (!char.IsLetterOrDigit((byte)(cp[-1])) && cp[-1] != '_')))
        {
            sbyte saved;
            for (xp = cp[1]; char.IsLetterOrDigit((byte)(xp)) || xp == '_'; xp++)
            {
                ;
            }
            saved = xp;
            xp = 0;
            if (rp.lhsalias != 0 && string.Compare(cp, rp.lhsalias) == 0)
            {
                append_str(zLhs, 0, 0, 0);
                cp = xp;
                lhsused = 1;
            }
            else
            {
                for (i = 0; i < rp.nrhs; i++)
                {
                    if (rp.rhsalias[i] != 0 && string.Compare(cp, rp.rhsalias[i]) == 0)
                    {
                        if (i == 0 && dontUseRhs0 != 0)
                        {
                            ErrorMsg(lemp.filename, rp.ruleline, "Label %s used after '%s'.", rp.rhsalias[0], zOvwrt);
                            lemp.errorcnt++;
                        }
                        else if (cp != rp.code && cp[-1] == '@')
                        {
                            /* If the argument is of the form @X then substituted
                            ** the token number of X, not the value of X */
                            append_str("yymsp[%d].major", -1, i - rp.nrhs + 1, 0);
                        }
                        else
                        {
                            symbol[] sp = rp.rhs[i];
                            int dtnum;
                            if (sp.type == symbol_type.MULTITERMINAL)
                            {
                                dtnum = sp.subsym[0].dtnum;
                            }
                            else
                            {
                                dtnum = sp.dtnum;
                            }
                            append_str("yymsp[%d].minor.yy%d", 0, i - rp.nrhs + 1, dtnum);
                        }
                        cp = xp;
                        used = StringFunctions.ChangeCharacter(used, i, 1);
                        break;
                    }
                }
            }
            xp = saved;
        }
        append_str(cp, 1, 0, 0);
    } // End loop

    /* Main code generation completed */
    cp = append_str(0, 0, 0, 0);
    if (cp != 0 && cp[0])
    {
        rp.code = Strsafe(cp);
    }
    append_str(0, 0, 0, 0);

    /* Check to make sure the LHS has been used */
    if (rp.lhsalias != 0 && lhsused == 0)
    {
        ErrorMsg(lemp.filename, rp.ruleline, "Label \"%s\" for \"%s(%s)\" is never used.", rp.lhsalias, rp.lhs.name, rp.lhsalias);
        lemp.errorcnt++;
    }

    /* Generate destructor code for RHS minor values which are not referenced.
    ** Generate error messages for unused labels and duplicate labels.
    */
    for (i = 0; i < rp.nrhs; i++)
    {
        if (rp.rhsalias[i] != 0)
        {
            if (i > 0)
            {
                int j;
                if (rp.lhsalias != 0 && string.Compare(rp.lhsalias, rp.rhsalias[i]) == 0)
                {
                    ErrorMsg(lemp.filename, rp.ruleline, "%s(%s) has the same label as the LHS but is not the left-most " + "symbol on the RHS.", rp.rhs[i].name, rp.rhsalias);
                    lemp.errorcnt++;
                }
                for (j = 0; j < i; j++)
                {
                    if (rp.rhsalias[j] != 0 && string.Compare(rp.rhsalias[j], rp.rhsalias[i]) == 0)
                    {
                        ErrorMsg(lemp.filename, rp.ruleline, "Label %s used for multiple symbols on the RHS of a rule.", rp.rhsalias[i]);
                        lemp.errorcnt++;
                        break;
                    }
                }
            }
            if (!used[i])
            {
                ErrorMsg(lemp.filename, rp.ruleline, "Label %s for \"%s(%s)\" is never used.", rp.rhsalias[i], rp.rhs[i].name, rp.rhsalias[i]);
                lemp.errorcnt++;
            }
        }
        //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
        //ORIGINAL LINE: else if (i>0 && has_destructor(rp->rhs[i], lemp))
        else if (i > 0 && has_destructor(new symbol(rp.rhs[i]), lemp) != 0)
        {
            append_str("  yy_destructor(yypParser,%d,&yymsp[%d].minor);\n", 0, rp.rhs[i].index, i - rp.nrhs + 1);
        }
    }

    /* If unable to write LHS values directly into the stack, write the
    ** saved LHS value now. */
    if (lhsdirect == 0)
    {
        append_str("  yymsp[%d].minor.yy%d = ", 0, 1 - rp.nrhs, rp.lhs.dtnum);
        append_str(zLhs, 0, 0, 0);
        append_str(";\n", 0, 0, 0);
    }

    /* Suffix code generation complete */
    cp = append_str(0, 0, 0, 0);
    if (cp != 0 && cp[0])
    {
        rp.codeSuffix = Strsafe(cp);
        rp.noCode = 0;
    }

    return rc;
}

/*
** Generate code which executes when the rule "rp" is reduced.  Write
** the code to "out".  Make sure lineno stays up-to-date.
*/
public static void emit_code(FILE @out, rule rp, lemon lemp, ref int lineno)
{
    //C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on this variable, so pointers on this variable are left unchanged:
    sbyte* cp;

    /* Setup code prior to the #line directive */
    if (rp.codePrefix != 0 && rp.codePrefix[0])
    {
        fprintf(@out, "{%s", rp.codePrefix);
        for (cp = rp.codePrefix; *cp; cp++)
        {
            if (*cp == '\n')
            {
                lineno++;
            }
        }
    }

    /* Generate code to do the reduce action */
    if (rp.code != 0)
    {
        if (lemp.nolinenosflag == 0)
        {
            lineno++;
            tplt_linedir(@out, rp.line, lemp.filename);
        }
        fprintf(@out, "{%s", rp.code);
        for (cp = rp.code; *cp; cp++)
        {
            if (*cp == '\n')
            {
                lineno++;
            }
        }
        fprintf(@out, "}\n");
        lineno++;
        if (lemp.nolinenosflag == 0)
        {
            lineno++;
            tplt_linedir(@out, lineno, lemp.outname);
        }
    }

    /* Generate breakdown code that occurs after the #line directive */
    if (rp.codeSuffix != 0 && rp.codeSuffix[0])
    {
        fprintf(@out, "%s", rp.codeSuffix);
        for (cp = rp.codeSuffix; *cp; cp++)
        {
            if (*cp == '\n')
            {
                lineno++;
            }
        }
    }

    if (rp.codePrefix != 0)
    {
        fprintf(@out, "}\n");
        lineno++;
    }

    return;
}

/*
** Print the definition of the union used for the parser's data stack.
** This union contains fields for every possible data type for tokens
** and nonterminals.  In the process of computing and printing this
** union, also set the ".dtnum" field of every terminal and nonterminal
** symbol.
*/
public static void print_stack_union(FILE @out, lemon lemp, ref int plineno, int mhflag)
{ // True if generating makeheaders output -  Pointer to the line number -  The main info structure for this parser -  The output stream
    int lineno = plineno; // The line number of the output
    string[] types; // A hash table of datatypes
    int arraysize; // Size of the "types" array
    int maxdtlength; // Maximum length of any ".datatype" field.
    string stddt; // Standardized name for a datatype
    int i; // Loop counters
    int j;
    uint hash; // For hashing the name of a type
    string name; // Name of the parser

    /* Allocate and initialize types[] and allocate stddt[] */
    arraysize = lemp.nsymbol * 2;
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'calloc' has no equivalent in C#:
    types = (string)calloc(arraysize, sizeof(string));
    if (types == 0)
    {
        Console.Error.Write("Out of memory.\n");
        Environment.Exit(1);
    }
    for (i = 0; i < arraysize; i++)
    {
        types[i] = 0;
    }
    maxdtlength = 0;
    if (lemp.vartype != 0)
    {
        maxdtlength = ((int)lemp.vartype.Length);
    }
    for (i = 0; i < lemp.nsymbol; i++)
    {
        int len;
        symbol[] sp = lemp.symbols[i];
        if (sp.datatype == 0)
        {
            continue;
        }
        len = ((int)sp.datatype.Length);
        if (len > maxdtlength)
        {
            maxdtlength = len;
        }
    }
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
    stddt = (string)malloc(maxdtlength * 2 + 1);
    if (stddt == 0)
    {
        Console.Error.Write("Out of memory.\n");
        Environment.Exit(1);
    }

    /* Build a hash table of datatypes. The ".dtnum" field of each symbol
    ** is filled in with the hash index plus 1.  A ".dtnum" value of 0 is
    ** used for terminal symbols.  If there is no %default_type defined then
    ** 0 is also used as the .dtnum value for nonterminals which do not specify
    ** a datatype using the %type directive.
    */
    for (i = 0; i < lemp.nsymbol; i++)
    {
        symbol[] sp = lemp.symbols[i];
        string cp;
        if (sp == lemp.errsym)
        {
            sp.dtnum = arraysize + 1;
            continue;
        }
        if (sp.type != symbol_type.NONTERMINAL || (sp.datatype == 0 && lemp.vartype == 0))
        {
            sp.dtnum = 0;
            continue;
        }
        cp = sp.datatype;
        if (cp == 0)
        {
            cp = lemp.vartype;
        }
        j = 0;
        while (char.IsWhiteSpace((byte)(cp)))
        {
            cp = cp.Substring(1);
        }
        while (cp != 0)
        {
            stddt[j++] = cp++;
        }
        while (j > 0 && char.IsWhiteSpace((byte)(stddt[j - 1])))
        {
            j--;
        }
        stddt = stddt.Substring(0, j);
        if (lemp.tokentype != 0 && string.Compare(stddt, lemp.tokentype) == 0)
        {
            sp.dtnum = 0;
            continue;
        }
        hash = 0;
        for (j = 0; stddt[j]; j++)
        {
            hash = hash * 53 + stddt[j];
        }
        hash = (hash & 0x7fffffff) % arraysize;
        while (types[hash] != 0)
        {
            if (string.Compare(types[hash], stddt) == 0)
            {
                sp.dtnum = hash + 1;
                break;
            }
            hash++;
            if (hash >= (uint)arraysize)
            {
                hash = 0;
            }
        }
        if (types[hash] == 0)
        {
            sp.dtnum = hash + 1;
            //C++ TO C# CONVERTER TODO TASK: The memory management function 'malloc' has no equivalent in C#:
            types[hash] = (string)malloc(((int)stddt.Length) + 1);
            if (types[hash] == 0)
            {
                Console.Error.Write("Out of memory.\n");
                Environment.Exit(1);
            }
            lemon_strcpy(types[hash], stddt);
        }
    }

    /* Print out the definition of YYTOKENTYPE and YYMINORTYPE */
    name = lemp.name != 0 ? lemp.name : "Parse";
    lineno = plineno;
    if (mhflag != 0)
    {
        fprintf(@out, "#if INTERFACE\n");
        lineno++;
    }
    fprintf(@out, "#define %sTOKENTYPE %s\n", name, lemp.tokentype != 0 ? lemp.tokentype : "void*");
    lineno++;
    if (mhflag != 0)
    {
        fprintf(@out, "#endif\n");
        lineno++;
    }
    fprintf(@out, "typedef union {\n");
    lineno++;
    fprintf(@out, "  int yyinit;\n");
    lineno++;
    fprintf(@out, "  %sTOKENTYPE yy0;\n", name);
    lineno++;
    for (i = 0; i < arraysize; i++)
    {
        if (types[i] == 0)
        {
            continue;
        }
        fprintf(@out, "  %s yy%d;\n", types[i], i + 1);
        lineno++;
        //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
        free(types[i]);
    }
    if (lemp.errsym.useCnt != 0)
    {
        fprintf(@out, "  int yy%d;\n", lemp.errsym.dtnum);
        lineno++;
    }
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
    free(stddt);
    //C++ TO C# CONVERTER TODO TASK: The memory management function 'free' has no equivalent in C#:
    free(types);
    fprintf(@out, "} YYMINORTYPE;\n");
    lineno++;
    plineno = lineno;
}

/*
** Compare to axset structures for sorting purposes
*/
internal static int axset_compare(object a, object b)
{
    axset p1 = (axset)a;
    axset p2 = (axset)b;
    int c;
    c = p2.nAction - p1.nAction;
    if (c == 0)
    {
        c = p1.iOrder - p2.iOrder;
    }
    Debug.Assert(c != 0 || p1 == p2);
    return c;
}

/*
** Write text on "out" that describes the rule "rp".
*/
internal static void writeRuleText(FILE @out, rule rp)
{
    int j;
    fprintf(@out, "%s ::=", rp.lhs.name);
    for (j = 0; j < rp.nrhs; j++)
    {
        symbol[] sp = rp.rhs[j];
        if (sp.type != symbol_type.MULTITERMINAL)
        {
            fprintf(@out, " %s", sp.name);
        }
        else
        {
            int k;
            fprintf(@out, " %s", sp.subsym[0].name);
            for (k = 1; k < sp.nsubsym; k++)
            {
                fprintf(@out, "|%s", sp.subsym[k].name);
            }
        }
    }
}


/*
** Compare two states for sorting purposes.  The smaller state is the
** one with the most non-terminal actions.  If they have the same number
** of non-terminal actions, then the smaller is the one with the most
** token actions.
*/
internal static int stateResortCompare(object a, object b)
{
    state pA = *(const struct state **)a;
		state pB = *(const struct state **)b;
		int n;

n = pB.nNtAct - pA.nNtAct;
		if (n == 0)
		{
			n = pB.nTknAct - pA.nTknAct;
			if (n == 0)
			{
				n = pB.statenum - pA.statenum;
			}
		}
		Debug.Assert(n != 0);
		return n;
	}


	/***************** From the file "set.c" ************************************/
	/*
	** Set manipulation routines for the LEMON parser generator.
	*/

	internal static int size = 0;
/********************** From the file "table.c" ****************************/
/*
** All code in this file has been automatically generated
** from a specification in the file
**              "table.q"
** by the associative array code building program "aagen".
** Do not edit this file!  Instead, edit the specification
** file, then rerun aagen.
*/
/*
** Code for processing tables in the LEMON parser generator.
*/

//C++ TO C# CONVERTER TODO TASK: Pointer arithmetic is detected on the parameter 'x', so pointers on this parameter are left unchanged:
public static uint strhash(sbyte* x)
{
    uint h = 0;
    while (*x != 0)
    {
        h = h * 13 + *(x++);
    }
    return h;
}

/* There is only one instance of the array, which is the following */
internal static s_x1 x1a;

/* There is only one instance of the array, which is the following */
internal static s_x2 x2a;

/* Compare two states */
public static int statecmp(config a, config b)
{
    int rc;
    for (rc = 0; rc == 0 && a != null && b != null; a = a.bp, b = b.bp)
    {
        rc = a.rp.index - b.rp.index;
        if (rc == 0)
        {
            rc = a.dot - b.dot;
        }
    }
    if (rc == 0)
    {
        if (a != null)
        {
            rc = 1;
        }
        if (b != null)
        {
            rc = -1;
        }
    }
    return rc;
}

/* Hash a state */
public static uint statehash(config a)
{
    uint h = 0;
    while (a != null)
    {
        h = h * 571 + a.rp.index * 37 + a.dot;
        a = a.bp;
    }
    return h;
}

/* There is only one instance of the array, which is the following */
internal static s_x3 x3a;

/* Hash a configuration */
public static uint confighash(config a)
{
    uint h = 0;
    h = h * 571 + a.rp.index * 37 + a.dot;
    return h;
}

/* There is only one instance of the array, which is the following */
internal static s_x4 x4a;
}

internal static class DefineConstants
{
    //C++ TO C# CONVERTER TODO TASK: The following #define constant was defined in different ways:
    public const int MAXRHS = 5; // Set low to exercise exception code
    public const int MAXRHS = 1000;
    public const int NO_OFFSET = -2147483647;
    public const int LISTSIZE = 30;
    public const int LINESIZE = 1000;
}

//----------------------------------------------------------------------------------------
//	Copyright © 2006 - 2016 Tangible Software Solutions Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class provides the ability to initialize array elements with the default
//	constructions for the array type.
//----------------------------------------------------------------------------------------
internal static class Arrays
{
    internal static T[] InitializeWithDefaultInstances<T>(int length) where T : new()
    {
        T[] array = new T[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = new T();
        }
        return array;
    }
}

//----------------------------------------------------------------------------------------
//	Copyright © 2006 - 2016 Tangible Software Solutions Inc.
//	This class can be used by anyone provided that the copyright notice remains intact.
//
//	This class provides the ability to simulate various classic C string functions
//	which don't have exact equivalents in the .NET Framework.
//----------------------------------------------------------------------------------------
internal static class StringFunctions
{
    //------------------------------------------------------------------------------------
    //	This method allows replacing a single character in a string, to help convert
    //	C++ code where a single character in a character array is replaced.
    //------------------------------------------------------------------------------------
    internal static string ChangeCharacter(string sourceString, int charIndex, char changeChar)
    {
        return (charIndex > 0 ? sourceString.Substring(0, charIndex) : "")
            + changeChar.ToString() + (charIndex < sourceString.Length - 1 ? sourceString.Substring(charIndex + 1) : "");
    }

    //------------------------------------------------------------------------------------
    //	This method simulates the classic C string function 'isxdigit' (and 'iswxdigit').
    //------------------------------------------------------------------------------------
    internal static bool IsXDigit(char character)
    {
        if (char.IsDigit(character))
            return true;
        else if ("ABCDEFabcdef".IndexOf(character) > -1)
            return true;
        else
            return false;
    }

    //------------------------------------------------------------------------------------
    //	This method simulates the classic C string function 'strchr' (and 'wcschr').
    //------------------------------------------------------------------------------------
    internal static string StrChr(string stringToSearch, char charToFind)
    {
        int index = stringToSearch.IndexOf(charToFind);
        if (index > -1)
            return stringToSearch.Substring(index);
        else
            return null;
    }

    //------------------------------------------------------------------------------------
    //	This method simulates the classic C string function 'strrchr' (and 'wcsrchr').
    //------------------------------------------------------------------------------------
    internal static string StrRChr(string stringToSearch, char charToFind)
    {
        int index = stringToSearch.LastIndexOf(charToFind);
        if (index > -1)
            return stringToSearch.Substring(index);
        else
            return null;
    }

    //------------------------------------------------------------------------------------
    //	This method simulates the classic C string function 'strstr' (and 'wcsstr').
    //------------------------------------------------------------------------------------
    internal static string StrStr(string stringToSearch, string stringToFind)
    {
        int index = stringToSearch.IndexOf(stringToFind);
        if (index > -1)
            return stringToSearch.Substring(index);
        else
            return null;
    }

    //------------------------------------------------------------------------------------
    //	This method simulates the classic C string function 'strtok' (and 'wcstok').
    //	Note that the .NET string 'Split' method cannot be used to simulate 'strtok' since
    //	it doesn't allow changing the delimiters between each token retrieval.
    //------------------------------------------------------------------------------------
    private static string activeString;
    private static int activePosition;
    internal static string StrTok(string stringToTokenize, string delimiters)
    {
        if (stringToTokenize != null)
        {
            activeString = stringToTokenize;
            activePosition = -1;
        }

        //the stringToTokenize was never set:
        if (activeString == null)
            return null;

        //all tokens have already been extracted:
        if (activePosition == activeString.Length)
            return null;

        //bypass delimiters:
        activePosition++;
        while (activePosition < activeString.Length && delimiters.IndexOf(activeString[activePosition]) > -1)
        {
            activePosition++;
        }

        //only delimiters were left, so return null:
        if (activePosition == activeString.Length)
            return null;

        //get starting position of string to return:
        int startingPosition = activePosition;

        //read until next delimiter:
        do
        {
            activePosition++;
        } while (activePosition < activeString.Length && delimiters.IndexOf(activeString[activePosition]) == -1);

        return activeString.Substring(startingPosition, activePosition - startingPosition);
    }
}
