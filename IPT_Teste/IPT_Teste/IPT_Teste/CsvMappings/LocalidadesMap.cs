using CsvHelper.Configuration;
using IPT_Teste.Models;

namespace IPT_Teste.CsvMappings;

public class LocalidadesMap : ClassMap<Localidades>
{
    public LocalidadesMap()
    {
        Map(m => m.Localidade).Name("localidade"); // este nome é o que vai estar no CSV
    }
}