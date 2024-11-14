using System;
using System.Text.Json;

namespace HttpClientLib.TokenProviding
{
    /// <summary>
    /// Parses the session token from the Tastytrade API response body.
    /// </summary>
    public class TokenParser
    {
        /// <summary>
        /// Extracts the session token from the Tastytrade API response body.
        /// </summary>
        /// <param name="responseBody">The JSON response body containing the token.</param>
        /// <returns>The extracted session token; otherwise, null.</returns>
        public string ParseToken(string responseBody)
        {
            responseBody = responseBody.Replace("-", "_"); // Fix invalid JSON property names

            try
            {
                Console.WriteLine("[Debug] Parsing session token from Tastytrade response body.");
                var responseJson = JsonSerializer.Deserialize<SessionResponse>(responseBody);

                string token = responseJson?.data?.session_token;

                if (!string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("[Debug] Session token successfully parsed from Tastytrade response.");
                    return token;
                }
                else
                {
                    Console.WriteLine("[Error] Session token not found in Tastytrade response.");
                    return null;
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[Error] JSON parsing error for Tastytrade response: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Represents the JSON structure of the session response from Tastytrade.
        /// </summary>
        private class SessionResponse
        {
            public SessionData data { get; set; }
        }

        private class SessionData
        {
            public string session_token { get; set; }
        }
    }
}
