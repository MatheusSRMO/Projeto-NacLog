using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nacional.Torre.ConnectToApi {
    public class Pedidos {
        public int documento { get; set; }
        public int cnpj { get; set; }
        public string cliente { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public float valorNf { get; set; }
        public float peso { get; set; }
        public int volume { get; set; }
        public string data { get; set; }
        public string disponivel { get; set; }
        public string cep { get; set; }
        public string pedido { get; set; }
    }
}
