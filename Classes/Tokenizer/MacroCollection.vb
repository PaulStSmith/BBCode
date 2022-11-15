'**************************************************
' FILE      : MacroCollection.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/30/2009 1:07:21 PM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   4/30/2009 1:07:21 PM
'       Paulo Santos
'       Created.
'***************************************************

Imports System.Collections.ObjectModel

Namespace Tokenization

    ''' <summary>
    ''' A read-only collection of <see cref="Macro"/>.
    ''' </summary>
    Public NotInheritable Class MacroCollection
        Inherits ReadOnlyCollection(Of Macro)

        ''' <summary>Initializes an instance of the <see cref="MacroCollection" /> class.</summary>
        ''' <param name="items">The items of the collection.</param>
        Friend Sub New(ByVal items As IEnumerable(Of Macro))
            MyBase.New(items)
        End Sub

    End Class

End Namespace

