'**************************************************
' FILE      : Utils.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/29/2009 2:14:26 PM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   4/29/2009 2:14:26 PM
'       Paulo Santos
'       Created.
'***************************************************

Imports System.Text.RegularExpressions
Imports System.Diagnostics.CodeAnalysis

Friend Module Utils

    Public Const STR_BBCodeSchemaNamespace As String = "" '= "http://pjondevelopment.50webs.com/schema/bbcode"
    Public Const STR_BBCodeDictionaryXmlElement As String = "Dictionary"
    Public Const STR_BBCodeElementTypesXmlElement As String = "ElementTypes"
    Public Const STR_BBCodeConfigurationXmlElement As String = "Configuration"

    ''' <summary>
    ''' Returns the specified text between quotes.
    ''' </summary>
    ''' <param name="text">The text to be placed between quotes.</param>
    ''' <returns>The text between quotes.</returns>
    Public Function Quote(ByVal text As String) As String
        Return "'" & text.Replace("'", "''") & "'"
    End Function

    ''' <summary>
    ''' Unquotes the specified text.
    ''' </summary>
    ''' <param name="text">The text to be unquoted.</param>
    ''' <returns>The unquoted text.</returns>
    Public Function UnQuote(ByVal text As String) As String
        Dim rx As New Text.RegularExpressions.Regex("^(?<quote>'(?<text>(?:''|[^'])*)')|(?<doubleQuote>""(?<text>(?:""""|[^""])*)"")$")
        Dim m = rx.Match(text)
        If (Not m.Success) Then
            Return text
        End If
        If (m.Groups("quote").Success) Then
            Return m.Groups("text").Value.Replace("''", "'")
        End If
        Return m.Groups("text").Value.Replace("""""", """")
    End Function

    ''' <summary>
    ''' Validates the specified tag name.
    ''' </summary>
    ''' <param name="tagName">The tagname to be validated.</param>
    Public Sub ValidateTagName(ByVal tagName As String)
        If (String.IsNullOrEmpty(tagName)) Then
            Throw New ArgumentNullException("tagName")
        End If
        If (tagName.IndexOf("=", StringComparison.Ordinal) <> -1) OrElse (tagName.IndexOf("[", StringComparison.Ordinal) <> -1) OrElse (tagName.IndexOf("]", StringComparison.Ordinal) <> -1) Then
            Throw New ArgumentException("Invalid tag name. The tag name cannot contain '=' '[' or ']'.", "tagName")
        End If
    End Sub

    ''' <summary>
    ''' Validates the specified type to ensure that it is a subclass of <see cref="BBCodeElement"/>.
    ''' </summary>
    ''' <param name="value">The <see cref="Type"/> to be validated.</param>
    Public Sub ValidateBBCodeElementType(ByVal value As Type)

        '*
        '* Validate is nothing
        '*
        If (value Is Nothing) Then
            Throw New ArgumentNullException("value")
        End If

        '*
        '* Validates the BBCodeElement itself
        '*
        Dim bbcodeType = GetType(BBCodeElement)
        If (value.Equals(bbcodeType)) Then
            Exit Sub
        End If

        '*
        '* Validate subclass
        '*
        If Not bbcodeType.IsAssignableFrom(value) Then
            Throw New InvalidOperationException("The type " & value.FullName & " must be a assingable to BBCodeElement.")
        End If

        '*
        '* Validate default constructor
        '*
        If (value.GetConstructor(New Type() {}) Is Nothing) Then
            Throw New InvalidOperationException("The type " & value.FullName & " does not provide a public default constructor.")
        End If

    End Sub

    ''' <summary>
    ''' Encodes the specified text as HTML.
    ''' </summary>
    ''' <param name="text">The text to be encoded.</param>
    ''' <returns>The encoded HTML.</returns>
    <SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Justification:="This methos is simple a list of substuition character by its HTML representation.")> _
    Public Function HtmlEncode(ByVal text As String) As String
        Dim sb As New Text.StringBuilder(text)
        sb.Replace(ChrW(&H26), "&amp;")
        sb.Replace(ChrW(&H22), "&quot;")
        sb.Replace(ChrW(&H27), "&apos;")
        sb.Replace(ChrW(&H3C), "&lt;")
        sb.Replace(ChrW(&H3E), "&gt;")
        sb.Replace(ChrW(&HA0), "&nbsp;")
        sb.Replace(ChrW(&HA1), "&iexcl;")
        sb.Replace(ChrW(&HA2), "&cent;")
        sb.Replace(ChrW(&HA3), "&pound;")
        sb.Replace(ChrW(&HA4), "&curren;")
        sb.Replace(ChrW(&HA5), "&yen;")
        sb.Replace(ChrW(&HA6), "&brvbar;")
        sb.Replace(ChrW(&HA7), "&sect;")
        sb.Replace(ChrW(&HA8), "&uml;")
        sb.Replace(ChrW(&HA9), "&copy;")
        sb.Replace(ChrW(&HAA), "&ordf;")
        sb.Replace(ChrW(&HAB), "&laquo;")
        sb.Replace(ChrW(&HAC), "&not;")
        sb.Replace(ChrW(&HAD), "&shy;")
        sb.Replace(ChrW(&HAE), "&reg;")
        sb.Replace(ChrW(&HAF), "&macr;")
        sb.Replace(ChrW(&HB0), "&deg;")
        sb.Replace(ChrW(&HB1), "&plusmn;")
        sb.Replace(ChrW(&HB2), "&sup2;")
        sb.Replace(ChrW(&HB3), "&sup3;")
        sb.Replace(ChrW(&HB4), "&acute;")
        sb.Replace(ChrW(&HB5), "&micro;")
        sb.Replace(ChrW(&HB6), "&para;")
        sb.Replace(ChrW(&HB7), "&middot;")
        sb.Replace(ChrW(&HB8), "&cedil;")
        sb.Replace(ChrW(&HB9), "&sup1;")
        sb.Replace(ChrW(&HBA), "&ordm;")
        sb.Replace(ChrW(&HBB), "&raquo;")
        sb.Replace(ChrW(&HBC), "&frac14;")
        sb.Replace(ChrW(&HBD), "&frac12;")
        sb.Replace(ChrW(&HBE), "&frac34;")
        sb.Replace(ChrW(&HBF), "&iquest;")
        sb.Replace(ChrW(&HC0), "&Agrave;")
        sb.Replace(ChrW(&HC1), "&Aacute;")
        sb.Replace(ChrW(&HC2), "&Acirc;")
        sb.Replace(ChrW(&HC3), "&Atilde;")
        sb.Replace(ChrW(&HC4), "&Auml;")
        sb.Replace(ChrW(&HC5), "&Aring;")
        sb.Replace(ChrW(&HC6), "&AElig;")
        sb.Replace(ChrW(&HC7), "&Ccedil;")
        sb.Replace(ChrW(&HC8), "&Egrave;")
        sb.Replace(ChrW(&HC9), "&Eacute;")
        sb.Replace(ChrW(&HCA), "&Ecirc;")
        sb.Replace(ChrW(&HCB), "&Euml;")
        sb.Replace(ChrW(&HCC), "&Igrave;")
        sb.Replace(ChrW(&HCD), "&Iacute;")
        sb.Replace(ChrW(&HCE), "&Icirc;")
        sb.Replace(ChrW(&HCF), "&Iuml;")
        sb.Replace(ChrW(&HD0), "&ETH;")
        sb.Replace(ChrW(&HD1), "&Ntilde;")
        sb.Replace(ChrW(&HD2), "&Ograve;")
        sb.Replace(ChrW(&HD3), "&Oacute;")
        sb.Replace(ChrW(&HD4), "&Ocirc;")
        sb.Replace(ChrW(&HD5), "&Otilde;")
        sb.Replace(ChrW(&HD6), "&Ouml;")
        sb.Replace(ChrW(&HD7), "&times;")
        sb.Replace(ChrW(&HD8), "&Oslash;")
        sb.Replace(ChrW(&HD9), "&Ugrave;")
        sb.Replace(ChrW(&HDA), "&Uacute;")
        sb.Replace(ChrW(&HDB), "&Ucirc;")
        sb.Replace(ChrW(&HDC), "&Uuml;")
        sb.Replace(ChrW(&HDD), "&Yacute;")
        sb.Replace(ChrW(&HDE), "&THORN;")
        sb.Replace(ChrW(&HDF), "&szlig;")
        sb.Replace(ChrW(&HE0), "&agrave;")
        sb.Replace(ChrW(&HE1), "&aacute;")
        sb.Replace(ChrW(&HE2), "&acirc;")
        sb.Replace(ChrW(&HE3), "&atilde;")
        sb.Replace(ChrW(&HE4), "&auml;")
        sb.Replace(ChrW(&HE5), "&aring;")
        sb.Replace(ChrW(&HE6), "&aelig;")
        sb.Replace(ChrW(&HE7), "&ccedil;")
        sb.Replace(ChrW(&HE8), "&egrave;")
        sb.Replace(ChrW(&HE9), "&eacute;")
        sb.Replace(ChrW(&HEA), "&ecirc;")
        sb.Replace(ChrW(&HEB), "&euml;")
        sb.Replace(ChrW(&HEC), "&igrave;")
        sb.Replace(ChrW(&HED), "&iacute;")
        sb.Replace(ChrW(&HEE), "&icirc;")
        sb.Replace(ChrW(&HEF), "&iuml;")
        sb.Replace(ChrW(&HF0), "&eth;")
        sb.Replace(ChrW(&HF1), "&ntilde;")
        sb.Replace(ChrW(&HF2), "&ograve;")
        sb.Replace(ChrW(&HF3), "&oacute;")
        sb.Replace(ChrW(&HF4), "&ocirc;")
        sb.Replace(ChrW(&HF5), "&otilde;")
        sb.Replace(ChrW(&HF6), "&ouml;")
        sb.Replace(ChrW(&HF7), "&divide;")
        sb.Replace(ChrW(&HF8), "&oslash;")
        sb.Replace(ChrW(&HF9), "&ugrave;")
        sb.Replace(ChrW(&HFA), "&uacute;")
        sb.Replace(ChrW(&HFB), "&ucirc;")
        sb.Replace(ChrW(&HFC), "&uuml;")
        sb.Replace(ChrW(&HFD), "&yacute;")
        sb.Replace(ChrW(&HFE), "&thorn;")
        sb.Replace(ChrW(&HFF), "&yuml;")
        sb.Replace(ChrW(&H152), "&OElig;")
        sb.Replace(ChrW(&H153), "&oelig;")
        sb.Replace(ChrW(&H160), "&Scaron;")
        sb.Replace(ChrW(&H161), "&scaron;")
        sb.Replace(ChrW(&H178), "&Yuml;")
        sb.Replace(ChrW(&H192), "&fnof;")
        sb.Replace(ChrW(&H2C6), "&circ;")
        sb.Replace(ChrW(&H2DC), "&tilde;")
        sb.Replace(ChrW(&H391), "&Alpha;")
        sb.Replace(ChrW(&H392), "&Beta;")
        sb.Replace(ChrW(&H393), "&Gamma;")
        sb.Replace(ChrW(&H394), "&Delta;")
        sb.Replace(ChrW(&H395), "&Epsilon;")
        sb.Replace(ChrW(&H396), "&Zeta;")
        sb.Replace(ChrW(&H397), "&Eta;")
        sb.Replace(ChrW(&H398), "&Theta;")
        sb.Replace(ChrW(&H399), "&Iota;")
        sb.Replace(ChrW(&H39A), "&Kappa;")
        sb.Replace(ChrW(&H39B), "&Lambda;")
        sb.Replace(ChrW(&H39C), "&Mu;")
        sb.Replace(ChrW(&H39D), "&Nu;")
        sb.Replace(ChrW(&H39E), "&Xi;")
        sb.Replace(ChrW(&H39F), "&Omicron;")
        sb.Replace(ChrW(&H3A0), "&Pi;")
        sb.Replace(ChrW(&H3A1), "&Rho;")
        sb.Replace(ChrW(&H3A3), "&Sigma;")
        sb.Replace(ChrW(&H3A4), "&Tau;")
        sb.Replace(ChrW(&H3A5), "&Upsilon;")
        sb.Replace(ChrW(&H3A6), "&Phi;")
        sb.Replace(ChrW(&H3A7), "&Chi;")
        sb.Replace(ChrW(&H3A8), "&Psi;")
        sb.Replace(ChrW(&H3A9), "&Omega;")
        sb.Replace(ChrW(&H3B1), "&alpha;")
        sb.Replace(ChrW(&H3B2), "&beta;")
        sb.Replace(ChrW(&H3B3), "&gamma;")
        sb.Replace(ChrW(&H3B4), "&delta;")
        sb.Replace(ChrW(&H3B5), "&epsilon;")
        sb.Replace(ChrW(&H3B6), "&zeta;")
        sb.Replace(ChrW(&H3B7), "&eta;")
        sb.Replace(ChrW(&H3B8), "&theta;")
        sb.Replace(ChrW(&H3B9), "&iota;")
        sb.Replace(ChrW(&H3BA), "&kappa;")
        sb.Replace(ChrW(&H3BB), "&lambda;")
        sb.Replace(ChrW(&H3BC), "&mu;")
        sb.Replace(ChrW(&H3BD), "&nu;")
        sb.Replace(ChrW(&H3BE), "&xi;")
        sb.Replace(ChrW(&H3BF), "&omicron;")
        sb.Replace(ChrW(&H3C0), "&pi;")
        sb.Replace(ChrW(&H3C1), "&rho;")
        sb.Replace(ChrW(&H3C2), "&sigmaf;")
        sb.Replace(ChrW(&H3C3), "&sigma;")
        sb.Replace(ChrW(&H3C4), "&tau;")
        sb.Replace(ChrW(&H3C5), "&upsilon;")
        sb.Replace(ChrW(&H3C6), "&phi;")
        sb.Replace(ChrW(&H3C7), "&chi;")
        sb.Replace(ChrW(&H3C8), "&psi;")
        sb.Replace(ChrW(&H3C9), "&omega;")
        sb.Replace(ChrW(&H3D1), "&thetasym;")
        sb.Replace(ChrW(&H3D2), "&upsih;")
        sb.Replace(ChrW(&H3D6), "&piv;")
        sb.Replace(ChrW(&H2002), "&ensp;")
        sb.Replace(ChrW(&H2003), "&emsp;")
        sb.Replace(ChrW(&H2009), "&thinsp;")
        sb.Replace(ChrW(&H200C), "&zwnj;")
        sb.Replace(ChrW(&H200D), "&zwj;")
        sb.Replace(ChrW(&H200E), "&lrm;")
        sb.Replace(ChrW(&H200F), "&rlm;")
        sb.Replace(ChrW(&H2013), "&ndash;")
        sb.Replace(ChrW(&H2014), "&mdash;")
        sb.Replace(ChrW(&H2018), "&lsquo;")
        sb.Replace(ChrW(&H2019), "&rsquo;")
        sb.Replace(ChrW(&H201A), "&sbquo;")
        sb.Replace(ChrW(&H201C), "&ldquo;")
        sb.Replace(ChrW(&H201D), "&rdquo;")
        sb.Replace(ChrW(&H201E), "&bdquo;")
        sb.Replace(ChrW(&H2020), "&dagger;")
        sb.Replace(ChrW(&H2021), "&Dagger;")
        sb.Replace(ChrW(&H2022), "&bull;")
        sb.Replace(ChrW(&H2026), "&hellip;")
        sb.Replace(ChrW(&H2030), "&permil;")
        sb.Replace(ChrW(&H2032), "&prime;")
        sb.Replace(ChrW(&H2033), "&Prime;")
        sb.Replace(ChrW(&H2039), "&lsaquo;")
        sb.Replace(ChrW(&H203A), "&rsaquo;")
        sb.Replace(ChrW(&H203E), "&oline;")
        sb.Replace(ChrW(&H2044), "&frasl;")
        sb.Replace(ChrW(&H20AC), "&euro;")
        sb.Replace(ChrW(&H2111), "&image;")
        sb.Replace(ChrW(&H2118), "&weierp;")
        sb.Replace(ChrW(&H211C), "&real;")
        sb.Replace(ChrW(&H2122), "&trade;")
        sb.Replace(ChrW(&H2135), "&alefsym;")
        sb.Replace(ChrW(&H2190), "&larr;")
        sb.Replace(ChrW(&H2191), "&uarr;")
        sb.Replace(ChrW(&H2192), "&rarr;")
        sb.Replace(ChrW(&H2193), "&darr;")
        sb.Replace(ChrW(&H2194), "&harr;")
        sb.Replace(ChrW(&H21B5), "&crarr;")
        sb.Replace(ChrW(&H21D0), "&lArr;")
        sb.Replace(ChrW(&H21D1), "&uArr;")
        sb.Replace(ChrW(&H21D2), "&rArr;")
        sb.Replace(ChrW(&H21D3), "&dArr;")
        sb.Replace(ChrW(&H21D4), "&hArr;")
        sb.Replace(ChrW(&H2200), "&forall;")
        sb.Replace(ChrW(&H2202), "&part;")
        sb.Replace(ChrW(&H2203), "&exist;")
        sb.Replace(ChrW(&H2205), "&empty;")
        sb.Replace(ChrW(&H2207), "&nabla;")
        sb.Replace(ChrW(&H2208), "&isin;")
        sb.Replace(ChrW(&H2209), "&notin;")
        sb.Replace(ChrW(&H220B), "&ni;")
        sb.Replace(ChrW(&H220F), "&prod;")
        sb.Replace(ChrW(&H2211), "&sum;")
        sb.Replace(ChrW(&H2212), "&minus;")
        sb.Replace(ChrW(&H2217), "&lowast;")
        sb.Replace(ChrW(&H221A), "&radic;")
        sb.Replace(ChrW(&H221D), "&prop;")
        sb.Replace(ChrW(&H221E), "&infin;")
        sb.Replace(ChrW(&H2220), "&ang;")
        sb.Replace(ChrW(&H2227), "&and;")
        sb.Replace(ChrW(&H2228), "&or;")
        sb.Replace(ChrW(&H2229), "&cap;")
        sb.Replace(ChrW(&H222A), "&cup;")
        sb.Replace(ChrW(&H222B), "&int;")
        sb.Replace(ChrW(&H2234), "&there4;")
        sb.Replace(ChrW(&H223C), "&sim;")
        sb.Replace(ChrW(&H2245), "&cong;")
        sb.Replace(ChrW(&H2248), "&asymp;")
        sb.Replace(ChrW(&H2260), "&ne;")
        sb.Replace(ChrW(&H2261), "&equiv;")
        sb.Replace(ChrW(&H2264), "&le;")
        sb.Replace(ChrW(&H2265), "&ge;")
        sb.Replace(ChrW(&H2282), "&sub;")
        sb.Replace(ChrW(&H2283), "&sup;")
        sb.Replace(ChrW(&H2284), "&nsub;")
        sb.Replace(ChrW(&H2286), "&sube;")
        sb.Replace(ChrW(&H2287), "&supe;")
        sb.Replace(ChrW(&H2295), "&oplus;")
        sb.Replace(ChrW(&H2297), "&otimes;")
        sb.Replace(ChrW(&H22A5), "&perp;")
        sb.Replace(ChrW(&H22C5), "&sdot;")
        sb.Replace(ChrW(&H2308), "&lceil;")
        sb.Replace(ChrW(&H2309), "&rceil;")
        sb.Replace(ChrW(&H230A), "&lfloor;")
        sb.Replace(ChrW(&H230B), "&rfloor;")
        sb.Replace(ChrW(&H2329), "&lang;")
        sb.Replace(ChrW(&H232A), "&rang;")
        sb.Replace(ChrW(&H25CA), "&loz;")
        sb.Replace(ChrW(&H2660), "&spades;")
        sb.Replace(ChrW(&H2663), "&clubs;")
        sb.Replace(ChrW(&H2665), "&hearts;")
        sb.Replace(ChrW(&H2666), "&diams;")
        sb.Replace("  ", " &nbsp;")
        Return sb.ToString()
    End Function

    Sub AddTokenizerBaseMacros(ByVal tokenizer As Tokenization.Tokenizer)

        '*
        '* Prepares the BBCode Grammar
        '*
        With tokenizer
            .Options = RegexOptions.Singleline Or RegexOptions.IgnoreCase

            '*
            '* Define the grammar macros
            '*
            .AddMacro("h", "[0-9a-f]")
            .AddMacro("nl", "\n|\r\n|\r|\f")
            .AddMacro("space", "[ \t\r\n\f]+")
            .AddMacro("s", "[ \t\r\n\f]*")
            .AddMacro("w", "{s}?")
            .AddMacro("nonascii", "[\u0080-\uffff]")
            .AddMacro("unicode", "\\{h}{1,6}(\r\n|[ \t\r\n\f])?")
            .AddMacro("escape", "{unicode}|\\[^\r\n\f0-9a-f]")
            .AddMacro("nmstart", "[_a-z]|{nonascii}|{escape}")
            .AddMacro("nmchar", "[_a-z0-9-]|{nonascii}|{escape}")
            .AddMacro("string1", "\""([^\n\r\f\\""]|\\{nl}|{escape})*\""")
            .AddMacro("string2", "\'([^\n\r\f\\']|\\{nl}|{escape})*\'")
            .AddMacro("invalid1", "\""([^\n\r\f\\""]|\\{nl}|{escape})*")
            .AddMacro("invalid2", "\'([^\n\r\f\\']|\\{nl}|{escape})*")
            .AddMacro("name", "{nmchar}+")
            .AddMacro("num", "[0-9]+|[0-9]*\.[0-9]+")
            .AddMacro("string", "{string1}|{string2}")
            .AddMacro("invalid", "{invalid1}|{invalid2}")
            .AddMacro("url", "(ftp|https?://)" & _
                             "(([0-9a-z_!~*'().&=+$%-]+):([0-9a-z_!~*'().&=+$%-]+@))?" & _
                             "(([0-9]{1,3}\.){3}[0-9]{1,3}|([0-9a-z_!~*'()-]+\.)*([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\.[a-z]{2,6})" & _
                             "(:[0-9]{1,4})?" & _
                             "((/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?|(/?))")
            .AddMacro("mail", "(?:mailto:)?[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}")
            .AddMacro("value", "{string}|{mail}|{url}|{num}|{name}")
            .AddMacro("param", "{name}{w}={w}{value}")
            .AddMacro("params", "({space}({param}|{name}))+")

        End With
    End Sub

End Module
