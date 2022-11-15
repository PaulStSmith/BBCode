Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Collections.Specialized

Namespace Tokenization
    ''' <summary>
    ''' Represents a Tokenizer RULE.
    ''' </summary>
    Public Structure Rule

#Region " Private Variables "

        Private Const __MacroNamePattern As String = "\{(?<name>[_A-Z].*?)\}"

        Private __Name As String
        Private __Type As Integer
        Private __Pattern As String
        Private __ExpandedPattern As String
        Private __References As StringCollection
        Private __RegexOptions As Text.RegularExpressions.RegexOptions
        Private __Regex As Text.RegularExpressions.Regex

#End Region

#Region " Public Properties "

        ''' <summary>
        ''' Gets or sets the name of the rule.
        ''' </summary>
        ''' <value>The name of the rule.</value>
        ''' <exception cref="ArgumentException">The specified name of the rule contains invalid characters.</exception>
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
                    Throw New ArgumentException(String.Format(CultureInfo.InvariantCulture, My.Resources.Name_ArgumentException_Message, "rule", "value"))
                End If
                __Name = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the type of the rule.
        ''' </summary>
        ''' <value>The type of the rule.</value>
        ''' <remarks>
        ''' The type of the rule is an implementation-specific value that has no meaning outside such implementation.
        ''' </remarks>
        Public Property RuleType() As Integer
            Get
                Return __Type
            End Get
            Set(ByVal value As Integer)
                __Type = value
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the pattern of the rule.
        ''' </summary>
        ''' <value>The pattern of the rule.</value>
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
                Dim mc As MatchCollection = RegEx.Matches(value, __MacroNamePattern, RegexOptions.IgnoreCase)
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
        ''' Gets the expanded pattern of the rule.
        ''' </summary>
        ''' <value>The expanded pattern of the rule.</value>
        Public ReadOnly Property ExpandedPattern() As String
            Get
                If (String.IsNullOrEmpty(__ExpandedPattern)) Then
                    Return __Pattern
                End If
                Return __ExpandedPattern
            End Get
        End Property

        ''' <summary>
        ''' Gets a list of rule names referenced by this rule.
        ''' </summary>
        ''' <value>A list of rule names referenced by this rule.</value>
        Public ReadOnly Property References() As StringCollection
            Get
                If (__References Is Nothing) Then
                    __References = New StringCollection()
                End If
                Return __References
            End Get
        End Property

        ''' <summary>
        ''' Gets the regular expression used to validate this rule.
        ''' </summary>
        ''' <value>A <see cref="Text.RegularExpressions.Regex"/> object used to validate this rule.</value>
        Public ReadOnly Property Regex() As Text.RegularExpressions.Regex
            Get
                If (Not Me.IsExpanded) Then
                    Return Nothing
                End If
                If (__Regex Is Nothing) Then
                    If (Me.ExpandedPattern.StartsWith("^", StringComparison.Ordinal)) Then
                        __Regex = New Text.RegularExpressions.Regex(Me.ExpandedPattern, __RegexOptions)
                    Else
                        __Regex = New Text.RegularExpressions.Regex("^" & Me.ExpandedPattern, __RegexOptions)
                    End If
                End If
                Return __Regex
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
        ''' Sets the expanded pattern of the rule.
        ''' </summary>
        ''' <param name="pattern">The pattern of the rule.</param>
        Friend Sub SetExpandedPattern(ByVal pattern As String, ByVal options As Text.RegularExpressions.RegexOptions)
            __ExpandedPattern = pattern
            __RegexOptions = options And (Not RegexOptions.Compiled)
        End Sub

#End Region

        ''' <summary>Indicates whether this instance and a specified object are equal.</summary>
        ''' <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
        ''' <param name="obj">Another object to compare to.</param>
        ''' <filterpriority>2</filterpriority>
        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If (TypeOf obj Is Rule) Then
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

        Public Shared Operator =(ByVal left As Rule, ByVal right As Rule) As Boolean
            Return left.Equals(right)
        End Operator

        Public Shared Operator <>(ByVal left As Rule, ByVal right As Rule) As Boolean
            Return Not left = right
        End Operator

    End Structure
End Namespace
