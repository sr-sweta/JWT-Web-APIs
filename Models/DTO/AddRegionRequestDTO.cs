using System.ComponentModel.DataAnnotations;

namespace JwtWebApi.Models.DTO
{
    public class AddRegionRequestDTO
    {
        [Required(ErrorMessage = "Enter region name.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter region code.")]
        public string Code { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
