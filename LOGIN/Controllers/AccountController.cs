using LOGIN.Dtos.RolDTOs;
using LOGIN.Dtos.UserDTOs;
using LOGIN.Entities;
using LOGIN.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LOGIN.Dtos;
using Microsoft.EntityFrameworkCore;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly UserManager<UserEntity> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly RoleManager<IdentityRole> _roleManager;


    public AccountController(
        IUserService userService,
        IEmailService emailService,
        IConfiguration configuration,
        UserManager<UserEntity> userManager,
        SignInManager<UserEntity> signInManager,
        ILogger<AccountController> logger,
        RoleManager<IdentityRole> roleManager

        )
    {
        _emailService = emailService;
        _userService = userService;
        _userManager = userManager;
        _logger = logger;
        _roleManager = roleManager;
    }

    [HttpPost("register")]
    [Authorize]
    public async Task<ActionResult<ResponseDto<CreateUserDto>>> Register([FromBody] CreateUserDto model)
    {
        if (ModelState.IsValid)
        {
            var response = await _userService.RegisterUserAsync(model);

            if (response.Status)
            {
                return Ok(new { Message = response.Message });
            }

            return BadRequest(new { Message = response.Message, Errors = response.Data.Errors });
        }

        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ResponseDto<LoginResponseDto>>> Login([FromBody] LoginDto model)
    {
        var authResponse = await _userService.LoginUserAsync(model);
        return StatusCode(authResponse.StatusCode, authResponse);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<ResponseDto<LoginResponseDto>>> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        var loginResponseDto = await _userService.RefreshTokenAsync(dto);

        return StatusCode(loginResponseDto.StatusCode, new
        {
            Status = true,
            loginResponseDto.Message,
            loginResponseDto.Data
        });
    }

    [HttpPost("create-role")]
    [Authorize]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.CreateRoleAsync(model);

        if (result.Succeeded)
        {
            return Ok(new { Result = "Role created successfully" });
        }

        return BadRequest(result.Errors);
    }

    [HttpGet("get-roles")]
    [Authorize]
    public async Task<ActionResult<ResponseDto<IEnumerable<RoleDto>>>> GetRoles()
    {

        var result = await _userService.GetRolesAsync();
        return StatusCode(result.StatusCode, result);

    }

    [HttpGet("get-role{id}")]
    [Authorize]
    public async Task<ActionResult<ResponseDto<RoleDto>>> GetRoleById(string id)
    {

        var result = await _userService.GetRolesByIdAsync(id);
        return StatusCode(result.StatusCode, result);

    }

    [HttpPost("assign-role/{userId}")]
    [Authorize]
    public async Task<IActionResult> AssignRole(string userId, [FromBody] AssignRoleRequest request)
    {
        if (string.IsNullOrEmpty(request.RoleName))
        {
            return BadRequest("El nombre del rol es requerido.");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("Usuario no encontrado");
        }

        if (!await _roleManager.RoleExistsAsync(request.RoleName))
        {
            return NotFound("Rol no encontrado");
        }

        var result = await _userManager.AddToRoleAsync(user, request.RoleName);

        if (result.Succeeded)
        {
            _logger.LogInformation("Rol {RoleName} asignado exitosamente al usuario: {User}", request.RoleName, user);
            return Ok("Rol asignado exitosamente");
        }

        _logger.LogError("Error al asignar el rol {RoleName} al usuario: {User}, Errores: {Errors}", request.RoleName, user, string.Join(", ", result.Errors.Select(e => e.Description)));
        return BadRequest(result.Errors);
    }

    public class AssignRoleRequest
    {
        public string RoleName { get; set; }
    }

    [HttpDelete("remove-role/{userId}/{roleName}")]
    [Authorize]
    public async Task<IActionResult> RemoveRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("Usuario no encontrado");
        }

        // Prevent removing admin role from current user
        if (User.Identity.Name == user.UserName && roleName == "Admin")
        {
            _logger.LogWarning("Intento de eliminar el rol de Administrador del usuario actual: {User}", user);
            return BadRequest("No puedes quitarte a ti mismo el rol de Administrador");
        }

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);

        if (result.Succeeded)
        {
            _logger.LogInformation("Rol {RoleName} eliminado exitosamente con el usuario: {User}", roleName, user);
            return Ok("Rol removido exitosamente");
        }

        _logger.LogError("Error al eliminar el rol {RoleName} del usuario: {User}, Errores: {Errors}", roleName, user, string.Join(", ", result.Errors.Select(e => e.Description)));
        return BadRequest(result.Errors);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null || user.PasswordResetToken != model.Token || user.PasswordResetTokenExpires < DateTime.UtcNow)
        {
            return BadRequest(new { code = "InvalidToken", description = "Invalid or expired token." });
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user); // Generar token para usar con Identity
        var resetPassResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

        if (!resetPassResult.Succeeded)
        {
            return BadRequest(resetPassResult.Errors);
        }

        // Limpiar el token y la fecha de expiración
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpires = null;
        await _userManager.UpdateAsync(user);

        return Ok(new { Result = "Password has been reset" });
    }

    [HttpPost("generate-password-reset-token")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
    {
        try
        {
            var token = await _userService.GeneratePasswordResetTokenAsync(model.Email);

            if (string.IsNullOrEmpty(token))
            {
                // Por seguridad, no revelamos si el email existe o no
                return Ok(new
                {
                    Status = true,
                    Message = "Si el email existe, se ha enviado un enlace de recuperación"
                });
            }

            // Enviar email con el token
            await _emailService.SendEmailAsync(model.Email, model.Email , token);

            _logger.LogInformation("Token de recuperación enviado a: {Email}", model.Email);

            return Ok(new
            {
                Status = true,
                Message = "Si el email existe, se ha enviado un enlace de recuperación"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al generar token de recuperación");
            return StatusCode(500, new
            {
                Status = false,
                Message = "Error interno al procesar la solicitud"
            });
        }
    }
}