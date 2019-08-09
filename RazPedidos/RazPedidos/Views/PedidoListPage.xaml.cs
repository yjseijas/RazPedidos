using RazPedidos.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RazPedidos.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PedidoListPage : ContentPage
	{
		public PedidoListPage ()
		{
			InitializeComponent ();
            //txtidBanco.Text = vBanco.idBanco.ToString();
            txtNumero.Text = MainViewModel.nNumero.ToString();
        }

        private void Entry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }
        public void searchProduct()
        {
            var a = 0;
            a = a + 1;
        }

    }
}