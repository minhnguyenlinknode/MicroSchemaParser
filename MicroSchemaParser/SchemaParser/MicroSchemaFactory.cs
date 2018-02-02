using System;
using System.Text.RegularExpressions;

namespace SchemaParser
{
    public class MicroSchemaFactory
    {
        /// <summary>
        /// Create micro schema instance from type
        /// </summary>
        /// <param name="canonicalURL">Micro schema standard canonical URL. For ex: schema.org/JobPosting</param>
        /// <returns>Micro schema object</returns>
        public static ISchema Create(string canonicalURL)
        {
            ISchema result = null;

            var groups = Regex.Match(canonicalURL, @"(.*)schema.org/(.*)$").Groups;
            string schemaType = groups[2].Value;

            //Create instance from string
            var typeName = $"SchemaParser.{schemaType}Schema";
            Type supportSchemaType = Type.GetType(typeName);

            if (supportSchemaType == null)
            {
                result = new MicroSchema(schemaType);
            }
            else
            {
                result = Activator.CreateInstance(supportSchemaType, schemaType) as ISchema;
            }

            return result;
        }
    }
}