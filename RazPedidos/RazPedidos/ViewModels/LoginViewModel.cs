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
    public class LoginViewModel : BaseViewModel
    {
        #region Properties
        private ObservableCollection<Users> usersShow;

        private ApiServices apiService;

        private bool isAceptEnabled;

        private bool isRunning;

        public string Email
        {
            get; set;
        }


        public string Password
        {
            get; set;
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsAceptEnabled
        {
            get { return this.isAceptEnabled; }
            set { SetValue(ref this.isAceptEnabled, value); }
        }

        public bool IsRefreshing
        {
            get; set;
        }
        #endregion

        #region collections
        public ObservableCollection<Users> UsersShow
        {
            get { return this.usersShow; }
            set { this.SetValue(ref this.usersShow, value); }

        }
        #endregion

        #region constructors
        public LoginViewModel()
        {
            this.IsAceptEnabled = false;
            this.apiService = new ApiServices();
            this.loadUsers();
        }

        private async void loadUsers()
        {
            this.IsRunning = true;
            var hayConexion = await this.apiService.CheckConnection("usuarios");

            if (!hayConexion.IsSuccess)
            {
                this.IsRefreshing = false;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error, no fue posible conectar a internet.",
                    hayConexion.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }

            var users = await this.apiService.GetUsersAsync();

            if (!users.IsSuccess)
            {
                this.IsRefreshing = false;
                this.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    users.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopModalAsync();
            }
            var lista = (List<Users>)users.Result;

            var myList = lista.Select(p => new Users
            {
                idusuario = p.idusuario,
                usuario = p.usuario,
                password = p.password
            });

            this.UsersShow = new ObservableCollection<Users>(myList);

            this.IsRunning = false;
            this.IsAceptEnabled = true;

        }

        #endregion
        #region commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        #endregion

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe indicar un nombre de usuario.", "Accept");
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe indicar un password.", "Accept");
                return;
            }

            string cBusca = this.Email;

            var item = this.UsersShow.FirstOrDefault(p => p.usuario == cBusca);

            if (item == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Usuario no registrado.",
                    "",
                    "Accept");
                return;
            }

            cBusca = this.Password;

            item = this.UsersShow.FirstOrDefault(p => p.password == cBusca);

            if (item == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Password Incorrecto.",
                    "",
                    "Accept");
                return;
            }


            var mainViewModel = MainViewModel.GetInstance();
            MainViewModel.idusuario = item.idusuario;
            mainViewModel.pedidosListViewModel = new PedidosListViewModel();


            NavigationPage otherPage = new NavigationPage(new PedidosListPage());
            await Application.Current.MainPage.Navigation.PushModalAsync(otherPage);

        }
    }
}
    