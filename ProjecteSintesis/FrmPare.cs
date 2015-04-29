using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;

using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace ProjecteSintesis
{
    public partial class FrmPare : Form
    {
        #region atributs
        //atributs del DataSetoracle(dataset, tableadaptermanager, tableadapters...) i oracle connection, que s'igualarà a la rebuda per paràmetre al constructor
        private DataSetOracle ds;
        private OracleConnection orCon;
        private TableAdapterManager tableAdatpterMan;
        private DataTable clients, articles, municipis, provincies, cabalbara, cabfact, lineasalbara, lineasfact;
        private ARTICLESTableAdapter artTabAd;
        private CABALBARATableAdapter cabTabAd;
        private CABFACTURASTableAdapter cabFactTabAd;
        private CLIENTSTableAdapter cliTabAd;
        private LINEASALBARATableAdapter linAlbTabAd;
        private LINEASFACTURATableAdapter linFactTabAd;
        private MUNICIPISTableAdapter munTabAd;
        private PROVINCIESTableAdapter provTabAd;

        //frmLogin que es el formulari de validació que obre aquest formulari, i que des d'aquí s'haurà d'amagar i tancar al tancar-se tota l'aplicació
        private FrmLogin frmlog;
        private XMLLector xmlLector;//atribut de la classe XMLLector utilitzat per importar la info dels fitxers XML
        private Random rand = new Random(); //atribut random que es fa servir en totes les funcions de generació aleatòria d'albarans i factures

        //totalAlbaransBefore: quantitat d'albarans abans de generar-ne de nous; totalAlbarans: quantitat d'albarans
        //després de generar-se els albarans; amb la diferència sé quins son els albarans que s'han de facturar
        private int totalAlbaransBefore, totalAlbarans;
        private const string facturar = "PROCEDURES.ferfactura";//constant amb el nom del procediment per fer la facturació
        private bool primerCop = true;//bool que s'utilitza per saber si es el primer cop que s'executa la funció obtenir dades; si és aixís no es mostra missatge de notificació, en canvi a partir del 2n cop com que sempre és l'usuari que tria aquesta opció manualment des del menú, al acabar la importació de dades si es mostra una notificació

        #endregion
        public FrmPare()
        {
            InitializeComponent();
        }
        //constructor del formulari que rep una OracleConnection i un form de validació des d'on s'ha obert aquest formulari
        public FrmPare(OracleConnection orCon, FrmLogin frmLogin)
        {
            InitializeComponent();
            this.orCon = orCon;
            this.frmlog = frmLogin;
            this.frmlog.Hide();

            importarDades();//immediatament s'executa la funció importarDades
        }
        #region propietats
        //propietat privada del formulari que retorna el valor del següent valor de la seqüència nalbara de la base de dades oracle
        private int nAlbara()
        {
            int res = 0;
            res = Convert.ToInt32(new OracleCommand("select nalbara.NEXTVAL from dual", orCon).ExecuteScalar().ToString());
            return res;
        }

        //propietat que retorna true si una determinada taula conté un determinat valor(clau primària)
        private bool conteCamp(DataTable taula, string camp)
        {
            return taula.Rows.Contains(camp);
        }

        //propietat pública del formulari que s'utilitza per recuperar la OracleConnection des dels altres formularis de l'aplicació
        public OracleConnection OrCon
        {
            get { return orCon; }
        }
        #endregion
        //mètode que tanca la connexió amb la bd en cas de què hi hagi hagut una excepció i l'aplicació s'hagi de tancar
        private void tancarConnexio()
        {
            if (orCon.State != ConnectionState.Closed)
                orCon.Close();
        }
        #region metodesImportacioDades
        //mètode que crida les funcions fillDataSet(inicialitzar el dataset i omplir les taules,...) i importarXML
        private void importarDades()
        {
            try
            {
                fillDataset();
                importarXML();
            }
            catch (OracleException oe)
            {
                MessageBox.Show(oe.Message);
                tancarConnexio();
                this.frmlog.Close();
            }
            catch (XPathException xe)
            {
                MessageBox.Show(xe.Message);
                tancarConnexio();
                this.frmlog.Close();
            }
            catch (DataException de)
            {
                MessageBox.Show(de.Message);
                tancarConnexio();
                this.frmlog.Close();
            }
            catch (Exception oe)
            {
                MessageBox.Show(oe.ToString());
                tancarConnexio();
                this.frmlog.Close();
            }
        }
        private void fillDataset()
        {
            //inicialitzar el dataset
            ds = new DataSetOracle();
            tableAdatpterMan = new TableAdapterManager();

            //omplir datatables/tableadapters,...
            #region datatables

            provTabAd = new DataSetOracleTableAdapters.PROVINCIESTableAdapter();
            provTabAd.Fill(ds.PROVINCIES);
            tableAdatpterMan.PROVINCIESTableAdapter = provTabAd;
            provincies = ds.PROVINCIES;

            munTabAd = new DataSetOracleTableAdapters.MUNICIPISTableAdapter();
            munTabAd.Fill(ds.MUNICIPIS);
            tableAdatpterMan.MUNICIPISTableAdapter = munTabAd;
            municipis = ds.MUNICIPIS;

            artTabAd = new DataSetOracleTableAdapters.ARTICLESTableAdapter();
            artTabAd.Fill(ds.ARTICLES);
            tableAdatpterMan.ARTICLESTableAdapter = artTabAd;
            articles = ds.ARTICLES;

            cliTabAd = new DataSetOracleTableAdapters.CLIENTSTableAdapter();
            cliTabAd.Fill(ds.CLIENTS);
            tableAdatpterMan.CLIENTSTableAdapter = cliTabAd;
            clients = ds.CLIENTS;

            cabTabAd = new DataSetOracleTableAdapters.CABALBARATableAdapter();
            cabTabAd.Fill(ds.CABALBARA);
            tableAdatpterMan.CABALBARATableAdapter = cabTabAd;
            cabalbara = ds.CABALBARA;

            cabFactTabAd = new DataSetOracleTableAdapters.CABFACTURASTableAdapter();
            cabFactTabAd.Fill(ds.CABFACTURAS);
            tableAdatpterMan.CABFACTURASTableAdapter = cabFactTabAd;
            cabfact = ds.CABFACTURAS;

            linAlbTabAd = new DataSetOracleTableAdapters.LINEASALBARATableAdapter();
            linAlbTabAd.Fill(ds.LINEASALBARA);
            tableAdatpterMan.LINEASALBARATableAdapter = linAlbTabAd;
            lineasalbara = ds.LINEASALBARA;

            linFactTabAd = new DataSetOracleTableAdapters.LINEASFACTURATableAdapter();
            linFactTabAd.Fill(ds.LINEASFACTURA);
            tableAdatpterMan.LINEASFACTURATableAdapter = linFactTabAd;
            lineasfact = ds.LINEASFACTURA;
            #endregion

        }
        private void importarXML()
        {
            //execució del programa .jar 
            System.Diagnostics.Process p = System.Diagnostics.Process.Start("C:/TEMP/XMLupdater.jar");
            while (!p.HasExited) { }

            obtenirInfoXML();//un cop ha acabat l'execució del java, es llegeix la informació provinent dels fitxer .xml
        }
        private void obtenirInfoXML()
        {
            //es crida una funció d'importació per a cada fitxer XML a importar
            importarProvincies();
            importarMunicipis();
            importarClients();
            importarArticles();

            try
            {
                tableAdatpterMan.UpdateAll(ds);//es guarden els cambis del dataset(importancions)

                //es comprova si és el primer cop i en cas contrari es mostra un missatge de notificació de que la importació s'ha realitzat correctament
                if (!primerCop)
                    MessageBox.Show("LES DADES S'HAN IMPORTAT CORRECTAMENT!");
                else
                    primerCop = false;
            }
            catch (DBConcurrencyException dbcex)//capturo l'excepcio i llanço un missatge de notificació de l'error
            {
                MessageBox.Show("error en la simultanietat de dades! Torni a importar les dades des del menú");
                primerCop = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void obtenirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            obtenirDades();
        }
        private void obtenirDades()//funció que fa el mateix que importarDades() però en aquest cas només s'omplen les taules de municipis, provincies, clients i articles
        {
            try
            {
                omplirDataSets();
                importarXML();
            }
            catch (OracleException oe)
            {
                MessageBox.Show(oe.Message);
                tancarConnexio();
                this.frmlog.Close();
            }
            catch (XPathException xe)
            {
                MessageBox.Show(xe.Message);
                tancarConnexio();
                this.frmlog.Close();
            }
            catch (DataException de)
            {
                MessageBox.Show(de.Message);
                tancarConnexio();
                this.frmlog.Close();
            }
            catch (Exception oe)
            {
                MessageBox.Show(oe.Message);
                tancarConnexio();
                this.frmlog.Close();
            }
        }
        private void omplirDataSets()//es fa un Fill de les 4 taules
        {
            artTabAd.Fill(ds.ARTICLES);
            cliTabAd.Fill(ds.CLIENTS);
            munTabAd.Fill(ds.MUNICIPIS);
            provTabAd.Fill(ds.PROVINCIES);
        }
        #region importacioProvincies
        private void importarProvincies()//mètode que realitza la importació de les dades de l'arxiu provincies.xml
        {
            xmlLector = new XMLLector("C:/TEMP/provincies.xml");//s'instancia un objecte de la classe XMLlector
            XPathNodeIterator iterador = xmlLector.iterador("provincies/provincia");//s'obté l'iterador amb tots els nodes 'provincia' de l'arrel 'provincies'

            int codi;
            string nom = "";
            XPathNavigator nav;

            while (iterador.MoveNext())//es recorre l'iterador obtingut abans i per cada XPathNavigator es recupera el valors dels atributs codi i nom
            {
                nav = iterador.Current;
                codi = Convert.ToInt32(XMLLector.nodeValor(nav, "codi"));
                nom = XMLLector.nodeValor(nav, "nom");

                //en cas de què el dataset(amb les dades originals de la bbdd oracle) contingui ja un registre amb el codi de l'element actual, s'actualitza aquest registre; en cas contrari s'afegeix una nova provincia
                if (conteCamp(provincies, codi.ToString()))
                    actualitzarProvincia(codi, nom);
                else
                    novaProvincia(codi, nom);
            }
        }
        //funció que donat un codi i nom de provincia actualitza el nom del datarow corresponent a aquest codi
        private void actualitzarProvincia(int codi, string nom)
        {
            DataRow fila = provincies.Rows.Find(codi);
            fila["NOMPROVINCIA"] = nom;
        }
        //funció que donat un codi i un nom de provincia insereix una nova fila a la taula provincies del dataset
        private void novaProvincia(int codi, string nom)
        {
            object[] cols = new object[] { codi, nom };
            provincies.Rows.Add(cols);
        }
        #endregion
        #region importacioMunicipis
        //metode que realitza la importació de les dades del fitxer municipis.xml
        private void importarMunicipis()
        {
            xmlLector = new XMLLector("C:/TEMP/municipis.xml");
            XPathNodeIterator iterador = xmlLector.iterador("municipis/municipi");

            int codi;
            string nom = "", abr = "";
            XPathNavigator nav;

            while (iterador.MoveNext())
            {
                nav = iterador.Current;
                codi = Convert.ToInt32(XMLLector.nodeValor(nav, "codimunicipi"));
                nom = XMLLector.nodeValor(nav, "nommunicipi");
                abr = XMLLector.nodeValor(nav, "abrev");

                if (conteCamp(municipis, codi.ToString()))
                    actualitzarMunicipi(codi, nom, abr);
                else
                    nouMunicipi(codi, nom, abr);
            }
        }

        //actualització d'un municipi existent
        private void actualitzarMunicipi(int codi, string nom, string abr)
        {
            DataRow fila = municipis.Rows.Find(codi);
            fila["NOMMUNICIPI"] = nom;
            fila["ABREVIATURA"] = abr;
        }

        //addició d'un municipi existent
        private void nouMunicipi(int codi, string nom, string abr)
        {
            object[] cols = new object[] { codi, nom, abr };
            municipis.Rows.Add(cols);
        }
        #endregion
        #endregion

        #region importacioArticles
        //mètode que realitza la importació del fitxer productes.xml
        private void importarArticles()
        {
            xmlLector = new XMLLector("C:/TEMP/productes.xml");
            XPathNodeIterator iter = xmlLector.iterador("articles/Productes");

            string codi = "", descr = "";
            double pcost;
            decimal pvenda;//com que rep molts de decimals del node d'origen he posat tipus decimal
            int stock;

            while (iter.MoveNext())
            {
                XPathNavigator nav = iter.Current;

                codi = XMLLector.nodeValor(nav, "idProducte");
                descr = XMLLector.nodeValor(nav, "descripcio");
                pcost = Math.Abs(Double.Parse(XMLLector.nodeValor(nav, "pcost"), CultureInfo.InvariantCulture));
                pvenda = Math.Abs(Math.Round(Decimal.Parse(XMLLector.nodeValor(nav, "pvenda"), CultureInfo.InvariantCulture), 2));
                stock = Convert.ToInt32(XMLLector.nodeValor(nav, "quantitatstock"));

                if (conteCamp(articles, codi))
                    actualitzarArticle(codi, descr, pcost, pvenda, stock);
                else
                    nouArticle(codi, descr, pcost, pvenda, stock);
            }
        }

        //actualització d'un article existent
        private void actualitzarArticle(string codi, string descr, double pcost, decimal pvenda, int stock)
        {
            DataRow fila = articles.Rows.Find(codi);
            fila["DESCRIPCIO"] = descr;
            fila["QUANTITATSTOCK"] = stock;
            fila["PCOST"] = pcost;
            fila["PVENDA"] = pvenda;
        }

        //afegir un nou article al dataset
        private void nouArticle(string id, string descr, double pcost, decimal pvenda, int stock)
        {
            object[] cols = new object[] { id, descr, stock, pcost, pvenda, null };
            articles.Rows.Add(cols);
        }
        #endregion

        #region importacioClients
        //mètode que realitza la importació de clients del fitxer clients.xml
        private void importarClients()
        {
            xmlLector = new XMLLector("C:/TEMP/clients.xml");
            XPathNodeIterator iterador = xmlLector.iterador("clients/client");

            string codi = "", nif = "", nom = "", adr = "", tlfn = "", cp = "", municipi = "";
            int mun = 0, prov = 0;
            XPathNavigator nav;

            while (iterador.MoveNext())
            {
                nav = iterador.Current;
                codi = XMLLector.nodeValor(nav, "codi");
                nif = XMLLector.nodeValor(nav, "NIF");
                nom = XMLLector.nodeValor(nav, "nom");
                nom = nom.Trim();
                adr = XMLLector.nodeValor(nav, "adreca");
                tlfn = XMLLector.nodeValor(nav, "telefon");
                cp = XMLLector.nodeValor(nav, "codipostal");

                municipi = XMLLector.nodeValor(nav, "municipi");
                mun = Convert.ToInt32(municipi);
                prov = Convert.ToInt32(municipi.Substring(0, 2));

                //comprovem que els camps municipi i la seva provincia existeixin a les taules corresponents,
                //ja que alguns nodes dels arxius xml origen tenen com a valor de provincies numeros inexistents!
                //en aquest cas el valor del codi de provincia seràn els dos primers dígits del codi postal del client
                if (!conteCamp(provincies, prov.ToString()))
                    prov = Convert.ToInt32(cp.Substring(0, 2));

                if (conteCamp(clients, codi))
                    actualitzarClient(codi, nif, nom, adr, tlfn, mun, prov);
                else
                    nouClient(codi, nif, nom, adr, tlfn, mun, prov);
            }

        }

        //mètode que actualitza un client existent al dataset
        private void actualitzarClient(string codi, string nif, string nom, string adr, string tlfn, int mun, int prov)
        {
            DataRow fila = clients.Rows.Find(codi);
            fila["NIF"] = nif;
            fila["NOM"] = nom;
            fila["ADREÇA"] = adr;
            fila["TELEFON"] = tlfn;
            fila["CODIMUNICIPI"] = mun;
            fila["CODIPROVINCIA"] = prov;
        }

        //mètode que afegeix al dataset un nou client
        private void nouClient(string codi, string nif, string nom, string adr, string tlfn, int mun, int prov)
        {
            object[] cols = new object[] { codi, nif, nom, adr, mun, prov, tlfn };
            clients.Rows.Add(cols);
        }

        #endregion
        #region generarDades
        //al clickar la opcio generarDades del menu es crida la funció generarDades()
        private void generarDadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            generarDades();
        }

        //mètode que realitza tot el procés de generació alèatòria d'albarans i factures
        private void generarDades()
        {
            int[] posicions = new int[clients.Rows.Count];//creo una taula d'enters on aniré guardant les posicions seleccionades de cada client
            int sel = 0;
            try
            {
                if (orCon.State != ConnectionState.Open)//comprovo si la connexió oracle está oberta i sinó l'obro(es farà servir pel procediment ferFactura...)
                    orCon.Open();

                totalAlbaransBefore = cabalbara.Rows.Count;//obtinc el total d'albarans que hi ha actualment, abans de generar-ne de nous

                int numClients = Convert.ToInt32(clients.Rows.Count * .6);//60% dels clients actuals
                int total = clients.Rows.Count;

                //per cada client que s'hagi d'obtenir per crear un nou albarà d'aquest 60% seleccionat, selecciono un numero aleatoriament(sel) fins que en trobi un que no s'hagi seleccionat anteriorment(que la taula posicions no el contingui) i genero l'albarà del client situat en aquesta posició de la taula clients
                for (int i = 0; i < numClients; i++)
                {
                    sel = rand.Next(0, total);
                    while (posicions.Contains(sel))
                        sel = rand.Next(0, total);
                    posicions[i] = sel;

                    ferAlbarans(clients.Rows[sel]);//crido la funcio FerAlbarans i li passo el DataRow del client seleccionat
                }

                tableAdatpterMan.UpdateAll(ds);//actualitzo els nous registres a l'origen de dades Oracle

                totalAlbarans = cabalbara.Rows.Count;//obtinc el total d'albarans actuals, de manera que sé que els albarans que he de seleccionar per generar factures es troben entre aquest valor i la variable totalAlbaransBefore

                ferFactures();//crido el mètode de fer factures

                //un cop generades noves factures i nous albarans, omplo de nou les taules del dataset
                this.tableAdatpterMan.CABALBARATableAdapter.Fill(this.ds.CABALBARA);
                this.tableAdatpterMan.LINEASALBARATableAdapter.Fill(this.ds.LINEASALBARA);
                this.tableAdatpterMan.CABFACTURASTableAdapter.Fill(this.ds.CABFACTURAS);
                this.tableAdatpterMan.LINEASFACTURATableAdapter.Fill(this.ds.LINEASFACTURA);

                MessageBox.Show("albarans i factures generats correctament!");
            }
            catch (OracleException oe)
            {
                MessageBox.Show(oe.ToString());
                tancarConnexio();
                this.frmlog.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                tancarConnexio();
                this.frmlog.Close();
            }
        }

        //mètode que donat un client genera un número aleatori d'albarans associats a aquest client
        private void ferAlbarans(DataRow client)
        {
            int nAlbarans = rand.Next(1, 6);//selecciono un número aleatori entre 1 i 5
            for (int i = 0; i < nAlbarans; i++)
            {
                ferAlbara(client);//genero tants albarans com números aleatoris generats
            }
        }
        //funció que donat un client genera un albarà
        private void ferAlbara(DataRow client)
        {
            //camps de la taula clients
            string codi = client["CODI"].ToString();
            DateTime data = DateTime.Now.Date;
            string nif = client["NIF"].ToString();
            string nom = client["Nom"].ToString();
            string dir = client["ADREÇA"].ToString();
            string pobl = client["CODIMUNICIPI"].ToString();

            int num = nAlbara();//per obtenir el següent número d'albarà crido a la funció nAlbara que fa una consulta a la bbdd oracle i retorna el següent valor de la seqüència nalbara.
            object[] fields = new object[] { num, data, codi, nif, nom, dir, pobl };//creo una matriu d'objectes amb tots els valors necessaris per crear un albarà
            cabalbara.Rows.Add(fields);//finalment afegeixo un albarà a la taula d'albarans

            //a continuació, genero un número de linies aleatori entre 3 i 20 per cada albarà
            rand = new Random();
            int nLinies = rand.Next(3, 21);
            for (int i = 0; i < nLinies; i++)
                ferLinia(num);
        }
        //mètode que donat un número d'albarà genera una linia d'albarà
        private void ferLinia(int nAlbara)
        {
            int quant = rand.Next(1, 26);//quantitat entre 1 i 25

            //article aleatori
            int artPos = 0;
            artPos = rand.Next(0, articles.Rows.Count);//obting l'article de forma aleatòria seleccionant una posició a l'atzar dentre totes les files de la columna articles
            System.Threading.Thread.Sleep(1);//per garantir que les dades es generen de forma més aleatòria paro durant 1 mseg el programa perquè sinó ens trobem que moltes línies d'un mateix albarà tenen el mateix article...

            //obtinc les dades de l'article seleccionat aleatoriament i afegeixo un nou registre a la taula liniesAlbara
            DataRow article = articles.Rows[artPos];
            string codiArt = (article["CODI"]).ToString();
            string descr = article["DESCRIPCIO"].ToString();
            decimal preuVenda = Convert.ToDecimal(article["PVENDA"].ToString());

            object[] fields = new object[] { nAlbara, codiArt, descr, quant, preuVenda };

            lineasalbara.Rows.Add(fields);

        }
        //mètode que genera factures aleatòriament
        private void ferFactures()
        {
            int pos = 0;//variable posició que servirà per seleccionar una posició aleatòria d'entre els albarans creats recentment
            int[] posicions = new int[(totalAlbarans - totalAlbaransBefore) * 2 / 3];//taula per anar guardant les posicions seleccionades i així assegurar-se que no hi hagi repeticions
            int num = posicions.Length;
            for (int i = 0; i < num; i++)
            {
                //selecciono una posició aleatòria, obtinc l'albarà d'aquesta posició i crido la funció ferFactura passantli aquest albarà
                pos = rand.Next(totalAlbaransBefore, totalAlbarans);
                while (posicions.Contains(pos))
                    pos = rand.Next(totalAlbaransBefore, totalAlbarans);
                posicions[i] = pos;

                DataRow alb = cabalbara.Rows[pos];
                ferFactura(alb);
            }
        }
        //funció que donat un albarà genera la factura d'aquest albarà
        private void ferFactura(DataRow alb)
        {
            int nAlb = Convert.ToInt32(alb["NALBARA"]);//obtinc numero d'albarà

            if (orCon.State != ConnectionState.Open)
                orCon.Open();

            //crido al procediment emmagatzemat facturar i li passo com a paràmetre el número d'albarà; el procés s'encarrega automàticament de generar la factura i borrar l'albarà, linies, etc...
            OracleCommand comm = new OracleCommand(facturar, orCon);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.Add("nAlb", OracleDbType.Int32, ParameterDirection.Input).Value = nAlb;
            comm.ExecuteNonQuery();
            comm.Dispose();
        }
        #endregion
        //al tancar el formulari comprovo que la connexió oracle no estigui oberta i tanco també el formulari de validació que havia obtingut per paràmetre del constructor
        private void FrmPare_FormClosed(object sender, FormClosedEventArgs e)
        {
            orCon.Dispose();
            if (!(orCon.State == ConnectionState.Closed))
                orCon.Close();

            frmlog.Close();
        }
        //al clickar el botó de sortir es comprova si el dataset té canvis i si és aíxis s'ofereix la opció de guardarlos, no guardarlos o no fer res(com que al generar dades ja guardo les dades mai sortirà aquesta opció
        private void sortirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ds.HasChanges())
            {
                DialogResult dialeg = MessageBox.Show("desitja guardar els canvis?", "sortir", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (dialeg == System.Windows.Forms.DialogResult.Yes)
                {
                    tableAdatpterMan.UpdateAll(ds);
                    this.Close();
                }
                else if (dialeg == System.Windows.Forms.DialogResult.No)
                    this.Close();
            }
            else
                this.Close();
        }
        #region formularis
        //al clickar la opció clients del menú de formularis s'obre un nou formulari de clients passantli com a paràmetre el dataset actual i el tableadaptermanager
        private void clientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.tableAdatpterMan.CLIENTSTableAdapter.Fill(this.ds.CLIENTS);
            FrmClients nou = new FrmClients(this.ds, this.tableAdatpterMan);
            nou.MdiParent = this;
            nou.Show();
        }

        //al clickar la opció articles del menú de formularis s'obre un nou formulari de articles passantli com a paràmetre el dataset actual i el tableadaptermanager
        private void articlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmArticles nou = new FrmArticles(this.ds, this.tableAdatpterMan);
            nou.MdiParent = this;
            nou.Show();
        }

        //al clickar la opció albarans del menú de formularis s'obre un nou formulari de albarans passantli com a paràmetre el dataset actual i el tableadaptermanager
        private void albaransToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAlbarans nou = new FrmAlbarans(this.ds, this.tableAdatpterMan);
            nou.MdiParent = this;
            nou.Show();
        }

        //al clickar la opció factures del menú de formularis s'obre un nou formulari de factures passantli com a paràmetre el dataset actual i el tableadaptermanager
        private void facturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmFactures nou = new FrmFactures(this.ds, this.tableAdatpterMan);
            nou.MdiParent = this;
            nou.Show();
        }
        #endregion

        #region llistats
        //mètode estàtic del formulari que al passarli el nom d'un fitxer xml i d'un fitxer xsl automàticament genera el fitxer html transformat
        /// <summary>
        /// mètode que donat un fitxer xml i un fitxer xsl genera un fitxer html transformat amb el mateix nom
        /// </summary>
        /// <param name="xmlFile"></param>
        /// <param name="xslFile"></param>
        public static void TransformacioXSL(string xmlFile, string xslFile)
        {
            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(xslFile);

            XPathDocument doc = new XPathDocument(xmlFile);
            string htmlFile = xmlFile.Replace("xml", "html");
            XmlTextWriter resultat = new XmlTextWriter(htmlFile, Encoding.Default);

            transform.Transform(doc, resultat);//transformació: genero un arxiu html
            resultat.Close();

            System.Diagnostics.Process pr = new System.Diagnostics.Process();
            pr.StartInfo.Arguments = htmlFile;
            pr.StartInfo.FileName = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
            pr.Start();
        }

        //al clickar la opció articles per descripció del menú de llistats genero un fitxer html amb tots els articles ordenats per la seva descripció
        private void totalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //creo un nou dataset
            DataSet dataSet = new DataSet();
            dataSet.DataSetName = "DataSet";//li assigno un nom genèric DataSet
            DataTable articlesClone = articles.Clone();//creo una taula d'articles que és una clonació de la d'articles original
            articlesClone.TableName = "Table";
            DataView vista = new DataView(articles, "", "", DataViewRowState.CurrentRows);//creo una vista personalitzada de la taula d'articles i la ordeno pel camp descripció
            vista.Sort = "DESCRIPCIO ASC";

            //per cada element de la vista generada creo un nou registre a la taula articlesClone
            foreach (DataRowView fila in vista)
            {
                if (fila["DESCOMPTE"].ToString().Length == 0)
                    fila["DESCOMPTE"] = 0;
                articlesClone.Rows.Add(fila.Row.ItemArray);
            }
            dataSet.Tables.Add(articlesClone);//afegeixo aquesta taula al dataset creat

            //exporto el contingut d'aquest dataset a un fitxer xml
            dataSet.WriteXml("articlesDescr.xml");
            TransformacioXSL("articlesDescr.xml", "articlesDescr.xsl");//crido la funció TransformacioXSL
        }

        //al clickar la opció articles per estoc es genera un un fitxer html amb tots els articles ordenats pel seu estoc...
        private void perEstocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //segueixo el procés idèntic a l'anterior
            DataSet dataSet = new DataSet();
            dataSet.DataSetName = "DataSet";
            DataTable articlesClone = articles.Clone();
            articlesClone.TableName = "Table";
            DataView vista = new DataView(articles, "", "", DataViewRowState.CurrentRows);
            vista.Sort = "QUANTITATSTOCK ASC";

            foreach (DataRowView fila in vista)
            {
                articlesClone.Rows.Add(fila.Row.ItemArray);
            }
            dataSet.Tables.Add(articlesClone);
            dataSet.WriteXml("articlesEstoc.xml");
            TransformacioXSL("articlesEstoc.xml", "articlesEstoc.xsl");
        }

        //si se selecciona la opció de llistar albarans es mostra el formulari frmLlistatAlbara que permet triar un número d'albarà i automaticament genera el fitxer html associat
        private void albaransToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmLlistatAlbara frmAlb = new frmLlistatAlbara(this.ds);
            frmAlb.ShowDialog();
        }

        //genera un fitxer html seguint el mateix procés que amb el llistat d'albarà
        private void facturesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmLlistatFactura frmFact = new FrmLlistatFactura(this.ds);
            frmFact.ShowDialog();
        }
        #endregion

        private void FrmPare_Load(object sender, EventArgs e)
        {

        }
    }

}
