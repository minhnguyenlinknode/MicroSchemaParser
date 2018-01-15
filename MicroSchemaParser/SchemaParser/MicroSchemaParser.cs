using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebGrader.Builder.SchemaParser
{
    public class MicroSchemaParser
    {
        private const string ItemScopeString = "itemscope";
        private const string ItemTypeString = "itemtype";
        private const string ItemPropString = "itemprop";
        private const string MetaString = "meta";
        private const string ContentString = "content";

        private HtmlDocument _htmlDoc;

        public MicroSchemaParser(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                throw new ArgumentNullException("Must provide html content");
            }

            _htmlDoc = new HtmlDocument();
            _htmlDoc.LoadHtml(html);
        }

        public MicroSchemaParser(HtmlDocument htmlDoc)
        {
            if (htmlDoc == null)
            {
                throw new ArgumentNullException("Must provide valid htmldoc");
            }

            _htmlDoc = htmlDoc;
        }

        /// <summary>
        /// Find a specific schema type in given page
        /// </summary>
        /// <typeparam name="T">Schema type to parse</typeparam>
        /// <returns>Returns the first micro schema type if found</returns>
        public T Parse<T>() where T : ISchema
        {
            T result = default(T);

            if (_htmlDoc == null)
            {
                return result;
            }

            var allItemScopeNodes = _htmlDoc.DocumentNode.SelectNodes($"//*[@{ItemScopeString}]");

            if (allItemScopeNodes != null)
            {
                foreach (var itemScopeNode in allItemScopeNodes)
                {
                    var itemScopeSchema = ParseItemScopeNode(itemScopeNode);
                    if (itemScopeSchema is T)
                    {
                        result = (T)itemScopeSchema;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Parse all supported micro schemas in given page
        /// </summary>
        public List<ISchema> Parse()
        {
            if (_htmlDoc == null)
            {
                return null;
            }

            var result = new List<ISchema>();

            var allItemScopeNodes = _htmlDoc.DocumentNode.SelectNodes($"//*[@{ItemScopeString}]");

            if (allItemScopeNodes != null)
            {
                var itemScopeRootNodes = allItemScopeNodes.Where(n => IsItemScopeRootNode(n)).ToList();

                foreach (var itemScopeRootNode in itemScopeRootNodes)
                {
                    var itemScopeSchema = ParseItemScopeNode(itemScopeRootNode);
                    if (itemScopeSchema != null)
                    {
                        result.Add(itemScopeSchema);
                    }
                }
            }

            return result;
        }

        #region Private methods

        private static ISchema ParseItemScopeNode(HtmlNode itemScopeNode)
        {
            ISchema microSchema = null;

            var itemType = itemScopeNode.Attributes[ItemTypeString];

            if (itemType != null)
            {
                microSchema = MicroSchemaFactory.Create(itemType.Value);

                if (microSchema != null)
                {
                    //Note:
                    //   . means: selecting nodes under current node.
                    //   double // means: selecting all nodes
                    //   single / means: selecting all nodes directly under current node
                    var itemScopeProperties = itemScopeNode.SelectNodes($".//*[@{ItemPropString}]");

                    foreach (var field in microSchema.Fields)
                    {
                        var itemScopePropertyCollection = itemScopeProperties
                                                            .Where(n => n.Attributes[ItemPropString] != null
                                                                        && String.Equals(n.Attributes[ItemPropString].Value, field.FieldName, StringComparison.CurrentCultureIgnoreCase)
                                                            ).ToList();

                        foreach (var itemScopeProperty in itemScopePropertyCollection)
                        {
                            var itemScopePropertySchema = ParseItemScopeNode(itemScopeProperty);

                            if (itemScopePropertySchema == null)
                            {
                                // is not our supported itemscope
                                // or just a single property
                                string fieldValue = ParseItemScopeProperty(itemScopeProperty);
                                field.SetFieldValue(fieldValue);
                            }
                            else
                            {
                                // property is an itemscope
                                field.SchemaItems.Add(itemScopePropertySchema);
                            }
                        }
                    }
                }
            }

            return microSchema;
        }

        private static string ParseItemScopeProperty(HtmlNode itemScopeProperty)
        {
            string fieldValue;

            if (itemScopeProperty.Name == MetaString)
            {
                fieldValue = itemScopeProperty.Attributes[ContentString].Value;
            }
            else
            {
                fieldValue = itemScopeProperty.InnerHtml;
            }

            return fieldValue;
        }

        private static bool IsItemScopeRootNode(HtmlNode itemScopeNode)
        {
            bool hasParentIsItemScopeNode = false;

            while (itemScopeNode.ParentNode != null)
            {
                if (IsItemScopeNode(itemScopeNode.ParentNode))
                {
                    hasParentIsItemScopeNode = true;
                    break;
                }

                itemScopeNode = itemScopeNode.ParentNode;
            }

            return !(hasParentIsItemScopeNode);
        }

        private static bool IsItemScopeNode(HtmlNode node)
        {
            return node.Attributes[ItemScopeString] != null;
        }

        #endregion Private methods
    }
}