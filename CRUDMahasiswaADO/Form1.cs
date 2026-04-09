using System;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUDMahasiswaADO
{
    public partial class FormMahasiswa : Form
      
    {
        private readonly SqlConnection conn;
        private readonly String connectionString =
          "Data Source=FASYALTP\\FASYALTP;Initial Catalog=DBAkademikADO;Integrated Security=True";
        public FormMahasiswa()

        {

            InitializeComponent();
            conn = new SqlConnection(connectionString);
        }

        private void txtNIM_TextChanged(object sender, EventArgs e)
        {

        }

        private void FormMahasiswa_Load(object sender, EventArgs e)
        {

        }
    }
}
