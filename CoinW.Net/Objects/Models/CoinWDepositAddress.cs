﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CoinW.Net.Objects.Models
{
    /// <summary>
    /// Deposit address info
    /// </summary>
    public record CoinWDepositAddress
    {
        /// <summary>
        /// Network name
        /// </summary>
        [JsonPropertyName("chainName")]
        public string NetworkName { get; set; } = string.Empty;
        /// <summary>
        /// Address
        /// </summary>
        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// Memo
        /// </summary>
        [JsonPropertyName("memo")]
        public string? Memo { get; set; }
        /// <summary>
        /// Min deposit quantity
        /// </summary>
        [JsonPropertyName("minRechargeAmount")]
        public decimal MinDepositQuantity { get; set; }
    }


}
