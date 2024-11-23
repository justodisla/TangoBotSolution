using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientLib
{
    public class DiagnoseComponent
    {
        public static async Task DiagnoseAsync(HttpResponseMessage response)
        {
            // Example diagnostic logic
            Console.WriteLine("[Diagnose] Diagnosing the issue...");

            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                Console.WriteLine("[Diagnose] Cause: Bad Request. Suggested Solution: Check the request parameters and try again.");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                Console.WriteLine("[Diagnose] Cause: Internal Server Error. Suggested Solution: Try again later or contact support.");
            }
            else if (response.StatusCode == (System.Net.HttpStatusCode)422)
            {
                await HandleUnprocessableEntityAsync(response);
            }
            else
            {
                Console.WriteLine($"[Diagnose] Cause: Unknown. Status code: {response.StatusCode}. Suggested Solution: Investigate further.");
            }

            // Additional diagnostic logic can be added here
        }

        private static async Task HandleUnprocessableEntityAsync(HttpResponseMessage response)
        {
            Console.WriteLine("[Diagnose] Cause: Unprocessable Entity (422).");

            // Read the response content for more details
            string content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[Diagnose] Response Content: {content}");

            // Example analysis of the content
            if (content.Contains("validation error"))
            {
                Console.WriteLine("[Diagnose] Suggested Solution: Check the data being sent for validation errors.");
            }
            else if (content.Contains("missing field"))
            {
                Console.WriteLine("[Diagnose] Suggested Solution: Ensure all required fields are included in the request.");
            }
            else
            {
                Console.WriteLine("[Diagnose] Suggested Solution: Review the response content for more details on the error.");
            }

            // Additional troubleshooting steps can be added here
            Console.WriteLine("[Diagnose] Troubleshooting: Ensure the request data is correctly formatted and all required fields are present.");
        }
    }
}
