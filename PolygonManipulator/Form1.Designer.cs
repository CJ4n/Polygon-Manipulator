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
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
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
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Scene2Button);
            this.Controls.Add(this.Scene1Button);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.Canvas);
            this.Controls.Add(this.AddNewPolygonToCanvasButton);
            this.Controls.Add(this.ClearCanvasButton);
            this.Name = "Form1";
            this.TransparencyKey = System.Drawing.Color.Red;
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}