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
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.FileNameEdit = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BrowseBtn = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.ShippingLabelField = new System.Windows.Forms.TextBox();
            this.ToFromLabelField = new System.Windows.Forms.TextBox();
            this.ShippingLabelObject = new System.Windows.Forms.TextBox();
            this.ToFromLabelObject = new System.Windows.Forms.TextBox();
            this.GetOrderButton = new System.Windows.Forms.Button();
            this.SelectDate = new System.Windows.Forms.DateTimePicker();
            this.OrderNumberBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SearchByDate = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // PrintLabelBtn
            // 
            this.PrintLabelBtn.BackColor = System.Drawing.Color.LightGreen;
            this.PrintLabelBtn.Font = new System.Drawing.Font("Modern No. 20", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrintLabelBtn.Location = new System.Drawing.Point(10, 72);
            this.PrintLabelBtn.Name = "PrintLabelBtn";
            this.PrintLabelBtn.Size = new System.Drawing.Size(131, 48);
            this.PrintLabelBtn.TabIndex = 4;
            this.PrintLabelBtn.Text = "Print Label";
            this.PrintLabelBtn.UseVisualStyleBackColor = false;
            this.PrintLabelBtn.Click += new System.EventHandler(this.PrintLabelBtn_Click);
            this.PrintLabelBtn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PrintLabelBtn_KeyDown);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.CalendarMonthBackground = System.Drawing.SystemColors.Info;
            this.dateTimePicker1.Font = new System.Drawing.Font("Modern No. 20", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Location = new System.Drawing.Point(10, 9);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(286, 28);
            this.dateTimePicker1.TabIndex = 2;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // FileNameEdit
            // 
            this.FileNameEdit.BackColor = System.Drawing.SystemColors.Window;
            this.FileNameEdit.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileNameEdit.Location = new System.Drawing.Point(417, 25);
            this.FileNameEdit.Name = "FileNameEdit";
            this.FileNameEdit.Size = new System.Drawing.Size(494, 26);
            this.FileNameEdit.TabIndex = 5;
            this.FileNameEdit.TextChanged += new System.EventHandler(this.FileNameEdit_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(417, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(404, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "Select a label file here : (click Browse... to browse to the file)";
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.BackColor = System.Drawing.Color.RoyalBlue;
            this.BrowseBtn.FlatAppearance.BorderColor = System.Drawing.Color.MidnightBlue;
            this.BrowseBtn.FlatAppearance.BorderSize = 2;
            this.BrowseBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.MidnightBlue;
            this.BrowseBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MidnightBlue;
            this.BrowseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BrowseBtn.Font = new System.Drawing.Font("Modern No. 20", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrowseBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BrowseBtn.Location = new System.Drawing.Point(313, 9);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.Size = new System.Drawing.Size(98, 41);
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
            // ShippingLabelField
            // 
            this.ShippingLabelField.Font = new System.Drawing.Font("Franklin Gothic Book", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShippingLabelField.Location = new System.Drawing.Point(313, 99);
            this.ShippingLabelField.Multiline = true;
            this.ShippingLabelField.Name = "ShippingLabelField";
            this.ShippingLabelField.Size = new System.Drawing.Size(306, 117);
            this.ShippingLabelField.TabIndex = 3;
            this.ShippingLabelField.TextChanged += new System.EventHandler(this.ShippingLabelField_TextChanged);
            this.ShippingLabelField.Leave += new System.EventHandler(this.ShippingLabelField_Leave);
            // 
            // ToFromLabelField
            // 
            this.ToFromLabelField.Font = new System.Drawing.Font("Franklin Gothic Book", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToFromLabelField.Location = new System.Drawing.Point(661, 99);
            this.ToFromLabelField.Multiline = true;
            this.ToFromLabelField.Name = "ToFromLabelField";
            this.ToFromLabelField.Size = new System.Drawing.Size(306, 117);
            this.ToFromLabelField.TabIndex = 9;
            this.ToFromLabelField.TextChanged += new System.EventHandler(this.ToFromLabelField_TextChanged);
            // 
            // ShippingLabelObject
            // 
            this.ShippingLabelObject.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShippingLabelObject.Location = new System.Drawing.Point(313, 69);
            this.ShippingLabelObject.Name = "ShippingLabelObject";
            this.ShippingLabelObject.ReadOnly = true;
            this.ShippingLabelObject.Size = new System.Drawing.Size(163, 25);
            this.ShippingLabelObject.TabIndex = 13;
            // 
            // ToFromLabelObject
            // 
            this.ToFromLabelObject.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToFromLabelObject.Location = new System.Drawing.Point(661, 69);
            this.ToFromLabelObject.Name = "ToFromLabelObject";
            this.ToFromLabelObject.ReadOnly = true;
            this.ToFromLabelObject.Size = new System.Drawing.Size(163, 25);
            this.ToFromLabelObject.TabIndex = 14;
            // 
            // GetOrderButton
            // 
            this.GetOrderButton.FlatAppearance.BorderColor = System.Drawing.Color.MediumSeaGreen;
            this.GetOrderButton.FlatAppearance.BorderSize = 2;
            this.GetOrderButton.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetOrderButton.Location = new System.Drawing.Point(13, 293);
            this.GetOrderButton.Name = "GetOrderButton";
            this.GetOrderButton.Size = new System.Drawing.Size(91, 35);
            this.GetOrderButton.TabIndex = 15;
            this.GetOrderButton.Text = "Get Order";
            this.GetOrderButton.UseVisualStyleBackColor = true;
            this.GetOrderButton.Click += new System.EventHandler(this.GetOrderButton_Click);
            // 
            // SelectDate
            // 
            this.SelectDate.CalendarFont = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectDate.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectDate.Location = new System.Drawing.Point(1034, 25);
            this.SelectDate.Name = "SelectDate";
            this.SelectDate.Size = new System.Drawing.Size(267, 26);
            this.SelectDate.TabIndex = 16;
            // 
            // OrderNumberBox
            // 
            this.OrderNumberBox.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrderNumberBox.Location = new System.Drawing.Point(13, 257);
            this.OrderNumberBox.Name = "OrderNumberBox";
            this.OrderNumberBox.Size = new System.Drawing.Size(145, 29);
            this.OrderNumberBox.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 236);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(286, 18);
            this.label2.TabIndex = 18;
            this.label2.Text = "Enter KIBO Order Number or PO Number";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.SteelBlue;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.Color.LightBlue;
            this.dataGridView1.Location = new System.Drawing.Point(10, 394);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1294, 189);
            this.dataGridView1.TabIndex = 20;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // SearchByDate
            // 
            this.SearchByDate.Font = new System.Drawing.Font("Modern No. 20", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchByDate.Location = new System.Drawing.Point(1219, 94);
            this.SearchByDate.Name = "SearchByDate";
            this.SearchByDate.Size = new System.Drawing.Size(82, 33);
            this.SearchByDate.TabIndex = 22;
            this.SearchByDate.Text = "Search";
            this.SearchByDate.UseVisualStyleBackColor = true;
            this.SearchByDate.Click += new System.EventHandler(this.SearchByDate_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.BackgroundColor = System.Drawing.Color.SteelBlue;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView2.GridColor = System.Drawing.Color.LightBlue;
            this.dataGridView2.Location = new System.Drawing.Point(10, 602);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(1294, 301);
            this.dataGridView2.TabIndex = 23;
            this.dataGridView2.SelectionChanged += new System.EventHandler(this.dataGridView2_SelectionChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Modern No. 20", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(1036, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(221, 21);
            this.label3.TabIndex = 24;
            this.label3.Text = "Search For Orders By Date";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1314, 912);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.SearchByDate);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.OrderNumberBox);
            this.Controls.Add(this.SelectDate);
            this.Controls.Add(this.GetOrderButton);
            this.Controls.Add(this.ToFromLabelObject);
            this.Controls.Add(this.ShippingLabelObject);
            this.Controls.Add(this.ToFromLabelField);
            this.Controls.Add(this.ShippingLabelField);
            this.Controls.Add(this.BrowseBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileNameEdit);
            this.Controls.Add(this.PrintLabelBtn);
            this.Controls.Add(this.dateTimePicker1);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.Text = "Label Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button PrintLabelBtn;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox FileNameEdit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BrowseBtn;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox ShippingLabelField;
        private System.Windows.Forms.TextBox ToFromLabelField;
        private System.Windows.Forms.TextBox ShippingLabelObject;
        private System.Windows.Forms.TextBox ToFromLabelObject;
        private System.Windows.Forms.Button GetOrderButton;
        private System.Windows.Forms.DateTimePicker SelectDate;
        private System.Windows.Forms.TextBox OrderNumberBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button SearchByDate;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label3;
    }
}

