using System;
using System.Linq;

namespace SchemaParser
{
    public class ListItemSchema : MicroSchema, ILinkableSchema
    {
        /// <summary>
        /// ListItem schema http://schema.org/ListItem
        /// </summary>
        public ListItemSchema(string name)
            : base(name)
        {
        }

        public string Url
        {
            get
            {
                var urlField = this.Fields.FirstOrDefault(n => String.Equals(n.FieldName, "url", StringComparison.CurrentCultureIgnoreCase));
                return urlField?.GetFieldValue() as string;
            }
        }
    }
}