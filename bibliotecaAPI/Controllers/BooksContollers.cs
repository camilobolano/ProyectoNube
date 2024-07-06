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
    public class BooksContollers : ControllerBase
    {
         private readonly AplicationDBContext _db;
        private responseDto _response;
        private readonly ILogger<BooksContollers> _logger;
        private readonly IMapper _mapper;

        public BooksContollers(AplicationDBContext db, ILogger<BooksContollers> logger, IMapper mapper)
        {
            _logger= logger;// imprime la consola
            _mapper = mapper;//Hace un mapeo del DTO con la clase de las entidades
            _db = db;// es el contexto de la base de datos
            _response = new responseDto();            
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
         public async Task<ActionResult<List<books>>> GetBooks(){
            _logger.LogInformation("Listado de libros");
             var lista =  await _db.book.ToListAsync();
            _response.Resultado= lista;
            _response.Mensaje="Listado de libros";
             return Ok(_response);
         }

        [HttpGet("{id}", Name= "GetBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
         public async Task<ActionResult<books>> GetBook(int id){
            if  (id==0){
                    _logger.LogError("Debe enviar el ID");
                    _response.Mensaje="Debe enviar el ID";
                    _response.IsExitoso=false;
                    return BadRequest(_response);
            }

             var books =  await _db.book.FindAsync(id);

             if(books==null){
                _logger.LogError("Libro no exite");
                _response.Mensaje="Libro no exite";
                _response.IsExitoso=false;
                return NotFound(_response);
             }
            _response.Resultado= books;
            _response.Mensaje="Datos del libro";
             return Ok(_response); //Status code = 200
         }
    
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        public async Task<ActionResult<books>> PostBook([FromBody] booksDto BooksDto){
            if (BooksDto==null){
                _response.Mensaje="Información Incorrecta";
                _response.IsExitoso=false;
                return BadRequest(_response);
            }
           

            

            

            books Books = _mapper.Map<books>(BooksDto);
            await _db.book.AddAsync(Books);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetBook",new{id = BooksDto.id}, Books); // Status code = 201
       
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        public async Task<ActionResult<books>> PutEmpresa(int id, [FromBody] booksDto BooksDto){
            if (id!=BooksDto.id){
                return BadRequest("Id libro no coincide");
            }
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            
            
            
            books Books = _mapper.Map<books>(BooksDto);
            _db.Update(Books);
            await _db.SaveChangesAsync();
            return Ok(Books);
        }        

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> DeleteBook(int id)
        {
            var Books = await _db.book.FindAsync(id);
            if (Books==null){
                return NotFound();
            }
            _db.book.Remove(Books);
            await _db.SaveChangesAsync();
            return NoContent();
        }

          [HttpGet("/libro/{name}", Name= "GetBookPorNombre")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
         public async Task<ActionResult<books>> GetBookPorNombre(string name){
            if  (name==""){
                    _logger.LogError("Debe enviar el Nombre");
                    _response.Mensaje="Debe enviar el Nombre";
                    _response.IsExitoso=false;
                    return BadRequest(_response);
            }

             var Books =  await _db.book.Where(b => b.nombre.ToLower() == name.ToLower()).ToListAsync();

             if(Books==null){
                _logger.LogError("libro no exite");
                _response.Mensaje="libro no exite";
                _response.IsExitoso=false;
                return NotFound(_response);
             }
            _response.Resultado= Books;
            _response.Mensaje="Datos del libro";
             return Ok(_response); //Status code = 200
         }
    }
}