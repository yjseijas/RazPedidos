using RazPedidos.Interfaces;
using RazPedidos.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RazPedidos.Services
{
    public class DataServices
    {
        private SQLiteAsyncConnection connection;

        public DataServices()
        {
            this.OpenOrCreateDB();
        }

        private async Task OpenOrCreateDB()
        {
            var databasePath = DependencyService.Get<IPathService>().GetDatabasePath();
            this.connection = new SQLiteAsyncConnection(databasePath);
            await connection.CreateTableAsync<PedidoGet>().ConfigureAwait(false);
        }

        public async Task Insert<T>(T model)
        {
            await this.connection.InsertAsync(model);
        }

        public async Task Insert<T>(List<T> models)
        {
            await this.connection.InsertAllAsync(models);
        }

        public async Task Update<T>(T model)
        {
            await this.connection.UpdateAsync(model);
        }

        public async Task Update<T>(List<T> models)
        {
            await this.connection.UpdateAllAsync(models);
        }

        public async Task Delete<T>(T model)
        {
            await this.connection.DeleteAsync(model);
        }

        public async Task DeleteAll()
        {
            var query = await this.connection.QueryAsync<PedidoGet>("delete from [PedidoGet]");
        }

        public async Task<List<PedidoGet>> GetPedido()
        {
            var query = await this.connection.QueryAsync<PedidoGet>("select * from [PedidoGet]");
            var array = query.ToArray();

            var list = array.Select(p => new PedidoGet
            {
                bultosrec = p.bultosrec,
                cajasc = p.cajasc,
                cantd = p.cantd,
                cajasrec = p.cajasrec,
                cantdrec = p.cantdrec,
                codigo = p.codigo,
                descrip = p.descrip,
                idsector = p.idsector,
                idusuario = p.idusuario,
                isCompleted = p.isCompleted,
                num = p.num,
                precio = p.precio,
                stock = p.stock,
                username = p.username
               
            }).ToList();
            return list;
        }
    }
}
     