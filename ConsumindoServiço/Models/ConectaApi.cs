using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace ConsumindoServiço.Models
{
    public class ConectaApi
    {
        private HttpClientHandler httpClientHandler;
        private readonly string _urlServidor;
        private string _urlApi;
        private HttpClient client;
        public HttpResponseMessage response;
        public HttpResponseMessage responseReturn;

        public ConectaApi()
        {
            client = new HttpClient();
            _urlServidor = "http://localhost:49800/";
            client.BaseAddress = new Uri(_urlServidor);
            _urlApi = "api/cliente/";
        }

        public ConectaApi(string urlapi)
        {
            client = new HttpClient();
            _urlServidor = "http://localhost:49800/";
            client.BaseAddress = new Uri(_urlServidor);
            _urlApi = urlapi;
        }


        public string urlServidor
        {
            get
            {
                return _urlServidor;
            }
        }

        public string urlApi
        {
            get
            {
                return _urlApi;
            }

            set
            {
                string _urlApi = value;
            }
        }

        //GET: retorna lista de clientes
        public async Task<List<Cliente>> getClientes()
        {
            client.BaseAddress = new Uri(urlServidor);
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = await client.GetStringAsync(urlApi);
            return JsonConvert.DeserializeObject<List<Cliente>>(content);
        }

        //GET: retornar único cliente(id)
        public async Task<Cliente> getCliente(int id)
        {
            client.BaseAddress = new Uri(urlServidor);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = await client.GetStringAsync(urlApi + id);
            return JsonConvert.DeserializeObject<Cliente>(content);
        }

        //POST
        public void RespostaPost(Cliente cliente)
        {
            client.BaseAddress = new Uri(urlServidor);
            var rs = client.PostAsJsonAsync(_urlApi, cliente);
            rs.Wait();
            response = rs.Result;
        }

        //PUT
        public void RespostaPut(Cliente cliente)
        {
            client.BaseAddress = new Uri(urlServidor);
            var rs = client.PutAsJsonAsync(_urlApi, cliente);
            rs.Wait();
            response = rs.Result;
        }

        //DELETE
        public void RespostaDelete(Cliente cliente)
        {
            client.BaseAddress = new Uri(urlServidor);
            var rs = client.DeleteAsync(_urlApi + cliente.ID);
            rs.Wait();
            response = rs.Result;
        }
    }
}