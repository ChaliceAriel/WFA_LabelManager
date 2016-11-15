﻿using System;
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


        }

        private void GetOrdersButton_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("16051178");
            //GetOrders("12120608");
            GetOneOrder("PO5278A151");

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
        public void AddSqlParameter(SqlCommand command, string orderNum)
        {
            SqlParameter param = new SqlParameter(
                "@OrderNumber", SqlDbType.VarChar);
            param.Value = orderNum;
            command.Parameters.Add(param);
        }

        public void GetOneOrder(string orderNum)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EcfSqlConnection"].ToString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("mur_GetMsgAndShippingInfoForOrder", sqlConnection);

                command.CommandType = CommandType.StoredProcedure;
                AddSqlParameter(command, orderNum);
                command.Connection = sqlConnection;

                sqlConnection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    GiftCardOrder order = new GiftCardOrder {
                    
                    PONum = GetStringFromReader(reader, "TrackingNumber"),
                    KiboOrderId = GetStringFromReader(reader, "ShopatronOrderId"),
                    DateOrdered = Convert.ToDateTime(reader["DateOrdered"]).ToString("U"),
                    CustomerFirstName = GetStringFromReader(reader, "BillTo_FirstName"),
                    CustomerLastName = GetStringFromReader(reader, "BillTo_LastName"),

                    };

                    
                    order.GiftCardData = new GiftCard {

                    CardImgURL = GetStringFromReader(reader, "GiftCardImageURL"),
                    ToMsg = "Michael",//GetStringFromReader(reader, "GiftCardTo"),
                    FromMsg = "Grandpa Joe",//GetStringFromReader(reader, "GiftCardFrom"),
                    GCMsg = GetStringFromReader(reader, "GiftCardMessage"),
                    GCAmount = (decimal)reader["GCAmount"],
                    };

                    order.ShipToAddress = new Address {

                        FirstName = "Chalice",//GetStringFromReader(reader, "ShipTo_FirstName"),
                        LastName = "Stevens",//GetStringFromReader(reader, "ShipTo_LastName"),
                        LineOne = "",//GetStringFromReader(reader, "ShipTo_Line1"),
                        LineTwo = "74 N Hanley Ave, Apt A",//GetStringFromReader(reader, "ShipTo_Line2"),
                        City = "Bozeman",//GetStringFromReader(reader, "ShipTo_City"),
                        State = "MT",//GetStringFromReader(reader, "ShipTo_State"),
                        Zip = "59718",//GetStringFromReader(reader, "ShipTo_ZipCode"),

                    };

                    SetGiftCardLabelText(order.GiftCardData);

                    SetShippingLabelText(order.ShipToAddress);

                }

            }

        }


        private void SetupLabelObject()
        {
            // clear edit control
            //ObjectDataEdit.Clear();

            // clear all items first
            //ObjectNameSelector.Items.Clear();
            ShippingLabelObject.Clear();

            if (_label == null)
                return;

            List<string> labelField = new List<string>();

            foreach (string objName in _label.ObjectNames)
            {
                if (!string.IsNullOrEmpty(objName))
                {
                    //ObjectNameSelector.Items.Add(objName);
                    labelField.Add(objName);
                }
                //if (ObjectNameSelector.Items.Count > 0)
                //ObjectNameSelector.SelectedIndex = 0;
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
            ShippingLabelField.Text += "\r\n" + Address.LineTwo;
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
                ToFromLabelField.Text += "\r\n\r\nAmount: $" + GiftCard.GCAmount;
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

                MessageBox.Show("Please select a label template file before editing or creating a new label.");
                FileNameEdit.BackColor = System.Drawing.Color.Tomato;
                BrowseBtn.FlatAppearance.BorderColor = System.Drawing.Color.Tomato;
            }

        }

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
            //public List<int> buildLineItemsList()
            //{
            //    List<int> lineItemsList = new List<int>();


            //    //add all line items associated with that Kibo order id to the list
            //    //for each line item in the list add table row

            //    return lineItemsList;
            //}
        



        class LabelFont : IFontInfo
        {
            public string FontName { get; set; }

            public double FontSize { get; set; }

            public DYMO.Label.Framework.FontStyle FontStyle { get; set; }

            //FontStyle Enum:
            
            //None	    0	Normal style text.
            //Bold	    1	Bold text.
            //Italic    2	Italic text.
            //Underline 4	Underline text.
            //Strikeout 8	Strikeout text.


        }

    }

}
