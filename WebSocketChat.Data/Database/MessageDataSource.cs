﻿using Microsoft.EntityFrameworkCore;
using WebSocketChat.Model;

namespace WebSocketChat.Database;

/// <summary>
/// Implements the IMessageDataSource interface to provide access to the message data store.
/// </summary>
/// <author>Anirudh Pandita</author>
public class MessageDataSource : IMessageDataSource
{
    private readonly MessageDbContext _context;

    /// <summary>
    /// Initializes a new instance of the MessageDataSource class.
    /// </summary>
    /// <param name="context">The MessageDbContext instance.</param>
    public MessageDataSource(MessageDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all messages from the message data store.
    /// </summary>
    /// <returns>A task representing the asynchronous operation that retrieves the list of messages.</returns>
    public async Task<List<Message>> GetAllMessages() => await GetAllMessageQuery(_context);

    /// <summary>
    /// Inserts a message into the message data store.
    /// </summary>
    /// <param name="message">The message to insert.</param>
    /// <returns>A task representing the asynchronous operation that inserts the message.</returns>
    public async Task InsertMessage(Message message)
    {
        await InsertMessageQuery(_context, message);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// A compiled query to insert a message into the message data store.
    /// </summary>
    private static readonly Func<MessageDbContext, Message, Task> InsertMessageQuery =
        EF.CompileAsyncQuery((MessageDbContext context, Message message) =>
            context.Messages.Add(message)
        );

    /// <summary>
    /// A compiled query to get all messages from the message data store in descending order of time stamp.
    /// </summary>
    private static readonly Func<MessageDbContext, Task<List<Message>>> GetAllMessageQuery =
        EF.CompileAsyncQuery(
            (MessageDbContext context) =>
                context.Messages.OrderByDescending(message => message.TimeStamp).ToList());
}