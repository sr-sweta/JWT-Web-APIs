using JwtWebApi.Data;
using JwtWebApi.Models.Domain;
using JwtWebApi.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JwtWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        User user = null;
        private readonly JwtWebApiDbContext dbContext;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RegionsController(JwtWebApiDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            this.dbContext = dbContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        

        [HttpGet]
        public IActionResult GetAll()
        {
            if (user == null)
            {
                user = GetUser();
            }
            var refreshToken = user.RefreshToken;

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token!");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired.");
            }

            var regionsDomain = dbContext.Regions.ToList();
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

            foreach (var regionDto in regionsDto)
            {
                Console.WriteLine(regionDto.Id);
                Console.WriteLine(regionDto.Name);
                Console.WriteLine(regionDto.Code);
                Console.WriteLine(regionDto.RegionImageUrl);
            }
            

            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            if (user == null)
            {
                user = GetUser();
            }
            var refreshToken = user.RefreshToken;

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token!");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired.");
            }

            var regionDomain = dbContext.Regions.Find(id);


            if (regionDomain == null)
            {
                return NotFound();
            }
           
            var regionDto = new RegionDTO()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl
            };
 
            return Ok(regionDto);
        }
        

        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            if (user == null)
            {
                user = GetUser();
            }
            var refreshToken = user.RefreshToken;

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token!");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired.");
            }

            var regionDomainModel = new Region()
            {
                Name = addRegionRequestDTO.Name,
                Code = addRegionRequestDTO.Code,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl
            };

            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            var regionDto = new RegionDTO()
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl

            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        
        [HttpPut]
        [Route("{id:int}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateRegionDTO updateRegion)
        {
            if (user == null)
            {
                user = GetUser();
            }
            var refreshToken = user.RefreshToken;

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token!");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired.");
            }

            var regionDomain = dbContext.Regions.Find(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            regionDomain.Name = updateRegion.Name;
            regionDomain.Code = updateRegion.Code;
            regionDomain.RegionImageUrl = updateRegion.RegionImageUrl;

            dbContext.Regions.Update(regionDomain);
            dbContext.SaveChanges();

            return Ok(updateRegion);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            if(user == null)
            {
                user = GetUser();
            }
            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token!");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired.");
            }

            var regionDomain = dbContext.Regions.Find(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            dbContext.Regions.Remove(regionDomain);
            dbContext.SaveChanges();

            return Ok();
        }
        

        private User GetUser()
        {
            var result = string.Empty;
            if (httpContextAccessor.HttpContext != null)
            {
                result = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                user = dbContext.Users.FirstOrDefault(acc => acc.UserName == result);
            }
                 
            return user;
        }
    }
}
