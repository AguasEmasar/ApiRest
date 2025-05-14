using AutoMapper;
using LOGIN.Dtos;
using LOGIN.Dtos.RolDTOs;
using LOGIN.Dtos.UserDTOs;
using LOGIN.Entities;
using LOGIN.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class UserService : IUserService
{
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<UserService> _logger;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    // Para saber el usuario que está logueado
    private readonly HttpContext _httpContext;
    private readonly string _USER_ID;

    public UserService(
        UserManager<UserEntity> userManager,
        SignInManager<UserEntity> signInManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        ILogger<UserService> logger,
        IHttpContextAccessor httpContextAccessor,
        ApplicationDbContext context,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
        _context = context;
        _mapper = mapper;
        _httpContext = httpContextAccessor.HttpContext;
        var idClaim = _httpContext.User.Claims.Where(x => x.Type == "UserId").FirstOrDefault();
        _USER_ID = idClaim?.Value;

    }

    public async Task<ResponseDto<IdentityResult>> RegisterUserAsync(CreateUserDto userDto)
    {
        var user = new UserEntity
        {
            UserName = userDto.UserName,
            Email = userDto.Email,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            CreatedDate = DateTime.UtcNow,
            Status = 1,
        };

        // Verifica si el email ya está registrado
        if (await _userManager.FindByEmailAsync(userDto.Email) != null)
        {
            return new ResponseDto<IdentityResult>
            {
                StatusCode = 400,
                Status = false,
                Message = "Email is already registered.",
                Data = IdentityResult.Failed(new IdentityError { Description = "Email is already registered." })
            };
        }

        var result = await _userManager.CreateAsync(user, userDto.Password);

        if (result.Succeeded)
        {
            _logger.LogInformation("Usuario registrado exitosamente: {Email}", userDto.Email);

            // Asignar roles al usuario
            foreach (var roleName in userDto.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
                    _logger.LogInformation("Rol creado: {RoleName}", roleName);
                }
                await _userManager.AddToRoleAsync(user, roleName);
                _logger.LogInformation("Rol {RoleName} asignado al usuario {Email}", roleName, userDto.Email);
            }

            return new ResponseDto<IdentityResult>
            {
                StatusCode = 201,
                Status = true,
                Message = "User registered successfully.",
                Data = result
            };
        }

        _logger.LogError("Error al registrar usuario: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
        return new ResponseDto<IdentityResult>
        {
            StatusCode = 400,
            Status = false,
            Message = "User registration failed.",
            Data = result
        };
    }

    public async Task<ResponseDto<LoginResponseDto>> LoginUserAsync(LoginDto dto)
    {
        try
        {
            var result = await _signInManager.PasswordSignInAsync(
                dto.UserName,
                dto.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                var userEntity = await _userManager.FindByNameAsync(dto.UserName);

                if (userEntity == null)
                {
                    return new ResponseDto<LoginResponseDto>
                    {
                        StatusCode = 404,
                        Status = false,
                        Message = "Usuario no encontrado."
                    };
                }

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userEntity.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", userEntity.Id)
                };

                var userRoles = await _userManager.GetRolesAsync(userEntity);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = GenerateJwtTokenAsync(authClaims);

                var responseDto = new LoginResponseDto
                {
                    Username = userEntity.UserName,
                    Email = userEntity.Email,
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    TokenExpiration = token.ValidTo,
                    RefreshToken = GenerateRefreshTokenString(),
                    Roles = userRoles.ToList()
                };

                userEntity.RefreshToken = responseDto.RefreshToken;
                userEntity.RefreshTokenDate = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JWT:RefreshTokenExpiry"]!));
                await _context.SaveChangesAsync();

                _logger.LogInformation("Inicio de sesión exitoso para el usuario: {UserName}", dto.UserName);
                return new ResponseDto<LoginResponseDto>
                {
                    StatusCode = 200,
                    Status = true,
                    Message = "Inicio de sesión realizado satisfactoriamente",
                    Data = responseDto
                };
            }

            return new ResponseDto<LoginResponseDto>
            {
                StatusCode = 400,
                Status = false,
                Message = "Credenciales inválidas."
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al iniciar sesión para el usuario: {UserName}", dto.UserName);
            return new ResponseDto<LoginResponseDto>
            {
                StatusCode = 500,
                Status = false,
                Message = "Error interno del servidor al iniciar sesión."
            };
        }
    }

    public async Task<IdentityResult> CreateRoleAsync(CreateRoleDto roleDto)
    {
        if (!await _roleManager.RoleExistsAsync(roleDto.RoleName))
        {
            var role = new IdentityRole { Name = roleDto.RoleName };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                _logger.LogInformation("Rol creado exitosamente: {RoleName}", roleDto.RoleName);
            }
            else
            {
                _logger.LogError("Error al crear rol: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return result;
        }

        return IdentityResult.Failed(new IdentityError { Description = "Role already exists" });
    }

    public async Task<ResponseDto<IEnumerable<RoleDto>>> GetRolesAsync()
    {
        var roles = await _context.Roles.ToListAsync();

        return new ResponseDto<IEnumerable<RoleDto>>
        {
            Status = true,
            StatusCode = 200,
            Data = _mapper.Map<IEnumerable<RoleDto>>(roles),
            Message = "Roles obtenidos correctamente"
        };
    }

    public async Task<ResponseDto<RoleDto>> GetRolesByIdAsync(string search)
    {
        var roles = await _context.Roles.FindAsync(search);

        if (roles == null)
        {
            return new ResponseDto<RoleDto>
            {
                Status = true,
                StatusCode = 200,
                Data = null,
                Message = "Role no encontrado"
            };
        }

        return new ResponseDto<RoleDto>
        {
            Status = true,
            StatusCode = 200,
            Data = _mapper.Map<RoleDto>(roles),
            Message = "Role obtenido correctamente"
        };
    }

    public async Task<string> GeneratePasswordResetTokenAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return null;
        }

        // Generar un código numérico aleatorio de 8 dígitos
        var token = new Random().Next(10000000, 99999999).ToString();

        // Guardar el token y la fecha de expiración en la base de datos
        user.PasswordResetToken = token;
        user.PasswordResetTokenExpires = DateTime.UtcNow.AddMinutes(15); // El token expira en 15 minutos
        await _userManager.UpdateAsync(user);

        _logger.LogInformation("Token generado exitosamente para: {Email}", email);
        return token;
    }

    public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user == null)
        {
            _logger.LogWarning("Usuario no encontrado: {Email}", resetPasswordDto.Email);
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

        if (result.Succeeded)
        {
            _logger.LogInformation("Contraseña restablecida exitosamente para: {Email}", resetPasswordDto.Email);
        }
        else
        {
            _logger.LogError("Error al restablecer contraseña: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        return result;
    }

    public async Task<bool> CheckEmailExistsAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email) != null;
    }

    public async Task<bool> CheckUserNameExistsAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName) != null;
    }

    private JwtSecurityToken GenerateJwtTokenAsync(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JWT:ExpiryMinutes"])),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    public ClaimsPrincipal GetTokenPrincipal(string token)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Secret").Value));

        var validation = new TokenValidationParameters
        {
            IssuerSigningKey = securityKey,
            ValidateLifetime = false,
            ValidateActor = false,
            ValidateIssuer = false,
            ValidateAudience = false,
        };
        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }

    public async Task<ResponseDto<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto dto)
    {
        try
        {
            var principal = GetTokenPrincipal(dto.Token!);
            var emailClaim = principal?.FindFirst(ClaimTypes.Name);

            if (emailClaim == null)
            {
                return new ResponseDto<LoginResponseDto>
                {
                    Status = false,
                    StatusCode = 401,
                    Message = "Acceso no autorizado: No se encontró el claim de correo electrónico en el token."
                };
            }

            string email = emailClaim.Value;
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null || user.RefreshToken != dto.RefreshToken || user.RefreshTokenDate < DateTime.UtcNow)
            {
                return new ResponseDto<LoginResponseDto>
                {
                    Status = false,
                    StatusCode = 401,
                    Message = "Token de actualización inválido o expirado."
                };
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id)
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var newToken = GenerateJwtTokenAsync(authClaims);

            user.RefreshToken = GenerateRefreshTokenString();
            user.RefreshTokenDate = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JWT:RefreshTokenExpiry"]!));
            await _context.SaveChangesAsync();

            return new ResponseDto<LoginResponseDto>
            {
                Status = true,
                StatusCode = 200,
                Message = "Token refrescado exitosamente.",
                Data = new LoginResponseDto
                {
                    Username = user.UserName,
                    Token = new JwtSecurityTokenHandler().WriteToken(newToken),
                    TokenExpiration = newToken.ValidTo,
                    RefreshToken = user.RefreshToken,
                    Roles = userRoles.ToList()
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al refrescar token.");
            return new ResponseDto<LoginResponseDto>
            {
                Status = false,
                StatusCode = 500,
                Message = "Error interno del servidor al refrescar token."
            };
        }
    }

    private string GenerateRefreshTokenString()
    {
        var randomNumber = new byte[64];

        using (var numberGenerator = RandomNumberGenerator.Create())
        {
            numberGenerator.GetBytes(randomNumber);
        }

        return Convert.ToBase64String(randomNumber);
    }
}