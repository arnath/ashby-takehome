using System;

namespace Ashby.Models
{
    public class Response
    {
        /// <summary>
        /// The ID of this response. This value is provided by the storage layer,
        /// not via user input.
        /// </summary>
        public ulong Id { get; }

        /// <summary>
        /// The ID of the <see cref="Form"/> for which this is a response.
        /// </summary>
        public ulong FormId { get; }

        /// <summary>
        /// Timestamp at which this response was submitted.
        /// </summary>
        public DateTime Submitted { get; }

        /// <summary>
        /// The set of answers provided in this response. Must be non-empty to be valid.
        /// </summary>
        public Dictionary<string, object> Answers { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Validates the provided values and fields for this response.
        /// </summary>
        /// <returns><An error string if validation failed; null otherwise./returns>
        public string Validate()
        {
            Form form = LoadFormFromDatabase();
            foreach (Field field in form.Fields)
            {
                object fieldValue = null;
                if (field.Required && !this.Answers.TryGetValue(field.Name, out fieldValue))
                {
                    return $"Required field {field.Name} does not have an answer.";
                }

                foreach (KeyValuePair<string, object> kvp in field.DependsOn)
                {
                    if (!this.Answers.TryGetValue(kvp.Key, out object answerValue) ||
                        !kvp.Value.Equals(answerValue))
                    {
                        return $"Dependent field {kvp.Key} has invalid value {answerValue}.";
                    }
                }

                if (field.AllowedValues.Count != 0 && !field.AllowedValues.Contains(fieldValue))
                {
                    return $"{fieldValue} is not an allowed value for field {field.Name}.";
                }
            }

            return null;
        }

        private Form LoadFormFromDatabase()
        {
            return new Form();
        }
    }
}

