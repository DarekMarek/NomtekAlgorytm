using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimationOfAuthorities.Estimation
{
    /// <summary>
    /// Klasa grafu skierowanego
    /// </summary>
    class Graph
    {
        #region Properties
        /// <summary>
        /// Nazwa ewaluacji
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Lista wierzchołków
        /// </summary>
        public List<Node> Nodes { get; set; }

        /// <summary>
        /// Firma której graf dotyczy
        /// </summary>
        public Company Corporation { get; set; }

        /// <summary>
        /// Czy okres ewaluacji dobiegł końca (czy można wciąż oceniać)
        /// </summary>
        public bool IsFinished { get; set; }
        #endregion

        #region Constructors
        public Graph(Company comp) {
            Nodes = new List<Node>();
            Corporation = comp;
            IsFinished = false;
            Name = "Evaluation " + comp.EvaluationsAmount;

            foreach (Employee e in comp.Employees) {
                Nodes.Add(new Node(e));
            }
        }


        #endregion

        #region Methods
        public void EstimateAuthorities() {
            Console.WriteLine("===========================================");
            Console.WriteLine("              {0}", Name);
            Console.WriteLine("-------------------------------------------");

            // ========== ALGORYTM ESTYMACJI AUTORYTETÓW ========== //




            // ---------- ALGORYTM ESTYMACJI AUTORYTETÓW ---------- //

            DisplayGraph();
            Console.WriteLine("-------------------------------------------");
        }

        private void DisplayGraph() {
            foreach (Node n in Nodes) {
                Console.WriteLine(n.Employee.Name);
                Console.WriteLine("  Authority:  " + n.EstimatedAutority);
                foreach (Neighbour ne in n.Neighbours) {
                    if (ne.ToNode.Employee == n.Employee) {
                        Console.WriteLine("   TO: " + ne.ToNode.Employee.Name);
                        Console.WriteLine("     From Employee:     " + ne.FromNode.Employee.Name);
                        Console.WriteLine("     Value for Company: " + ne.ValueForCompany);
                        Console.WriteLine("     Time Worked:       " + ne.WorkedHours);
                    }
                }
            }
        }
        #endregion
    }
}
