using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartForm
{
    public class DashboardGrid
    {
        public Tuple<List<int>, List<int>> FillGrid(Widget item, int rowCount, int colCount, bool[,] array)
        {
                List<int> rows = new List<int>();
                List<int> cols = new List<int>();
            try
            {
                MakeGridSuitable(item, rowCount, colCount, array);
                bool found = false;
                for (int i = 0; i < rowCount && !found; i++)
                {
                    for (int j = 0; j < colCount && !found; j++)
                    {
                        if (!array[i, j])
                        {
                            for (int k = 0; k < item.RowCount; k++)
                            {
                                for (int l = 0; l < item.ColumnCount; l++)
                                {
                                    if (i + k >= rowCount || j + l >= colCount)
                                    {
                                        found = true;
                                        break;
                                    }
                                    if (!array[i + k, j + l])
                                    {
                                        array[i + k, j + l] = true;                                        
                                        rows.Add(i + k);
                                        cols.Add(j + l);
                                    }

                                }
                            }
                            found = true;
                        }
                    }
                }
                if (!found)
                {
                    Console.WriteLine("Not Enough Space");
                }

                return Tuple.Create(rows, cols);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Tuple.Create(rows, cols);
            }

        }
        public void MakeGridSuitable(Widget item, int rowCount, int colCount, bool[,] array)
        {
            int firstEmptyPlace = array.Cast<bool>().ToList().IndexOf(false);
            int firstEmptyRow = firstEmptyPlace / colCount;
            int firstEmptyCol = firstEmptyPlace % colCount;

            if (item.ColumnCount + firstEmptyCol > colCount)
            {
                for (int i = firstEmptyCol; i < colCount; i++)
                {
                    array[firstEmptyRow, i] = true;
                }
                MakeGridSuitable(item, rowCount, colCount, array);
            }
        }
    }
}
