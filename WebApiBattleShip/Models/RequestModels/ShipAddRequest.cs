using Newtonsoft.Json;

namespace WebApiBattleShip.Models.Request
{
    public class ShipAddRequest
    {
        [JsonProperty("VerticalHeadPosition")]
        public string VerticalStartingPoint { get; set; }

        [JsonProperty("HorizontalHeadPosition")]
        public string HorizontalStartingPoint { get; set; }

        [JsonProperty("Size")]
        public string Length { get; set; }

        [JsonProperty("Orientation")]
        public string Orientation { get; set; }
    }
}
