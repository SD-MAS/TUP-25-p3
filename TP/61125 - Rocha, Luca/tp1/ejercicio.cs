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
using System;
using System.IO;

struct Contacto
{
    public int Id;
    public string Nombre;
    public string Apellido;
    public string Telefono;
    public string Email;
}

class Program
{
    static Contacto[] contactos = new Contacto[100];
    static int contador_contactos = 0;

    static void Main()
    {
        int opcion;
        do
        {
            Console.Clear();
            Console.WriteLine("===== AGENDA DE CONTACTOS =====");
            Console.WriteLine("1) Agregar contacto");
            Console.WriteLine("2) Modificar contacto");
            Console.WriteLine("3) Borrar contacto");
            Console.WriteLine("4) Listar contactos");
            Console.WriteLine("5) Buscar contacto");
            Console.WriteLine("6) Salir");
            Console.Write("Seleccione una opción: ");

            string input = Console.ReadLine() ?? "0";
            opcion = int.TryParse(input, out int result) ? result : 0;

            switch (opcion)
            {
                case 1:
                    AgregarContacto();
                    break;
                case 2:
                    ModificarContacto();
                    break;
                case 3:
                    BorrarContacto();
                    break;
                case 4:
                    ListarContactos();
                    break;
                case 5:
                    BuscarContacto();
                    break;
                case 6:
                    Console.WriteLine("Saliendo...");
                    break;
                default:
                    Console.WriteLine("Opción inválida. Intente nuevamente.");
                    break;
            }

            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();

        } while (opcion != 6);
    }

    static void AgregarContacto()
    {
        Console.Clear();
        Console.WriteLine("===== AGREGAR CONTACTO =====");
        Console.Write("Nombre: ");
        string nombre = Console.ReadLine() ?? "";
        Console.Write("Apellido: ");
        string apellido = Console.ReadLine() ?? "";
        Console.Write("Teléfono: ");
        string telefono = Console.ReadLine() ?? "";
        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        contactos[contador_contactos] = new Contacto
        {
            Id = contador_contactos + 1,
            Nombre = nombre,
            Apellido = apellido,
            Telefono = telefono,
            Email = email
        };

        contador_contactos++;
        Console.WriteLine("Contacto agregado correctamente.");
    }

    static void ModificarContacto()
    {
        Console.Clear();
        Console.WriteLine("===== MODIFICAR CONTACTO =====");
        Console.Write("Ingrese el ID del contacto a modificar: ");

        string input = Console.ReadLine() ?? "0";
        int id = int.TryParse(input, out int result) ? result : -1;
        int index = BuscarIndicePorId(id);
        if (index == -1)
        {
            Console.WriteLine("ID inválido. Intente nuevamente.");
            return;
        }

        Console.WriteLine("Contacto encontrado:");
        Console.Write("Nuevo Nombre: ");
        string nombre = Console.ReadLine() ?? "";
        Console.Write("Nuevo Apellido: ");
        string apellido = Console.ReadLine() ?? "";
        Console.Write("Nuevo Teléfono: ");
        string telefono = Console.ReadLine() ?? "";
        Console.Write("Nuevo Email: ");
        string email = Console.ReadLine() ?? "";

        contactos[index].Nombre = nombre;
        contactos[index].Apellido = apellido;
        contactos[index].Telefono = telefono;
        contactos[index].Email = email;

        Console.WriteLine("Contacto modificado con éxito.");
    }

    static void BorrarContacto()
    {
        if (contador_contactos == 0)
        {
            Console.WriteLine("No hay contactos para eliminar.");
            Console.ReadKey();
            return;
        }

        Console.Clear();
        Console.WriteLine("===== BORRAR CONTACTO =====");
        Console.Write("Ingrese el ID del contacto a borrar: ");

        string input = Console.ReadLine() ?? "0";
        int id = int.TryParse(input, out int result) ? result : -1;

        int index = BuscarIndicePorId(id);
        if (index == -1)
        {
            Console.WriteLine("ID inválido. Intente nuevamente.");
            return;
        }

        // Mover último contacto a la posición eliminada
        contactos[index] = contactos[contador_contactos - 1];
        contador_contactos--;

        Console.WriteLine($"Contacto con ID = {id} eliminado con éxito.");
    }

    static void ListarContactos()
    {
        Console.Clear();
        Console.WriteLine("===== LISTAR CONTACTOS =====");
        Console.WriteLine($"{"ID",-5}{"Nombre",-15}{"Apellido",-15}{"Teléfono",-15}{"Email",-25}");
        Console.WriteLine(new string('-', 75));

        if (contador_contactos == 0)
        {
            Console.WriteLine("No hay contactos para mostrar.");
        }
        else
        {
            for (int i = 0; i < contador_contactos; i++)
            {
                Console.WriteLine($"{contactos[i].Id}{contactos[i].Nombre}{contactos[i].Apellido}{contactos[i].Telefono}{contactos[i].Email}");
            }
        }
    }

    static void BuscarContacto()
    {
        Console.Clear();
        Console.WriteLine("===== BUSCAR CONTACTO =====");
        Console.Write("Ingrese el ID del contacto a buscar: ");

        string input = Console.ReadLine() ?? "0";
        int id = int.TryParse(input, out int result) ? result : -1;

        int index = BuscarIndicePorId(id);
        if (index == -1)
        {
            Console.WriteLine($"No se encontró un contacto con ID = {id}");
        }
        else
        {
            var c = contactos[index];
            Console.WriteLine($"Contacto encontrado: {c.Nombre} {c.Apellido}, Teléfono: {c.Telefono}, Email: {c.Email}");
        }
    }

    static int BuscarIndicePorId(int id)
    {
        for (int i = 0; i < contador_contactos; i++)
        {
            if (contactos[i].Id == id)
            {
                return i;
            }
        }
        return -1;
    }
}