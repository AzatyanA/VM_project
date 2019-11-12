using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;


namespace Wpf_CoffeeMachine
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WalletContext db; VM_WalletContext db1; MenuContext db2; int sum; int[] operations;
        public MainWindow()
        {
            InitializeComponent();
            operations = new int[4];
            db = new WalletContext();
            db1 = new VM_WalletContext();
            db2 = new MenuContext();
            db.Wallet.Load();
            MainDataGrid.ItemsSource = db.Wallet.Local.ToBindingList();
            db1.VM_Wallet.Load();
            VMDataGrid.ItemsSource = db1.VM_Wallet.Local.ToBindingList();
            db2.Menu.Load();
            MenuDataGrid.ItemsSource = db2.Menu.Local.ToBindingList();
            sum = 0;
            sum_txtbox.Text = "0";
        }
        //private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        //{
        //    Regex regex = new Regex("[^0-9]+");
        //    e.Handled = regex.IsMatch(e.Text);
        //}
        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MainDataGrid.SelectedItem != null)
                {
                    Wallet_Class w = MainDataGrid.SelectedItems[0] as Wallet_Class;
                    if (w != null)
                    {
                        if (w.Quantity < 1)
                            MessageBox.Show("Нет монет!");
                        else
                        {
                            w.Quantity = w.Quantity - 1;
                            if (w.Rating_Value == 10) { operations[3]++; }
                            if (w.Rating_Value == 5) { operations[2]++; }
                            if (w.Rating_Value == 2) { operations[1]++; }
                            if (w.Rating_Value == 1) { operations[0]++; }
                            // var VM = db1.VM_Wallet.Where(p => p.Rating_Value == w.Rating_Value).FirstOrDefault();
                            sum = sum + w.Rating_Value;
                            update_view();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void btn_Buy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MenuDataGrid.SelectedItem != null)
                {
                    Menu_Class m = MenuDataGrid.SelectedItems[0] as Menu_Class;
                    if (m != null)
                    {
                        if (m.Quantity < 1)
                            MessageBox.Show("Данного товара нет в наличии! Извините!");
                        else
                        {
                            if (m.Price <= sum)
                            {
                                List<VM_Wallet_Class> vm_w = db1.VM_Wallet
                                               .ToList();
                                m.Quantity = m.Quantity - 1;
                                sum = sum - m.Price;
                                for (int i = 0; i < 4; i++)
                                {
                                    vm_w[i].Quantity = vm_w[i].Quantity + operations[i];
                                    operations[i] = 0;
                                }
                                int sum_copy = sum;
                                for (int n = 3; n >= 0; n--)
                                {
                                    int a;
                                    if ((a = (int)(sum_copy / vm_w[n].Rating_Value)) > 0)
                                    {
                                        if (a <= vm_w[n].Quantity)
                                        {
                                            sum_copy = sum_copy - a * vm_w[n].Rating_Value;
                                            vm_w[n].Quantity = vm_w[n].Quantity - a;
                                            operations[n] = operations[n] + a;
                                        }
                                        else
                                        {
                                            sum_copy = sum_copy - vm_w[n].Quantity * vm_w[n].Rating_Value;
                                            operations[n] = operations[n] + vm_w[n].Quantity;
                                            vm_w[n].Quantity = 0;
                                        }
                                    }
                                    if (sum_copy == 0) break;
                                }
                                update_view();
                                give_order();
                                MessageBox.Show("Спасибо!");
                            }
                            else
                            {
                                MessageBox.Show("Недостаточно средств!");
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Return_money_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Wallet_Class> w = db.Wallet
                                               .ToList();
                List<VM_Wallet_Class> vm_w = db1.VM_Wallet
                                               .ToList();
                for (int i = 0; i < 4; i++)
                {
                    vm_w[i].Quantity = vm_w[i].Quantity + operations[i];
                    operations[i] = 0;
                }
                for (int n = 3; n >= 0; n--)
                {
                    int a;
                    if ((a = (int)(sum / vm_w[n].Rating_Value)) > 0)
                    {
                        if (a <= vm_w[n].Quantity)
                        {
                            sum = sum - a * vm_w[n].Rating_Value;
                            vm_w[n].Quantity = vm_w[n].Quantity - a;
                            w[n].Quantity = w[n].Quantity + a;
                        }
                        else
                        {
                            sum = sum - vm_w[n].Quantity * vm_w[n].Rating_Value;
                            w[n].Quantity = w[n].Quantity + vm_w[n].Quantity;
                            vm_w[n].Quantity = 0;
                        }
                    }
                    if (sum == 0) break;
                }
                update_view();
                for (int i = 0; i < 4; operations[i++] = 0) { }
                //MessageBox.Show(operations[0].ToString()+ operations[1]+ operations[2]+ operations[3]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClosingMainWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void transfer_balance_to_VM()
        {

        }
        private void give_order()
        {
            MessageBox.Show("Товар выдан");
        }
        private void update_view()
        {
            db.SaveChanges();
            db1.SaveChanges();
            db2.SaveChanges();
            MainDataGrid.ItemsSource = null;
            VMDataGrid.ItemsSource = null;
            MenuDataGrid.ItemsSource = null;
            MainDataGrid.ItemsSource = db.Wallet.Local.ToBindingList();
            VMDataGrid.ItemsSource = db1.VM_Wallet.Local.ToBindingList();
            MenuDataGrid.ItemsSource = db2.Menu.Local.ToBindingList();
            sum_txtbox.Text = sum.ToString();
        }
    }
}
