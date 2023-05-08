using System.Net.WebSockets;

namespace WebSocketChat.Api.Controllers.Room;

/// <summary>
/// Represents a member in a chat room.
/// </summary>
/// <author>Anirudh Pandita</author>
public class Member
{
    /// <summary>
    /// Gets or sets the username of the member.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets the unique identifier for the session of the member.
    /// </summary>
    public Guid SessionId { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the WebSocket instance for the member.
    /// </summary>
    public WebSocket Socket { get; set; }
}
