using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using COMP72070_Section3_Group1.Models;
//using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using String = System.String;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace ExcelHandler
{
    internal class ExcelHandler
    {
        //        //String docName = "test4.xlsx";


        //        //InsertText(docName, "TimeStamp", "A", 1);
        //        //InsertText(docName, "User", "B", 1);
        //        //InsertText(docName, "Action", "C", 1);

        //        //InsertText(docName, "5:03:00PM July 27th 2024", "A", 2);
        //        //InsertText(docName, "12:53:30AM April 16th 2021", "A", 3);
        //        //InsertText(docName, "Yao", "B", 2);
        //        //InsertText(docName, "Tried to hack into the system and failed", "C", 2);

        //        //InsertText(docName, GetNextEmptyCell(docName, "Sheet1", "A"), "D", 5);

        //        //Console.WriteLine(GetNextEmptyCell(docName, "Sheet1", "A"));
        //        //InsertText(docName, "today idk", "A", GetNextEmptyCell(docName, "Sheet1", "A"));
        //        //InsertText(docName, "yao", "B", GetNextEmptyCell(docName, "Sheet1", "B"));
        //        //InsertText(docName, "he tried to unionize", "C", GetNextEmptyCell(docName, "Sheet1", "C"));




        //        //CreateWorksheetForToday(docName);












        //static string CreateWorksheetForToday(string filename)
        //{
        //    using (SpreadsheetDocument document = SpreadsheetDocument.Open(filename, false))
        //    {
        //        WorkbookPart workbookPart = document.WorkbookPart;
        //        DateTime now = DateTime.Now;
        //        string name = now.ToString("dd/MM/yyyy");

        //        //if ()
        //        //{

        //        //}


        //        // Add a new worksheet part to the workbook.
        //        WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
        //        newWorksheetPart.Worksheet = new Worksheet(new SheetData());
        //        newWorksheetPart.Worksheet.Save();

        //        Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>() ?? workbookPart.Workbook.AppendChild(new Sheets());


        //        // Append the new worksheet and associate it with the workbook.
        //        Sheet sheet = new Sheet() { Name = name };
        //        sheets.Append(sheet);
        //        workbookPart.Workbook.Save();
        //    }
        //    return "test";
        //}





        static uint GetNextEmptyCell(string FileName, string sheetName, string col)
        {
            bool inLoop = true;
            uint rowIndex = 1;
            string cellReference;
            do
            {
                cellReference = col + rowIndex;
                if (GetCellValue(FileName, sheetName, cellReference) != string.Empty)
                {
                    rowIndex++;
                }
                else
                {
                    inLoop = false;
                }
            }
            while (inLoop);

            return rowIndex;
        }



        // Retrieve the value of a cell, given a file name, sheet name, 
        // and address name.
        static string GetCellValue(string fileName, string sheetName, string addressName)
        {
            string value = null;
            // Open the spreadsheet document for read-only access.
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(fileName, false))
            {
                // Retrieve a reference to the workbook part.
                WorkbookPart wbPart = document.WorkbookPart;
                // Find the sheet with the supplied name, and then use that 
                // Sheet object to retrieve a reference to the first worksheet.
                Sheet theSheet = wbPart?.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).FirstOrDefault();

                // Throw an exception if there is no sheet.
                if (theSheet is null || theSheet.Id is null)
                {
                    throw new ArgumentException("sheetName");
                }
                // Retrieve a reference to the worksheet part.
                WorksheetPart wsPart = (WorksheetPart)wbPart.GetPartById(theSheet.Id);
                // Use its Worksheet property to get a reference to the cell 
                // whose address matches the address you supplied.
                Cell theCell = wsPart.Worksheet?.Descendants<Cell>()?.Where(c => c.CellReference == addressName).FirstOrDefault();
                // If the cell does not exist, return an empty string.
                if (theCell is null || theCell.InnerText.Length < 0)
                {
                    return string.Empty;
                }
                value = theCell.InnerText;
                // If the cell represents an integer number, you are done. 
                // For dates, this code returns the serialized value that 
                // represents the date. The code handles strings and 
                // Booleans individually. For shared strings, the code 
                // looks up the corresponding value in the shared string 
                // table. For Booleans, the code converts the value into 
                // the words TRUE or FALSE.
                if (theCell.DataType is null)
                {
                    return value;
                }
                if (theCell.DataType.Value == CellValues.SharedString)
                {
                    // For shared strings, look up the value in the
                    // shared strings table.
                    var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                    // If the shared string table is missing, something 
                    // is wrong. Return the index that is in
                    // the cell. Otherwise, look up the correct text in 
                    // the table.
                    if (stringTable is null)
                    {
                    }
                    else
                    {
                        value = stringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
                    }
                }
                else if (theCell.DataType.Value == CellValues.Boolean)
                {
                    switch (value)
                    {
                        case "0":
                            value = "FALSE";
                            break;
                        default:
                            value = "TRUE";
                            break;
                    }
                }

                return value;
            }
        }
    }
}













//    // Given a document name and text, 
//    // inserts a new work sheet and writes the text to cell "A1" of the new worksheet.
//    static void InsertText(string docName, string text, string col, uint row)
//    {
//        // Open the document for editing.
//        using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(docName, true))
//        {
//            WorkbookPart workbookPart = spreadSheet.WorkbookPart ?? spreadSheet.AddWorkbookPart();

//            // Get the SharedStringTablePart. If it does not exist, create a new one.
//            SharedStringTablePart shareStringPart;
//            if (workbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
//            {
//                shareStringPart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
//            }
//            else
//            {
//                shareStringPart = workbookPart.AddNewPart<SharedStringTablePart>();
//            }

//            // Insert the text into the SharedStringTablePart.
//            int index = InsertSharedStringItem(text, shareStringPart);

//            //// Insert a new worksheet.
//            //WorksheetPart worksheetPart = InsertWorksheet(workbookPart);

//            // Insert cell A1 into the new worksheet.
//            Cell cell = InsertCellInWorksheet(col, row, workbookPart.WorksheetParts.First());

//            // Set the value of cell A1.
//            cell.CellValue = new CellValue(index.ToString());
//            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

//            // Save the new worksheet.
//            workbookPart.WorksheetParts.First().Worksheet.Save();
//        }
//    }

//    // Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text 
//    // and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
//    static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
//    {
//        // If the part does not contain a SharedStringTable, create one.
//        if (shareStringPart.SharedStringTable is null)
//        {
//            shareStringPart.SharedStringTable = new SharedStringTable();
//        }

//        int i = 0;

//        // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
//        foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
//        {
//            if (item.InnerText == text)
//            {
//                return i;
//            }

//            i++;
//        }

//        // The text does not exist in the part. Create the SharedStringItem and return its index.
//        shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
//        shareStringPart.SharedStringTable.Save();

//        return i;
//    }

//    // Given a WorkbookPart, inserts a new worksheet.
//    static WorksheetPart InsertWorksheet(WorkbookPart workbookPart)
//    {
//        // Add a new worksheet part to the workbook.
//        WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
//        newWorksheetPart.Worksheet = new Worksheet(new SheetData());
//        newWorksheetPart.Worksheet.Save();

//        Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>() ?? workbookPart.Workbook.AppendChild(new Sheets());
//        string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

//        // Get a unique ID for the new sheet.
//        uint sheetId = 1;
//        if (sheets.Elements<Sheet>().Count() > 0)
//        {
//            sheetId = sheets.Elements<Sheet>().Select<Sheet, uint>(s =>
//            {
//                if (s.SheetId is not null && s.SheetId.HasValue)
//                {
//                    return s.SheetId.Value;
//                }

//                return 0;
//            }).Max() + 1;
//        }

//        string sheetName = "Sheet" + sheetId;

//        // Append the new worksheet and associate it with the workbook.
//        Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
//        sheets.Append(sheet);
//        workbookPart.Workbook.Save();

//        return newWorksheetPart;
//    }


//    // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
//    // If the cell already exists, returns it. 
//    static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
//    {
//        Worksheet worksheet = worksheetPart.Worksheet;
//        SheetData? sheetData = worksheet.GetFirstChild<SheetData>();
//        string cellReference = columnName + rowIndex;

//        // If the worksheet does not contain a row with the specified row index, insert one.
//        Row row;

//        if (sheetData?.Elements<Row>().Where(r => r.RowIndex is not null && r.RowIndex == rowIndex).Count() != 0)
//        {
//            row = sheetData!.Elements<Row>().Where(r => r.RowIndex is not null && r.RowIndex == rowIndex).First();
//        }
//        else
//        {
//            row = new Row() { RowIndex = rowIndex };
//            sheetData.Append(row);
//        }

//        // If there is not a cell with the specified column name, insert one.  
//        if (row.Elements<Cell>().Where(c => c.CellReference is not null && c.CellReference.Value == columnName + rowIndex).Count() > 0)
//        {
//            return row.Elements<Cell>().Where(c => c.CellReference is not null && c.CellReference.Value == cellReference).First();
//        }
//        else
//        {
//            // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
//            Cell? refCell = null;

//            foreach (Cell cell in row.Elements<Cell>())
//            {
//                if (string.Compare(cell.CellReference?.Value, cellReference, true) > 0)
//                {
//                    refCell = cell;
//                    break;
//                }
//            }

//            Cell newCell = new Cell() { CellReference = cellReference };
//            row.InsertBefore(newCell, refCell);

//            worksheet.Save();
//            return newCell;
//        }
//    }
//}
//}
