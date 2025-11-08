using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CongCuTraCuuDNS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnTraCuu_Click(object sender, EventArgs e)
        {
            string domain = txtDomain.Text.Trim();

            if (string.IsNullOrEmpty(domain))
            {
                MessageBox.Show("Vui lòng nhập tên miền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lstKetQua.Items.Clear();
            lblTrangThai.Text = "Trạng thái: Đang tra cứu...";

            try
            {
                DateTime startTime = DateTime.Now;

                // Tra cứu bất đồng bộ
                IPAddress[] addresses = await Dns.GetHostAddressesAsync(domain);

                lstKetQua.Items.Add("=====================================");
                lstKetQua.Items.Add("Tên miền: " + domain);
                lstKetQua.Items.Add("");

                int countIPv4 = 0, countIPv6 = 0;
                foreach (IPAddress ip in addresses)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        lstKetQua.Items.Add("→ IPv4: " + ip);
                        countIPv4++;
                    }
                    else if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        lstKetQua.Items.Add("→ IPv6: " + ip);
                        countIPv6++;
                    }
                }

                lstKetQua.Items.Add("");
                lstKetQua.Items.Add($"Tổng cộng: {countIPv4} IPv4, {countIPv6} IPv6");
                lstKetQua.Items.Add("Thời gian tra cứu: " + startTime.ToString("HH:mm:ss dd/MM/yyyy"));
                lstKetQua.Items.Add("=====================================");

                lblTrangThai.Text = "Trạng thái: Hoàn tất";
            }
            catch (Exception ex)
            {
                lblTrangThai.Text = "Trạng thái: Lỗi tra cứu";
                MessageBox.Show("Lỗi khi tra cứu DNS: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
