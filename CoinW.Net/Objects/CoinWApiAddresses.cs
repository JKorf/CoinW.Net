namespace CoinW.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class CoinWApiAddresses
    {
        /// <summary>
        /// The address used by the CoinWRestClient for the API
        /// </summary>
        public string RestClientAddress { get; set; } = "";
        /// <summary>
        /// The address used by the CoinWSocketClient for the websocket API
        /// </summary>
        public string SocketClientAddress { get; set; } = "";

        /// <summary>
        /// The default addresses to connect to the CoinW API
        /// </summary>
        public static CoinWApiAddresses Default = new CoinWApiAddresses
        {
            RestClientAddress = "https://api.coinw.com",
            SocketClientAddress = "wss://ws.futurescw.com"
        };
    }
}
