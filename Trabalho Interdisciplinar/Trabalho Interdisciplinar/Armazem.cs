using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trabalho_Interdisciplinar
{
    public class Armazem
    {
        class Entrega
        {
            public Revenda destino;
            public double custo;
            public int quant_prod;
        }

        public int estoque;
        public int id;
        public double[] custos;
        private Entrega[] entregas;

        public Armazem(int estoque, int id)
        {
            this.estoque = estoque;
            this.id = id;
        }

        public void setCusto(double[] custos)
        {
            this.custos = custos;
            this.entregas = new Entrega[custos.Length];
        }

        public bool addEntrega(int quantProd, Revenda dest)
        {
            if (estoque < quantProd || estoque == 0)
            {
                return(false);
            }
            else
            {
                estoque -= quantProd;
                dest.addRecebidos(this, quantProd);

                this.entregas[dest.id] = new Entrega
                {
                    custo = this.custos[dest.id],
                    destino = dest,
                    quant_prod = quantProd,

                };

                return (true);
            }
        }

        public override string ToString()
        {
            double custoTotal = 0;
            string ret = "Armazem "+this.id+"; ";
            foreach (Entrega x in this.entregas)
            {
                if (x == null)
                {
                    ret += 0 + ";";
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
