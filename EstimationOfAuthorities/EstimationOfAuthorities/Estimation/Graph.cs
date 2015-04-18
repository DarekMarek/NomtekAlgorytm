#define WEIGHTS
#undef WEIGHTS

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
        // TO DO
        // Rafał:
        // - Wziąć pod uwagę kilka poprzendich ocen
        // - Co jeśli użytkownik nie dostał oceny
        //
        // Przemo:
        // - Wziąć pod uwagę kilka estymacji przy przepracowanych godzinach
        // - Dane do testów

        private static int SEARCH_IN_X_ESTIMATIONS = 3;
        private static double A = Double.Parse(Properties.Resources.A);
        private static double B = Double.Parse(Properties.Resources.B);
        private static double C = Double.Parse(Properties.Resources.C);
        private static double D = Double.Parse(Properties.Resources.D);


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

        /// <summary>
        /// Gotówka do podziału
        /// </summary>
        public double CashToShare { get; set; }
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

            CashToShare = 20000;
        }


        #endregion

        #region Methods
        /// <summary>
        /// Wyestymowanie autorytetów w danej estymacji
        /// </summary>
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
            double MaxWorkedHours = FindMaxWorkedHours(SEARCH_IN_X_ESTIMATIONS);
            Console.WriteLine("Max Worked Hours:           " + MaxWorkedHours);

            // ////////// Wyliczenie globalnych wartości do obliczeń ...

            Console.WriteLine("-------------------------------------------");

            // Aktualny pracownik: n
            foreach (Node n in Nodes) {
#if WEIGHTS
                Console.WriteLine();
                Console.WriteLine(n.Employee.Name);
#endif

                // Dla każdego pracownika, który ocenił pracownika n, wyliczam wagę/ważność jego oceny na podstawie poniższych czynników
                int EstimatedByCount = 0;       // Liczba pracowników, którzy ocenili pracownika n
                foreach (Neighbour neigbour in n.Neighbours) {
                    if (n == neigbour.ToNode) {
                        var EstimatedBy = neigbour.FromNode;
                        EstimatedByCount++;
                        // ================ WYLICZENIE AUTORYTETU PRACOWNIKA  ================ //
                        TimeSpan experience = Now - EstimatedBy.Employee.EmploymentDate;

                        // =============== WAGA WYNIKAJĄCA Z DATY ZATRUDNIENIA =============== //
                        double EmploymentDateWeight = EvaluateEmplymentDateWeight(maxExperience, experience);

                        // ================ WAGA WYNIKAJĄCA Z ROLI PRACOWNIKA ================ //
                        double RolesWeight = EvaluateRolesWeight(EstimatedBy.Employee);

                        // == WAGA WYNIKAJĄCA Z CZĘSTOTLIWOŚCI OCENIANIA INNYCH PRACOWNIKÓW == //
                        double EstimationFrequencyWeight = EvaluateEmplymentDateWeight(MaxEstimationCountByOneEmployee, EstimatedBy.Employee, SEARCH_IN_X_ESTIMATIONS);

                        // = WAGA WYNIKAJĄCA Z LICZBY PRZEPRACOWANYCH GODZIN Z PRACOWNIKIEM  = //
                        double WorkedHoursWeight = EvaluateWorkedHoursWeight(MaxWorkedHours, GetWorkedHours(SEARCH_IN_X_ESTIMATIONS, EstimatedBy.Employee, neigbour.ToNode.Employee));

                        // ========== WYLICZENIE WAGI OCENY DANEGO WSPÓŁPRACOWNIKA  ========== //
                        // Tymczasowo średnia wszystkich powyższych wag
                        neigbour.EstimationWeight = (A*EmploymentDateWeight + B*RolesWeight + C*EstimationFrequencyWeight + D*WorkedHoursWeight) / 4;

#if WEIGHTS
                        Console.WriteLine("     Waga wynikająca z daty zatrudnienia:     {0}", EmploymentDateWeight);
                        Console.WriteLine("     Waga wynikająca z roli użytkownika:      {0}", RolesWeight);
                        Console.WriteLine("     Waga wynikająca z częst. oceniania:      {0}", EstimationFrequencyWeight);
                        Console.WriteLine("     Waga wynikająca z il. przper. czasu:     {0}", WorkedHoursWeight);
                        Console.WriteLine("     Waga oceny {1,24}:     {0}", neigbour.EstimationWeight, EstimatedBy.Employee.Name);
#endif
                        n.EstimatedAutority += neigbour.EstimationWeight * neigbour.ValueForCompany;
                    }
                }
                // ================ WYLICZENIE AUTORYTETU PRACOWNIKA  ================ //
                // Wyliczenie autorytetu pracownika n na podstawie ocen współpracowników i wyżej wyliczonych wag dla każdego współpracownika.
                if (EstimatedByCount != 0) n.EstimatedAutority /= EstimatedByCount;

                // ============== UWZGLĘDNIENIE POPRZEDNICH KILKU OCEN  ============== //
                // Do wymyślenia
                n.EstimatedAutority = n.EstimatedAutority;

            }

            // ====================== MAKSYMALNA  ESTYMACJA ====================== //
            double MaxEstimationValue = Nodes.Max(n => n.EstimatedAutority);
            
            // ================ GDY PRACOWNIK NIE ZOSTAŁ OCENIONY ================ //
            // Aktualny pracownik: n
            foreach (Node n in Nodes) {
                if (n.EstimatedAutority == 0) n.EstimatedAutority = MaxEstimationValue / 2;
            }

            // ========================= POMINIĘCIE  CEO ========================= //
            if (Corporation.CEO != null) {
                Nodes.Find(n => n.Employee.Name == Corporation.CEO.Name).EstimatedAutority = 0;// MaxEstimationValue * 1.5;
                //Console.WriteLine(Nodes.Find(n => n.Employee.Name == Corporation.CEO.Name));
            }

            // ==================== SUMA WSZYSTKICH ESTYMACJI ==================== //
            double TotalEstimationsValue = Nodes.Sum(n => n.EstimatedAutority);

            // ===================== PRZYDZIAŁ WYNAGRODZENIA ===================== //
            // Aktualny pracownik: n
            foreach (Node n in Nodes) {
                n.Salary = (n.EstimatedAutority / TotalEstimationsValue) * CashToShare;
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
        /// Wyliczenie wagi wynikającej z posiadanych ról użytkownika
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        private double EvaluateRolesWeight(Employee emp) {
            double weight = 0.0;

            weight = emp.Roles.Count > 0 ?
                (double)emp.Roles.Max(role => role.Priority) / 10 + 0.4 :
                0.4;

            //foreach (Role role in emp.Roles) {
            //    weight += role.Priority;
            //}
            //weight = emp.Roles.Count > 0 ?
            //    (weight / emp.Roles.Count) + 0.5 :
            //    0.1;

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
        /// <param name="employee">Pracownik</param>
        /// <param name="lastFew">Liczba ostatnich estymacji</param>
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
        /// Znalezienie największej liczby przepracowanych godzin między współpracownikami w ciągu ostatnich kilku estymacji
        /// </summary>
        /// <param name="lastFew">Liczba ostatnich estymacji</param>
        /// <returns></returns>
        private double FindMaxWorkedHours(int lastFew) {
            //return Nodes.Max(node => {
            //    if (node.Neighbours.Count > 0)
            //        return node.Neighbours.Max(n => n.WorkedHours);
            //    else return 0.0;
            //});

            double max = 0.0;

            foreach (Node node in Nodes) {
                foreach (Neighbour n in node.Neighbours) {
                    double workedHours = 0;// = GetWorkedHours(lastFew, node.Employee);
                    if (n.ToNode != node) {
                        workedHours += GetWorkedHours(lastFew, node.Employee, n.ToNode.Employee);
                        //Console.WriteLine(workedHours);
                    }
                    if (max < workedHours) max = workedHours;
                }
            }

            return max;
        }

        /// <summary>
        /// Zwraca liczbę przepracowanych godzin z danym pracownikiem w ciągu ostatnich kilku estymacji
        /// </summary>
        /// <param name="emp"></param>
        /// <param name="lastFew"></param>
        /// <returns></returns>
        private double GetWorkedHours(int lastFew, Employee employee, Employee with = null) {
            double workedHours = 0;
            for (int i = Corporation.Evaluations.Count - 1; i > Corporation.Evaluations.Count - 1 - lastFew && i >= 0; i--) {
                Graph current = Corporation.Evaluations[i];
                if (current.Nodes.Exists(n => n.Employee.Name == employee.Name)) {
                    Node temp = current.Nodes.Find(n => n.Employee.Name == employee.Name);
                    foreach (Neighbour n in temp.Neighbours) {
                        if (with == null) {
                            if (n.ToNode != temp) workedHours += n.WorkedHours;
                        } else {
                            if (n.ToNode.Employee.Name == with.Name) workedHours += n.WorkedHours;
                        }

                    }
                }
            }

            return workedHours;
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
                Console.WriteLine("  Salary:     " + n.Salary);
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
