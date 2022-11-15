'**************************************************
' FILE      : ReferencedMacroNotFoundException.vb
' AUTHOR    : Paulo Santos
' CREATION  : 9/29/2007 11:40:10 PM
' COPYRIGHT : Copyright © 2007
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       Represents an error when a macro reference 
'       an unknown macro.
'
' Change log:
' 0.1   9/29/2007 11:40:10 PM
'       Paulo Santos
'       Created.
'***************************************************

Imports System.Globalization
Imports System.Runtime.Serialization
Imports System.Security.Permissions

''' <summary>
''' Represents an error when a referenced macro is not found in the collection.
''' </summary>
<Serializable()> _
Public Class ReferencedMacroNotFoundException
    Inherits Exception

    Private __ReferencedMacroName As String

#Region " Constructors "

    ''' <summary>
    ''' Initializes an instance of the <see cref="ReferencedMacroNotFoundException" /> class.
    ''' This is the default constructor for this class.
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Initializes an instance of the <see cref="ReferencedMacroNotFoundException" /> class.
    ''' </summary>
    ''' <param name="referencedMacroName">The name of the macro not found.</param>
    Public Sub New(ByVal referencedMacroName As String)
        Me.New(String.Format(CultureInfo.InvariantCulture, My.Resources.ReferencedMacroNotFoundException_Message, referencedMacroName), referencedMacroName)
    End Sub

    ''' <summary>
    ''' Initializes an instance of the <see cref="ReferencedMacroNotFoundException" /> class.
    ''' </summary>
    ''' <param name="message">The message that describes the error.</param>
    ''' <param name="referencedMacroName">The name of the macro not found.</param>
    Public Sub New(ByVal message As String, ByVal referencedMacroName As String)
        MyBase.New(message)
        __ReferencedMacroName = referencedMacroName
    End Sub

    ''' <summary>
    ''' Initializes an instance of the <see cref="ReferencedMacroNotFoundException" /> class.
    ''' </summary>
    ''' <param name="message">The message that describes the error.</param>
    ''' <param name="innerException">The exception that is the cause of the current exception, or a <langword name="null"/> if no inner exception is specified.</param>
    Public Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    ''' <summary>
    ''' Initializes an instance of the <see cref="ReferencedMacroNotFoundException" /> class.
    ''' </summary>
    ''' <param name="message">The message that describes the error.</param>
    ''' <param name="referencedMacroName">The name of the macro not found.</param>
    ''' <param name="innerException">The exception that is the cause of the current exception, or a <langword name="null"/> if no inner exception is specified.</param>
    Public Sub New(ByVal message As String, ByVal referencedMacroName As String, ByVal innerException As Exception)
        Me.New(message, innerException)
        __ReferencedMacroName = referencedMacroName
    End Sub

    ''' <summary>Initializes a new instance of the <see cref="CircularReferenceException" /> class with serialized data.</summary>
    ''' <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
    ''' <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
    ''' <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null.</exception>
    ''' <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.CircularReferenceException.HResult" /> is zero (0).</exception>
    Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
        MyBase.New(info, context)
        __ReferencedMacroName = info.GetString("__ReferencedMacroName")
    End Sub

#End Region

#Region " Public Properties "

    ''' <summary>
    ''' Gets the name of the referenced macro not found in the collection.
    ''' </summary>
    ''' <value>The name of the referenced macro not found in the collection.</value>
    Public ReadOnly Property ReferencedMacroName() As String
        Get
            Return __ReferencedMacroName
        End Get
    End Property

#End Region

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
        info.AddValue("__ReferencedMacroName", __ReferencedMacroName, GetType(String))
    End Sub

End Class
