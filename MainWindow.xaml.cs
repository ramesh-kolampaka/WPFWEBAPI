using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPFContacts.Services;
using WPFContacts.ViewModel;

namespace WPFContacts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ContactViewModel();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           var lhhist=  ((ListBox)sender).SelectedItems;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBoxName = (TextBox)sender;
            string filterText = textBoxName.Text;
            ICollectionView cv = CollectionViewSource.GetDefaultView(grdContacts.ItemsSource);

            if (!string.IsNullOrEmpty(filterText))
            {
                cv.Filter = o =>
                {
                    /* change to get data row value */
                    ContactModel p = o as ContactModel;
                    return (p.FirstName.ToUpper().StartsWith(filterText.ToUpper()));
                    /* end change to get data row value */
                };
            }
            else
            {
               grdContacts.ItemsSource= cv.SourceCollection;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {


                string filterText = txtSearch.Text;
            //ICollectionView cv = CollectionViewSource.GetDefaultView(grdContacts.ItemsSource);

            //if (!string.IsNullOrEmpty(filterText))
            //{
            //    cv.Filter = o =>
            //    {
            //        /* change to get data row value */
            //        ContactModel p = o as ContactModel;
            //        return (p.FirstName.ToUpper().StartsWith(filterText.ToUpper()));
            //        /* end change to get data row value */
            //    };
            //}
            //else
            //{
            //    grdContacts.ItemsSource = cv.SourceCollection;
            //}

            var colBind = ((DataGridTextColumn)grdContacts.Columns[0]).Binding as Binding;

            Func<object, Binding, object> getValue = (srcObj, bind) =>
            {
                var cntrl = new UserControl();
                cntrl.DataContext = srcObj;
                cntrl.SetBinding(UserControl.ContentProperty, bind);
                return cntrl.GetValue(UserControl.ContentProperty);
            };

            foreach (var data in grdContacts.Items)
            {
                var value = getValue(data, colBind);
                if (value.ToString().ToUpper() == filterText.ToUpper())
                {
                    grdContacts.SelectedItem = data;
                    grdContacts.ScrollIntoView(data);

                    break;
                }
            }

        }
    }
}
