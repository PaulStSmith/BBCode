'**************************************************
' FILE      : BBCodeElementTypes.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/30/2009 10:16:36 PM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   4/30/2009 10:16:36 PM
'       Paulo Santos
'       Created.
'***************************************************

Imports System.Xml.Serialization
Imports System.Globalization
Imports System.Diagnostics.CodeAnalysis

''' <summary>
''' Represents a dictionary of types that implements a <see cref="BBCodeElement"/>.
''' </summary>
<Serializable()> _
<XmlRoot(ElementName:=STR_BBCodeElementTypesXmlElement, [Namespace]:=STR_BBCodeSchemaNamespace)> _
Public NotInheritable Class BBCodeElementTypeDictionary
    Inherits Dictionary(Of String, Type)
    Implements IXmlSerializable

    Private Const STR_ConfigurationItem As String = "element"

    ''' <summary>Initializes an instance of the <see cref="BBCodeElementTypeDictionary" /> class.
    ''' This is the default constructor for this class.</summary>
    Friend Sub New()
    End Sub

    ''' <summary>Initializes a new instance of the <see cref="BBCodeElementDictionary" /> class with serialized data.</summary>
    ''' <param name="info">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object containing the information required to serialize the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
    ''' <param name="context">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure containing the source and destination of the serialized stream associated with the <see cref="T:System.Collections.Generic.Dictionary`2" />.</param>
    Private Sub New(ByVal info As System.Runtime.Serialization.SerializationInfo, ByVal context As System.Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

    ''' <summary>
    ''' Adds the specified key and value to the dictionary.
    ''' </summary>
    ''' <param name="tagName">The key of the element to add.</param>
    ''' <param name="value">The value of the element to add.</param>
    Public Shadows Sub Add(ByVal tagName As String, ByVal value As Type)
        ValidateTagName(tagName)
        ValidateBBCodeElementType(value)
        MyBase.Add(tagName.ToUpperInvariant(), value)
    End Sub

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
        If (reader.NamespaceURI <> STR_BBCodeSchemaNamespace) OrElse (reader.LocalName <> STR_BBCodeElementTypesXmlElement) Then
            Exit Sub
        End If

        '*
        '* Reads the items
        '*
        Do While (reader.Read() AndAlso reader.LocalName = STR_ConfigurationItem AndAlso reader.NamespaceURI = STR_BBCodeSchemaNamespace)
            Dim name = reader.GetAttribute("name")
            Dim type = System.Type.GetType(reader.GetAttribute("type"))
            If (type.IsSubclassOf(GetType(BBCodeElement))) Then
                Me.Add(name, type)
            End If
        Loop

        If (reader.NodeType = Xml.XmlNodeType.EndElement) Then
            reader.Read()
        End If

    End Sub

    ''' <summary>Converts an object into its XML representation.</summary>
    ''' <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
    <SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", justification:="For human legibility purposes the tagname must be lower case in the configuration file.")> _
    Public Sub WriteXml(ByVal writer As System.Xml.XmlWriter) Implements System.Xml.Serialization.IXmlSerializable.WriteXml

        For Each it In Me
            writer.WriteStartElement(STR_ConfigurationItem, STR_BBCodeSchemaNamespace)
            writer.WriteAttributeString("name", it.Key.ToLower(CultureInfo.InvariantCulture))
            writer.WriteAttributeString("type", it.Value.AssemblyQualifiedName)
            writer.WriteEndElement()
        Next

    End Sub

End Class
