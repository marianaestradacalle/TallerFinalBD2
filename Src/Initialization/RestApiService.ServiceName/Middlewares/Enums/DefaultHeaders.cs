using System.ComponentModel;

namespace RestApi.Middlewares.Enums;
public enum DefaultHeaders
{
    [Description("sc-origen|Ambiente en el que esta desplegado el servicio, ejemplos: Development, Qa, Staging, Production.")]
    SCOrigen,

    [Description("sc-location|Se envían las coordenadas (latitud, longitud) separadas por coma (,), ejemplo: -7.104,-1.102.")]
    SCLocation,

    [Description("country|Sigla del país desde el cuál se esta enviando la petición, ejemplo: co")]
    Country,
}