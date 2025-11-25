using CoinW.Net;
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
    internal class CoinWSocketFuturesMessageHandler : JsonSocketMessageHandler
    {
        private static readonly HashSet<string> _typesWithoutSymbol = new HashSet<string>
        {
            "order",
            "position",
            "position_change",
            "assets",
        };

        public override JsonSerializerOptions Options { get; } = CoinWExchange._serializerContext;

        protected override MessageEvaluator[] MessageEvaluators { get; } = [

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
                Fields = [
                    new PropertyFieldReference("type"),
                    new PropertyFieldReference("pairCode"),
                    new PropertyFieldReference("interval"),
                    new PropertyFieldReference("channel"),
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("type")}-{x.FieldValue("pairCode")!.ToLowerInvariant()}-{x.FieldValue("interval")}-{x.FieldValue("channel")}"
            },

            new MessageEvaluator {
                Priority = 4,
                Fields = [
                    new PropertyFieldReference("type"),
                    new PropertyFieldReference("pairCode"),
                    new PropertyFieldReference("channel"),
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("type")}-{x.FieldValue("pairCode")!.ToLowerInvariant()}-{x.FieldValue("channel")}"
            },

            new MessageEvaluator {
                Priority = 5,
                Fields = [
                    new PropertyFieldReference("type"),
                    new PropertyFieldReference("pairCode"),
                    new PropertyFieldReference("interval"),
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("type")}-{x.FieldValue("pairCode")!.ToLowerInvariant()}-{x.FieldValue("interval")}"
            },

            new MessageEvaluator {
                Priority = 6,
                Fields = [
                    new PropertyFieldReference("type") { Constraint = x => !_typesWithoutSymbol.Contains(x!) },
                    new PropertyFieldReference("pairCode"),
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("type")}-{x.FieldValue("pairCode")!.ToLowerInvariant()}"
            },

            new MessageEvaluator {
                Priority = 6,
                Fields = [
                    new PropertyFieldReference("type"),
                    new PropertyFieldReference("channel"),
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("type")}-{x.FieldValue("channel")}"
            },

            new MessageEvaluator {
                Priority = 7,
                Fields = [
                    new PropertyFieldReference("type"),
                ],
                IdentifyMessageCallback = x => $"{x.FieldValue("type")}"
            }
        ];
    }
}
