using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using bibliotecaModels.Entity;
using Microsoft.IdentityModel.Tokens;

namespace bibliotecaAPI.services
{
    public class encrptationAndAuthentication
    {
        private readonly IConfiguration _configuration;
    public encrptationAndAuthentication(IConfiguration configuration)
    {
        _configuration=configuration;
    }
     
     public string encriptarSHA256(string texto){
        //encripta la contraseña bajo esta libreria
        using(SHA256 sha256Hash = SHA256.Create()){
            //array de byte
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));
            //Convierte el array de bytes de en string
            StringBuilder builder = new StringBuilder ();
            //Recorre el array 
            for(int i= 0; i< bytes.Length; i++){
                //guarda cada dato del array para convertiro en un string
                //? Para que servira el x2
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        };
     }

    public string generarJWT(users user){
        // crear la informacion del usaurio para el token
        var userClaim = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim(ClaimTypes.Name,user.nombre!),
            new Claim(ClaimTypes.Name,user.permisos!),
      

        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credential = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature);

        //crear detalle de la token
        var jwtConfig = new JwtSecurityToken(
            claims: userClaim,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials : credential

        );
        return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
    }
    }
}