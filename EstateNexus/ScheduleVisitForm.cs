using System;
using System.Windows.Forms;

namespace EstateNexus
{
    public partial class ScheduleVisitForm : Form
    {
        public int PropertyId { get; private set; }
        public DateTime SelectedDate { get; private set; }
        public string SelectedTime { get; private set; }
        public string CustomerNote { get; private set; }

        public ScheduleVisitForm(int propertyId, string propertyTitle, string propertyLocation)
        {
            InitializeComponent();
            PropertyId = propertyId;
            lblPropertyTitleVal.Text = propertyTitle;
            lblPropertyLocationVal.Text = propertyLocation;

            // Default visit date: tomorrow, minimum date: today
            dtpVisitDate.MinDate = DateTime.Today;
            dtpVisitDate.Value = DateTime.Today.AddDays(1);
            if (cmbVisitTime.Items.Count > 2)
            {
                cmbVisitTime.SelectedIndex = 2; // "11:30 AM"
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (dtpVisitDate.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Please select a valid future visit date.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbVisitTime.SelectedItem == null)
            {
                MessageBox.Show("Please select a preferred time slot.", "Select Time Slot", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SelectedDate = dtpVisitDate.Value.Date;
            SelectedTime = cmbVisitTime.SelectedItem.ToString();
            CustomerNote = txtNotes.Text.Trim();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
