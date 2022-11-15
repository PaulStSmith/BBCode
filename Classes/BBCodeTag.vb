'**************************************************
' FILE      : BBCodeTag.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/30/2009 10:14:00 AM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   4/30/2009 10:14:00 AM
'       Paulo Santos
'       Created.
'***************************************************

Imports System.Text.RegularExpressions
Imports System.Diagnostics.CodeAnalysis

''' <summary>
''' Helper class to parse a BBCode tag.
''' </summary>
Friend NotInheritable Class BBCodeTag

    Private Shared ReadOnly __Tokenizer As Tokenization.Tokenizer = PrepareTokenizer()

    Private __Text As String
    Private __TagName As String
    Private __IsClosingTag As Boolean
    Private __Params As New BBCodeAttributeDictionary

    ''' <summary>Initializes an instance of the <see cref="BBCodeTag" /> class.</summary>
    ''' <param name="text">The text that defines the tag.</param>
    Public Sub New(ByVal text As String)
        __Text = text

        '*
        '* Read each token to the tag
        '*
        Dim sb As New Text.StringBuilder()
        Dim addParam As Boolean
        Dim paramName As String = String.Empty
        Do
            Dim tk = Tokenizer.GetToken(text)
            If (tk Is Nothing) Then
                Exit Do
            End If

            Select Case tk.RuleType
                Case -1
                    '*
                    '* Check if it's a closing tag
                    '*
                    If (String.CompareOrdinal(tk.Name, "EndTagStart") = 0) Then
                        __IsClosingTag = True
                    End If
                Case 0
                    '*
                    '* Whitespace, ignoring
                    '*
                Case 1
                    '*
                    '* Name. it can be the name of the tag, or the name of a parameter
                    '*
                    If (String.IsNullOrEmpty(__TagName)) Then
                        __TagName = tk.Value.ToUpperInvariant()
                    Else
                        '*
                        '* Check if the paramName is already filled
                        '*
                        If (Not addParam) Then
                            paramName = tk.Value
                            Me.Paramters(paramName) = String.Empty
                        ElseIf (addParam) Then
                            '*
                            '* Add the parameter and it's value
                            '*
                            addParam = False
                            AddParamFromToken(paramName, tk)
                        End If
                    End If
                Case 2
                    '*
                    '* Finds the equals sign
                    '*
                    addParam = True
                Case 3
                    '*
                    '* Value: the value of a parameter
                    '*
                    If (addParam) Then
                        addParam = False
                        AddParamFromToken(paramName, tk)
                    End If
                Case Else
                    sb.Append(tk.Value)
            End Select
        Loop

        If (sb.Length > 0 AndAlso String.IsNullOrEmpty(__TagName)) Then
            __TagName = sb.ToString()
        End If
    End Sub

    ''' <summary>
    ''' Gets the text of the tag.
    ''' </summary>
    <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")> _
    Public ReadOnly Property Text() As String
        Get
            Return __Text
        End Get
    End Property

    ''' <summary>
    ''' Gets the name of the tag.
    ''' </summary>
    Public ReadOnly Property Name() As String
        Get
            Return __TagName
        End Get
    End Property

    ''' <summary>
    ''' Indicates wheter or not the tag is an empty tag.
    ''' </summary>
    ''' <value><c>True</c> if the tag is empty; otherwise <c>False</c>.</value>
    <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification:="Provide description of the object.")> _
    Public ReadOnly Property IsEmptyTag() As Boolean
        Get
            Return String.IsNullOrEmpty(Name)
        End Get
    End Property

    ''' <summary>
    ''' Indicates wheter or not the tag is a closing tag.
    ''' </summary>
    ''' <value><c>True</c> if the tag is a closing tag; otherwise <c>False</c>.</value>
    <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification:="Provide description of the object.")> _
    Public ReadOnly Property IsClosingTag() As Boolean
        Get
            Return __IsClosingTag
        End Get
    End Property

    ''' <summary>
    ''' Indicates wheter or not the tag is a value tag.
    ''' </summary>
    ''' <value><c>True</c> if the tag is a value tag; otherwise <c>False</c>.</value>
    <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification:="Provide description of the object.")> _
    Public ReadOnly Property IsValueTag() As Boolean
        Get
            Return Me.Paramters.Count = 1 AndAlso Me.Paramters.Keys(0) = "default"
        End Get
    End Property

    ''' <summary>
    ''' Indicates wheter or not the tag is a parametrized tag.
    ''' </summary>
    ''' <value><c>True</c> if the tag is a parametrized tag; otherwise <c>False</c>.</value>
    <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification:="Provide description of the object.")> _
    Public ReadOnly Property IsParamTag() As Boolean
        Get
            Return Me.Paramters.Count > 0
        End Get
    End Property

    ''' <summary>
    ''' Indicates wheter or not the tag is a tag.
    ''' </summary>
    ''' <value><c>True</c> if the tag is a tag; otherwise <c>False</c>.</value>
    <SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification:="Provide description of the object.")> _
    Public ReadOnly Property IsTag() As Boolean
        Get
            Return (Not Me.IsEmptyTag) AndAlso (Not Me.IsClosingTag)
        End Get
    End Property

    ''' <summary>
    ''' Gets the paramters of the tag.
    ''' </summary>
    Public ReadOnly Property Paramters() As BBCodeAttributeDictionary
        Get
            Return __Params
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="Tokenization.Tokenizer"/>.
    ''' </summary>
    Private Shared ReadOnly Property Tokenizer() As Tokenization.Tokenizer
        Get
            Return __Tokenizer
        End Get
    End Property

    Private Sub AddParamFromToken(ByRef paramName As String, ByVal tk As Tokenization.Token)

        If (String.IsNullOrEmpty(paramName)) Then
            Me.Paramters("default") = UnQuote(tk.Value)
        Else
            Me.Paramters(paramName) = UnQuote(tk.Value)
        End If
        paramName = String.Empty

    End Sub

    Private Shared Function PrepareTokenizer() As Tokenization.Tokenizer

        Dim tk As New Tokenization.Tokenizer

        '*
        '* Prepares the BBCode Grammar
        '*
        With tk
            '*
            '* Define the grammar macros
            '*
            AddTokenizerBaseMacros(tk)

            '*
            '* Define the grammar rules
            '*
            .AddRule("TagStart", -1, "\[")
            .AddRule("EndTagStart", -1, "\[/")
            .AddRule("TagEnd", -1, "\]")
            .AddRule("Space", 0, "{w}")
            .AddRule("Name", 1, "{name}")
            .AddRule("Equals", 2, "{w}={w}")
            .AddRule("Value", 3, "{value}")
            .AddRule("Text", 99, ".")
        End With

        Return tk

    End Function

End Class
