using System.Text.Json.Serialization;

public record NestJsMessage<T>
{
    [JsonPropertyName("pattern")]
    public string Pattern { get; init; }

    [JsonPropertyName("data")]
    public T Data { get; init; }
}