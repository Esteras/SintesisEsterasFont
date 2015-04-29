using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace ProjecteSintesis
{
    public partial class FrmLogin : Form
    {
        #region atributs
        //atributs del formulari: user i password, connexio oracle(s'inicia aquí i es fa servir en tota l'aplicació) i cadena que es correspon a l'string que es passa a la variable oracle connection
        private string usr = "", pwd = "";
        private OracleConnection orCon;
        private string cadena = "";
        #endregion
        public FrmLogin()
        {
            InitializeComponent();
            try
            {
                //s'inicialitza l'atribut de connexió
                orCon = new OracleConnection();
            }
            catch (OracleException oe)
            {
                MessageBox.Show(oe.Message);
            }
        }

        private void btnAccedir_Click(object sender, EventArgs e)
        {
            //BLABLALBA
            //es posen els valors dels atributs user i passwd a la cadena de text
            cadena = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=xe)));User Id=" + usr + ";Password=" + pwd + ";";
            orCon.ConnectionString = cadena;

            try
            {
                if (orCon.State != ConnectionState.Open)//es comprova que la connexió no estigui oberta i s'obre
                    orCon.Open();

                //s'instancia un nou formulari del tipus FrMPare i se li passen per paràmetres la OracleConnection actual(orCon) i el formulari actual(this)
                FrmPare formPare = new FrmPare(orCon, this);
                formPare.ShowDialog();
            }
            catch (OracleException oe)
            {
                MessageBox.Show("ERROR. Credencials d'Oracle incorrectes");//l'excepció es genera quan la connexió ha fallat, és a dir, les credencials entrades per l'usuari eren errònies; llavors es tanca la connexió oracle en el cas de que s'hagués obert
                if (orCon.State != ConnectionState.Closed)
                    orCon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (orCon.State != ConnectionState.Closed)//qualsevol altra excepció fa tancar també la connexió oracle si es que s'havia obert
                    orCon.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUsr_TextChanged(object sender, EventArgs e)
        {
            usr = txtUsr.Text;
        }

        private void txtPwd_TextChanged(object sender, EventArgs e)
        {
            pwd = txtPwd.Text;
        }

        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            //es comprova si la tecla es ESC(en aquest cas es tanca l'aplicació) o si és INTRO(s'executa perform click del buto d'Acceptar)
            if (e.KeyCode == Keys.Escape)
                this.Close();
            else if (e.KeyCode == Keys.Enter)
                this.btnAccedir.PerformClick();
        }

        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (orCon.State != ConnectionState.Closed)//al tancar el formulari es tanca la connexió oracle
                orCon.Close();
        }
    }
}
