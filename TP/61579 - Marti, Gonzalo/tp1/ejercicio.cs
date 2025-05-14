using System;       // Para usar la consola  (Console)
using System.IO;    // Para leer archivos    (File)

// Ayuda: 
//   Console.Clear() : Borra la pantalla
//   Console.Write(texto) : Escribe texto sin salto de línea
//   Console.WriteLine(texto) : Escribe texto con salto de línea
//   Console.ReadLine() : Lee una línea de texto
//   Console.ReadKey() : Lee una tecla presionada

// File.ReadLines(origen) : Lee todas las líneas de un archivo y devuelve una lista de strings
// File.WriteLines(destino, lineas) : Escribe una lista de líneas en un archivo

// Escribir la solucion al TP1 en este archivo. (Borre el ejemplo de abajo)
Console.WriteLine("Hola, soy el ejercicio 1 del TP1 de la materia Programación 3");
Console.Write("Presionar una tecla para continuar...");
Console.ReadKey();using System;
using System.IO;

struct Contacto
{
    public int Id;
    public string Nombre;
    public string Telefono;
    public string Email;
}

class Program
{
    const int MAX_CONTACTOS = 100;
    static Contacto[] agenda = new Contacto[MAX_CONTACTOS];
    static int contador = 0;
    static string archivo = "agenda.csv";

    static void Main()
    {
        CargarContactos();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("===== AGENDA DE CONTACTOS =====");
            Console.WriteLine("1) Agregar contacto");
            Console.WriteLine("2) Modificar contacto");
            Console.WriteLine("3) Borrar contacto");
            Console.WriteLine("4) Listar contactos");
            Console.WriteLine("5) Buscar contacto");
            Console.WriteLine("0) Salir");
            Console.Write("Seleccione una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1": AgregarContacto(); break;
                case "2": ModificarContacto(); break;
                case "3": BorrarContacto(); break;
                case "4": ListarContactos(); break;
                case "5": BuscarContacto(); break;
                case "0": GuardarContactos(); return;
                default: Console.WriteLine("Opción inválida. Intente de nuevo."); break;
            }
        }
    }

    static void CargarContactos()
    {
        if (File.Exists(archivo))
        {
            string[] lineas = File.ReadAllLines(archivo);
            foreach (string linea in lineas)
            {
                string[] datos = linea.Split(',');
                if (contador < MAX_CONTACTOS)
                {
                    agenda[contador] = new Contacto
                    {
                        Id = int.Parse(datos[0]),
                        Nombre = datos[1],
                        Telefono = datos[2],
                        Email = datos[3]
                    };
                    contador++;
                }
            }
        }
    }

    static void GuardarContactos()
    {
        using (StreamWriter sw = new StreamWriter(archivo))
        {
            for (int i = 0; i < contador; i++)
            {
                sw.WriteLine($"{agenda[i].Id},{agenda[i].Nombre},{agenda[i].Telefono},{agenda[i].Email}");
            }
        }
    }

    static void AgregarContacto()
    {
        if (contador >= MAX_CONTACTOS)
        {
            Console.WriteLine("No se pueden agregar más contactos.");
            Console.ReadKey();
            return;
        }

        Contacto nuevo;
        nuevo.Id = contador + 1;
        Console.Write("Nombre: "); nuevo.Nombre = Console.ReadLine();
        Console.Write("Teléfono: "); nuevo.Telefono = Console.ReadLine();
        Console.Write("Email: "); nuevo.Email = Console.ReadLine();

        agenda[contador] = nuevo;
        contador++;
        Console.WriteLine($"Contacto agregado con ID = {nuevo.Id}");
        Console.ReadKey();
    }

    static void ModificarContacto()
    {
        Console.Write("Ingrese el ID del contacto a modificar: ");
        int id = int.Parse(Console.ReadLine());
        for (int i = 0; i < contador; i++)
        {
            if (agenda[i].Id == id)
            {
                Console.WriteLine($"Datos actuales => Nombre: {agenda[i].Nombre}, Teléfono: {agenda[i].Telefono}, Email: {agenda[i].Email}");
                Console.Write("Nuevo Nombre (deje vacío para mantener): ");
                string nombre = Console.ReadLine();
                if (!string.IsNullOrEmpty(nombre)) agenda[i].Nombre = nombre;
                
                Console.Write("Nuevo Teléfono (deje vacío para mantener): ");
                string telefono = Console.ReadLine();
                if (!string.IsNullOrEmpty(telefono)) agenda[i].Telefono = telefono;
                
                Console.Write("Nuevo Email (deje vacío para mantener): ");
                string email = Console.ReadLine();
                if (!string.IsNullOrEmpty(email)) agenda[i].Email = email;
                
                Console.WriteLine("Contacto modificado con éxito.");
                Console.ReadKey();
                return;
            }
        }
        Console.WriteLine("ID no encontrado.");
        Console.ReadKey();
    }

    static void BorrarContacto()
    {
        Console.Write("Ingrese el ID del contacto a borrar: ");
        int id = int.Parse(Console.ReadLine());
        for (int i = 0; i < contador; i++)
        {
            if (agenda[i].Id == id)
            {
                for (int j = i; j < contador - 1; j++)
                {
                    agenda[j] = agenda[j + 1];
                }
                contador--;
                Console.WriteLine($"Contacto con ID={id} eliminado con éxito.");
                Console.ReadKey();
                return;
            }
        }
        Console.WriteLine("ID no encontrado.");
        Console.ReadKey();
    }

    static void ListarContactos()
    {
        Console.WriteLine("=== Lista de Contactos ===");
        Console.WriteLine("ID    NOMBRE               TELÉFONO       EMAIL");
        for (int i = 0; i < contador; i++)
        {
            Console.WriteLine($"{agenda[i].Id,-5} {agenda[i].Nombre,-20} {agenda[i].Telefono,-15} {agenda[i].Email,-25}");
        }
        Console.ReadKey();
    }

    static void BuscarContacto()
    {
        Console.Write("Ingrese un término de búsqueda: ");
        string termino = Console.ReadLine().ToLower();
        Console.WriteLine("Resultados de la búsqueda:");
        Console.WriteLine("ID    NOMBRE               TELÉFONO       EMAIL");
        for (int i = 0; i < contador; i++)
        {
            if (agenda[i].Nombre.ToLower().Contains(termino) ||
                agenda[i].Telefono.Contains(termino) ||
                agenda[i].Email.ToLower().Contains(termino))
            {
                Console.WriteLine($"{agenda[i].Id,-5} {agenda[i].Nombre,-20} {agenda[i].Telefono,-15} {agenda[i].Email,-25}");
            }
        }
        Console.ReadKey();
    }
}