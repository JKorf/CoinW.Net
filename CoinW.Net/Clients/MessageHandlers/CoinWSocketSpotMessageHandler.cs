using CoinW.Net.Objects.Internal;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using System.Collections.Generic;
using System.Text.Json;

namespace CoinW.Net.Clients.MessageHandlers
{
    internal class CoinWSocketSpotMessageHandler : JsonSocketMessageHandler
    {
        private static readonly HashSet<string?> _subActions = new HashSet<string?>()
        {
            "subscribe",
            "unsub"
        };
        public override JsonSerializerOptions Options { get; } = CoinWExchange._serializerContext;

        public CoinWSocketSpotMessageHandler()
        {
            AddTopicMapping<CoinWSocketResponse>(x => x.Type + x.PairCode?.ToLowerInvariant() + x.Interval);
        }
        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [

            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("event").WithEqualConstraint("pong"),
                ],
                StaticIdentifier = "pong"
            },

            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("channel").WithEqualConstraint("login"),
                ],
                StaticIdentifier = "login"
            },

            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("channel").WithFilterConstraint(_subActions),
                ],
                StaticIdentifier = "SubResponse"
            },

            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("type"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("type")!
            }

        ];
    }
}
