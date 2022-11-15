'**************************************************
' FILE      : BBCodeNode.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/29/2009 11:30:13 AM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       Represents the basic node of an BBCode document
'
' Change log:
' 0.1   4/29/2009 11:30:13 AM
'       Paulo Santos
'       Created.
'***************************************************

''' <summary>
''' Represents the basic node of an BBCode document.
''' </summary>
Public MustInherit Class BBCodeNode

    Private __Parent As BBCodeNode
    Private __Parser As BBCodeParser

    ''' <summary>Initializes an instance of the <see cref="BBCodeNode" /> class.
    ''' This is the default constructor for this class.</summary>
    Protected Sub New()
    End Sub

    ''' <summary>Initializes an instance of the <see cref="BBCodeNode" /> class.</summary>
    ''' <param name="parser">The parser used to create this element.</param>
    Protected Sub New(ByVal parser As BBCodeParser)
        __Parser = parser
    End Sub

    ''' <summary>
    ''' Gets the parent node.
    ''' </summary>
    Public ReadOnly Property Parent() As BBCodeNode
        Get
            Return __Parent
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="BBCodeParser"/> that create this instance of the <see cref="BBCodeNode"/>.
    ''' </summary>
    Protected Friend ReadOnly Property Parser() As BBCodeParser
        Get
            Return __Parser
        End Get
    End Property

    ''' <summary>
    ''' When implemented in a derived class, transforms this instance of <see cref="BBCodeNode"/> into its desired text representation.
    ''' </summary>
    ''' <param name="formatter">An object that implements the <see cref="ITextFormatter"/> interface.</param>
    ''' <returns>The text formatted by the <see cref="ITextFormatter"/>.</returns>
    Public MustOverride Function Format(ByVal formatter As ITextFormatter) As String

    ''' <summary>
    ''' When implemented in a derived class, gets or sets the inner BBCode.
    ''' </summary>
    ''' <value>The BBCode between the start and end tags.</value>
    Public MustOverride Property InnerBBCode() As String

    ''' <summary>
    ''' When implemented in a derived class, gets the outer BBCode.
    ''' </summary>
    ''' <value>The BBCode of this instance of the <see cref="BBCodeNode"/>.</value>
    Public MustOverride ReadOnly Property OuterBBCode() As String

    ''' <summary>
    ''' When implemented in a derived class, gets or sets the plain text of the node.
    ''' </summary>
    ''' <value>The plain text between the start and end tags.</value>
    Public MustOverride Property InnerText() As String

    ''' <summary>
    ''' Sets the <see cref="BBCodeParser"/> of this instance.
    ''' </summary>
    ''' <param name="parser">The new parser of this instance.</param>
    Friend Sub SetParser(ByVal parser As BBCodeParser)
        __Parser = parser
    End Sub

    ''' <summary>
    ''' The the parent node of this instance of the <see cref="BBCodeNode"/>.
    ''' </summary>
    ''' <param name="parentNode">The parent node.</param>
    Protected Friend Sub SetParent(ByVal parentNode As BBCodeNode)
        Dim element = TryCast(parentNode, BBCodeElement)
        If (element Is Nothing) OrElse (String.IsNullOrEmpty(element.Name)) Then
            __Parent = Nothing
        Else
            __Parent = element
        End If
    End Sub

End Class
