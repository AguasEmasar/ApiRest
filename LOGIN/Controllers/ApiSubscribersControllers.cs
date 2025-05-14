using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using LOGIN.Services.Interfaces;
using LOGIN.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace LOGIN.Controllers
{
    [Route("api/subscribers")]
    [ApiController]
    public class ApiSubscribersControllers : ControllerBase
    {
        private readonly IAPiSubscriberServices _apiSubscriberServices;

        public ApiSubscribersControllers(IAPiSubscriberServices apiSubscriberServices)
        {
            _apiSubscriberServices = apiSubscriberServices;
        }

        [HttpGet("buscar-abonado/{clave}")]
        public async Task<IActionResult> GetAbonado(string clave)
        {
            try
            {
                var json = await _apiSubscriberServices.GetUserAsync(clave);
                var abonado = JsonConvert.DeserializeObject<Suscriber>(json);

                if (abonado == null)
                    return NotFound(new { message = "No se encontró el abonado con la clave especificada." });

                var result = new
                {
                    abonado.clave,
                    nombre_abonado = MaskName(abonado.nombreabonado),
                    abonado.totalMora,
                };

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el abonado.", error = ex.Message });
            }
        }

        [HttpGet("buscar-abonado-completo/{clave}")]
        [Authorize]
        public async Task<IActionResult> GetAbonadoCompleto(string clave)
        {
            try
            {
                var json = await _apiSubscriberServices.GetUserAsync(clave);
                var abonado = JsonConvert.DeserializeObject<Suscriber>(json);

                if (abonado == null)
                    return NotFound(new { message = "No se encontró el abonado con la clave especificada." });

                return Ok(abonado);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el abonado completo.", error = ex.Message });
            }
        }

        [HttpGet("comentario/{clave}")]
        [Authorize]
        public async Task<IActionResult> GetComment(string clave)
        {
            try
            {
                var json = await _apiSubscriberServices.GetCommentAsync(clave);
                var comments = JsonConvert.DeserializeObject<List<Comment>>(json);

                if (comments == null || !comments.Any())
                    return NotFound(new { message = "No se encontraron los comentarios con la clave especificada" });

                return Ok(comments);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener los comentarios", error = ex.Message });
            }
        }

        [HttpGet("historial/{clave}")]
        [Authorize]
        public async Task<IActionResult> GetHistory(string clave)
        {
            try
            {
                var json = await _apiSubscriberServices.GetHistoryAsync(clave);
                var history = JsonConvert.DeserializeObject<List<History>>(json);

                if (history == null || !history.Any())
                {
                    return NotFound(new { message = "No se encontró el historial con la clave especificada" });
                }

                // Ordena por fecha de pago (más reciente primero)
                var orderedHistory = history.OrderByDescending(h => h.fechaPago).ToList();

                // Obtiene los últimos 50 registros antes de eliminar duplicados
                var last50 = orderedHistory.Take(50).ToList();

                //Elimina duplicados en base al número de recibo dentro de los primeros 50 registros
                var historyFiltered = last50
                   .GroupBy(h => h.recibo)
                   .Select(g => g.First())
                   .ToList();

                return Ok(historyFiltered);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al obtener el historial", error = ex.Message });
            }
        }

        private string MaskName(string fullName)
        {
            var parts = fullName.Split(' ');
            if (parts.Length < 2) return fullName;

            var firstName = parts[0];
            var lastName = parts[1];

            string MaskString(string str)
            {
                if (str.Length <= 3)
                    return str[0] + new string('*', str.Length - 1);
                return str[0] + new string('*', str.Length - 2) + str[^1];
            }

            return $"{MaskString(firstName)} {MaskString(lastName)}";
        }

    }
}
