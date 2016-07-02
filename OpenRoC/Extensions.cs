﻿namespace oroc
{
    using System.IO;
    using System.Xml;
    using System.Drawing;
    using System.Xml.Linq;
    using System.Reflection;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using System.Diagnostics.CodeAnalysis;

    public static class Extensions
    {
        public static void SetDoubleBuffered(this Control control, bool enable)
        {
            PropertyInfo doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            if (doubleBufferPropertyInfo != null) doubleBufferPropertyInfo.SetValue(control, enable, null);
        }

        // http://stackoverflow.com/a/3839419/388751
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static string ToXmlNodeString<T>(this T self)
        {
            XmlSerializerNamespaces serializer_namespace = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            XmlWriterSettings serializer_settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringWriter string_writer = new StringWriter())
            using (XmlWriter xml_writer = XmlWriter.Create(string_writer, serializer_settings))
            {
                serializer.Serialize(xml_writer, self, serializer_namespace);
                return string_writer.ToString();
            }
        }

        // http://stackoverflow.com/a/3839419/388751
        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static T FromXmlNodeString<T>(string node, string root)
        {
            XmlReaderSettings serializer_settings = new XmlReaderSettings { ValidationType = ValidationType.None };
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(root));

            using (StringReader string_reader = new StringReader(node))
            using (XmlReader xml_reader = XmlReader.Create(string_reader, serializer_settings))
            {
                return (T)serializer.Deserialize(xml_reader);
            }
        }

        public static XmlElement AsXmlElement(this XElement el)
        {
            var doc = new XmlDocument();
            doc.Load(el.CreateReader());
            return doc.DocumentElement;
        }

        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
