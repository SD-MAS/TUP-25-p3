using System;
using System.Collections.Generic;
using servidor.Data;

namespace servidor.Entidades
{
    public class Compra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public List<ItemCompra> Items { get; set; }
        public double Total { get; set; }
    }
}
