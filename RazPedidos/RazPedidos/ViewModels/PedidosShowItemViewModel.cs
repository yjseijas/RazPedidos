using GalaSoft.MvvmLight.Command;
using RazPedidos.Models;
using RazPedidos.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace RazPedidos.ViewModels
{
    public class PedidosShowItemViewModel : PedidosShow
    {
        #region Commands
        public ICommand ProcesarPedidoCommand
        {
            get
            {
                return new RelayCommand(ProcesarPedido);
            }
        }

        private async void ProcesarPedido()
        {
            var mainViewModel = MainViewModel.GetInstance();
            mainViewModel.pedidoLoadViewModel = new PedidoLoadViewModel(this.num);

            NavigationPage otherPage  = new NavigationPage(new PedidoLoadPage());

            //await Application.Current.MainPage.Navigation.PushAsync(otherPage);
            await Application.Current.MainPage.Navigation.PushModalAsync(otherPage);

            //await Navigation.PushAsync(new NavigationPage(new PedidoList()));

        }
        #endregion

    }
}


     
