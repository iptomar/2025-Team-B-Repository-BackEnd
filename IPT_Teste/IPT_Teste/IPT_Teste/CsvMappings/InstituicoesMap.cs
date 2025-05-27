using CsvHelper.Configuration;
using IPT_Teste.Models;

namespace IPT_Teste.CsvMappings;

public class InstituicoesMap : ClassMap<Instituicoes>
{
    public InstituicoesMap()
    {
        Map(m => m.Instituicao).Name("instituicao");
        Map(m => m.LocalidadeFK).Name("localidade_id");
    }
}