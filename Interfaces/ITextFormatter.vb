'**************************************************
' FILE      : ITextFormatter.vb
' AUTHOR    : Paulo Santos
' CREATION  : 5/2/2009 10:22:55 AM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   5/2/2009 10:22:55 AM
'       Paulo Santos
'       Created.
'***************************************************

''' <summary>
''' Defines a generic text formatter.
''' </summary>
Public Interface ITextFormatter

    ''' <summary>
    ''' Formats the specified text.
    ''' </summary>
    ''' <param name="source">The text to be formatted.</param>
    ''' <returns>The formatted text.</returns>
    Function Format(ByVal source As String) As String

End Interface
