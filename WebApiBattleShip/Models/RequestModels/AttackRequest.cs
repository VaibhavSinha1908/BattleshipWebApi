using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebApiBattleShip.Models.RequestModels
{
    /// <summary>
    /// Request Model to map the AttackShip request object.
    /// </summary>
    public class AttackRequest
    {
        /// <summary>
        /// Vertical position of the grid cell. [Required]
        /// </summary>
        [Required]
        [JsonProperty("verticalPosition")]
        public string VerticalPosition { get; set; }


        /// <summary>
        /// Horizontal Position of the grid cell. [Required]
        /// </summary>
        [Required]
        [JsonProperty("horizontalPosition")]
        public string HorizontalPosition { get; set; }
    }
}
