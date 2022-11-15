'**************************************************
' FILE      : BBCodeText.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/29/2009 2:20:10 PM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   4/29/2009 2:20:10 PM
'       Paulo Santos
'       Created.
'***************************************************

''' <summary>
''' Represents a simple text in the BBCode.
''' </summary>
Public NotInheritable Class BBCodeText
    Inherits BBCodeNode

    Private __InnerText As String

    ''' <summary>Initializes an instance of the <see cref="BBCodeText" /> class.
    ''' This is the default constructor for this class.</summary>
    Friend Sub New()
    End Sub

    ''' <summary>Initializes an instance of the <see cref="BBCodeText" /> class.</summary>
    ''' <param name="text">The text of the <see cref="BBCodeText"/>.</param>
    Friend Sub New(ByVal text As String)
        Me.InnerText = text
    End Sub

    ''' <summary>Transforms this instance of <see cref="BBCodeText" /> into its desired text representation.</summary>
    ''' <param name="formatter">An object that implements the <see cref="ITextFormatter" /> interface.</param>
    ''' <returns>The text formatted by the <see cref="ITextFormatter" />.</returns>
    Public Overrides Function Format(ByVal formatter As ITextFormatter) As String
        Return formatter.Format(__InnerText)
    End Function

    ''' <summary>Gets or sets the inner BBCode.</summary>
    ''' <value>The BBCode between the start and end tags.</value>
    Public Overrides Property InnerBBCode() As String
        Get
            Return Me.InnerText
        End Get
        Set(ByVal value As String)
            Me.InnerText = value
        End Set
    End Property

    ''' <summary>Gets or sets the plain text of the node.</summary>
    ''' <value>The plain text between the start and end tags.</value>
    Public Overrides Property InnerText() As String
        Get
            Return __InnerText
        End Get
        Set(ByVal value As String)
            __InnerText = value
        End Set
    End Property

    ''' <summary>Gets the outer BBCode.</summary>
    ''' <value>The BBCode of this instance of the <see cref="BBCodeNode" /> .</value>
    Public Overrides ReadOnly Property OuterBBCode() As String
        Get
            Return Me.InnerText
        End Get
    End Property

    ''' <summary>Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.</summary>
    ''' <returns>A <see cref="T:System.String" /> that represents the current <see cref="T:System.Object" />.</returns>
    ''' <filterpriority>2</filterpriority>
    Public Overrides Function ToString() As String
        Return Me.InnerText
    End Function

End Class
