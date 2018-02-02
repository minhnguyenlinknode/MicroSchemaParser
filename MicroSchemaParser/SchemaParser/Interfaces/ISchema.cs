using System;
using System.Collections.Generic;

namespace SchemaParser
{
    public interface ISchema : ISchemaValidate
    {
        string Name { get; set; }
        List<IField> Fields { get; set; }
    }
}