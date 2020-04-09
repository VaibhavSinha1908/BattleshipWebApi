using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WebApiBattleShip.Models.Request
{
    /// <summary>
    /// Request Model to map the AddShip request object.
    /// </summary>
    public class ShipAddRequest
    {
        /// <summary>
        /// Vertical position for Ship's head. [Required]
        /// </summary>
        [Required]
        [JsonProperty("verticalHeadPosition")]
        public string VerticalStartingPoint { get; set; }


        /// <summary>
        /// Horizontal position for Ship's head. [Required]
        /// </summary>
        [Required]
        [JsonProperty("horizontalHeadPosition")]
        public string HorizontalStartingPoint { get; set; }


        /// <summary>
        /// Size/Length of the ship on the grid. [Required]
        /// </summary>
        [Required]
        [JsonProperty("size")]
        public string Length { get; set; }

        /// <summary>
        /// Orientation (vertical/horizontal) of the Ship on the board. [Required] 
        /// </summary>
        [Required]
        [JsonProperty("orientation")]
        public string Orientation { get; set; }
    }
}
