using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Trabalho_Interdisciplinar
{
    public class Logistica
    {
        public int[] demandas;
        public double[,] custos;
        public int[] estoques;
        public List<Revenda> revendas;
        public List<Armazem> armazens;

        public Logistica(string path_revendas, string path_armazens, string path_custos)
        {

            this.armazens = new List<Armazem>();
            this.revendas = new List<Revenda>();
            this.carregarArmazens(path_armazens);
            this.carregarRevendas(path_revendas);
            this.carregarCustos(path_custos);
        }

        public void carregarRevendas(string path_revendas)
        {
            StreamReader stre = new StreamReader(path_revendas);

            string linha = stre.ReadLine();
            int tamanho = int.Parse(linha);
            for (int i = 0; i < tamanho; i++)
            {
                string[] spl = stre.ReadLine().Trim().Split(';');

                revendas.Add(new Revenda(int.Parse(spl[1]), i, this.armazens.Count));
            }

        }

        public void carregarArmazens(string path_armazens)
        {
            StreamReader stre = new StreamReader(path_armazens);

            string linha = stre.ReadLine();
            int tamanho = int.Parse(linha);
            for (int i = 0; i < tamanho; i++)
            {
                string[] spl = stre.ReadLine().Split(';');

                armazens.Add(new Armazem(int.Parse(spl[1].Trim()), i));

            }
        }

        public void carregarCustos(string path_custos)
        {
            StreamReader stre = new StreamReader(path_custos);

            string[] tam = stre.ReadLine().Split(';');

            this.custos = new double[int.Parse(tam[0]), int.Parse(tam[1])];

            double[] custo_arm = new double[int.Parse(tam[1])];
 
            for (int i = 0; i < int.Parse(tam[0]); i++)
            {
                string[] spl = stre.ReadLine().Split(';');

                for (int j = 1; j < spl.Length; j++)
                {
                    custo_arm[j-1] = double.Parse(spl[j].Trim());
                    this.custos[i, j-1] = double.Parse(spl[j].Trim());
                }


                this.armazens[i].setCusto(custo_arm);

                custo_arm = new double[int.Parse(tam[1])];

            }
        }

        public void calcular()
        {
            int j = 0, i, im=0;

            while (j < this.custos.GetLength(1))
            {

                do
                {
                    im = 0;
                    for (i = 0; i < this.custos.GetLength(0); i++)
                    {
                        if (this.custos[i, j] <= this.custos[im, j] && this.custos[i, j] != -1)
                        {
                            im = i;
                        }
                    }

                    bool estado = this.armazens[im].addEntrega(this.revendas[j].getDemanda(), this.revendas[j]);
                    if (!estado)
                    {
                        this.armazens[im].addEntrega(this.armazens[im].estoque, this.revendas[j]);
                    }

                    this.custos[im, j] = -1;

                } while (!this.revendas[j].atendido);

                j++;

            }

            this.gerarSaidaArmazem();
            this.gerarSaidaRevenda();
        }

        public void gerarSaidaArmazem()
        {
            StreamWriter str = new StreamWriter("saida_armazem.txt", true, ASCIIEncoding.ASCII);

            foreach(Armazem x in armazens)
            {
                str.WriteLine(x.ToString());
            }
            str.Close();
        }

        public void gerarSaidaRevenda()
        {
            StreamWriter str = new StreamWriter("saida_revenda.txt", true, ASCIIEncoding.ASCII);

            foreach (Revenda x in revendas)
            {
                str.WriteLine(x.ToString());
            }

            str.Close();

        }

    }
}
