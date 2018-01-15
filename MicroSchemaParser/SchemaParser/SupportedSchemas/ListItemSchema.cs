using System;
using System.Linq;

namespace WebGrader.Builder.SchemaParser
{
    public class ListItemSchema : MicroSchema, ILinkableSchema
    {
        /// <summary>
        /// ListItem schema http://schema.org/ListItem
        /// </summary>
        public ListItemSchema()
            : base()
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