using System;
using System.Collections.Generic;
using System.Linq;

namespace SchemaParser
{
    public class SchemaField<T> : IField
    {
        private T _fieldValue;

        public string FieldName { get; set; }
        public bool IsRequired { get; set; }
        public List<ISchema> SchemaItems { get; set; }

        public SchemaField()
        {
            SchemaItems = new List<ISchema>();
        }

        public SchemaField(string fieldName, bool isRequired = false)
            : this()
        {
            FieldName = fieldName;
            IsRequired = isRequired;
        }

        public void SetFieldValue(string value)
        {
            this._fieldValue = (T)Convert.ChangeType(value, typeof(T));
        }

        public object GetFieldValue()
        {
            return this._fieldValue;
        }

        public Type GetFieldType()
        {
            return typeof(T);
        }

        public SchemaValidatedStatus Validate()
        {
            var result = SchemaValidatedStatus.Error;

            var fieldHasValue = !EqualityComparer<T>.Default.Equals(_fieldValue, default(T));
            var fieldHasSchemaItems = this.SchemaItems.Any(n => n.Validate() != SchemaValidatedStatus.Error);

            var fieldIsValid = fieldHasValue || fieldHasSchemaItems;

            if (this.IsRequired)
            {
                result = fieldIsValid ? SchemaValidatedStatus.Success
                                       : SchemaValidatedStatus.Error;
            }
            else
            {
                //not required
                result = fieldIsValid ? SchemaValidatedStatus.Success
                                       : SchemaValidatedStatus.Warning;
            }

            return result;
        }
    }

    public class SchemaField : SchemaField<string>
    {
        public SchemaField(string fieldName, bool isRequired = false)
            : base(fieldName, isRequired)
        {
        }
    }
}