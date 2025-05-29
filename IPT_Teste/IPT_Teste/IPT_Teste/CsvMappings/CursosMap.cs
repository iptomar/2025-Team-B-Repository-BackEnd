using CsvHelper.Configuration;
using IPT_Teste.Models;

namespace IPT_Teste.CsvMappings;

public class CursosMap : ClassMap<Cursos>
{
    public CursosMap()
    {
        Map(m => m.Curso).Name("curso");
        Map(m => m.AnoLetivoFK).Name("ano_letivo");
        Map(m => m.InstituicaoFK).Name("instituicao");
        Map(m => m.GrauFK).Name("grau");
        Map(m => m.ProfessorFK).Name("professor");
    }
}