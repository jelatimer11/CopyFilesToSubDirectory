using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Norama.CopyFilesToSubDirectory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ((INotifyCollectionChanged)lv_items.Items).CollectionChanged += ListView_CollectionChanged;
        }

        private void ListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // scroll the new item into view   
                lv_items.ScrollIntoView(e.NewItems[0]);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofd = new();
            ofd.Multiselect = false;
            ofd.ShowDialog();

            if (!string.IsNullOrEmpty(ofd.FileName))
            {
                tbx_cfile.Text = ofd.FileName;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new();
            fbd.ShowDialog();

            if (!string.IsNullOrEmpty(fbd.SelectedPath))
            {
                tbx_ddir.Text = fbd.SelectedPath;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            lv_items.Items.Clear();

            System.IO.SearchOption so = System.IO.SearchOption.TopDirectoryOnly;

            if ((bool)rb_all.IsChecked)
            {
                so = System.IO.SearchOption.AllDirectories;
            }

            if (System.IO.File.Exists(tbx_cfile.Text))
            {
                string fileName = System.IO.Path.GetFileName(tbx_cfile.Text);

                foreach (string dirName in System.IO.Directory.GetDirectories(tbx_ddir.Text, "*.*", so))
                {
                    string nfileLoc = System.IO.Path.Combine(dirName, fileName);

                    try
                    {
                        System.IO.File.Copy(tbx_cfile.Text, nfileLoc, (bool)ckb_owrite.IsChecked);

                        lv_items.Items.Add("Copied " + fileName + " to " + dirName);
                    }
                    catch
                    {
                        lv_items.Items.Add("Could not copy " + fileName + " to " + dirName);
                    }
                }
            }
            else
            {
                MessageBox.Show("The source file does not exist.");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
