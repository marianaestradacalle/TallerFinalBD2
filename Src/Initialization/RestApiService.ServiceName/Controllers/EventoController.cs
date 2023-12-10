using Application.DTOs;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace RestApi.Controllers;

[ApiController]
[Route("api/eventos")]
public class EventoController : ControllerBase
{
    private readonly IEventoUseCase _eventoUseCases;

    public EventoController(IEventoUseCase eventoUseCases)
    {
        _eventoUseCases = eventoUseCases;
    }

    [HttpPost("post")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> CrearEvento([FromBody] EventoDTO body)
    {
        return Ok(await _eventoUseCases.CrearEvento(body));
    }

    [HttpGet("getall")]
    [ProducesResponseType(200)]
    public IActionResult ObtenerTodosLosEvento()
    {
        return Ok(_eventoUseCases.ObtenerEventos());
    }

    [HttpDelete("delete")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ObtenerEventoPoridEvento([FromQuery] string idEvento)
    {
        await _eventoUseCases.ObtenerEventoPorId(idEvento);
        return Ok();
    }

    [HttpDelete("checked-isolate-eventos")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> EliminarEvento([FromQuery] string idEvento)
    {
        await _eventoUseCases.EliminarEvento(idEvento);
        return Ok();
    }

    [HttpPost("eventos/states")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ActualizarEvento([FromBody] EventoDTO body)
    {
        await _eventoUseCases.ActualizarEvento(body);
        return Ok();
    }

}