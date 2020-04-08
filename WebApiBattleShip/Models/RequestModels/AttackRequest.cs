using Newtonsoft.Json;

namespace WebApiBattleShip.Models.RequestModels
{
    public class AttackRequest
    {
        [JsonProperty("verticalPosition")]
        public string VerticalPosition { get; set; }

        [JsonProperty("horizontalPosition")]
        public string HorizontalPosition { get; set; }
    }
}
