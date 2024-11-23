using System;
using System.Text.Json;

namespace HttpClientLib.TokenManagement
{
    /// <summary>
    /// Parses the session token from the TastyTrade API response body.
    /// </summary>
    public class TokenParser
    {
        /// <summary>
        /// Extracts the session token from the TastyTrade API response body.
        /// </summary>
        /// <param name="responseBody">The JSON response body containing the token.</param>
        /// <returns>The extracted session token; otherwise, null.</returns>
        public static string? ParseToken(string responseBody)
        {
            responseBody = responseBody.Replace("-", "_"); // Fix invalid JSON property names

            try
            {
                Console.WriteLine("[Debug] Parsing session token from TastyTrade response body.");
                var responseJson = JsonSerializer.Deserialize<SessionResponse>(responseBody);

                string? token = responseJson?.Data?.Session_token;

                if (!string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("[Debug] Session token successfully parsed from TastyTrade response.");
                    return token;
                }
                else
                {
                    Console.WriteLine("[Error] Session token not found in TastyTrade response.");
                    return null;
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[Error] JSON parsing error for TastyTrade response: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Represents the JSON structure of the session response from TastyTrade.
        /// </summary>
        private class SessionResponse
        {
            public SessionData? Data { get; set; }
        }

        private class SessionData
        {
            public string? Session_token { get; set; }
        }
    }
}
