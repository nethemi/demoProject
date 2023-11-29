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
using ОТК_ЗАО__Русская_косметика_.Classes;

namespace ОТК_ЗАО__Русская_косметика_.Views
{
    /// <summary>
    /// Логика взаимодействия для OrderFormationWindow.xaml
    /// </summary>
    public partial class OrderFormationWindow : Window
    {
        Workers worker = new Workers();
        List <Services> servicesList = new List<Services>();
        Services service = new Services();
        int totalPrice = 0;

        public OrderFormationWindow()
        {
            InitializeComponent();

            int lasId = Entities.GetContext().Orders.Count();
            OrderID.Text = Convert.ToString(lasId + 1);

            WorkerName.ItemsSource = Entities.GetContext().Workers.ToList();
            ServiceName.ItemsSource = Entities.GetContext().Services.ToList();
            
        }

        private void addServiceClick(object sender, RoutedEventArgs e)
        {
            servicesList.Add(service);
            serviceListView.Items.Add(service);
            totalPrice += service.Price;
            OrderPrice.Text = totalPrice.ToString()+" руб.";

        }

        private void OrderFormingClick(object sender, RoutedEventArgs e)
        {
            if (typeClient.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите вид клиента!");
                return;
            }
            if (string.IsNullOrEmpty(OrderID.Text))
            {
                MessageBox.Show("Введите код лабораторного сосуда!");
                return;
            }


            Orders order = new Orders();
            if (typeClient.SelectedIndex == 0)
            {
                PersonClients person = Entities.GetContext().PersonClients.Where(x => x.PersonFIO == PersonFIO.Text).FirstOrDefault();
                if (person == null)
                {
                    if (MessageBox.Show($"Такого клиента нет в базе данных\nДобавить нового клиента?", "Внимание",
                  MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        AddingClientWindow addingClient = new AddingClientWindow();
                        addingClient.ShowDialog();
                    }
                    return;
                }
                else order.PersonFK = person.PersonID;
            }
            if (typeClient.SelectedIndex == 1)
            {
                CompanyClients company = Entities.GetContext().CompanyClients.Where(x => x.CompanyName == CompanyName.Text).FirstOrDefault();
                if (company == null)
                {
                    if (MessageBox.Show($"Такой компании нет в базе данных\nДобавить нового клиента?", "Внимание",
                  MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        AddingClientWindow addingClient = new AddingClientWindow();
                        addingClient.ShowDialog();
                    }
                    return;
                }
                else order.CompanyFK = company.CompanyID;
            }


            order.OrderID = int.Parse(OrderID.Text);
            order.WorkerFK = worker.WorkerID;

            if (order == Entities.GetContext().Orders.Where(x=>x.OrderID == order.OrderID).FirstOrDefault())
            {
                MessageBox.Show("Такой заказ уже есть в базе данных!");
                return;
            }

            Entities.GetContext().Orders.Add(order);
            Entities.GetContext().SaveChanges();

            foreach (Services service in serviceListView.Items)
            {
                ServiceInOrder serviceInOrder = new ServiceInOrder();
                serviceInOrder.ServiceFK = service.ServiceID;
                serviceInOrder.OrderFK = order.OrderID;
                Entities.GetContext().ServiceInOrder.Add(serviceInOrder);
                Entities.GetContext().SaveChanges();
            }

            MessageBox.Show("Заказ успешно добавлен!");
            OrderFormationWindow orderFormation = new OrderFormationWindow();
            orderFormation.Show();
            this.Close();
        }

        private void typeClientSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (typeClient.SelectedIndex == 0)
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

        private void WorkerNameSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            worker = WorkerName.SelectedItem as Workers;
            CheckingWorker checking = new CheckingWorker();

            bool flag = checking.CheckWorker((int)worker.PostFK);
            if (!flag)
            {
                MessageBox.Show("Добавление сотрудника отклонено!");
                WorkerName.SelectedIndex = 3;
                return;
            }
        }

        private void ServiceNameSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            service = ServiceName.SelectedItem as Services;
        }

        private void WindowToolTipClosing(object sender, ToolTipEventArgs e)
        {
            if (MessageBox.Show($"Вы точно хотите выйти?", "Внимание",
              MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
