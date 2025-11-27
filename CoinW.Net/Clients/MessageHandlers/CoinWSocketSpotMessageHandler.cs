using CoinW.Net;
using CoinW.Net.Objects.Internal;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoinW.Net.Clients.MessageHandlers
{
    internal class CoinWSocketSpotMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = CoinWExchange._serializerContext;

        public CoinWSocketSpotMessageHandler()
        {
            AddTopicMapping<CoinWSocketResponse>(x => x.Type + x.PairCode?.ToLowerInvariant() + x.Interval);
        }
        protected override MessageEvaluator[] TypeEvaluators { get; } = [

            new MessageEvaluator {
                Priority = 1,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("event") { Constraint = x => x == "pong" },
                ],
                StaticIdentifier = "pong"
            },

            new MessageEvaluator {
                Priority = 2,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("channel") { Constraint = x => x == "login" },
                ],
                StaticIdentifier = "login"
            },

            new MessageEvaluator {
                Priority = 3,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("channel") { Constraint = x => x == "subscribe" || x == "unsubscribe" },
                ],
                StaticIdentifier = "SubResponse"
            },

            new MessageEvaluator {
                Priority = 4,
                ForceIfFound = true,
                Fields = [
                    new PropertyFieldReference("type"),
                ],
                IdentifyMessageCallback = x => x.FieldValue("type")!
            }

        ];
    }
}
