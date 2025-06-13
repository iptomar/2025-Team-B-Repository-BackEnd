namespace IPT_Teste.Models.DTOs;

public class BlocoDTO
{
    
    public int HorarioId { get; set; }
    
    
    public int DiaDaSemana { get; set; }
    public TimeOnly HoraInicio { get; set; }
    
    public int SalaFK { get; set; }
    public int AulaFK { get; set; }
}