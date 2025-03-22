using Newtonsoft.Json;
using Services.SessionAPI.ApplicationLayer.IService;
using Services.SessionAPI.Domain.DTO;

namespace Services.SessionAPI.ApplicationLayer
{
    public class TrainerService : ITrainerService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TrainerService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<TrainerDTO> GetTrainer(int trainerID)
        {
            var client = _httpClientFactory.CreateClient("Trainer");
            var response = await client.GetAsync($"api/trainer/{trainerID}");

            var apiContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
            if (result.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<TrainerDTO>(Convert.ToString(result.Result));
            }
            return new TrainerDTO();
        }
    }
}
