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
            "unsubscribe"
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
                    new PropertyFieldReference("event").WithEqualContstraint("pong"),
                ],
                StaticIdentifier = "pong"
            },

            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("channel").WithEqualContstraint("login"),
                ],
                StaticIdentifier = "login"
            },

            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("channel").WithFilterContstraint(_subActions),
                ],
                StaticIdentifier = "SubResponse"
            },

            new MessageTypeDefinition {
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("type"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("type")!
            }

        ];
    }
}
