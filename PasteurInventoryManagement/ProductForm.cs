using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasteurInventoryManagement
{
    public partial class ProductForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\luong\OneDrive\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public ProductForm()
        {
            InitializeComponent();
            LoadProduct();
        }

        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(pid,pname,pprice,pdescription,pcategory) LIKE '%"+txtSearch.Text+"%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            con.Close();
        }
        

        private void dgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvProduct.Columns[e.ColumnIndex].Name;
            if (colName == "dataGridViewImageColumn1")
            {
                ProductModuleForm moduleForm = new ProductModuleForm();
                moduleForm.lblPID.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                moduleForm.txtPname.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                moduleForm.txtPQty.Text = dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                moduleForm.txtPprice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                moduleForm.txtPDes.Text = dgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                moduleForm.comboCat.Text = dgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();

                moduleForm.btnSave.Enabled = false;
                moduleForm.btnUpdate.Enabled = true;
                moduleForm.ShowDialog();
            }
            else if (colName == "dataGridViewImageColumn2")
            {
                if (MessageBox.Show("Are you sure you want to delete this product?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbProduct WHERE pid LIKE '" + dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been successfully deleted");
                }
            }
            LoadProduct();
        }

        private void btnAddPro_Click(object sender, EventArgs e)
        {
            ProductModuleForm formModule = new ProductModuleForm();
            formModule.btnSave.Enabled = true;
            formModule.btnUpdate.Enabled = false;
            formModule.ShowDialog();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }
    }
}
