using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DotNet_Introducao_API.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UsuarioController:ControllerBase
    {
        [HttpGet("ObterDataHoraAtual")]
        public IActionResult ObterDataHora()
        {

            var obj = new {
                Data = DateTime.Now.ToLongDateString(),
                Hora = DateTime.Now.ToShortTimeString()
            };

            return Ok(obj);
        }

        [HttpGet("Apresentar/{nome}")]
        public IActionResult Apresentar(string nome){
            var mensagem = $"Olá {nome} seja bem vindo"; //criando mensagem
            return Ok(new {mensagem}); // retornando um objeto anônimo
        }


    }
}
