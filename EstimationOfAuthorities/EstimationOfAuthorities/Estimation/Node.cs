using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimationOfAuthorities.Estimation
{
    /// <summary>
    /// Klasa wierzchołku grafu
    /// </summary>
    class Node
    {
        #region Properties
        /// <summary>
        /// Pracownik firmy
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        /// Wyestymowany autorytet
        /// </summary>
        public double EstimatedAutority { get; set; }

        /// <summary>
        /// Lista sąsiedztwa (lista pracowników, którzy ocenili danego pracownika (whis.Employee) - jak pracownik (this.Employee) został oceniony przez innych pracowników)
        /// </summary>
        public List<Neighbour> Neighbours { get; set; }
        #endregion

        #region Constructors
        public Node(Employee emp) {
            Employee = emp;
            Neighbours = new List<Neighbour>();
            EstimatedAutority = 0.0;
        }

        #endregion

        #region Methods
        public void AddNeighbour(Neighbour n) {
            if (Neighbours.Exists(ne => ne.ContainsEmployee(n.FromNode, n.ToNode))) {
                Neighbour current = Neighbours.Find(ne => ne.ContainsEmployee(n.FromNode, n.ToNode));
                current.ValueForCompany = n.ValueForCompany;
                current.WorkedHours = n.WorkedHours;
                //Console.WriteLine("TO: " + n.ToNode.Employee.Name);
            } else
                Neighbours.Add(n);
        }

        #endregion
    }
}
