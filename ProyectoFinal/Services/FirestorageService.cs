using System.Net.Http;
using System.Text;
using System.Text.Json;

public class FirestoreService
{
    private readonly HttpClient _http;
    private readonly string projectId = "programacionavanzada2026"; // 👈 CAMBIA SI ES NECESARIO

    public FirestoreService(HttpClient http)
    {
        _http = http;
    }

    public async Task GuardarLog(string coleccion, string usuario, string accion)
    {
        var url = $"https://firestore.googleapis.com/v1/projects/{projectId}/databases/(default)/documents/{coleccion}";

        var data = new
        {
            fields = new
            {
                usuario = new { stringValue = usuario },
                accion = new { stringValue = accion },
                fecha = new { stringValue = DateTime.Now.ToString("s") }
            }
        };

        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _http.PostAsync(url, content);

        var result = await response.Content.ReadAsStringAsync();

        
        Console.WriteLine("Firestore response: " + result);
    }
}