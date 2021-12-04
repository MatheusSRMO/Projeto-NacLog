using Newtonsoft.Json;
using Projetinho;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IronPy {
    class Verifica {
        string url = "http://54.94.96.51/";
        string senha = "KPK*BI2ca3KSEXOF$!2RfVw%NPFI$lkLZZjtD8t8KTDs$!Mbdv";
        public bool tem = false;
        public int setor = -1;

        public async Task PostLogin(string username, string senhaUsuario) {
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage();
            var objeto = new {
                username = username,
                senhaUsuario = senhaUsuario,
                senha = senha
            };
            var content = ToRequest(objeto);
            var response = await httpClient.PostAsync(requestUri: url + "login/json/", content);
            string data1 = await response.Content.ReadAsStringAsync();
            var myDeserializedClass = JsonConvert.DeserializeObject<Json>(data1);
            tem = myDeserializedClass.Valid;
            setor = myDeserializedClass.Setor;
        }
        public StringContent ToRequest(object obj) {
            var json = JsonConvert.SerializeObject(obj);
            var data = new StringContent(json, Encoding.UTF8, mediaType: "application/json");
            return data;
        }
    }
    class Json {
        public int Setor { get; set; }
        public bool Valid { get; set; }
    }
}
