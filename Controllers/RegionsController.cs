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
        private readonly IUserService userService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRegionService regionService;

        public RegionsController(IUserService userService, IHttpContextAccessor httpContextAccessor, IRegionService regionService)
        {
            this.userService = userService;
            this.httpContextAccessor = httpContextAccessor;
            this.regionService = regionService;
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

            var regionsDto = regionService.GetAll();

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
           
            var regionDto = regionService.GetById(id);

            if (regionDto == null)
            {
                return NotFound();
            }

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

            var regionDto = regionService.CreateRegion(addRegionRequestDTO);

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

            var region = regionService.UpdateRegion(id, updateRegion);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(region);
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

            var result = regionService.DeleteRegion(id);

            if (result == false)
            {
                return NotFound();
            }

            return Ok();
        }
        

        private User GetUser()
        {
            string username = string.Empty;
            if (httpContextAccessor.HttpContext != null)
            {
                username = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                user = userService.GetUser(username);
            }
                 
            return user;
        }
    }
}
