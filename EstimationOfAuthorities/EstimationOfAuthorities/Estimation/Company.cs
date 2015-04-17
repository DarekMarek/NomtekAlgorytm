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
            if (!Employees.Exists(e => e.Name.Equals(em.Name))) Employees.Add(em);
            else {
                Employee emp = Employees.Find(e => e.Name.Equals(em.Name));
                emp.EmploymentDate = em.EmploymentDate;
                foreach (Role r in em.Roles) {
                    emp.AddRole(r);
                }
            }
        }

        public void CreateNewEvaluation() {
            EvaluationsAmount++;
            CurrentEvaluation = new Graph(this);
            Evaluations.Add(CurrentEvaluation);
            IsEvaluationInProgress = true;
        }

        public void CloseEvaluation(string evalName) {
            Graph eval = Evaluations.Find(e => e.Name == evalName);
            eval.IsFinished = true;
            eval.EstimateAuthorities();
            IsEvaluationInProgress = false;
        }
        #endregion
    }
}
