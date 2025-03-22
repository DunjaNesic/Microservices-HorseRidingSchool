using Newtonsoft.Json;
using Services.SessionAPI.ApplicationLayer.IService;
using Services.SessionAPI.Domain.DTO;

namespace Services.SessionAPI.ApplicationLayer
{
    public class HorseService : IHorseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HorseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IEnumerable<HorseDTO>> GetAllHorses()
        {
            var client = _httpClientFactory.CreateClient("Horse");
            var response = await client.GetAsync($"api/horse");

            var apiContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ResponseDTO>(apiContent);
            if (result.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<IEnumerable<HorseDTO>>(Convert.ToString(result.Result));
            }
            return new List<HorseDTO>();
        }
    }
}
