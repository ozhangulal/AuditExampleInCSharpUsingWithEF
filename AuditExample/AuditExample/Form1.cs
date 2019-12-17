using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuditExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        NorthwindEntities _db = new NorthwindEntities();
        private void Form1_Load(object sender, EventArgs e)
        {
            List<Category> CatList = _db.Categories.ToList();
            foreach (var item in CatList)
            {
                cmbCategories.Items.Add(item);
            }
        }

        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            Category cat = (Category)cmbCategories.SelectedItem;
            txtCatDesc.Text = cat.Description;
            txtCatId.Text = cat.CategoryID.ToString();
            txtCatName.Text = cat.CategoryName;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            #region Audit

            Category cat = (Category)cmbCategories.SelectedItem;
            AuditCategory auditCat = new AuditCategory();
            auditCat.CategoryID = cat.CategoryID;
            auditCat.OldCategoryName = cat.CategoryName;
            auditCat.OldDescription = cat.Description;
            auditCat.CategoryName = txtCatName.Text;
            auditCat.Description = txtCatDesc.Text;
            auditCat.EditDate = DateTime.Now;
            _db.AuditCategories.Add(auditCat);

            #endregion

            #region UpdateObjectFields

            Category c = _db.Categories.SingleOrDefault(t => t.CategoryID == cat.CategoryID);

            c.CategoryName = txtCatName.Text;
            c.Description = txtCatDesc.Text;

            if (_db.SaveChanges() > 0)
            {
                MessageBox.Show("Data Updated");
            }
            else
            {
                MessageBox.Show("ERROR 404");
            }

            #endregion

            #region CurrentValuesOfCategories

            txtAuditCatID.Text = cat.CategoryID.ToString();
            txtAuditCatName.Text = cat.CategoryName;
            txtAuditDesc.Text = cat.Description;
            
            AuditCategory cAudit = _db.AuditCategories.SingleOrDefault(x => x.CategoryID==auditCat.CategoryID );
            txtOldCatName.Text = cAudit.OldCategoryName;
            txtOldDescription.Text = cAudit.OldDescription;

            #endregion

        }
    }
}
