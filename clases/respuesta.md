```csharp
public static int Factorial(int n)
{
    if (n == 0)
    {
        return 1;
    }
    else
    {
        return n * Factorial(n - 1);
    }
}
```

**Explicación:**

* **`public static int Factorial(int n)`:**  Define una función pública y estática llamada `Factorial` que toma un entero `n` como entrada y devuelve un entero (el factorial).  `static` permite llamar a la función directamente desde la clase sin necesidad de crear una instancia.
* **`if (n == 0)`:** Caso base de la recursión.  El factorial de 0 es 1.  Este es crucial para que la recursión se detenga.
* **`return 1;`:**  Devuelve 1 cuando `n` es 0.
* **`else`:** Si `n` no es 0, ejecuta el código siguiente.
* **`return n * Factorial(n - 1);`:**  Esta es la llamada recursiva. Calcula el factorial multiplicando `n` por el factorial de `n-1`.  Esto continúa hasta que `n` llega a 0.

**Cómo funciona la recursión:**

La función se llama a sí misma con un valor de `n` decreciente hasta que `n` llega a 0. Luego, las llamadas a la función se "desenrollan" desde el caso base, multiplicando los valores a medida que regresan.

**Ejemplo:**

Si llamamos `Factorial(4)`, esto es lo que pasa:

1. `Factorial(4)` devuelve `4 * Factorial(3)`
2. `Factorial(3)` devuelve `3 * Factorial(2)`
3. `Factorial(2)` devuelve `2 * Factorial(1)`
4. `Factorial(1)` devuelve `1 * Factorial(0)`
5. `Factorial(0)` devuelve `1` (caso base)

Luego, las llamadas se resuelven:

* `Factorial(1)` devuelve `1 * 1 = 1`
* `Factorial(2)` devuelve `2 * 1 = 2`
* `Factorial(3)` devuelve `3 * 2 = 6`
* `Factorial(4)` devuelve `4 * 6 = 24`

Por lo tanto, `Factorial(4)` devuelve 24.

**Consideraciones:**

* **Overflow:** El factorial crece muy rápido. Para números grandes de `n`, el resultado puede exceder la capacidad de un `int`. Para manejar números más grandes, podrías considerar usar el tipo de dato `long` o `BigInteger`.
* **Negativos:**  El factorial no está definido para números negativos.  Podrías agregar una verificación al principio de la función para lanzar una excepción si `n` es negativo:

   ```csharp
   if (n < 0)
   {
       throw new ArgumentException("El factorial no está definido para números negativos.");
   }
   ```

Esta versión es la más simple y directa para comprender el concepto de factorial y la recursión en C#.  Para producción, considera las consideraciones de overflow y manejo de entradas negativas.
