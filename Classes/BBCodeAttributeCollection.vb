'**************************************************
' FILE      : BBCodeAttributeDictionary.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/29/2009 11:39:24 AM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   4/29/2009 11:39:24 AM
'       Paulo Santos
'       Created.
'***************************************************

''' <summary>
''' Represents a collection of Attributes.
''' </summary>
Public Class BBCodeAttributeDictionary
    Implements IDictionary(Of String, String)

    Dim __dic As IDictionary(Of String, String)

    ''' <summary>Initializes an instance of the <see cref="BBCodeAttributeDictionary" /> class.
    ''' This is the default constructor for this class.</summary>
    Public Sub New()
        __dic = New Dictionary(Of String, String)
    End Sub

    ''' <summary>
    ''' Removes all items from the <see cref="BBCodeAttributeDictionary" />.
    ''' </summary>
    Public Sub Clear() Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, String)).Clear
        __dic.Clear()
    End Sub

    ''' <summary>
    ''' Copies the elements of the <see cref="BBCodeAttributeDictionary" /> to an System.Array, starting at a particular System.Array index.
    ''' </summary>
    ''' <param name="array">The one-dimensional System.Array that is the destination of the elements copied from <see cref="BBCodeAttributeDictionary" />. The System.Array must have zero-based indexing.</param>
    ''' <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
    ''' <remarks></remarks>
    Private Sub CopyTo(ByVal array() As System.Collections.Generic.KeyValuePair(Of String, String), ByVal arrayIndex As Integer) Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, String)).CopyTo
        __dic.CopyTo(array, arrayIndex)
    End Sub

    ''' <summary>
    ''' Gets the number of elements contained in the <see cref="BBCodeAttributeDictionary" />.
    ''' </summary>
    ''' <value>The number of elements contained in the <see cref="BBCodeAttributeDictionary" />.</value>
    Public ReadOnly Property Count() As Integer Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, String)).Count
        Get
            Return __dic.Count
        End Get
    End Property

    ''' <summary>
    ''' Gets a value indicating whether the <see cref="BBCodeAttributeDictionary" /> is read-only.
    ''' </summary>
    ''' <value>true if the <see cref="BBCodeAttributeDictionary" /> is read-only; otherwise, false.</value>
    Public ReadOnly Property IsReadOnly() As Boolean Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, String)).IsReadOnly
        Get
            Return __dic.IsReadOnly
        End Get
    End Property

    ''' <summary>
    ''' Adds an element with the provided key and value to the <see cref="BBCodeAttributeDictionary" />.
    ''' </summary>
    ''' <param name="key">The object to use as the key of the element to add.</param>
    ''' <param name="value">The object to use as the value of the element to add.</param>
    Public Sub Add(ByVal key As String, ByVal value As String) Implements System.Collections.Generic.IDictionary(Of String, String).Add
        __dic.Add(key.ToUpperInvariant(), value)
    End Sub

    ''' <summary>
    ''' Determines whether the <see cref="BBCodeAttributeDictionary" /> contains an element with the specified key.
    ''' </summary>
    ''' <param name="key">The key to locate in the <see cref="BBCodeAttributeDictionary" />.</param>
    ''' <returns><c>True</c> if the <see cref="BBCodeAttributeDictionary" /> contains an element with the key; otherwise, <c>False</c>.</returns>
    ''' <remarks></remarks>
    Public Function ContainsKey(ByVal key As String) As Boolean Implements System.Collections.Generic.IDictionary(Of String, String).ContainsKey
        Return __dic.ContainsKey(key.ToUpperInvariant())
    End Function

    ''' <summary>
    ''' Gets or sets the element with the specified key.
    ''' </summary>
    ''' <param name="key">The key of the element to get or set.</param>
    ''' <value>The element with the specified key.</value>
    Default Public Property Item(ByVal key As String) As String Implements System.Collections.Generic.IDictionary(Of String, String).Item
        Get
            Return __dic.Item(key.ToUpperInvariant())
        End Get
        Set(ByVal value As String)
            __dic.Item(key.ToUpperInvariant()) = value
        End Set
    End Property

    ''' <summary>
    ''' Gets an <see cref="System.Collections.Generic.ICollection(Of T)" /> containing the keys of the <see cref="BBCodeAttributeDictionary" />.
    ''' </summary>
    ''' <value>An <see cref="System.Collections.Generic.ICollection(Of T)" /> containing the keys of the object that implements <see cref="BBCodeAttributeDictionary" />.</value>
    Public ReadOnly Property Keys() As System.Collections.Generic.ICollection(Of String) Implements System.Collections.Generic.IDictionary(Of String, String).Keys
        Get
            Return __dic.Keys
        End Get
    End Property

    ''' <summary>
    ''' Removes the element with the specified key from the <see cref="BBCodeAttributeDictionary" />.
    ''' </summary>
    ''' <param name="key">The key of the element to remove.</param>
    ''' <returns><c>True</c> if the element is successfully removed; otherwise, <c>False</c>. This method also returns <c>False</c> if key was not found in the original <see cref="BBCodeAttributeDictionary" />.</returns>
    ''' <remarks></remarks>
    Public Function Remove(ByVal key As String) As Boolean Implements System.Collections.Generic.IDictionary(Of String, String).Remove
        Return __dic.Remove(key)
    End Function

    ''' <summary>
    ''' Gets the value associated with the specified key.
    ''' </summary>
    ''' <param name="key">The key whose value to get.</param>
    ''' <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
    ''' <returns><c>True</c> if the object that implements <see cref="BBCodeAttributeDictionary" /> contains an element with the specified key; otherwise, <c>False</c>.</returns>
    ''' <remarks></remarks>
    Public Function TryGetValue(ByVal key As String, ByRef value As String) As Boolean Implements System.Collections.Generic.IDictionary(Of String, String).TryGetValue
        Return __dic.TryGetValue(key, value)
    End Function

    ''' <summary>
    ''' Gets an <see cref="System.Collections.Generic.ICollection(Of T)" /> containing the values in the <see cref="BBCodeAttributeDictionary" />.
    ''' </summary>
    ''' <value>An <see cref="System.Collections.Generic.ICollection(Of T)" /> containing the values in the object that implements <see cref="BBCodeAttributeDictionary" />.</value>
    Public ReadOnly Property Values() As System.Collections.Generic.ICollection(Of String) Implements System.Collections.Generic.IDictionary(Of String, String).Values
        Get
            Return __dic.Values
        End Get
    End Property

    ''' <summary>
    ''' Returns an enumerator that iterates through the collection.
    ''' </summary>
    ''' <returns>A System.Collections.Generic.IEnumerator(Of T) that can be used to iterate through the collection.</returns>
    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of System.Collections.Generic.KeyValuePair(Of String, String)) Implements System.Collections.Generic.IEnumerable(Of System.Collections.Generic.KeyValuePair(Of String, String)).GetEnumerator
        Return __dic.GetEnumerator()
    End Function

    ''' <summary>
    ''' Adds an item to the <see cref="BBCodeAttributeDictionary"/>.
    ''' </summary>
    ''' <param name="item">The attribute to be added.</param>
    Private Sub Add(ByVal item As System.Collections.Generic.KeyValuePair(Of String, String)) Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, String)).Add
        __dic.Add(item.Key.ToUpperInvariant(), item.Value)
    End Sub

    ''' <summary>
    ''' Removes the first occurrence of a specific object from the <see cref="BBCodeAttributeDictionary" />.
    ''' </summary>
    ''' <param name="item">The object to remove from the <see cref="BBCodeAttributeDictionary" />.</param>
    ''' <returns><c>True</c> if item was successfully removed from the <see cref="BBCodeAttributeDictionary" />; otherwise, <c>False</c>. This method also returns <c>False</c> if item is not found in the original <see cref="BBCodeAttributeDictionary" />.</returns>
    ''' <remarks></remarks>
    Private Function Remove(ByVal item As System.Collections.Generic.KeyValuePair(Of String, String)) As Boolean Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, String)).Remove
        Return __dic.Remove(item)
    End Function

    ''' <summary>
    ''' Determines whether the <see cref="BBCodeAttributeDictionary" /> contains a specific value.
    ''' </summary>
    ''' <param name="item">The object to locate in the <see cref="BBCodeAttributeDictionary" />.</param>
    ''' <returns><c>True</c> if item is found in the <see cref="BBCodeAttributeDictionary" />; otherwise, <c>False</c>.</returns>
    ''' <remarks></remarks>
    Private Function Contains(ByVal item As System.Collections.Generic.KeyValuePair(Of String, String)) As Boolean Implements System.Collections.Generic.ICollection(Of System.Collections.Generic.KeyValuePair(Of String, String)).Contains
        Return __dic.Contains(item)
    End Function

    Private Function IEnumerableGetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return __dic.GetEnumerator()
    End Function

End Class
