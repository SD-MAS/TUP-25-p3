using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// === Formato de entrada ===
//{ 
//  "contents": [{
//      "parts": [{ "text": "string" }],
//      "role": "user"
//  }]
//}

// === Formato de salida ===
//{
//  "candidates": [
//     { 
//          "content": { 
//              "parts": [{ "text": "string"}],
//              "role": "assistant"
//          }
//      }
//   ]
// }

public record MessageResponse(List<Candidate> Candidates);
public record ContentRequest(List<Content> Contents);
public record Candidate(Content Content);
public record Content(List<Part> Parts, string Role);
public record Part(string Text);

var opciones = new JsonSerializerOptions { 
    PropertyNameCaseInsensitive = true,                 // Para la deserialización 
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase   // Para la serialización (convierte a camelCase)
};

const string ApiKey = "AIzaSyDNb3IWwI5tjnGXj4EFAsih0HJww1HgN7M";
const string Modelo = "gemini-2.0-flash";

async Task<string> Completar(string indicacion) {
    string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{Modelo}:generateContent?key={ApiKey}";

    // El payload debe tener la estructura esperada por la API
    var payload = new ContentRequest(
        new List<Content> {
            new Content( new List<Part> { new Part(indicacion) }, "user")
        }
    );

    using var httpClient = new HttpClient();
    var json = JsonSerializer.Serialize(payload, opciones);
    using var content = new StringContent(json, Encoding.UTF8, "application/json");

    try {
        var respuesta = await httpClient.PostAsync(endpoint, content);
        var texto = await respuesta.Content.ReadAsStringAsync();
        respuesta.EnsureSuccessStatusCode();
        var r = JsonSerializer.Deserialize<MessageResponse>(texto, opciones);
        return r.Candidates[0].Content.Parts[0].Text;
    } catch (Exception e) {
        return $"Error: {e.Message}";
    }
}

var respuesta = await Completar("Hace la version mas simple de la funcion factorial en C#");

Console.WriteLine(respuesta);
File.WriteAllText("respuesta.md", respuesta);
