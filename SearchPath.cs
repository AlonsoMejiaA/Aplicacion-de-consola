using System;
using System.Collections.Generic;
using System.Text;

namespace Parte_2_Parcial_Scripting
{

    public class Nodo
    {
        public bool explorado = false;

        public Nodo exploradoDe;

        public Vector2D ubicacion;

        public Nodo(int x, int y)
        {
            ubicacion = new Vector2D(x,y);
        }

        public Vector2D GetPosition()
        {
            return ubicacion;
        }

        public override string ToString()
        {
            return ("explorado: " + explorado + "ubicacion: " + ubicacion);
        }
    }
    public struct Vector2D
    {
        public int vaX, vaY;

        public Vector2D(int X, int Y)
        {
            vaX = Y;
            vaY = X;
        }
        public Vector2D ayudaExtra(Vector2D vector)
        {
            int extraX = vaX + vector.vaX;
            int extraY = vaY + vector.vaY;
            Vector2D respuesta = new Vector2D(extraX, extraY);
            return respuesta;
        }
        public override string ToString()
        {
            return ("(" + vaX.ToString() + " , " + vaY.ToString() + ")");
        }
    }


    class SearchPath
    {
        //puntos finales e iniciales
        private Nodo Inicio;
        private Nodo Final;
        //valores para el incio y final
        int xInicial = 0;
        int yInicial = 0;
        int xFinal = 0;
        int yFinal = 0;

        private Vector2D[] Direcciones = { new Vector2D(0, 1), new Vector2D(0, -1), new Vector2D(1, 0), new Vector2D(-1, 0) };//direcciones en las que podria moverse
        private bool explorando = true;//está explorando
        private Nodo punDeExplo;
        private Queue<Nodo> cola = new Queue<Nodo>();// la cola de nodos a explorar
        private List<Nodo> camino = new List<Nodo>();
        private List<Nodo> caminoTomado = new List<Nodo>();
        private Dictionary<Vector2D, int> Objetivos = new Dictionary<Vector2D, int>();//vectores que marcaran el inicio y el final
        private Dictionary<Vector2D, Nodo> espacios = new Dictionary<Vector2D, Nodo>();// diccionario de los nodos
        private Dictionary<Vector2D, int> Paredes = new Dictionary<Vector2D, int>();//diccionario para las paredes
        private int tamañoLab = 6;

        public void CreateMaze()
        {
            for (int i = 0; i < tamañoLab; i++)
            {
                for (int j = 0; j < tamañoLab; j++)
                {
                    Vector2D vectorCreado = new Vector2D(i,j); 
                    Nodo espacio = new Nodo(i, j);
                    espacios.Add(vectorCreado, espacio);
                }
            }
            do
            {
                Console.WriteLine("Escriba la coordenada X del inicio: ");
                xInicial = int.Parse(Console.ReadLine());
                Console.WriteLine("Escriba la coordenada Y del inicio: ");
                yInicial = int.Parse(Console.ReadLine());
                Console.WriteLine("Escriba la coordenada X del final: ");
                xFinal = int.Parse(Console.ReadLine());
                Console.WriteLine("Escriba la coordenada Y del final: ");
                yFinal = int.Parse(Console.ReadLine());

                if (xInicial < 0 || xFinal < 0 || yInicial < 0 || yFinal < 0 || xInicial > tamañoLab || yInicial > tamañoLab || xFinal > tamañoLab || yFinal > tamañoLab)
                {
                    Console.WriteLine("Escriba valores dentro del limite de 25");
                }

            } while (xInicial < 0 || xFinal < 0 || yInicial < 0 || yFinal < 0 || xInicial > tamañoLab || yInicial > tamañoLab || xFinal > tamañoLab || yFinal > tamañoLab);

            Vector2D nInicial = new Vector2D(xInicial, yInicial);
            Vector2D nFinal = new Vector2D(xFinal, yFinal);
            Objetivos.Add(nInicial, 1);
            Objetivos.Add(nFinal, 1);
            Inicio = espacios[nInicial];
            Final = espacios[nFinal];

            List<Vector2D> paredes = new List<Vector2D>();
            Vector2D pared1 = new Vector2D(3, 4);
            Vector2D pared2 = new Vector2D(3, 8);
            Vector2D pared3 = new Vector2D(5, 4);
            Vector2D pared4 = new Vector2D(2, 1);
            Vector2D pared5 = new Vector2D(1, 2);
            paredes.Add(pared1);
            paredes.Add(pared2);
            paredes.Add(pared3);
            paredes.Add(pared4);
            paredes.Add(pared5);

            foreach (Vector2D pared in paredes)
            {
                if (espacios.ContainsKey(pared))
                {
                    espacios.Remove(pared);
                    Paredes.Add(pared, 1);
                }
            }


            for (int i = 0; i < tamañoLab; i++)
            {
                for (int j = 0; j < tamañoLab; j++)
                {
                    Vector2D Pincel = new Vector2D(i, j);
                    if (Objetivos.ContainsKey(Pincel))
                    {
                        Console.Write("F");
                    }
                    else if (espacios.ContainsKey(Pincel))
                    {
                        Console.Write("O");
                    }
                    else if (Paredes.ContainsKey(Pincel))
                    {
                        Console.Write("X");
                    }
                }
                Console.WriteLine("");
            }

        }
        public void BFS()
        {
            cola.Enqueue(Inicio);
            while (cola.Count >0 && explorando)
            {
                punDeExplo = cola.Dequeue();
                OnReachingEnd();
                ExploreNeighbourNodes();
            } 


        }
        public void OnReachingEnd()
        {
            if (punDeExplo == Final)
            {
                explorando = false;
            }
            else
            {
                explorando = true;
            }

        }
        public void ExploreNeighbourNodes()
        {
            if (!explorando)
            {
                return;
            }
            foreach (var opcion in Direcciones)
            {
                Vector2D lugAmo = punDeExplo.GetPosition().ayudaExtra(opcion);
                if (espacios.ContainsKey(lugAmo))
                {
                    Nodo nuevoSitio = espacios[lugAmo];
                    if (!nuevoSitio.explorado)
                    {
                        nuevoSitio.exploradoDe = punDeExplo;
                        cola.Enqueue(nuevoSitio);
                        nuevoSitio.explorado = true;
                        
                    }
                }
            }
            
        }

        public void CreatePath()
        {
            SetPath(Final);
            Nodo caminoAnterior = Final.exploradoDe;
            while (caminoAnterior != Inicio)
            {
                SetPath(caminoAnterior);
                caminoTomado.Add(caminoAnterior);
                caminoAnterior = caminoAnterior.exploradoDe;
            }
            SetPath(Inicio);
            camino.Reverse();
        }
        public void SetPath(Nodo nodo)
        {
            camino.Add(nodo);
        }
        public void ShowPathTaken()
        {
            foreach (var item in caminoTomado)
            {
                Console.WriteLine(item.ubicacion);
            }
        }
    }
}
