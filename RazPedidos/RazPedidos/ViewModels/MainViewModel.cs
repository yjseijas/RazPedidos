using System;
using System.Collections.Generic;
using System.Text;

namespace RazPedidos.ViewModels
{
    public class MainViewModel
    {
        public static int nNumero;
        public static int idusuario { get; set; }

        #region ViewModels
        public PedidoLoadViewModel pedidoLoadViewModel
        {
            get;
            set;
        }

        public PedidosListViewModel pedidosListViewModel
        {
            get;
            set;
        }

        public PedidoListViewModel pedidoListViewModel
        {
            get;
            set;
        }

        public PedidosViewModel pedidos { get; set; }

        public LoginViewModel loginViewModel
        {
            get;
            set;
        }
        #endregion


        public MainViewModel()
        {
            //this.pedidos = new PedidosViewModel();  
            instance = this;

            //this.pedidosListViewModel = new PedidosListViewModel();

            this.loginViewModel = new LoginViewModel();
        }

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }
            return instance;
        }

        #endregion


    }
}
