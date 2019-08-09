using RazPedidos.Models;
using RazPedidos.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace RazPedidos.ViewModels
{
    public class PedidosViewModel : BaseViewModel
    {
        private ApiServices apiService;
        private bool isRunning;
        private bool isRefreshing;

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        //private BindingList<PedidosShow> listaPedidos;

        private ObservableCollection<PedidosShow> pedidosShow;

        public ObservableCollection<PedidosShow> PedidosShow
        {   get { return this.pedidosShow; }
            set { this.SetValue(ref this.pedidosShow, value); }

        }
        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        //public BindingList<PedidosShow> ListaPedidos
        //{
        //    get { return this.listaPedidos; }
        //}



        public PedidosViewModel()
        {
            this.IsRefreshing = true;
            this.IsRunning = true;
            this.apiService = new ApiServices();
            this.LoadPedidos();  
        }

        private async void LoadPedidos()
        {
            var pedidos = await this.apiService.GetPedidosAsync();
            
            if (!pedidos.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    pedidos.Message,
                    "Accept");

            }
            var lista = (List<PedidosShow>) pedidos.Result;
            this.PedidosShow = new ObservableCollection<PedidosShow>(lista);

            this.IsRunning = false;
            this.IsRefreshing = false;

            //            ListaPedidos = pedidos;
        }
    }
}
