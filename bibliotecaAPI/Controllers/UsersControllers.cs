using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using bibliotecaAPI.services;
using bibliotecaDataAccess;
using bibliotecaModels.Dto;
using bibliotecaModels.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersControllers : ControllerBase
    {
         private readonly AplicationDBContext _db;
        private responseDto _response;
        private readonly ILogger<UsersControllers> _logger;
        private readonly IMapper _mapper;
 
        private readonly encrptationAndAuthentication _encrptationAndAuthentication;
        
        public UsersControllers(AplicationDBContext db, ILogger<UsersControllers> logger, IMapper mapper, encrptationAndAuthentication EncrptationAndAuthentication)
        {
            _logger= logger;// imprime la consola
            _mapper = mapper;//Hace un mapeo del DTO con la clase de las entidades
            _db = db;// es el contexto de la base de datos
            _response = new responseDto();   
            _encrptationAndAuthentication = EncrptationAndAuthentication;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
         public async Task<ActionResult<List<users>>> GetUsuarios(){
            _logger.LogInformation("Listado de usuarios");
             var lista =  await _db.usuario.ToListAsync();
            _response.Resultado= lista;
            _response.Mensaje="Listado de empleado";
             return Ok(_response);
         }

         [HttpPost("register")]
        public async Task<ActionResult<users>>  Register([FromBody] usersDto UsersDto)
        {
            var modelUser= new users
            {
               
                id = UsersDto.id,
                nombre= UsersDto.nombre,
                clave= _encrptationAndAuthentication.encriptarSHA256(UsersDto.clave),
                permisos= UsersDto.permisos
                 
                
            };

         
            

            await _db.usuario.AddAsync(modelUser);
            await _db.SaveChangesAsync();


            if(modelUser.id!=0){
              
                return StatusCode(StatusCodes.Status200OK,new{IsExitoso= true});

            }
            else{
               
                return StatusCode(StatusCodes.Status200OK,new{IsExitoso= false});
            }
            

      
    }

         [HttpPost("login")]
    public async Task<ActionResult<users>>  Login([FromBody] loginDto login)
    {
        var userfinded = await _db.usuario.Where(u => 
                                                u.nombre == login.nombre && u.clave == _encrptationAndAuthentication.encriptarSHA256(login.clave)).FirstOrDefaultAsync();

        if(userfinded == null){
           
            return StatusCode(StatusCodes.Status200OK,new{IsExitoso= false, token = ""});

        }
        else{
           
             return StatusCode(StatusCodes.Status200OK,new{IsExitoso= true ,token = _encrptationAndAuthentication.generarJWT(userfinded)});
        }
    }
    }
}