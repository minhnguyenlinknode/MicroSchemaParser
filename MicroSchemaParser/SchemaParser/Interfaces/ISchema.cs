using System;
using System.Collections.Generic;

namespace WebGrader.Builder.SchemaParser
{
    public interface ISchema : ISchemaValidate
    {
        List<IField> Fields { get; set; }
    }
}