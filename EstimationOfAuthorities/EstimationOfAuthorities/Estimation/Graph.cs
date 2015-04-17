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
        private static int SEARCH_IN_X_ESTIMATIONS = 3;

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

            // ========== Wyliczenie globalnych wartości do obliczeń - DATA ZATRUDNIENIA
            DateTime EarliestEmploymentDate = FindEarliestEmploymentDate();
            Console.WriteLine("Earliest Employment Date:   " + EarliestEmploymentDate);

            DateTime Now = DateTime.Now;
            TimeSpan maxExperience = Now - EarliestEmploymentDate;
            // ////////// Wyliczenie globalnych wartości do obliczeń ...


            // ========== Wyliczenie globalnych wartości do obliczeń - ROLE UŻYTKOWNIKA

            
            // ////////// Wyliczenie globalnych wartości do obliczeń ...


            // ========== Wyliczenie globalnych wartości do obliczeń - CZĘSTOTLIWOŚC OCENIANIA
            double MaxEstimationCountByOneEmployee = FindMaxEstimationCountByOneEmployee(SEARCH_IN_X_ESTIMATIONS);
            Console.WriteLine("Max Estimation Made by one: " + MaxEstimationCountByOneEmployee);

            // ////////// Wyliczenie globalnych wartości do obliczeń ...


            // ========== Wyliczenie globalnych wartości do obliczeń - LICZBA PRZEPRACOWANYCH GODZIN
            double MaxWorkedHours = FindMaxWorkedHours();
            Console.WriteLine("Max Worked Hours:           " + MaxWorkedHours);

            // ////////// Wyliczenie globalnych wartości do obliczeń ...

            Console.WriteLine("-------------------------------------------");

            // Aktualny pracownik: n
            foreach (Node n in Nodes) {
                Console.WriteLine();
                Console.WriteLine(n.Employee.Name);

                // Dla każdego pracownika, który ocenił pracownika n, wyliczam wagę/ważność jego oceny na podstawie poniższych czynników
                int EstimatedByCount = 0;       // Liczba pracowników, którzy ocenili pracownika n
                foreach (Neighbour neigbour in n.Neighbours) {
                    if (n == neigbour.ToNode) {
                        EstimatedByCount++;
                        // ================ WYLICZENIE AUTORYTETU PRACOWNIKA  ================ //
                        TimeSpan experience = Now - neigbour.FromNode.Employee.EmploymentDate;

                        // =============== WAGA WYNIKAJĄCA Z DATY ZATRUDNIENIA =============== //
                        double EmploymentDateWeight = EvaluateEmplymentDateWeight(maxExperience, experience);
                        Console.WriteLine("     Waga wynikająca z daty zatrudnienia:     {0}", EmploymentDateWeight);

                        // ================ WAGA WYNIKAJĄCA Z ROLI PRACOWNIKA ================ //
                        double RolesWeight = 0.5;

                        // == WAGA WYNIKAJĄCA Z CZĘSTOTLIWOŚCI OCENIANIA INNYCH PRACOWNIKÓW == //
                        double EstimationFrequencyWeight = EvaluateEmplymentDateWeight(MaxEstimationCountByOneEmployee, neigbour.FromNode.Employee, SEARCH_IN_X_ESTIMATIONS);
                        Console.WriteLine("     Waga wynikająca z częst. oceniania:      {0}", EstimationFrequencyWeight);

                        // = WAGA WYNIKAJĄCA Z LICZBY PRZEPRACOWANYCH GODZIN Z PRACOWNIKIEM  = //
                        double WorkedHoursWeight = EvaluateWorkedHoursWeight(MaxWorkedHours, neigbour.WorkedHours);
                        Console.WriteLine("     Waga wynikająca z il. przper. czasu:     {0}", WorkedHoursWeight);

                        // ========== WYLICZENIE WAGI OCENY DANEGO WSPÓŁPRACOWNIKA  ========== //
                        // Tymczasowo średnia wszystkich powyższych wag
                        neigbour.EstimationWeight = (EmploymentDateWeight + RolesWeight + EstimationFrequencyWeight + WorkedHoursWeight) / 4;
                        Console.WriteLine("     Waga oceny {1,24}:     {0}", neigbour.EstimationWeight, neigbour.FromNode.Employee.Name);

                        n.EstimatedAutority += neigbour.EstimationWeight * neigbour.ValueForCompany;
                    }
                }
                // ================ WYLICZENIE AUTORYTETU PRACOWNIKA  ================ //
                // Wyliczenie autorytetu pracownika n na podstawie ocen współpracowników i wyżej wyliczonych wag dla każdego współpracownika.
                if (EstimatedByCount != 0) n.EstimatedAutority /= EstimatedByCount;
            }
            // ---------- ALGORYTM ESTYMACJI AUTORYTETÓW ---------- //

            Console.WriteLine();
            DisplayGraph();
            Console.WriteLine("-------------------------------------------");
        }

        //====================================================================================================
        //          METODY POMOCNICZE DO WYLICZENIA WAG I INNYCH WARTOŚCI UŻYWANYCH W ALGORYTMIE
        //====================================================================================================

        /// <summary>
        /// Pobranie najwcześniejszej daty zatrudnienia wśród wszystkich pracowników
        /// </summary>
        /// <returns></returns>
        private DateTime FindEarliestEmploymentDate() {
            DateTime earliest = DateTime.Now;
            foreach (Node node in Nodes) {
                if (earliest > node.Employee.EmploymentDate) {
                    earliest = node.Employee.EmploymentDate;
                }
            }
            return earliest;
        }

        /// <summary>
        /// Wyliczenie wagi wynikającej z daty zatrudnienia
        /// </summary>
        /// <param name="maxExperience">Najdłuższe możliwe doświadczenie uwzględniając najwcześniejszą datę zatrudnienia wśród wszystkich pracowników</param>
        /// <param name="employeeExperience">Długość stażu pracownika</param>
        /// <returns></returns>
        private double EvaluateEmplymentDateWeight(TimeSpan maxExperience, TimeSpan employeeExperience) {
            double weight = 0.0;
            //Console.WriteLine(maxExperience.Days);
            //Console.WriteLine(employeeExperience.Days);
            weight = (double)employeeExperience.Days / maxExperience.Days;//) / maxExperience.Days;

            return weight;
        }

        // ------------------------------------------------------------------------

        /// <summary>
        /// Znajduje maksymalną liczbę ocen dokonanych przez pracownika w ostatnich kilku iteracjach
        /// </summary>
        /// <param name="lastFew"></param>
        /// <returns></returns>
        private double FindMaxEstimationCountByOneEmployee(int lastFew) {
            double max = 0.0;
            //double maxFromOneEval = 0.0;

            foreach (Node node in Nodes) {
                double estimationsCount = GetEstimationCount(node.Employee, lastFew);
                if (max < estimationsCount) max = estimationsCount;
            }

            return max;
        }

        /// <summary>
        /// Zwraca liczbę dokonanych ocen przez pracownika w ostatnich kilku estymacji
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private double GetEstimationCount(Employee employee, int lastFew) {
            int estimationsCount = 0;
            for (int i = Corporation.Evaluations.Count - 1; i > Corporation.Evaluations.Count - 1 - lastFew && i >= 0; i--) {
                Graph current = Corporation.Evaluations[i];
                if (current.Nodes.Exists(n => n.Employee.Name == employee.Name)) {
                    Node temp = current.Nodes.Find(n => n.Employee.Name == employee.Name);
                    foreach (Neighbour n in temp.Neighbours) {
                        if (n.FromNode == temp) estimationsCount++;
                    }
                }
            }
            return estimationsCount;
        }

        /// <summary>
        /// Wyliczenie wagi wynikającej z częstotliwości oceniania innych pracowników;
        /// </summary>
        /// <param name="maxEstimationCountByOneEmployee"></param>
        /// <param name="currentEmployeeEstimationCount"></param>
        /// <returns></returns>
        private double EvaluateEmplymentDateWeight(double maxEstimationCountByOneEmployee, Employee currentEmployee, int lastFew) {
            return GetEstimationCount(currentEmployee, lastFew) / maxEstimationCountByOneEmployee;
        }

        // ------------------------------------------------------------------------

        /// <summary>
        /// Znalezienie największej liczby przepracowanych godzin
        /// </summary>
        /// <returns></returns>
        private double FindMaxWorkedHours() {
            return Nodes.Max(node => {
                if (node.Neighbours.Count > 0)
                    return node.Neighbours.Max(n => n.WorkedHours);
                else return 0.0;
            });
        }

        /// <summary>
        /// Wyliczenie wagi wynikającej z ilości przepracowanych godzin
        /// </summary>
        /// <param name="maxWorkedHours"></param>
        /// <returns></returns>
        private double EvaluateWorkedHoursWeight(double maxWorkedHours, double workedHours) {


            return (double)workedHours / maxWorkedHours;
        }

        //====================================================================================================
        //====================================================================================================
        //====================================================================================================

        /// <summary>
        /// Wyświetlenie grafu
        /// </summary>
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
