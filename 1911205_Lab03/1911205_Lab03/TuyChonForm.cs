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
    public partial class TuyChonForm : Form
    {
        public string _string;
        public KieuTim _type;
        private KieuTim _loai;

        public TuyChonForm(KieuTim loai)
        {
            InitializeComponent();
            _loai = loai;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if(txtFind.Text != "")
            {
                if (rdMaSV.Checked)
                    _type = KieuTim.MaSV;
                else if (rdHoTen.Checked)
                    _type = KieuTim.HoTen;
                else if (rdNgaySinh.Checked)
                    _type = KieuTim.NgaySinh;
                _string = txtFind.Text;
            DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Hãy nhập thông tin", "Lỗi nhập thông tin", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TuyChonForm_Load(object sender, EventArgs e)
        {
            if(_loai == KieuTim.Tim)
            {
                btnSort.Hide();
            }
            else if (_loai == KieuTim.SapXep)
            {
                pnlFind.Hide();
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            if (rdMaSV.Checked)
                _type = KieuTim.MaSV;
            else if (rdHoTen.Checked)
                _type = KieuTim.HoTen;
            else if (rdNgaySinh.Checked)
                _type = KieuTim.NgaySinh;
            DialogResult = DialogResult.OK;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
