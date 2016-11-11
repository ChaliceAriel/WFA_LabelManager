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

namespace WindowsFormsApplication_LabelManager
{

    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
           
        }

        private void SetupLabelObject()
        {
            // clear edit control
            //ObjectDataEdit.Clear();

            // clear all items first
            ObjectNameSelector.Items.Clear();

            if (_label == null)
                return;

            foreach (string objName in _label.ObjectNames)
                if (!string.IsNullOrEmpty(objName))
                    ObjectNameSelector.Items.Add(objName);

            if (ObjectNameSelector.Items.Count > 0)
                ObjectNameSelector.SelectedIndex = 0;
        }

        private void UpdateControls()
        {
            ObjectNameSelector.Enabled = ObjectNameSelector.Items.Count > 0;
            //ObjectDataEdit.Enabled = ObjectNameCmb.Items.Count > 0 && !string.IsNullOrEmpty(ObjectNameCmb.Text);
            //PrintLabelBtn.Enabled = _label != null && !string.IsNullOrEmpty(PrintLabelBtn.Text);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FileNameEdit_TextChanged(object sender, EventArgs e)
        {

        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            // use the current file name's folder as the initial
            // directory for the open file dialog
            //string str = FileNameEdit.Text;
            //int i = str.LastIndexOf("\\");
            //str = str.Substring(0, i);
            //openFileDialog1.InitialDirectory = str;

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
            _label.SetObjectText(ObjectNameSelector.Text, ShippingLabelField.Text);
        }

        private void SetShippingLabelText ()
        { 

            Address address1 = new Address()
            {
                FirstName = "Chalice",
                LastName = "Stevens",
                Company = "Murdoch's",
                StreetApt = "74 N Hanley Ave, Apt A",
                City = "Bozeman",
                State = "MT",
                Zip = 59718

            };



            string firstLine = address1.FirstName + " " + address1.LastName;
            string secondLine = address1.Company;
            string thirdLine = address1.StreetApt;
            string fourthLine = address1.City + "," + address1.State + " " + address1.Zip;

            string shippingAddress = firstLine + "\n" + secondLine + "\n" + thirdLine + "\n" + fourthLine;

            //ShippingLabelField.Text = shippingAddress;

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

            SetShippingLabelText();
        }
    }
    public class Address
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string StreetApt { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }

    }
}
