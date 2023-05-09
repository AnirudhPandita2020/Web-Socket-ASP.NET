using System.Text.Json.Serialization;

namespace WebSocketChat.Model;

/// <summary>
/// Represents a message in the message data store.
/// </summary>
/// <author>Anirudh Pandita</author>
[Serializable]
public class Message
{
    /// <summary>
    /// Gets or sets the unique identifier for this message.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the text of this message.
    /// </summary>
    [JsonPropertyName("text")]
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the username of the sender of this message.
    /// </summary>
    [JsonPropertyName("username")]
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of this message.
    /// </summary>
    [JsonPropertyName("timeStamp")]
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}