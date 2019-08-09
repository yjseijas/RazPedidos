using System;
//using Android.OS;
using RazPedidos.Interfaces;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof (RazPedidos.Droid.Implementations.PathServices))]
namespace RazPedidos.Droid.Implementations
{
    public class PathServices : IPathService
    {
        public string GetDatabasePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path,"Pedido.db3");
        }
    }
}