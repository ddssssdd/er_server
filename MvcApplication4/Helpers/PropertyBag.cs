﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ExpenseReportServer.Helpers
{
     /// <summary>
    /// Creates a serializable string/object dictionary that is XML serializable
    /// Encodes keys as element names and values as simple values with a type
    /// attribute that contains an XML type name. Complex names encode the type 
    /// name with type='___namespace.classname' format followed by a standard xml
    /// serialized format. The latter serialization can be slow so it's not recommended
    /// to pass complex types if performance is critical.
    /// </summary>
    [XmlRoot("properties")]
    public class PropertyBag : PropertyBag<object>
    {
        /// <summary>
        /// Creates an instance of a propertybag from an Xml string
        /// </summary>
        /// <param name="xml">Serialize</param>
        /// <returns></returns>
        public static new PropertyBag CreateFromXml(string xml)
        {
            var bag = new PropertyBag();
            bag.FromXml(xml);
            return bag;            
        }
    }

    /// <summary>
    /// Creates a serializable string for generic types that is XML serializable.
    /// 
    /// Encodes keys as element names and values as simple values with a type
    /// attribute that contains an XML type name. Complex names encode the type 
    /// name with type='___namespace.classname' format followed by a standard xml
    /// serialized format. The latter serialization can be slow so it's not recommended
    /// to pass complex types if performance is critical.
    /// </summary>
    /// <typeparam name="TValue">Must be a reference type. For value types use type object</typeparam>
    [XmlRoot("properties")]    
    public class PropertyBag<TValue> : Dictionary<string, TValue>, IXmlSerializable               
    {           
        /// <summary>
        /// Not implemented - this means no schema information is passed
        /// so this won't work with ASMX/WCF services.
        /// </summary>
        /// <returns></returns>       
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }


        /// <summary>
        /// Serializes the dictionary to XML. Keys are 
        /// serialized to element names and values as 
        /// element values. An xml type attribute is embedded
        /// for each serialized element - a .NET type
        /// element is embedded for each complex type and
        /// prefixed with three underscores.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (string key in this.Keys)
            {
                TValue value = this[key];

                Type type = null;
                if (value != null)
                    type = value.GetType();

                writer.WriteStartElement("item");

                writer.WriteStartElement("key");
                writer.WriteString(key as string);
                writer.WriteEndElement();

                writer.WriteStartElement("value");
                string xmlType = XmlUtils.MapTypeToXmlType(type);
                bool isCustom = false;

                // Type information attribute if not string
                if (value == null)
                {
                    writer.WriteAttributeString("type", "nil");
                }
                else if (!string.IsNullOrEmpty(xmlType))
                {
                    if (xmlType != "string")
                    {
                        writer.WriteStartAttribute("type");
                        writer.WriteString(xmlType);
                        writer.WriteEndAttribute();
                    }
                }
                else
                {
                    isCustom = true;
                    xmlType = "___" + value.GetType().FullName;
                    writer.WriteStartAttribute("type");
                    writer.WriteString(xmlType);
                    writer.WriteEndAttribute();
                }

                // Actual deserialization
                if (!isCustom)
                {
                    if (value != null)
                        writer.WriteValue(value);
                }
                else
                {
                    XmlSerializer ser = new XmlSerializer(value.GetType());
                    ser.Serialize(writer, value);
                }
                writer.WriteEndElement(); // value

                writer.WriteEndElement(); // item
            }
        }
        

        /// <summary>
        /// Reads the custom serialized format
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            this.Clear();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "key")
                {                    
                    string xmlType = null;
                    string name = reader.ReadElementContentAsString(); 

                    // item element
                    reader.ReadToNextSibling("value");
                    
                    if (reader.MoveToNextAttribute())
                        xmlType = reader.Value;
                    reader.MoveToContent();

                    TValue value;
                    if (xmlType == "nil")
                        value = default(TValue); // null
                    else if (string.IsNullOrEmpty(xmlType))
                    {
                        // value is a string or object and we can assign TValue to value
                        string strval = reader.ReadElementContentAsString();
                        value = (TValue) Convert.ChangeType(strval, typeof(TValue)); 
                    }
                    else if (xmlType.StartsWith("___"))
                    {
                        while (reader.Read() && reader.NodeType != XmlNodeType.Element)
                        { }

                        Type type = ReflectionUtils.GetTypeFromName(xmlType.Substring(3));
                        //value = reader.ReadElementContentAs(type,null);
                        XmlSerializer ser = new XmlSerializer(type);
                        value = (TValue)ser.Deserialize(reader);
                    }
                    else
                        value = (TValue)reader.ReadElementContentAs(XmlUtils.MapXmlTypeToType(xmlType), null);

                    this.Add(name, value);
                }
            }
        }


        /// <summary>
        /// Serializes this dictionary to an XML string
        /// </summary>
        /// <returns>XML String or Null if it fails</returns>
        public string ToXml()
        {
            string xml = null;
            SerializationUtils.SerializeObject(this, out xml);
            return xml;
        }

        /// <summary>
        /// Deserializes from an XML string
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>true or false</returns>
        public bool FromXml(string xml)
        {
            this.Clear();

            // if xml string is empty we return an empty dictionary                        
            if (string.IsNullOrEmpty(xml))
                return true;

            var result = SerializationUtils.DeSerializeObject(xml, 
                                                 this.GetType()) as PropertyBag<TValue>;
            if (result != null)
            {
                foreach (var item in result)
                {
                    this.Add(item.Key, item.Value);
                }
            }
            else
                // null is a failure
                return false;

            return true;
        }


        /// <summary>
        /// Creates an instance of a propertybag from an Xml string
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static PropertyBag<TValue> CreateFromXml(string xml)
        {
            var bag = new PropertyBag<TValue>();
            bag.FromXml(xml);
            return bag;
        }
    }
    // Serialization specific code
 
    public static class SerializationUtils
    {
 
        /// <summary>
        /// Serializes an object instance to a file.
        /// </summary>
        /// <param name="instance">the object instance to serialize</param>
        /// <param name="fileName"></param>
        /// <param name="binarySerialization">determines whether XML serialization or binary serialization is used</param>
        /// <returns></returns>
        public static bool SerializeObject(object instance, string fileName, bool binarySerialization)
        {
            bool retVal = true;
 
            if (!binarySerialization)
            {
                XmlTextWriter writer = null;
                try
                {
                    XmlSerializer serializer =
                        new XmlSerializer(instance.GetType());
 
                    // Create an XmlTextWriter using a FileStream.
                    Stream fs = new FileStream(fileName, FileMode.Create);
                    writer = new XmlTextWriter(fs, new UTF8Encoding());
                    writer.Formatting = Formatting.Indented;
                    writer.IndentChar = ' ';
                    writer.Indentation = 3;
 
                    // Serialize using the XmlTextWriter.
                    serializer.Serialize(writer, instance);
                }
                catch(Exception ex)
                {
                    Debug.Write("SerializeObject failed with : " + ex.Message, "West Wind");
                    retVal = false;
                }
                finally
                {
                    if (writer != null)
                        writer.Close();
                }
            }
            else
            {
                Stream fs = null;
                try
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    fs = new FileStream(fileName, FileMode.Create);
                    serializer.Serialize(fs, instance);
                }
                catch
                {
                    retVal = false;
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
            }
 
            return retVal;
        }
 
        /// <summary>
        /// Overload that supports passing in an XML TextWriter. 
        /// </summary>
        /// <remarks>
        /// Note the Writer is not closed when serialization is complete 
        /// so the caller needs to handle closing.
        /// </remarks>
        /// <param name="instance">object to serialize</param>
        /// <param name="writer">XmlTextWriter instance to write output to</param>       
        /// <param name="throwExceptions">Determines whether false is returned on failure or an exception is thrown</param>
        /// <returns></returns>
        public static bool SerializeObject(object instance, XmlTextWriter writer, bool throwExceptions)
        {
            bool retVal = true;
 
            try
            {
                XmlSerializer serializer =
                    new XmlSerializer(instance.GetType());
 
                // Create an XmlTextWriter using a FileStream.
                writer.Formatting = Formatting.Indented;
                writer.IndentChar = ' ';
                writer.Indentation = 3;
 
                // Serialize using the XmlTextWriter.
                serializer.Serialize(writer, instance);
            }
            catch (Exception ex)
            {            
                Debug.Write("SerializeObject failed with : " + ex.GetBaseException().Message + "\r\n" + (ex.InnerException != null ? ex.InnerException.Message : ""), "West Wind");
 
                if (throwExceptions)
                    throw;
 
                retVal = false;
            }
 
            return retVal;
        }
 
        
        /// <summary>
        /// Serializes an object into an XML string variable for easy 'manual' serialization
        /// </summary>
        /// <param name="instance">object to serialize</param>
        /// <param name="xmlResultString">resulting XML string passed as an out parameter</param>
        /// <returns>true or false</returns>
        public static bool SerializeObject(object instance, out string xmlResultString)
        {        
            return SerializeObject(instance, out xmlResultString, false);
        }       
 
        /// <summary>
        /// Serializes an object into a string variable for easy 'manual' serialization
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="xmlResultString">Out parm that holds resulting XML string</param>
        /// <param name="throwExceptions">If true causes exceptions rather than returning false</param>
        /// <returns></returns>
        public static bool SerializeObject(object instance, out string xmlResultString, bool throwExceptions)
        {            
            xmlResultString = string.Empty;
            MemoryStream ms = new MemoryStream();
 
            XmlTextWriter writer = new XmlTextWriter(ms, new UTF8Encoding());
 
            if (!SerializeObject(instance, writer,throwExceptions))
            {
                ms.Close();
                return false;
            }
            
            xmlResultString = Encoding.UTF8.GetString(ms.ToArray(), 0, (int)ms.Length);
 
            ms.Close();
            writer.Close();
 
            return true;
        }
 
 
        /// <summary>
        /// Serializes an object instance to a file.
        /// </summary>
        /// <param name="instance">the object instance to serialize</param>
        /// <param name="Filename"></param>
        /// <param name="BinarySerialization">determines whether XML serialization or binary serialization is used</param>
        /// <returns></returns>
        public static bool SerializeObject(object instance, out byte[] resultBuffer, bool throwExceptions = false)
        {
            bool retVal = true;
 
            MemoryStream ms = null;
            try
            {
                BinaryFormatter serializer = new BinaryFormatter();
                ms = new MemoryStream();
                serializer.Serialize(ms, instance);
            }
            catch(Exception ex)
            {                
                Debug.Write("SerializeObject failed with : " + ex.GetBaseException().Message, "West Wind");
                retVal = false;
 
                if (throwExceptions)
                    throw;
            }
            finally
            {
                if (ms != null)
                    ms.Close();
            }
 
            resultBuffer = ms.ToArray();
 
            return retVal;
        }
 
        /// <summary>
        /// Serializes an object to an XML string. Unlike the other SerializeObject overloads
        /// this methods *returns a string* rather than a bool result!
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="throwExceptions">Determines if a failure throws or returns null</param>
        /// <returns>
        /// null on error otherwise the Xml String.         
        /// </returns>
        /// <remarks>
        /// If null is passed in null is also returned so you might want
        /// to check for null before calling this method.
        /// </remarks>
        public static string SerializeObjectToString(object instance, bool throwExceptions = false)
        {
            string xmlResultString = string.Empty;
 
            if (!SerializeObject(instance, out xmlResultString, throwExceptions))
                return null;
 
            return xmlResultString;
        }
 
        public static byte[] SerializeObjectToByteArray(object instance, bool throwExceptions = false)
        {
            byte[] byteResult = null;
 
            if ( !SerializeObject(instance, out byteResult) )
                return null;
                        
            return byteResult;
        }
 
 
 
                /// <summary>
        /// Deserializes an object from file and returns a reference.
        /// </summary>
        /// <param name="fileName">name of the file to serialize to</param>
        /// <param name="objectType">The Type of the object. Use typeof(yourobject class)</param>
        /// <param name="binarySerialization">determines whether we use Xml or Binary serialization</param>
        /// <returns>Instance of the deserialized object or null. Must be cast to your object type</returns>
        public static object DeSerializeObject(string fileName, Type objectType, bool binarySerialization)
        {
            return DeSerializeObject(fileName, objectType, binarySerialization, false);
        }
 
        /// <summary>
        /// Deserializes an object from file and returns a reference.
        /// </summary>
        /// <param name="fileName">name of the file to serialize to</param>
        /// <param name="objectType">The Type of the object. Use typeof(yourobject class)</param>
        /// <param name="binarySerialization">determines whether we use Xml or Binary serialization</param>
        /// <param name="throwExceptions">determines whether failure will throw rather than return null on failure</param>
        /// <returns>Instance of the deserialized object or null. Must be cast to your object type</returns>
        public static object DeSerializeObject(string fileName, Type objectType, bool binarySerialization, bool throwExceptions)
        {
            object instance = null;
 
            if (!binarySerialization)
            {
 
                XmlReader reader = null;
                XmlSerializer serializer = null;
                FileStream fs = null;
                try
                {
                    // Create an instance of the XmlSerializer specifying type and namespace.
                    serializer = new XmlSerializer(objectType);
 
                    // A FileStream is needed to read the XML document.
                    fs = new FileStream(fileName, FileMode.Open);
                    reader = new XmlTextReader(fs);
 
                    instance = serializer.Deserialize(reader);
                }
                catch(Exception ex)
                {
                    if (throwExceptions)
                        throw;
 
                    string message = ex.Message;
                    return null;
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
 
                    if (reader != null)
                        reader.Close();
                }
            }
            else
            {
 
                BinaryFormatter serializer = null;
                FileStream fs = null;
 
                try
                {
                    serializer = new BinaryFormatter();
                    fs = new FileStream(fileName, FileMode.Open);
                    instance = serializer.Deserialize(fs);
 
                }
                catch
                {
                    return null;
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                }
            }
 
            return instance;
        }
 
        /// <summary>
        /// Deserialize an object from an XmlReader object.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static object DeSerializeObject(XmlReader reader, Type objectType)
        {
            XmlSerializer serializer = new XmlSerializer(objectType);
            object Instance = serializer.Deserialize(reader);
            reader.Close();
 
            return Instance;
        }
 
        public static object DeSerializeObject(string xml, Type objectType)
        {
            XmlTextReader reader = new XmlTextReader(xml, XmlNodeType.Document, null);
            return DeSerializeObject(reader, objectType);
        }
 
        /// <summary>
        /// Deseializes a binary serialized object from  a byte array
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="objectType"></param>
        /// <param name="throwExceptions"></param>
        /// <returns></returns>
        public static object DeSerializeObject(byte[] buffer, Type objectType, bool throwExceptions = false)
        {
            BinaryFormatter serializer = null;
            MemoryStream ms = null;
            object Instance = null;
 
            try
            {
                serializer = new BinaryFormatter();
                ms = new MemoryStream(buffer);
                Instance = serializer.Deserialize(ms);
 
            }
            catch
            {
                if (throwExceptions)
                    throw;
 
                return null;
            }
            finally
            {
                if (ms != null)
                    ms.Close();
            }
 
            return Instance;
        }
 
 
        /// <summary>
        /// Returns a string of all the field value pairs of a given object.
        /// Works only on non-statics.
        /// </summary>
        /// <param name="instanc"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ObjectToString(object instanc, string separator, ObjectToStringTypes type)
        {
            FieldInfo[] fi = instanc.GetType().GetFields();
 
            string output = string.Empty;
 
            if (type == ObjectToStringTypes.Properties || type == ObjectToStringTypes.PropertiesAndFields)
            {
                foreach (PropertyInfo property in instanc.GetType().GetProperties())
                {
                    try
                    {
                        output += property.Name + ":" + property.GetValue(instanc, null).ToString() + separator;
                    }
                    catch
                    {
                        output += property.Name + ": n/a" + separator;
                    }
                }
            }
 
            if (type == ObjectToStringTypes.Fields || type == ObjectToStringTypes.PropertiesAndFields)
            {
                foreach (FieldInfo field in fi)
                {
                    try
                    {
                        output = output + field.Name + ": " + field.GetValue(instanc).ToString() + separator;
                    }
                    catch
                    {
                        output = output + field.Name + ": n/a" + separator;
                    }
                }
            }
            return output;
        }
 
    }
 
    public enum ObjectToStringTypes
    {
        Properties,
        PropertiesAndFields,
        Fields
    }
    /// <summary>
    /// String utility class that provides a host of string related operations
    /// </summary>
    public static class XmlUtils
    {
        /// <summary>
        /// Retrieves a result string from an XPATH query. Null if not found.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static string GetXmlString(XmlNode node, string XPath, XmlNamespaceManager ns)
        {
            XmlNode selNode = node.SelectSingleNode(XPath, ns);
            if (selNode == null)
                return null;

            return selNode.InnerText;
        }

        /// <summary>
        /// Retrieves a result int value from an XPATH query. 0 if not found.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static int GetXmlInt(XmlNode node, string XPath, XmlNamespaceManager ns)
        {
            string val = GetXmlString(node, XPath, ns);
            if (val == null)
                return 0;

            int result = 0;
            int.TryParse(val, out result);

            return result;
        }

        /// <summary>
        /// Retrieves a result bool from an XPATH query. false if not found.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static bool GetXmlBool(XmlNode node, string XPath, XmlNamespaceManager ns)
        {
            string val = GetXmlString(node, XPath, ns);
            if (val == null)
                return false;

            if (val == "1" || val == "true" || val == "True")
                return true;

            return false;
        }

        /// <summary>
        /// Retrieves a result DateTime from an XPATH query. 1/1/1900  if not found.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static DateTime GetXmlDateTime(XmlNode node, string XPath, XmlNamespaceManager ns)
        {
            DateTime dtVal = new DateTime(1900, 1, 1, 0, 0, 0);

            string val = GetXmlString(node, XPath, ns);
            if (val == null)
                return dtVal;

            try
            {
                dtVal = XmlConvert.ToDateTime(val, XmlDateTimeSerializationMode.Utc);
            }
            catch { }

            return dtVal;
        }

        /// <summary>
        /// Gets an attribute by name
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns>value or null if not available</returns>
        public static string GetXmlAttributeString(XmlNode node, string attributeName)
        {
            XmlAttribute att = node.Attributes[attributeName];
            if (att == null)
                return null;

            return att.InnerText;
        }

        /// <summary>
        /// Returns an integer value from an attribute
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetXmlAttributeInt(XmlNode node, string attributeName, int defaultValue)
        {
            string val = GetXmlAttributeString(node, attributeName);
            if (val == null)
                return defaultValue;

            return XmlConvert.ToInt32(val);
        }



        /// <summary>
        /// Converts a .NET type into an XML compatible type - roughly
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string MapTypeToXmlType(Type type)
        {
            if (type == typeof(string) || type == typeof(char))
                return "string";
            if (type == typeof(int) || type == typeof(Int32))
                return "integer";
            if (type == typeof(long) || type == typeof(Int64))
                return "long";
            if (type == typeof(bool))
                return "boolean";
            if (type == typeof(DateTime))
                return "datetime";

            if (type == typeof(float))
                return "float";
            if (type == typeof(decimal))
                return "decimal";
            if (type == typeof(double))
                return "double";
            if (type == typeof(Single))
                return "single";

            if (type == typeof(byte))
                return "byte";

            if (type == typeof(byte[]))
                return "base64Binary";

            return null;

            // *** hope for the best
            //return type.ToString().ToLower();
        }


        public static Type MapXmlTypeToType(string xmlType)
        {
            xmlType = xmlType.ToLower();

            if (xmlType == "string")
                return typeof(string);
            if (xmlType == "integer")
                return typeof(int);
            if (xmlType == "long")
                return typeof(long);
            if (xmlType == "boolean")
                return typeof(bool);
            if (xmlType == "datetime")
                return typeof(DateTime);
            if (xmlType == "float")
                return typeof(float);
            if (xmlType == "decimal")
                return typeof(decimal);
            if (xmlType == "double")
                return typeof(Double);
            if (xmlType == "single")
                return typeof(Single);

            if (xmlType == "byte")
                return typeof(byte);
            if (xmlType == "base64binary")
                return typeof(byte[]);


            // return null if no match is found
            // don't throw so the caller can decide more efficiently what to do 
            // with this error result
            return null;
        }


        /// <summary>
        /// Creates an Xml NamespaceManager for an XML document by looking
        /// at all of the namespaces defined on the document root element.
        /// </summary>
        /// <param name="doc">The XmlDom instance to attach the namespacemanager to</param>
        /// <param name="defaultNamespace">The prefix to use for prefix-less nodes (which are not supported if any namespaces are used in XmlDoc).</param>
        /// <returns></returns>
        public static XmlNamespaceManager CreateXmlNamespaceManager(XmlDocument doc, string defaultNamespace)
        {
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            foreach (XmlAttribute attr in doc.DocumentElement.Attributes)
            {
                if (attr.Prefix == "xmlns")
                    nsmgr.AddNamespace(attr.LocalName, attr.Value);
                if (attr.Name == "xmlns")
                    // default namespace MUST use a prefix
                    nsmgr.AddNamespace(defaultNamespace, attr.Value);
            }

            return nsmgr;
        }

    }
    public static class ReflectionUtils
    {

        /// <summary>
        /// Binding Flags constant to be reused for all Reflection access methods.
        /// </summary>
        public const BindingFlags MemberAccess =
            BindingFlags.Public | BindingFlags.NonPublic |
            BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase;


        /// <summary>
        /// Retrieve a property value from an object dynamically. This is a simple version
        /// that uses Reflection calls directly. It doesn't support indexers.
        /// </summary>
        /// <param name="instance">Object to make the call on</param>
        /// <param name="property">Property to retrieve</param>
        /// <returns>Object - cast to proper type</returns>
        public static object GetProperty(object instance, string property)
        {
            return instance.GetType().GetProperty(property, ReflectionUtils.MemberAccess).GetValue(instance, null);
        }

        /// <summary>
        /// Retrieve a field dynamically from an object. This is a simple implementation that's
        /// straight Reflection and doesn't support indexers.
        /// </summary>
        /// <param name="Object">Object to retreve Field from</param>
        /// <param name="Property">name of the field to retrieve</param>
        /// <returns></returns>
        public static object GetField(object Object, string Property)
        {
            return Object.GetType().GetField(Property, ReflectionUtils.MemberAccess).GetValue(Object);
        }

        /// <summary>
        /// Parses Properties and Fields including Array and Collection references.
        /// Used internally for the 'Ex' Reflection methods.
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="Property"></param>
        /// <returns></returns>
        private static object GetPropertyInternal(object Parent, string Property)
        {
            if (Property == "this" || Property == "me")
                return Parent;

            object result = null;
            string pureProperty = Property;
            string indexes = null;
            bool isArrayOrCollection = false;

            // Deal with Array Property
            if (Property.IndexOf("[") > -1)
            {
                pureProperty = Property.Substring(0, Property.IndexOf("["));
                indexes = Property.Substring(Property.IndexOf("["));
                isArrayOrCollection = true;
            }

            // Get the member
            MemberInfo member = Parent.GetType().GetMember(pureProperty, ReflectionUtils.MemberAccess)[0];
            if (member.MemberType == MemberTypes.Property)
                result = ((PropertyInfo)member).GetValue(Parent, null);
            else
                result = ((FieldInfo)member).GetValue(Parent);

            if (isArrayOrCollection)
            {
                indexes = indexes.Replace("[", string.Empty).Replace("]", string.Empty);

                if (result is Array)
                {
                    int Index = -1;
                    int.TryParse(indexes, out Index);
                    result = CallMethod(result, "GetValue", Index);
                }
                else if (result is ICollection)
                {
                    if (indexes.StartsWith("\""))
                    {
                        // String Index
                        indexes = indexes.Trim('\"');
                        result = CallMethod(result, "get_Item", indexes);
                    }
                    else
                    {
                        // assume numeric index
                        int index = -1;
                        int.TryParse(indexes, out index);
                        result = CallMethod(result, "get_Item", index);
                    }
                }

            }

            return result;
        }


        /// <summary>
        /// Parses Properties and Fields including Array and Collection references.
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="Property"></param>
        /// <returns></returns>
        private static object SetPropertyInternal(object Parent, string Property, object Value)
        {
            if (Property == "this" || Property == "me")
                return Parent;

            object Result = null;
            string PureProperty = Property;
            string Indexes = null;
            bool IsArrayOrCollection = false;

            // Deal with Array Property
            if (Property.IndexOf("[") > -1)
            {
                PureProperty = Property.Substring(0, Property.IndexOf("["));
                Indexes = Property.Substring(Property.IndexOf("["));
                IsArrayOrCollection = true;
            }

            if (!IsArrayOrCollection)
            {
                // Get the member
                MemberInfo Member = Parent.GetType().GetMember(PureProperty, ReflectionUtils.MemberAccess)[0];
                if (Member.MemberType == MemberTypes.Property)
                    ((PropertyInfo)Member).SetValue(Parent, Value, null);
                else
                    ((FieldInfo)Member).SetValue(Parent, Value);
                return null;
            }
            else
            {
                // Get the member
                MemberInfo Member = Parent.GetType().GetMember(PureProperty, ReflectionUtils.MemberAccess)[0];
                if (Member.MemberType == MemberTypes.Property)
                    Result = ((PropertyInfo)Member).GetValue(Parent, null);
                else
                    Result = ((FieldInfo)Member).GetValue(Parent);
            }
            if (IsArrayOrCollection)
            {
                Indexes = Indexes.Replace("[", string.Empty).Replace("]", string.Empty);

                if (Result is Array)
                {
                    int Index = -1;
                    int.TryParse(Indexes, out Index);
                    Result = CallMethod(Result, "SetValue", Value, Index);
                }
                else if (Result is ICollection)
                {
                    if (Indexes.StartsWith("\""))
                    {
                        // String Index
                        Indexes = Indexes.Trim('\"');
                        Result = CallMethod(Result, "set_Item", Indexes, Value);
                    }
                    else
                    {
                        // assume numeric index
                        int Index = -1;
                        int.TryParse(Indexes, out Index);
                        Result = CallMethod(Result, "set_Item", Index, Value);
                    }
                }
            }

            return Result;
        }

        /// <summary>
        /// Returns a property or field value using a base object and sub members including . syntax.
        /// For example, you can access: oCustomer.oData.Company with (this,"oCustomer.oData.Company")
        /// This method also supports indexers in the Property value such as:
        /// Customer.DataSet.Tables["Customers"].Rows[0]
        /// </summary>
        /// <param name="Parent">Parent object to 'start' parsing from. Typically this will be the Page.</param>
        /// <param name="Property">The property to retrieve. Example: 'Customer.Entity.Company'</param>
        /// <returns></returns>
        public static object GetPropertyEx(object Parent, string Property)
        {
            Type type = Parent.GetType();

            int at = Property.IndexOf(".");
            if (at < 0)
            {
                // Complex parse of the property    
                return GetPropertyInternal(Parent, Property);
            }

            // Walk the . syntax - split into current object (Main) and further parsed objects (Subs)
            string main = Property.Substring(0, at);
            string subs = Property.Substring(at + 1);

            // Retrieve the next . section of the property
            object sub = GetPropertyInternal(Parent, main);

            // Now go parse the left over sections
            return GetPropertyEx(sub, subs);
        }

        /// <summary>
        /// Returns a PropertyInfo object for a given dynamically accessed property
        /// 
        /// Property selection can be specified using . syntax ("Address.Street" or "DataTable[0].Rows[1]") hence the <<i>>Ex<</i>> name for this function.
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="Property"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfoEx(object Parent, string Property)
        {
            Type type = Parent.GetType();

            int at = Property.IndexOf(".");
            if (at < 0)
            {
                // Complex parse of the property    
                return GetPropertyInfoInternal(Parent, Property);
            }

            // Walk the . syntax - split into current object (Main) and further parsed objects (Subs)
            string main = Property.Substring(0, at);
            string subs = Property.Substring(at + 1);

            // Retrieve the next . section of the property
            object sub = GetPropertyInternal(Parent, main);

            // Now go parse the left over sections
            return GetPropertyInfoEx(sub, subs);
        }

        /// <summary>
        /// Returns a PropertyInfo structure from an extended Property reference
        /// </summary>
        /// <param name="Parent"></param>
        /// <param name="Property"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfoInternal(object Parent, string Property)
        {
            if (Property == "this" || Property == "me")
                return null;

            string propertyName = Property;

            // Deal with Array Property - strip off array indexer
            if (Property.IndexOf("[") > -1)
                propertyName = Property.Substring(0, Property.IndexOf("["));

            // Get the member
            return Parent.GetType().GetProperty(propertyName, ReflectionUtils.MemberAccess);
        }

        /// <summary>
        /// Sets the property on an object. This is a simple method that uses straight Reflection 
        /// and doesn't support indexers.
        /// </summary>
        /// <param name="obj">Object to set property on</param>
        /// <param name="property">Name of the property to set</param>
        /// <param name="value">value to set it to</param>
        public static void SetProperty(object obj, string property, object value)
        {
            obj.GetType().GetProperty(property, ReflectionUtils.MemberAccess).SetValue(obj, value, null);
        }

        /// <summary>
        /// Sets the field on an object. This is a simple method that uses straight Reflection 
        /// and doesn't support indexers.
        /// </summary>
        /// <param name="obj">Object to set property on</param>
        /// <param name="property">Name of the field to set</param>
        /// <param name="value">value to set it to</param>
        public static void SetField(object obj, string property, object value)
        {
            obj.GetType().GetField(property, ReflectionUtils.MemberAccess).SetValue(obj, value);
        }

        /// <summary>
        /// Sets a value on an object. Supports . syntax for named properties
        /// (ie. Customer.Entity.Company) as well as indexers.
        /// </summary>
        /// <param name="Object Parent">
        /// Object to set the property on.
        /// </param>
        /// <param name="String Property">
        /// Property to set. Can be an object hierarchy with . syntax and can 
        /// include indexers. Examples: Customer.Entity.Company, 
        /// Customer.DataSet.Tables["Customers"].Rows[0]
        /// </param>
        /// <param name="Object Value">
        /// Value to set the property to
        /// </param>
        public static object SetPropertyEx(object parent, string property, object value)
        {
            Type Type = parent.GetType();

            // no more .s - we got our final object
            int lnAt = property.IndexOf(".");
            if (lnAt < 0)
            {
                SetPropertyInternal(parent, property, value);
                return null;
            }

            // Walk the . syntax
            string Main = property.Substring(0, lnAt);
            string Subs = property.Substring(lnAt + 1);

            object Sub = GetPropertyInternal(parent, Main);

            SetPropertyEx(Sub, Subs, value);

            return null;
        }

        /// <summary>
        /// Calls a method on an object dynamically. This version requires explicit
        /// specification of the parameter type signatures.
        /// </summary>
        /// <param name="instance">Instance of object to call method on</param>
        /// <param name="method">The method to call as a stringToTypedValue</param>
        /// <param name="parameterTypes">Specify each of the types for each parameter passed. 
        /// You can also pass null, but you may get errors for ambiguous methods signatures
        /// when null parameters are passed</param>
        /// <param name="parms">any variable number of parameters.</param>        
        /// <returns>object</returns>
        public static object CallMethod(object instance, string method, Type[] parameterTypes, params object[] parms)
        {
            if (parameterTypes == null && parms.Length > 0)
                // Call without explicit parameter types - might cause problems with overloads    
                // occurs when null parameters were passed and we couldn't figure out the parm type
                return instance.GetType().GetMethod(method, ReflectionUtils.MemberAccess | BindingFlags.InvokeMethod).Invoke(instance, parms);
            else
                // Call with parameter types - works only if no null values were passed
                return instance.GetType().GetMethod(method, ReflectionUtils.MemberAccess | BindingFlags.InvokeMethod, null, parameterTypes, null).Invoke(instance, parms);
        }

        /// <summary>
        /// Calls a method on an object dynamically. 
        /// 
        /// This version doesn't require specific parameter signatures to be passed. 
        /// Instead parameter types are inferred based on types passed. Note that if 
        /// you pass a null parameter, type inferrance cannot occur and if overloads
        /// exist the call may fail. if so use the more detailed overload of this method.
        /// </summary> 
        /// <param name="instance">Instance of object to call method on</param>
        /// <param name="method">The method to call as a stringToTypedValue</param>
        /// <param name="parameterTypes">Specify each of the types for each parameter passed. 
        /// You can also pass null, but you may get errors for ambiguous methods signatures
        /// when null parameters are passed</param>
        /// <param name="parms">any variable number of parameters.</param>        
        /// <returns>object</returns>
        public static object CallMethod(object instance, string method, params object[] parms)
        {
            // Pick up parameter types so we can match the method properly
            Type[] parameterTypes = null;
            if (parms != null)
            {
                parameterTypes = new Type[parms.Length];
                for (int x = 0; x < parms.Length; x++)
                {
                    // if we have null parameters we can't determine parameter types - exit
                    if (parms[x] == null)
                    {
                        parameterTypes = null;  // clear out - don't use types        
                        break;
                    }
                    parameterTypes[x] = parms[x].GetType();
                }
            }
            return CallMethod(instance, method, parameterTypes, parms);
        }

        /// <summary>
        /// Calls a method on an object with extended . syntax (object: this Method: Entity.CalculateOrderTotal)
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="method"></param>
        /// <param name="params"></param>
        /// <returns></returns>
        public static object CallMethodEx(object parent, string method, params object[] parms)
        {
            Type Type = parent.GetType();

            // no more .s - we got our final object
            int lnAt = method.IndexOf(".");
            if (lnAt < 0)
            {
                return ReflectionUtils.CallMethod(parent, method, parms);
            }

            // Walk the . syntax
            string Main = method.Substring(0, lnAt);
            string Subs = method.Substring(lnAt + 1);

            object Sub = GetPropertyInternal(parent, Main);

            // Recurse until we get the lowest ref
            return CallMethodEx(Sub, Subs, parms);
        }




        /// <summary>
        /// Creates an instance from a type by calling the parameterless constructor.
        /// 
        /// Note this will not work with COM objects - continue to use the Activator.CreateInstance
        /// for COM objects.
        /// <seealso>Class wwUtils</seealso>
        /// </summary>
        /// <param name="typeToCreate">
        /// The type from which to create an instance.
        /// </param>
        /// <returns>object</returns>
        public static object CreateInstanceFromType(Type typeToCreate, params object[] args)
        {
            if (args == null)
            {
                Type[] Parms = Type.EmptyTypes;
                return typeToCreate.GetConstructor(Parms).Invoke(null);
            }

            return Activator.CreateInstance(typeToCreate, args);
        }




        /// <summary>
        /// Creates an instance of a type based on a string. Assumes that the type's
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateInstanceFromString(string typeName, params object[] args)
        {
            object instance = null;
            Type type = null;

            try
            {
                type = GetTypeFromName(typeName);
                if (type == null)
                    return null;

                instance = Activator.CreateInstance(type, args);
            }
            catch
            {
                return null;
            }

            return instance;
        }

        /// <summary>
        /// Helper routine that looks up a type name and tries to retrieve the
        /// full type reference in the actively executing assemblies.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static Type GetTypeFromName(string typeName)
        {
            Type type = null;

            type = Type.GetType(typeName, false);
            if (type != null)
                return type;

            // try to find manually
            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = ass.GetType(typeName, false);

                if (type != null)
                    break;

            }
            return type;
        }


        /// <summary>
        /// Creates a COM instance from a ProgID. Loads either
        /// Exe or DLL servers.
        /// </summary>
        /// <param name="progId"></param>
        /// <returns></returns>
        public static object CreateComInstance(string progId)
        {
            Type type = Type.GetTypeFromProgID(progId);
            if (type == null)
                return null;

            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Converts a type to string if possible. This method supports an optional culture generically on any value.
        /// It calls the ToString() method on common types and uses a type converter on all other objects
        /// if available
        /// </summary>
        /// <param name="rawValue">The Value or Object to convert to a string</param>
        /// <param name="culture">Culture for numeric and DateTime values</param>
        /// <param name="unsupportedReturn">Return string for unsupported types</param>
        /// <returns>string</returns>
        public static string TypedValueToString(object rawValue, CultureInfo culture = null, string unsupportedReturn = null)
        {
            if (rawValue == null)
                return string.Empty;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            Type valueType = rawValue.GetType();
            string returnValue = null;

            if (valueType == typeof(string))
                returnValue = rawValue as string;
            else if (valueType == typeof(int) || valueType == typeof(decimal) ||
                valueType == typeof(double) || valueType == typeof(float) || valueType == typeof(Single))
                returnValue = string.Format(culture.NumberFormat, "{0}", rawValue);
            else if (valueType == typeof(DateTime))
                returnValue = string.Format(culture.DateTimeFormat, "{0}", rawValue);
            else if (valueType == typeof(bool) || valueType == typeof(Byte) || valueType.IsEnum)
                returnValue = rawValue.ToString();
            else if (valueType == typeof(Guid?))
            {
                if (rawValue == null)
                    returnValue = string.Empty;
                else
                    return rawValue.ToString();
            }
            else
            {
                // Any type that supports a type converter
                TypeConverter converter = TypeDescriptor.GetConverter(valueType);
                if (converter != null && converter.CanConvertTo(typeof(string)) && converter.CanConvertFrom(typeof(string)))
                    returnValue = converter.ConvertToString(null, culture, rawValue);
                else
                {
                    // Last resort - just call ToString() on unknown type
                    if (!string.IsNullOrEmpty(unsupportedReturn))
                        returnValue = unsupportedReturn;
                    else
                        returnValue = rawValue.ToString();
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Turns a string into a typed value generically.
        /// Explicitly assigns common types and falls back
        /// on using type converters for unhandled types.         
        /// 
        /// Common uses: 
        /// * UI -&gt; to data conversions
        /// * Parsers
        /// <seealso>Class ReflectionUtils</seealso>
        /// </summary>
        /// <param name="sourceString">
        /// The string to convert from
        /// </param>
        /// <param name="targetType">
        /// The type to convert to
        /// </param>
        /// <param name="culture">
        /// Culture used for numeric and datetime values.
        /// </param>
        /// <returns>object. Throws exception if it cannot be converted.</returns>
        public static object StringToTypedValue(string sourceString, Type targetType, CultureInfo culture = null)
        {
            object result = null;

            bool isEmpty = string.IsNullOrEmpty(sourceString);

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (targetType == typeof(string))
                result = sourceString;
            else if (targetType == typeof(Int32) || targetType == typeof(int))
            {
                if (isEmpty)
                    result = 0;
                else
                    result = Int32.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(Int64))
            {
                if (isEmpty)
                    result = (Int64)0;
                else
                    result = Int64.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(Int16))
            {
                if (isEmpty)
                    result = (Int16)0;
                else
                    result = Int16.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(decimal))
            {
                if (isEmpty)
                    result = 0M;
                else
                    result = decimal.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(DateTime))
            {
                if (isEmpty)
                    result = DateTime.MinValue;
                else
                    result = Convert.ToDateTime(sourceString, culture.DateTimeFormat);
            }
            else if (targetType == typeof(byte))
            {
                if (isEmpty)
                    result = 0;
                else
                    result = Convert.ToByte(sourceString);
            }
            else if (targetType == typeof(double))
            {
                if (isEmpty)
                    result = 0F;
                else
                    result = Double.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(Single))
            {
                if (isEmpty)
                    result = 0F;
                else
                    result = Single.Parse(sourceString, NumberStyles.Any, culture.NumberFormat);
            }
            else if (targetType == typeof(bool))
            {
                if (!isEmpty &&
                    sourceString.ToLower() == "true" || sourceString.ToLower() == "on" || sourceString == "1")
                    result = true;
                else
                    result = false;
            }
            else if (targetType == typeof(Guid))
            {
                if (isEmpty)
                    result = Guid.Empty;
                else
                    result = new Guid(sourceString);
            }
            else if (targetType.IsEnum)
                result = Enum.Parse(targetType, sourceString);
            else if (targetType == typeof(byte[]))
            {
                // TODO: Convert HexBinary string to byte array
                result = null;
            }

            // Handle nullables explicitly since type converter won't handle conversions
            // properly for things like decimal separators currency formats etc.
            // Grab underlying type and pass value to that
            else if (targetType.Name.StartsWith("Nullable`"))
            {
                if (sourceString.ToLower() == "null" || sourceString == string.Empty)
                    result = null;
                else
                {
                    targetType = Nullable.GetUnderlyingType(targetType);
                    result = StringToTypedValue(sourceString, targetType);
                }
            }
            else
            {
                TypeConverter converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null && converter.CanConvertFrom(typeof(string)))
                    result = converter.ConvertFromString(null, culture, sourceString);
                else
                {
                    Debug.Assert(false, string.Format("Type Conversion not handled in StringToTypedValue for {0} {1}",
                                                        targetType.Name, sourceString));
                    throw (new InvalidCastException( targetType.Name));
                }
            }

            return result;
        }



        /// <summary>
        /// Generic version allow for automatic type conversion without the explicit type
        /// parameter
        /// </summary>
        /// <typeparam name="T">Type to be converted to</typeparam>
        /// <param name="sourceString">input string value to be converted</param>
        /// <param name="culture">Culture applied to conversion</param>
        /// <returns></returns>
        public static T StringToTypedValue<T>(string sourceString, CultureInfo culture = null)
        {
            return (T)StringToTypedValue(sourceString, typeof(T), culture);
        }


        /// <summary>
        /// Returns a List of KeyValuePair object
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> GetEnumList(Type enumType, bool valueAsFieldValueNumber = false)
        {
            //string[] enumStrings = Enum.GetNames(enumType);
            Array enumValues = Enum.GetValues(enumType);
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

            foreach (var enumValue in enumValues)
            {
                var strValue = enumValue.ToString();

                if (!valueAsFieldValueNumber)
                    items.Add(new KeyValuePair<string, string>(enumValue.ToString(), StringUtils.FromCamelCase(strValue)));
                else
                    items.Add(new KeyValuePair<string, string>(((int)enumValue).ToString(),
                                                                 StringUtils.FromCamelCase(strValue)
                                                              ));
            }
            return items;
        }


        /// <summary>
        /// Retrieves a value from  a static property by specifying a type full name and property
        /// </summary>
        /// <param name="typeName">Full type name (namespace.class)</param>
        /// <param name="property">Property to get value from</param>
        /// <returns></returns>
        public static object GetStaticProperty(string typeName, string property)
        {
            Type type = GetTypeFromName(typeName);
            if (type == null)
                return null;

            return GetStaticProperty(type, property);
        }

        /// <summary>
        /// Returns a static property from a given type
        /// </summary>
        /// <param name="type">Type instance for the static property</param>
        /// <param name="property">Property name as a string</param>
        /// <returns></returns>
        public static object GetStaticProperty(Type type, string property)
        {
            object result = null;
            try
            {
                result = type.InvokeMember(property, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty, null, type, null);
            }
            catch
            {
                return null;
            }

            return result;
        }

        #region COM Reflection Routines

        /// <summary>
        /// Retrieve a dynamic 'non-typelib' property
        /// </summary>
        /// <param name="instance">Object to make the call on</param>
        /// <param name="property">Property to retrieve</param>
        /// <returns></returns>
        public static object GetPropertyCom(object instance, string property)
        {
            return instance.GetType().InvokeMember(property, ReflectionUtils.MemberAccess | BindingFlags.GetProperty | BindingFlags.GetField, null,
                                                instance, null);
        }


        /// <summary>
        /// Returns a property or field value using a base object and sub members including . syntax.
        /// For example, you can access: oCustomer.oData.Company with (this,"oCustomer.oData.Company")
        /// </summary>
        /// <param name="parent">Parent object to 'start' parsing from.</param>
        /// <param name="property">The property to retrieve. Example: 'oBus.oData.Company'</param>
        /// <returns></returns>
        public static object GetPropertyExCom(object parent, string property)
        {

            Type Type = parent.GetType();

            int lnAt = property.IndexOf(".");
            if (lnAt < 0)
            {
                if (property == "this" || property == "me")
                    return parent;

                // Get the member
                return parent.GetType().InvokeMember(property, ReflectionUtils.MemberAccess | BindingFlags.GetProperty | BindingFlags.GetField, null,
                    parent, null);
            }

            // Walk the . syntax - split into current object (Main) and further parsed objects (Subs)
            string Main = property.Substring(0, lnAt);
            string Subs = property.Substring(lnAt + 1);

            object Sub = parent.GetType().InvokeMember(Main, ReflectionUtils.MemberAccess | BindingFlags.GetProperty | BindingFlags.GetField, null,
                parent, null);

            // Recurse further into the sub-properties (Subs)
            return ReflectionUtils.GetPropertyExCom(Sub, Subs);
        }

        /// <summary>
        /// Sets the property on an object.
        /// </summary>
        /// <param name="Object">Object to set property on</param>
        /// <param name="Property">Name of the property to set</param>
        /// <param name="Value">value to set it to</param>
        public static void SetPropertyCom(object Object, string Property, object Value)
        {
            Object.GetType().InvokeMember(Property, ReflectionUtils.MemberAccess | BindingFlags.SetProperty | BindingFlags.SetField, null, Object, new object[1] { Value });
        }

        /// <summary>
        /// Sets the value of a field or property via Reflection. This method alws 
        /// for using '.' syntax to specify objects multiple levels down.
        /// 
        /// ReflectionUtils.SetPropertyEx(this,"Invoice.LineItemsCount",10)
        /// 
        /// which would be equivalent of:
        /// 
        /// Invoice.LineItemsCount = 10;
        /// </summary>
        /// <param name="Object Parent">
        /// Object to set the property on.
        /// </param>
        /// <param name="String Property">
        /// Property to set. Can be an object hierarchy with . syntax.
        /// </param>
        /// <param name="Object Value">
        /// Value to set the property to
        /// </param>
        public static object SetPropertyExCom(object parent, string property, object value)
        {
            Type Type = parent.GetType();

            int lnAt = property.IndexOf(".");
            if (lnAt < 0)
            {
                // Set the member
                parent.GetType().InvokeMember(property, ReflectionUtils.MemberAccess | BindingFlags.SetProperty | BindingFlags.SetField, null,
                    parent, new object[1] { value });

                return null;
            }

            // Walk the . syntax - split into current object (Main) and further parsed objects (Subs)
            string Main = property.Substring(0, lnAt);
            string Subs = property.Substring(lnAt + 1);


            object Sub = parent.GetType().InvokeMember(Main, ReflectionUtils.MemberAccess | BindingFlags.GetProperty | BindingFlags.GetField, null,
                parent, null);

            return SetPropertyExCom(Sub, Subs, value);
        }


        /// <summary>
        /// Wrapper method to call a 'dynamic' (non-typelib) method
        /// on a COM object
        /// </summary>
        /// <param name="params"></param>
        /// 1st - Method name, 2nd - 1st parameter, 3rd - 2nd parm etc.
        /// <returns></returns>
        public static object CallMethodCom(object instance, string method, params object[] parms)
        {
            return instance.GetType().InvokeMember(method, ReflectionUtils.MemberAccess | BindingFlags.InvokeMethod, null, instance, parms);
        }

        /// <summary>
        /// Calls a method on a COM object with '.' syntax (Customer instance and Address.DoSomeThing method)
        /// </summary>
        /// <param name="parent">the object instance on which to call method</param>
        /// <param name="method">The method or . syntax path to the method (Address.Parse)</param>
        /// <param name="parms">Any number of parameters</param>
        /// <returns></returns>
        public static object CallMethodExCom(object parent, string method, params object[] parms)
        {
            Type Type = parent.GetType();

            // no more .s - we got our final object
            int at = method.IndexOf(".");
            if (at < 0)
            {
                return ReflectionUtils.CallMethod(parent, method, parms);
            }

            // Walk the . syntax - split into current object (Main) and further parsed objects (Subs)
            string Main = method.Substring(0, at);
            string Subs = method.Substring(at + 1);

            object Sub = parent.GetType().InvokeMember(Main, ReflectionUtils.MemberAccess | BindingFlags.GetProperty | BindingFlags.GetField, null,
                parent, null);

            // Recurse until we get the lowest ref
            return CallMethodEx(Sub, Subs, parms);
        }
        #endregion

    }
    /// <summary>
    /// String utility class that provides a host of string related operations
    /// </summary>
    public static class StringUtils
    {

        /// <summary>
        /// Extracts a string from between a pair of delimiters. Only the first 
        /// instance is found.
        /// </summary>
        /// <param name="source">Input String to work on</param>
        /// <param name="StartDelim">Beginning delimiter</param>
        /// <param name="endDelim">ending delimiter</param>
        /// <param name="CaseInsensitive">Determines whether the search for delimiters is case sensitive</param>
        /// <returns>Extracted string or ""</returns>
        public static string ExtractString(string source,
                                           string beginDelim,
                                           string endDelim,
                                           bool caseSensitive = false,
                                           bool allowMissingEndDelimiter = false,
                                           bool returnDelimiters = false)
        {
            int at1, at2;

            if (string.IsNullOrEmpty(source))
                return string.Empty;

            if (caseSensitive)
            {
                at1 = source.IndexOf(beginDelim);
                if (at1 == -1)
                    return string.Empty;

                if (!returnDelimiters)
                    at2 = source.IndexOf(endDelim, at1 + beginDelim.Length);
                else
                    at2 = source.IndexOf(endDelim, at1);
            }
            else
            {
                //string Lower = source.ToLower();
                at1 = source.IndexOf(beginDelim, 0, source.Length, StringComparison.OrdinalIgnoreCase);
                if (at1 == -1)
                    return string.Empty;

                if (!returnDelimiters)
                    at2 = source.IndexOf(endDelim, at1 + beginDelim.Length, StringComparison.OrdinalIgnoreCase);
                else
                    at2 = source.IndexOf(endDelim, at1, StringComparison.OrdinalIgnoreCase);
            }

            if (allowMissingEndDelimiter && at2 == -1)
                return source.Substring(at1 + beginDelim.Length);

            if (at1 > -1 && at2 > 1)
            {
                if (!returnDelimiters)
                    return source.Substring(at1 + beginDelim.Length, at2 - at1 - beginDelim.Length);
                else
                    return source.Substring(at1, at2 - at1 + endDelim.Length);
            }

            return string.Empty;
        }



        /// <summary>
        /// String replace function that support
        /// </summary>
        /// <param name="origString">Original input string</param>
        /// <param name="findString">The string that is to be replaced</param>
        /// <param name="replaceWith">The replacement string</param>
        /// <param name="instance">Instance of the FindString that is to be found. if Instance = -1 all are replaced</param>
        /// <param name="caseInsensitive">Case insensitivity flag</param>
        /// <returns>updated string or original string if no matches</returns>
        public static string ReplaceStringInstance(string origString, string findString,
                                                   string replaceWith, int instance,
                                                   bool caseInsensitive)
        {
            if (instance == -1)
                return ReplaceString(origString, findString, replaceWith, caseInsensitive);

            int at1 = 0;
            for (int x = 0; x < instance; x++)
            {

                if (caseInsensitive)
                    at1 = origString.IndexOf(findString, at1, origString.Length - at1, StringComparison.OrdinalIgnoreCase);
                else
                    at1 = origString.IndexOf(findString, at1);

                if (at1 == -1)
                    return origString;

                if (x < instance - 1)
                    at1 += findString.Length;
            }

            return origString.Substring(0, at1) + replaceWith + origString.Substring(at1 + findString.Length);
        }

        /// <summary>
        /// Replaces a substring within a string with another substring with optional case sensitivity turned off.
        /// </summary>
        /// <param name="origString">String to do replacements on</param>
        /// <param name="findString">The string to find</param>
        /// <param name="replaceString">The string to replace found string wiht</param>
        /// <param name="caseInsensitive">If true case insensitive search is performed</param>
        /// <returns>updated string or original string if no matches</returns>
        public static string ReplaceString(string origString, string findString,
                                           string replaceString, bool caseInsensitive)
        {
            int at1 = 0;
            while (true)
            {
                if (caseInsensitive)
                    at1 = origString.IndexOf(findString, at1, origString.Length - at1, StringComparison.OrdinalIgnoreCase);
                else
                    at1 = origString.IndexOf(findString, at1);

                if (at1 == -1)
                    break;

                origString = origString.Substring(0, at1) + replaceString + origString.Substring(at1 + findString.Length);

                at1 += replaceString.Length;
            }

            return origString;
        }



        /// <summary>
        /// Trims a sub string from a string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textToTrim"></param>
        /// <returns></returns>
        public static string TrimStart(string text, string textToTrim, bool caseInsensitive)
        {
            while (true)
            {
                string match = text.Substring(0, textToTrim.Length);

                if (match == textToTrim ||
                    (caseInsensitive && match.ToLower() == textToTrim.ToLower()))
                {
                    if (text.Length <= match.Length)
                        text = "";
                    else
                        text = text.Substring(textToTrim.Length);
                }
                else
                    break;
            }
            return text;
        }

        /// <summary>
        /// Replicates an input string n number of times
        /// </summary>
        /// <param name="input"></param>
        /// <param name="charCount"></param>
        /// <returns></returns>
        public static string Replicate(string input, int charCount)
        {
            return new StringBuilder().Insert(0, "input", charCount).ToString();
        }

        /// <summary>
        /// Replicates a character n number of times and returns a string
        /// </summary>
        /// <param name="charCount"></param>
        /// <param name="character"></param>
        /// <returns></returns>
        public static string Replicate(char character, int charCount)
        {
            return new string(character, charCount);
        }

        /// <summary>
        /// Determines if a string is contained in a list of other strings
        /// </summary>
        /// <param name="s"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool Inlist(string s, params string[] list)
        {
            return list.Contains(s);
        }

        /// <summary>
        /// Return a string in proper Case format
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public static string ProperCase(string Input)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Input);
        }

        /// <summary>
        /// Takes a phrase and turns it into CamelCase text.
        /// White Space, punctuation and separators are stripped
        /// </summary>
        /// <param name="?"></param>
        public static string ToCamelCase(string phrase)
        {
            if (phrase == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder(phrase.Length);

            // First letter is always upper case
            bool nextUpper = true;

            foreach (char ch in phrase)
            {
                if (char.IsWhiteSpace(ch) || char.IsPunctuation(ch) || char.IsSeparator(ch))
                {
                    nextUpper = true;
                    continue;
                }

                if (nextUpper)
                    sb.Append(char.ToUpper(ch));
                else
                    sb.Append(char.ToLower(ch));

                nextUpper = false;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Tries to create a phrase string from CamelCase text.
        /// Will place spaces before capitalized letters.
        /// 
        /// Note that this method may not work for round tripping 
        /// ToCamelCase calls, since ToCamelCase strips more characters
        /// than just spaces.
        /// </summary>
        /// <param name="camelCase"></param>
        /// <returns></returns>
        public static string FromCamelCase(string camelCase)
        {
            if (camelCase == null)
                throw new ArgumentException("Null is not allowed for StringUtils.FromCamelCase");

            StringBuilder sb = new StringBuilder(camelCase.Length + 10);
            bool first = true;
            char lastChar = '\0';

            foreach (char ch in camelCase)
            {
                if (!first &&
                     (char.IsUpper(ch) ||
                       char.IsDigit(ch) && !char.IsDigit(lastChar)))
                    sb.Append(' ');

                sb.Append(ch);
                first = false;
                lastChar = ch;
            }

            return sb.ToString(); ;
        }

        /// <summary>
        /// Terminates a string with the given end string/character, but only if the
        /// value specified doesn't already exist and the string is not empty.
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string TerminateString(string value, string terminator)
        {
            if (string.IsNullOrEmpty(value) || value.EndsWith(terminator))
                return value;

            return value + terminator;
        }

        /// <summary>
        /// Trims a string to a specific number of max characters
        /// </summary>
        /// <param name="value"></param>
        /// <param name="charCount"></param>
        /// <returns></returns>
        public static string TrimTo(string value, int charCount)
        {
            if (value == null)
                return string.Empty;

            if (value.Length > charCount)
                return value.Substring(0, charCount);

            return value;
        }

        /// <summary>
        /// Strips any common white space from all lines of text that have the same
        /// common white space text. Effectively removes common code indentation from
        /// code blocks for example so you can get a left aligned code snippet.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string NormalizeIndentation(string code)
        {
            // normalize tabs to 3 spaces
            string text = code.Replace("\t", "   ");

            string[] lines = text.Split(new string[3] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // keep track of the smallest indent
            int minPadding = 1000;

            foreach (var line in lines)
            {
                if (line.Length == 0)  // ignore blank lines
                    continue;

                int count = 0;
                foreach (char chr in line)
                {
                    if (chr == ' ' && count < minPadding)
                        count++;
                    else
                        break;
                }
                if (count == 0)
                    return code;

                minPadding = count;
            }

            string strip = new String(' ', minPadding);

            StringBuilder sb = new StringBuilder();
            foreach (var line in lines)
            {
                sb.AppendLine(StringUtils.ReplaceStringInstance(line, strip, "", 1, false));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Converts a string into bytes for storage in any byte[] type
        /// buffer or stream format (like MemoryStream).
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding">The character encoding to use. Defaults to Unicode</param>
        /// <returns></returns>
        public static byte[] StringToBytes(string text, Encoding encoding = null)
        {
            if (text == null)
                return null;

            if (encoding == null)
                encoding = Encoding.Unicode;

            return encoding.GetBytes(text);
        }

        /// <summary>
        /// Converts a byte array to a stringUtils
        /// </summary>
        /// <param name="buffer">raw string byte data</param>
        /// <param name="encoding">Character encoding to use. Defaults to Unicode</param>
        /// <returns></returns>
        public static string BytesToString(byte[] buffer, Encoding encoding = null)
        {
            if (buffer == null)
                return null;

            if (encoding == null)
                encoding = Encoding.Unicode;

            return encoding.GetString(buffer);
        }

        /// <summary>
        /// Returns an abstract of the provided text by returning up to Length characters
        /// of a text string. If the text is truncated a ... is appended.
        /// </summary>
        /// <param name="text">Text to abstract</param>
        /// <param name="length">Number of characters to abstract to</param>
        /// <returns>string</returns>
        public static string TextAbstract(string text, int length)
        {
            if (text == null)
                return string.Empty;

            if (text.Length <= length)
                return text;

            text = text.Substring(0, length);

            text = text.Substring(0, text.LastIndexOf(" "));
            return text + "...";
        }


        /// <summary>
        /// Simple Logging method that allows quickly writing a string to a file
        /// </summary>
        /// <param name="output"></param>
        /// <param name="filename"></param>
        public static void LogString(string output, string filename)
        {
            StreamWriter Writer = new StreamWriter(filename, true);
            Writer.WriteLine(DateTime.Now.ToString() + " - " + output);
            Writer.Close();
        }

        /// <summary>
        /// Creates short string id based on a GUID hashcode.
        /// Not guaranteed to be unique across machines, but unlikely
        /// to duplicate in medium volume situations.
        /// </summary>
        /// <returns></returns>
        public static string NewStringId()
        {
            return Guid.NewGuid().ToString().GetHashCode().ToString("x");
        }

        /// <summary>
        /// Creates a new random string of upper, lower case letters and digits.
        /// Very useful for generating random data for storage in test data.
        /// </summary>
        /// <param name="size">The number of characters of the string to generate</param>
        /// <returns>randomized string</returns>
        public static string RandomString(int size, bool includeNumbers = false)
        {
            StringBuilder builder = new StringBuilder(size);
            char ch;
            int num;

            for (int i = 0; i < size; i++)
            {
                if (includeNumbers)
                    num = Convert.ToInt32(Math.Floor(62 * random.NextDouble()));
                else
                    num = Convert.ToInt32(Math.Floor(52 * random.NextDouble()));

                if (num < 26)
                    ch = Convert.ToChar(num + 65);
                // lower case
                else if (num > 25 && num < 52)
                    ch = Convert.ToChar(num - 26 + 97);
                // numbers
                else
                    ch = Convert.ToChar(num - 52 + 48);

                builder.Append(ch);
            }

            return builder.ToString();
        }
        private static Random random = new Random((int)DateTime.Now.Ticks);


        /// <summary>
        /// Parses an string into an integer. If the value can't be parsed
        /// a default value is returned instead
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public static int ParseInt(string input, int defaultValue, IFormatProvider numberFormat)
        {
            int val = defaultValue;
            int.TryParse(input, NumberStyles.Any, numberFormat, out val);
            return val;
        }

        /// <summary>
        /// Parses an string into an integer. If the value can't be parsed
        /// a default value is returned instead
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ParseInt(string input, int defaultValue)
        {
            return ParseInt(input, defaultValue, CultureInfo.CurrentCulture.NumberFormat);
        }

        /// <summary>
        /// Parses an string into an decimal. If the value can't be parsed
        /// a default value is returned instead
        /// </summary>
        /// <param name="input"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ParseDecimal(string input, decimal defaultValue, IFormatProvider numberFormat)
        {
            decimal val = defaultValue;
            decimal.TryParse(input, NumberStyles.Any, numberFormat, out val);
            return val;
        }

        /// <summary>
        /// Strips all non digit values from a string and only
        /// returns the numeric string.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StripNonNumber(string input)
        {
            var chars = input.ToCharArray();
            StringBuilder sb = new StringBuilder();
            foreach (var chr in chars)
            {
                if (char.IsNumber(chr) || char.IsSeparator(chr))
                    sb.Append(chr);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Creates a Stream from a string. Internally creates
        /// a memory stream and returns that.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static Stream StringToStream(string text, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;

            MemoryStream ms = new MemoryStream(text.Length * 2);
            byte[] data = encoding.GetBytes(text);
            ms.Write(data, 0, data.Length);
            ms.Position = 0;
            return ms;
        }



        /// <summary>
        /// Turns a BinHex string that contains raw byte values
        /// into a byte array
        /// </summary>
        /// <param name="hex">BinHex string (just two byte hex digits strung together)</param>
        /// <returns></returns>
        public static byte[] BinHexToString(string hex)
        {
            int offset = hex.StartsWith("0x") ? 2 : 0;
            if ((hex.Length % 2) != 0)
                throw new ArgumentException(String.Format("{0}", hex.Length));

            byte[] ret = new byte[(hex.Length - offset) / 2];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (byte)((ParseHexChar(hex[offset]) << 4)
                                 | ParseHexChar(hex[offset + 1]));
                offset += 2;
            }
            return ret;
        }

        static int ParseHexChar(char c)
        {
            if (c >= '0' && c <= '9')
                return c - '0';
            if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;
            if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;

            throw new ArgumentException("" + c);
        }

        /// <summary>
        /// Retrieves a value from an XML-like string
        /// </summary>
        /// <param name="propertyString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetProperty(string propertyString, string key)
        {
            return StringUtils.ExtractString(propertyString, "<" + key + ">", "</" + key + ">");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyString"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetProperty(string propertyString, string key, string value)
        {
            string extract = StringUtils.ExtractString(propertyString, "<" + key + ">", "</" + key + ">");

            if (string.IsNullOrEmpty(value) && extract != string.Empty)
            {
                return propertyString.Replace(extract, "");
            }

            string xmlLine = "<" + key + ">" + value + "</" + key + ">";

            // replace existing
            if (extract != string.Empty)
                return propertyString.Replace(extract, xmlLine);

            // add new
            return propertyString + xmlLine + "\r\n";
        }

        #region UrlEncoding and UrlDecoding without System.Web
        /// <summary>
        /// UrlEncodes a string without the requirement for System.Web
        /// </summary>
        /// <param name="String"></param>
        /// <returns></returns>
        // [Obsolete("Use System.Uri.EscapeDataString instead")]
        public static string UrlEncode(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // Sytem.Uri provides reliable parsing
            return System.Uri.EscapeDataString(text);
        }

        /// <summary>
        /// UrlDecodes a string without requiring System.Web
        /// </summary>
        /// <param name="text">String to decode.</param>
        /// <returns>decoded string</returns>
        public static string UrlDecode(string text)
        {
            // pre-process for + sign space formatting since System.Uri doesn't handle it
            // plus literals are encoded as %2b normally so this should be safe
            text = text.Replace("+", " ");
            string decoded = System.Uri.UnescapeDataString(text);
            return decoded;
        }

        /// <summary>
        /// Retrieves a value by key from a UrlEncoded string.
        /// </summary>
        /// <param name="urlEncoded">UrlEncoded String</param>
        /// <param name="key">Key to retrieve value for</param>
        /// <returns>returns the value or "" if the key is not found or the value is blank</returns>
        public static string GetUrlEncodedKey(string urlEncoded, string key)
        {
            urlEncoded = "&" + urlEncoded + "&";

            int Index = urlEncoded.IndexOf("&" + key + "=", StringComparison.OrdinalIgnoreCase);
            if (Index < 0)
                return string.Empty;

            int lnStart = Index + 2 + key.Length;

            int Index2 = urlEncoded.IndexOf("&", lnStart);
            if (Index2 < 0)
                return string.Empty;

            return UrlDecode(urlEncoded.Substring(lnStart, Index2 - lnStart));
        }

        /// <summary>
        /// Allows setting of a value in a UrlEncoded string. If the key doesn't exist
        /// a new one is set, if it exists it's replaced with the new value.
        /// </summary>
        /// <param name="urlEncoded">A UrlEncoded string of key value pairs</param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SetUrlEncodedKey(string urlEncoded, string key, string value)
        {
            if (!urlEncoded.EndsWith("?") && !urlEncoded.EndsWith("&"))
                urlEncoded += "&";

            Match match = Regex.Match(urlEncoded, "[?|&]" + key + "=.*?&");

            if (match == null || string.IsNullOrEmpty(match.Value))
                urlEncoded = urlEncoded + key + "=" + UrlEncode(value) + "&";
            else
                urlEncoded = urlEncoded.Replace(match.Value, match.Value.Substring(0, 1) + key + "=" + UrlEncode(value) + "&");

            return urlEncoded.TrimEnd('&');
        }



        static char[] base36CharArray = "0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();
        static string base36Chars = "0123456789abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Encodes an integer into a string by mapping to alpha and digits (36 chars)
        /// chars are embedded as lower case
        /// 
        /// Example: 4zx12ss
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Base36Encode(long value)
        {
            string returnValue = "";
            bool isNegative = value < 0;
            if (isNegative)
                value = value * -1;

            do
            {
                returnValue = base36CharArray[value % base36CharArray.Length] + returnValue;
                value /= 36;
            } while (value != 0);

            return isNegative ? returnValue + "-" : returnValue;
        }

        /// <summary>
        /// Decodes a base36 encoded string to an integer
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static long Base36Decode(string input)
        {
            bool isNegative = false;
            if (input.EndsWith("-"))
            {
                isNegative = true;
                input = input.Substring(0, input.Length - 1);
            }

            char[] arrInput = input.ToCharArray();
            Array.Reverse(arrInput);
            long returnValue = 0;
            for (long i = 0; i < arrInput.Length; i++)
            {
                long valueindex = base36Chars.IndexOf(arrInput[i]);
                returnValue += Convert.ToInt64(valueindex * Math.Pow(36, i));
            }
            return isNegative ? returnValue * -1 : returnValue;
        }


        #endregion

        #region Obsolete
        /// <summary>
        /// Determines whether a string is empty (null or zero length)
        /// </summary>
        /// <param name="text">Input string</param>
        /// <returns>true or false</returns>
        [Obsolete("Use string.IsNullOrEmpty() instead")]
        public static bool Empty(string text)
        {
            return string.IsNullOrEmpty(text);
        }

        /// <summary>
        /// Determines wheter a string is empty (null or zero length)
        /// </summary>
        /// <param name="text">Input string (in object format)</param>
        /// <returns>true or false/returns>        
        [Obsolete("Use string.IsNullOrEmpty() instead")]
        public static bool Empty(object text)
        {
            return string.IsNullOrEmpty(text as string);
        }

        #endregion
    }
}
