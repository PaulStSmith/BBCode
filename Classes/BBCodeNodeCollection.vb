'**************************************************
' FILE      : BBCodeNodeCollection.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/29/2009 11:31:52 AM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   4/29/2009 11:31:52 AM
'       Paulo Santos
'       Created.
'***************************************************

''' <summary>
''' Represents a collection of <see cref="BBCodeNode"/>.
''' </summary>
Public Class BBCodeNodeCollection
    Inherits ObjectModel.Collection(Of BBCodeNode)

    Private __Owner As BBCodeNode

    ''' <summary>Initializes an instance of the <see cref="BBCodeNodeCollection" /> class.
    ''' This is the default constructor for this class.</summary>
    Friend Sub New()
    End Sub

    ''' <summary>Initializes an instance of the <see cref="BBCodeNodeCollection" /> class.</summary>
    ''' <param name="owner">The collection owner.</param>
    ''' <exception cref="ArgumentNullException">The argument <paramref name="owner" /> is <langword name="null" />.</exception>
    Friend Sub New(ByVal owner As BBCodeNode)
        If (owner Is Nothing) Then
            Throw New ArgumentNullException("owner")
        End If
        __Owner = owner
    End Sub

    ''' <summary>
    ''' Gets or sets the owner of the collection.
    ''' </summary>
    Friend ReadOnly Property Owner() As BBCodeNode
        Get
            Return __Owner
        End Get
    End Property

    ''' <summary>Adds an object to the end of the <see cref="BBCodeNodeCollection" />.</summary>
    ''' <param name="node">The object to be added to the end of the <see cref="BBCodeNodeCollection" />.</param>
    ''' <exception cref="ArgumentNullException">The argument <paramref name="node" /> is <langword name="null" />.</exception>
    Public Shadows Sub Add(ByVal node As BBCodeNode)
        If (node Is Nothing) Then
            Throw New ArgumentNullException("node")
        End If
        node.SetParent(Me.Owner)
        MyBase.Add(node)
    End Sub

    ''' <summary>Adds the elements of the specified collection to the end of the <see cref="BBCodeNodeCollection" />.</summary>
    ''' <param name="collection">The collection whose elements should be added to the end of the <see cref="BBCodeNodeCollection" />.</param>
    ''' <exception cref="ArgumentNullException">The argument <paramref name="collection" /> is <langword name="null" />.</exception>
    Public Shadows Sub AddRange(ByVal collection As IEnumerable(Of BBCodeNode))
        If (collection Is Nothing) Then
            Throw New ArgumentNullException("collection")
        End If
        For Each n In collection
            Me.Add(n)
        Next
    End Sub

    ''' <summary>
    ''' Inserts an element into the <see cref="BBCodeNodeCollection" /> at the specified index.
    ''' </summary>
    ''' <param name="index">The zero-based index at which item should be inserted.</param>
    ''' <param name="node">The object to insert.</param>
    Public Shadows Sub Insert(ByVal index As Integer, ByVal node As BBCodeNode)
        node.SetParent(Me.Owner)
        MyBase.Insert(index, node)
    End Sub

    ''' <summary>
    ''' Inserts the elements of a collection into the <see cref="BBCodeNodeCollection" /> at the specified index.
    ''' </summary>
    ''' <param name="index">The zero-based index at which item should be inserted.</param>
    ''' <param name="collection">The collection whose elements should be inserted into the <see cref="BBCodeNodeCollection" />.</param>
    Public Shadows Sub InsertRange(ByVal index As Integer, ByVal collection As IEnumerable(Of BBCodeNode))
        For Each node In collection
            node.SetParent(Me.Owner)
        Next
        MyBase.Insert(index, collection)
    End Sub

End Class
