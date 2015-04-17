using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimationOfAuthorities.Estimation
{
    /// <summary>
    /// Klasa krawędzi grafu
    /// </summary>
    class Neighbour
    {
        #region Properties
        /// <summary>
        /// Wierzchołek na który wskazuje sąsiedztwo
        /// </summary>
        public Node ToNode { get; set; }

        /// <summary>
        /// Wierzchołek na który wskazuje sąsiedztwo
        /// </summary>
        public Node FromNode { get; set; }

        /// <summary>
        /// Wartość pracownika dla firmy
        /// </summary>
        public double ValueForCompany { get; set; }

        /// <summary>
        /// Przepracowane godziny z pracownikiem
        /// </summary>
        public double WorkedHours { get; set; }

        /// <summary>
        /// Waga oceny
        /// </summary>
        public double EstimationWeight { get; set; }
        #endregion

        #region Constructors
        public Neighbour(Node from_node, Node to_node, double value, double hours) {
            FromNode = from_node;
            ToNode = to_node;
            ValueForCompany = value;
            WorkedHours = hours;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Czy opis sąsiada wskazuje na zadany wierzchołek
        /// </summary>
        /// <param name="n">wierzchołek</param>
        /// <returns></returns>
        public bool ContainsEmployee(Node from, Node to) {
            return FromNode.Employee.Name == from.Employee.Name && ToNode.Employee.Name == to.Employee.Name;
        }

        #endregion
    }
}
