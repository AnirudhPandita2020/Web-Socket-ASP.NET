using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WebSocketChat.Database;
using WebSocketChat.Model;

namespace WebSocketChat.Api.Controllers.Room;

/// <summary>
/// Controller class for managing chat rooms and WebSocket connections.
/// </summary>
/// <author>Anirudh Pandita</author>
public class RoomController
{
    /// <summary>
    /// Dictionary to keep track of the members in the chat room.
    /// </summary>
    private readonly ConcurrentDictionary<string, Member> _members = new();

    /// <summary>
    /// Data source for managing messages in the chat room.
    /// </summary>
    private readonly IMessageDataSource _messageDataSource;

    /// <summary>
    /// Creates a new instance of the <see cref="RoomController"/> class.
    /// </summary>
    /// <param name="messageDataSource">The data source for managing messages in the chat room.</param>
    public RoomController(IMessageDataSource messageDataSource)
    {
        _messageDataSource = messageDataSource;
    }

    /// <summary>
    /// Adds a new member to the chat room.
    /// </summary>
    /// <param name="username">The username of the new member.</param>
    /// <param name="socket">The WebSocket connection for the new member.</param>
    /// <exception cref="Exception">Thrown if the member is already present in the chat room.</exception>
    public void OnJoin(string username, WebSocket socket)
    {
        if (_members.ContainsKey(username))
        {
            throw new Exception("Member is already present");
        }

        _members[username] = new Member
        {
            Username = username,
            Socket = socket
        };
    }

    /// <summary>
    /// Sends a message to all members in the chat room.
    /// </summary>
    /// <param name="senderUserName">The username of the member who sent the message.</param>
    /// <param name="message">The message to send.</param>
    public async Task SendMessage(string senderUserName, string message)
    {
        foreach (var member in _members.Values)
        {
            // Create a new message entity and insert it into the data source
            var messageEntity = new Message
            {
                Text = message,
                Username = senderUserName
            };
            await _messageDataSource.InsertMessage(messageEntity);

            // Serialize the message entity and send it to the member's WebSocket connection
            var parsedMessage = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(messageEntity));
            await member.Socket.SendAsync(
                new ArraySegment<byte>(parsedMessage),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }
    }

    /// <summary>
    /// Retrieves all messages from the data source.
    /// </summary>
    /// <returns>A list of all messages in the chat room.</returns>
    public async Task<List<Message>> GetAllMessages() => await _messageDataSource.GetAllMessages();

    /// <summary>
    /// Tries to disconnect a member from the chat room.
    /// </summary>
    /// <param name="username">The username of the member to disconnect.</param>
    public async Task TryDisconnect(string username)
    {
        // Close the member's WebSocket connection and remove them from the dictionary
        await _members[username].Socket.CloseAsync(
            WebSocketCloseStatus.NormalClosure,
            "Connection closed by User",
            CancellationToken.None);
        if (_members.ContainsKey(username))
        {
            _members.Remove(username, out _);
        }
    }
}
