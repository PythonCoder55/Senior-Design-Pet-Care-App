using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat; // Added the new gpt-5.4 SDK namespace

namespace Senior_Design_Pet_Care_App.Services
{
    public class OpenAiService : IOpenAiService
    {
        private readonly ChatClient _client;

        // Note: HttpClient was removed from the constructor as the new SDK handles networking
        public OpenAiService(IConfiguration config)
        {
            string apiKey = config["OpenAI:ApiKey"] ?? config["OPENAI_API_KEY"] ?? "";
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("OpenAI API key not configured. Set OpenAI:ApiKey in configuration or OPENAI_API_KEY env var.");

            // Initialize the new GPT-5.4 client here
            _client = new ChatClient(model: "gpt-5.4", apiKey: apiKey);
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
            int maxTokens = 600) // Kept maxTokens in signature to preserve compatibility with the interface
        {
            // The new SDK example uses a single string prompt.
            // We'll combine the system instruction and pet details into one clear prompt.
            var promptBuilder = new StringBuilder();
            promptBuilder.AppendLine("You are a professional veterinarian assistant. Provide clear, specialized, actionable, and straightforward advice for the following pet based on the information below. Include sections for: (1) Summary of issues/considerations, (2) Diet & feeding recommendations, (3) Exercise & activity recommendations, (4) Medication & veterinary follow-up suggestions, (5) Any warning signs that require urgent vet care or other notes you may have. Suggest seeing a vet or changing their diet and/or exercise if you suspect an issue with the pet. Keep it short but thorough (5 paragraphs maximum). Make sure to give specific and personalized advice based on the provided details for the pet. For example if a pet is overweight, you need to say that they are overweight and that they need to adjust their diet and/or exercise");
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

            try
            {
                // We wrap the synchronous CreateResponse in Task.Run to keep the method asynchronous
                // and avoid blocking the main/UI thread in your Blazor app.
                var response = await Task.Run(() => _client.CompleteChat(promptBuilder.ToString()));

                return response.Value.Content[0].Text;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get advice from GPT-5.4: {ex.Message}", ex);
            }
        }
    }
}