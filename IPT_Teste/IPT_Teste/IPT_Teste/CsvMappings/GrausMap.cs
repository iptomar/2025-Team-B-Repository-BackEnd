using CsvHelper.Configuration;
using IPT_Teste.Models;

namespace IPT_Teste.CsvMappings;

public class GrausMap : ClassMap<Graus>
{
    public GrausMap() 
    {
      Map(m => m.Grau).Name("grau");  
    }
}