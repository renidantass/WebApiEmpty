using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogo.Services
{
    public class MeuServico : IMeuServico
    {
        public string Saudacao(string nome)
        {
            return $"Bem-vindo(a) {nome}, {DateTime.Now}";
        }
    }
}
