
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace User.Infrastructure.Persistence.Services;

public class TurnstileValidatorService : ITurnstileValidatorService
{
  private readonly IAppSettingsService _appSettingsService;
  private readonly HttpClient _httpClient;
  private readonly ILogger<TurnstileValidatorService> _logger;

  public TurnstileValidatorService(IAppSettingsService appSettingsService, ILogger<TurnstileValidatorService> logger)
  {
    _appSettingsService = appSettingsService;
    _httpClient = new HttpClient();
    _logger = logger;
  }

  public async Task<bool> ValidateHumanAsync(string token)
  {
    var secret = _appSettingsService.CapthaParams();

    var content = new FormUrlEncodedContent(new[]
    {
      new KeyValuePair<string,string>("secret",secret.Item2),
      new KeyValuePair<string, string>("response",token)
    });

    var response = await _httpClient
      .PostAsync("https://challenges.cloudflare.com/turnstile/v0/siteverify", content);
    var json = await response.Content.ReadAsStringAsync();

    var result = JsonSerializer.Deserialize<TurnstileResponse>(json);
    _logger.LogInformation("Turnstile response: {Json}", json);
    return result?.Success ?? false;
  }

  private class TurnstileResponse
  {
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("challenge_ts")]
    public string ChallengeTs { get; set; }

    [JsonPropertyName("hostname")]
    public string Hostname { get; set; }

    [JsonPropertyName("error-codes")]
    public string[] ErrorCodes { get; set; }

    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("cdata")]
    public string CData { get; set; }

    [JsonPropertyName("metadata")]
    public Metadata Metadata { get; set; }
  }

  private class Metadata
  {
    [JsonPropertyName("interactive")]
    public bool Interactive { get; set; }
  }
}
