using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RazPedidos.Models
{
    public class PedidoGet
    {
        public int num { get; set; }
        public int codigo { get; set; }
        public string descrip { get; set; }
        public int cantd { get; set; }
        public int cajasc { get; set; }
        public float precio { get; set; }
        public string username { get; set; }
        public string idsector { get; set; }
        public int cantdrec { get; set; }
        public int cajasrec { get; set; }
        public float stock { get; set; }
        public int bultosrec { get; set; }
        public bool isCompleted { get; set; }
        public int idusuario { get; set; }

        public static explicit operator PedidoGet(ObservableCollection<PedidoGet> v)
        {
            throw new NotImplementedException();
        }
    }
}
