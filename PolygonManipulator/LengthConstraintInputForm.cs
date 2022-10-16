namespace PolygonManipulator
{
    public partial class LengthConstraintInputForm : Form
    {
        public float Length { get; set; }
        public LengthConstraintInputForm()
        {
            InitializeComponent();
            Length = -1;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            try
            {
                var res = float.Parse(this.textBox1.Text);
                this.DialogResult = DialogResult.OK;
                this.Length = res;
                this.Close();
            }
            catch (Exception excep)
            {
                MessageBox.Show(excep.ToString());
                return;

            }

        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
