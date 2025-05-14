using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

const string ApiKey = "AIzaSyDNb3IWwI5tjnGXj4EFAsih0HJww1HgN7M";
// const string Modelo = "gemini-2.5-pro-preview-05-06";
const string Modelo = "gemini-2.0-flash";

// Clases para manejar la comunicación con la API de Gemini
public record MessageResponse(List<Candidate> Candidates);
public record ContentRequest(List<Content> Contents);
public record Candidate(Content Content);
public record Content(List<Part> Parts, string Role);
public record Part(string Text);

// Crear un historial vacío para la conversación
var historialChat = new List<Content>();

var opciones = new JsonSerializerOptions { 
    PropertyNameCaseInsensitive = true,  // Para la deserialización 
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase  // Para la serialización (convierte a camelCase)
};

// Función que envía el historial completo y obtiene la respuesta de Gemini
async Task<string> CompletarChat(List<Content> historial) {
    string endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/{Modelo}:generateContent?key={ApiKey}";

    // El payload incluye todo el historial de la conversación
    var payload = new ContentRequest(historial);

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

// Añadir mensaje del usuario al historial
void AgregarMensajeUsuario(string mensaje) {
    historialChat.Add(new Content(new List<Part> { new Part(mensaje) }, "user"));
}

// Añadir respuesta de la IA al historial
void AgregarRespuestaIA(string respuesta) {
    historialChat.Add(new Content(new List<Part> { new Part(respuesta) }, "model"));
}

// Establecer un mensaje inicial para dar contexto a la conversación
AgregarMensajeUsuario("""
    Eres un asistente amigable y conversacional que responde a todas mis 
    preguntas de manera clara y concisa. 
    
    Eres un experto programador en c#, cuando te pida ayuda con un código 
    entrega el ejemplo completo y funcional sin expicaciones de como funciona.
    Si no sabes la respuesta a una pregunta, simplemente di "No lo sé".
    
    # Instrucciones para programa.
        * Siempre que sea posible entrega el codigo para que pueda 
          correr con `dotnet script`.
        * Cuando usar '{` asegurate el mismo este a la derecha de la sentencias 
          en lugar de estar abajo. En el caso del `else` debe estar en la misma linea.
    
    # Instrucciones para examen.
        * Cuando te pide que hagas un examen mostra 5 preguntas multiples choice con 3 
          opciones de respuesta cada una. Solo una respuesta deberá ser correcta.
        * No muestres las respuestas correctas, solo el examen. 
        * Cuando el usuario responda decile la nota y si es incorrecta la respuesta explicale porque.
    
    *Importante* Responde en español.
""");

// Obtener y guardar la respuesta inicial
var respuestaInicial = await CompletarChat(historialChat);
AgregarRespuestaIA(respuestaInicial);

// Mostrar información inicial para el usuario
Console.WriteLine("=== Chat con Gemini ===");
Console.WriteLine("(Escribe 'salir' para terminar la conversación)");
Console.WriteLine();
Console.WriteLine($"IA: {respuestaInicial}");

// Bucle principal del chat
while (true) {
    Console.WriteLine();
    Console.Write("Tú: ");
    string entradaUsuario = Console.ReadLine();
    
    // Verificar si el usuario quiere salir
    if (string.IsNullOrWhiteSpace(entradaUsuario) || 
        entradaUsuario.Trim().Equals("salir", StringComparison.OrdinalIgnoreCase)) {
        Console.WriteLine("Finalizando el chat. ¡Hasta pronto!");
        break;
    }
    
    // Agregar la entrada del usuario al historial
    AgregarMensajeUsuario(entradaUsuario);
    
    // Obtener la respuesta de la IA
    var respuestaIA = await CompletarChat(historialChat);
    if(respuestaIA.StartsWith("```")) respuestaIA = "\n" + respuestaIA;
    // Agregar la respuesta al historial
    AgregarRespuestaIA(respuestaIA);
    
    // Mostrar la respuesta
    Console.WriteLine($"\nIA: {respuestaIA}");

    // Opcional: guardar toda la conversación en un archivo después de cada interacción
    File.WriteAllText("conversacion.md", ConvertirHistorialAMarkdown(historialChat));
}

// Función para convertir el historial a formato Markdown para guardar
string ConvertirHistorialAMarkdown(List<Content> historial) {
    var sb = new StringBuilder();
    sb.AppendLine("# Conversación con Gemini");
    sb.AppendLine();
    
    foreach (var mensaje in historial) {
        string autor = mensaje.Role.Equals("user", StringComparison.OrdinalIgnoreCase) ? "**Tú**" : "**IA**";
        sb.AppendLine($"{autor}: {mensaje.Parts[0].Text}");
        sb.AppendLine();
    }
    
    return sb.ToString();
}
