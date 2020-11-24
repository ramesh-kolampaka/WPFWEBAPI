using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFContacts.Controls
{
    public class SearchableDataGrid : DataGrid
    {
        public string SearchTextBoxName { get; set; }
        public bool IsEnableSearch { get; set; }
        public System.Windows.Media.Brush SearchedValueBackGroundColor { get; set; }
        public System.Windows.Media.Brush SearchedValueForeGroundColor { get; set; }
        private TextBox SearchTextBox { get; set; }
        // Dictionary<string, object> properties = new Dictionary<string,object>();

        //public object this[string name]
        //{
        //    get
        //    {
        //        if (properties.ContainsKey(name))
        //        {
        //            return properties[name];
        //        }
        //        return null;
        //    }
        //    set
        //    {
        //        properties[name] = value;
        //    }
        //}
        public SearchableDataGrid()
        {
            this.IsEnableSearch = false;
            this.SearchedValueBackGroundColor = System.Windows.Media.Brushes.Yellow;
            this.SearchedValueForeGroundColor = System.Windows.Media.Brushes.Gray;

        }
        private void SearchTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            TextBox TB = (TextBox)sender;
            foreach (DataGridColumn DGC in this.Columns)
            {
                for (int ind = 0; ind < this.Items.Count; ind++)
                {

                    FrameworkElement FE = DGC.GetCellContent(this.Items[ind]);
                    if (FE != null && FE.GetType().Name == "TextBlock")
                    {
                        TextBlock TX = (TextBlock)FE;
                        if (TX != null)
                        {
                            if (!string.IsNullOrEmpty(TB.Text) && TX.Text.ToUpper().Contains(TB.Text.ToUpper()))
                            {
                                System.Windows.Media.BrushConverter BC = new System.Windows.Media.BrushConverter();
                                TX.Background = this.SearchedValueBackGroundColor;
                                //TX.Background =  System.Windows.Media.Brushes.Orange;
                                TX.Foreground = System.Windows.Media.Brushes.YellowGreen;
                            }
                            else
                            {
                                TX.Background = System.Windows.Media.Brushes.White;
                                TX.Foreground = System.Windows.Media.Brushes.Black;
                            }
                        }
                    }
                }
            }
        }
        protected override void OnItemsSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            //code searchs the textbox control that exists in the parent of datagrid 
            if (this.IsEnableSearch == true && !string.IsNullOrEmpty(this.SearchTextBoxName))
            {
                foreach (TextBox tb in FindVisualChildren<TextBox>(this.Parent))
                {
                    if (this.SearchTextBoxName == tb.Name)
                    {
                        this.SearchTextBox = tb;
                        this.SearchTextBox.TextChanged += new TextChangedEventHandler(SearchTextBox_TextChanged);
                    }
                }
            }

        }
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject ParentControl) where T : DependencyObject
        {
            if (ParentControl != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(ParentControl); i++)
                {
                    DependencyObject ChildControl = VisualTreeHelper.GetChild(ParentControl, i);
                    if (ChildControl != null && ChildControl is T)
                    {
                        yield return (T)ChildControl;
                    }

                    foreach (T ChildOfChildControl in FindVisualChildren<T>(ChildControl))
                    {
                        yield return ChildOfChildControl;
                    }
                }
            }
        }
    }
}
