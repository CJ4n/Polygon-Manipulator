namespace PolygonManipulator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.canvasContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.AddNewPolygonToCanvasButton = new System.Windows.Forms.Button();
            this.ClearCanvasButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.Scene1Button = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lineWidthtextBox = new System.Windows.Forms.TextBox();
            this.wuRadioButton = new System.Windows.Forms.RadioButton();
            this.bresenhamRadioButton = new System.Windows.Forms.RadioButton();
            this.basicRadioButton = new System.Windows.Forms.RadioButton();
            this.fancyRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.ContextMenuStrip = this.canvasContextMenuStrip;
            resources.ApplyResources(this.Canvas, "Canvas");
            this.Canvas.Name = "Canvas";
            this.Canvas.TabStop = false;
            this.Canvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseClick);
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            this.Canvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseUp);
            // 
            // canvasContextMenuStrip
            // 
            this.canvasContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.canvasContextMenuStrip.Name = "contextMenuStrip1";
            resources.ApplyResources(this.canvasContextMenuStrip, "canvasContextMenuStrip");
            // 
            // AddNewPolygonToCanvasButton
            // 
            this.AddNewPolygonToCanvasButton.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.AddNewPolygonToCanvasButton, "AddNewPolygonToCanvasButton");
            this.AddNewPolygonToCanvasButton.Name = "AddNewPolygonToCanvasButton";
            this.AddNewPolygonToCanvasButton.UseVisualStyleBackColor = true;
            this.AddNewPolygonToCanvasButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AddNewPolygonToCanvasButton_MouseClick);
            // 
            // ClearCanvasButton
            // 
            resources.ApplyResources(this.ClearCanvasButton, "ClearCanvasButton");
            this.ClearCanvasButton.Name = "ClearCanvasButton";
            this.ClearCanvasButton.UseVisualStyleBackColor = true;
            this.ClearCanvasButton.Click += new System.EventHandler(this.ClearCanvasButton_Click);
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Scene1Button
            // 
            resources.ApplyResources(this.Scene1Button, "Scene1Button");
            this.Scene1Button.Name = "Scene1Button";
            this.Scene1Button.UseVisualStyleBackColor = true;
            this.Scene1Button.Click += new System.EventHandler(this.Scene1Button_Click);
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.Canvas);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lineWidthtextBox);
            this.groupBox1.Controls.Add(this.wuRadioButton);
            this.groupBox1.Controls.Add(this.bresenhamRadioButton);
            this.groupBox1.Controls.Add(this.basicRadioButton);
            this.groupBox1.Controls.Add(this.fancyRadioButton);
            this.groupBox1.Controls.Add(this.AddNewPolygonToCanvasButton);
            this.groupBox1.Controls.Add(this.ClearCanvasButton);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.Scene1Button);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lineWidthtextBox
            // 
            resources.ApplyResources(this.lineWidthtextBox, "lineWidthtextBox");
            this.lineWidthtextBox.Name = "lineWidthtextBox";
            this.lineWidthtextBox.TextChanged += new System.EventHandler(this.lineWidthtextBox_TextChanged);
            // 
            // wuRadioButton
            // 
            resources.ApplyResources(this.wuRadioButton, "wuRadioButton");
            this.wuRadioButton.Name = "wuRadioButton";
            this.wuRadioButton.TabStop = true;
            this.wuRadioButton.UseVisualStyleBackColor = true;
            this.wuRadioButton.CheckedChanged += new System.EventHandler(this.wuRadioButton_CheckedChanged);
            // 
            // bresenhamRadioButton
            // 
            resources.ApplyResources(this.bresenhamRadioButton, "bresenhamRadioButton");
            this.bresenhamRadioButton.Name = "bresenhamRadioButton";
            this.bresenhamRadioButton.TabStop = true;
            this.bresenhamRadioButton.UseVisualStyleBackColor = true;
            this.bresenhamRadioButton.CheckedChanged += new System.EventHandler(this.bresenhamRadioButton_CheckedChanged);
            // 
            // basicRadioButton
            // 
            resources.ApplyResources(this.basicRadioButton, "basicRadioButton");
            this.basicRadioButton.Name = "basicRadioButton";
            this.basicRadioButton.TabStop = true;
            this.basicRadioButton.UseVisualStyleBackColor = true;
            this.basicRadioButton.CheckedChanged += new System.EventHandler(this.basicRadioButton_CheckedChanged);
            // 
            // fancyRadioButton
            // 
            resources.ApplyResources(this.fancyRadioButton, "fancyRadioButton");
            this.fancyRadioButton.Name = "fancyRadioButton";
            this.fancyRadioButton.TabStop = true;
            this.fancyRadioButton.UseVisualStyleBackColor = true;
            this.fancyRadioButton.CheckedChanged += new System.EventHandler(this.fancyRadioButton_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.TransparencyKey = System.Drawing.Color.Red;
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox Canvas;
        private Button AddNewPolygonToCanvasButton;
        private Button ClearCanvasButton;
        private ContextMenuStrip canvasContextMenuStrip;
        private CheckBox checkBox1;
        private Button Scene1Button;
        private SplitContainer splitContainer1;
        private GroupBox groupBox1;
        private RadioButton wuRadioButton;
        private RadioButton bresenhamRadioButton;
        private RadioButton basicRadioButton;
        private RadioButton fancyRadioButton;
        private TextBox lineWidthtextBox;
    }
}