using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EstimationOfAuthorities.Estimation
{
    /// <summary>
    /// Klasa pracownika
    /// </summary>
   [XmlRoot("Employee")]
    public class Employee
    {
       #region Properties
        /// <summary>
        /// Imię i nazwisko pracownika
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// Data zatrudnienia pracownika
        /// </summary>

        public DateTime EmploymentDate{ get; set; }

        /// <summary>
        /// Lista ról pracowników
        /// </summary>
       [XmlArrayItem(ElementName = "Role", Type = typeof(Role))]
       public List<Role> Roles { get; set; }
       #endregion

       #region Constructors
       public Employee(string name, DateTime employDate) {
            Name = name;
            EmploymentDate = employDate;
            Roles = new List<Role>();
        }
       public Employee(){}
       #endregion

       #region Methods
       /// <summary>
        /// Dodanie roli do pracownika
        /// </summary>
        /// <param name="r">Nowa rola</param>
        public void AddRole(Role r) {
            if (!Roles.Exists(role => role.Name.Equals(r.Name))) Roles.Add(r);
        }
        public static string SerializeToXml(Employee p)
        {
            var writer = new StringWriter();
            var serializer = new XmlSerializer(typeof(Employee));
            serializer.Serialize(writer, p);

            return writer.ToString();
        }

        public static Employee DeserializeFromXml(string p)
        {
            StringReader reader = new StringReader(p);

            XmlSerializer serializer = new XmlSerializer(typeof(Employee));
            var em = (Employee)serializer.Deserialize(reader);

            return em;
        }

        #endregion
    }
}
