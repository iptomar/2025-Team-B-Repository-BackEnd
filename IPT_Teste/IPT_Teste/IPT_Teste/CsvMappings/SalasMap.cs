using CsvHelper.Configuration;
using IPT_Teste.Models;

namespace IPT_Teste.CsvMappings;

public class SalasMap : ClassMap<Salas>
{
    public SalasMap()
    {
        Map(m => m.Sala).Name("sala");
        Map(m => m.LocalidadeFK).Name("localidade_id");
    }
}