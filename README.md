# MicroSchema Parser .NET library

This is a basic framework for parsing micro schema syntax (http://schema.org) 
It works like google testing tool https://search.google.com/structured-data/testing-tool/u/0/ 

# Usage:

//To parse all supported micro schema:

HtmlDocument doc = CreateHtmlDocFromUrl("http://www.google.com/");

var microSchemaParser = new MicroSchemaParser(doc);
List<ISchema> allSupportedSchemas = microSchemaParser.Parse();

//To find specific micro schema:

ItemListSchema itemListSchema = microSchemaParser.Find<ItemListSchema>();
  
//To check schema is validated
var validateResult = itemListSchema.Validate();

# Author
 Minh Nguyen (c) 2018
 
