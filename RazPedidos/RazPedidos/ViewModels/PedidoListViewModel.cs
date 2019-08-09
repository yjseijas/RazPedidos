using RazPedidos.Models;
using RazPedidos.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Diagnostics;

namespace RazPedidos.ViewModels
{
    public class PedidoListViewModel : BaseViewModel
    {
//        private Response hayConexion = new Response();
        private string nCodBuscar;
        private int nCodigo;
        private int nFactor, nNumero;
        private ApiServices apiService;
        private DataServices dataServices;
        private bool isRunning;
        private ObservableCollection<PedidoGet> pedidoShow;
        private ObservableCollection<ArticuloBarras> articulosBarras;
        public List<PedidoGet> PedidoList;
        private int num;

        private string cColor;

        public string CCodBuscar
        {
            get
            {
                return this.nCodBuscar;
            }
            set
            {
                SetValue(ref this.nCodBuscar, value);
            }
        }

        public int NNumero
        {
            get
            {
                return this.nNumero;
            }
            set
            {
                SetValue(ref this.nNumero, value);
            }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public string CColor
        {
            get
            {
               return this.cColor;
            }
            set { SetValue(ref this.cColor, value); }
        }

        public ObservableCollection<PedidoGet> PedidoShow
        {
            get { return this.pedidoShow;}
            set { this.SetValue(ref this.pedidoShow, value); }
        }

        public ObservableCollection<ArticuloBarras> ArticulosBarras 
        {
            get { return this.articulosBarras; }
            set { this.SetValue(ref this.articulosBarras, value); }
        }
        #region Commands
        public ICommand ProcesarCommand
        {
            get
            {
                return new RelayCommand(Procesar);
            }
        }
        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadPedido);
            }
        }

        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(searchProduct);
            }

        }

        #endregion

        public PedidoListViewModel(int nNum)
        {
            this.num = nNum;
            //MainViewModel.nNumero = this.num;
            this.isRunning = true;
            this.apiService = new ApiServices();
            this.dataServices = new DataServices();
            this.LoadPedido();
            
            this.IsRunning = false;
                        
        }

        #region procedures
        private async void Procesar()
        {
            var item = this.PedidoShow.FirstOrDefault(p => p.isCompleted == false);

            if (item != null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Faltan Artículos por Cargar.",
                    "",
                    "Accept");
                return;
            }
            var continua = await Application.Current.MainPage.DisplayAlert(
                "Confirme.",
                "Desea procesar el pedido?",
                "Sí",
                "No");

            if (!continua)
            {
                return;
            }
            var listaPedido = (IEnumerable<PedidoGet>)this.PedidoShow;

            var proceso = await this.apiService.UpdatePedidoAsync(listaPedido);

            if (!proceso.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error", 
                    proceso.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }


            //mainViewModel.pedidosListViewModel = new PedidosListViewModel();
            //this.pedidosShow02 = new ObservableCollection<PedidosShowItemViewModel>(myList);

            var pedidosListViewModel = PedidosListViewModel.GetInstance();
            //pedidosListViewModel.PedidosShow = new ObservableCollection<PedidosShowItemViewModel>(pedidosListViewModel.pedidosShow02);

            pedidosListViewModel.pedidosShow02 = new ObservableCollection<PedidosShowItemViewModel>(
                pedidosListViewModel.pedidosShow02.Where(b => b.num != this.num));

            pedidosListViewModel.PedidosShow = new ObservableCollection<PedidosShowItemViewModel>(
                pedidosListViewModel.pedidosShow.Where(b => b.num != this.num));

            pedidosListViewModel.Filter = "";

            await this.dataServices.DeleteAll();
            await Application.Current.MainPage.DisplayAlert(
                "",
                "Pedido ha sido procesado.",
                "Accept");
            await Application.Current.MainPage.Navigation.PopModalAsync();
            //await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async void LoadPedido()
        {
            //ºawait this.dataServices.DeleteAll();
            this.PedidoList = new List<PedidoGet>();
            this.PedidoList = await dataServices.GetPedido();

            bool lAnterior = false;
            if (this.PedidoList.Count > 0)
            {
                lAnterior = await Application.Current.MainPage.DisplayAlert(
                    "ATENCIÓN! Hay un pedido no Procesado.",
                    "Desea cargar ese pedido?",
                    "Sí",
                    "No");
            }

            if (!lAnterior)
            {
                var hayConexion = await this.apiService.CheckConnection("un pedido");
                //            hayConexion = await this.apiService.CheckConnection("un pedido");
                if (!hayConexion.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error, no fue posible conectar a internet.",
                        hayConexion.Message,
                        "Accept");
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }

                var pedido = await this.apiService.GetPedidoAsync(this.num);

                if (!pedido.IsSuccess)
                {
                    //this.IsRunning = false;
                    await Application.Current.MainPage.DisplayAlert(
                        "Error Buscando detalle del pedido",
                        pedido.Message,
                        "Accept");
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
                var lista = (List<PedidoGet>)pedido.Result;

                var myList = lista.Select(p => new PedidoGet
                {
                    num = p.num,
                    cajasc = p.cajasc,
                    cajasrec = p.cajasrec,
                    cantd = p.cantd,
                    cantdrec = p.cantdrec,
                    codigo = p.codigo,
                    descrip = p.descrip,
                    idsector = p.idsector,
                    precio = p.precio,
                    username = p.username,
                    stock = p.stock,
                    bultosrec = p.bultosrec,
                    isCompleted = false,
                    idusuario = MainViewModel.idusuario
                });

                this.PedidoShow = new ObservableCollection<PedidoGet>(myList);
            }
            else
            {
                this.PedidoShow = new ObservableCollection<PedidoGet>(this.PedidoList);
                var itemnum = this.PedidoList.FirstOrDefault();
                this.num = itemnum.num;
            }


            this.loadBarras();
            this.NNumero = this.num;
        }

        private async void loadBarras()
        {
            //Look for bar codes YJS 22112018
            string cProductos = "";
            foreach (var _item in this.PedidoShow)
            {
                cProductos = cProductos + _item.codigo + ",";
            }
            //string cProductosAll = cProductos.Substring(0,cProductos.Length - 1) ;

            var hayConexion = await this.apiService.CheckConnection("las barras");
            if (!hayConexion.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error, no fue posible conectar a internet.",
                    hayConexion.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }

            var codBarras = await this.apiService.GetBarrasAsync(cProductos);

            if (!codBarras.IsSuccess)
            {
               // this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error Buscando detalle de los códigos de barra",
                    codBarras.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopModalAsync();
             }
            var lista02 = (List<ArticuloBarras>)codBarras.Result;

            var myList02 = lista02.Select(b => new ArticuloBarras
            {

                codigo = b.codigo,
                codigobarra = b.codigobarra,
                factor = b.factor,
                bultos = b.bultos,
                cajas = b.cajas,
                unidades = b.unidades
            });

            this.ArticulosBarras = new ObservableCollection<ArticuloBarras>(myList02);
        }


        public async void searchProduct()
        {
            if (string.IsNullOrEmpty(this.CCodBuscar))
            {
                this.ArticulosBarras = new ObservableCollection<ArticuloBarras>(this.articulosBarras);
            }
            else
            {
                string cBusca = this.CCodBuscar.ToString();

                int nMultiplica = 1;
                int nPosicion = cBusca.IndexOf('*');
                if (nPosicion > 0)
                {
                    nMultiplica = Int32.Parse( cBusca.Substring(0, nPosicion));

                    string cBusca02 = cBusca.Substring(nPosicion + 1);
                    cBusca = cBusca02;
                }

                var item = this.ArticulosBarras.FirstOrDefault(p => p.codigobarra ==  cBusca);
                                    
                if (item == null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Producto no está en este pedido.",
                        "", 
                        "Accept");
                    this.CCodBuscar = "";
                    return; 
                }
                this.nCodigo = item.codigo;
                this.nFactor = item.factor * nMultiplica;
                var item02 = this.PedidoShow.FirstOrDefault(p => p.codigo == this.nCodigo);

                if (item02 == null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error buscando producto.",
                        "",
                        "Accept");
                    this.CCodBuscar = "";
                    return;
                }

                if (item02.cantd == item02.cantdrec) 
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error, Pedido completo.",
                        "",
                        "Accept");
                    this.CCodBuscar = "";
                    return;
                }


                if ((this.nFactor > item02.cantd) || this.nFactor > (item02.cantdrec + item02.cantd)  )
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error, cantidad scaneada es superior a la requerida.",
                        "",
                        "Accept");
                    this.CCodBuscar = "";
                    return;
                }

                //if (item.stock <= 0)
                //{
                //    await Application.Current.MainPage.DisplayAlert(
                //        "Producto no tiene stock suficiente en sistema.",
                //        "",
                //        "Accept");
                //    return;
                //}

                int nReg = PedidoShow.IndexOf(item02) ;
                //int nCode = this.PedidoShow[nReg].codigo;

                PedidoGet _item02 = new PedidoGet();
                _item02.num = this.PedidoShow[nReg].num;
                _item02.codigo = this.nCodigo;
                _item02.cajasrec = this.PedidoShow[nReg].cajasrec + (item.cajas * nMultiplica);
                _item02.cajasc = this.PedidoShow[nReg].cajasc;
                _item02.cantd = this.PedidoShow[nReg].cantd;
                _item02.cantdrec = this.PedidoShow[nReg].cantdrec + this.nFactor;
                _item02.descrip = this.PedidoShow[nReg].descrip;
                _item02.idsector = this.PedidoShow[nReg].idsector;
                _item02.precio = this.PedidoShow[nReg].precio;
                _item02.username = this.PedidoShow[nReg].username;
                _item02.stock = this.PedidoShow[nReg].stock - 1;
                _item02.bultosrec = this.PedidoShow[nReg].bultosrec + (item.bultos * nMultiplica);
                _item02.idusuario = this.PedidoShow[nReg].idusuario;
                if (_item02.cantd == _item02.cantdrec)
                {
                    _item02.isCompleted = true;
                }

                var copy = new ObservableCollection<PedidoGet>(this.PedidoShow);
                foreach (var _item in copy)
                {
                    if (_item.codigo == this.nCodigo)
                    {
                        this.PedidoShow.Remove(_item);
                        break;
                    }
                }
                this.PedidoShow.Add(_item02);
                this.PedidoShow = new ObservableCollection<PedidoGet>(this.PedidoShow.OrderBy(b => b.codigo));
                this.PedidoList = new List<PedidoGet>(this.PedidoShow); // lista pata local db yjs 08012019
                await this.dataServices.DeleteAll();
                await this.dataServices.Insert(this.PedidoList);
                this.CCodBuscar = "";
                //this.PedidoShow
            }

        }
        #endregion
    }
}

