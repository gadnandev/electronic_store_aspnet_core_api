﻿using AutoMapper;
using ElectronicsStore.Domain.Models;
using ElectronicsStore.Domain.Services;
using ElectronicsStore.Domain.Services.Communication;
using ElectronicsStore.Resources.Errors;
using ElectronicsStore.Resources.Requests;
using ElectronicsStore.Resources.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ElectronicsStore.Controllers {

    [Authorize]
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:ApiVersion}/[controller]")]
    public class AuthController : ControllerBase {

        private readonly IAuthService authService;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AuthController(IAuthService authService, ITokenService tokenService, IMapper mapper) {
            this.authService = authService;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        // Validate the token, if valid it returns the proper data..
        // unauthorized if not.
        [HttpGet("me")]
        public async Task<ActionResult> AuthorizeMe() {
            var token = Request.Headers["Authorization"];
            User user = await tokenService.ValidateTokenAsync(token);
            if (user != null)
                return Ok(mapper.Map<User, UserResponse>(user));
            return Unauthorized();
        }

        [HttpPost("signout")]
        public ActionResult Logout() {
            // Unimplemented Yet.
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<ActionResult> SignInAsync([FromForm] UserSignInRequest request) {
            User user = mapper.Map<UserSignInRequest, User>(request);
            AuthStatusResponse response = await authService.SignInAsync(user);
            if (response.Status)
                return Ok(response);
            return Unauthorized(new ErrorResponse { Error = response.Message, Status = response.Status });
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        [Consumes(contentType: "application/json", otherContentTypes: "multipart/form-data")]
        public async Task<ActionResult> SignUpAsync([FromForm] UserSignUpRequest request) {
            User user = mapper.Map<UserSignUpRequest, User>(request);
            AuthStatusResponse response = await authService.SignUpAsync(request, user);
            if (response.Status)
                return Ok(response);
            return BadRequest(new ErrorResponse { Error = response.Message, Status = response.Status });
        }
    }
}
