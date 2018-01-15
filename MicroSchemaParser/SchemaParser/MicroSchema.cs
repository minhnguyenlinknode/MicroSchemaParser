using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace WebGrader.Builder.SchemaParser
{
    public abstract class MicroSchema : ISchema
    {
        public List<IField> Fields { get; set; }

        public SchemaValidatedStatus Validate()
        {
            int errorCount = 0,
                warningCount = 0;

            var validateFieldsResult = this.Fields.Select(n => new
            {
                FieldName = n.FieldName,
                ValidateResult = n.Validate()
            }).ToList();

            errorCount = validateFieldsResult.Count(n => n.ValidateResult == SchemaValidatedStatus.Error);
            warningCount = validateFieldsResult.Count(n => n.ValidateResult == SchemaValidatedStatus.Warning);

            if (errorCount > 0)
            {
                return SchemaValidatedStatus.Error;
            }

            if (warningCount > 0)
            {
                return SchemaValidatedStatus.Warning;
            }

            return SchemaValidatedStatus.Success;
        }

        public MicroSchema()
        {
            Fields = CreateFields();
        }

        protected List<IField> CreateFields()
        {
            var fields = new List<IField>();

            var microSchemaXmlDoc = LoadMicroSchemaXml();

            var schemaName = this.GetType().Name.Replace("Schema", "");
            var schemaElement = microSchemaXmlDoc.Root.Elements()
                                                 .FirstOrDefault(n => GetAttributeValue(n, "name") == schemaName);

            if (schemaElement != null)
            {
                var fieldElements = schemaElement.Elements();

                foreach (var fieldElement in fieldElements)
                {
                    var fieldName = GetAttributeValue(fieldElement, "name");
                    var isFieldRequired = GetAttributeValue<bool>(fieldElement, "isRequired");
                    var fieldTypeName = GetAttributeValue(fieldElement, "dotNetType") ?? "System.String";

                    var fieldType = Type.GetType(fieldTypeName);
                    if (fieldType != null)
                    {
                        var fieldTypeGeneric = typeof(SchemaField<>).MakeGenericType(fieldType);
                        var field = Activator.CreateInstance(fieldTypeGeneric) as IField;

                        field.FieldName = fieldName;
                        field.IsRequired = isFieldRequired;

                        fields.Add(field);
                    }
                }
            }

            return fields;
        }

        private static XDocument LoadMicroSchemaXml()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var embededResourceName = "MicroSchemaParser.SchemaParser.SupportedSchemas.MicroSchema.xml";

            using (var stream = assembly.GetManifestResourceStream(embededResourceName))
            {
                return XDocument.Load(stream);
            }
        }

        private static string GetAttributeValue(XElement element, string attributeName)
        {
            return GetAttributeValue<string>(element, attributeName);
        }

        private static T GetAttributeValue<T>(XElement element, string attributeName)
        {
            T attributeValue = default(T);

            if (element != null
                && !string.IsNullOrWhiteSpace(attributeName)
                && element.Attribute(attributeName) != null)
            {
                attributeValue = (T)Convert.ChangeType(element.Attribute(attributeName).Value, typeof(T));
            }

            return attributeValue;
        }
    }
}