using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trabalho_Interdisciplinar
{
    public class Revenda
    {

        class Recebe
        {
            public Armazem fornecedor;
            public double custo;
            public int quant_prod;
        }

        private int demanda;
        public int id;
        public bool atendido;
        private Recebe[] recebidos;

        public Revenda(int demanda, int id, int n_armazem)
        {
            this.atendido = false;
            this.demanda = demanda;
            this.id = id;
            this.recebidos = new Recebe[n_armazem];
        }

        public void decDemanda(int valor)
        {
            if (demanda > 0 && demanda >= valor)
            {
                this.demanda -= valor;
            }
            if (demanda == 0)
            {
                this.atendido = true;
            }
        }

        public int getDemanda()
        { 
            return(this.demanda);
        }

        public void addRecebidos(Armazem armazem, int quantProd)
        {
            this.decDemanda(quantProd);
            this.recebidos[armazem.id] = new Recebe{
            
                custo = armazem.custos[this.id],
                fornecedor = armazem,
                quant_prod = quantProd
                
            
            };

        }

        public override string ToString()
        {
            double custoTotal = 0;
            string ret = "Revenda " + this.id + "; ";
            foreach (Recebe x in this.recebidos)
            {
                if (x == null)
                {
                    ret += 0 + "; ";
                }
                else
                {

                    ret += x.quant_prod + ";";
                    custoTotal = x.custo * x.quant_prod;
                }
            }

            ret += custoTotal;

            return (ret);
        }
    }
}
