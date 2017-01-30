using System;
using System.Xml;
using System.Xml.Schema;
using System.Text.RegularExpressions;

namespace XSDValidator
{
    public class Program
    {
        public class XmlSchemaSetExample
        {
            public static void Main()
            {
                const string fileLocation = @"\books.xsd";

                var doc1 = new XmlDocument();
                doc1.Load(fileLocation);
                var nameSpaceReader = XmlReader.Create(fileLocation);
                nameSpaceReader.MoveToContent();
                var NameSpace1 = nameSpaceReader.GetAttribute("targetNamespace");

                var booksSettings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
                booksSettings.ValidationEventHandler += booksSettingsValidationEventHandler;

                var schemaElement = doc1.DocumentElement;
                foreach (XmlNode ele in schemaElement.ChildNodes)
                {
                    foreach (XmlAttribute attr in ele.Attributes)
                    {
                        if (Regex.IsMatch(attr.Value, @"\s+$") || Regex.IsMatch(attr.Value, @"\A\s+"))
                        {
                            Console.WriteLine("There is a space in the front or back of a root element at the parent level. When that is fixed resubmit Schema ");
                            Console.ReadLine();
                        }
                    }
                }

                try
                {
                    booksSettings.Schemas.Add(NameSpace1, fileLocation);
                    XmlReader.Create(fileLocation, booksSettings);
                    Console.WriteLine("Your XSD is valid");
                    Console.ReadLine();
                }
                catch (XmlSchemaValidationException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("The error is on line " + e.LineNumber + " and position " + e.LinePosition);
                    Console.ReadLine();
                }
                catch (XmlSchemaException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("The error is on line " + e.LineNumber + " and position " + e.LinePosition);
                    Console.ReadLine();
                }
                catch (XmlException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }
            }

            static void booksSettingsValidationEventHandler(object sender, ValidationEventArgs e)
            {
                switch (e.Severity)
                {
                    case XmlSeverityType.Warning:
                        Console.Write("WARNING: ");
                        Console.WriteLine(e.Message, e.Exception.LineNumber, e.Exception.LinePosition);
                        Console.ReadLine();
                        break;
                    case XmlSeverityType.Error:
                        Console.Write("ERROR: ");
                        Console.WriteLine(e.Message, e.Exception.LineNumber, e.Exception.LinePosition);

                        Console.ReadLine();
                        break;
                }
            }
        }
    }
}