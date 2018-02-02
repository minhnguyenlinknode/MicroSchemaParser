using System;

namespace SchemaParser
{
    public interface ISchemaValidate
    {
        SchemaValidatedStatus Validate();
    }
}