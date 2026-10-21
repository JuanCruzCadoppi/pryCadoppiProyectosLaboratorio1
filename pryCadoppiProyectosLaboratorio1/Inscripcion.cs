using System.Text.Json;

namespace pryCadoppiProyectosLaboratorio1;

public sealed class Inscripcion
{
    public DateTime Fecha { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Edad { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public string Plan { get; set; } = string.Empty;
    public string Horario { get; set; } = string.Empty;
    public int Meses { get; set; }
    public bool ClasesGrupales { get; set; }
    public bool EvaluacionInicial { get; set; }
    public string FormaPago { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
    public decimal Descuento { get; set; }
    public decimal Total { get; set; }
    public decimal ValorCuota { get; set; }
}

public static class RepositorioInscripciones
{
    private static readonly string Carpeta = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "ModoFit");
    private static readonly string Ruta = Path.Combine(Carpeta, "inscripciones.json");
    private static readonly JsonSerializerOptions Opciones = new() { WriteIndented = true };

    public static List<Inscripcion> ObtenerTodas()
    {
        if (!File.Exists(Ruta)) return [];

        try
        {
            return JsonSerializer.Deserialize<List<Inscripcion>>(File.ReadAllText(Ruta), Opciones) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }

    public static void Guardar(Inscripcion inscripcion)
    {
        Directory.CreateDirectory(Carpeta);
        var inscripciones = ObtenerTodas();
        inscripciones.Add(inscripcion);
        File.WriteAllText(Ruta, JsonSerializer.Serialize(inscripciones, Opciones));
    }
}
