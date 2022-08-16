using System;

namespace Ashby.Models
{
    public class Form
    {
        /// <summary>
        /// The ID of this form. This value is provided by the storage layer,
        /// not via user input.
        /// </summary>
        public ulong Id { get; }

        /// <summary>
        /// Timestamp at which this form was created.
        /// </summary>
        public DateTime Created { get; }

        /// <summary>
        /// The title of this form.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// The set of fields present in this form. Must be non-empty to be valid.
        /// </summary>
        public IReadOnlyList<Field> Fields { get; } = Enumerable.Empty<Field>().ToList();

        /// <summary>
        /// Validates the provided values and fields for this form.
        /// </summary>
        /// <returns><An error string if validation failed; null otherwise./returns>
        public string Validate()
        {
            if (string.IsNullOrEmpty(this.Title))
            {
                return "Title must be a non-empty string.";
            }

            if (this.Fields.Count == 0)
            {
                return "A form must have a non-empty set of fields.";
            }

            HashSet<string> fieldNames = new HashSet<string>();
            HashSet<string> dependedFields = new HashSet<string>();
            foreach (Field field in this.Fields)
            {
                string fieldError = field.Validate();
                if (fieldError != null)
                {
                    return fieldError;
                }

                if (!fieldNames.Add(field.Name))
                {
                    return "All the fields in a form must have unique names.";
                }

                foreach (string dependedField in field.DependsOn.Keys)
                {
                    dependedFields.Add(dependedField);
                }
            }

            if (!dependedFields.All((f) => fieldNames.Contains(f)))
            {
                return "All dependent fields must refer to valid field names in the form.";
            }

            return null;
        }
    }
}

