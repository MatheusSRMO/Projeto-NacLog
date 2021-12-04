using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Nacional.Torre.ConnectToApi {
    class Requests {
        string url = "http://54.94.96.51/";
        public string ValidSenha = "KPK*BI2ca3KSEXOF$!2RfVw%NPFI$lkLZZjtD8t8KTDs$!Mbdv";
        public int quantidadeAdd = 0;
        public List<string> placas = new List<string>();
        public List<string> motoristas = new List<string>();
        public List<string> QuadroAviso = new List<string>();
        public Pedidos[] notasDisp;
        public Dados[] notasDisp2;
        public ListNotas[] listNotas;
        public async Task PosUpgradeData(List<int> notas) {
            var httpClient = new HttpClient();
            var content = ToRequest(new {
                senha = ValidSenha,
                documentos = JsonConvert.SerializeObject(notas)
            });
            await httpClient.PostAsync(requestUri: url + "upgrade-data/", content);
        }
        public async Task PostBuscaEntreDatas(string inicio,string fim) {
            var httpClient = new HttpClient();
            var content = ToRequest(new {
                senha = ValidSenha,
                inicio = inicio,
                fim = fim
            });
            var response = await httpClient.PostAsync(requestUri: url + "buscar-entre-datas/", content);
            string data = await response.Content.ReadAsStringAsync();
            listNotas = JsonConvert.DeserializeObject<ListNotas[]>(data);
        }
        public async Task PostBuscaDados(List<int> notas) {
            var httpClient = new HttpClient();
            var content = ToRequest(new {
                senha = ValidSenha,
                notas = JsonConvert.SerializeObject(notas)
            });
            var response = await httpClient.PostAsync(requestUri: url + "buscar-dados/", content);
            string data = await response.Content.ReadAsStringAsync();
            notasDisp2 = JsonConvert.DeserializeObject<Dados[]>(data);
        }
        public async Task PostBuscaNotas(List<int> notas) {
            var httpClient = new HttpClient();
            var content = ToRequest(new {
                senha = ValidSenha,
                notas = JsonConvert.SerializeObject(notas)
            });
            var response = await httpClient.PostAsync(requestUri: url + "buscar-notas/", content);
            string data = await response.Content.ReadAsStringAsync();
            notasDisp = JsonConvert.DeserializeObject<Pedidos[]>(data);
        }
        public async Task PostRegistraPlaca(List<Registra> registros) {
            var httpClient = new HttpClient();
            var content = ToRequest(registros);
            var response = await httpClient.PostAsync(requestUri: url + "registra-placa/", content);
        }
        public async Task PostVerPedidosPlacaData(string placa, string data) {
            var httpClient = new HttpClient();
            var content = ToRequest(new {
                senha = ValidSenha,
                value = placa,
                data = data
            });
            var response = await httpClient.PostAsync(requestUri: url + "buscar-placa-data/", content);
            string data1 = await response.Content.ReadAsStringAsync();
            notasDisp = JsonConvert.DeserializeObject<Pedidos[]>(data1);
        }
        public async Task PostVerPedidos(string value) {
            var httpClient = new HttpClient();
            var content = ToRequest(new { 
                senha = ValidSenha,
                value = value
            });
            var response = await httpClient.PostAsync(requestUri: url + "buscar-disponivel/", content);
            string data = await response.Content.ReadAsStringAsync();
            notasDisp = JsonConvert.DeserializeObject<Pedidos[]>(data);
        }
        public async Task PostRegistroDados(List<Dados> dados) {
            var httpClient = new HttpClient();
            var content = ToRequest(dados);
            var response = await httpClient.PostAsync(requestUri: url + "registra-dados/", content);
            string data1 = await response.Content.ReadAsStringAsync();
            quantidadeAdd = int.Parse(data1);
        }
        public async Task PostRegistroPedidos(List<Pedidos> pedidos) {
            var httpClient = new HttpClient();
            var content = ToRequest(pedidos);
            var response = await httpClient.PostAsync(requestUri: url + "registra-pedidos/", content);
            string data1 = await response.Content.ReadAsStringAsync();
            quantidadeAdd = int.Parse(data1);
        }
        public async Task GetQuadroAviso() {
            QuadroAviso.Clear();
            var httpClient = new HttpClient();
            var resposta = await httpClient.GetAsync(requestUri: url + "quadro-aviso/json/");
            var data = await resposta.Content.ReadAsStringAsync();
            var myDeserializedClass = JsonConvert.DeserializeObject<ArrayQuadroAviso[]>(data);
            foreach (var i in myDeserializedClass) {
                QuadroAviso.Add(i.Aviso);
                QuadroAviso.Add(i.Nivel.ToString());
            }
        }
        public async Task PostRomaneio(string placa, string motorista, List<int> notas) {
            var Json = JsonConvert.SerializeObject(notas);
            var httpClient = new HttpClient();
            var objeto = new {
                placa = placa,
                motorista = motorista,
                notas = Json,
                senha = ValidSenha
            };
            var content = ToRequest(objeto);
            var response = await httpClient.PostAsync(requestUri: url + "Notas/", content);
            string data1 = await response.Content.ReadAsStringAsync();
        }
        public async Task PostRetornos(List<int> notas) {
            var Json = JsonConvert.SerializeObject(notas);
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage();
            var objeto = new {
                notas = Json
            };
            var content = ToRequest(objeto);
            var response = await httpClient.PostAsync(requestUri: url + "Retornos/", content);
            var data = await response.Content.ReadAsStringAsync();
        }
        public StringContent ToRequest(object obj) {
            return new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, mediaType: "application/json"); ;
        }
        public async Task GetVeiculos(string datat) {
            var httpClient = new HttpClient();
            var resposta = await httpClient.GetAsync(requestUri: url + datat);
            var data = await resposta.Content.ReadAsStringAsync();
            var myDeserializedClass = JsonConvert.DeserializeObject<ArrayVeiculos[]>(data);
            foreach (var i in myDeserializedClass) {
                placas.Add(i.Placa);
            }
        }
        public async Task GetMotoristas(string datat) {
            var httpClient = new HttpClient();
            var resposta = await httpClient.GetAsync(requestUri: url + datat);
            var data = await resposta.Content.ReadAsStringAsync();
            var myDeserializedClass = JsonConvert.DeserializeObject<ArrayMotoristas[]>(data);
            foreach (var i in myDeserializedClass) {
                motoristas.Add(i.Motorista);
            }
        }
    }
}
