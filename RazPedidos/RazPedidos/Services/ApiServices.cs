using Newtonsoft.Json;
using Plugin.Connectivity;
using RazPedidos.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

using System.Threading.Tasks;
//using Newtonsoft.Json;
using Xamarin.Forms;

namespace RazPedidos.Services
{
    public class ApiServices
    { 
        HttpClient client = new HttpClient();

        public async Task<Response> GetPedidosAsync()
        {
            try
            {
                string url = "http://35.153.240.83:11444/PedidosApi/api/PedidoApi";

               // string url = "http://192.168.10.111:82/api/PedidoApi";

                //string url = "http://localhost:56052/api/PedidoApi/";
                var response = await client.GetAsync(url);


                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var pedidos = JsonConvert.DeserializeObject<List<PedidosShow>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = pedidos,
                };
                    
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Response> CheckConnection(string cWho)
        {
            try
            {
                string quienconio = cWho;

//                var isConect = await CrossConnectivity.Current.IsConnected;

                if (!CrossConnectivity.Current.IsConnected)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Favor encender los seteos a internet.",
                    };
                }
                var isReachable = await CrossConnectivity.Current.IsRemoteReachable(
                    "google.com");
                if (!isReachable)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Favor chequear su conexión a internet.",
                    };
                }
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                };

            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<Response> GetPedidoAsync(int nNum)
        {
            try
            {
                //   string url = "http://192.168.10.111:82/api/PedidoApi/" + nNum;
                string url = "http://35.153.240.83:11444/PedidosApi/api/PedidoApi/" + nNum;

                var response = await client.GetAsync(url);
           
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var pedidos = JsonConvert.DeserializeObject<List<PedidoGet>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = pedidos,
                };

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Response> GetBarrasAsync(string cArticulos)
        {
            try
            {
               // string url = "http://192.168.10.112:82/api/ArticuloBarrasApi/" + cArticulos;
                string url = "http://35.153.240.83:11444/PedidosApi/api/ArticuloBarrasApi/" + cArticulos;

                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var barras = JsonConvert.DeserializeObject<List<ArticuloBarras>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = barras,
                };

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Response> GetUsersAsync()
        {
            try
            {
                 //string url = "http://192.168.10.111:82/api/UserApi/";
                string url = "http://35.153.240.83:11444/PedidosApi/api/UserApi/";

                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var users = JsonConvert.DeserializeObject<List<Users>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = users,
                };

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Response> UpdatePedidoAsync(IEnumerable<PedidoGet> listapedido)
        {
            try
            {
                //                string url = "http://192.168.10.111:82/api/PedidoApi";
                string url = "http://35.153.240.83:11444/PedidosApi/api/PedidoApi/";

                var uri = new Uri(string.Format(url, listapedido));
                var request = JsonConvert.SerializeObject(listapedido);

                var content = new StringContent(request, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PutAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al editar el pedido",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Pedido actualizado con éxito.",
                };
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error Actualizando el Pedido",
                        ex.ToString(),
                        "Accept");

                return null;
            }
        }

        public async Task<Response> AddColaArmadoAsync(ColaArmadoPedidos itemPedido)
        {
            try
            {
                //                string url = "http://192.168.10.111:82/api/ColaArmadoApi";
                string url = "http://35.153.240.83:11444/PedidosApi/api/ColaArmadoApi/";

                //POR AQUI
                //url = "http://localhost:56052/api/ColaArmadoApi/";

                var uri = new Uri(string.Format(url, itemPedido));
                var request = JsonConvert.SerializeObject(itemPedido);

                var content = new StringContent(request, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al agregar pedido a la cola de armado",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Cola Armado pedido actualizado con éxito.",
                };
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error al agregar pedido a la cola de armado",
                        ex.ToString(),
                        "Accept");

                return null;
            }
        }

        public async Task<Response> UpdateColaArmadoAsync(ColaArmadoPedidos itemPedido)
        {
            try
            {
                //                string url = "http://192.168.10.111:82/api/ColaArmadoApi";
                string url = "http://35.153.240.83:11444/PedidosApi/api/ColaArmadoApi/";

                var uri = new Uri(string.Format(url, itemPedido));
                var request = JsonConvert.SerializeObject(itemPedido);

                var content = new StringContent(request, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PutAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al actualizar la cola de armado",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Cola Armado pedido actualizada con éxito.",
                };
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error al agregar pedido a la cola de armado",
                        ex.ToString(),
                        "Accept");

                return null;
            }
        }

        public async Task<Response> AddAllColaArmadoAsync(IEnumerable<ColaArmadoPedidos> allCola) //yjs 220319
        {
            try
            {
                //                string url = "http://192.168.10.111:82/api/ColaArmadoApi";

                string url = "http://35.153.240.83:11444/PedidosApi/api/AllColaArmadoApi/";

                //POR AQUI ii
                //string url = "";
                //url = "http://10.0.2.2:56052/api/AllColaArmadoApi/";

                var uri = new Uri(string.Format(url, allCola));
                var request = JsonConvert.SerializeObject(allCola);

                var content = new StringContent(request, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await client.PostAsync(uri, content);

                /*
                var request = JsonConvert.SerializeObject(allCola);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                client.BaseAddress = new Uri(url);
                //var url = $"{prefix}{controller}";
                var response = await client.PostAsync(url, content);*/


                var bb = 0;

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = "Error al agregar todo el pedido a la cola de armado (Response)",
                    };
                }

                return new Response
                {
                    IsSuccess = true,
                    Message = "Toda la Cola Armado pedido actualizado con éxito.",
                };
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                        "Error al agregar todo el pedido a la cola de armado (Catch)",
                        ex.ToString(),
                        "Accept");

                return null;
            }
        }

        public async Task<Response> GetExisteCola(int nPedido) //yjs 250319
        {
            try
            {
                string url = "http://35.153.240.83:11444/PedidosApi/api/AllColaArmadoApi/" + nPedido;

                var response = await client.GetAsync(url);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                var pedidos = JsonConvert.DeserializeObject<List<ColaArmadoPedidos>>(result);
                return new Response
                {
                    IsSuccess = true,
                    Message = "Ok",
                    Result = pedidos,
                };

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
