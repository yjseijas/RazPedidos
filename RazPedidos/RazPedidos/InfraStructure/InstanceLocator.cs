using RazPedidos.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace RazPedidos.InfraStructure
{
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        { 
            this.Main = new MainViewModel();
        }
    }
}
