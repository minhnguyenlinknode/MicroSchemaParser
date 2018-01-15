using System;
using System.Collections.Generic;
using System.Linq;

namespace WebGrader.Builder.SchemaParser
{
    /// <summary>
    /// ItemList schema http://schema.org/ItemList
    /// </summary>
    public class ItemListSchema : MicroSchema
    {
        public ItemListSchema()
            : base()
        {
        }

        /// <summary>
        /// Get all child item links if any
        /// </summary>
        public List<string> GetListItemLinks()
        {
            var itemLinks = this.Fields.Where(n => n.FieldName == "itemListElement")
                                       .SelectMany(n => n.SchemaItems)
                                       .Select(n => n as ILinkableSchema)
                                       .Where(n => n != null)
                                       .Select(n => n.Url)
                                       .ToList();

            return itemLinks;
        }
    }
}