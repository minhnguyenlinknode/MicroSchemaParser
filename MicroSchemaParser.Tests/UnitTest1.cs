using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using WebGrader.Builder.SchemaParser;

namespace WebGrader.Builder.Test
{
    [TestClass]
    public class MicroSchemaParser_Tests
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        private static HtmlDocument CreateHtmlDocFromUrl(string url)
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(url);
        }

        private static HtmlDocument CreateHtmlDocFromFile(string filePath)
        {
            var htmlFileContent = File.ReadAllText(filePath);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(htmlFileContent);

            return doc;
        }

        [TestMethod]
        public void ParseJobPostingSchemaTest()
        {
            HtmlDocument doc = CreateHtmlDocFromFile("TestData/JobDetail.html");

            var microSchemaParser = new MicroSchemaParser(doc);
            var schemaResult = microSchemaParser.Parse<JobPostingSchema>();

            // Test result
            Assert.IsNotNull(schemaResult);

            var titleField = schemaResult.Fields.FirstOrDefault(n => n.FieldName == "title");
            var jobBenefitsField = schemaResult.Fields.FirstOrDefault(n => n.FieldName == "jobBenefits");
            var datePostedField = schemaResult.Fields.FirstOrDefault(n => n.FieldName == "datePosted");
            var hiringOrganizationField = schemaResult.Fields.FirstOrDefault(n => n.FieldName == "hiringOrganization");
            var jobLocationField = schemaResult.Fields.FirstOrDefault(n => n.FieldName == "jobLocation");

            Assert.AreEqual(titleField.GetFieldValue().ToString(), "API Developer");
            Assert.AreEqual(jobBenefitsField.GetFieldValue().ToString(), "£24,000 - £32,000 + healthcare, flexitime, free beer, early Friday finish and more!");

            Assert.IsTrue(datePostedField.GetFieldValue() is DateTime);
            Assert.AreEqual(datePostedField.GetFieldValue().ToString(), "09/01/2018 00:00:00");

            Assert.AreEqual(hiringOrganizationField.GetFieldValue().ToString(), "Firefish Software");

            Assert.AreEqual(jobLocationField.GetFieldValue(), null);
            Assert.AreEqual(jobLocationField.SchemaItems.Count(), 1);
            Assert.IsTrue(jobLocationField.SchemaItems[0] is PlaceSchema);
            Assert.AreEqual(jobLocationField.SchemaItems[0].Fields[0].GetFieldValue(), null);
            Assert.IsTrue(jobLocationField.SchemaItems[0].Fields[0].SchemaItems[0] is PostalAddressSchema);
            Assert.AreEqual(jobLocationField.SchemaItems[0].Fields[0].SchemaItems[0].Fields[0].GetFieldValue(), "Glasgow");

            // Test validation
            var validateResult = schemaResult.Validate();

            Assert.AreEqual(validateResult, SchemaValidatedStatus.Warning);
        }

        [TestMethod]
        public void ParseItemListSchemaTest()
        {
            HtmlDocument doc = CreateHtmlDocFromFile("TestData/Jobs.html");

            var microSchemaParser = new MicroSchemaParser(doc);
            var schemaResult = microSchemaParser.Parse<ItemListSchema>();

            // Test result
            Assert.IsNotNull(schemaResult);

            // Test validating
            var validateResult = schemaResult.Validate();

            Assert.AreEqual(validateResult, SchemaValidatedStatus.Success);

            // Test get child links
            var listItemLinks = schemaResult.GetListItemLinks();

            Assert.AreEqual(listItemLinks.Count(), 6);
        }

        [TestMethod]
        public void ParseAllMicroSchemaTest()
        {
            HtmlDocument doc = CreateHtmlDocFromFile("TestData/Jobs.html");

            var microSchemaParser = new MicroSchemaParser(doc);
            var schemas = microSchemaParser.Parse();

            Assert.IsTrue(schemas.Count() > 0);
        }

        [TestMethod]
        public void ParseNullMicroSchemaTest()
        {
            HtmlDocument doc = CreateHtmlDocFromUrl("http://www.google.com/");

            var microSchemaParser = new MicroSchemaParser(doc);
            var schemas = microSchemaParser.Parse();

            Assert.AreEqual(schemas.Count(), 0);
        }
    }
}