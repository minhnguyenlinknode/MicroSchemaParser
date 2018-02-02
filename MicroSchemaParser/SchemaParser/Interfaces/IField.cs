using System;
using System.Collections.Generic;

namespace SchemaParser
{
    public interface IField : ISchemaValidate
    {
        string FieldName { get; set; }
        bool IsRequired { get; set; }
        List<ISchema> SchemaItems { get; set; }

        void SetFieldValue(string value);

        object GetFieldValue();

        Type GetFieldType();
    }
}