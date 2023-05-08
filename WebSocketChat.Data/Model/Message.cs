using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the text of this message.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the username of the sender of this message.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of this message.
    /// </summary>
    public long TimeStamp { get; set; }
}
