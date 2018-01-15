# MicroSchema Parser .NET library

This is a basic .Net framework for parsing micro schema syntax (http://schema.org) 

It works like the way Google Structured Data Test Tool doing https://search.google.com/structured-data/testing-tool/u/0/ 

# Usage:

```c#
// To parse all supported micro schema:

HtmlDocument doc = CreateHtmlDocFromUrl("http://www.google.com/");

var microSchemaParser = new MicroSchemaParser(doc);

List<ISchema> allSupportedSchemas = microSchemaParser.Parse();  
  

// To find a specific micro schema:

var itemListSchema = microSchemaParser.Find<ItemListSchema>();


// To check schema is validated

var validateResult = itemListSchema.Validate();

```

# Extensions

To support more schema:
* Edit XML configuration at SchemaParser/SupportedSchemas/MicroSchema.xml to add more definition.
* Add a new class to reflect that newly added schema as following, where XXX is the name of your new schema.
  
  ```c#
  public class XXXSchema : MicroSchema  
  {    
        public XXXSchema()        
            : base()
        {
        }
  }
  ```

# Author
 Minh Nguyen (c) 2018
 
