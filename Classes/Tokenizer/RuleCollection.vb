'**************************************************
' FILE      : RuleCollection.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/30/2009 1:11:18 PM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   4/30/2009 1:11:18 PM
'       Paulo Santos
'       Created.
'***************************************************

Imports System.Collections.ObjectModel

Namespace Tokenization

    ''' <summary>
    ''' A read-only collection of <see cref="Rule"/>.
    ''' </summary>
    Public NotInheritable Class RuleCollection
        Inherits ReadOnlyCollection(Of Rule)

        ''' <summary>Initializes an instance of the <see cref="RuleCollection" /> class.</summary>
        ''' <param name="items">The items of the collection.</param>
        Friend Sub New(ByVal items As IEnumerable(Of Rule))
            MyBase.New(items)
        End Sub

    End Class

End Namespace
