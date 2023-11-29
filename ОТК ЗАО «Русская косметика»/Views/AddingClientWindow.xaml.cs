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
using System.Windows.Shapes;

namespace ОТК_ЗАО__Русская_косметика_.Views
{
    /// <summary>
    /// Логика взаимодействия для AddingClientWindow.xaml
    /// </summary>
    public partial class AddingClientWindow : Window
    {
        public AddingClientWindow()
        {
            InitializeComponent();
        }

        private void AddingClientClick(object sender, RoutedEventArgs e)
        {
            if (typeClient.SelectedIndex == 0) AddPerson();
            else if (typeClient.SelectedIndex == 1) AddCompany();
            else
            {
                MessageBox.Show("Выберите вид клиента!");
                return;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы точно хотите вернуться?\nДанные НЕ будут сохранены", "Внимание",
              MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        public void AddPerson()
        {
            if (string.IsNullOrEmpty(PersonFIO.Text))
            {
                MessageBox.Show("Введите ФИО клиента!");
                return;
            }
            if (string.IsNullOrEmpty(PassportSeries.Text) || string.IsNullOrEmpty(PassportSeries.Text))
            {
                MessageBox.Show("Введите серию и номер паспорта!");
                return;
            }
            if (string.IsNullOrEmpty(PersonMail.Text))
            {
                MessageBox.Show("Введите почту клиента!");
                return;
            }
            if (string.IsNullOrEmpty(PersonPhone.Text))
            {
                MessageBox.Show("Введите номер телефона клиента!");
                return;
            }

            if (PersonBirthday is null)
            {
                MessageBox.Show("Выберит дату рождения клиента!");
                return;
            }

            PersonClients person = new PersonClients();

            person.PersonFIO = PersonFIO.Text;
            person.Birthday = (DateTime)PersonBirthday.SelectedDate;
            person.PassportData = "Серия " + PassportSeries.Text + " номер " + PassportNumber.Text;
            person.Email = PersonMail.Text;
            person.Phone = PersonPhone.Text;

            if (person == Entities.GetContext().PersonClients.Where(x=>x.PersonID == person.PersonID))
            {
                MessageBox.Show("Такой клиент уже есть в базе данных!");
                return;
            }
            else
            {
                Entities.GetContext().PersonClients.Add(person);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Клиент успешно добавлен!");
                this.Close();
            }
        }

        public void AddCompany()
        {
            if (string.IsNullOrEmpty(CompanyName.Text))
            {
                MessageBox.Show("Введите название компании!");
                return;
            }
            if (string.IsNullOrEmpty(CompanyAddress.Text))
            {
                MessageBox.Show("Введите адрес компании!");
                return;
            }
            if (string.IsNullOrEmpty(CompanyINN.Text))
            {
                MessageBox.Show("Введите ИНН компании!");
                return;
            }
            if (string.IsNullOrEmpty(CompanyRS.Text))
            {
                MessageBox.Show("Введите р/с компании!");
                return;
            }
            if (string.IsNullOrEmpty(CompanyBIK.Text))
            {
                MessageBox.Show("Введите БИК компании!");
                return;
            }
            if (string.IsNullOrEmpty(CompanyDirector.Text))
            {
                MessageBox.Show("Введите ФИО руководителя!");
                return;
            }
            if (string.IsNullOrEmpty(CompanyContact.Text))
            {
                MessageBox.Show("Введите ФИО контактного лица!");
                return;
            }
            if (string.IsNullOrEmpty(ContactPhone.Text))
            {
                MessageBox.Show("Введите контактный номер телефона!");
                return;
            }

            CompanyClients companyLast = Entities.GetContext().CompanyClients.Last();
            CompanyClients company = new CompanyClients();

            company.CompanyID = companyLast.CompanyID+1;
            company.CompanyName = CompanyName.Text;
            company.Address = CompanyAddress.Text;
            company.INN = CompanyINN.Text;
            company.R_S = CompanyRS.Text;
            company.BIK = CompanyBIK.Text;
            company.DirectorFIO = CompanyDirector.Text;
            company.ContactFIO = CompanyContact.Text;
            company.Email = CompanyMail.Text;
            company.ContactPhone = ContactPhone.Text;

            if (company == Entities.GetContext().CompanyClients.Where(x=>x.CompanyID == company.CompanyID).FirstOrDefault())
            {
                MessageBox.Show("Такой клиент уже есть в базе данных!");
                return;
            }
            else
            {
                Entities.GetContext().CompanyClients.Add(company);
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Клиент успешно добавлен!");
                this.Close();
            }
        }

        private void typeClientSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(typeClient.SelectedIndex == 0)
            {
                CompanyInfo.Visibility = Visibility.Collapsed;
                PersonInfo.Visibility = Visibility.Visible;
            }

            if (typeClient.SelectedIndex == 1)
            {
                PersonInfo.Visibility = Visibility.Collapsed;
                CompanyInfo.Visibility = Visibility.Visible;
            }
        }

        //private void WindowToolTipClosing(object sender, ToolTipEventArgs e)
        //{
        //    if (MessageBox.Show($"Вы точно хотите вернуться?\nДанные НЕ будут сохранены", "Внимание",
        //      MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        //    {
        //        this.Close();
        //    }
        //}
    }
}
