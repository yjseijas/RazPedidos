using GalaSoft.MvvmLight.Command;
using RazPedidos.Models;
using RazPedidos.Services;
using RazPedidos.Views;
using System;
using System.Collections.Generic;   
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
namespace RazPedidos.ViewModels
{
    public class PedidosListViewModel : BaseViewModel
    {
        private string filter;
        private ApiServices apiService;
        private bool isRunning;

        private bool isRefreshing;

        public ObservableCollection<PedidosShowItemViewModel> pedidosShow;
        public ObservableCollection<PedidosShowItemViewModel> pedidosShow02;

        public string Filter
        {
            get
            {
                return this.filter;
            }
            set
            {
                SetValue(ref this.filter, value);
                this.Search();
            }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { SetValue(ref this.isRefreshing, value); }
        }

        //private BindingList<PedidosShow> listaPedidos;


        public ObservableCollection<PedidosShowItemViewModel> PedidosShow
        {
            get { return this.pedidosShow; }
            set { this.SetValue(ref this.pedidosShow, value); }

        }
        //public BindingList<PedidosShow> ListaPedidos
        //{
        //    get { return this.listaPedidos; }
        //}



        public PedidosListViewModel()
        {
            instance = this;
            this.IsRefreshing = true;
            this.IsRunning = true;
            this.apiService = new ApiServices();
            this.LoadPedidos();
            this.IsRefreshing = false;
            this.IsRunning = false;

        }



        #region Commands
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadPedidos);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(Search);
            }
        }

        public ICommand SortNumberCommand
        {
            get
            {
                return new RelayCommand(SortNumber);
            }
        }
        public ICommand SortDirCommand
        {
            get
            {
                return new RelayCommand(SortDir);

            }
        }
        public ICommand SortCliCommand
        {
            get
            {
                return new RelayCommand(SortCli);

            }
        }


        public ICommand SortRepCommand
        {
            get
            {
                return new RelayCommand(SortRep);

            }
        }

        public ICommand SortTomoCommand
        {
            get
            {
                return new RelayCommand(SortTomo);

            }
        }

        #endregion

        private void SortTomo()
        {
            this.PedidosShow = new ObservableCollection<PedidosShowItemViewModel>(
                this.pedidosShow.OrderBy(b => b.username));
        }
        private void SortRep()
        {
            this.PedidosShow = new ObservableCollection<PedidosShowItemViewModel>(
                this.pedidosShow.OrderBy(b => b.reparto));
        }
        private void SortCli()
        {
            this.PedidosShow = new ObservableCollection<PedidosShowItemViewModel>(
                this.pedidosShow.OrderBy(b => b.codcli));
        }

        private void SortNumber()
        {
            this.PedidosShow = new ObservableCollection<PedidosShowItemViewModel>(
                this.pedidosShow.OrderBy(b => b.num));
        }

        private void SortDir()
        {
            this.PedidosShow = new ObservableCollection<PedidosShowItemViewModel>(
                this.pedidosShow.OrderBy(b => b.direcion));
        }

        private async void LoadPedidos()
        {
                var hayConexion = await this.apiService.CheckConnection("todos los pedidos");
            if (!hayConexion.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error, no fue posible conectar a internet.",
                    hayConexion.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }

            var pedidos = await this.apiService.GetPedidosAsync();

            if (!pedidos.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    pedidos.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            var lista = (List<PedidosShow>)pedidos.Result;

            var myList = lista.Select(p => new PedidosShowItemViewModel
            {
                num = p.num,
                direcion = p.direcion,
                codcli = p.codcli,
                username = p.username,
                tipofac = p.tipofac,
                reparto = p.reparto,
                bloqueada = p.bloqueada,
                bloqueada2 = p.bloqueada2,
                cf = p.cf,
                fechacuando = p.fechacuando,
                fechaf = p.fechaf,
                identif = p.identif,
                impreso = p.impreso,
                isBlocked = p.isBlocked,
                marca = p.marca,
                marcar = p.marcar,
                recontrainhab = p.recontrainhab
            });

            this.pedidosShow02 = new ObservableCollection<PedidosShowItemViewModel>(myList);
            this.PedidosShow = new ObservableCollection<PedidosShowItemViewModel>(myList);

            this.IsRunning = false;

            //            this.PedidosShow = new ObservableCollection<PedidosShow>(pedidos.Result);


            //            ListaPedidos = pedidos;
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter.Trim()))
            {
                this.PedidosShow = new ObservableCollection<PedidosShowItemViewModel>(this.pedidosShow02);
            }
            else
            {
                this.PedidosShow = new ObservableCollection<PedidosShowItemViewModel>(this.pedidosShow02);
                this.PedidosShow = new ObservableCollection<PedidosShowItemViewModel>(
                    this.pedidosShow.Where(b => b.num.ToString().Contains(this.Filter.Trim())));
            }

        }

        #region Singleton
        private static PedidosListViewModel instance;

        public static PedidosListViewModel GetInstance()
        {
            return instance;
        }

        #endregion
    }
}
