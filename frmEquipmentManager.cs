using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Payot_Week06_Week07
{
    public partial class frmEquipmentManager : Form
    {
        private DataTable equipmentTable;

        public frmEquipmentManager()
        {
            InitializeComponent();
        }

        private void frmEquipmentManager_Load(object sender, EventArgs e)
        {
            
            equipmentTable = new DataTable();
            equipmentTable.Columns.Add("Asset Tag");
            equipmentTable.Columns.Add("Equipment");
            equipmentTable.Columns.Add("Category");
            equipmentTable.Columns.Add("Location");
            equipmentTable.Columns.Add("Status");

            
            equipmentTable.Rows.Add("EQ-101", "Laptop", "Computer", "Room 204", "Available");
            equipmentTable.Rows.Add("EQ-102", "Projector", "Audio-Visual", "Library", "Checked Out");
            equipmentTable.Rows.Add("EQ-103", "Camera", "Media", "Room 108", "Under Maintenance");

            dgvEquipment.DataSource = equipmentTable;

            cboStatus.Items.Clear();
            cboStatus.Items.Add("Available");
            cboStatus.Items.Add("Checked Out");
            cboStatus.Items.Add("Under Maintenance");

            cboStatus.SelectedIndex = 0;
        }

       
        private string Escape(string s)
        {
            return s.Replace("'", "''")
                    .Replace("[", "[[]")
                    .Replace("%", "[%]")
                    .Replace("*", "[*]");
        }

        private void ApplyFilter(string text)
        {
            string t = Escape(text.Trim());
            if (t == "")
            {
                equipmentTable.DefaultView.RowFilter = "";
                return;
            }
            equipmentTable.DefaultView.RowFilter =
                $"[Asset Tag] LIKE '%{t}%' OR [Equipment] LIKE '%{t}%'";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ApplyFilter(txtSearch.Text);

            if (dgvEquipment.Rows.Count == 0)
            {
                MessageBox.Show("No matching equipment found.", "Search",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            ApplyFilter("");
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dgvEquipment.CurrentRow == null)
            {
                MessageBox.Show("Please select a row first.", "Update Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboStatus.SelectedItem == null)
            {
                MessageBox.Show("Please choose a status.", "Update Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRowView row = (DataRowView)dgvEquipment.CurrentRow.DataBoundItem;
            row["Status"] = cboStatus.SelectedItem.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            
            string tag = txtAssetTag.Text.Trim();
            string name = txtEquipment.Text.Trim();
            string category = txtCategory.Text.Trim();
            string location = txtLocation.Text.Trim();

            if (tag == "" || name == "")
            {
                MessageBox.Show("Asset Tag and Equipment name are required.",
                    "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            if (equipmentTable.Select($"[Asset Tag] = '{tag.Replace("'", "''")}'").Length > 0)
            {
                MessageBox.Show("That Asset Tag already exists.", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            equipmentTable.Rows.Add(tag, name, category, location, "Available");

            
            txtAssetTag.Clear();
            txtEquipment.Clear();
            txtCategory.Clear();
            txtLocation.Clear();
            txtSearch.Clear();
            ApplyFilter("");
            txtAssetTag.Focus();
        }
    }
}
