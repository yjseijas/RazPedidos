using System;
using System.Collections.Generic;
using System.Text;

namespace RazPedidos.Models
{
    public class Pedidos
    {
        public int num { get; set; }
        public int codigo { get; set; }
        public float cantd { get; set; }
        public float precio { get; set; }
        public float bonif { get; set; }
        public float impuestos { get; set; }
        public float exentos { get; set; }
        public int tipofac { get; set; }
        public string identif { get; set; }
        public string lista { get; set; }
        public int codcli { get; set; }
        public string cajasc { get; set; }
        public string username { get; set; }
        public int mostrador { get; set; }
        public char impreso { get; set; }
        public DateTime fechaf { get; set; }
        public int notadevol { get; set; }
        public int paracuando { get; set; }
        public int codcombo { get; set; }
        public DateTime vencimiento { get; set; }
        public int condicpago { get; set; }
        public int diasfvto { get; set; }
        public DateTime fechacuando { get; set; }
        public string TipoDevolucion { get; set; }
        public int depositodestino { get; set; }
        public int idUsuarioPedido { get; set; }
        public int idCanalPedido { get; set; }
        public int idGrupoFacturacionEspecial { get; set; }
        public byte EsFaltante { get; set; }
        public byte Emboque { get; set; }
        public int faltante { get; set; }
        public int rotura { get; set; }
        public int nopidio { get; set; }
        public int valepor { get; set; }
        public int idRepartidorVale { get; set; }
        public float preciovale { get; set; }
        public float dscto_porc { get; set; }
        public int depositorituras { get; set; }
        public string tipoitem { get; set; }
        public int donjose { get; set; }
        public string emboqueitem { get; set; }
        public int idUsuarioControlo { get; set; }
        public DateTime FechaControlo { get; set; }
        public int cantdfaltante { get; set; }
        public string cf { get; set; }
        public string rateoff { get; set; }

    }
}
