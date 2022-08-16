using System;
using System.Text.Json;
using Ashby.Models;

namespace Ashby
{
    public class ApiController
    {
        public HttpResponse CreateForm(string requestContent)
        {
            Form form = JsonSerializer.Deserialize<Form>(requestContent);

            string validationError = form.Validate();
            if (validationError != null)
            {
                return new HttpResponse(400, validationError);
            }

            WriteToDatabase(form);

            return new HttpResponse(200, null);
        }

        public HttpResponse SubmitFormResponse(string requestContent)
        {
            Response response = JsonSerializer.Deserialize<Response>(requestContent);

            string validationError = response.Validate();
            if (validationError != null)
            {
                return new HttpResponse(400, validationError);
            }

            WriteToDatabase(response);

            return new HttpResponse(200, null);
        }

        private void WriteToDatabase(object model) { }
    }
}

