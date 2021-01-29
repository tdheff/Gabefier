using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Gabefier
{
    internal class Program
    {
        private const string Token = "ODA0NTE2Nzc0NTYwNTMwNDcz.YBNeog.bymd6loUowb9FgTUlIg8J8lgiO8";
        
        private DiscordSocketClient _client;
        private ulong _storedId;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
        

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;

            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.

            // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
            // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
            // var token = File.ReadAllText("token.txt");
            // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

            await _client.LoginAsync(TokenType.Bot, Token);
            await _client.StartAsync();

            _client.MessageReceived += GabeifyMessage;

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        
        private async Task GabeifyMessage(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;

            if (message.Author.Username.ToLower() == "waimbes" || message.Author.Id == _storedId)
            {
                _storedId = message.Author.Id;
                
                var gabefiedMessage = "";
                for (int i = 0; i < message.Content.Length; i++)
                {
                    gabefiedMessage += i % 2 == 0
                        ? message.Content[i].ToString().ToLower()
                        : message.Content[i].ToString().ToUpper();
                }
                
                await message.Channel.SendMessageAsync(gabefiedMessage);
            }

            // // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            // if (!(message.HasCharPrefix('!', ref argPos) || 
            //       message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
            //     message.Author.IsBot)
            //     return;
            //
            // // Create a WebSocket-based command context based on the message
            // var context = new SocketCommandContext(_client, message);
            //
            // // Execute the command with the command context we just
            // // created, along with the service provider for precondition checks.
            // await _commands.ExecuteAsync(
            //     context: context, 
            //     argPos: argPos,
            //     services: null);
        }
    }
}