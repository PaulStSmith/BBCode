'**************************************************
' FILE      : Tokenizer.vb
' AUTHOR    : Paulo Santos
' CREATION  : 9/29/2007 10:18:09 PM
' COPYRIGHT : Copyright © 2007
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       Implements a class that retrieves tokens from an input.
'
' Change log:
' 0.1   9/29/2007 10:18:09 PM
'       Paulo Santos
'       Created.
'***************************************************

Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports System.Diagnostics.CodeAnalysis

Namespace Tokenization

    ''' <summary>
    ''' An utility class to get tokens from an input.
    ''' </summary>
    <SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId:="Tokenizer", Justification:="The name is correct")> _
    Public Class Tokenizer

        Private Const __MacroNamePattern As String = "\{(?<name>[_A-Z].*?)\}"

        Private __Macros As New Dictionary(Of String, Macro)
        Private __Rules As New Dictionary(Of String, Rule)
        Private __Options As RegexOptions

#Region " Public Properties "

        ''' <summary>
        ''' Gets a read-only list of the macros of this instance of the <see cref="Tokenizer"/> class.
        ''' </summary>
        ''' <value>A read-only list of the macros of this instance of the <see cref="Tokenizer"/> class.</value>
        Public ReadOnly Property Macros() As MacroCollection
            Get
                Return New MacroCollection(__Macros.Values)
            End Get
        End Property

        ''' <summary>
        ''' Gets a read-only list of the rules of this instance of the <see cref="Tokenizer"/> class.
        ''' </summary>
        ''' <value>A read-only list of the rules of this instance of the <see cref="Tokenizer"/> class.</value>
        Public ReadOnly Property Rules() As RuleCollection
            Get
                Return New RuleCollection(__Rules.Values)
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the regular expression options used to match the rules.
        ''' </summary>
        ''' <value>The regular expression options used to match the rules.</value>
        Public Property Options() As RegexOptions
            Get
                Return __Options
            End Get
            Set(ByVal value As RegexOptions)
                __Options = value
            End Set
        End Property

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Adds a new macro to the list.
        ''' </summary>
        ''' <param name="macro">The macro to be added.</param>
        ''' <exception cref="ReferencedMacroNotFoundException">One or more referenced macros was not found in the collection.</exception>
        ''' <exception cref="CircularReferenceException">The macro has a circular reference.</exception>
        Public Sub AddMacro(ByVal macro As Macro)
            For Each s As String In macro.References
                If (Not __Macros.ContainsKey(s)) Then
                    Throw New ReferencedMacroNotFoundException(s)
                End If
            Next
            EnsureNoCircularReference(macro)
            Expand(macro)
            __Macros.Add(macro.Name, macro)
        End Sub

        ''' <summary>
        ''' Adds a new macro to the list.
        ''' </summary>
        ''' <param name="name">The name of the macro to add.</param>
        ''' <param name="pattern">The pattern of the macro to add.</param>
        Public Sub AddMacro(ByVal name As String, ByVal pattern As String)
            If (String.IsNullOrEmpty(name) OrElse String.IsNullOrEmpty(pattern)) Then
                Throw New ArgumentNullException(IIf(String.IsNullOrEmpty(name), "name", "pattern"))
            End If
            Dim m As New Macro
            m.Name = name
            m.Pattern = pattern
            Me.AddMacro(m)
        End Sub

        ''' <summary>
        ''' Adds a new rule to the list.
        ''' </summary>
        ''' <param name="rule">The rule to be added.</param>
        ''' <exception cref="ReferencedMacroNotFoundException">One or more referenced macro was not found in the collection.</exception>
        Public Sub AddRule(ByVal rule As Rule)
            For Each s As String In rule.References
                If (Not __Macros.ContainsKey(s)) Then
                    Throw New ReferencedMacroNotFoundException(s)
                End If
            Next
            Expand(rule)
            __Rules.Add(rule.Name, rule)
        End Sub

        ''' <summary>
        ''' Adds a new rule to the list.
        ''' </summary>
        ''' <param name="name">The name of the rule to add.</param>
        ''' <param name="type">The type of the rule to add.</param>
        ''' <param name="pattern">The pattern of the rule to add.</param>
        Public Sub AddRule(ByVal name As String, ByVal type As Integer, ByVal pattern As String)
            If (String.IsNullOrEmpty(name) OrElse String.IsNullOrEmpty(pattern)) Then
                Throw New ArgumentNullException(IIf(String.IsNullOrEmpty(name), "name", "pattern"))
            End If
            Dim m As New Rule
            m.Name = name
            m.RuleType = type
            m.Pattern = pattern
            Me.AddRule(m)
        End Sub

        ''' <summary>
        ''' Returns a token from the specified text, consuming it.
        ''' </summary>
        ''' <param name="text">The text from which to extract the token.</param>
        ''' <returns>A token from the specified text if a matching rule is found; otherwise <langword name="null"/>.</returns>
        ''' <remarks>
        ''' The text is passed by reference and the token is consumed, i.e., 
        ''' the token will be removed from the specified text.
        ''' 
        ''' If the text is <see cref="String.Empty"/> or <langword name="null"/> GetToken also returns <langword name="null"/>.
        ''' </remarks>
        <SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#", Justification:="Once a token is read from the string, the token must be consumed, therefore, the string needs to be changed.")> _
        Public Function GetToken(ByRef text As String) As Token

            '*
            '* Check the passed parameters
            '*
            If (String.IsNullOrEmpty(text)) Then
                Return Nothing
            End If

            Dim token As Token
            Dim matchedToken As Token = Nothing

            Dim maxLen As Integer = -1
            For Each name As String In __Rules.Keys
                Dim r As Rule = __Rules(name)
                If (Not r.IsExpanded) Then
                    Expand(r)
                End If

                '*
                '* Try to match the rule 
                '*
                Dim match As Match = r.Regex.Match(text)

                '*
                '* If the match is a success
                '*
                If (match.Success) Then
                    '*
                    '* Add the token to the match list
                    '*
                    token = New Token(r.Name, r.RuleType, match.Value)

                    '*
                    '* Check for the maximum amount of characters consumed
                    '*
                    If (maxLen < match.Value.Length) Then
                        maxLen = match.Value.Length
                        matchedToken = token
                    End If
                End If
            Next

            '*
            '* Was any token found?
            '*
            If (matchedToken IsNot Nothing) Then
                '*
                '* Consumes the token
                '*
                text = text.Substring(matchedToken.Value.Length)
            End If

            Return matchedToken

        End Function

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Determines whether the specified macro has circular reference.
        ''' </summary>
        ''' <param name="macro">The macro to be tested.</param>
        Private Sub EnsureNoCircularReference(ByVal macro As Macro, Optional ByVal path As String = "")

            If (path.IndexOf("{" & macro.Name & "}", StringComparison.OrdinalIgnoreCase) <> (-1)) Then
                Throw New CircularReferenceException(path & "{" & macro.Name & "}")
            End If

            For Each s As String In macro.References
                EnsureNoCircularReference(__Macros.Item(s), path & "{" & macro.Name & "}")
            Next

        End Sub

        ''' <summary>
        ''' Expands all referenced macros in the rule.
        ''' </summary>
        ''' <param name="rule">The rule to be expanded.</param>
        Private Sub Expand(ByRef rule As Rule)

            '*
            '* Resolve literals
            '*
            Dim iPos As Integer = 0
            Dim sb As New StringBuilder
            For Each m As Match In Regex.Matches(rule.Pattern, """([^""]|\\"")*?""")
                sb.Append(rule.Pattern.Substring(iPos, m.Index - iPos))
                sb.Append(EscapeRegExpChars(m.Value.Substring(1, m.Value.Length - 2).Replace("\""", """")))
                iPos = m.Index + m.Length
            Next
            sb.Append(rule.Pattern.Substring(iPos))
            Dim expanded As String = sb.ToString()

            '*
            '* Expand macros
            '*
            For Each s As String In rule.References
                Dim m As Macro = __Macros(s)
                If (Not m.IsExpanded) Then
                    Expand(m)
                End If

                expanded = expanded.Replace("{" & m.Name & "}", "(" & m.ExpandedPattern & ")")
            Next

            rule.SetExpandedPattern(expanded, Me.Options)

        End Sub

        ''' <summary>
        ''' Expands all referenced macros in the macro.
        ''' </summary>
        ''' <param name="macro">The macro to be expanded.</param>
        Private Sub Expand(ByRef macro As Macro)

            Dim expanded As String = macro.Pattern
            For Each s As String In macro.References
                Dim im As Macro = __Macros(s)
                If (Not im.IsExpanded) Then
                    Expand(im)
                End If

                expanded = expanded.Replace("{" & im.Name & "}", "(" & im.ExpandedPattern & ")")
            Next

            macro.SetExpandedPattern(expanded)

        End Sub

        ''' <summary>
        ''' Escapes all regular expression special characters.
        ''' </summary>
        ''' <param name="text">The regular expression pattern to be escaped.</param>
        ''' <returns>A string with all the regular expression special characters escaped.</returns>
        Private Shared Function EscapeRegExpChars(ByVal text As String) As String

            Dim s As String = text
            s = s.Replace("\", "\\")
            s = s.Replace("^", "\^")
            s = s.Replace("$", "\$")
            s = s.Replace("*", "\*")
            s = s.Replace("+", "\+")
            s = s.Replace("?", "\?")
            s = s.Replace("{", "\{")
            s = s.Replace("}", "\}")
            s = s.Replace(".", "\.")
            s = s.Replace("(", "\(")
            s = s.Replace(")", "\)")
            s = s.Replace("[", "\[")
            s = s.Replace("]", "\]")
            s = s.Replace("|", "\|")
            Return s

        End Function

#End Region

    End Class

End Namespace

