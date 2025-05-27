using IPT_Teste.Data;
using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using System.Globalization;
using IPT_Teste.CsvMappings;
using IPT_Teste.Models;
using Microsoft.EntityFrameworkCore;

namespace IPT_Teste.Controllers.API;

[Route("api/[controller]")]
[ApiController]
public class CsvController : Controller
{
    private readonly ApplicationDbContext _context;
    
    /***
     * Adição de injeção da DB.
     */
    public CsvController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok("Hello World");
    }
    
    /***
     * Esta função estará responsavél pela importação do CSV.
     * 
     */
    [HttpPost]
    public async Task<IActionResult> ImportCsv(List<IFormFile> files)
    {
        // Verificação de inserção dos ficheiros e se existe pelo menos 1
        if (files == null || !files.Any()) return BadRequest("Nenhum arquivo enviado.");

        // vamos percorrer a lista de ficheiros de forma a obter informação de cada um
        foreach (var file in files)
        {
            // caso não sejá CSV é automaticamente passado a frente
            if (!file.FileName.EndsWith(".csv"))
                continue;
            
            using var reader = new StreamReader(file.OpenReadStream());
            using var csvPreview = new CsvReader(reader, CultureInfo.InvariantCulture);
            csvPreview.Read();
            csvPreview.ReadHeader();

            var headers = csvPreview.HeaderRecord.Select(h => h.ToLower()).ToList();
            
            // Caso seja uma localidade
            if (headers.Contains("localidade"))
            {
                csvPreview.Context.RegisterClassMap<LocalidadesMap>();
                var localidades = csvPreview.GetRecords<Localidades>().ToList();
                
                // Nomes que já existem na DB
                var nomesExistentes = await _context.Localidades
                    .Select(l => l.Localidade.ToLower()).ToListAsync();
                // Verificamos se nas localidades recebidas existe alguma que já esteja na DB e retiramos
                var localidadesFiltradas = localidades
                    .Where(l => !nomesExistentes.Contains(l.Localidade.ToLower()))
                    .ToList();

                // caso existam localidades novas, vamos adiciona-las...
                if (localidadesFiltradas.Any())
                {
                    _context.Localidades.AddRange(localidadesFiltradas);
                    await _context.SaveChangesAsync();
                }
            }
            // Caso seja uma sala
            else if (headers.Contains("sala") && headers.Contains("localidade_id"))
            {
                csvPreview.Context.RegisterClassMap<SalasMap>();
                var salas = csvPreview.GetRecords<Salas>().ToList();
               
                // Nomes que já existem na DB
                var nomesExistentes = await _context.Salas
                    .Select(l => l.Sala.ToLower()).ToListAsync();
                // Verificamos se nas salas recebidas existe alguma que já esteja na DB e retiramos
                var salasFiltradas = salas
                    .Where(l => !nomesExistentes.Contains(l.Sala.ToLower()))
                    .ToList();

                // caso existam salas novas, vamos adiciona-las...
                if (salasFiltradas.Any())
                {
                    _context.Salas.AddRange(salasFiltradas);
                    await _context.SaveChangesAsync();
                }
            }
            // caso sejam os cursos
            else if (headers.Contains("curso") && headers.Contains("ano_letivo") && headers.Contains("instituicao") && headers.Contains("grau") && headers.Contains("professor"))
            {
                csvPreview.Context.RegisterClassMap<CursosMap>();
                var cursos = csvPreview.GetRecords<Cursos>().ToList();
               
                // Nomes que já existem na DB
                var nomesExistentes = await _context.Cursos
                    .Select(l => l.Curso.ToLower()).ToListAsync();
                // Verificamos se nas cadeiras recebidas existe alguma que já esteja na DB e retiramos
                var cursosFiltrados = cursos
                    .Where(l => !nomesExistentes.Contains(l.Curso.ToLower()))
                    .ToList();

                // caso existam cadeiras novas, vamos adiciona-los...
                if (cursosFiltrados.Any())
                {
                    _context.Cursos.AddRange(cursosFiltrados);
                    await _context.SaveChangesAsync();
                }
            }
            //caso seja os graus
            else if (headers.Contains("grau"))
            {
                csvPreview.Context.RegisterClassMap<GrausMap>();
                var graus = csvPreview.GetRecords<Graus>().ToList();
                
                
                // Nomes que já existem na DB
                var nomesExistentes = await _context.Graus
                    .Select(l => l.Grau.ToLower()).ToListAsync();
                // Verificamos se nos graus recebidos existe algum que já esteja na DB e retiramos
                var grausFiltrados = graus
                    .Where(l => !nomesExistentes.Contains(l.Grau.ToLower()))
                    .ToList();

                // caso existam graus novos, vamos adiciona-los...
                if (grausFiltrados.Any())
                {
                    _context.Graus.AddRange(grausFiltrados);
                    await _context.SaveChangesAsync();
                }
            }
            // caso seja as instituicoes
            else if (headers.Contains("instituicao") && headers.Contains("localidade_id"))
            {
                csvPreview.Context.RegisterClassMap<InstituicoesMap>();
                var instituicoes = csvPreview.GetRecords<Instituicoes>().ToList();
              
                // Nomes que já existem na DB
                var nomesExistentes = await _context.Instituicoes
                    .Select(l => l.Instituicao.ToLower()).ToListAsync();
                // Verificamos se nas instituicoes recebidas existe alguma que já esteja na DB e retiramos
                var instituicoesFiltradas = instituicoes
                    .Where(l => !nomesExistentes.Contains(l.Instituicao.ToLower()))
                    .ToList();

                // caso existam instituicoes novas, vamos adiciona-los...
                if (instituicoesFiltradas.Any())
                {
                    _context.Instituicoes.AddRange(instituicoesFiltradas);
                    await _context.SaveChangesAsync();
                }
            }
            // caso seja as tipologias
            else if (headers.Contains("tipologia"))
            {
                csvPreview.Context.RegisterClassMap<TipologiasMap>();
                var tipologias = csvPreview.GetRecords<Tipologias>().ToList();
               
                // Nomes que já existem na DB
                var nomesExistentes = await _context.Tipologias
                    .Select(l => l.Tipologia.ToLower()).ToListAsync();
                // Verificamos se nas tipologias recebidas existe alguma que já esteja na DB e retiramos
                var tipologiasFiltradas = tipologias
                    .Where(l => !nomesExistentes.Contains(l.Tipologia.ToLower()))
                    .ToList();

                // caso existam tipologias novas, vamos adiciona-los...
                if (tipologiasFiltradas.Any())
                {
                    _context.Tipologias.AddRange(tipologiasFiltradas);
                    await _context.SaveChangesAsync();
                }
            }
            // caso sejam as cadeiras
            else if (headers.Contains("cadeira") && headers.Contains("ano") && headers.Contains("semestre") && headers.Contains("ects"))
            {
                csvPreview.Context.RegisterClassMap<CadeirasMap>();
                var cadeiras = csvPreview.GetRecords<Cadeiras>().ToList();
               
                // Nomes que já existem na DB
                var nomesExistentes = await _context.Cadeiras
                    .Select(l => l.Cadeira.ToLower()).ToListAsync();
                // Verificamos se nas cadeiras recebidas existe alguma que já esteja na DB e retiramos
                var cadeirasFiltradas = cadeiras
                    .Where(l => !nomesExistentes.Contains(l.Cadeira.ToLower()))
                    .ToList();

                // caso existam cadeiras novas, vamos adiciona-los...
                if (cadeirasFiltradas.Any())
                {
                    _context.Cadeiras.AddRange(cadeirasFiltradas);
                    await _context.SaveChangesAsync();
                }
            }
        }
        
        return Ok("Ficheiros importados com sucesso.");
    }

}