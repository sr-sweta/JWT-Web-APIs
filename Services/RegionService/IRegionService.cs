using JwtWebApi.Models.Domain;
using JwtWebApi.Models.DTO;

namespace JwtWebApi.Services.RegionService
{
    public interface IRegionService
    {
        RegionDTO GetById(int id);

        List<RegionDTO> GetAll();

        RegionDTO CreateRegion(AddRegionRequestDTO addRegionRequestDTO);

        RegionDTO UpdateRegion(int id, UpdateRegionDTO updateRegionDTO);

        Boolean DeleteRegion(int id);

    }
}
