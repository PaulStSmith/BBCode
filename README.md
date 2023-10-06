# BBCode
.NET Implementation of BBCode formatter [archival]

This is an old project of mine, that I created back in 2009.<br/>
Imported into GitHub for archival puposes only.<br/>
There will be no updates.

<h2>Introduction</h2>
<p>I've been working on improving the website of the company I work for during the last few weeks . One of my tasks was to provide means to allow the end user to edit rich content text while providing the management with enough peace of mind that the users wouldn't mess up with the site design and configuration.</p>
<p>So I proposed using <a href="http://en.wikipedia.org/wiki/BBCode" target="_blank">
        <u>BBCode</u>
    </a> which is pretty common among many Internet forums (like <a href="http://www.phpbb.com/" target="_blank">
        <u>phpBB</u>
    </a>, <a href="http://forum.snitz.com/" target="_blank">
        <u>Snitz</u>
    </a>, among others) given a very low learning curve, if any, among our user base.</p>
<p>The problem is that the few parsers for BBCode I've found on line weren't flexible and extensible enough to meet our needs. So, as a any developer, I rolled up my sleeves and coded one myself.</p>
<h2>Parsing the BBCode</h2>
<p>The first thing I took into account when creating this parser is that it needed to be flexible, able to add as many tags as wanted, formatting the text in any way possible.</p>
<p>The heart of the parser is a <a href="http://en.wikipedia.org/wiki/Tokenizer#Tokenizer" target="_blank">
        <u>tokenizer</u>
    </a> I've created a while back for another project that never took off, but it was pretty robust for this project, so, I grabbed it along.</p>
<p>With the tokenizer the rest of the code pretty much came by itself, and the desired objective was accomplished.</p>
<p>Within the syntax of BBCode, a tag starts with an open square bracket '<code>[</code>' and ends with a close square bracket '<code>]</code>'. Anything between this two markers is considered a tag.</p>
<h2>BBCode Tags</h2>
<p>There are basically three types of tags:</p>
<ol type="1">
    <li>Simple Tags <code>[tag]</code></li>
    <li>Value Tags <code>[name=value]</code></li>
    <li>Parametrized Tags <code>[tag param=value]</code></li>
</ol>
<p>Each tag may or may not require a closing tag. A closing tag has the same name of the tag starting with a forward slash '<code>/</code>'.</p>
<p>The name of a tag can be any character except the equals sign (<code>=</code>) and the square brackets (<code>[]</code>).</p>
<p>Yes, I know that there are some parsers out there that allows tags with the square bracket within the tag, but this is a limitation I can live with.</p>
<h2>Configuring Tags</h2>
<p>While there are some basic tags common to most of BBCode parsers, there is no standard, although there are a few attempts to do so, like for instance <a href="http://www.bbcode.org/" target="_blank"><u>BBCode.org</u></a>.</p>
<p>With this in mind this parser is very configurable and flexible. It can be configured either programmatically or through an XML file</p>
<p>To configure the parser programmatically you need to add tag definition to the <code>BBCodeParser.Dictionary</code> collection, as shown on the example below.</p>
<pre class="vb" xml:space="preserve">Dim parser As New BBCodeParser()&#13;parser.Dictionary.Add(&quot;b&quot;, &quot;&lt;b&gt;{value}&lt;/b&gt;&quot;, True)&#13;parser.Dictionary.Add(&quot;u&quot;, &quot;&lt;u&gt;{value}&lt;/u&gt;&quot;, True)&#13;parser.Dictionary.Add(&quot;i&quot;, &quot;&lt;i&gt;{value}&lt;/i&gt;&quot;, True)</pre>
<p>The last parameter indicates that the tag requires a closing tag. With the configuration above the text </p>
<pre>[b][u][i]Sample Text[/i][/u][/b]</pre>
<p>would be formatted as:</p>
<p><b><i><u>Sample Text</u></i></b></p>
<h3>Tag Parameters</h3>
<p>Any tag can have any number of parameters. Each parameter consists of a pair of <code>name=value</code>. The value can be between a single (<code>'</code>) or double (<code>&quot;</code>) quote.</p>
<p>This parameters can then be used in placeholders within the replacement text format.</p>
<p>There are two special parameters:</p>
<ul type="disc">
    <li class="MsoNormal">default : Only used for value tags and it has the value of the text after the first equal sign in the tag. </li>
    <li class="MsoNormal">value : Used for any tag that requires a closing tag, represents the formatted text inside the tag.</li>
</ul>
<p>For example, with the configuration below:</p>
<pre class="vb" xml:space="preserve">parser.Dictionary.Add _<br/>  (&quot;url&quot;, _&#13;   &quot;&lt;a href='{default|value}'&gt;{value|default}&lt;/a&gt;&quot;, _&#13;   True)</pre>
<p>the text <code>[url=http://pjondevelopment.50webs.com]here[/url]</code> could be used as a link pointing to this site.</p>
<p>Note that on the replacement format text, the placeholders are formed with '<code>{</code>' and '<code>}</code>', separated by pipes '<code>|</code>', if multiple parameters can be used on its place. When formatting, the first non-empty parameter will be used.</p>
<p>So, in the example above, we have two placeholders: <code>{default|value}</code> and <code>{value|default}</code> this means that in the first placeholder, it will be used either the value of the <code>default</code> parameter or the formatted text between <code>[url=...]</code> and <code>[/url]</code>.</p>
<h2>Extending Tags</h2>
<p>Besides the tag definition the parser can be extended even further by simply creating a derived class from <code>BBCodeElement</code> and add it to the <code>BBCodeParser.ElementTypes</code> collection. With custom types the possibilities are endless, as one might read the contents from a file, a database, or wherever the object wants, based or not on the tag parameters. </p>
<p>Just note that the derived type must provide a public default constructor.</p>
<pre class="vb" xml:space="preserve">parser.ElementTypes.Add _&#13;  (&quot;userImage&quot;, _&#13;   GetType(SubClassOfBBCodeElement))</pre>
<p>With this configuration, every time the text <code>[userImage]</code> is found, it will be replaced by whatever text the class <code>SubClassOfBBCodeElement</code> provides through the method <code>Format(ITextFormatter)</code>.</p>
<h2>Saving and Loading Configurations</h2>
<p>By calling the method <code>SaveConfiguration</code> all the elements' configuration will be saved to an XML file, that can be loaded by the <code>LoadConfiguration</code> method.</p>
<p>The BBCode XML Configuration File has the following structure:</p>
<pre class="xml" xml:space="preserve">&lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&#13;&lt;Configuration&gt;&#13;  &lt;ElementTypes&gt;&#13;    &lt;element &#13;      name=&quot;tagName&quot; &#13;      type=&quot;SubClassOfBBCodeElement, AssemblyQualifiedName&quot; /&gt;&#13;  &lt;/ElementTypes&gt;&#13;  &lt;Dictionary&gt;&#13;    &lt;tag name=&quot;tagName&quot; &#13;         requireClosingTag=&quot;True|False&quot;&gt;&#13;      &lt;!-- XHTML Replacement Text --&gt;&#13;      Use {parameter} as a placeholder &#13;      for any variable portion of the text.&#13;    &lt;/tag&gt;&#13;    &lt;tag name=&quot;otherTagName&quot; &#13;         requireClosingTag=&quot;True|False&quot; &#13;         escaped=&quot;True&quot;&gt;&#13;      Use the escaped=&quot;True&quot; if the text &#13;      is not a valid XML, escapting all &#13;      the necessary XML characters.&#13;    &lt;/tag&gt;&#13;  &lt;/Dictionary&gt;&#13;&lt;/Configuration&gt;</pre>
<p>The tag can be in either or both sections, if a tag is in both sections of the configuration file, the tag will be created using the type specified in the <code>&lt;ElementTypes&gt;</code> section and having its <code>BBCodeElement.ReplacementFormat</code> property set with the text from the <code>&lt;Dictionary&gt;</code> section.</p>
<p>The order of the sections is not important, however the tags must be described, so the BBCode parser can format the tag properly. If a tag is not found in either sections, it will be rendered as simple text, ignoring the fact that it is a tag.</p>
<h2>BBCodeParser Class</h2>
<h3>Public Properties</h3>
<table id="table3" class="Members" border="1" rules="rows" cellSpacing="0" cellPadding="2">
    <tbody>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>Dictionary</code>
                <br/>
                <strong>Read Only</strong>.<br/>Gets the dictionary of known elements of the BBCodeParser Class.
                <pre class="vb" xml:space="preserve">ReadOnly Property Dictionary() _<br/>                As BBCodeElementDictionary</pre>
            </td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>ElementTypes</code>
                <br/>
                <strong>Read Only</strong>.<br/>Gets the dictionary of known custom element of the BBCodeParser Class.
                <pre class="vb" xml:space="preserve">ReadOnly Property ElementTypes() _<br/>            As BBCodeElementTypeDictionary</pre>
            </td>
        </tr>
    </tbody>
</table>
<h3>Public Methods</h3>
<table id="table2" class="Members" border="1" rules="rows" cellSpacing="0" cellPadding="2">
    <tbody>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Method" src="http://pjondevelopment.50webs.com/images/class/method.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>LoadConfiguration</code>
                <br/>Loads the configuration (Dictionary and ElementTypes) from the specified file, Stream or TextReader.
                <pre class="vb" xml:space="preserve">Sub LoadConfiguration(ByVal fileName As String)&#13;Sub LoadConfiguration(ByVal stream As IO.Stream)&#13;Sub LoadConfiguration(ByVal reader As IO.TextReader)</pre>
            </td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Method" src="http://pjondevelopment.50webs.com/images/class/method.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>SaveConfiguration</code>
                <br/>Saves the configuration (Dictionary and ElementTypes) to the specified file, Stream or TextWriter.
                <pre class="vb" xml:space="preserve">Sub SaveConfiguration(ByVal fileName As String)<br/>Sub SaveConfiguration(ByVal stream As IO.Stream)<br/>Sub SaveConfiguration(ByVal writer As IO.TextWriter)</pre>
            </td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Method" src="http://pjondevelopment.50webs.com/images/class/method.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>Parse</code>
                <br/>Parses the BBCode returning an BBCodeDocument.
                <pre class="vb" xml:space="preserve">Function Parse(ByVal text As String) As BBCodeDocument<br/>Function Parse(ByVal stream As IO.Stream) _<br/>                                     As BBCodeDocument<br/>Function Parse(ByVal stream As IO.Stream, _<br/>               ByVal encoding As Text.Encoding) _<br/>                                     As BBCodeDocument<br/>Function Parse(ByVal reader As IO.TextReader) _<br/>                                     As BBCodeDocument</pre>
            </td>
        </tr>
    </tbody>
</table>
</p>
<h2>BBCodeDocument&nbsp;Class</h2>
<h3>Public Properties</h3>
<table id="table3" class="Members" border="1" rules="rows" cellSpacing="0" cellPadding="2">
    <tbody>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>Nodes</code>
                <br/>
                <strong>Read Only</strong>.<br/>Gets the collection of BBCodeNode parsed by the BBCodeParser.
                <pre class="vb" xml:space="preserve">ReadOnly Property Nodes() As BBCodeNodeCollection</pre>
            </td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>Text</code>
                <br/>Gets or sets the BBCode of the BBCodeDocument.
                <pre class="vb" xml:space="preserve">Property Text() As String</pre>
            </td>
        </tr>
    </tbody>
</table>
<h3>Public Methods</h3>
<table id="table2" class="Members" border="1" rules="rows" cellSpacing="0" cellPadding="2">
    <tbody>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Method" src="http://pjondevelopment.50webs.com/images/class/method.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>Format</code>
                <br/>Formats the BBCode into HTML by default, or any other format using an implementation of the ITextFormatter interface.
                <pre class="vb" xml:space="preserve">Function Format() As String &#13;Function Format(ByVal formatter As ITextFormatter) _<br/>                                            As String</pre>
            </td>
        </tr>
    </tbody>
</table>
<h2>BBCodeNode&nbsp;Class</h2>
<h3>Public Properties</h3>
<table id="table3" class="Members" border="1" rules="rows" cellSpacing="0" cellPadding="2">
    <tbody>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>InnerBBCode</code>
                <br/>Gets&nbsp;or sets the BBCode for the node.
                <pre class="vb" xml:space="preserve">Property InnerBBCode() As String</pre>
            </td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>InnerText</code>
                <br/>Gets&nbsp;or sets the text of the BBCodeNode.
                <pre class="vb" xml:space="preserve">Property InnerText() As String</pre>
            </td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>OuterBBCode</code>
                <br/>
                <strong>Read Only</strong>.<br/>Gets&nbsp;the BBCode for the complete node.
                <pre class="vb" xml:space="preserve">Property OuterBBCode() As String</pre>
            </td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>Parent</code>
                <br/>
                <strong>Read Only</strong>.<br/>Gets node above the current BBCodeNode.
                <pre class="vb" xml:space="preserve">Property Parent() As BBCodeNode</pre>
            </td>
        </tr>
    </tbody>
</table>
<h3>Public Methods</h3>
<table id="table2" class="Members" border="1" rules="rows" cellSpacing="0" cellPadding="2">
    <tbody>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Method" src="http://pjondevelopment.50webs.com/images/class/method.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>Format</code>
                <br/>Formats the BBCode using an implementation of the ITextFormatter interface.
                <pre class="vb" xml:space="preserve">Function Format(ByVal formatter As ITextFormatter) _<br/>                                             As String</pre>
            </td>
        </tr>
    </tbody>
</table>
<h2>BBCodeElement&nbsp;Class</h2>Inherits from BBCodeNode <h3>Public Properties</h3>
<table id="table3" class="Members" border="1" rules="rows" cellSpacing="0" cellPadding="2">
    <tbody>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>Attributes</code>
                <br/>
                <strong>Read Only</strong>.<br/>Gets&nbsp;a collection of attributes of the BBCodeElement.
                <pre class="vb" xml:space="preserve">Property ReadOnly Attributes() _<br/>            As BBCodeAttributeDictionary</pre>
            </td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>InnerBBCode</code>
                <br/>Inherited from BBCodeNode.</td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>InnerText</code>
                <br/>Inherited from BBCodeNode.</td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>Name</code>
                <br/>
                <strong>Read Only</strong>.<br/>Gets&nbsp;the name of the tag.
                <pre class="vb" xml:space="preserve">Property ReadOnly Name() As String</pre>
            </td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>Nodes</code>
                <br/>
                <strong>Read Only</strong>.<br/>Gets&nbsp;a collection of BBCodeNode children of the current BBCodeElement.
                <pre class="vb" xml:space="preserve">Property ReadOnly Attributes() _<br/>         As BBCodeAttributeDictionary</pre>
            </td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>OuterBBCode</code>
                <br/>Inherited from BBCodeNode.</td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>Parent</code>
                <br/>Inherited from BBCodeNode.</td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>ReplacementFormat</code>
                <br/>
                <strong>Read Only</strong>.<br/>Gets&nbsp;the string that describes how the element should be formatted by the ITextFormatter.
                <pre class="vb" xml:space="preserve">Property ReadOnly ReplacementFormat() As String</pre>
            </td>
        </tr>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Property" src="http://pjondevelopment.50webs.com/images/class/property.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>RequireClosingTag</code>
                <br/>
                <strong>Read Only</strong>.<br/>Gets&nbsp;a value indicating if the&nbsp;tag requires a closing tag.
                <pre class="vb" xml:space="preserve">Property ReadOnly RequireClosingTag() As Boolean</pre>
            </td>
        </tr>
    </tbody>
</table>
<h3>Public Methods</h3>
<table id="table2" class="Members" border="1" rules="rows" cellSpacing="0" cellPadding="2">
    <tbody>
        <tr>
            <td class="Icon" vAlign="top" align="center">
                <img alt="Public Method" src="http://pjondevelopment.50webs.com/images/class/method.gif" width="16" height="16"/>
            </td>
            <td class="Definition" vAlign="top">
                <code>Format</code>
                <br/>Inherited from BBCodeNode.</td>
        </tr>
    </tbody>
</table>

