Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Collections.Specialized

Namespace Tokenization
    ''' <summary>
    ''' Represents a Tokenizer MACRO.
    ''' </summary>
    Public Structure Macro

#Region " Private Variables "

        Private Const __MacroNamePattern As String = "\{(?<name>[_A-Z].*?)\}"
        Private __Name As String
        Private __Pattern As String
        Private __ExpandedPattern As String
        Private __References As StringCollection
        Private __CircularReferencePath As String

#End Region

#Region " Public Properties "

        ''' <summary>
        ''' Gets or sets the name of the macro.
        ''' </summary>
        ''' <value>The name of the macro.</value>
        ''' <exception cref="ArgumentException">The specified name of the macro contains invalid characters.</exception>
        ''' <exception cref="ArgumentNullException">The argument <paramref name="value" /> is <langword name="null" />.</exception>
        Public Property Name() As String
            Get
                Return __Name
            End Get
            Set(ByVal value As String)
                If (String.IsNullOrEmpty(value)) Then
                    Throw New ArgumentNullException("value")
                End If
                If ((value.IndexOf("{", StringComparison.Ordinal) <> (-1)) OrElse (value.IndexOf("}", StringComparison.Ordinal) <> (-1))) Then
                    Throw New ArgumentException(String.Format(CultureInfo.InvariantCulture, My.Resources.Name_ArgumentException_Message, "macro"), "value")
                End If
                __Name = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the pattern of the macro.
        ''' </summary>
        ''' <value>The pattern of the macro.</value>
        ''' <exception cref="ArgumentNullException">The argument <paramref name="value" /> is <langword name="null" />.</exception>
        Public Property Pattern() As String
            Get
                Return __Pattern
            End Get
            Set(ByVal value As String)
                If (String.IsNullOrEmpty(value)) Then
                    Throw New ArgumentNullException("value")
                End If

                '*
                '* Set the references
                '*
                Dim mc As MatchCollection = Regex.Matches(value, __MacroNamePattern, RegexOptions.IgnoreCase)
                Me.References.Clear()
                For Each m As Match In mc
                    Me.References.Add(m.Groups("name").Value)
                Next

                '*
                '* Set the property
                '*
                __Pattern = value
            End Set
        End Property

        ''' <summary>
        ''' Gets the expanded pattern of the macro.
        ''' </summary>
        ''' <value>The expanded pattern of the macro.</value>
        Public ReadOnly Property ExpandedPattern() As String
            Get
                If (String.IsNullOrEmpty(__ExpandedPattern)) Then
                    Return __Pattern
                End If
                Return __ExpandedPattern
            End Get
        End Property

        ''' <summary>
        ''' Gets a list of macro names referenced by this macro.
        ''' </summary>
        ''' <value>A list of macro names referenced by this macro.</value>
        Public ReadOnly Property References() As StringCollection
            Get
                If (__References Is Nothing) Then
                    __References = New StringCollection()
                End If
                Return __References
            End Get
        End Property

#End Region

#Region " Friend Properties "

        ''' <summary>
        ''' Gets a value indicating whether this instance is expanded.
        ''' </summary>
        ''' <value>
        ''' <c>true</c> if this instance is expanded; otherwise, <c>false</c>.
        ''' </value>
        Friend ReadOnly Property IsExpanded() As Boolean
            Get
                Return Not String.IsNullOrEmpty(__ExpandedPattern)
            End Get
        End Property

#End Region

#Region " Friend Methods "

        ''' <summary>
        ''' Sets the expanded pattern of the macro.
        ''' </summary>
        ''' <param name="pattern">The pattern of the macro.</param>
        Friend Sub SetExpandedPattern(ByVal pattern As String)
            __ExpandedPattern = pattern
        End Sub

#End Region

        ''' <summary>Indicates whether this instance and a specified object are equal.</summary>
        ''' <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
        ''' <param name="obj">Another object to compare to.</param>
        ''' <filterpriority>2</filterpriority>
        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If (TypeOf obj Is Macro) Then
                Return Me.Name = obj.Name AndAlso Me.Pattern = obj.Pattern
            End If
            Return False
        End Function

        ''' <summary>Returns the hash code for this instance.</summary>
        ''' <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        ''' <filterpriority>2</filterpriority>
        Public Overrides Function GetHashCode() As Integer
            Dim hash As Long = Me.Name.GetHashCode() + Me.Pattern.GetHashCode()
            Return hash And Integer.MinValue
        End Function

        Public Shared Operator =(ByVal left As Macro, ByVal right As Macro) As Boolean
            Return left.Equals(right)
        End Operator

        Public Shared Operator <>(ByVal left As Macro, ByVal right As Macro) As Boolean
            Return Not left = right
        End Operator

    End Structure
End Namespace
