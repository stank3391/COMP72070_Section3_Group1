using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using COMP72070_Section3_Group1.Models;
using COMP72070_Section3_Group1.Visitors;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using excel;

namespace TcpServer
{
    public class TcpServer
    {
        // networking props
        private TcpListener listener;
        private int port = 27000;
        private IPAddress localAddress = IPAddress.Any;
        private TcpClient tcpClient;
        private NetworkStream stream;

        private bool isStop = false; // flag to stop the server

        // database props
        private List<Post> posts = new List<Post>(); // list of posts from the database



        /// <summary>
        /// Constructor for the TcpServer class
        /// </summary>
        public TcpServer()
        {
            this.listener = new TcpListener(this.localAddress, this.port);
        }

        /// <summary>
        /// Listens and connects to clients. 
        /// load the posts from the database
        /// </summary>
        public void Start()
        {
            InsertText("test.xlsx", "Timestamp", "A", 1);
            InsertText("test.xlsx", "User", "B", 1);
            InsertText("test.xlsx", "Action", "C", 1);
            Console.WriteLine("Server started.");
            while (!this.isStop)
            {
                Console.WriteLine("Listenting.");
                this.listener.Start();

                this.tcpClient = this.listener.AcceptTcpClient();
                Console.WriteLine("Connected.");

                this.stream = this.tcpClient.GetStream();

                HandleClient();
            }
        }

        /// <summary>
        /// MAIN FUNCTION TO COMMUNICATE WITH CLIENT!
        /// </summary>
        private void HandleClient()
        {

            byte[] bufferIn = new byte[1024]; // buffer for incoming data
            byte[] bufferOut = new byte[1024]; // buffer for outgoing data
            string data = string.Empty; // parsed data from buffer

            bool isDisconnect = false;

            while (!isDisconnect)
            {
                // receive data 
                this.stream.Read(bufferIn, 0, bufferIn.Length);
                Packet packetIn = Packet.DeserializePacket(bufferIn);
                HandlePacket(packetIn);

                if (!tcpClient.Connected)
                {
                    Console.WriteLine($"Disconnected.");
                    tcpClient.Close();
                }
            }

        }

        /// <summary>
        /// Handles the packet received from the client
        /// determines the type of packet and do action
        /// </summary>
        private void HandlePacket(Packet packet)
        {
            Console.WriteLine($"Packet received:\n{packet.ToString()}");
            Packet.Type type = packet.header.messageType;

            //InsertText("test.xlsx", "test", "E", 7);


            switch (type)
            {
                case Packet.Type.Ack:
                    Console.WriteLine("Ack received"); 
                    CreateLog("test.xlsx", "yao", "Acknowledgement Signal Recieved");
                    break;
                case Packet.Type.Error:
                    Console.WriteLine("Error received");
                    CreateLog("test.xlsx", "yao", "Error Signal Recieved");
                    break;
                case Packet.Type.Ready:
                    Console.WriteLine("Ready received");
                    CreateLog("test.xlsx", "yao", "Ready Signal Recieved");
                    HandleReadyPacket();
                    break;
                case Packet.Type.Auth:
                    Console.WriteLine("Auth received");
                    CreateLog("test.xlsx", "yao", "Authorization Signal Recieved");
                    break;
                default:
                    Console.WriteLine("Unknown packet type received");
                    CreateLog("test.xlsx", "yao", "Unknown Signal Recieved");
                    break;
            }
        }

        /// <summary>
        /// Handles the ready packet
        /// 1. send all posts to the client
        /// 2. checks if source is authenticated
        /// -- if not, do not send messages
        /// -- if yes, send messages too
        /// </summary>
        private void HandleReadyPacket()
        {
            // send ack packet with the total number of posts as the body
            int postCount = posts.Count;
            byte[] body = Encoding.ASCII.GetBytes(postCount.ToString());
            Packet packet = new Packet("SERVER", Packet.Type.Ack, false, body);
            byte[] serializedPacket = Packet.SerializePacket(packet);
            stream.Write(serializedPacket, 0, serializedPacket.Length);

            // then we start blasting
            for (int i = 0; i < postCount; i++)
            {
                Console.WriteLine($"Sending post {i + 1} of {postCount}");
                body = posts[i].ToByte();
                packet = new Packet("SERVER", Packet.Type.Post, false, body);
                serializedPacket = Packet.SerializePacket(packet);
                stream.Write(serializedPacket, 0, serializedPacket.Length);

                // wait for 10 ms
                System.Threading.Thread.Sleep(100); // MUST BE HERE OR ELSE ASP BREAKS (wont recevie all packets)
            }
        }

        /// <summary>
        /// Fetches all the posts from the database and stores them in 'posts' property
        /// </summary>
        public void UpdatePosts()
        {
            // update the posts from the database

            // just return some dummy posts for now
            posts.Add(new Post(1, "HEELOO!!! I am a post1", "user1", DateTime.Now));

            posts.Add(new Post(2, "HEELOO!!! I am a post2", "user2", DateTime.Now));

            posts.Add(new Post(3, "HEELOO!!! I am a post3", "user3", DateTime.Now));

            posts.Add(new Post(4, "HEELOO!!! I am a post4", "user4", DateTime.Now));

            posts.Add(new Post(5, "HEELOO!!! I am a post5", "user5", DateTime.Now));

            Console.WriteLine("Posts list updated from 'Database'.");

        }






        static void CreateLog(string filename, string username, string action)
        {
            DateTime now = DateTime.Now;
            //string name = now.ToString("dd/MM/yyyy");
            string time = now.ToString("h:mm:ss tt");
            InsertText(filename, time, "A", GetNextEmptyCell(filename, "Sheet1", "A"));
            InsertText(filename, username, "B", GetNextEmptyCell(filename, "Sheet1", "B"));
            InsertText(filename, action, "C", GetNextEmptyCell(filename, "Sheet1", "C"));
        }



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



        static void InsertText(string docName, string text, string col, uint row)
        {
            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(docName, true))
            {
                WorkbookPart workbookPart = spreadSheet.WorkbookPart ?? spreadSheet.AddWorkbookPart();
                SharedStringTablePart shareStringPart;
                if (workbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                {
                    shareStringPart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                }
                else
                {
                    shareStringPart = workbookPart.AddNewPart<SharedStringTablePart>();
                }
                int index = InsertSharedStringItem(text, shareStringPart);
                Cell cell = InsertCellInWorksheet(col, row, workbookPart.WorksheetParts.First());
                cell.CellValue = new CellValue(index.ToString());
                cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                workbookPart.WorksheetParts.First().Worksheet.Save();
            }
        }

        static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            if (shareStringPart.SharedStringTable is null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }
            int i = 0;
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();
            return i;
        }

        static WorksheetPart InsertWorksheet(WorkbookPart workbookPart)
        {
            WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());
            newWorksheetPart.Worksheet.Save();
            Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>() ?? workbookPart.Workbook.AppendChild(new Sheets());
            string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);
            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Count() > 0)
            {
                sheetId = sheets.Elements<Sheet>().Select<Sheet, uint>(s =>
                {
                    if (!(s.SheetId is null || !s.SheetId.HasValue))
                    {
                        return s.SheetId.Value;
                    }
                    return 0;
                }).Max() + 1;
            }
            string sheetName = "Sheet" + sheetId;
            Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);
            workbookPart.Workbook.Save();
            return newWorksheetPart;
        }

        static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;
            Row row;
            if (sheetData?.Elements<Row>().Where(r => !(r.RowIndex is null || r.RowIndex != rowIndex)).Count() != 0)
            {
                row = sheetData.Elements<Row>().Where(r => !(r.RowIndex is null || r.RowIndex != rowIndex)).First();
            }
            else
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }
            if (row.Elements<Cell>().Where(c => !(c.CellReference is null || c.CellReference.Value != columnName + rowIndex)).Count() > 0)
            {
                return row.Elements<Cell>().Where(c => !(c.CellReference is null || c.CellReference.Value != cellReference)).First();
            }
            else
            {
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    if (string.Compare(cell.CellReference?.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }
                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);
                worksheet.Save();
                return newCell;
            }
        }


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


        static string GetCellValue(string fileName, string sheetName, string addressName)
        {
            string value = null;
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                Sheet theSheet = wbPart?.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).FirstOrDefault();
                if (theSheet is null || theSheet.Id is null)
                {
                    throw new ArgumentException("sheetName");
                }
                WorksheetPart wsPart = (WorksheetPart)wbPart.GetPartById(theSheet.Id);
                Cell theCell = wsPart.Worksheet?.Descendants<Cell>()?.Where(c => c.CellReference == addressName).FirstOrDefault();
                if (theCell is null || theCell.InnerText.Length < 0)
                {
                    return string.Empty;
                }
                value = theCell.InnerText;
                if (theCell.DataType is null)
                {
                    return value;
                }
                if (theCell.DataType.Value == CellValues.SharedString)
                {
                    var stringTable = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                    if (!(stringTable is null))
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