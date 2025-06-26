using CsvHelper.Configuration;
using IPT_Teste.Models;

namespace IPT_Teste.CsvMappings;

public class CadeirasMap : ClassMap<Cadeiras>
{
    public CadeirasMap()
    {
        Map(m => m.Cadeira).Name("cadeira");
        Map(m => m.Ano).Name("ano");
        Map(m => m.Semestre).Name("semestre");
        Map(m => m.ECTS).Name("ects");
    }
}