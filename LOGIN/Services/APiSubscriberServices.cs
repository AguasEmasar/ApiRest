using LOGIN.Services.Interfaces;

namespace LOGIN.Services
{
    public class APiSubscriberServices : IAPiSubscriberServices
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _baseUrlComment;
        private readonly string _baseUrlHistory;
        private readonly string _authId;
        private readonly string _authKey;
        private readonly ILogger<APiSubscriberServices> _logger;

        public APiSubscriberServices(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<APiSubscriberServices> logger)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["ApiConfig:BaseUrl"];
            _baseUrlComment = configuration["ApiConfig:BaseUrlComment"];
            _baseUrlHistory = configuration["ApiConfig:BaseUrlHistory"];
            _authId = configuration["ApiConfig:AuthId"];
            _authKey = configuration["ApiConfig:AuthKey"];
            _logger = logger;
        }

        public async Task<string> GetUserAsync(string clave)
        {
            try
            {
                var url = $"{_baseUrl}/{clave}";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-auth-id", _authId);
                _httpClient.DefaultRequestHeaders.Add("x-auth-key", _authKey);
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error en la solicitud: {StatusCode}, Detalles: {ErrorContent}", response.StatusCode, errorContent);
                    throw new Exception($"Error en la solicitud: {response.StatusCode}, Detalles: {errorContent}");
                }

                _logger.LogInformation("Solicitud exitosa para obtener usuario con clave: {Clave}", clave);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario con clave: {Clave}", clave);
                throw;
            }
        }

        public async Task<string> GetCommentAsync(string clave)
        {
            try
            {
                var urlcomment = $"{_baseUrlComment}/{clave}";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-auth-id", _authId);
                _httpClient.DefaultRequestHeaders.Add("x-auth-key", _authKey);
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = await _httpClient.GetAsync(urlcomment);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error en la solicitud de comentarios: {StatusCode}, Detalles: {ErrorContent}", response.StatusCode, errorContent);
                    throw new Exception($"Error en la solicitud de comentarios: {response.StatusCode}, Detalles: {errorContent}");
                }

                _logger.LogInformation("Solicitud exitosa para obtener comentarios con clave: {Clave}", clave);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener comentarios con clave: {Clave}", clave);
                throw;
            }
        }

        public async Task<string> GetHistoryAsync(string clave)
        {
            try
            {
                var claveLimpia = clave.Trim();
                var urlhistory = $"{_baseUrlHistory}/{claveLimpia}";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("x-auth-id", _authId);
                _httpClient.DefaultRequestHeaders.Add("x-auth-key", _authKey);
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                var response = await _httpClient.GetAsync(urlhistory);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error en la solicitud del historial: {StatusCode}, Detalles: {ErrorContent}", response.StatusCode, errorContent);
                    throw new Exception($"Error en la solicitud del historial: {response.StatusCode}, Detalles: {errorContent}");
                }

                _logger.LogInformation("Solicitud exitosa para obtener historial con clave: {Clave}", clave);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener historial con clave: {Clave}", clave);
                throw;
            }
        }
    }
}