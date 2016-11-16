using DYMO.Label.Framework;

namespace WindowsFormsApplication_LabelManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private ILabel _label;
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.PrintLabelBtn = new System.Windows.Forms.Button();
            this.OrderGrid = new System.Windows.Forms.DataGridView();
            this.orderNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.trackingNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shipToAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toFromAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cardImg = new System.Windows.Forms.DataGridViewImageColumn();
            this.printTwoLabels = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.FileNameEdit = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BrowseBtn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ObjectNameSelector = new System.Windows.Forms.ComboBox();
            this.ShippingLabelField = new System.Windows.Forms.TextBox();
            this.ToFromLabelField = new System.Windows.Forms.TextBox();
            this.ShippingLabelObject = new System.Windows.Forms.TextBox();
            this.ToFromLabelObject = new System.Windows.Forms.TextBox();
            this.GetOrdersButton = new System.Windows.Forms.Button();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.OrderNumberBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.OrderGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // PrintLabelBtn
            // 
            this.PrintLabelBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.PrintLabelBtn.Location = new System.Drawing.Point(1170, 306);
            this.PrintLabelBtn.Name = "PrintLabelBtn";
            this.PrintLabelBtn.Size = new System.Drawing.Size(131, 51);
            this.PrintLabelBtn.TabIndex = 4;
            this.PrintLabelBtn.Text = "PRINT LABEL";
            this.PrintLabelBtn.UseVisualStyleBackColor = false;
            this.PrintLabelBtn.Click += new System.EventHandler(this.PrintLabelBtn_Click);
            this.PrintLabelBtn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PrintLabelBtn_KeyDown);
            // 
            // OrderGrid
            // 
            this.OrderGrid.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OrderGrid.BackgroundColor = System.Drawing.SystemColors.Menu;
            this.OrderGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OrderGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.orderNum,
            this.trackingNumber,
            this.customerName,
            this.shipToAddress,
            this.toFromAmount,
            this.cardImg,
            this.printTwoLabels});
            this.OrderGrid.Location = new System.Drawing.Point(13, 390);
            this.OrderGrid.Name = "OrderGrid";
            this.OrderGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.OrderGrid.Size = new System.Drawing.Size(1288, 375);
            this.OrderGrid.TabIndex = 4;
            // 
            // orderNum
            // 
            this.orderNum.HeaderText = "KIBO Order#";
            this.orderNum.Name = "orderNum";
            this.orderNum.ReadOnly = true;
            this.orderNum.Width = 120;
            // 
            // trackingNumber
            // 
            this.trackingNumber.HeaderText = "PO Number";
            this.trackingNumber.Name = "trackingNumber";
            this.trackingNumber.ReadOnly = true;
            this.trackingNumber.Width = 120;
            // 
            // customerName
            // 
            this.customerName.HeaderText = "Customer";
            this.customerName.Name = "customerName";
            this.customerName.ReadOnly = true;
            this.customerName.Width = 200;
            // 
            // shipToAddress
            // 
            this.shipToAddress.HeaderText = "Ship To Address";
            this.shipToAddress.Name = "shipToAddress";
            this.shipToAddress.Width = 300;
            // 
            // toFromAmount
            // 
            this.toFromAmount.HeaderText = "To/From & Amount Label";
            this.toFromAmount.Name = "toFromAmount";
            this.toFromAmount.Width = 300;
            // 
            // cardImg
            // 
            this.cardImg.HeaderText = "Gift Card";
            this.cardImg.Name = "cardImg";
            this.cardImg.ReadOnly = true;
            this.cardImg.Width = 140;
            // 
            // printTwoLabels
            // 
            this.printTwoLabels.HeaderText = "Print Labels";
            this.printTwoLabels.Name = "printTwoLabels";
            this.printTwoLabels.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.printTwoLabels.Width = 60;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.dateTimePicker1.Location = new System.Drawing.Point(13, 15);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(263, 26);
            this.dateTimePicker1.TabIndex = 2;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // FileNameEdit
            // 
            this.FileNameEdit.BackColor = System.Drawing.SystemColors.Window;
            this.FileNameEdit.Location = new System.Drawing.Point(774, 31);
            this.FileNameEdit.Name = "FileNameEdit";
            this.FileNameEdit.Size = new System.Drawing.Size(527, 26);
            this.FileNameEdit.TabIndex = 5;
            this.FileNameEdit.TextChanged += new System.EventHandler(this.FileNameEdit_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(770, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(423, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Select a label file here: (click Browse... to browse to the file)";
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.BackColor = System.Drawing.Color.RoyalBlue;
            this.BrowseBtn.FlatAppearance.BorderColor = System.Drawing.Color.MidnightBlue;
            this.BrowseBtn.FlatAppearance.BorderSize = 2;
            this.BrowseBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.MidnightBlue;
            this.BrowseBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MidnightBlue;
            this.BrowseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseBtn.Font = new System.Drawing.Font("Book Antiqua", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BrowseBtn.Location = new System.Drawing.Point(670, 17);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(98, 40);
            this.BrowseBtn.TabIndex = 7;
            this.BrowseBtn.Text = "Browse...";
            this.BrowseBtn.UseVisualStyleBackColor = false;
            this.BrowseBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "lwl";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "DYMO Label File (*.label)|*.label";
            // 
            // ObjectNameSelector
            // 
            this.ObjectNameSelector.FormattingEnabled = true;
            this.ObjectNameSelector.Location = new System.Drawing.Point(56, 318);
            this.ObjectNameSelector.Name = "ObjectNameSelector";
            this.ObjectNameSelector.Size = new System.Drawing.Size(150, 28);
            this.ObjectNameSelector.TabIndex = 8;
            // 
            // ShippingLabelField
            // 
            this.ShippingLabelField.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShippingLabelField.Location = new System.Drawing.Point(647, 155);
            this.ShippingLabelField.Multiline = true;
            this.ShippingLabelField.Name = "ShippingLabelField";
            this.ShippingLabelField.Size = new System.Drawing.Size(306, 110);
            this.ShippingLabelField.TabIndex = 3;
            this.ShippingLabelField.TextChanged += new System.EventHandler(this.ShippingLabelField_TextChanged);
            this.ShippingLabelField.Leave += new System.EventHandler(this.ShippingLabelField_Leave);
            // 
            // ToFromLabelField
            // 
            this.ToFromLabelField.Location = new System.Drawing.Point(995, 155);
            this.ToFromLabelField.Multiline = true;
            this.ToFromLabelField.Name = "ToFromLabelField";
            this.ToFromLabelField.Size = new System.Drawing.Size(306, 110);
            this.ToFromLabelField.TabIndex = 9;
            this.ToFromLabelField.TextChanged += new System.EventHandler(this.ToFromLabelField_TextChanged);
            // 
            // ShippingLabelObject
            // 
            this.ShippingLabelObject.Location = new System.Drawing.Point(647, 123);
            this.ShippingLabelObject.Name = "ShippingLabelObject";
            this.ShippingLabelObject.ReadOnly = true;
            this.ShippingLabelObject.Size = new System.Drawing.Size(163, 26);
            this.ShippingLabelObject.TabIndex = 13;
            // 
            // ToFromLabelObject
            // 
            this.ToFromLabelObject.Location = new System.Drawing.Point(995, 123);
            this.ToFromLabelObject.Name = "ToFromLabelObject";
            this.ToFromLabelObject.ReadOnly = true;
            this.ToFromLabelObject.Size = new System.Drawing.Size(163, 26);
            this.ToFromLabelObject.TabIndex = 14;
            // 
            // GetOrdersButton
            // 
            this.GetOrdersButton.Location = new System.Drawing.Point(193, 182);
            this.GetOrdersButton.Name = "GetOrdersButton";
            this.GetOrdersButton.Size = new System.Drawing.Size(145, 49);
            this.GetOrdersButton.TabIndex = 15;
            this.GetOrdersButton.Text = "Submit";
            this.GetOrdersButton.UseVisualStyleBackColor = true;
            this.GetOrdersButton.Click += new System.EventHandler(this.GetOrdersButton_Click);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(309, 15);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(267, 26);
            this.dateTimePicker2.TabIndex = 16;
            // 
            // OrderNumberBox
            // 
            this.OrderNumberBox.Location = new System.Drawing.Point(193, 137);
            this.OrderNumberBox.Name = "OrderNumberBox";
            this.OrderNumberBox.Size = new System.Drawing.Size(145, 26);
            this.OrderNumberBox.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(187, 20);
            this.label2.TabIndex = 18;
            this.label2.Text = "Enter Kibo or PO Number";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1314, 938);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OrderNumberBox);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.GetOrdersButton);
            this.Controls.Add(this.ToFromLabelObject);
            this.Controls.Add(this.ShippingLabelObject);
            this.Controls.Add(this.ToFromLabelField);
            this.Controls.Add(this.ShippingLabelField);
            this.Controls.Add(this.ObjectNameSelector);
            this.Controls.Add(this.BrowseBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileNameEdit);
            this.Controls.Add(this.OrderGrid);
            this.Controls.Add(this.PrintLabelBtn);
            this.Controls.Add(this.dateTimePicker1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Label Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OrderGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button PrintLabelBtn;
        private System.Windows.Forms.DataGridView OrderGrid;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox FileNameEdit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BrowseBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox ObjectNameSelector;
        private System.Windows.Forms.TextBox ShippingLabelField;
        private System.Windows.Forms.DataGridViewTextBoxColumn orderNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn trackingNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipToAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn toFromAmount;
        private System.Windows.Forms.DataGridViewImageColumn cardImg;
        private System.Windows.Forms.DataGridViewCheckBoxColumn printTwoLabels;
        private System.Windows.Forms.TextBox ToFromLabelField;
        private System.Windows.Forms.TextBox ShippingLabelObject;
        private System.Windows.Forms.TextBox ToFromLabelObject;
        private System.Windows.Forms.Button GetOrdersButton;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.TextBox OrderNumberBox;
        private System.Windows.Forms.Label label2;
    }
}

