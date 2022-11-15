Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Collections.Generic

Namespace Tokenization
    ''' <summary>
    ''' Represents a Tokenized TOKEN.
    ''' </summary>
    Public Class Token

#Region " Private Variables "

        Private __Name As String
        Private __Type As Integer
        Private __Value As String

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes an instance of the <see cref="Token" /> class.
        ''' </summary>
        ''' <param name="name">The name of the token.</param>
        ''' <param name="type">The type of the token.</param>
        ''' <param name="value">The literal value of the token.</param>
        Friend Sub New(ByVal name, ByVal type, ByVal value)
            __Name = name
            __Type = type
            __Value = value
        End Sub

#End Region

#Region " Public Properties "

        ''' <summary>
        ''' Gets the name of the rule that matched the token.
        ''' </summary>
        ''' <value>The name of the rule that matched the token.</value>
        ''' <remarks>
        ''' The name of the token is the name of the rule that matched the toklen.
        ''' </remarks>
        Public ReadOnly Property Name() As String
            Get
                Return __Name
            End Get
        End Property

        ''' <summary>
        ''' Gets the type of the rule that matched the token.
        ''' </summary>
        ''' <value>The type of the rule.</value>
        ''' <remarks>
        ''' The type of the rule is an implementation-specific value that has no meaning outside such implementation.
        ''' </remarks>
        Public ReadOnly Property RuleType() As Integer
            Get
                Return __Type
            End Get
        End Property

        ''' <summary>
        ''' Gets the literal value of the token.
        ''' </summary>
        ''' <value>The literal value of the token.</value>
        Public ReadOnly Property Value() As String
            Get
                Return __Value
            End Get
        End Property

#End Region

    End Class
End Namespace
