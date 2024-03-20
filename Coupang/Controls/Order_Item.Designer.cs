namespace Coupang.Controls
{
    partial class Order_Item
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.abbrOrderId = new System.Windows.Forms.Label();
            this.O_Amount = new System.Windows.Forms.Label();
            this.O_Price = new System.Windows.Forms.Label();
            this.note = new System.Windows.Forms.Label();
            this.Order_Time = new System.Windows.Forms.Label();
            this.O_modifiers = new System.Windows.Forms.FlowLayoutPanel();
            this.O_status = new System.Windows.Forms.Label();
            this.O_orderServiceType = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.orderAssignStatus = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.statusText = new System.Windows.Forms.Label();
            this.orderPrepareStatus = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pickupTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.remainingTime = new System.Windows.Forms.Label();
            this.remainType = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // abbrOrderId
            // 
            this.abbrOrderId.AutoSize = true;
            this.abbrOrderId.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.abbrOrderId.Location = new System.Drawing.Point(20, 15);
            this.abbrOrderId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.abbrOrderId.Name = "abbrOrderId";
            this.abbrOrderId.Size = new System.Drawing.Size(84, 23);
            this.abbrOrderId.TabIndex = 0;
            this.abbrOrderId.Text = "16G800";
            // 
            // O_Amount
            // 
            this.O_Amount.AutoSize = true;
            this.O_Amount.Location = new System.Drawing.Point(141, 19);
            this.O_Amount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.O_Amount.Name = "O_Amount";
            this.O_Amount.Size = new System.Drawing.Size(55, 18);
            this.O_Amount.TabIndex = 1;
            this.O_Amount.Text = "label2";
            this.O_Amount.Visible = false;
            // 
            // O_Price
            // 
            this.O_Price.AutoSize = true;
            this.O_Price.Location = new System.Drawing.Point(260, 19);
            this.O_Price.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.O_Price.Name = "O_Price";
            this.O_Price.Size = new System.Drawing.Size(55, 18);
            this.O_Price.TabIndex = 2;
            this.O_Price.Text = "label4";
            this.O_Price.Visible = false;
            // 
            // note
            // 
            this.note.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.note.BackColor = System.Drawing.Color.MistyRose;
            this.note.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.note.ForeColor = System.Drawing.Color.Red;
            this.note.Location = new System.Drawing.Point(146, 96);
            this.note.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.note.Name = "note";
            this.note.Size = new System.Drawing.Size(397, 31);
            this.note.TabIndex = 3;
            this.note.Text = "[수저포크X]";
            this.note.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Order_Time
            // 
            this.Order_Time.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Order_Time.AutoSize = true;
            this.Order_Time.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Order_Time.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Order_Time.Location = new System.Drawing.Point(20, 105);
            this.Order_Time.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Order_Time.Name = "Order_Time";
            this.Order_Time.Size = new System.Drawing.Size(55, 19);
            this.Order_Time.TabIndex = 4;
            this.Order_Time.Text = "10:09";
            // 
            // O_modifiers
            // 
            this.O_modifiers.AutoSize = true;
            this.O_modifiers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.O_modifiers.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.O_modifiers.Location = new System.Drawing.Point(19, 84);
            this.O_modifiers.Name = "O_modifiers";
            this.O_modifiers.Size = new System.Drawing.Size(0, 0);
            this.O_modifiers.TabIndex = 5;
            // 
            // O_status
            // 
            this.O_status.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.O_status.BackColor = System.Drawing.Color.Transparent;
            this.O_status.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.O_status.ForeColor = System.Drawing.SystemColors.ControlText;
            this.O_status.Location = new System.Drawing.Point(368, 58);
            this.O_status.Name = "O_status";
            this.O_status.Size = new System.Drawing.Size(169, 29);
            this.O_status.TabIndex = 6;
            this.O_status.Text = "label1";
            this.O_status.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // O_orderServiceType
            // 
            this.O_orderServiceType.AutoSize = true;
            this.O_orderServiceType.Location = new System.Drawing.Point(21, 62);
            this.O_orderServiceType.Name = "O_orderServiceType";
            this.O_orderServiceType.Size = new System.Drawing.Size(55, 18);
            this.O_orderServiceType.TabIndex = 7;
            this.O_orderServiceType.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 18);
            this.label1.TabIndex = 8;
            this.label1.Text = "주문접수";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // orderAssignStatus
            // 
            this.orderAssignStatus.AutoCheck = false;
            this.orderAssignStatus.AutoSize = true;
            this.orderAssignStatus.Location = new System.Drawing.Point(50, 143);
            this.orderAssignStatus.Name = "orderAssignStatus";
            this.orderAssignStatus.Size = new System.Drawing.Size(15, 14);
            this.orderAssignStatus.TabIndex = 9;
            this.orderAssignStatus.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(66, 140);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "-------->";
            // 
            // statusText
            // 
            this.statusText.AutoSize = true;
            this.statusText.Location = new System.Drawing.Point(110, 160);
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(47, 18);
            this.statusText.TabIndex = 11;
            this.statusText.Text = "조리중";
            this.statusText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // orderPrepareStatus
            // 
            this.orderPrepareStatus.AutoCheck = false;
            this.orderPrepareStatus.AutoSize = true;
            this.orderPrepareStatus.Location = new System.Drawing.Point(126, 143);
            this.orderPrepareStatus.Name = "orderPrepareStatus";
            this.orderPrepareStatus.Size = new System.Drawing.Size(15, 14);
            this.orderPrepareStatus.TabIndex = 12;
            this.orderPrepareStatus.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(214, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 18);
            this.label4.TabIndex = 13;
            this.label4.Text = "고객픽업";
            // 
            // pickupTime
            // 
            this.pickupTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pickupTime.AutoSize = true;
            this.pickupTime.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pickupTime.ForeColor = System.Drawing.Color.Green;
            this.pickupTime.Location = new System.Drawing.Point(288, 142);
            this.pickupTime.Name = "pickupTime";
            this.pickupTime.Size = new System.Drawing.Size(73, 29);
            this.pickupTime.TabIndex = 14;
            this.pickupTime.Text = "00:00";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(357, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 18);
            this.label6.TabIndex = 15;
            this.label6.Text = "예상";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(394, 149);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 18);
            this.label7.TabIndex = 16;
            this.label7.Text = "조리시간";
            // 
            // remainingTime
            // 
            this.remainingTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.remainingTime.AutoSize = true;
            this.remainingTime.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.remainingTime.Location = new System.Drawing.Point(451, 142);
            this.remainingTime.Name = "remainingTime";
            this.remainingTime.Size = new System.Drawing.Size(39, 29);
            this.remainingTime.TabIndex = 17;
            this.remainingTime.Text = "00";
            // 
            // remainType
            // 
            this.remainType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.remainType.AutoSize = true;
            this.remainType.Location = new System.Drawing.Point(485, 148);
            this.remainType.Name = "remainType";
            this.remainType.Size = new System.Drawing.Size(51, 18);
            this.remainType.TabIndex = 18;
            this.remainType.Text = "분 남음";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(354, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 36);
            this.button1.TabIndex = 19;
            this.button1.Text = "포장완료알림";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.button2.Location = new System.Drawing.Point(468, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 36);
            this.button2.TabIndex = 20;
            this.button2.Text = "전달완료";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // Order_Item
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Azure;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.remainType);
            this.Controls.Add(this.remainingTime);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pickupTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.orderPrepareStatus);
            this.Controls.Add(this.statusText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.orderAssignStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.O_orderServiceType);
            this.Controls.Add(this.O_status);
            this.Controls.Add(this.note);
            this.Controls.Add(this.O_modifiers);
            this.Controls.Add(this.Order_Time);
            this.Controls.Add(this.O_Price);
            this.Controls.Add(this.O_Amount);
            this.Controls.Add(this.abbrOrderId);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Order_Item";
            this.Size = new System.Drawing.Size(568, 190);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.FlowLayoutPanel O_modifiers;
        internal System.Windows.Forms.Label abbrOrderId;
        internal System.Windows.Forms.Label O_Amount;
        internal System.Windows.Forms.Label O_Price;
        internal System.Windows.Forms.Label note;
        internal System.Windows.Forms.Label Order_Time;
        internal System.Windows.Forms.Label O_status;
        internal System.Windows.Forms.Label O_orderServiceType;
        internal System.Windows.Forms.CheckBox orderAssignStatus;
        internal System.Windows.Forms.CheckBox orderPrepareStatus;
        internal System.Windows.Forms.Label pickupTime;
        internal System.Windows.Forms.Label remainingTime;
        internal System.Windows.Forms.Label remainType;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Label statusText;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.Label label6;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Button button1;
        internal System.Windows.Forms.Button button2;
    }
}
