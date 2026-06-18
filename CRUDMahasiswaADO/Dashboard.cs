using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class Dashboard : Form
    {

        DAL dbLogic = new DAL();
        bool isInitializing = true;
        DataTable dt;
        int button = 0;
        public Dashboard()
        {
            InitializeComponent();

            dtpTanggalMasuk.MinDate = new DateTime(2000, 1, 1);
            dtpTanggalMasuk.Format = DateTimePickerFormat.Custom;
            dtpTanggalMasuk.CustomFormat = "yyyy";
            dtpTanggalMasuk.ShowUpDown = true;
            dtpTanggalMasuk.MaxDate = DateTime.Now;
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }
    }
}
