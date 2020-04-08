using Newtonsoft.Json;

namespace WebApiBattleShip.Models.Request
{
    public class ShipAddRequest
    {
        [JsonProperty("verticalHeadPosition")]
        public string VerticalStartingPoint { get; set; }

        [JsonProperty("horizontalHeadPosition")]
        public string HorizontalStartingPoint { get; set; }

        [JsonProperty("size")]
        public string Length { get; set; }

        [JsonProperty("orientation")]
        public string Orientation { get; set; }
    }
}
