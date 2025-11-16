// Services/OpenAiService.cs
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Senior_Design_Pet_Care_App.Services
{
    public class OpenAiService : IOpenAiService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private const string OpenAiUrl = "https://api.openai.com/v1/chat/completions";

        public OpenAiService(HttpClient http, Microsoft.Extensions.Configuration.IConfiguration config)
        {
            _http = http;
            _apiKey = config["OpenAI:ApiKey"] ?? config["OPENAI_API_KEY"] ?? "";
            if (string.IsNullOrWhiteSpace(_apiKey))
                throw new InvalidOperationException("OpenAI API key not configured. Set OpenAI:ApiKey in configuration or OPENAI_API_KEY env var.");
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> GeneratePetAdviceAsync(
            string petName,
            string breed,
            int age,
            decimal height,
            decimal weight,
            string activityLevel,
            string? foodsCsv,
            string? medsCsv,
            DateTime? mostRecentVetAppointment,
            string? notes,
            int maxTokens = 600)
        {
            // Build a friendly prompt that includes all provided pet information and asks for actionable, safe advice
            var promptBuilder = new StringBuilder();
            promptBuilder.AppendLine($"You are a professional veterinarian assistant. Provide clear, specialized, actionable, and straightforward advice for the following pet based on the information below. Include sections for: (1) Summary of issues/considerations, (2) Diet & feeding recommendations, (3) Exercise & activity recommendations, (4) Medication & veterinary follow-up suggestions, (5) Any warning signs that require urgent vet care. Suggest seeing a vet or changing their diet and/or exercise if you suspect an issue with the pet. Keep it short but thorough (5 paragraphs maximum). Make sure to give specific advice based on the provided details for the pet. For example if a pet is overweight, you need to say that they are overweight and that they need to adjust their diet and/or exercise");
            promptBuilder.AppendLine();
            promptBuilder.AppendLine("Pet Information:");
            promptBuilder.AppendLine($"- Name: {petName}");
            promptBuilder.AppendLine($"- Breed: {breed}");
            promptBuilder.AppendLine($"- Age (years): {age}");
            promptBuilder.AppendLine($"- Height: {height} (inches)");
            promptBuilder.AppendLine($"- Weight: {weight} (pounds)");
            promptBuilder.AppendLine($"- Activity level: {activityLevel}");
            promptBuilder.AppendLine($"- Foods: {(!string.IsNullOrWhiteSpace(foodsCsv) ? foodsCsv : "None listed")}");
            promptBuilder.AppendLine($"- Medications: {(!string.IsNullOrWhiteSpace(medsCsv) ? medsCsv : "None listed")}");
            promptBuilder.AppendLine($"- Most recent vet appointment: {(mostRecentVetAppointment.HasValue ? mostRecentVetAppointment.Value.ToString("yyyy-MM-dd") : "None listed")}");
            promptBuilder.AppendLine($"- Notes: {(!string.IsNullOrWhiteSpace(notes) ? notes : "None")}");
            promptBuilder.AppendLine();
            promptBuilder.AppendLine("Provide the advice now:");

            var systemMessage = new
            {
                role = "system",
                content = "You are an expert small-animal veterinary assistant. Provide clear, straightforward, specialized, and actionable pet care advice based on the provided information."
            };

            var userMessage = new
            {
                role = "user",
                content = promptBuilder.ToString()
            };

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[] { systemMessage, userMessage },
                max_tokens = maxTokens,
                temperature = 1
            };

            var requestJson = JsonSerializer.Serialize(requestBody);
            using var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
            using var resp = await _http.PostAsync(OpenAiUrl, content);
            var respString = await resp.Content.ReadAsStringAsync();

            if (!resp.IsSuccessStatusCode)
            {
                // Try to extract error message or return generic
                try
                {
                    var doc = JsonDocument.Parse(respString);
                    if (doc.RootElement.TryGetProperty("error", out var err) && err.TryGetProperty("message", out var msg))
                    {
                        throw new Exception($"OpenAI API error: {msg.GetString()}");
                    }
                }
                catch { /* ignore parse error */ }

                throw new Exception($"OpenAI API request failed ({resp.StatusCode}): {respString}");
            }

            // Parse response
            try
            {
                using var doc = JsonDocument.Parse(respString);
                var root = doc.RootElement;
                var choice = root.GetProperty("choices")[0];
                var message = choice.GetProperty("message");
                var advice = message.GetProperty("content").GetString() ?? "";
                // Trim and return
                return advice.Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to parse OpenAI response: " + ex.Message + " Raw: " + respString);
            }
        }
    }
}
