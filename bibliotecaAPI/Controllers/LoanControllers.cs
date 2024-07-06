using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using bibliotecaDataAccess;
using bibliotecaModels.Dto;
using bibliotecaModels.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bibliotecaAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanControllers : ControllerBase
    {
        private readonly AplicationDBContext _db;
        private responseDto _response;
        private readonly ILogger<LoanControllers> _logger;
        private readonly IMapper _mapper;

        public LoanControllers(AplicationDBContext db, ILogger<LoanControllers> logger, IMapper mapper)
        {
            _logger= logger;// imprime la consola
            _mapper = mapper;//Hace un mapeo del DTO con la clase de las entidades
            _db = db;// es el contexto de la base de datos
            _response = new responseDto();            
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
         public async Task<ActionResult<List<loan>>> GetLoan(){
            _logger.LogInformation("Listado de empresas");
             var lista =  await _db.loans.ToListAsync();
            _response.Resultado= lista;
            _response.Mensaje="Listado de empresas";
             return Ok(_response);
         }

        [HttpGet("{id}", Name= "Getloan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
         public async Task<ActionResult<loan>> GetEmpresa(int id){
            if  (id==0){
                    _logger.LogError("Debe enviar el ID");
                    _response.Mensaje="Debe enviar el ID";
                    _response.IsExitoso=false;
                    return BadRequest(_response);
            }

             var Loan =  await _db.loans.FindAsync(id);

             if(Loan==null){
                _logger.LogError("prestamo no exite");
                _response.Mensaje="prestamo no exite";
                _response.IsExitoso=false;
                return NotFound(_response);
             }
            _response.Resultado= Loan;
            _response.Mensaje="Datos del prestamo";
             return Ok(_response); //Status code = 200
         }
    
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        public async Task<ActionResult<loan>> PostEmpresa([FromBody] loanDto LoanDto){
            if (LoanDto==null){
                _response.Mensaje="Información Incorrecta";
                _response.IsExitoso=false;
                return BadRequest(_response);
            }
           
             var libroExiste = await _db.book.FirstOrDefaultAsync
                                        (b => b.id == LoanDto.libro_id);
                                        

            if(libroExiste==null){
                ModelState.AddModelError("libro no existe","libro no existe");
                return BadRequest(ModelState);
            }
            
            var usuarioExiste = await _db.usuario.FirstOrDefaultAsync
                                        (u => u.id == LoanDto.usuario_id);
                                        

            if(usuarioExiste==null){
                ModelState.AddModelError("usuario no existe","usuario no existe");
                return BadRequest(ModelState);
            }
            
            

            loan Loan = _mapper.Map<loan>(LoanDto);
            await _db.loans.AddAsync(Loan);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("Getloan",new{id = LoanDto.id}, Loan); // Status code = 201
       
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        public async Task<ActionResult<loan>> PutEmpresa(int id, [FromBody] loanDto LoanDto){
            if (id!=LoanDto.id){
                return BadRequest("Id empresas no coincide");
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            
            
            loan Loan = _mapper.Map<loan>(LoanDto);
            _db.Update(Loan);
            await _db.SaveChangesAsync();
            return Ok(Loan);
        }   
    }
}