using System;

namespace WebGrader.Builder.SchemaParser
{
    // Important note:
    // - Schema class name must follow this convention:
    //          Name + "Schema"
    //          where 'Name' must be the name defined in http://schema.org
    //
    // - Schema specification is defined in this embeded XML file:
    //          WebGrader.Builder.SchemaParser.SupportedSchemas.MicroSchema.xml

    /// <summary>
    /// JobPosting schema http://schema.org/JobPosting
    /// </summary>
    public class JobPostingSchema : MicroSchema
    {
        public JobPostingSchema()
            : base()
        {
        }
    }
}