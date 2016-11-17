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

namespace WindowsFormsApplication_LabelManager
{

    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            PrintLabelBtn.BringToFront();

            this.Text = "Murdoch's Gift Card Label Manager";
        }

        private void GetOrderButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OrderNumberBox.Text))
            {
                MessageBox.Show("Please enter a valid Kibo Order Number or PO Number");
            }
            else
            {
                //12652028 12663347 PO6132A541
                GetOneOrder(OrderNumberBox.Text);
            }

        }

        private void SearchByDate_Click(object sender, EventArgs e)
        {

                var date = SelectDate.Value.Date;

                GetOrdersByDate(date);

        }



        //public List<GiftCardOrder> GetOrders(string orderNum)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["EcfSqlConnection"].ToString();

        //    List<GiftCardOrder>
        //      orders = new List<GiftCardOrder>();

        //    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = new SqlCommand("mur_GetMsgAndShippingInfoForOrder", sqlConnection);
        //        SqlParameter param = new SqlParameter();
        //        param.ParameterName = "@ShopatronOrderId";
        //        param.Value = orderNum;

        //        command.CommandType = CommandType.StoredProcedure;
        //        //command.Parameters.Add(orderNum, SqlDbType.VarChar);
        //        //command.Parameters["@ID"].Value = orderNum;
        //        command.Connection = sqlConnection;

        //        sqlConnection.Open();

        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                GiftCardOrder order = new GiftCardOrder();

        //                order.PONum = GetStringFromReader(reader, "TrackingNumber");
        //                order.KiboOrderId = GetStringFromReader(reader, "ShopatronOrderId");
        //                order.PurchasingCustomer = GetStringFromReader(reader, "BillTo_FirstName" + " " + "BillTo_LastName");

        //                orders.Add(order);
        //            }

        //            return orders;

        //        }

        //    }

        //}


        //public void AddSqlParameter(SqlCommand command, string orderNum)
        //{
        //    SqlParameter param = new SqlParameter(
        //        "@OrderNumber", SqlDbType.VarChar);
        //    param.Value = orderNum;
        //    command.Parameters.Add(param);
        //}

        public List<GiftCardOrder> orderList = new List<GiftCardOrder>();
        public List<GiftCardOrder> orderListByDate = new List<GiftCardOrder>();

        public List<GiftCard> giftCardList = new List<GiftCard>();
        public List<GiftCard> giftCardListByDate = new List<GiftCard>();


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

                //Tie UI GridView to dataTable
                dataGridView1.DataSource = dataTable;

                //Remove boolean (checkbox) columns from grid
                dataGridView1.Columns.Remove("IsGiftCard");
                dataGridView1.Columns.Remove("GiftCardIsElectronicGiftCard");
                //Improve appearance, organize data grid, and only show necessary columns
                AdjustColumnOrder();
                HideColumns();
                AdjustColumnWidths();
                SetHeaderText();


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
                    dataGridView1.Rows[0].Selected = true;
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

                //Tie UI GridView to dataTable
                dataGridView2.DataSource = dataTable;

                //Remove boolean (checkbox) columns from grid
                dataGridView2.Columns.Remove("IsGiftCard");
                dataGridView2.Columns.Remove("GiftCardIsElectronicGiftCard");

                //Improve appearance, organize data grid, and only show necessary columns
                //AdjustColumnOrder();
                //HideColumns();
                //AdjustColumnWidths();
                //SetHeaderText();


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
                    dataGridView2.Rows[0].Selected = true;
                    SetGiftCardLabelText(giftCardListByDate[0]);
                    SetShippingLabelText(orderListByDate[0].ShipToAddress);

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
            //ObjectNameSelector.Enabled = ObjectNameSelector.Items.Count > 0;
            //FileNameEdit.Enabled = ObjectNameSelector.Items.Count > 0 && !string.IsNullOrEmpty(ObjectNameSelector.Text);
            PrintLabelBtn.Enabled = _label != null && !string.IsNullOrEmpty(PrintLabelBtn.Text);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FileNameEdit_TextChanged(object sender, EventArgs e)
        {

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
                    SetLabelsToSelectedRow(index);
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
                    SetLabelsToSelectedRow2(index);
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("No row has been selected for order date view.");
            }
        }

        public void SetLabelsToSelectedRow(int selectedRowIndex)
        {
            int i = selectedRowIndex;

            var row = dataGridView1.Rows[i];

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

        public void SetLabelsToSelectedRow2(int selectedRowIndex)
        {
            int i = selectedRowIndex;

            var row = dataGridView2.Rows[i];

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

        private void BrowseBtn_Click(object sender, EventArgs e)
        {


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _label = Framework.Open(openFileDialog1.FileName);

                // show the file name
                FileNameEdit.Text = openFileDialog1.FileName;
                

                // populate label objects
                SetupLabelObject();

                // setup paper tray selection
                //SetupLabelWriterSelection(false);

                UpdateControls();
            }
        }

        private void ShippingLabelField_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //_label.SetObjectText(ObjectNameSelector.Text, ShippingLabelField.Text);
                _label.SetObjectText(ShippingLabelObject.Text, ShippingLabelField.Text);

            }
            catch (NullReferenceException)
            {

                MessageBox.Show("Please select a label template file before editing or creating a new label.");
            }

        }



        private void ShippingLabelField_Leave(object sender, EventArgs e)
        {

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

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = bindingSource1;
            // populate label objects
            SetupLabelObject();

            UpdateControls();
            
        
        }

        private void ToFromLabelField_TextChanged(object sender, EventArgs e)
        {

            try
            {
                //_label.SetObjectText(ObjectNameSelector.Text, ShippingLabelField.Text);
                _label.SetObjectText(ToFromLabelObject.Text, ToFromLabelField.Text);

            }
            catch (NullReferenceException)
            {

                //MessageBox.Show("Please select a label template file before editing or creating a new label.");

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

        private void HideColumns()
        {

            dataGridView1.Columns["BillTo_FirstName"].Visible = false;
            dataGridView1.Columns["GiftCardMessage"].Visible = false;
            dataGridView1.Columns["LineItemId"].Visible = false;
            dataGridView1.Columns["GiftCardRecipientEmail"].Visible = false;
            dataGridView1.Columns["ShipTo_Line1"].Visible = false;
            dataGridView1.Columns["ShipTo_Line2"].Visible = false;
            dataGridView1.Columns["ShipTo_City"].Visible = false;
            dataGridView1.Columns["ShipTo_State"].Visible = false;
            dataGridView1.Columns["ShipTo_ZipCode"].Visible = false;
            dataGridView1.Columns["BillTo_Line1"].Visible = false;
            dataGridView1.Columns["BillTo_Line2"].Visible = false;
            dataGridView1.Columns["BillTo_City"].Visible = false;
            dataGridView1.Columns["BillTo_State"].Visible = false;
            dataGridView1.Columns["BillTo_ZipCode"].Visible = false;
            dataGridView1.Columns["ShipTo_FirstName"].Visible = false;
            dataGridView1.Columns["ShipTo_LastName"].Visible = false;
            dataGridView1.Columns["PaidAmount"].Visible = false;

        }

        private void AdjustColumnOrder()
        {
            dataGridView1.Columns["ShopatronOrderId"].DisplayIndex = 0;
            dataGridView1.Columns["TrackingNumber"].DisplayIndex = 1;
            dataGridView1.Columns["BillTo_LastName"].DisplayIndex = 2;
            dataGridView1.Columns["DateOrdered"].DisplayIndex = 3;
            dataGridView1.Columns["GCAmount"].DisplayIndex = 4;
            dataGridView1.Columns["GiftCardTo"].DisplayIndex = 5;
            dataGridView1.Columns["GiftCardFrom"].DisplayIndex = 6;
            dataGridView1.Columns["GiftCardImageURL"].DisplayIndex = 7;
        }

        private void AdjustColumnWidths()
        {
            dataGridView1.Columns["ShopatronOrderId"].Width = 130;
            dataGridView1.Columns["TrackingNumber"].Width = 140;
            dataGridView1.Columns["BillTo_LastName"].Width = 130;
            dataGridView1.Columns["DateOrdered"].Width = 110;
            dataGridView1.Columns["GCAmount"].Width = 120;
            dataGridView1.Columns["GiftCardTo"].Width = 130;
            dataGridView1.Columns["GiftCardFrom"].Width = 140;
            dataGridView1.Columns["GiftCardImageURL"].Width = 320;
        }

        private void SetHeaderText()
        {
            dataGridView1.Columns["ShopatronOrderId"].HeaderText = "Kibo Order #";
            dataGridView1.Columns["TrackingNumber"].HeaderText = "PO/Tracking #";
            dataGridView1.Columns["BillTo_LastName"].HeaderText = "Purchasing Customer";
            dataGridView1.Columns["DateOrdered"].HeaderText = "Date Ordered";
            dataGridView1.Columns["GCAmount"].HeaderText = "Gift Card Amount";
            dataGridView1.Columns["GiftCardTo"].HeaderText = "To Message";
            dataGridView1.Columns["GiftCardFrom"].HeaderText = "From Message";
            dataGridView1.Columns["GiftCardImageURL"].HeaderText = "Gift Card";
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }

}
