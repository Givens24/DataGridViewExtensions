using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DataGridViewExtensions
{
    public static class DataGridExtensions
    {
        /// <summary>
        /// Retunrs a collection of data rows from a data grid view based on the specific data grid cell where clause.
        /// </summary>
        /// <param name="dataGridView">The data grid view being searched</param>
        /// <param name="whereExpression">Where clause used to search data rows by cell values</param>
        /// <returns>A collection of data grid rows</returns>
        public static IEnumerable<DataGridViewRow> FindRows(this DataGridView dataGridView, Func<DataGridViewCell, bool> whereExpression)
        {
            var dataRowsFound = new List<DataGridViewRow>();
            dataGridView.Rows.Cast<DataGridViewRow>().ToList().ForEach(x =>
            {
                if (x != null && x.Cells != null)
                {
                    var cells = x.Cells.Cast<DataGridViewCell>().Where(cell => cell.Value != null)
                                                                .Where(whereExpression);
                    if (cells.Any())
                    {
                        dataRowsFound.Add(x);
                    }
                }
            });

            return dataRowsFound;
        }

        /// <summary>
        /// Updates cells value in a grid based on a specific where clause and an update value.
        /// </summary>
        /// <param name="dataGridView">The data grid to update</param>
        /// <param name="whereExpression">Where clause used to search data cells</param>
        /// <param name="value">Update value for collection of cells</param>
        public static void UpdateCells(this DataGridView dataGridView, Func<DataGridViewCell, bool> whereExpression, object value)
        {
            dataGridView.Rows.Cast<DataGridViewRow>().ToList().ForEach(x =>
            {
                if (x != null && x.Cells != null)
                {
                    var cells = x.Cells.Cast<DataGridViewCell>().Where(cell => cell.Value != null)
                                                                .Where(whereExpression);
                    if (cells.Any())
                    {
                        cells.ToList().ForEach(cell =>
                        {
                            cell.Value = value;
                        });
                    }
                }
            });
        }

        /// <summary>
        /// Removes rows from a data grid based on a specific data grid cell where clause.
        /// </summary>
        /// <param name="dataGridView">The data grid to be updated</param>
        /// <param name="whereExpression">Where clause used to search data cells</param>
        public static void RemoveRows(this DataGridView dataGridView, Func<DataGridViewCell, bool> whereExpression)
        {
            var rowsFound = dataGridView.FindRows(whereExpression);
            if (rowsFound.Any())
            {
                rowsFound.ToList().ForEach(row =>
                {
                    dataGridView.Rows.RemoveAt(row.Index);
                });
            }
        }

        /// <summary>
        /// Sets the font and background color for a data grid's cells based on a specific where clause.
        /// </summary>
        /// <param name="dataGridView">The data grid to format</param>
        /// <param name="whereExpression">Where caluse used to locate the cells for formatting</param>
        /// <param name="foreColor">Font color for cells</param>
        /// <param name="backColor">Background color for cells</param>
        public static void FormatCells(this DataGridView dataGridView, Func<DataGridViewCell, bool> whereExpression, System.Drawing.Color foreColor, System.Drawing.Color backColor)
        {
            dataGridView.Rows.Cast<DataGridViewRow>().ToList().ForEach(x =>
            {
                if (x != null && x.Cells != null)
                {
                    var cells = x.Cells.Cast<DataGridViewCell>().Where(cell => cell.Value != null)
                                                                .Where(whereExpression);
                    if (cells.Any())
                    {
                        cells.ToList().ForEach(cell =>
                        {
                            cell.Style.ForeColor = foreColor;
                            cell.Style.BackColor = backColor;
                        });
                    }
                }
            });
        }

        /// <summary>
        /// Binds data of any class without nested collections to the data grid. If a class property has a description attribute, that will be used as the column header.
        /// </summary>
        /// <param name="dataGrid">The data grid to apply the data</param>
        /// <typeparam name="T">Type of the class to bind</typeparam>
        /// <param name="dataToBindToGrid">Collection of data to bind to the data grid</param>
        public static void BindData<T>(this DataGridView dataGridView, IEnumerable<T> dataToBindToGrid) where T : class
        {
            var properties = typeof(T).GetProperties().ToList();
            if (ClassHasNestedCollections(properties))
            {
                throw new InvalidOperationException("Class cannot have nested collections.");
            }

            var columns = new Dictionary<PropertyInfo, string>();
            properties.ForEach(p => columns.Add(p, (GetColumnName(p))));
            columns.ToList().ForEach(column => dataGridView.Columns.Add(column.Value, column.Key.Name));
            dataGridView.Rows.Add(dataToBindToGrid.Count());
            var rowIndex = 0;
            dataToBindToGrid.ToList().ForEach(data =>
            {
                var columnIndex = 0;
                properties.ForEach(prop =>
                {
                    dataGridView.Rows[rowIndex].Cells[columnIndex].Value = SetCellValue(prop, data);
                    columnIndex++;
                });
                rowIndex++;
            });
        }

        private static bool ClassHasNestedCollections(IEnumerable<PropertyInfo> properties)
        {
            return properties.ToList().Any(x => x.PropertyType != typeof(string) &&
                                                 typeof(IEnumerable).IsAssignableFrom(x.PropertyType) ||
                                                 typeof(IEnumerable<>).IsAssignableFrom(x.PropertyType));
        }

        private static object SetCellValue<T>(PropertyInfo property, T data) where T : class
        {
            var value = property.GetValue(data, null);
            return value is Guid ? value.ToString() : value;
        }

        private static string GetColumnName(PropertyInfo propertyInfo)
        {
            var descriptionAttribute = propertyInfo.GetCustomAttributes(typeof(DescriptionAttribute)).FirstOrDefault();
            if (descriptionAttribute == null)
            {
                return propertyInfo.Name;
            }
            var description = descriptionAttribute as DescriptionAttribute;
            return description == null ? propertyInfo.Name : description.Description;
        }
    }
}
