// Services/IOpenAiService.cs
using System.Threading.Tasks;

namespace Senior_Design_Pet_Care_App.Services
{
    public interface IOpenAiService
    {
        Task<string> GeneratePetAdviceAsync(
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
            int maxTokens = 600);
    }
}