namespace IPT_Teste.Models.DTOs;

public class Horario2DTO
{
    public int Id { get; set; }

    public required DateOnly Inicio { get; set; }

    public required DateOnly Fim { get; set; }

    public required int TurmaFK { get; set; }

    public List<int> Blocos { get; set; } 
}