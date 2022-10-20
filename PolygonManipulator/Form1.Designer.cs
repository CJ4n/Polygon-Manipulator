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
            this.Scene2Button = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            // Scene2Button
            // 
            resources.ApplyResources(this.Scene2Button, "Scene2Button");
            this.Scene2Button.Name = "Scene2Button";
            this.Scene2Button.UseVisualStyleBackColor = true;
            this.Scene2Button.Click += new System.EventHandler(this.Scene2Button_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.AddNewPolygonToCanvasButton);
            this.groupBox1.Controls.Add(this.ClearCanvasButton);
            this.groupBox1.Controls.Add(this.Scene2Button);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.Scene1Button);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
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
        private Button Scene2Button;
        private Button button1;
        private SplitContainer splitContainer1;
        private GroupBox groupBox1;
    }
}