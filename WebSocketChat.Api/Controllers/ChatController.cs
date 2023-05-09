using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebSocketChat.Api.Controllers.Room;

namespace WebSocketChat.Api.Controllers;

/// <summary>
/// Controller responsible for handling WebSocket connections for the chat room.
/// </summary>
/// <author>Anirudh Pandita</author>
[ApiController]
public class ChatController : ControllerBase
{
    /// <summary>
    /// The room controller managing the chat room.
    /// </summary>
    private readonly RoomController _roomController;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChatController"/> class.
    /// </summary>
    /// <param name="roomController">The room controller managing the chat room.</param>
    public ChatController(RoomController roomController)
    {
        _roomController = roomController;
    }

    /// <summary>
    /// Accepts WebSocket requests and handles the chat functionality for the chat room.
    /// </summary>
    /// <param name="username">The username of the WebSocket client.</param>
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/chat-socket/{username}")]
    public async Task ChatSocket(string username)
    {
        // Check if the request is a WebSocket request.
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            try
            {
                // Add the WebSocket client to the chat room.
                _roomController.OnJoin(username, webSocket);

                // Receive messages from the WebSocket client until the connection is closed.
                while (webSocket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 4];
                    var receiveResult =
                        await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (receiveResult.MessageType == WebSocketMessageType.Text)
                    {
                        // Send the received message to all members in the chat room.
                        await _roomController.SendMessage(username,
                            Encoding.UTF8.GetString(buffer, 0, receiveResult.Count));
                    }
                }
            }
            catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.ConnectionClosedPrematurely)
            {
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception)
            {
                // Return a bad request status code if an error occurs.
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            finally
            {
                // Remove the WebSocket client from the chat room.
                await _roomController.TryDisconnect(username);
            }
        }
    }

    [HttpGet("/api/chat")]
    public async Task<IActionResult> GetAllMessages() => Ok(await _roomController.GetAllMessages());
}