using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class FormReport : Form
    {
        static string connectionString = "Data Source=FASYALTP\\FASYALTP;Initial Catalog=DBAkademikADO;Integrated Security=True";
        SqlConnection conn;
        SqlDataAdapter da;
        DataTable dtMahasiswa;
        DataMahasiswa listMahasiswa = new DataMahasiswa();

        string prodi { get; set; }
        DateTime tglmasuk { get; set; }

        public FormReport(string prodi, DateTime tglmasuk)
        {
            InitializeComponent();

            conn = new SqlConnection(connectionString);


            this.prodi = prodi;
            this.tglmasuk = tglmasuk;

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("sp_Report", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@inProdi", this.prodi);

                cmd.Parameters.AddWithValue("@inTglMsuk", this.tglmasuk.Year);

                da = new SqlDataAdapter(cmd);
                dtMahasiswa = new DataTable();

                da.Fill(dtMahasiswa);

                conn.Close();

                listMahasiswa.SetDataSource(dtMahasiswa);
                crystalReportViewer1.ReportSource = listMahasiswa;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gagal Load Data: " + ex.Message);
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