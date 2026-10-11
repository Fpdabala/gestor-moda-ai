using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace GestorModaAi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public IaController(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public class RequisicaoIa
        {
            public string NomePeca { get; set; } = string.Empty;
            public string Categoria { get; set; } = string.Empty;
            public string Estilo { get; set; } = string.Empty;
        }

        [HttpPost("gerar-descricao")]
        public async Task<IActionResult> GerarDescricao([FromBody] RequisicaoIa requisicao)
        {
            if (string.IsNullOrWhiteSpace(requisicao.NomePeca))
            {
                return BadRequest("O nome da peça é obrigatório.");
            }

            var apiKey = _configuration["Gemini:ApiKey"];

            // Se a chave não estiver configurada, usa o tratamento de falha (Fallback)
            if (string.IsNullOrEmpty(apiKey) || apiKey == "SUA_CHAVE_AQUI")
            {
                return Ok(new
                {
                    origem = "Fallback (Sistema Local)",
                    descricao = $"Peça excelente da categoria {requisicao.Categoria}, ideal para composições no estilo {requisicao.Estilo}."
                });
            }

            try
            {
                var prompt = $"Crie uma descrição atraente para e-commerce de moda para a peça '{requisicao.NomePeca}' da categoria '{requisicao.Categoria}', com estilo '{requisicao.Estilo}'. Mantenha a resposta com até 3 frases em tom comercial e elegante.";

                var payload = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var url = $"{_configuration["Gemini:Endpoint"]}?key={apiKey}";
                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    // Tratamento de erro/falha da API
                    return Ok(new
                    {
                        origem = "Fallback (Erro na API)",
                        descricao = $"Peça {requisicao.NomePeca} de alta qualidade na categoria {requisicao.Categoria}, versátil para o estilo {requisicao.Estilo}."
                    });
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);

                var textoGerado = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                return Ok(new
                {
                    origem = "Gemini AI",
                    descricao = textoGerado?.Trim()
                });
            }
            catch (Exception)
            {
                // Tratamento de exceção/timeout
                return Ok(new
                {
                    origem = "Fallback (Exceção)",
                    descricao = $"Peça {requisicao.NomePeca} perfeita para seu guarda-roupa na categoria {requisicao.Categoria}."
                });
            }
        }
    }
}