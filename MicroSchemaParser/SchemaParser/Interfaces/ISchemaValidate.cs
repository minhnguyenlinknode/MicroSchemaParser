using System;

namespace WebGrader.Builder.SchemaParser
{
    public interface ISchemaValidate
    {
        SchemaValidatedStatus Validate();
    }
}