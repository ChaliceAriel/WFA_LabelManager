using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DYMO.Label.Framework;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication_LabelManager
{

    public partial class Form1 : Form
    {
        //Initialize lists
        public List<GiftCardOrder> orderList = new List<GiftCardOrder>();
        public List<GiftCardOrder> orderListByDate = new List<GiftCardOrder>();
        public List<GiftCard> giftCardList = new List<GiftCard>();
        public List<GiftCard> giftCardListByDate = new List<GiftCard>();




        public Form1()
        {
            InitializeComponent();

            PrintLabelBtn.BringToFront();
        }

        //Event Handlers

        private void Form1_Load(object sender, EventArgs e)
        {

            // populate label objects
            SetupLabelObject();

            UpdateControls();

            string message = "Please begin by selecting a saved label template.\n\r\n\r You will be unable to edit or create new labels without a template, so this step is very important. \n\r\n\r Thank You!";
            string caption = "Choose A Label Template";
            MessageBoxButtons button = MessageBoxButtons.OK;

            DialogResult result;
            //Display label template message and trigger click on Browse button
            result = MessageBox.Show(message, caption, button);
            BrowseBtn_Click(sender, e);

        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _label = Framework.Open(openFileDialog1.FileName);

                // show the file name
                FileNameEdit.Text = openFileDialog1.FileName;

                // populate label objects
                SetupLabelObject();

                UpdateControls();
            }
        }

        private void GetOrderButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OrderNumberBox.Text))
            {
                MessageBox.Show("Please enter a valid Kibo Order Number or PO Number");
            }
            else
            {
                GetOneOrder(OrderNumberBox.Text);
            }

        }

        private void SearchByDate_Click(object sender, EventArgs e)
        {
                var date = SelectDate.Value.Date;
                GetOrdersByDate(date);
        }

        private void PrintLabelBtn_Click(object sender, EventArgs e)
        {

            IPrinter printer = Framework.GetPrinters()["DYMO LabelWriter 450 Turbo"];
            if (printer is ILabelWriterPrinter)
            {
                ILabelWriterPrintParams printParams = null;
                ILabelWriterPrinter labelWriterPrinter = printer as ILabelWriterPrinter;
                if (labelWriterPrinter.IsTwinTurbo)
                {
                    printParams = new LabelWriterPrintParams();
                    printParams.RollSelection = (RollSelection)Enum.Parse(typeof(RollSelection), "Auto");
                }

                _label.Print(printer, printParams);
            }
            else
                _label.Print(printer); // print with default params
        }

        private void PrintLabelBtn_KeyDown(object sender, KeyEventArgs e)
        {
            PrintLabelBtn_Click(sender, e);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    dataGridView2.ClearSelection();
                }

                if (dataGridView1.SelectedRows.Count > 0)
                {
                    var index = dataGridView1.CurrentCell.RowIndex;
                    SetLabelsToSelectedRow(index, dataGridView1);
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("No row has been selected for single order view.");
            }

        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    dataGridView1.ClearSelection();
                }
                if (dataGridView2.SelectedRows.Count > 0)
                {
                    var index = dataGridView2.CurrentCell.RowIndex;
                    SetLabelsToSelectedRow(index, dataGridView2);
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("No row has been selected for order date view.");
            }
        }

        //SQL Methods For Getting Data

        public void GetOneOrder(string orderNum)
        {
            //clear Lists first to remove SQL data from previous query
            giftCardList.Clear();
            orderList.Clear();

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();

            string connectionString = ConfigurationManager.ConnectionStrings["EcfSqlConnection"].ToString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                //Create SQL connection using stored procedure
                SqlCommand command = new SqlCommand("mur_GetMsgAndShippingInfoForOrder", sqlConnection);
                //Add parameter to stored procedure
                command.Parameters.Add(new SqlParameter("@OrderNumber", orderNum));
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = sqlConnection;

                dataAdapter.SelectCommand = command;

                dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(dataTable);

                FormatGiftCardURLColumn(dataTable);

                var g1 = dataGridView1;
                
                //Tie UI GridView to dataTable
                g1.DataSource = dataTable;

                //Remove boolean (checkbox) columns from grid
                g1.Columns.Remove("IsGiftCard");
                g1.Columns.Remove("GiftCardIsElectronicGiftCard");
                //Improve appearance, organize data grid, and only show necessary columns
                AdjustColumnOrder(g1);
                FormatAmountColumn(g1);
                HideColumns(g1);
                AdjustColumnWidths(g1);
                SetHeaderText(g1);


                sqlConnection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        //create new objects from each row of data

                        GiftCardOrder order = new GiftCardOrder
                        {

                            PONum = GetStringFromReader(reader, "TrackingNumber"),
                            KiboOrderId = GetStringFromReader(reader, "ShopatronOrderId"),
                            DateOrdered = Convert.ToDateTime(reader["DateOrdered"]).ToString("U"),
                            CustomerFirstName = GetStringFromReader(reader, "BillTo_FirstName"),
                            CustomerLastName = GetStringFromReader(reader, "BillTo_LastName"),

                        };

                        order.GiftCardData = new GiftCard
                        {

                            CardImgURL = GetStringFromReader(reader, "GiftCardImageURL"),
                            ToMsg = GetStringFromReader(reader, "GiftCardTo"),
                            FromMsg = GetStringFromReader(reader, "GiftCardFrom"),
                            GCMsg = GetStringFromReader(reader, "GiftCardMessage"),
                            GCAmount = (decimal)reader["GCAmount"],
                        };

                        order.ShipToAddress = new Address
                        {

                            FirstName = GetStringFromReader(reader, "ShipTo_FirstName"),
                            LastName = GetStringFromReader(reader, "ShipTo_LastName"),
                            LineOne = GetStringFromReader(reader, "ShipTo_Line1"),
                            LineTwo = GetStringFromReader(reader, "ShipTo_Line2"),
                            City = GetStringFromReader(reader, "ShipTo_City"),
                            State = GetStringFromReader(reader, "ShipTo_State"),
                            Zip = GetStringFromReader(reader, "ShipTo_ZipCode"),

                        };


                        //add new objects to empty object lists
                        giftCardList.Add(order.GiftCardData);
                        orderList.Add(order);
                    }

                    //Set Label field text to first index (row 0) and set that row as selected
                    dataGridView2.ClearSelection();
                    g1.Rows[0].Selected = true;
                    SetGiftCardLabelText(giftCardList[0]);
                    SetShippingLabelText(orderList[0].ShipToAddress);
                }

                else
                {
                    MessageBox.Show("I'm sorry. No order was found with the PO Number or Kibo Order value of '" + orderNum+"'.");
                }
            }

        }

        public void GetOrdersByDate(DateTime date)
        {
            //clear Lists first to remove SQL data from previous query
            giftCardListByDate.Clear();
            orderListByDate.Clear();

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataTable dataTable = new DataTable();

            string connectionString = ConfigurationManager.ConnectionStrings["EcfSqlConnection"].ToString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                //Create SQL connection using stored procedure
                SqlCommand command = new SqlCommand("mur_GetMsgAndShippingInfoForGCOrders_ByDate", sqlConnection);
                //Add parameter to stored procedure
                command.Parameters.Add(new SqlParameter("@StartDate", date));
                command.CommandType = CommandType.StoredProcedure;
                command.Connection = sqlConnection;

                dataAdapter.SelectCommand = command;

                dataTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dataAdapter.Fill(dataTable);

                FormatGiftCardURLColumn(dataTable);
                var g2 = dataGridView2;
                //Tie UI GridView to dataTable
                g2.DataSource = dataTable;

                //Remove boolean (checkbox) columns from grid
                RemoveCheckBoxColumns(g2);
                
                //Improve appearance, organize data grid, and only show necessary columns
                AdjustColumnOrder(g2);
                FormatAmountColumn(g2);
                HideColumns(g2);
                AdjustColumnWidths(g2);
                SetHeaderText(g2);


                sqlConnection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        //create new objects from each row of data

                        GiftCardOrder order = new GiftCardOrder
                        {

                            PONum = GetStringFromReader(reader, "TrackingNumber"),
                            KiboOrderId = GetStringFromReader(reader, "ShopatronOrderId"),
                            DateOrdered = Convert.ToDateTime(reader["DateOrdered"]).ToString("U"),
                            CustomerFirstName = GetStringFromReader(reader, "BillTo_FirstName"),
                            CustomerLastName = GetStringFromReader(reader, "BillTo_LastName"),

                        };

                        order.GiftCardData = new GiftCard
                        {

                            CardImgURL = GetStringFromReader(reader, "GiftCardImageURL"),
                            ToMsg = GetStringFromReader(reader, "GiftCardTo"),
                            FromMsg = GetStringFromReader(reader, "GiftCardFrom"),
                            GCMsg = GetStringFromReader(reader, "GiftCardMessage"),
                            GCAmount = (decimal)reader["GCAmount"],
                        };

                        order.ShipToAddress = new Address
                        {

                            FirstName = GetStringFromReader(reader, "ShipTo_FirstName"),
                            LastName = GetStringFromReader(reader, "ShipTo_LastName"),
                            LineOne = GetStringFromReader(reader, "ShipTo_Line1"),
                            LineTwo = GetStringFromReader(reader, "ShipTo_Line2"),
                            City = GetStringFromReader(reader, "ShipTo_City"),
                            State = GetStringFromReader(reader, "ShipTo_State"),
                            Zip = GetStringFromReader(reader, "ShipTo_ZipCode"),

                        };


                        //add new objects to empty object lists
                        giftCardListByDate.Add(order.GiftCardData);
                        orderListByDate.Add(order);
                    }

                    //Set Label field text to first index (row 0) and set that row as selected
                    dataGridView1.ClearSelection();
                    g2.Rows[0].Selected = true;
                    SetGiftCardLabelText(giftCardListByDate[0]);
                    SetShippingLabelText(orderListByDate[0].ShipToAddress);

                    //MarkUniqueOrders();

                }

                else
                {
                    MessageBox.Show("I'm sorry. No gift card orders were found for this date: " + date + ".");
                }
            }

        }

        public string GetStringFromReader(SqlDataReader instance, string col)
        {
            if (instance[col] == DBNull.Value)
            {
                return string.Empty;
            }
            else
            {
                return (string)instance[col];
            }
        }

        //Label Methods
        private void SetupLabelObject()
        {

            ShippingLabelObject.Clear();

            if (_label == null)
                return;

            List<string> labelField = new List<string>();

            foreach (string objName in _label.ObjectNames)
            {
                if (!string.IsNullOrEmpty(objName))
                {
                    
                    labelField.Add(objName);
                }

            }

            ShippingLabelObject.Text = labelField[0];
            try
            {
                if (!string.IsNullOrEmpty(labelField[1]))
                {
                    ToFromLabelObject.Text = labelField[1];
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                ToFromLabelObject.Text = " ";
                MessageBox.Show("The label template selected has only one field.");
            }

        }

        private void SetShippingLabelText(Address Address)
        {

            ShippingLabelField.Text = Address.FirstName + " " + Address.LastName;
            if (!String.IsNullOrEmpty(Address.LineOne))
            {
                ShippingLabelField.Text += "\r\n" + Address.LineOne;
            }
            if (!String.IsNullOrEmpty(Address.LineTwo))
            {
                ShippingLabelField.Text += "\r\n" + Address.LineTwo;
            }
            ShippingLabelField.Text += "\r\n" + Address.City + " " + Address.State + ", " + Address.Zip;
        }

        private void SetGiftCardLabelText(GiftCard GiftCard)
        {
            GiftCard.GCAmount = Math.Round(GiftCard.GCAmount, 2);
            GiftCard.GCAmount.ToString();
            

            if (!String.IsNullOrEmpty(GiftCard.ToMsg))
            {
                ToFromLabelField.Text = "To: " + GiftCard.ToMsg;
            }
            if (!String.IsNullOrEmpty(GiftCard.FromMsg))
            {
                ToFromLabelField.Text += "\r\nFrom: " + GiftCard.FromMsg;
                ToFromLabelField.Text += "\r\nAmount: $" + GiftCard.GCAmount;
            }
            else
            {
                ToFromLabelField.Text = "Amount: $" + GiftCard.GCAmount;
            }
            
        }

        private void UpdateControls()
        {
            PrintLabelBtn.Enabled = _label != null && !string.IsNullOrEmpty(PrintLabelBtn.Text);
        }

        public void SetLabelsToSelectedRow(int selectedRowIndex, DataGridView grid)
        {
            int i = selectedRowIndex;

            var row = grid.Rows[i];

            try
            {
                var giftCardData = new GiftCard
                {
                    ToMsg = row.Cells["GiftCardTo"].Value.ToString(),
                    FromMsg = row.Cells["GiftCardFrom"].Value.ToString(),
                    GCAmount = (decimal)row.Cells["GCAmount"].Value,
                };

                var shipToAddress = new Address
                {

                    FirstName = row.Cells["ShipTo_FirstName"].Value.ToString(),
                    LastName = row.Cells["ShipTo_LastName"].Value.ToString(),
                    LineOne = row.Cells["ShipTo_Line1"].Value.ToString(),
                    LineTwo = row.Cells["ShipTo_Line2"].Value.ToString(),
                    City = row.Cells["ShipTo_City"].Value.ToString(),
                    State = row.Cells["ShipTo_State"].Value.ToString(),
                    Zip = row.Cells["ShipTo_ZipCode"].Value.ToString(),

                };

                SetGiftCardLabelText(giftCardData);
                SetShippingLabelText(shipToAddress);
            }
            catch (System.InvalidCastException)
            {
                
            }
        }

        private void ShippingLabelField_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _label.SetObjectText(ShippingLabelObject.Text, ShippingLabelField.Text);

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select a label template file before editing or creating a new label.");
            }

        }

        private void ToFromLabelField_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _label.SetObjectText(ToFromLabelObject.Text, ToFromLabelField.Text);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select a label template file before editing or creating a new label.");
            }

        }

        // DEFINE OBJECTS
        public class Address
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string LineOne { get; set; }
            public string LineTwo { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }

        }

        public class GiftCard
        {
            public string ToMsg { get; set; }
            public string FromMsg { get; set; }
            public string CardImgURL { get; set; }
            public string IsGiftCard { get; set; }
            public string GiftCardIsElectronicGiftCard { get; set; }
            public decimal GCAmount { get; set; }
            public string GCMsg { get; set; }
        }

        public class GiftCardOrder
        {
            public string PONum { get; set; }
            public string KiboOrderId { get; set; }
            public string CustomerFirstName { get; set; }
            public string CustomerLastName { get; set; }
            public string DateOrdered { get; set; }

            public GiftCard GiftCardData { get; set; }
            public Address ShipToAddress { get; set; }

        }

        // METHODS FOR ADJUSTING DATAGRID DISPLAY

        private void RemoveCheckBoxColumns(DataGridView grid)
        {
            grid.Columns.Remove("IsGiftCard");
            grid.Columns.Remove("GiftCardIsElectronicGiftCard");
        }

        private void FormatAmountColumn(DataGridView grid)
        {
            grid.Columns["GCAmount"].DefaultCellStyle.Format = "$0.00##";
        }

        private void FormatGiftCardURLColumn(DataTable table)
        {
            var col = "GiftCardImageURL";

            //string a = "http://s7d9.scene7.com/is/image/murdochs/GCS-";
            //string replacement = " ";
            //Regex rgx = new Regex(a);

            //for (int i = 0; i < table.Rows.Count; i++)
            //{
            //    string input = table.Rows[i][col].ToString();

            //    string result = rgx.Replace(input, replacement);

            //    var index = result.IndexOf("?");

            //    var final = result.Remove(index);

            //    table.Rows[i][col] = final;

            //}



            //Display truncated form of gift card image URLb
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][col].ToString() == "http://s7d9.scene7.com/is/image/murdochs/GCS-Carhartt-9833151?$jpg$&wid=260")
                {
                    table.Rows[i][col] = "Carhartt-9833151";
                }
                if (table.Rows[i][col].ToString() == "http://s7d9.scene7.com/is/image/murdochs/GCS-Jean-9833149?$jpg$&wid=260")
                {
                    table.Rows[i][col] = "Jean-9833149";
                }
                if (table.Rows[i][col].ToString() == "http://s7d9.scene7.com/is/image/murdochs/GCS-Old-Truck-107745?$jpg$&wid=260")
                {
                    table.Rows[i][col] = "Old-Truck-107745";
                }
                if (table.Rows[i][col].ToString() == "http://s7d9.scene7.com/is/image/murdochs/GCS-Sheep-9833145?$jpg$&wid=260")
                {
                    table.Rows[i][col] = "Sheep-9833145";
                }
                if (table.Rows[i][col].ToString() == "http://s7d9.scene7.com/is/image/murdochs/GCS-Holiday-9833147?$jpg$&wid=260")
                {
                    table.Rows[i][col] = "Holiday(Ornament)-9833147";
                }
                if (table.Rows[i][col].ToString() == "http://s7d9.scene7.com/is/image/murdochs/GCS-Dogbone-9833157?$jpg$&wid=260")
                {
                    table.Rows[i][col] = "Dogbone-9833157";
                }
                if (table.Rows[i][col].ToString() == "http://s7d9.scene7.com/is/image/murdochs/GCS-Horselogo-9833153?$jpg$&wid=260")
                {
                    table.Rows[i][col] = "Horselogo-9833153";
                }
                if (table.Rows[i][col].ToString() == "http://s7d9.scene7.com/is/image/murdochs/GCS-Sunflower-107765?$jpg$&wid=260")
                {
                    table.Rows[i][col] = "Sunflower-107765";
                }
            }

        }

        private void HideColumns(DataGridView grid)
        {

            grid.Columns["BillTo_FirstName"].Visible = false;
            grid.Columns["GiftCardMessage"].Visible = false;
            grid.Columns["LineItemId"].Visible = false;
            grid.Columns["GiftCardRecipientEmail"].Visible = false;
            //grid.Columns["ShipTo_Line1"].Visible = false;
            //grid.Columns["ShipTo_Line2"].Visible = false;
            //grid.Columns["ShipTo_City"].Visible = false;
            //grid.Columns["ShipTo_State"].Visible = false;
            //grid.Columns["ShipTo_ZipCode"].Visible = false;
            grid.Columns["BillTo_Line1"].Visible = false;
            grid.Columns["BillTo_Line2"].Visible = false;
            grid.Columns["BillTo_City"].Visible = false;
            grid.Columns["BillTo_State"].Visible = false;
            grid.Columns["BillTo_ZipCode"].Visible = false;
            //grid.Columns["ShipTo_FirstName"].Visible = false;
            //grid.Columns["ShipTo_LastName"].Visible = false;
            grid.Columns["PaidAmount"].Visible = false;

        }

        private void AdjustColumnOrder(DataGridView grid)
        {
            grid.Columns["ShopatronOrderId"].DisplayIndex = 0;
            grid.Columns["TrackingNumber"].DisplayIndex = 1;
            grid.Columns["BillTo_LastName"].DisplayIndex = 2;
            grid.Columns["DateOrdered"].DisplayIndex = 3;
            grid.Columns["GCAmount"].DisplayIndex = 4;
            grid.Columns["GiftCardTo"].DisplayIndex = 5;
            grid.Columns["GiftCardFrom"].DisplayIndex = 6;
            grid.Columns["GiftCardImageURL"].DisplayIndex = 7;
        }

        private void AdjustColumnWidths(DataGridView grid)
        {
            grid.Columns["ShopatronOrderId"].Width = 130;
            grid.Columns["TrackingNumber"].Width = 140;
            grid.Columns["BillTo_LastName"].Width = 130;
            grid.Columns["DateOrdered"].Width = 100;
            grid.Columns["GCAmount"].Width = 80;
            grid.Columns["GiftCardTo"].Width = 160;
            grid.Columns["GiftCardFrom"].Width = 160;
            grid.Columns["GiftCardImageURL"].Width = 130;
        }

        private void SetHeaderText(DataGridView grid)
        {
            grid.Columns["ShopatronOrderId"].HeaderText = "Kibo Order #";
            grid.Columns["TrackingNumber"].HeaderText = "PO/Tracking #";
            grid.Columns["BillTo_LastName"].HeaderText = "Purchasing Customer";
            grid.Columns["DateOrdered"].HeaderText = "Date Ordered";
            grid.Columns["GCAmount"].HeaderText = "Gift Card Amount";
            grid.Columns["GiftCardTo"].HeaderText = "To Message";
            grid.Columns["GiftCardFrom"].HeaderText = "From Message";
            grid.Columns["GiftCardImageURL"].HeaderText = "Gift Card";
        }

    }

}
