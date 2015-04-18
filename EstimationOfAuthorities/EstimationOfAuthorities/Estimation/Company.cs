using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstimationOfAuthorities.Estimation
{
    /// <summary>
    /// Klasa firmy
    /// </summary>
    class Company
    {
        #region Properties
        /// <summary>
        /// Nazwa firmy
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Lista pracowników
        /// </summary>
        public List<Employee> Employees { get; set; }

        /// <summary>
        /// Lista ról jakie pracownicy mogą przyjąć
        /// </summary>
        public List<Role> Roles { get; set; }

        /// <summary>
        /// Zapamiętane ewaluacje
        /// </summary>
        public List<Graph> Evaluations { get; set; }

        /// <summary>
        /// Aktualna ewaluacja
        /// </summary>
        public Graph CurrentEvaluation { get; set; }

        /// <summary>
        /// Liczba wszystkich ewaluacji
        /// </summary>
        public int EvaluationsAmount { get; set; }

        /// <summary>
        /// Czy obecna ewaluacje jest w trakcie
        /// </summary>
        public bool IsEvaluationInProgress { get; set; }

        /// <summary>
        /// Prezes firmy
        /// </summary>
        public Employee CEO { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Główny konstruktor
        /// </summary>
        /// <param name="name">Nazwa firmy</param>
        public Company(string name) {
            Name = name;
            Employees = new List<Employee>();
            Roles = new List<Role>();
            EvaluationsAmount = 0;
            CurrentEvaluation = null;
            IsEvaluationInProgress = false;
            Evaluations = new List<Graph>();
            CEO = null;
        }
        #endregion

        /// <summary>
        /// Dodanie roli jaką pracownicy mogą przyjąć
        /// </summary>
        /// <param name="r">Nowa rola</param>
        public void AddRole(Role r) {
            if (!Roles.Exists(role => role.Name.Equals(r.Name))) Roles.Add(r);
        }

        #region Methods
        /// <summary>
        /// Dodanie nowego pracownika do firmy
        /// </summary>
        /// <param name="em">Nowy pracownik</param>
        public void AddEditEmployee(Employee em) {
            if (!Employees.Exists(e => e.Name.Equals(em.Name))) {
                if (em.Roles.Exists(r => r.Priority == 6)) CEO = em;
                Employees.Add(em);
            } else {
                Employee emp = Employees.Find(e => e.Name.Equals(em.Name));
                emp.EmploymentDate = em.EmploymentDate;
                emp.Roles.Clear();
                foreach (Role r in em.Roles) {
                    emp.AddRole(r);
                }
                if (emp.Roles.Exists(r => r.Priority == 6)) CEO = emp;
            }
        }

        /// <summary>
        /// Rozpoczęcie nowej ewaluacji
        /// </summary>
        public void CreateNewEvaluation() {
            EvaluationsAmount++;
            CurrentEvaluation = new Graph(this);
            Evaluations.Add(CurrentEvaluation);
            IsEvaluationInProgress = true;
        }

        /// <summary>
        /// Zakończenie zadanej ewaluacji
        /// </summary>
        /// <param name="evalName"></param>
        public void CloseEvaluation(string evalName) {
            Graph eval = Evaluations.Find(e => e.Name == evalName);
            eval.IsFinished = true;
            eval.EstimateAuthorities();
            IsEvaluationInProgress = false;
        }

        //private void NewNeighbour(object sender, RoutedEventArgs e) {
        //    double value = 0;
        //    double time = 0;
        //    if (CBEmployees.SelectedIndex != -1 && ListOfEmployees1.SelectedIndex != -1 &&
        //        Double.TryParse(ValueForCompany.Text, out value) && Double.TryParse(TimeWorked.Text, out time)) {
        //        Node employee = nomtek.CurrentEvaluation.Nodes.Find(n => n.Employee.Name == CBEmployees.SelectedValue.ToString());
        //        Node next = nomtek.CurrentEvaluation.Nodes.Find(n => n.Employee.Name == ListOfEmployees1.SelectedValue.ToString());
        //        if (employee != next) {
        //            Neighbour n = new Neighbour(employee, next, value, time);
        //            employee.AddNeighbour(n);
        //            next.AddNeighbour(n);
        //            MessageBox.Show("Oceniono pracownika");
        //        }
        //    }
        //}

        public void TestData1() {
            // Dane testowe obejmują 10 estymacji (załóżmy, że są wykonywane co miesiąc)

            // UŻYTKOWNICY:

            // CEO - niech on już nie daje ocen, bo tylko nas kontroluje
            string Krzysztof = "Krzysztof Choma";

            /* Rafał
             * Przez wszystkie estymacje radzi sobie na poziomie obiektywnej oceny równej 70
             * Nikogo nie ocenia
             */
            string Rafal = "Rafał Broda";

            /* Paulina
             * W każdej estymacji radzi sobie na poziomie obiektywnej oceny równej NR_ESTYMACJI * 10
             * Ocenia wszystkich z kim pracowała w co drugiej estymacji
             */
            string Paulina = "Paulina Brzechffa";

            /* Magda
             * W estymacjach 1-5 słabo sobie radzi (obiektywna ocena - 10), ale w estymacjach 6-10 zaczyna sobie dobrze radzić (obiektywna ocena - 80)
             * Ocenia wszystkich z kim pracowała w każdej estymacji
             */
            string Magda = "Magdalena Roksela";

            /* Przemek
             * W estymacjach 1-5 sobie radzi bardzo dobrze (obiektywna ocena - 80), ale w estymacjach 6-10 zaczyna sobie gorzej radzić (obiektywna ocena - 30)
             * Często ocenia (co czwartą estymację nie ocenia)
             * Zawyża oceny
             */
            string Przemek = "Przemysław Kołaczek";

            /* Piotrek
             * Radzi sobie na poziomie obiektywnej oceny równej 50
             * Jego nikt nie lubi i nikt go nie ocenia
             * Rzadko ocenia (ocenia co trzecią estymację)
             * Zaniża oceny innym
             */
            string Piotrek = "Piotrek Sobierski";

            /* OPIS WSPÓŁPRACY:
             * - Rafał   współpracuje z Pauliną   w estymacjach 1-3 i 8-10
             * - Przemek współpracuje z Magda     w estymacjach 1-3 i 8-10
             * - Przemek współpracuje z Pauliną   w estymacjach 4-7
             * - Rafał   współpracuje z Magda     w estymacjach 4-7
             * - Magda   współpracuje z Pauliną   w estymacjach 3-5 i 7-9
             * - Przemek współpracuje z Rafałem   w estymacjach 1-5
             * - Przemek współpracuje z Piotrkiem w estymacjach 1-4
             * - Rafał   współpracuje z Piotrkiem w estymacjach 7-10
             * - Paulina współpracuje z Piotrkiem w estymacjach 6-10
             * - Magda   współpracuje z Piotrkiem w estymacjach 1-5
             */

            /* 
             * 
             * 
             */


            // ========================================================= //
            // ====================== EWALUACJA 1 ====================== //
            // ========================================================= //
            CreateNewEvaluation();

            // SPRAWDZENIE, CZY PRACOWNICY ISTNIEJĄ
            if (!CurrentEvaluation.Nodes.Exists(n => n.Employee.Name == Krzysztof) ||
                !CurrentEvaluation.Nodes.Exists(n => n.Employee.Name == Rafal) ||
                !CurrentEvaluation.Nodes.Exists(n => n.Employee.Name == Paulina) ||
                !CurrentEvaluation.Nodes.Exists(n => n.Employee.Name == Magda) ||
                !CurrentEvaluation.Nodes.Exists(n => n.Employee.Name == Przemek) ||
                !CurrentEvaluation.Nodes.Exists(n => n.Employee.Name == Piotrek))
                throw new Exception("Employee does not exists!");

            //Oceny Rafała

            //Oceny Pauliny
            AddEstimation(Paulina, Rafal, 100, 160);

            //Oceny Magdy
            AddEstimation(Magda, Przemek, 100, 80);

            //Oceny Przemka
            AddEstimation(Przemek, Rafal, 100, 55);
            AddEstimation(Przemek, Magda, 60, 55);

            //Oceny Piotrka
            AddEstimation(Piotrek, Przemek, 100, 80);
            AddEstimation(Piotrek, Magda, 15, 80);

            CloseEvaluation(CurrentEvaluation.Name);

            // ========================================================= //
            // ====================== EWALUACJA 2 ====================== //
            // ========================================================= //
            CreateNewEvaluation();

            //Oceny Rafała

            //Oceny Pauliny

            //Oceny Magdy
            AddEstimation(Magda, Przemek, 100, 80);

            //Oceny Przemka
            AddEstimation(Przemek, Rafal, 100, 55);
            AddEstimation(Przemek, Magda, 55, 55);

            //Oceny Piotrka

            CloseEvaluation(CurrentEvaluation.Name);

            // ========================================================= //
            // ====================== EWALUACJA 3 ====================== //
            // ========================================================= //
            CreateNewEvaluation();

            //Oceny Rafała

            //Oceny Pauliny
            AddEstimation(Paulina, Rafal, 100, 80);
            AddEstimation(Paulina, Magda, 20, 80);

            //Oceny Magdy
            AddEstimation(Magda, Przemek, 100, 55);
            AddEstimation(Magda, Paulina, 40, 55);

            //Oceny Przemka
            AddEstimation(Przemek, Rafal, 100, 55);
            AddEstimation(Przemek, Magda, 65, 55);

            //Oceny Piotrka


            CloseEvaluation(CurrentEvaluation.Name);

            // ========================================================= //
            // ====================== EWALUACJA 4 ====================== //
            // ========================================================= //
            CreateNewEvaluation();

            //Oceny Rafała

            //Oceny Pauliny

            //Oceny Magdy
            AddEstimation(Magda, Rafal, 100, 55);
            AddEstimation(Magda, Paulina, 50, 55);

            //Oceny Przemka

            //Oceny Piotrka
            AddEstimation(Piotrek, Przemek, 100, 80);
            AddEstimation(Piotrek, Magda, 25, 80);

            CloseEvaluation(CurrentEvaluation.Name);

            // ========================================================= //
            // ====================== EWALUACJA 5 ====================== //
            // ========================================================= //
            CreateNewEvaluation();

            //Oceny Rafała

            //Oceny Pauliny
            AddEstimation(Paulina, Przemek, 100, 80);
            AddEstimation(Paulina, Magda, 25, 80);

            //Oceny Magdy
            AddEstimation(Magda, Rafal, 100, 55);
            AddEstimation(Magda, Paulina, 60, 55);

            //Oceny Przemka
            AddEstimation(Przemek, Rafal, 100, 80);
            AddEstimation(Przemek, Paulina, 75, 80);

            //Oceny Piotrka


            CloseEvaluation(CurrentEvaluation.Name);

            // ========================================================= //
            // ====================== EWALUACJA 6 ====================== //
            // ========================================================= //
            CreateNewEvaluation();

            //Oceny Rafała

            //Oceny Pauliny

            //Oceny Magdy
            AddEstimation(Magda, Rafal, 100, 160);

            //Oceny Przemka
            AddEstimation(Przemek, Paulina, 100, 160);

            //Oceny Piotrka


            CloseEvaluation(CurrentEvaluation.Name);

            // ========================================================= //
            // ====================== EWALUACJA 7 ====================== //
            // ========================================================= //
            CreateNewEvaluation();

            //Oceny Rafała

            //Oceny Pauliny
            AddEstimation(Paulina, Przemek, 50, 55);
            AddEstimation(Paulina, Magda, 100, 55);
            AddEstimation(Paulina, Piotrek, 70, 55);

            //Oceny Magdy
            AddEstimation(Magda, Rafal, 100, 80);
            AddEstimation(Magda, Paulina, 85, 80);

            //Oceny Przemka
            AddEstimation(Przemek, Paulina, 100, 160);

            //Oceny Piotrka
            AddEstimation(Piotrek, Rafal, 100, 80);
            AddEstimation(Piotrek, Paulina, 60, 80);

            CloseEvaluation(CurrentEvaluation.Name);

            // ========================================================= //
            // ====================== EWALUACJA 8 ====================== //
            // ========================================================= //
            CreateNewEvaluation();

            //Oceny Rafała

            //Oceny Pauliny

            //Oceny Magdy
            AddEstimation(Magda, Paulina, 100, 80);
            AddEstimation(Magda, Przemek, 40, 80);

            //Oceny Przemka

            //Oceny Piotrka


            CloseEvaluation(CurrentEvaluation.Name);

            // ========================================================= //
            // ====================== EWALUACJA 9 ====================== //
            // ========================================================= //
            CreateNewEvaluation();

            //Oceny Rafała

            //Oceny Pauliny
            AddEstimation(Paulina, Rafal, 95, 55);
            AddEstimation(Paulina, Magda, 100, 55);
            AddEstimation(Paulina, Piotrek, 70, 55);

            //Oceny Magdy
            AddEstimation(Magda, Paulina, 100, 80);
            AddEstimation(Magda, Przemek, 30, 80);

            //Oceny Przemka
            AddEstimation(Przemek, Magda, 100, 160);

            //Oceny Piotrka


            CloseEvaluation(CurrentEvaluation.Name);

            // ========================================================= //
            // ====================== EWALUACJA 10 ====================== //
            // ========================================================= //
            CreateNewEvaluation();

            //Oceny Rafała

            //Oceny Pauliny

            //Oceny Magdy
            AddEstimation(Magda, Przemek, 100, 160);

            //Oceny Przemka
            AddEstimation(Przemek, Magda, 100, 160);

            //Oceny Piotrka
            AddEstimation(Piotrek, Rafal, 100, 80);
            AddEstimation(Piotrek, Paulina, 90, 80);

            CloseEvaluation(CurrentEvaluation.Name);

        }

        private void AddEstimation(string from, string to, double value, double workedHours) {
            Node employee = CurrentEvaluation.Nodes.Find(n => n.Employee.Name == from);
            Node next = CurrentEvaluation.Nodes.Find(n => n.Employee.Name == to);
            Neighbour neighbour = new Neighbour(employee, next, value, workedHours);
            employee.AddNeighbour(neighbour);
            next.AddNeighbour(neighbour);
        }
        #endregion
    }
}
