using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trabalho_Interdisciplinar
{
    class Program
    {
        static void Main(string[] args)
        {
            Logistica log = new Logistica("revenda.txt", "armazem.txt", "custo.txt");

            log.calcular();

            Console.WriteLine("Finalizado: arquivos gerados na pasta bin/debug do projeto");


        }
    }
}
