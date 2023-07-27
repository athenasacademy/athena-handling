using AthenasAcademy.Handling.MessageEvents;

namespace AthenasAcademy.Handling.Services.Interfaces;

public interface IBoletoAlunoService
{
    Task<bool> GerarBoletoPDF(BoletoEventMessage boletoEvent);
} 