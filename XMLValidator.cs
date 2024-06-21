using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Xunit;


namespace ValidateXml
{
    public static class Program
    {
        public static void Main() // might be an issue with brackets stopping this from compiling
        {
        }
        [Fact]
        public static void Validator_OnValidateWithCorrectXML_ReturnsSuccess()
        {
            // should do onetimesetup but low on time
            var sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var sFile = System.IO.Path.Combine(sCurrentDirectory, @"../../../EsmaSample.xml");
            var sFilePath = Path.GetFullPath(sFile);
            XmlDocument doc = new XmlDocument();
            doc.Load(sFilePath);

            var schemaPath = System.IO.Path.Combine(sCurrentDirectory, @"../../../DRAFT1auth.098.001.04_1.3.0.xsd");
            var schemaFullPath = Path.GetFullPath(schemaPath);

            XmlSchemaSet schemas = new XmlSchemaSet();
            using (var schemaReader = new XmlTextReader(schemaFullPath))
            {
                XmlSchema schema = XmlSchema.Read(schemaReader, XmlValidator.OnValidationEvent);
                schemas.Add(schema);
            }

            var validator = new XmlValidator();
            Assert.True(validator.Validate(doc, schemas));
        }
        [Fact]
        public static void Validator_OnValidateWithInCorrectXML_ReturnsFalse()
        {
            var sCurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var sFile = System.IO.Path.Combine(sCurrentDirectory, @"../../../EsmaSample.xml");
            var sFilePath = Path.GetFullPath(sFile);
            XmlDocument doc = new XmlDocument();
            doc.Load(sFilePath);

            var schemaPath = System.IO.Path.Combine(sCurrentDirectory, @"../../../DRAFT1auth.098.001.04_1.3.0.xsd");
            var schemaFullPath = Path.GetFullPath(schemaPath);

            XmlSchemaSet schemas = new XmlSchemaSet();
            using (var schemaReader = new XmlTextReader(schemaFullPath))
            {
                XmlSchema schema = XmlSchema.Read(schemaReader, XmlValidator.OnValidationEvent);
                schemas.Add(schema);
            }

            var validator = new XmlValidator();
            Assert.False(validator.Validate(doc, schemas));
        }

        public class XmlValidator
        {
            public bool Validate(XmlDocument doc, XmlSchemaSet schemas)
            {
                var isValid = true;
                var ms = new MemoryStream();
                doc.Save(ms);
                ms.Position = 0;

                var settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemas;
                settings.ValidationEventHandler += (sender, e) =>
                {
                    OnValidationEvent(sender,
                        e); // this is duplicated and will probably lead to strange behaviour. with more time I would remove it and check functionality
                    isValid = false;
                };

                using XmlReader reader = XmlReader.Create(ms, settings);
                try
                {
                    while (reader.Read())
                    {
                    }
                }
                catch (XmlException e)
                {
                    Console.WriteLine($"XML Error: {e.Message}");
                    isValid = false;
                }

                return isValid;
            }

            public static void OnValidationEvent(object? sender, ValidationEventArgs e)
            {
                Console.WriteLine($"Validation Error: {e.Message}");
            }
        }
    }
}