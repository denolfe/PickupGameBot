using System;
using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordPugBotcore
{
    public class UriTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> Read(ICommandContext context, string input)
            =>
                Task.FromResult(Uri.TryCreate(input, UriKind.Absolute, out Uri uri)
                    ? TypeReaderResult.FromSuccess(uri)
                    : TypeReaderResult.FromError(CommandError.ParseFailed, $"`{input}` is not a valid url."));
    }
}