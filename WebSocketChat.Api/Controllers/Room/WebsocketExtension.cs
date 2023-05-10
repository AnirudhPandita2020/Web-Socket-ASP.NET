using System.Net.WebSockets;
using System.Text;

namespace WebSocketChat.Api.Controllers.Room;

/// <summary>
/// Extension methods for sending text messages over a WebSocket.
/// </summary>
public static class WebsocketExtension
{
    /// <summary>
    /// Sends a text message over the specified WebSocket.
    /// </summary>
    /// <param name="socket">The WebSocket to send the message over.</param>
    /// <param name="message">The message to send.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    public static async Task SendTextAsync(this WebSocket socket, string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        await socket.SendAsync(new ArraySegment<byte>(bytes),
            WebSocketMessageType.Text,
            true,
            CancellationToken.None);
    }

    /// <summary>
    /// Receives a text message from the specified WebSocket.
    /// </summary>
    /// <param name="webSocket">The WebSocket to receive the message from.</param>
    /// <returns>A task that represents the asynchronous receive operation. The task result contains the received message, or an empty string if the received message is not a text message.</returns>
    public static async Task<string> ReceiveTextAsync(this WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        return result.MessageType == WebSocketMessageType.Text ? Encoding.UTF8.GetString(buffer, 0, result.Count) : string.Empty;
    }
}
