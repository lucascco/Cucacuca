using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

//Lucas Carvalho Corrêa
//469006

namespace TILABIII
{
    class Program
    {
        static int[,] matrizCalculo;
        static int[,] custos;
        static int[] ofertas;
        static int[] demanda;

        static void teste()
        {
            //demanda = new int[5] { 25, 15, 10, 10, 40 };
            //ofertas = new int[4] { 20, 30, 40, 10 };
            //custos = new int[4, 5] { { 2, 3, 1, 2, 3 }, { 2, 5, 1, 1, 4 }, { 2, 1, 1, 3, 2 }, { 1, 4, 4, 3, 1 } };
            //matrizCalculo = new int[ofertas.Length + 2, demanda.Length + 2];

            demanda = new int[3] { 100, 50, 200 };
            ofertas = new int[3] { 120, 190, 140 };
            custos = new int[3, 3] { { 4, 7, 6 }, { 3, 3, 9 }, { 5, 5, 4 } };
            matrizCalculo = new int[ofertas.Length + 2, demanda.Length + 2];

            Calculo();

        }

        static void lerCarregarArquivo(string pathRevendas, string pathArmazens, string pathCustos)
        {
            StreamReader strRev = new StreamReader(pathRevendas);
            StreamReader strCus = new StreamReader(pathCustos);
            StreamReader strArm = new StreamReader(pathArmazens);

            //Carrega revendas
            int tam, i = 0;
            string linha = strRev.ReadLine();
            string[] spl = linha.Split(' ');
            tam = int.Parse(spl[0]);
            demanda = new int[tam];
            linha = strRev.ReadLine();
            while (linha != String.Empty)
            {
                spl = linha.Split(';');
                demanda[i] = int.Parse(spl[1].Substring(1, 3));
                linha = strRev.ReadLine();
                i++;
            }


            //Carrega oferta
            i = 0;
            linha = strArm.ReadLine();
            spl = linha.Split(' ');
            tam = int.Parse(spl[0]);
            demanda = new int[tam];
            linha = strArm.ReadLine();
            while (linha != String.Empty)
            {
                spl = linha.Split(';');
                ofertas[i] = int.Parse(spl[1].Substring(1, 3));
                linha = strArm.ReadLine();
                i++;
            }


            //carrega os custos
            i = 0;
            linha = strCus.ReadLine();
            spl = linha.Split(' ')[0].Split(';');
            int tami = int.Parse(spl[0]);
            int tamj = int.Parse(spl[1]);
            custos = new int[tami, tamj];
            linha = strCus.ReadToEnd();

        }


        /// <summary>
        /// Calcula a diferença entre os dois menores custos, seja pela linha, seja pela coluna.
        /// </summary>
        /// <param name="tipo">1- calculo pela linha 2- calculo pela coluna</param>
        /// <param name="inicio">linha ou coluna inicial</param>
        /// <returns>A diferença calculada da linha ou da coluna</returns>
        static int calcularDiferenca(int tipo, int inicio, bool novo)
        {
            if (tipo == 1)
            {
                if (matrizCalculo[inicio, matrizCalculo.GetLength(1) - 1] != 0)
                {

                    List<int> valoresl = new List<int>();
                    for (int i = 0; i < demanda.Length; i++)
                    {
                        if (novo || matrizCalculo[matrizCalculo.GetLength(0) - 1, i] != 0)
                            valoresl.Add(custos[inicio, i]);
                    }
                    if (valoresl.Count > 1)
                    {
                        valoresl.Sort();
                        return (valoresl[1] - valoresl[0]);
                    }
                    else if (valoresl.Count == 1)
                    {
                        return (0);
                    }
                }

                return (0);
            }
            else
            {
                if (matrizCalculo[matrizCalculo.GetLength(0) - 1, inicio] != 0)
                {
                    List<int> valoresl = new List<int>();
                    for (int j = 0; j < ofertas.Length; j++)
                    {
                        if (novo || matrizCalculo[j, matrizCalculo.GetLength(1) - 1] != 0)
                            valoresl.Add(custos[j, inicio]);
                    }
                    if (valoresl.Count > 1)
                    {
                        valoresl.Sort();
                        return (valoresl[1] - valoresl[0]);
                    }
                    else if (valoresl.Count == 1)
                    {
                        return (0);
                    }
                }

                return (0);
            }
        }

        /// <summary>
        /// Recebe as diferenças do custo de cada linha e coluna.
        /// </summary>
        /// <param name="nova">True - Primeira vez que a Matriz é preparada</param>
        static void prepararMatriz(bool nova)
        {
            for (int i = 0; i < matrizCalculo.GetLength(0) - 2; i++)
            {
                if (nova)
                    matrizCalculo[i, matrizCalculo.GetLength(1) - 1] = ofertas[i];
                matrizCalculo[i, matrizCalculo.GetLength(1) - 2] = calcularDiferenca(1, i, nova);

            }
            for (int j = 0; j < matrizCalculo.GetLength(1) - 2; j++)
            {
                if (nova)
                    matrizCalculo[matrizCalculo.GetLength(0) - 1, j] = demanda[j];
                matrizCalculo[matrizCalculo.GetLength(0) - 2, j] = calcularDiferenca(2, j, nova);

            }
            Console.WriteLine("\n\nMatriz Preparada");
            imprimirMatriz(matrizCalculo);
        }

        /// <summary>
        /// Movimenta os valores da tabela, pegando do armazem e transportando para a revenda.
        /// </summary>
        /// <param name="linha">Armazem</param>
        /// <param name="coluna">Revenda</param>
        /// <returns>1 - Revenda foi suprida totalmente  0 - Revenda não foi suprida totalmente</returns>
        static int movimentarValores(int linha, int coluna)
        {

            if (ofertas[linha] >= demanda[coluna])
            {
                matrizCalculo[matrizCalculo.GetLength(0) - 1, coluna] = 0;
                matrizCalculo[linha, matrizCalculo.GetLength(1) - 1] = ofertas[linha] - demanda[coluna];
                matrizCalculo[linha, coluna] = demanda[coluna];
                ofertas[linha] = ofertas[linha] - demanda[coluna];
                demanda[coluna] = 0;
                return (1);
            }
            else
            {
                matrizCalculo[linha, matrizCalculo.GetLength(1) - 1] = 0;
                matrizCalculo[matrizCalculo.GetLength(0) - 1, coluna] = demanda[coluna] - ofertas[linha];
                matrizCalculo[linha, coluna] = ofertas[linha];
                demanda[coluna] = demanda[coluna] - ofertas[linha];
                ofertas[linha] = 0;

                return (0);
            }

            Console.WriteLine("Movimento\n\n");
            imprimirMatriz(matrizCalculo);

        }

        /// <summary>
        /// Deve ser chamado para começar o calculo do menor custo.
        /// </summary>
        static void Calculo()
        {
            int pos_dif_linha = matrizCalculo.GetLength(1) - 2;
            int pos_dif_coluna = matrizCalculo.GetLength(0) - 2;
            int demandas_zeradas = 0;
            prepararMatriz(true);

            do
            {
                //Busca a maior diferença das linhas
                int max = int.MinValue;
                int imax = 0;
                for (int i = 0; i < matrizCalculo.GetLength(0) - 2; i++)
                {
                    if (matrizCalculo[i, matrizCalculo.GetLength(1) - 2] > max && matrizCalculo[i, matrizCalculo.GetLength(1) - 1] != 0)
                    {
                        imax = i;
                        max = matrizCalculo[i, matrizCalculo.GetLength(1) - 2];
                    }
                }

                //Busca a maior diferença das colunas 
                int maxcol = int.MinValue;
                int jmaxcol = 0;
                for (int j = 0; j < matrizCalculo.GetLength(1) - 2; j++)
                {
                    if (matrizCalculo[matrizCalculo.GetLength(0) - 2, j] > maxcol && matrizCalculo[matrizCalculo.GetLength(0) - 1, j] != 0)
                    {
                        jmaxcol = j;
                        maxcol = matrizCalculo[matrizCalculo.GetLength(0) - 2, j];
                    }
                }

                if (max <= maxcol)
                {
                    int menorcusto = int.MaxValue;
                    int imenorcusto = 0;
                    for (int i = 0; i < matrizCalculo.GetLength(0) - 2; i++)
                    {
                        if (menorcusto > custos[i, jmaxcol]
                            && matrizCalculo[i, jmaxcol] == 0
                            && matrizCalculo[i, matrizCalculo.GetLength(1) - 1] > 0)
                        {
                            imenorcusto = i;
                            menorcusto = custos[i, jmaxcol];
                        }
                    }
                    demandas_zeradas += movimentarValores(imenorcusto, jmaxcol);
                    prepararMatriz(false);

                }
                else
                {
                    int menorcusto = int.MaxValue;
                    int jmenorcusto = 0;
                    for (int j = 0; j < matrizCalculo.GetLength(1) - 2; j++)
                    {
                        if (menorcusto > custos[imax, j]
                            && matrizCalculo[imax, j] == 0
                            && matrizCalculo[matrizCalculo.GetLength(0) - 1, j] > 0)
                        {
                            jmenorcusto = j;
                            menorcusto = custos[imax, j];
                        }
                    }

                    demandas_zeradas += movimentarValores(imax, jmenorcusto);
                    prepararMatriz(false);
                }
            } while (demandas_zeradas < demanda.Length);


        }

        /// <summary>
        /// Imprimir uma matriz generica
        /// </summary>
        /// <param name="matriz">Matriz</param>
        static void imprimirMatriz(int[,] matriz)
        {
            for (int i = 0; i < matriz.GetLength(0); i++)
            {
                for (int j = 0; j < matriz.GetLength(1); j++)
                {
                    Console.Write("{0}  ", matriz[i, j].ToString("D3"));
                }
                Console.WriteLine();
            }
        }
            
        static void imprimirResultadoArmazem()
        {
            int custoTotal = 0;
            for (int i = 0; i < matrizCalculo.GetLength(0) - 2; i++)
            {
                for (int j = 0; j < matrizCalculo.GetLength(1) - 2; j++)
                {
                    if (matrizCalculo[i, j] != 0)
                    {
                        Console.WriteLine("Armazem {0} para a Revenda {1}. Produto: {2} Custo: {3}",
                            i + 1, j + 1, matrizCalculo[i, j], custos[i, j] * matrizCalculo[i, j]);
                        custoTotal += custos[i, j] * matrizCalculo[i, j];
                    }
                }
            }

            Console.WriteLine("Custo Total: " + custoTotal);
        }
        static void imprimirResultadoRevenda()
        {

        }

        static void gerarArquivoRevendas()
        {
            FileStream file = File.Create("saidaRevenda.txt");
            StreamWriter stw = new StreamWriter(file);
            int custoTotal = 0;
            StringBuilder linha = new StringBuilder();
            for (int i = 0; i < matrizCalculo.GetLength(1) - 2; i++)
            {
                int custolocal = 0;
                linha.Append("Revenda " + (i + 1) + ";  ");
                for (int j = 0; j < matrizCalculo.GetLength(0) - 2; j++)
                {
                    linha.Append(matrizCalculo[j, i].ToString("D3") + "; ");
                    custolocal += custos[j, i] * matrizCalculo[j, i];
                }
                linha.Append(custolocal);
                stw.WriteLine(linha);
                custoTotal += custolocal;
                linha.Clear();
            }
            stw.WriteLine("Custo Total: "+custoTotal);
            Console.WriteLine("O arquivo de saida dos revendas foi gravado com sucesso na pasta " + file.Name);
            stw.Close();


        }
        static void gerarArquivoArmazens()
        {
            FileStream file = File.Create("saidaArmazens.txt");
            StreamWriter stw = new StreamWriter(file);
            int custoTotal = 0;
            StringBuilder linha = new StringBuilder();
            for (int i = 0; i < matrizCalculo.GetLength(0) - 2; i++)
            {
                int custolocal = 0;
                linha.Append("Armazem "+(i+1)+";  ");
                for (int j = 0; j < matrizCalculo.GetLength(1) - 2; j++)
                {
                    linha.Append(matrizCalculo[i,j].ToString("D3")+"; ");
                    custolocal += custos[i, j] * matrizCalculo[i, j];
                }
                linha.Append(custolocal);
                stw.WriteLine(linha);
                custoTotal += custolocal;
                linha.Clear();
            }
            stw.WriteLine("Custo Total: " + custoTotal);
            Console.WriteLine("O arquivo de saida dos armazens foi gravado com sucesso na pasta "+file.Name);
            stw.Close();

        }

        static void Main(string[] args)
        {
            teste();
            gerarArquivoArmazens();
            gerarArquivoRevendas();
            Console.ReadKey();
        }
    }
}
