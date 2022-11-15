'**************************************************
' FILE      : BBCodeElementFactory.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/30/2009 10:48:21 PM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   4/30/2009 10:48:21 PM
'       Paulo Santos
'       Created.
'***************************************************

''' <summary>
''' Creates instances of <see cref="BBCodeElement"/>.
''' </summary>
Friend NotInheritable Class BBCodeElementFactory

    Private __Parser As BBCodeParser

    ''' <summary>Initializes an instance of the <see cref="BBCodeElementFactory" /> class.</summary>
    ''' <param name="parser">The <see cref="BBCodeParser"/> that will utilize the new instance of the <see cref="BBCodeElementFactory"/>.</param>
    Friend Sub New(ByVal parser As BBCodeParser)
        __Parser = parser
    End Sub

    ''' <summary>
    ''' Gets the <see cref="BBCodeParser"/> that utilizes this instance of the <see cref="BBCodeElementFactory"/>.
    ''' </summary>
    Public ReadOnly Property Parser() As BBCodeParser
        Get
            Return __Parser
        End Get
    End Property

    ''' <summary>
    ''' Creates a new <see cref="BBCodeElement"/>.
    ''' </summary>
    ''' <param name="name">The name of the element.</param>
    ''' <param name="attributes">The attributes of the element.</param>
    ''' <returns>A new <see cref="BBCodeElement"/>.</returns>
    Public Function CreateElement(ByVal name As String, ByVal attributes As BBCodeAttributeDictionary) As BBCodeElement

        Dim el As BBCodeElement

        '*
        '* Check if we have a different type to create
        '*
        If (Parser.ElementTypes.ContainsKey(name.ToUpperInvariant())) Then
            '*
            '* Gets the type of the element
            '*
            Dim type = Parser.ElementTypes(name.ToUpperInvariant())

            '*
            '* Check if the type IS BBCodeElement
            '*
            If (type.Equals(GetType(BBCodeElement))) Then
                el = New BBCodeElement()
            Else
                '*
                '* Gets the default constructor
                '*
                Dim ctor = type.GetConstructor(New Type() {})
                If (ctor Is Nothing) Then
                    Throw New InvalidOperationException("The type " & type.FullName & " does not have a default constructor.")
                End If

                '*
                '* Creates the new element.
                '*
                el = TryCast(ctor.Invoke(New Object() {}), BBCodeElement)
                If (el Is Nothing) Then
                    Throw New InvalidOperationException("The type " & type.FullName & " could not be assingned to BBCodeElement.")
                End If
            End If
        Else
            el = New BBCodeElement()
        End If

        '*
        '* Set the element properties
        '*
        el.SetName(name)
        el.SetParser(Parser)
        For Each attr In attributes
            el.Attributes.Add(attr.Key, attr.Value)
        Next

        '*
        '* Returns the created element.
        '*
        Return el

    End Function

End Class
