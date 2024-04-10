using System.Security.Cryptography;

using PrettyBots.Environment;

namespace PrettyBots.Tests.Environment.Messages;

public class TestMessage : IUserMessage
{
    public long UserId { get; } = RandomNumberGenerator.GetInt32(0, 1000000);
}