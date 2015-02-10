using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.Collections.Generic;

using System.IO;
using System.Linq;

namespace slSecureLib
{
    public static class DataGridExtendUtility
    {
        public static ICollection<DataGridRow> GetRows(this DataGrid grid)
        {
            List<DataGridRow> rows = new List<DataGridRow>();
            foreach (var rowItem in grid.ItemsSource)
            {
                grid.ScrollIntoView(rowItem, grid.Columns.Last());
                FrameworkElement fel = grid.Columns.Last().GetCellContent(rowItem);
                DataGridRow row = DataGridRow.GetRowContainingElement(fel.Parent as FrameworkElement);
                if (row != null) rows.Add(row);
            }//
            return rows;
        }

        public static void ToCSV(this DataGrid grid)
        {
            var title = "";

            //DataGrid的title也要匯出
            foreach (var c in grid.Columns)
            {
                //2014特別處理:自訂欄位顯示才列出
                if (c.Visibility == Visibility.Visible)
                {
                    title += "\t" + c.Header.ToString();
                }
            }
            title = title.Remove(0, 1);


            string data = "" + title;
            //DataGrid的title也要匯出換行
            data += "\r\n";

            foreach (DataGridRow rowItem in grid.GetRows())
            {

                foreach (var c in grid.Columns)
                {
                    //2014特別處理:自訂欄位顯示才列出
                    if (c.Visibility == Visibility.Visible)
                    {
                        var res = "";
                        try
                        {
                            DataGridCell cell = c.GetCellContent(rowItem.DataContext).Parent as DataGridCell;
                            res = (cell.Content as TextBlock).Text;
                        }
                        catch
                        {
                            res = "";
                        }
                        data += res + "\t";
                    }
                }
                data += "\r\n";
            }

            SaveFileDialog sfd = new SaveFileDialog()
            {
                DefaultExt = "csv",
                Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*",
                FilterIndex = 1
            };

            if (sfd.ShowDialog() == true)
            {
                using (Stream stream = sfd.OpenFile())
                {
                    using (StreamWriter writer = new StreamWriter(stream, System.Text.UnicodeEncoding.Unicode))
                    {
                        writer.Write(data);
                        writer.Close();
                        MessageBox.Show("匯出Excel成功！");
                    }
                    stream.Close();
                }
            }
        }
    }
}
