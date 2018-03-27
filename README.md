# MicroSchema Parser .NET library

This is a basic .Net framework for parsing micro schema syntax (http://schema.org) 

It works like the way Google Structured Data Test Tool doing https://search.google.com/structured-data/testing-tool/u/0/ 

It makes use of HtmlAgilityPack library to parse html.

# Usage:

```c#
// To parse all supported micro schema:

HtmlDocument doc = CreateHtmlDocFromUrl("http://www.google.com/");

var microSchemaParser = new MicroSchemaParser(doc);

List<ISchema> allSupportedSchemas = microSchemaParser.Parse();  
  

// To find a specific micro schema:

var itemListSchema = microSchemaParser.Find<ItemListSchema>();

// To get schema fields
IField fields = itemListSchema.Fields;

// To get schema field name or value
string fieldName = fields[0].FieldName;
object fieldValue = fields[0].GetFieldValue();

// Each field could be another schema
List<ISchema> childSchemas = fields[0].SchemaItems;


// To check schema is validated
var validateResult = itemListSchema.Validate();

```
See unit test for more details: https://github.com/minhnguyenlinknode/MicroSchemaParser/blob/master/MicroSchemaParser.Tests/UnitTest1.cs 

# Extensions

To support more schemas for your need:
* Edit XML configuration at SchemaParser/SupportedSchemas/MicroSchema.xml to add more definition.

If you need your defined schema to be a strong-type class, do following:
* Add a new class to reflect that newly added schema as following, where XXX is the name of your new schema.
  
```c#  
  public class XXXSchema : MicroSchema  
  {    
        public XXXSchema(string name)        
            : base(name)
        {
        }
  }
  
```

# Author
 Minh Nguyen (c) 2018
 
