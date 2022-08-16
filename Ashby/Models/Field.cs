using System.Net.Mail;

namespace Ashby.Models
{
    public enum FieldType
    {
        None = 0,
        Text,
        Email,
        Select,
        Boolean,
        File
    }

    public class Field
    {
        private static readonly Dictionary<FieldType, Type> ExpectedValueTypes =
            new Dictionary<FieldType, Type>()
            {
                { FieldType.Text, typeof(string) },
                { FieldType.Email, typeof(string) },
                { FieldType.Select, typeof(string) },
                { FieldType.Boolean, typeof(bool) },
                { FieldType.File, typeof(string) },
            };

        /// <summary>
        /// The ID of this field. This value is provided by the storage layer,
        /// not via user input.
        /// </summary>
        public ulong Id { get; }

        /// <summary>
        /// The name of this field. This must be unique within the form.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The type of this field. 
        /// </summary>
        public FieldType Type { get; }

        /// <summary>
        /// Indicates whether the field must have a value when submitting responses.
        /// </summary>
        public bool Required { get; }

        /// <summary>
        /// A set of allowed values for this field. This is required for Select fields.
        /// </summary>
        public IReadOnlyList<object> AllowedValues { get; } = Enumerable.Empty<object>().ToList();

        /// <summary>
        /// Fields can be conditionally visible based on the values of other fields.
        /// This contains a mapping of field name -> required field value for this
        /// field to be visible.
        /// </summary>
        public IReadOnlyDictionary<string, object> DependsOn { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Validates the provided values for this field. 
        /// </summary>
        /// <returns>An error string if validation failed; null otherwise.</returns>
        public string Validate()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                return "Name must be a non-empty string.";
            }

            if (this.Type == FieldType.None)
            {
                return "None is not a valid field type.";
            }

            int a = 5;
            var b = a.GetType() == ExpectedValueTypes[this.Type];

            Type expectedValueType = ExpectedValueTypes[this.Type];
            if (!this.AllowedValues.All((v) => v.GetType() == expectedValueType))
            {
                return $"All AllowedValues for a {this.Type} field must be valid {expectedValueType} objects.";
            }

            if (this.DependsOn.Count != this.DependsOn.Keys.Distinct().Count())
            {
                return "Fields can only be in DependsOn once.";
            }

            switch (this.Type)
            {
                case FieldType.Select:
                    if (this.AllowedValues.Count != 0)
                    {
                        return "A Select field must have a non-empty set of AllowedValues.";
                    }

                    break;

                case FieldType.Email:
                    foreach (string allowedValue in this.AllowedValues)
                    {
                        try
                        {
                            MailAddress address = new MailAddress(allowedValue);
                        }
                        catch
                        {
                            return "All AllowedValues must be valid emails for an Email field.";
                        }
                    }

                    break;
            }

            return null;
        }
    }
}

