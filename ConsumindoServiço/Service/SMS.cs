using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;


namespace PMESP.Cofin.Business
{
    public class SMS
    {

        private HttpClientHandler httpClientHandler;
        public string urlServidor;
        public string urlProxy;
        public string urlApi;
        public string urlToken;
        private string valueTokem;

        /// <summary>
        /// Classe de Negócio que envia SMS
        /// </summary>
        public SMS()
        {
            urlServidor = "http://www.intranet.dfp.policiamilitar.sp.gov.br/cofinsms/";
            urlApi = "Api/Sms/";
            urlToken = "Token";
            this.getToken();
        }

        /// <summary>
        /// Setar url de swerviço REST/FULL
        /// </summary>
        /// <param name="pUrlServidor">Url servidor REST/FULL</param>
        /// <param name="pUrlApi">Url Api</param>
        /// <param name="pUrlToken">Url Token</param>      
        public SMS(string pUrlServidor, string pUrlApi, string pUrlToken)
        {

            urlServidor = pUrlServidor;
            urlApi = pUrlApi;
            urlToken = pUrlToken;
            this.getToken();

        }

        /// <summary>
        /// Setar usuário para passar pelo proxy
        /// </summary>
        /// <param name="pSenhaproxy">Senha de acesso a intranet</param>
        /// <param name="pCpfProxy">CPF do usuária de acesso a intranet</param>
        public SMS(string pSenhaProxy, string pCpfProxy)
        {
            urlProxy = "http://proxy.policiamilitar.sp.gov.br:3128";
            System.Net.WebProxy proxy = new System.Net.WebProxy(urlProxy);
            proxy.Credentials = new System.Net.NetworkCredential(pSenhaProxy, pCpfProxy);
            httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
                PreAuthenticate = true,
                UseDefaultCredentials = true,
            };
            this.getToken();

        }

        /// <summary>
        /// Obter cliente Rest/Full, com acesso ao proxy casa este seja setado
        /// </summary>
        private HttpClient getHttp()
        {
            if (httpClientHandler == null)
                return new HttpClient();
            else
                return new HttpClient(httpClientHandler);

        }

        private void getToken()
        {
            if (string.IsNullOrEmpty(valueTokem))
            {
                HttpClient clientTokem = getHttp();

                clientTokem.BaseAddress = new Uri(urlServidor);
                clientTokem.DefaultRequestHeaders.Accept.Clear();

                //dados do usuário cadastrado no sistema sms
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", "df3@policiamilitar.sp.gov.br"),
                    new KeyValuePair<string, string>("password", "df3@PM")
                });

                var result = clientTokem.PostAsync(urlToken, content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;

                //objeto de retorno 
                var jObject = JObject.Parse(resultContent);

                //seleciona o tokem do objeto
                valueTokem = jObject.GetValue("access_token").ToString();

            }

        }


        /// <summary>
        /// Enviar SMS 
        /// </summary>
        /// <param name="pCelular">Número do Celular com DDD</param>
        /// <param name="pTexto">Mensagem a ser enviada</param>
        /// <returns></returns>
        public string enviarSMS(string pCelular, string pTexto)
        {
            string smsRetorno = "";
            try
            {

                // client o objeto que envia o sms
                HttpClient client = new HttpClient();
               

                client.BaseAddress = new Uri(urlServidor);
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //adiciona o tokem 
               // client.DefaultRequestHeaders.Add("Authorization", "Bearer " + valueTokem);

                //json de mensagem a ser enviado
                var mensagem = new { idMensagem = "0", celular = pCelular, mensagem = pTexto };

                var response = client.GetAsync(urlApi).Result;

                smsRetorno = response.ReasonPhrase;


            }
            catch (OperationCanceledException) { }
            return smsRetorno;
        }
        /// <summary>
        /// Enviar SMS 
        /// </summary>
        /// <param name="pCelular">Número do Celular com DDD</param>
        /// <param name="pTexto">Mensagem a ser enviada</param>
        /// <returns></returns>
        public void enviarSMSSemReturn(string pCelular, string pTexto)
        {
            try
            {

                // client o objeto que envia o sms
                HttpClient client = getHttp();

                client.BaseAddress = new Uri(urlServidor);
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //adiciona o tokem 
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + valueTokem);

                //json de mensagem a ser enviado
                var mensagem = new { idMensagem = "0", celular = pCelular, mensagem = pTexto };

                var response = client.PostAsync(urlApi, new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(mensagem),
                Encoding.UTF8, "application/json")).Result;

            }
            catch (OperationCanceledException) { }


        }

    }

}


