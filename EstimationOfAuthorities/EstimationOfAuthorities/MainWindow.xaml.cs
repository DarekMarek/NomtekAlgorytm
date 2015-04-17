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
        bool evaluationInProgress = false;
        bool allowChangeSelectedValue = true, allowChangeText = true;
        CheckBox[] CBRoles;

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

            CBRoles = new CheckBox[nomtek.Roles.Count];
            int i = 0;
            StackPanel sp = new StackPanel();
            foreach (Role r in nomtek.Roles) {
                //CBRoles.Items.Add(r.Name);
                CheckBox cb = new CheckBox();
                //cb.Name = "Role" + i;
                cb.Content = r.Name;
                CBRoles[i++] = cb;
                sp.Children.Add(cb);
            }
            Roles.Content = sp;

            EmployeeName.TextChanged += EmployeeNameChanged;
            EmployeeName.Text = "Imię Nazwisko";

            allowChangeSelectedValue = true;
        }
        #endregion

        #region Events
        private void EnableEmployeesPanel(object sender, RoutedEventArgs e) {
            //EmployeeColumn
            //ListOfEmployees1.IsEnabled = true;
            if (nomtek.IsEvaluationInProgress) {
                EmployeeChooser.IsEnabled = true;
                EmployeePanel.IsEnabled = true;
            }
            CEOChooser.IsEnabled = false;
            CEOPanel.IsEnabled = false;
            CEOPanelAddEmployee.IsEnabled = false;
            NewEvaluation.IsEnabled = false;
        }
        private void EnableCEOPanel(object sender, RoutedEventArgs e) {

            EmployeeChooser.IsEnabled = false;
            EmployeePanel.IsEnabled = false;
            CEOChooser.IsEnabled = true;
            CEOPanel.IsEnabled = true;
            CEOPanelAddEmployee.IsEnabled = true;
            NewEvaluation.IsEnabled = true;
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
            if (!EmployeeName.Text.Equals("") && CalendarE.SelectedDate != null) {// && CBRoles.SelectedIndex != -1) {
                Employee newEmp = new Employee(EmployeeName.Text, (DateTime)CalendarE.SelectedDate);
                //newEmp.AddRole(nomtek.Roles[CBRoles.SelectedIndex]);
                for (int i = 0; i < CBRoles.Length; i++) {
                    if (CBRoles[i].IsChecked.Value) {
                        newEmp.AddRole(nomtek.Roles[i]);
                    }
                }
                nomtek.AddEditEmployee(newEmp);
                RefreshControls();
                RefreshLists();
                ListOfEmployees2.SelectedValue = newEmp.Name;
                Show_EmployeesDetails();
            }
            else MessageBox.Show("Nie przekazano wszystkich danych");

        }
        private void EmployeeNameChanged(object sender, TextChangedEventArgs e) {
            //if (allowChangeText) {
                RefreshControls();
            //} else allowChangeText = true;
        }
        private void DataWindow_Closing(object sender, EventArgs e)
        {
            MessageBox.Show("Dane zostały zapisane.");
            SerializeToXML();
        }
        private void Show_EmployeesDetails()
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

                string role = String.Empty;
                foreach(var r in employee.Roles)
                    role+=(r.Name+"\n");

                string autorithy = "";
                if (nomtek.IsEvaluationInProgress) {
                    autorithy = "\nAutorytet: " + nomtek.CurrentEvaluation.Nodes.Find(node => node.Employee == employee);
                }
                string text = "Nazwa: " + employee.Name + "\nData zatrudnienia " + employee.EmploymentDate + "\nRole: "+role + autorithy;
                EmployeesDetails.Text = text;
            }
        }

        private void NewEvaluation_Click(object sender, RoutedEventArgs e) {
            if (!evaluationInProgress) {
                nomtek.CreateNewEvaluation();
                NewEvaluation.Content = "       Zakończ ewaluację       ";
            } else {
                nomtek.CloseEvaluation(nomtek.CurrentEvaluation.Name);
                NewEvaluation.Content = "Rozpocznij nową ewaluację";
            }
            evaluationInProgress = !evaluationInProgress;
        }

        private void EmployeeSelected(object sender, SelectionChangedEventArgs e) {
            if (allowChangeSelectedValue) {
                if (((ListBox)sender).SelectedValue != null) {
                    EmployeeName.Text = ((ListBox)sender).SelectedValue.ToString();
                    //Show_EmployeesDetails();
                } else {
                    EmployeesDetails.Text = "";
                }
                //allowChangeText = false;
            } else allowChangeSelectedValue = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Odświeżenie kontrolek po wybraniu/wpisaniu nazwy pracownika
        /// </summary>
        private void RefreshControls() {
            var CBRolesList = CBRoles.ToList<CheckBox>();
            CBRolesList.RemoveAll(cb => cb == null);
            //CBRolesList.ForEach(Console.WriteLine);
            allowChangeSelectedValue = false;
            if (nomtek.Employees.Exists(em => em.Name.Equals(EmployeeName.Text))) {
                var currEmp = nomtek.Employees.Find(em => em.Name.Equals(EmployeeName.Text));
                CalendarE.SelectedDate = currEmp.EmploymentDate;
                AddEditButton.Content = "EDYTUJ PRACOWNIKA";
                CBRolesList.ForEach(cb => cb.IsChecked = false);
                foreach (Role role in currEmp.Roles) {
                    CBRolesList.Find(cb => cb.Content.ToString().Equals(role.Name)).IsChecked = true;
                }
                //allowChangeSelectedValue = false;
                ListOfEmployees2.SelectedValue = currEmp.Name;
                Show_EmployeesDetails();
                //CBRolesList.ForEach(cb => Console.WriteLine(cb.Content));
            } else {
                AddEditButton.Content = "DODAJ PRACOWNIKA";
                CBRolesList.ForEach(cb => cb.IsChecked = false);
                CalendarE.SelectedDate = null;
                //allowChangeSelectedValue = false;
                ListOfEmployees2.SelectedValue = null;
                EmployeesDetails.Text = "";
            }
            allowChangeSelectedValue = true;
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

        private void NewNeighbour(object sender, RoutedEventArgs e) {
            double value = 0;
            double time = 0;
            if (CBEmployees.SelectedIndex != -1 && ListOfEmployees1.SelectedIndex != -1 &&
                Double.TryParse(ValueForCompany.Text, out value) && Double.TryParse(TimeWorked.Text, out time)) {
                Node employee = nomtek.CurrentEvaluation.Nodes.Find(n => n.Employee.Name == CBEmployees.SelectedValue.ToString());
                Node next = nomtek.CurrentEvaluation.Nodes.Find(n => n.Employee.Name == ListOfEmployees1.SelectedValue.ToString());
                if (employee != next) {
                    Neighbour n = new Neighbour(employee, next, value, time);
                    employee.AddNeighbour(n);
                    next.AddNeighbour(n);
                    MessageBox.Show("Oceniono pracownika");
                }
            }
        }


      
        
    }
}
