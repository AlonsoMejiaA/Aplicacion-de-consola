using System;
using System.Runtime;
using System.Threading;
using System.Diagnostics;

namespace Parte_2_Parcial_Scripting
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Stopwatch contador = new Stopwatch();


            SearchPath Programa = new SearchPath();
            Programa.CreateMaze();
            contador.Start();
            Programa.BFS();
            Programa.CreatePath();
            Programa.ShowPathTaken();
            contador.Stop();
            TimeSpan time = contador.Elapsed;
            string timer = string.Format("tiempo demorado: " + time.ToString("mm\\:ss\\.ff"));
            Console.WriteLine(timer);
        }
    }
}
