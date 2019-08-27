using System;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace PlusTiPruebaRegistros
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            MainClass m = new MainClass();
            Stopwatch ListWatch = new Stopwatch();
            Stopwatch SubstrWatch = new Stopwatch();
            Stopwatch SplitWatchMultiple = new Stopwatch();
            Char comma = ',';

            Console.WriteLine("Ingresa la ruta del archivo: ");
            String filepath = Console.ReadLine();

            try{

                String datos = File.ReadAllText(filepath);
                string cadenaConexion = @"Data Source=SUINSTANCIA;Initial Catalog=SUDB;User ID=SUUSUARIO; Password=SUCONTRASEÑA; Connect Timeout=60"; //Ingrese sus permisos para conectar a base de SQL
                int Ncolumnas = 300;
                string Tabla = "DatosGuardados";
                string sentenciaSql = "";
                string rutacompleta = filepath;

                if (datos != null)
                {

                    if (!File.Exists(filepath))
                    {
                        Console.WriteLine("Archivo inexistente");
                    }
                    else
                    {
                        Console.WriteLine("Con List: ");
                        ListWatch.Start();

                        using (SqlConnection con = new SqlConnection(cadenaConexion))
                        {
                            con.Open();

                            string[] renglones = File.ReadAllLines(rutacompleta);
                            try
                            {
                                foreach (var linea in renglones)
                                {

                                    List<String> filtered = SplitWords(linea, comma);

                                    for (int columna = 0; columna < Ncolumnas; columna++)
                                    {
                                        sentenciaSql = sentenciaSql + filtered[columna] + "','";
                                    }

                                    int datossql = 1;
                                    string datostring = "";

                                    while (datossql <= 300)
                                    {

                                        if (datossql == 300)
                                        {
                                            datostring = datostring + "dato" + datossql;
                                        }
                                        else
                                        {
                                            datostring = datostring + "dato" + datossql + ", ";
                                        }
                                        datossql = datossql + 1;
                                    }

                                    sentenciaSql = "INSERT INTO " + Tabla + " (" + datostring + ") VALUES ('" + sentenciaSql;
                                    sentenciaSql = sentenciaSql.Substring(0, sentenciaSql.Length - 2) + ")";

                                    using (SqlCommand comando = new SqlCommand(sentenciaSql, con))
                                    {
                                        comando.ExecuteNonQuery();
                                        comando.Dispose();
                                    }
                                    sentenciaSql = "";
                                }
                                ListWatch.Stop();
                                Console.WriteLine("Time elapsed: {0}", ListWatch.Elapsed);
                                Console.ReadKey();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }



                        Console.WriteLine("==========================================");

                        Console.WriteLine("Con Split: ");
                        SplitWatchMultiple.Start();

                        using (SqlConnection con = new SqlConnection(cadenaConexion))
                        {
                            con.Open();

                            try
                            {
                                string[] lines = File.ReadAllLines(rutacompleta);
                                List<string[]> data = new List<string[]>();
                                string actualLine;

                                for (int i = 0; i < lines.Length; i++)
                                {
                                    actualLine = lines[i];
                                    string[] linesValues = actualLine.Split(',').Select(val => "'" + val + "'").ToArray();
                                    data.Add(linesValues);
                                }

                                String campos = "";
                                for (int i = 1; i <= Ncolumnas; i++)
                                {
                                    campos += (i == Ncolumnas) ? "dato" + i : "dato" + i + ", ";
                                }

                                int times = 1;
                                int total = data.Count;
                                int count2 = 1;
                                int cantidad = (total % times == 0) ? total / times : total / times + 1;
                                int salto = 0;
                                int sueltas = (total % times == 0) ? 0 : times - total % times;
                                do
                                {
                                    string start = "INSERT INTO " + Tabla + " (" + campos + ") VALUES ";

                                    string end = "";
                                    string single = "";
                                    string[] inf;
                                    for (int i = 0; i < times; i++)
                                    {
                                        int position = salto + i;
                                        if (i + sueltas == times && count2 == cantidad && total % times != 0) break;
                                        inf = data[position];
                                        single = "(";
                                        for (int j = 0; j < inf.Length; j++)
                                        {
                                            single += (j + 1 == inf.Length) ? inf[j] + ")" : inf[j] + ", ";
                                        }
                                        if (count2 == cantidad)
                                        {
                                            if (i + 1 + sueltas == times) end += single;
                                            else end += single + ", ";
                                        }
                                        else
                                        {
                                            if (i + 1 == times) end += single;
                                            else end += single + ", ";
                                        }
                                        single = "(";
                                    }
                                    salto += times;
                                    String finalSentence = start + end;

                                    using (SqlCommand comando = new SqlCommand(finalSentence, con))
                                    {
                                        comando.ExecuteNonQuery();
                                        comando.Dispose();
                                    }

                                    count2++;
                                } while (cantidad >= count2);

                                SplitWatchMultiple.Stop();
                                Console.WriteLine("Time elapsed: {0}", SplitWatchMultiple.Elapsed);
                                Console.ReadKey();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.ReadKey();
                            }
                            con.Close();
                        }

                        Console.WriteLine("==========================================");

                        Console.WriteLine("Con Metodo Substring:");
                        SubstrWatch.Start();

                      

                            using (SqlConnection con = new SqlConnection(cadenaConexion))
                            {
                                con.Open();

                                string[] renglones = File.ReadAllLines(rutacompleta);
                                try
                                {
                                    foreach (var linea in renglones)
                                    {

                                        List<String> substr = Substr(linea, comma);

                                        for (int columna = 0; columna < Ncolumnas; columna++)
                                        {
                                            sentenciaSql = sentenciaSql + substr[columna] + "','";
                                        }

                                        int datossql = 1;
                                        string datostring = "";

                                        while (datossql <= 300)
                                        {

                                            if (datossql == 300)
                                            {
                                                datostring = datostring + "dato" + datossql;
                                            }
                                            else
                                            {
                                                datostring = datostring + "dato" + datossql + ", ";
                                            }
                                            datossql = datossql + 1;
                                        }

                                        sentenciaSql = "INSERT INTO " + Tabla + " (" + datostring + ") VALUES ('" + sentenciaSql;
                                        sentenciaSql = sentenciaSql.Substring(0, sentenciaSql.Length - 2) + ")";

                                        using (SqlCommand comando = new SqlCommand(sentenciaSql, con))
                                        {
                                            comando.ExecuteNonQuery();
                                            comando.Dispose();
                                        }
                                        sentenciaSql = "";
                                    }
                                    ListWatch.Stop();
                                    Console.WriteLine("Time elapsed: {0}", ListWatch.Elapsed);
                                    Console.ReadKey();
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                    }
                }
            }
            catch(IOException e){
                Console.WriteLine(e.Message);
            }

        }

        //===================================================================================================================================================

        static List<String> Substr(String TodosLosDatos, char ToDelete)
        {
            List<String> palabras = new List<String>();
            int length = (TodosLosDatos = TodosLosDatos + ',').Length;
            int i = 0;

            int lastFound = 0;
            for (i = 0; i < length; i++)
            {
                if (TodosLosDatos[i] != ',') continue;
                String found = TodosLosDatos.Substring(lastFound, i - lastFound);
                lastFound = i + 1;
                palabras.Add(found);
            }
            return palabras;
        }

        static List<String> SplitWords(String Data, char ToDelete)
        {
            String palabra = "";
            Data = Data + ToDelete;
            int length = Data.Length;
            List<String> FilteredWords = new List<String>();
            for (int i = 0; i < length; i++)
            {
                if (Data[i] != ToDelete)
                {
                    palabra = palabra + Data[i];
                }
                else
                {
                    if (palabra.Length != 0)
                    {
                        FilteredWords.Add(palabra);
                    }
                    palabra = "";
                }
            }
            return FilteredWords;
        }

    }
}
