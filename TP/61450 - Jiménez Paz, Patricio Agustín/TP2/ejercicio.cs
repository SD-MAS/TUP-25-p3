// TP2: Sistema de Cuentas Bancarias
//
using System.Collections.Generic;
// Implementar un sistema de cuentas bancarias que permita realizar operaciones como depósitos, retiros, transferencias y pagos.

class Banco
{
    public string Nombre { get; private set; }
    public List<Cliente> Clientes { get; private set; }
    public List<Operacion> Operaciones { get; private set; }
    public static Dictionary<string, Cuenta> Cuentas = new Dictionary<string, Cuenta>();

    public Banco(string nombre)
    {
        Nombre = nombre;
        Clientes = new List<Cliente>();
        Operaciones = new List<Operacion>();
    }

    public void Agregar(Cliente cliente)
    {
        Clientes.Add(cliente);
    }

    public void Registrar(Operacion operacion)
    {
        operacion.EjecutarOperacion();
        Operaciones.Add(operacion);
    }

    public void Informe()
    {
        Console.WriteLine($"Banco {Nombre} | Clientes: {Clientes.Count}\n");
        foreach (var cliente in Clientes)
        {
            cliente.Informe();
        }
    }

    public static void RegistrarCuenta(Cuenta cuenta)
    {
        if (Cuentas.ContainsKey(cuenta.Numero))
        {
            Console.WriteLine($"La cuenta {cuenta.Numero} ya existe");
        }
        else
        {
            Cuentas.Add(cuenta.Numero, cuenta);
        }
    }

    public static Cuenta BuscarCuenta(string numeroCuenta)
    {
        if (Cuentas.ContainsKey(numeroCuenta))
        {
            return Cuentas[numeroCuenta];
        }
        else
        {
            return null;
        }
    }
}
class Cliente
{
    public string Nombre { get; private set; }
    public List<Cuenta> Cuentas { get; private set; }

    public Cliente(string nombre)
    {
        Nombre = nombre;
        Cuentas = new List<Cuenta>();
    }

    public void Agregar(Cuenta cuenta)
    {
        cuenta.AsignarPropietario(this);
        Cuentas.Add(cuenta);
        Banco.RegistrarCuenta(cuenta);
    }

    public void Informe()
    {
        Console.WriteLine($"  Cliente: {Nombre} | Saldo Total: {Cuentas.Sum(c => c.Saldo):C0} | Puntos Total: {Cuentas.Sum(c => c.Puntos):C}\n");
        foreach (var cuenta in Cuentas)
        {
            cuenta.Informe();
        }
        Console.WriteLine("");
    }
}

abstract class Cuenta
{
    public string Numero { get; private set; }
    public Cliente Propietario { get; private set; }
    public decimal Saldo { get; private set; }
    public int Puntos { get; set; }
    public List<Operacion> Historial { get; private set; } = new List<Operacion>();

    public Cuenta(string numero, decimal saldo = 0)
    {
        Numero = numero;
        Saldo = saldo;
        Puntos = 0;
    }

    public void AsignarPropietario(Cliente propietario)
    {
        Propietario = propietario;
    }

    public void AgregarSaldo(decimal monto)
    {
        if (monto > 0) Saldo += monto;
    }

    public void RestarSaldo(decimal monto)
    {
        if (monto > 0 && monto <= Saldo) Saldo -= monto;
    }

    public abstract void Pagar(decimal monto);

    public void RegistrarOperacion(Operacion operacion)
    {
        if (operacion.Realizada) Historial.Add(operacion);
    }

    public void Informe()
    {
        Console.WriteLine($"    Cuenta: {Numero} | Saldo: {Saldo:C} | Puntos: {Puntos:C}");
        foreach (Operacion operacion in Historial)
        {
            Console.WriteLine($"     - {operacion.Descripcion}");
        }
        Console.WriteLine("");
    }
}
class CuentaOro : Cuenta
{
    public CuentaOro(string numero, decimal saldo = 0) : base(numero, saldo) { }
    public override void Pagar(decimal monto)
    {
        RestarSaldo(monto);
        if (monto < 1000)
        {
            Puntos += (int)(monto * (decimal)0.03);
        }
        else
        {
            Puntos += (int)(monto * (decimal)0.05);
        }
    }
}
class CuentaPlata : Cuenta
{
    public CuentaPlata(string numero, decimal saldo = 0) : base(numero, saldo) { }
    public override void Pagar(decimal monto)
    {
        RestarSaldo(monto);
        Puntos += (int)(monto * (decimal)0.02);
    }
}
class CuentaBronce : Cuenta
{
    public CuentaBronce(string numero, decimal saldo = 0) : base(numero, saldo) { }
    public override void Pagar(decimal monto)
    {
        RestarSaldo(monto);
        Puntos += (int)(monto * (decimal)0.01);
    }
}

abstract class Operacion
{
    public Cuenta Origen { get; private set; }
    public decimal Monto { get; private set; }
    public bool Realizada { get; protected set; }

    public Operacion(string numeroCuenta, decimal monto)
    {
        Origen = Banco.BuscarCuenta(numeroCuenta);
        Monto = monto;
    }

    public abstract void EjecutarOperacion();

    public virtual string Descripcion => string.Empty;
}
class Deposito : Operacion
{
    public Deposito(string numero, decimal monto) : base(numero, monto) { }
    public override void EjecutarOperacion()
    {
        if (Monto <= 0)
        {
            Realizada = false;
            return;
        }
        Origen.AgregarSaldo(Monto);
        Realizada = true;
        Origen.RegistrarOperacion(this);
    }

    public override string Descripcion => $"Depósito {Monto:C} a [{Origen.Numero}/{Origen.Propietario.Nombre}]";
}
class Retiro : Operacion
{
    public Retiro(string numero, decimal monto) : base(numero, monto) { }
    public override void EjecutarOperacion()
    {
        if (Monto > Origen.Saldo || Monto <= 0)
        {
            Realizada = false;
            return;
        }
        Origen.RestarSaldo(Monto);
        Realizada = true;
        Origen.RegistrarOperacion(this);
    }

    public override string Descripcion => $"Retiro {Monto:C} de [{Origen.Numero}/{Origen.Propietario.Nombre}]";
}
class Transferencia : Operacion
{
    public Cuenta Destino { get; private set; }

    public Transferencia(string origen, string destino, decimal monto) : base(origen, monto)
    {
        Destino = Banco.BuscarCuenta(destino);
    }
    public override void EjecutarOperacion()
    {
        if (Monto <= 0 || Origen.Saldo <= Monto)
        {
            Realizada = false;
            return;
        }
        Origen.RestarSaldo(Monto);
        Destino.AgregarSaldo(Monto);
        Realizada = true;
        Origen.RegistrarOperacion(this);
        Destino.RegistrarOperacion(this);
    }

    public override string Descripcion => $"Transferencia {Monto:C} de [{Origen.Numero}/{Origen.Propietario.Nombre}] a [{Destino.Numero}/{Destino.Propietario.Nombre}]";

    public override string ToString()
    {
        return $"{Descripcion}";
    }
}
class Pago : Operacion
{
    public Pago(string numero, decimal monto) : base(numero, monto) { }
    public override void EjecutarOperacion()
    {
        if (Monto <= 0 || Origen.Saldo <= Monto)
        {
            Realizada = false;
            return;
        }
        Origen.Pagar(Monto);
        Realizada = true;
        Origen.RegistrarOperacion(this);
    }

    public override string Descripcion => $"Pago {Monto:C} con [{Origen.Numero}/{Origen.Propietario.Nombre}]";

    public override string ToString()
    {
        return $"Origen: {Origen.Numero} - {Descripcion}";
    }
}


// Definiciones 

Cliente raul = new Cliente("Raul Perez");
raul.Agregar(new CuentaOro("10001", 1000));
raul.Agregar(new CuentaPlata("10002", 2000));

Cliente sara = new Cliente("Sara Lopez");
sara.Agregar(new CuentaPlata("10003", 3000));
sara.Agregar(new CuentaPlata("10004", 4000));

Cliente luis = new Cliente("Luis Gomez");
luis.Agregar(new CuentaBronce("10005", 5000));

Banco nac = new Banco("Banco Nac");
nac.Agregar(raul);
nac.Agregar(sara);

Banco tup = new Banco("Banco TUP");
tup.Agregar(luis);


// Registrar Operaciones
nac.Registrar(new Deposito("10001", 100));
nac.Registrar(new Retiro("10002", 200));
nac.Registrar(new Transferencia("10001", "10002", 300));
nac.Registrar(new Transferencia("10003", "10004", 500));
nac.Registrar(new Pago("10002", 400));

tup.Registrar(new Deposito("10005", 100));
tup.Registrar(new Retiro("10005", 200));
tup.Registrar(new Transferencia("10005", "10002", 300));
tup.Registrar(new Pago("10005", 400));


// Informe final
nac.Informe();
tup.Informe();

