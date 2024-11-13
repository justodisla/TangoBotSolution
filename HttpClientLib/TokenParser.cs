using System;
using System.Text.Json;

namespace TangoBot.HttpClientLib
{
    /// <summary>
    /// Parses the token from the API response body.
    /// </summary>
    public class TokenParser
    {
        /// <summary>
        /// Extracts the session token from the response body.
        /// </summary>
        /// <param name="responseBody">The JSON response body containing the token.</param>
        /// <returns>The extracted session token; otherwise, null.</returns>
        public string ParseToken(string responseBody)
        {
            try
            {
                Console.WriteLine("[Debug] Parsing token from response body.");
                var responseJson = JsonSerializer.Deserialize<SessionResponse>(responseBody);

                string token = responseJson?.token;

                if (!string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("[Debug] Token successfully parsed from response body.");
                    return token;
                }
                else
                {
                    Console.WriteLine("[Error] Token not found in response body.");
                    return null;
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[Error] JSON parsing error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Represents the JSON structure of the session response.
        /// </summary>
        private class SessionResponse
        {
            public string token { get; set; }
        }
    }
}
