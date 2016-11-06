# DataGridViewAssistant
A library of extensions for Windows forms data grid views

This library is used for performing searches, updates, formatting and generic data binding for **Windows Forms** **DataGridViews**

The following functions are included in the library. **FindRows**, **UpdateCells**, **RemoveRows**, **FormatCells**, **BindData**

## FindRows
**FindRows** takes a ```Func<DataGridViewCell, bool>``` parameter in order to find rows based on a specific data grid cells.
Example:
```C#
var dataRowsSearched = _dataGridView.FindRows(x => x.Value.ToString() == "Grant");
```

## UpdateCells
**UpdateCells** takes a ```Func<DataGridViewCell, bool>``` parameter for searching specific cells to update and a value for updating the searched cells.
Example: 
```C#
_dataGridView.UpdateCells(x => x.Value.ToString() == "test@email.com", "ggtgivens24@gmail.com");
```

## RemoveRows
**RemoveRows** takes a ```Func<DataGridViewCell, bool>``` parameter for searching specific cells and removing the rows containing those cells.
Example:
```C#
_dataGridView.RemoveRows(x => x.Value.ToString() == "test@email.com");
```

## FormatCells
**FormatCells** takes a ```Func<DataGridViewCell, bool>``` parameter for searching specific cells and two additional parameters for formatting the cells font and background color. These parameters are **System.Drawing.Color** enums.
Example:
```C#
_dataGridView.FormatCells(x => x.Value.ToString() == "test@email.com", Color.White, Color.Red);
```

## BindData
**BindData** takes an ```IEnumerable<T>``` with T being any .Net class without nested collections and builds the data grid from that collection. The property names of the class become the column names. If there are description attributes on the properties, those will be used as the column header's.
Example:
```C#
var contactInfo = new List<ContactInfoModel>
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

var dataGrid = new DataGridView();
dataGrid.BindData(contactInfo);
```

