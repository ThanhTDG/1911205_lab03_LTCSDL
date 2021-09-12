using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace _1911205_Lab03
{
    public partial class SinhVienForm : Form
    {
        QuanLySinhVien DSSV;
        public SinhVienForm()
        {
            InitializeComponent();
        }
        private void SinhVienForm_Load(object sender, System.EventArgs e)
        {
            lvDSSV.FullRowSelect = true;
            DSSV = new QuanLySinhVien();
            DSSV.DocTuFile();
            LoadToLV(DSSV.DanhSach);
        }
        private SinhVien GetSinhVien()
        {
            SinhVien sv = new SinhVien();
            bool gt = true;
            List<string> cn = new List<string>();
            if (mtxMaSo.Text.Contains("SV."))
            {
                string str = mtxMaSo.Text.Substring(3);
                sv.MaSo = str;
            }
            else
            {
                sv.MaSo = mtxMaSo.Text;
            }
            sv.HoTen = txtHoTen.Text;
            sv.NgaySinh = dtpNgaySinh.Value;
            sv.DiaChi = txtDiaChi.Text;
            sv.Lop = cbbLop.Text;
            sv.Hinh = txtLinkHinh.Text;
            if (rdNu.Checked)
                gt = false;
            sv.GioiTinh = gt;
            for (int i = 0; i < this.cklChuyenNganh.Items.Count; i++)
                if (cklChuyenNganh.GetItemChecked(i))
                    cn.Add(cklChuyenNganh.Items[i].ToString());
            sv.ChuyenNganh = cn;
            return sv;
        }
        private SinhVien GetSinhVienLV(ListViewItem lvitem)
        {
            SinhVien sv = new SinhVien();
            sv.MaSo = lvitem.SubItems[0].Text;
            sv.HoTen = lvitem.SubItems[1].Text;
            sv.NgaySinh = DateTime.Parse(lvitem.SubItems[2].Text);
            sv.DiaChi = lvitem.SubItems[3].Text;
            sv.Lop = lvitem.SubItems[4].Text;
            sv.GioiTinh = false;
            if (lvitem.SubItems[5].Text == "Nam")
                sv.GioiTinh = true;
            List<string> cn = new List<string>();
            string[] s = lvitem.SubItems[6].Text.Split(',');
            foreach (string t in s)
                cn.Add(t);
            sv.ChuyenNganh = cn;
            sv.Hinh = lvitem.SubItems[7].Text;
            return sv;
        }
        private void GetSVToConTrols(SinhVien sv)
        {
            mtxMaSo.Text = sv.MaSo;
            txtHoTen.Text = sv.HoTen;
            dtpNgaySinh.Value = sv.NgaySinh;
            cbbLop.Text = sv.Lop;
            txtDiaChi.Text = sv.DiaChi;
            txtLinkHinh.Text = sv.Hinh;
            picSinhVien.ImageLocation = sv.Hinh;
            if (sv.GioiTinh)
                rdNam.Checked = true;
            else
                rdNu.Checked = true;
            for (int i = 0; i < cklChuyenNganh.Items.Count; i++)
                cklChuyenNganh.SetItemChecked(i, false);
            foreach (string s in sv.ChuyenNganh)
            {
                for (int i = 0; i < cklChuyenNganh.Items.Count; i++)
                {
                    if (s.CompareTo(cklChuyenNganh.Items[i]) == 0)
                        cklChuyenNganh.SetItemChecked(i, true);
                }
            }
        }
        private void AddtoLV(SinhVien sv)
        {
            ListViewItem lvItem = new ListViewItem(sv.MaSo);
            lvItem.SubItems.Add(sv.HoTen);
            lvItem.SubItems.Add(sv.NgaySinh.ToShortDateString());
            lvItem.SubItems.Add(sv.DiaChi);
            lvItem.SubItems.Add(sv.Lop);
            string gt = "Nữ";
            if (sv.GioiTinh)
                gt = "Nam";
            lvItem.SubItems.Add(gt);
            string cn = "";
            foreach (string s in sv.ChuyenNganh)
                cn += s + ",";
            cn = cn.Substring(0, cn.Length - 1);
            lvItem.SubItems.Add(cn);
            lvItem.SubItems.Add(sv.Hinh);
            lvDSSV.Items.Add(lvItem);
        }
        private void LoadToLV(List<SinhVien> ds)
        {
            lvDSSV.Items.Clear();
            foreach (SinhVien sv in ds)
            {
                AddtoLV(sv);
            }
        }
        private void lvDSSV_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int count = lvDSSV.SelectedItems.Count;
            if (count > 0)
            {
                ListViewItem lvItem = lvDSSV.SelectedItems[0];
                SinhVien sv = GetSinhVienLV(lvItem);
                GetSVToConTrols(sv);
            }
        }
        private void remove()
        {
            int i, count;
            ListViewItem lvItem;
            count = lvDSSV.Items.Count - 1;
            for (i = count; i >= 0; i--)
            {
                lvItem = lvDSSV.Items[i];
                if (lvItem.Checked)
                    DSSV.Xoa(lvItem.SubItems[0].Text, SoSanhTheoMa);
            }
            LoadToLV(DSSV.DanhSach);
            btnDefault.PerformClick();
        }
        private void btnRemove_Click(object sender, System.EventArgs e)
        {
            remove();
        }
        private void ThemDuLieu()
        {
            SinhVien sv = GetSinhVien();
            SinhVien kq = DSSV.Tim(sv.MaSo, delegate (object obj1, object obj2)
            {
                return (obj2 as SinhVien).MaSo.CompareTo(obj1.ToString());
            });
            if (kq != null)
                MessageBox.Show(
                    "Mã sinh viên đã tồn tạo!",
                    "Lỗi thêm dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else
            {
                DSSV.Them(sv);
                LoadToLV(DSSV.DanhSach);
            }
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            ThemDuLieu();

        }

        private void btnExit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();

        }

        private void btnDefault_Click(object sender, System.EventArgs e)
        {

            mtxMaSo.Text = "";
            txtHoTen.Text = "";
            txtDiaChi.Text = "";
            dtpNgaySinh.Value = DateTime.Now;
            cbbLop.Text = cbbLop.Items[0].ToString();
            txtLinkHinh.Text = "";
            picSinhVien.ImageLocation = "";
            rdNam.Checked = true;
            for (int i = 0; i < cklChuyenNganh.Items.Count; i++)
                cklChuyenNganh.SetItemChecked(i, false);
            LoadToLV(DSSV.DanhSach);

        }
        private void btnSua_Click(object sender, System.EventArgs e)
        {
            SinhVien sv = GetSinhVien();
            bool kqSua;
            kqSua = DSSV.Sua(sv, sv.MaSo, SoSanhTheoMa);
            if (kqSua)
                LoadToLV(DSSV.DanhSach);
        }
        private int SoSanhTheoMa(object obj1, object obj2)
        {
            SinhVien sv = obj2 as SinhVien;
            return sv.MaSo.CompareTo(obj1);
        }
        private void btnAddPic_Click(object sender, System.EventArgs e)
        {
            openpic();
        }
        private void mởFileToolStripMenuItem1_Click(object sender, System.EventArgs e)
        {
            openpic();
        }
        private void thoátToolStripMenuItem1_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }
        private void thêmToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ThemDuLieu();
        }
        private void openpic()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Chọn hình ảnh";// "Add Photos";
            dlg.Multiselect = true;
            dlg.Filter = "Image Files (JPEG, GIF, BMP, etc.)|"
            + "*.jpg;*.jpeg;*.gif;*.bmp;"
            + "*.tif;*.tiff;*.png|"
            + "JPEG files (*.jpg;*.jpeg)|*.jpg;*.jpeg|"
            + "GIF files (*.gif)|*.gif|"
            + "BMP files (*.bmp)|*.bmp|"
            + "TIFF files (*.tif;*.tiff)|*.tif;*.tiff|"
            + "PNG files (*.png)|*.png|"
            + "All files (*.*)|*.*";
            dlg.InitialDirectory = Environment.CurrentDirectory;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var fileName = dlg.FileName;
                txtLinkHinh.Text = fileName;
                picSinhVien.Load(fileName);
            }
        }
        private void xóaToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            remove();
        }
        private void sắpXếpToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            TuyChonForm frm = new TuyChonForm(KieuTim.SapXep);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                switch (frm._type)
                {
                    case KieuTim.MaSV:
                        DSSV.DanhSach = DSSV.DanhSach.OrderBy(x => x.MaSo).ToList();
                        break;
                    case KieuTim.HoTen:
                        DSSV.DanhSach = DSSV.DanhSach.OrderBy(x => x.HoTen).ToList();
                        break;
                    case KieuTim.NgaySinh:
                        DSSV.DanhSach = DSSV.DanhSach.OrderBy(x => x.NgaySinh).ToList();
                        break;
                }
                LoadToLV(DSSV.DanhSach);
            }
            else { }

        }
        private void tìmToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            TuyChonForm frm = new TuyChonForm(KieuTim.Tim);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                List<SinhVien> list = new List<SinhVien>();
                switch (frm._type)
                {
                    case KieuTim.MaSV:
                        list = DSSV.DanhSach.FindAll(x => x.MaSo == frm._string).ToList();
                        break;
                    case KieuTim.HoTen:
                        list = DSSV.DanhSach.FindAll(x => x.HoTen == frm._string).ToList();
                        break;
                    case KieuTim.NgaySinh:
                        list = DSSV.DanhSach.FindAll(x => x.NgaySinh == DateTime.Parse(frm._string)).ToList();
                        break;
                }
                MessageBox.Show("Số sinh viên tìm thấy: " + list.Count, "Thông báo", MessageBoxButtons.OK);
                LoadToLV(list);
            }
            else { }
        }
        private void màuChữToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();
            lvDSSV.ForeColor = dlg.Color;
        }
        private void fontToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            FontDialog dlg = new FontDialog();
            dlg.ShowDialog();
            lvDSSV.Font = dlg.Font;
        }
      
    }
}
