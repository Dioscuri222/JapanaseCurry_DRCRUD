using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class FormReport : Form
    {
        DAL dbLogic = new DAL();
        DataMahasiswa listMahasiswa = new DataMahasiswa();

        string prodi { get; set; }
        DateTime tglmasuk { get; set; }

        public FormReport(string Prodi, DateTime TglMasuk)
        {
            InitializeComponent();

            prodi = Prodi;
            tglmasuk = TglMasuk;

            try
            {
                // Ambil data report melalui DAL
                DataTable dtMahasiswa = dbLogic.getDataRekap(prodi, tglmasuk);

                listMahasiswa.SetDataSource(dtMahasiswa);
                crystalReportViewer1.ReportSource = listMahasiswa;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal load data: " + ex.Message);
            }
        }

        private void FormReport_Load(object sender, EventArgs e)
        {
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {
        }
    }
}