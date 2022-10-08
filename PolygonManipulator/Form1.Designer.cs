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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.MovePointsRadioButton = new System.Windows.Forms.RadioButton();
            this.AddPointsRadioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
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
            this.canvasContextMenuStrip.Name = "contextMenuStrip1";
            resources.ApplyResources(this.canvasContextMenuStrip, "canvasContextMenuStrip");
            this.canvasContextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.canvasContextMenuStrip_Opening);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.MovePointsRadioButton);
            this.groupBox1.Controls.Add(this.AddPointsRadioButton);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // MovePointsRadioButton
            // 
            resources.ApplyResources(this.MovePointsRadioButton, "MovePointsRadioButton");
            this.MovePointsRadioButton.Name = "MovePointsRadioButton";
            this.MovePointsRadioButton.TabStop = true;
            this.MovePointsRadioButton.UseVisualStyleBackColor = true;
            this.MovePointsRadioButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MovePointsRadioButton_MouseClick);
            // 
            // AddPointsRadioButton
            // 
            resources.ApplyResources(this.AddPointsRadioButton, "AddPointsRadioButton");
            this.AddPointsRadioButton.Name = "AddPointsRadioButton";
            this.AddPointsRadioButton.TabStop = true;
            this.AddPointsRadioButton.UseVisualStyleBackColor = true;
            this.AddPointsRadioButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.AddPointsRadioButton_MouseClick);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Canvas);
            this.Controls.Add(this.AddNewPolygonToCanvasButton);
            this.Controls.Add(this.ClearCanvasButton);
            this.Name = "Form1";
            this.TransparencyKey = System.Drawing.Color.Red;
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox Canvas;
        private Button AddNewPolygonToCanvasButton;
        private Button ClearCanvasButton;
        private GroupBox groupBox1;
        private RadioButton MovePointsRadioButton;
        private RadioButton AddPointsRadioButton;
        private Label label1;
        private ContextMenuStrip canvasContextMenuStrip;
    }
}