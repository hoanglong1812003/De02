using DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DAL;
using System.Globalization;

namespace GUI
{
    public partial class frmSanpham : Form
    {
        private readonly SanphamService sanpham = new SanphamService();
        private readonly LoaiSPService loaiSP = new LoaiSPService();
        public frmSanpham()
        {
            InitializeComponent();
        }

        private void frmSanpham_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvSanpham);
                var listLoaiSP = loaiSP.GetAll();
                var listBenhnhans = sanpham.GetAll();
                FillLoaiSPCombobox(listLoaiSP);
                BindGrid(listBenhnhans);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.BorderStyle = BorderStyle.None;
            dgview.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dgview.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgview.BackgroundColor = Color.White;
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void FillLoaiSPCombobox(List<LoaiSP> listLoaiSP)
        {
            listLoaiSP.Insert(0, new LoaiSP());
            this.cboLoaiSP.DataSource = listLoaiSP;
            this.cboLoaiSP.DisplayMember = "TenLoai";
            this.cboLoaiSP.ValueMember = "MaLoai";
        }
        private void BindGrid(List<Sanpham> listSanpham)
        {
            dgvSanpham.Rows.Clear();
            foreach (var item in listSanpham)
            {
                int index = dgvSanpham.Rows.Add();
                dgvSanpham.Rows[index].Cells[0].Value = item.MaSP;
                dgvSanpham.Rows[index].Cells[1].Value = item.TenSP;
                dgvSanpham.Rows[index].Cells[2].Value = item.Ngaynhap;
                dgvSanpham.Rows[index].Cells[3].Value = item.LoaiSP.TenLoai;
            }
        }
        private void dgvSanpham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            DataGridViewRow row = dgvSanpham.Rows[e.RowIndex];
            txtMaSP.Text = row.Cells[0].Value.ToString();
            txtTenSP.Text = row.Cells[1].Value.ToString();
            dtNgaynhap.Text = row.Cells[2].Value.ToString();
            cboLoaiSP.Text = row.Cells[3].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaSP.Text == "" || txtTenSP.Text == "")
                    throw new Exception("Vui lòng nhập đầy đủ thông tin sản phẩm");
                else if (txtMaSP.Text.Length < 1)
                {
                    throw new Exception("Mã SP không hợp lệ");
                }
                else if (txtTenSP.Text.Length < 3)
                {
                    throw new Exception("Tên SP không hợp lệ");
                }
                int selectedRow = GetSelectedRow(txtMaSP.Text);
                if (selectedRow == -1)
                {
                    selectedRow = dgvSanpham.Rows.Add();
                    InsertUpdate(selectedRow);
                    MessageBox.Show("Thêm sản phẩm mới thành công",
                        "Thông báo", MessageBoxButtons.OK);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaSP.Text == "" || txtTenSP.Text == "")
                    throw new Exception("Vui lòng nhập đầy đủ thông tin sản phẩm");
                else if (txtMaSP.Text.Length < 1)
                {
                    throw new Exception("Mã SP không hợp lệ");
                }
                else if (txtTenSP.Text.Length < 3)
                {
                    throw new Exception("Tên SP không hợp lệ");
                }
                int selectedRow = GetSelectedRow(txtMaSP.Text);
                if (selectedRow == -1)
                {
                    selectedRow = dgvSanpham.Rows.Add();
                    InsertUpdate(selectedRow);
                    MessageBox.Show("Cập nhật dữ liệu thành công",
                        "Thông báo", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private int GetSelectedRow(string MaSP)
        {
            for (int i = 0; i < dgvSanpham.Rows.Count; i++)
            {
                if (dgvSanpham.Rows[i].Cells[0].Value != null)
                    if (dgvSanpham.Rows[i].Cells[0].Value.ToString() == MaSP)
                    {
                        return i;
                    }
            }
            return -1;
        }
        private void InsertUpdate(int selectedRow)
        {
            dgvSanpham.Rows[selectedRow].Cells[0].Value = txtMaSP.Text;
            dgvSanpham.Rows[selectedRow].Cells[1].Value = txtTenSP.Text;
            dgvSanpham.Rows[selectedRow].Cells[2].Value = dtNgaynhap.Text;
            dgvSanpham.Rows[selectedRow].Cells[3].Value = cboLoaiSP.Text;
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            SanphamModel context = new SanphamModel();
            Sanpham dbDelete = context.Sanpham.FirstOrDefault(p => p.MaSP == txtMaSP.Text);

            if (dbDelete != null)
            {
                DialogResult dr = MessageBox.Show("Bạn có muốn xóa sản phẩm này không",
                    "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    context.Sanpham.Remove(dbDelete);
                    context.SaveChanges();
                }
                
            }

            frmSanpham_Load(sender, e);

        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có muốn đóng không?",
                "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btKLuu_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn không muốn lưu?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            string findName = txtTim.Text;
            findName = RemoveDiacritics(findName);
            for (int i = 0; i < dgvSanpham.Rows.Count; i++)
            {
                if (dgvSanpham.Rows[i].Cells[1].Value != null)
                {
                    string name = dgvSanpham.Rows[i].Cells[1].Value.ToString();


                    name = RemoveDiacritics(name);


                    bool contains = name.IndexOf(findName, StringComparison.OrdinalIgnoreCase) >= 0;
                    if (contains)
                    {
                        dgvSanpham.Rows[i].Visible = true;
                    }
                    else
                    {
                        dgvSanpham.Rows[i].Visible = false;
                    }
                }
            }
        }
        private string RemoveDiacritics(string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}

    

