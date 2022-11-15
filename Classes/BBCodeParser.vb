'**************************************************
' FILE      : BBCodeParser.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/29/2009 11:03:53 AM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   4/29/2009 11:03:53 AM
'       Paulo Santos
'       Created.
'***************************************************

Imports System.Text.RegularExpressions
Imports System.Diagnostics.CodeAnalysis

''' <summary>
''' The parser of 
''' </summary>
Public NotInheritable Class BBCodeParser

    Private __Factory As BBCodeElementFactory
    Private __Configuration As BBCodeConfiguration

    Private Shared ReadOnly __ConfigSerializer As New System.Xml.Serialization.XmlSerializer(GetType(BBCodeConfiguration))
    Private Shared ReadOnly __Tokenizer As Tokenization.Tokenizer = PrepareTokenizer()

    ''' <summary>Initializes an instance of the <see cref="BBCodeParser" /> class.
    ''' This is the default constructor for this class.</summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Gets the dictionary of elements to be replaced by the <see cref="BBCodeParser"/>.
    ''' </summary>
    Public ReadOnly Property Dictionary() As BBCodeElementDictionary
        Get
            If (__Configuration Is Nothing) Then
                __Configuration = New BBCodeConfiguration()
            End If
            Return __Configuration.Dictionary
        End Get
    End Property

    ''' <summary>
    ''' Gets the dictionary of types created by the parser.
    ''' </summary>
    Public ReadOnly Property ElementTypes() As BBCodeElementTypeDictionary
        Get
            If (__Configuration Is Nothing) Then
                __Configuration = New BBCodeConfiguration()
            End If
            Return __Configuration.ElementTypes
        End Get
    End Property

#Region " LoadConfiguration Methods "

    ''' <summary>
    ''' Loads the configuration from the specified filename.
    ''' </summary>
    ''' <param name="fileName">The name of the file to read the dictionary from.</param>
    Public Sub LoadConfiguration(ByVal fileName As String)
        If (String.IsNullOrEmpty(fileName)) Then
            Throw New ArgumentNullException("fileName")
        End If
        Using fileStream As New IO.FileStream(fileName, IO.FileMode.Open)
            LoadConfiguration(fileStream)
        End Using
    End Sub

    ''' <summary>
    ''' Loads the configuration from the specified <see cref="IO.Stream"/>.
    ''' </summary>
    ''' <param name="stream">A <see cref="IO.Stream"/> to read the dictionary from.</param>
    Public Sub LoadConfiguration(ByVal stream As IO.Stream)
        LoadConfiguration(New IO.StreamReader(stream, Text.Encoding.UTF8, True))
    End Sub

    ''' <summary>
    ''' Loads the configuration from the specified <see cref="IO.TextReader"/>.
    ''' </summary>
    ''' <param name="reader">The <see cref="IO.TextReader"/> to read the dictionary from.</param>
    Public Sub LoadConfiguration(ByVal reader As IO.TextReader)
        Dim dic = __ConfigSerializer.Deserialize(reader)
        If (dic IsNot Nothing) Then
            __Configuration = dic
        End If
    End Sub

#End Region

#Region " SaveConfiguration Methods "

    ''' <summary>
    ''' Saves the conficuration to the specified file.
    ''' </summary>
    ''' <param name="fileName">The name of the file to save the dictionary.</param>
    Public Sub SaveConfiguration(ByVal fileName As String)
        If (String.IsNullOrEmpty(fileName)) Then
            Throw New ArgumentNullException("fileName")
        End If
        Using fileStream As New IO.FileStream(fileName, IO.FileMode.Create)
            SaveConfiguration(fileStream)
        End Using
    End Sub

    ''' <summary>
    ''' Saves the conficuration to the specified <see cref="IO.Stream"/>.
    ''' </summary>
    ''' <param name="stream">The <see cref="IO.Stream"/> to save the dictionary.</param>
    Public Sub SaveConfiguration(ByVal stream As IO.Stream)
        SaveConfiguration(New IO.StreamWriter(stream, Text.Encoding.UTF8))
    End Sub

    ''' <summary>
    ''' Saves the conficuration to the specified <see cref="IO.TextWriter"/>.
    ''' </summary>
    ''' <param name="writer">The <see cref="IO.TextWriter"/> to save the dictionary.</param>
    Public Sub SaveConfiguration(ByVal writer As IO.TextWriter)
        __ConfigSerializer.Serialize(writer, __Configuration)
    End Sub

#End Region

#Region " Parse Methods "

    ''' <summary>
    ''' Parses the specified text, returning a collection of <see cref="BBCodeNode"/>.
    ''' </summary>
    ''' <param name="text">The text to be parsed.</param>
    ''' <returns>A <see cref="BBCodeNodeCollection"/> containing the parsed text.</returns>
    Public Function Parse(ByVal text As String) As BBCodeDocument
        Using reader As New IO.StringReader(text)
            Return Parse(reader)
        End Using
    End Function

    ''' <summary>
    ''' Parses the specified stream, returning a collection of <see cref="BBCodeNode"/>.
    ''' </summary>
    ''' <param name="stream">The <see cref="IO.Stream"/> to be parsed.</param>
    ''' <returns>A <see cref="BBCodeNodeCollection"/> containing the parsed <see cref="IO.Stream"/>.</returns>
    Public Function Parse(ByVal stream As IO.Stream) As BBCodeDocument
        Return Parse(stream, Text.Encoding.UTF8)
    End Function

    ''' <summary>
    ''' Parses the specified stream, returning a collection of <see cref="BBCodeNode"/>.
    ''' </summary>
    ''' <param name="stream">The <see cref="IO.Stream"/> to be parsed.</param>
    ''' <param name="encoding">The encoding of the stream.</param>
    ''' <returns>A <see cref="BBCodeNodeCollection"/> containing the parsed <see cref="IO.Stream"/>.</returns>
    Public Function Parse(ByVal stream As IO.Stream, ByVal encoding As Text.Encoding) As BBCodeDocument
        Return Parse(New IO.StreamReader(stream, encoding))
    End Function

    ''' <summary>
    ''' Parses the specified <see cref="IO.TextReader"/>, returning a collection of <see cref="BBCodeNode"/>.
    ''' </summary>
    ''' <param name="reader">The <see cref="IO.TextReader"/> to be parsed.</param>
    ''' <returns>A <see cref="BBCodeNodeCollection"/> containing the parsed <see cref="IO.TextReader"/>.</returns>
    Public Function Parse(ByVal reader As IO.TextReader) As BBCodeDocument

        Dim doc = New BBCodeDocument(Me)
        Dim rootElement As New BBCodeElement(Me)
        Dim currentElement As BBCodeElement = rootElement

        Dim tk As Tokenization.Token
        Dim sb As New Text.StringBuilder()
        Dim sbText As New Text.StringBuilder()
        Do While (reader.Peek() <> -1)
            Dim line As String = reader.ReadLine() & vbCrLf
            sbText.AppendLine(line)
            Do
                '*
                '* Get the next token
                '*
                tk = Tokenizer.GetToken(line)
                If (tk Is Nothing) Then
                    Exit Do
                End If

                Dim tag = New BBCodeTag(tk.Value)

                ParseElement(rootElement, currentElement, tk, sb, tag)
            Loop
        Loop

        '*
        '* Add the text node
        '*
        If (sb.Length > 0) Then
            currentElement.Nodes.Add(New BBCodeText(sb.ToString()))
        End If

        '*
        '* Add the nodes to the document
        '*
        doc.Nodes.AddRange(rootElement.Nodes)

        '*
        '* Sets the source text
        '*
        doc.SetText(sbText.ToString())

        Return doc

    End Function

#End Region

    ''' <summary>
    ''' Gets the <see cref="Tokenization.Tokenizer"/>.
    ''' </summary>
    Private Shared ReadOnly Property Tokenizer() As Tokenization.Tokenizer
        Get
            Return __Tokenizer
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="BBCodeElementFactory"/>.
    ''' </summary>
    Private ReadOnly Property Factory() As BBCodeElementFactory
        Get
            If (__Factory Is Nothing) Then
                __Factory = New BBCodeElementFactory(Me)
            End If
            Return __Factory
        End Get
    End Property

    Private Sub ParseElement(ByVal rootElement As BBCodeElement, ByRef currentElement As BBCodeElement, ByVal token As Tokenization.Token, ByVal sb As Text.StringBuilder, ByVal tag As BBCodeTag)

        '*
        '* Check the token Type
        '*
        Select Case token.RuleType
            Case 0, -1
                '*
                '* Empty tag or char
                '*
                sb.Append(token.Value)
            Case 1
                '*
                '* Closing tag
                '*

                ParseClosingTag(rootElement, currentElement, token, sb, tag)
            Case 2, 3, 4
                '*
                '* Value Tag, Parametrized Tag, Generic Tag
                '*
                ParseTag(currentElement, sb, tag)
        End Select

    End Sub

    Private Sub ParseTag(ByRef currentElement As BBCodeElement, ByVal sb As Text.StringBuilder, ByVal tag As BBCodeTag)

        '*
        '* Add the text previous to the current element
        '*
        If (sb.Length > 0) Then
            currentElement.Nodes.Add(New BBCodeText(sb.ToString()))
            sb.Remove(0, sb.Length)
        End If

        '*
        '* Add the new element to the list of nodes
        '*
        Dim el = Factory.CreateElement(tag.Name, tag.Paramters)
        currentElement.Nodes.Add(el)

        '*
        '* Change the current element, if it requires an closing tag
        '*
        If (el.RequireClosingTag) Then
            currentElement = el
        End If

    End Sub

    Private Shared Sub ParseClosingTag(ByVal rootElement As BBCodeElement, ByRef currentElement As BBCodeElement, ByVal token As Tokenization.Token, ByVal sb As Text.StringBuilder, ByVal tag As BBCodeTag)

        '*
        '* Check if the closing tag is closing a previously open tag
        '*
        If currentElement.RequireClosingTag AndAlso (String.CompareOrdinal(currentElement.Name, tag.Name) = 0) Then
            '*
            '* Add the inner text
            '*
            If (sb.Length > 0) Then
                currentElement.Nodes.Add(New BBCodeText(sb.ToString()))
                sb.Remove(0, sb.Length)
            End If

            '*
            '* Move up a level
            '*
            currentElement = If(currentElement.Parent, rootElement)
        Else
            '*
            '* Adds to the text
            '*
            sb.Append(token.Value)
        End If

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
            .AddRule("EmptyTag", 0, "\[{w}\]")
            .AddRule("ClosingTag", 1, "\[/{name}\]")
            .AddRule("ValueTag", 2, "\[{param}\]")
            .AddRule("ParamsTag", 3, "\[{name}{params}\]")
            .AddRule("Tag", 4, "\[[^ \t\r\n\f\]]+?\]")
            .AddRule("Char", -1, ".")
        End With
        Return tk
    End Function

End Class
