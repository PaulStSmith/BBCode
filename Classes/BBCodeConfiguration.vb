'**************************************************
' FILE      : BBCodeConfiguration.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/30/2009 10:57:20 PM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   4/30/2009 10:57:20 PM
'       Paulo Santos
'       Created.
'***************************************************

Imports System.Xml.Serialization

''' <summary>
''' Represents the configuration of the <see cref="BBCodeParser"/>.
''' </summary>
<Serializable()> _
<XmlRoot(ElementName:=STR_BBCodeConfigurationXmlElement, [Namespace]:=STR_BBCodeSchemaNamespace)> _
Public NotInheritable Class BBCodeConfiguration
    Implements IXmlSerializable

    Private Shared ReadOnly __CurrentVersion As New System.Version(1, 0)

    Private __Version As System.Version
    Private __Dictionary As BBCodeElementDictionary
    Private __ElementTypes As BBCodeElementTypeDictionary

    ''' <summary>Initializes an instance of the <see cref="BBCodeConfiguration" /> class.
    ''' This is the default constructor for this class.</summary>
    Friend Sub New()
        __Version = New Version(1, 0)
    End Sub

    ''' <summary>
    ''' Gets the version of the configuration file.
    ''' </summary>
    Public Property Version() As System.Version
        Get
            Return __Version
        End Get
        Private Set(ByVal value As System.Version)
            If (value > __CurrentVersion) Then
                Throw New ArgumentException("Unrecognized version.")
            End If
            __Version = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the dictionary configuration.
    ''' </summary>
    <XmlArrayItem()> _
    Public ReadOnly Property Dictionary() As BBCodeElementDictionary
        Get
            If (__Dictionary Is Nothing) Then
                __Dictionary = New BBCodeElementDictionary()
            End If
            Return __Dictionary
        End Get
    End Property

    ''' <summary>
    ''' Gets the factory configuration.
    ''' </summary>
    <XmlArrayItem()> _
    Public ReadOnly Property ElementTypes() As BBCodeElementTypeDictionary
        Get
            If (__ElementTypes Is Nothing) Then
                __ElementTypes = New BBCodeElementTypeDictionary()
            End If
            Return __ElementTypes
        End Get
    End Property

    ''' <summary>This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.</summary>
    ''' <returns>An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method.</returns>
    Public Function GetSchema() As System.Xml.Schema.XmlSchema Implements System.Xml.Serialization.IXmlSerializable.GetSchema
        Return Nothing
    End Function

    ''' <summary>Generates an object from its XML representation.</summary>
    ''' <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is deserialized.</param>
    Public Sub ReadXml(ByVal reader As System.Xml.XmlReader) Implements System.Xml.Serialization.IXmlSerializable.ReadXml

        '*
        '* Check the name of the element
        '*
        If (reader.NamespaceURI <> STR_BBCodeSchemaNamespace) OrElse (reader.LocalName <> STR_BBCodeConfigurationXmlElement) Then
            Exit Sub
        End If

        '*
        '* Gets the version of the configuration
        '*
        Dim versionString = reader.GetAttribute("version")
        If (Not String.IsNullOrEmpty(versionString)) Then
            Me.Version = New System.Version(versionString)
        End If

        '*
        '* Move to the first item
        '*
        reader.Read()

        Dim dictionarySerializer = New XmlSerializer(GetType(BBCodeElementDictionary))
        Dim typesSerializer = New XmlSerializer(GetType(BBCodeElementTypeDictionary))

        __ElementTypes = Nothing
        __Dictionary = Nothing

        If (reader.LocalName = STR_BBCodeElementTypesXmlElement AndAlso reader.NamespaceURI = STR_BBCodeSchemaNamespace) Then
            __ElementTypes = typesSerializer.Deserialize(reader)
        End If

        If (reader.LocalName = STR_BBCodeDictionaryXmlElement AndAlso reader.NamespaceURI = STR_BBCodeSchemaNamespace) Then
            __Dictionary = dictionarySerializer.Deserialize(reader)
        End If

        If (__ElementTypes Is Nothing AndAlso reader.LocalName = STR_BBCodeElementTypesXmlElement AndAlso reader.NamespaceURI = STR_BBCodeSchemaNamespace) Then
            __ElementTypes = typesSerializer.Deserialize(reader)
        End If

    End Sub

    ''' <summary>Converts an object into its XML representation.</summary>
    ''' <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
    Public Sub WriteXml(ByVal writer As System.Xml.XmlWriter) Implements System.Xml.Serialization.IXmlSerializable.WriteXml

        Dim dictionarySerializer = New XmlSerializer(GetType(BBCodeElementDictionary))
        Dim typesSerializer = New XmlSerializer(GetType(BBCodeElementTypeDictionary))

        If (ElementTypes.Count > 0) Then
            typesSerializer.Serialize(writer, ElementTypes)
        End If

        If (Dictionary.Count > 0) Then
            dictionarySerializer.Serialize(writer, Dictionary)
        End If

    End Sub

End Class
