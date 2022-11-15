'**************************************************
' FILE      : BBCodeDictionary.vb
' AUTHOR    : Paulo Santos
' CREATION  : 4/29/2009 4:57:05 PM
' COPYRIGHT : Copyright © 2009
'             PJ on Development
'             All Rights Reserved.
'
' Description:
'       TODO: Add file description
'
' Change log:
' 0.1   4/29/2009 4:57:05 PM
'       Paulo Santos
'       Created.
'***************************************************

Imports System.Xml.Serialization
Imports System.Runtime.Serialization
Imports System.Diagnostics.CodeAnalysis
Imports System.Globalization

''' <summary>
''' Represents the dictionary of terms.
''' </summary>
<Serializable()> _
<XmlRoot(ElementName:=STR_BBCodeDictionaryXmlElement, [Namespace]:=STR_BBCodeSchemaNamespace)> _
Public NotInheritable Class BBCodeElementDictionary
    Inherits Dictionary(Of String, BBCodeElementDefinition)
    Implements IXmlSerializable
    Private Const STR_DictionaryItem As String = "tag"

    ''' <summary>Initializes an instance of the <see cref="BBCodeElementDictionary" /> class.
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
    Private Shadows Sub Add(ByVal tagName As String, ByVal value As BBCodeElementDefinition)
        ValidateTagName(tagName)
        MyBase.Add(tagName.ToUpperInvariant(), value)
    End Sub

    ''' <summary>
    ''' Adds a new tag definition to the <see cref="BBCodeElementDictionary"/>.
    ''' </summary>
    ''' <param name="tagName">The name of the tag.</param>
    ''' <param name="replacementFormat">The text that the tag will generate, with placeholders for tag parameters.</param>
    Public Shadows Sub Add(ByVal tagName As String, ByVal replacementFormat As String)
        Add(tagName, replacementFormat, False)
    End Sub

    ''' <summary>
    ''' Adds a new tag definition to the <see cref="BBCodeElementDictionary"/>.
    ''' </summary>
    ''' <param name="tagName">The name of the tag.</param>
    ''' <param name="replacementFormat">The text that the tag will generate, with placeholders for tag parameters.</param>
    ''' <param name="requireClosingTag">A value indicating wether or not the taq requires a closing tag.</param>
    Public Shadows Sub Add(ByVal tagName As String, ByVal replacementFormat As String, ByVal requireClosingTag As Boolean)
        Add(tagName, New BBCodeElementDefinition() With {.TagName = tagName, _
                                                     .ReplacementFormat = replacementFormat, _
                                                     .RequireClosingTag = requireClosingTag})
    End Sub

    ''' <summary>This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute" /> to the class.</summary>
    ''' <returns>An <see cref="T:System.Xml.Schema.XmlSchema" /> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)" /> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)" /> method.</returns>
    Private Function GetSchema() As System.Xml.Schema.XmlSchema Implements System.Xml.Serialization.IXmlSerializable.GetSchema
        Return Nothing
    End Function

    ''' <summary>Generates an object from its XML representation.</summary>
    ''' <param name="reader">The <see cref="T:System.Xml.XmlReader" /> stream from which the object is deserialized.</param>
    <SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId:="System.Boolean.TryParse(System.String,System.Boolean@)", Justification:="The return value is not important in this context.")> _
    Private Sub ReadXml(ByVal reader As System.Xml.XmlReader) Implements System.Xml.Serialization.IXmlSerializable.ReadXml

        '*
        '* Check the name of the element
        '*
        If (reader.NamespaceURI <> STR_BBCodeSchemaNamespace) OrElse (reader.LocalName <> STR_BBCodeDictionaryXmlElement) Then
            Exit Sub
        End If

        '*
        '* Move to the first item
        '*
        reader.Read()

        '*
        '* Reads the items
        '*
        Do While (reader.LocalName = STR_DictionaryItem AndAlso reader.NamespaceURI = STR_BBCodeSchemaNamespace)
            Dim definition As New BBCodeElementDefinition
            Dim escaped As Boolean
            With definition
                .TagName = reader.GetAttribute("name")
                Boolean.TryParse(reader.GetAttribute("requireClosingTag"), .RequireClosingTag)
                Boolean.TryParse(reader.GetAttribute("escaped"), escaped)
                If (escaped) Then
                    .ReplacementFormat = reader.ReadElementContentAsString()
                Else
                    .ReplacementFormat = reader.ReadInnerXml().Replace(" xmlns=""""", "")
                End If
            End With
            Me.Add(definition.TagName, definition)
        Loop

        If (reader.NodeType = Xml.XmlNodeType.EndElement) Then
            reader.Read()
        End If

    End Sub

    ''' <summary>Converts an object into its XML representation.</summary>
    ''' <param name="writer">The <see cref="T:System.Xml.XmlWriter" /> stream to which the object is serialized.</param>
    <SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", justification:="For human legibility purposes the tagname must be lower case in the configuration file.")> _
    Private Sub WriteXml(ByVal writer As System.Xml.XmlWriter) Implements System.Xml.Serialization.IXmlSerializable.WriteXml

        Dim doc = New Xml.XmlDocument()
        Dim xml = doc.CreateElement(STR_DictionaryItem, STR_BBCodeSchemaNamespace)
        For Each it In Me
            xml.Attributes.RemoveAll()

            xml.SetAttribute("name", it.Key.ToLower(CultureInfo.InvariantCulture))
            xml.SetAttribute("requireClosingTag", it.Value.RequireClosingTag.ToString())

            Try
                '*
                '* Check if the Replacement format is valid XML
                '*
                xml.InnerXml = it.Value.ReplacementFormat
            Catch ex As Xml.XmlException
                '*
                '* It's not a valid XML, so adds it as a value
                '*
                xml.SetAttribute("escaped", True)
                xml.InnerText = it.Value.ReplacementFormat
            End Try

            '*
            '* OK, we attemp to write it of
            '*
            writer.WriteNode(xml.CreateNavigator(), True)
        Next

    End Sub

End Class
