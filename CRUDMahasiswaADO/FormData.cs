using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; // Ditambahkan untuk MemoryStream (Gambar)
using ExcelDataReader; // Ditambahkan untuk membaca file Excel

namespace CRUDMahasiswaADO
{
    public partial class FormMahasiswa : Form
    {
        // 14.a: Deklarasi objek DAL
        DAL dbLogic = new DAL();

        private BindingSource bindingSource = new BindingSource();
        private DataTable dtMahasiswa = new DataTable();

        public FormMahasiswa()
        {
            InitializeComponent();
            // connectionString dan SqlConnection manual dihapus karena sudah diurus oleh class DAL
        }

        private void FormMahasiswa_Load(object sender, EventArgs e)
        {
            cmbJK.Items.Clear();
            cmbJK.Items.Add("L");
            cmbJK.Items.Add("P");

            cmbJK.DataSource = new string[] { "L", "P" };

            // Setting Grid
            DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGridView1.MultiSelect = false;
            DataGridView1.ReadOnly = true;
            DataGridView1.AllowUserToAddRows = false;
            DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            bindingNavigator1.BindingSource = bindingSource;

            LoadData();
        }

        // 14.b: Mengubah LoadData menggunakan DAL
        private void LoadData()
        {
            try
            {
                dtMahasiswa = dbLogic.GetMhs();
                bindingSource.DataSource = dtMahasiswa;
                DataGridView1.DataSource = bindingSource;

                // Mengatur agar gambar pas (Stretch/Zoom) di dalam sel DataGridView
                if (DataGridView1.Columns.Contains("Foto"))
                {
                    DataGridViewImageColumn fotoColumn = (DataGridViewImageColumn)DataGridView1.Columns["Foto"];
                    fotoColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
                }

                BindControls();
                HitungTotal();

                DataGridView1.Enabled = true;
                btnImpDb.Enabled = false;
                btnInsert.Enabled = true;
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }

        private void BindControls()
        {
            txtNIM.DataBindings.Clear();
            txtNama.DataBindings.Clear();
            cmbJK.DataBindings.Clear();
            dtpTanggalLahir.DataBindings.Clear();
            txtAlamat.DataBindings.Clear();
            txtKodeProdi.DataBindings.Clear();

            txtNIM.DataBindings.Add("Text", bindingSource, "NIM");
            txtNama.DataBindings.Add("Text", bindingSource, "Nama");
            cmbJK.DataBindings.Add("Text", bindingSource, "JenisKelamin");
            dtpTanggalLahir.DataBindings.Add("Value", bindingSource, "TanggalLahir");
            txtAlamat.DataBindings.Add("Text", bindingSource, "Alamat");
            txtKodeProdi.DataBindings.Add("Text", bindingSource, "KodeProdi");
        }

        // 14.b: Mengubah HitungTotal menggunakan DAL
        private void HitungTotal()
        {
            try
            {
                int total = dbLogic.CountMhs();
                // Pastikan nama komponen label totalmu benar (misal lblTotal)
                // Jika error, sesuaikan dengan nama label di desain form milikmu
                // lblTotal.Text = "Total Mahasiswa: " + total; 
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
            }
        }

        // 14.b: Mengubah SimpanLog menggunakan DAL
        private void SimpanLog(string pesan)
        {
            dbLogic.InsertLog(pesan);
        }

        // 14.b: Menambahkan logika untuk menghapus preview gambar
        private void ClearForm()
        {
            txtNIM.Enabled = true;
            txtNIM.Clear();
            txtNama.Clear();
            cmbJK.SelectedIndex = -1;
            txtAlamat.Clear();
            txtKodeProdi.Clear();
            dtpTanggalLahir.Value = DateTime.Now;
            if (fotoMhs != null) fotoMhs.Image = null; // Menghapus gambar
            txtNIM.Focus();
        }

        // Fungsi Bantuan: Mengonversi gambar di PictureBox menjadi array byte (BLOB)
        private byte[] ConvertImageToBytes(PictureBox pb)
        {
            if (pb.Image == null) return null;
            using (MemoryStream ms = new MemoryStream())
            {
                pb.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        // 14.c: Mengubah logika Insert, Update, Delete menggunakan DAL
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] imgBytes = ConvertImageToBytes(fotoMhs);
                dbLogic.InsertMhs(txtNIM.Text, txtNama.Text, txtAlamat.Text, cmbJK.Text, dtpTanggalLahir.Value.Date, txtKodeProdi.Text, imgBytes);

                MessageBox.Show("Data mahasiswa berhasil ditambahkan");
                ClearForm();
                LoadData();
            }
            catch (Exception ex)
            {
                SimpanLog("General Error : " + ex.Message);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] imgBytes = ConvertImageToBytes(fotoMhs);
                dbLogic.UpdateMhs(txtNIM.Text, txtNama.Text, txtAlamat.Text, cmbJK.Text, dtpTanggalLahir.Value.Date, txtKodeProdi.Text, imgBytes);

                MessageBox.Show("Data mahasiswa berhasil diupdate");
                ClearForm();
                LoadData();
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dg = MessageBox.Show("Yakin ingin menghapus data?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dg == DialogResult.Yes)
                {
                    dbLogic.DeleteMhs(txtNIM.Text);
                    MessageBox.Show("Data mahasiswa berhasil dihapus");
                    ClearForm();
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                SimpanLog(ex.Message);
                MessageBox.Show("Terjadi kesalahan: " + ex.Message);
            }
        }

        // 14.d: Ubah event DataGrid agar bisa memunculkan gambar
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataRow row = ((DataRowView)bindingSource[e.RowIndex]).Row;

                txtNIM.Text = row["NIM"].ToString();
                txtNama.Text = row["Nama"].ToString();
                cmbJK.Text = row["JenisKelamin"].ToString();
                dtpTanggalLahir.Value = Convert.ToDateTime(row["TanggalLahir"]);
                txtAlamat.Text = row["Alamat"].ToString();
                // Jika dari SP menggunakan 'KodeProdi' atau 'Nama Prodi', sesuaikan index/nama kolomnya
                txtKodeProdi.Text = row["KodeProdi"] != DBNull.Value ? row["KodeProdi"].ToString() : "";

                // Menampilkan Foto
                if (row.Table.Columns.Contains("Foto") && row["Foto"] != DBNull.Value)
                {
                    byte[] imgBytes = (byte[])row["Foto"];
                    using (MemoryStream ms = new MemoryStream(imgBytes))
                    {
                        fotoMhs.Image = Image.FromStream(ms);
                        fotoMhs.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                }
                else
                {
                    fotoMhs.Image = null;
                }

                txtNIM.Enabled = false; // Disable NIM agar tidak diubah saat update
            }
        }

        // 14.e: Fungsi Upload Gambar dan Excel
        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fotoMhs.Image = Image.FromFile(ofd.FileName);
                fotoMhs.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void btnImpExcel_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel Workbook|*.xlsx" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                    {
                        using (var reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            });

                            DataTable dt = result.Tables[0];
                            DataGridView1.DataSource = dt;
                            DataGridView1.Enabled = false;

                            btnImpDb.Enabled = true;
                            btnInsert.Enabled = false;
                            btnUpdate.Enabled = false;
                            btnDelete.Enabled = false;
                        }
                    }
                }
            }
        }

        private void btnImpDb_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)DataGridView1.DataSource;
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("Tidak ada data untuk diimport.");
                    return;
                }

                foreach (DataRow row in dt.Rows)
                {
                    string nim = row["NIM"].ToString().Trim();
                    string nama = row["Nama"].ToString().Trim();
                    string jk = row["JenisKelamin"].ToString().Trim();
                    string alamat = row["Alamat"].ToString().Trim();

                    // Terkadang di Excel namanya "Nama Prodi" atau "KodeProdi"
                    string kodeProdi = row.Table.Columns.Contains("KodeProdi") ? row["KodeProdi"].ToString().Trim() : string.Empty;

                    if (string.IsNullOrEmpty(nim) || string.IsNullOrEmpty(nama)) continue;

                    DateTime tglLahir;
                    if (!DateTime.TryParse(row["TanggalLahir"].ToString(), out tglLahir)) continue;

                    // Insert data massal dari Excel (tanpa foto)
                    dbLogic.InsertMhs(nim, nama, alamat, jk, tglLahir, kodeProdi, null);
                }

                MessageBox.Show("Data mahasiswa berhasil diimport ke Database");
                ClearForm();
                LoadData();
            }
            catch (Exception ex)
            {
                SimpanLog("General Error: " + ex.Message);
                MessageBox.Show("Error Import: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormRekapData fm3 = new FormRekapData();
            fm3.Show();
            this.Hide();
        }

        private void txtNIM_TextChanged(object sender, EventArgs e) { }
        private void txtKodeProdi_TextChanged(object sender, EventArgs e) { }
        private void fotoMhs_Click(object sender, EventArgs e) { }

        // Logika Reset dan Test Injection dialihkan ke DAL jika masih dipakai
        private void btnReset_Click(object sender, EventArgs e) { }
        private void btnTestInjection_Click(object sender, EventArgs e) { }
        private void btnConnect_Click(object sender, EventArgs e) { }
    }
}