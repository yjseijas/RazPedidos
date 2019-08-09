using RazPedidos.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace RazPedidos.ViewModels
{
    public class PedidoDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate isComplete { get; set; }

        public DataTemplate isNotComplete { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            return ((PedidoGet)item).isCompleted  ? isComplete : isNotComplete;
        }
    }
}


