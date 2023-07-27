using AthenasAcademy.Handling.MessageEvents;

namespace AthenasAcademy.Handling.Services.Interfaces;

public interface IContratoAlunoService
{
    Task<bool> GerarContratoPDF(ContratoMessageEvent contratoEvent);
}