'**************************************************
' FILE      : CircularReferenceException.vb
' AUTHOR    : Paulo Santos
' CREATION  : 9/30/2007 12:12:13 AM
' COPYRIGHT : Copyright © 2007
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       Represents an error when a macro has a circular reference.
'
' Change log:
' 0.1   9/30/2007 12:12:13 AM
'       Paulo Santos
'       Created.
'***************************************************

Imports System.Runtime.Serialization
Imports System.Security.Permissions

''' <summary>
''' Represents an error when a macro has a circular reference.
''' </summary>
<Serializable()> _
Public Class CircularReferenceException
    Inherits Exception

    Dim __Path As String

    ''' <summary>Initializes an instance of the <see cref="CircularReferenceException" /> class.
    ''' This is the default constructor for this class.</summary>
    Public Sub New()
        MyBase.New(My.Resources.CircularReferenceException_Message)
    End Sub

    ''' <summary>Initializes an instance of the <see cref="CircularReferenceException" /> class.</summary>
    ''' <param name="circularReferencePath ">The path of the circular reference.</param>
    Public Sub New(ByVal circularReferencePath As String)
        MyBase.New(My.Resources.CircularReferenceException_Message)
        __Path = circularReferencePath.Replace("}{", "} -> {")
    End Sub

    ''' <summary>Initializes a new instance of the <see cref="CircularReferenceException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    ''' <param name="circularReferencePath ">The path of the circular reference.</param>
    ''' <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    Public Sub New(ByVal circularReferencePath As String, ByVal innerException As System.Exception)
        MyBase.New(My.Resources.CircularReferenceException_Message, innerException)
        __Path = circularReferencePath.Replace("}{", "} -> {")
    End Sub

    ''' <summary>
    ''' Initializes an instance of the <see cref="CircularReferenceException" /> class.
    ''' </summary>
    ''' <param name="message">The message that describes the error.</param>
    ''' <param name="circularReferencePath ">The path of the circular reference.</param>
    Public Sub New(ByVal message As String, ByVal circularReferencePath As String)
        MyBase.New(message)
        __Path = circularReferencePath.Replace("}{", "} -> {")
    End Sub

    ''' <summary>
    ''' Initializes an instance of the <see cref="CircularReferenceException" /> class.
    ''' </summary>
    ''' <param name="message">The message that describes the error.</param>
    ''' <param name="circularReferencePath ">The path of the circular reference.</param>
    ''' <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
    Public Sub New(ByVal message As String, ByVal circularReferencePath As String, ByVal innerException As System.Exception)
        MyBase.New(message, innerException)
        __Path = circularReferencePath.Replace("}{", "} -> {")
    End Sub

    ''' <summary>Initializes a new instance of the <see cref="CircularReferenceException" /> class with serialized data.</summary>
    ''' <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    ''' <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    ''' <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null.</exception>
    ''' <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.CircularReferenceException.HResult" /> is zero (0).</exception>
    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
        __Path = info.GetString("__Path")
    End Sub

    ''' <summary>
    ''' Gets the path of the circular reference.
    ''' </summary>
    ''' <value>The path of the circular reference.</value>
    Public ReadOnly Property CircularReferencePath() As String
        Get
            Return __Path
        End Get
    End Property

    ''' <summary>When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.</summary>
    ''' <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    ''' <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    ''' <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is a null reference (Nothing in Visual Basic).</exception>
    ''' <filterpriority>2</filterpriority>
    ''' <PermissionSet>
    ''' <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
    ''' <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
    ''' </PermissionSet>
    <SecurityPermission(SecurityAction.LinkDemand, flags:=SecurityPermissionFlag.SerializationFormatter)> _
    Public Overrides Sub GetObjectData(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.GetObjectData(info, context)
        info.AddValue("__Path", __Path, GetType(String))
    End Sub

End Class
