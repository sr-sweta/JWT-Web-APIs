using JwtWebApi.Data;
using JwtWebApi.Models.Domain;
using JwtWebApi.Models.DTO;

namespace JwtWebApi.Services.RegionService
{
    public class RegionService : IRegionService
    {
        private readonly JwtWebApiDbContext _dbContext;

        public RegionService(JwtWebApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        Boolean IRegionService.DeleteRegion(int id)
        {
            var regionDomain = _dbContext.Regions.Find(id);

            if (regionDomain == null)
            {
                return false;
            }

            _dbContext.Regions.Remove(regionDomain);
            _dbContext.SaveChanges();

            return true;
        }

        RegionDTO IRegionService.UpdateRegion(int id, UpdateRegionDTO updateRegionDTO)
        {
            var regionDomain = _dbContext.Regions.Find(id);

            if (regionDomain == null)
            {
                return null;
            }

            regionDomain.Name = updateRegionDTO.Name;
            regionDomain.Code = updateRegionDTO.Code;
            regionDomain.RegionImageUrl = updateRegionDTO.RegionImageUrl;

            _dbContext.Regions.Update(regionDomain);
            _dbContext.SaveChanges();

            var regionDto = new RegionDTO()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl

            };

            return regionDto;
        }

        RegionDTO IRegionService.CreateRegion(AddRegionRequestDTO addRegionRequestDTO)
        {
            var regionDomainModel = new Region()
            {
                Name = addRegionRequestDTO.Name,
                Code = addRegionRequestDTO.Code,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl
            };

            _dbContext.Regions.Add(regionDomainModel);
            _dbContext.SaveChanges();

            var regionDto = new RegionDTO()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl

            };

            return regionDto;
        }

        List<RegionDTO> IRegionService.GetAll()
        {
            var regionsDomain = _dbContext.Regions.ToList();
            var regionsDto = new List<RegionDTO>();

            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDTO()
                {
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    Code = regionDomain.Code,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }

            return regionsDto;
        }

        RegionDTO IRegionService.GetById(int id)
        {
            var regionDomain = _dbContext.Regions.Find(id);

            if (regionDomain == null)
            {
                return null;
            }

            var regionDto = new RegionDTO()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return regionDto;
        }
    }
}
