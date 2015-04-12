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
        /// Główny konstruktor
        /// </summary>
        /// <param name="name">Nazwa firmy</param>
        public Company(string name) {
            Name = name;
            Employees = new List<Employee>();
            Roles = new List<Role>();
        }

        /// <summary>
        /// Dodanie roli jaką pracownicy mogą przyjąć
        /// </summary>
        /// <param name="r">Nowa rola</param>
        public void AddRole(Role r) {
            if (!Roles.Exists(role => role.Name.Equals(r.Name))) Roles.Add(r);
        }

        /// <summary>
        /// Dodanie nowego pracownika do firmy
        /// </summary>
        /// <param name="em">Nowy pracownik</param>
        public void AddEditEmployee(Employee em) {
            if (!Employees.Exists(e => e.Name.Equals(em.Name))) Employees.Add(em);
            else {
                Employee emp = Employees.Find(e => e.Name.Equals(em.Name));
                emp.EmploymentDate = emp.EmploymentDate;
                foreach (Role r in em.Roles) {
                    emp.AddRole(r);
                }
            }
        }
    }
}
