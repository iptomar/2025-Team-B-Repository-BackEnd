using CsvHelper.Configuration;
using IPT_Teste.Models;

namespace IPT_Teste.CsvMappings;

public class TipologiasMap : ClassMap<Tipologias>
{
    public TipologiasMap()
    {
        Map(m => m.Tipologia).Name("tipologia");
    }
}