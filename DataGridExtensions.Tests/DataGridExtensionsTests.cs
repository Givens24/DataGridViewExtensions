using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System.Data;
using DataGridViewExtensions;
using System.Linq;
using System.Drawing;
using DataGridExtensions.Tests.Models;
using System.Collections.Generic;
using System;

namespace DataGridExtensions.Tests
{
    [TestClass]
    public class DataGridExtensionsTests
    {
        private DataGridView _dataGridView;

        [TestInitialize]
        public void Setup()
        {
            _dataGridView = new DataGridView();
            var dataTable = SetupDataTable();
            AddDataTableToDataGrid(dataTable);
        }

        [TestMethod]
        public void FindRows_Successfully_Find_Data_Rows()
        {
            var dataRowsSearched = _dataGridView.FindRows(x => x.Value.ToString() == "Grant");

            Assert.IsTrue(dataRowsSearched.Count() == 1);
        }

        [TestMethod]
        public void UpdateCells_Successfully_Update_Cells_In_Data_Grid()
        {
            _dataGridView.UpdateCells(x => x.Value.ToString() == "test@email.com", "ggtgivens24@gmail.com");
            var dataRowsSearched = _dataGridView.FindRows(x => x.Value.ToString() == "ggtgivens24@gmail.com");

            Assert.IsTrue(dataRowsSearched.Count() == 1);
        }

        [TestMethod]
        public void RemoveRows_Successfully_Remove_Rows_By_Cell_Value()
        {
            _dataGridView.RemoveRows(x => x.Value.ToString() == "test@email.com");
            var dataRowsSearched = _dataGridView.FindRows(x => x.Value.ToString() == "ggtgivens24@gmail.com");

            Assert.IsTrue(!dataRowsSearched.Any());
        }

        [TestMethod]
        public void FormtCells_Successfully_Format_Cells_In_Data_Grid()
        {
            _dataGridView.FormatCells(x => x.Value.ToString() == "test@email.com", Color.White, Color.Red);
            var dataRowsSearched = _dataGridView.FindRows(x => x.Value.ToString() == "test@email.com");

            var testEmailCellRow = dataRowsSearched.FirstOrDefault();

            Assert.IsTrue(testEmailCellRow != null &&
                testEmailCellRow.Cells != null &&
                testEmailCellRow.Cells[3] != null &&
                testEmailCellRow.Cells[3].Style.BackColor == Color.Red &&
                testEmailCellRow.Cells[3].Style.ForeColor == Color.White);
        }

        [TestMethod]
        public void BindData_Successfully_Bind_Generic_Data_To_Data_Grid()
        {
            var contactInfo = SetupContactInfo();
            var dataGrid = new DataGridView();
            dataGrid.BindData(contactInfo);
            var dataRowsSearched = dataGrid.FindRows(x => x.Value.ToString() == "(952) 564-1170");

            Assert.IsTrue(dataRowsSearched.Count() == 1);
        }

        private DataTable SetupDataTable()
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("First Name");
            dataTable.Columns.Add("Last Name");
            dataTable.Columns.Add("Phone Number");
            dataTable.Columns.Add("Email");

            var firstDataRow = dataTable.NewRow();
            firstDataRow[0] = "Grant";
            firstDataRow[1] = "Taylor";
            firstDataRow[2] = "(763) 413-6221";
            firstDataRow[3] = "test@email.com";

            var secondDataRow = dataTable.NewRow();
            secondDataRow[0] = "John";
            secondDataRow[1] = "Doe";
            secondDataRow[2] = "(952) 890-0215";
            secondDataRow[3] = "test24@email.com";

            dataTable.Rows.Add(firstDataRow);
            dataTable.Rows.Add(secondDataRow);

            return dataTable;
        }

        private void AddDataTableToDataGrid(DataTable dataTable)
        {
            dataTable.Columns.Cast<DataColumn>().ToList().ForEach(x =>
            {
                _dataGridView.Columns.Add(x.ColumnName, x.ColumnName);
            });

            _dataGridView.Rows.Add(dataTable.Rows.Count);
            var rowIndex = 0;
            dataTable.Rows.Cast<DataRow>().ToList().ForEach(x =>
            {
                var columnIndex = 0;
                x.ItemArray.ToList().ForEach(cell =>
                {
                    _dataGridView.Rows[rowIndex].Cells[columnIndex].Value = x[columnIndex].ToString();
                    columnIndex++;
                });

                rowIndex++;
            });
        }

        private IEnumerable<ContactInfoModel> SetupContactInfo()
        {
            return new List<ContactInfoModel>
            {
                new ContactInfoModel
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Age = DateTime.Now.Year - new DateTime(1991, 9, 2).Year,
                    DateOfBirth = new DateTime(1991, 9, 2),
                    PhoneNumber = "(952) 564-1170",
                    Email = "test@email.com"
                },
                new ContactInfoModel
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Doe",
                    Age = DateTime.Now.Year - new DateTime(1990, 8, 5).Year,
                    DateOfBirth = new DateTime(1990, 8, 5),
                    PhoneNumber = "(952) 564-2173",
                    Email = "test24@email.com"
                }
            };
        }
    }
}
