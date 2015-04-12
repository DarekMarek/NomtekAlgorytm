using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EstimationOfAuthorities.Estimation;
using System.ComponentModel;
using System.Xml.Serialization;
using System.IO;

namespace EstimationOfAuthorities
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Company nomtek;

        #region Constructors
        public MainWindow() {
            InitializeComponent();

            nomtek = new Company("Nomtek");
            #region AddingRoles
            nomtek.AddRole(new Role("admin", 0));
            nomtek.AddRole(new Role("accountant", 1));
            nomtek.AddRole(new Role("external", 2));
            nomtek.AddRole(new Role("internal", 3));
            nomtek.AddRole(new Role("line_manager", 4));
            nomtek.AddRole(new Role("cashier", 5));
            nomtek.AddRole(new Role("ceo", 6));
            #endregion
            DeserializeFromXml();
            RefreshLists();
            RBEnableEmployeesPanel.Checked += EnableEmployeesPanel;
            RBEnableCEOPanel.Checked += EnableCEOPanel;
            RBEnableCEOPanel.IsChecked = true;
            EmployeeName.Text = "Imię Nazwisko";

            foreach (Role r in nomtek.Roles) {
                CBRoles.Items.Add(r.Name);
            }
        }
        #endregion

        #region Events
        private void EnableEmployeesPanel(object sender, RoutedEventArgs e) {
            //EmployeeColumn
            //ListOfEmployees1.IsEnabled = true;
            EmployeeChooser.IsEnabled = true;
            EmployeePanel.IsEnabled = true;
            CEOChooser.IsEnabled = false;
            CEOPanel.IsEnabled = false;
            CEOPanelAddEmployee.IsEnabled = false;
        }
        private void EnableCEOPanel(object sender, RoutedEventArgs e) {

            EmployeeChooser.IsEnabled = false;
            EmployeePanel.IsEnabled = false;
            CEOChooser.IsEnabled = true;
            CEOPanel.IsEnabled = true;
            CEOPanelAddEmployee.IsEnabled = true;
            //ListOfEmployees1.IsEnabled = false;
        }
        /// <summary>
        /// Ustawienie widoku konkretnego pracownika
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedEmployeeView(object sender, SelectionChangedEventArgs e) {

        }
        private void AddEditEmployee(object sender, RoutedEventArgs e) {
            if (!EmployeeName.Text.Equals("") && Calendar.SelectedDate != null && CBRoles.SelectedIndex != -1) {
                Employee newEmp = new Employee(EmployeeName.Text, (DateTime)Calendar.SelectedDate);
                newEmp.AddRole(nomtek.Roles[CBRoles.SelectedIndex]);
                nomtek.AddEditEmployee(newEmp);
                RefreshButton();
                RefreshLists();
            }
            else MessageBox.Show("Nie przekazano wszystkich danych");

        }
        private void EmployeeNameChanged(object sender, TextChangedEventArgs e) {
            RefreshButton();
        }
        private void DataWindow_Closing(object sender, EventArgs e)
        {
            MessageBox.Show("Dane zostały zapisane.");
            SerializeToXML();
        }
        private void Show_EmployeesDetails(object sender, RoutedEventArgs e)
        {
            string name = (string)ListOfEmployees2.SelectedValue;
            Employee employee = null;
            foreach(var em in nomtek.Employees)
                if (em.Name == name)
                    employee = em;
            
            if(employee!=null)
            {
                DetailsLabel.Visibility = Visibility.Visible;
                EmployeesDetails.Visibility = Visibility.Visible;

                string role =String.Empty;
                foreach(var r in employee.Roles)
                    role+=(r.Name+"\n");

                string text = "Nazwa: " + employee.Name + "\nData zatrudnienia " + employee.EmploymentDate + "\nRole: "+role;
                EmployeesDetails.Text = text;
            }
        }
        #endregion

        #region Methods
        private void RefreshButton() {
            if (nomtek.Employees.Exists(em => em.Name.Equals(EmployeeName.Text))) AddEditButton.Content = "EDYTUJ PRACOWNIKA";
            else AddEditButton.Content = "DODAJ PRACOWNIKA";
        }
        /// <summary>
        /// Odświeżenie list pracowników
        /// </summary>
        private void RefreshLists() {
            ListOfEmployees1.Items.Clear();
            ListOfEmployees2.Items.Clear();
            CBEmployees.Items.Clear();

            foreach (Employee emp in nomtek.Employees) {
                ListOfEmployees1.Items.Add(emp.Name);
                ListOfEmployees2.Items.Add(emp.Name);
                CBEmployees.Items.Add(emp.Name);
            }
        }
        public void SerializeToXML()
        {
            XmlSerializer oSerializer = new XmlSerializer(typeof(List<Employee>), new Type[] { typeof(Role) });
            StreamWriter oStreamWriter = null;
            try
            {
                oStreamWriter = new StreamWriter("../../employee.xml");
                oSerializer.Serialize(oStreamWriter,nomtek.Employees);
            }
            catch (Exception oException)
            {
                Console.WriteLine("Aplikacja wygenerowała następujący wyjątek: " + oException.Message);
            }
            finally
            {
                if (null != oStreamWriter)
                {
                    oStreamWriter.Dispose();
                }
            }
        }
        public void DeserializeFromXml()
        {
            XmlSerializer oSerializer = null;
            StreamReader reader = null;
            FileInfo file = new FileInfo("../../employee.xml");

            if (file.Exists)
            {
                try
                {
                    reader = new StreamReader("../../employee.xml");
                    oSerializer = new XmlSerializer(typeof(List<Employee>), new Type[] { typeof(Role) });
                }
                catch (Exception oException)
                {
                    Console.WriteLine("Aplikacja wygenerowała następujący wyjątek: " + oException.Message);
                }
                nomtek.Employees = (List<Employee>)oSerializer.Deserialize(reader);
            }
        }
        #endregion

      
        
    }
}
