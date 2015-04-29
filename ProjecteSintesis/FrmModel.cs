using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjecteSintesis
{
    public partial class FrmModel : Form
    {
        #region atributs
        private DataSetOracle ds;
        private DataSetOracleTableAdapters.TableAdapterManager tableAdapterManager;
        private Navegador nav;//control navegador
        private BindingSource bindSource;//bindsource propi de cada formulari en funció de la taula que mostra
        private ToolStripMenuItem menu;//opcions del menú de navegació que s'afegeix a cada formulari
        private ToolStripMenuItem post, ant, inici, fi, filtreForm, filtreSel, filtreExcl, treureFiltre, Ordre, Asc, Desc, noOrdre;
        #endregion

        #region propietats
        public DataSetOracle Ds
        {
            get { return ds; }
            set { this.ds = value; }
        }

        public DataSetOracleTableAdapters.TableAdapterManager TableAdapterManager
        {
            get { return this.tableAdapterManager; }
            set { this.tableAdapterManager = value; }
        }

        protected BindingSource BindSource
        {
            get { return bindSource; }
            set { bindSource = value; }
        }

        public Navegador Nav
        {
            get { return nav; }
            set { nav = value; }
        }

        protected ToolStripMenuItem MenuNavegador
        {
            get { return menu; }
        }

        //propietat heredable que donat un control retorna true si aquest és de la classe TextBox
        protected bool esTextBox(Control c)
        {
            return c.GetType() == typeof(TextBox);
        }
        #endregion

        //constructor sense paràmetres que crida la funció inicialitzar
        public FrmModel()
        {
            InitializeComponent();
            inicialitzar();
            Refresh();
        }

        //constructor amb paràmetres que després d'inicialitzar els diferents controls assigna el dataset i el tableadapter manager als rebuts per paràmetre
        public FrmModel(DataSetOracle dataset, DataSetOracleTableAdapters.TableAdapterManager manager)
        {
            InitializeComponent();
            inicialitzar();
            this.ds = dataset;
            this.tableAdapterManager = manager;
            Refresh();
        }

        protected virtual void FrmModel_Load(object sender, EventArgs e)
        {
            try
            {
                //al carregar el formulari s'assigna l'event keydown del formulari a tots els controls d'aquest, per poder així navegar amb tecles indepedentment de a quin control estigui actualment...
                subscriureEventKeyDown(true);
                actualitzarTag(this);//per a cada textbox d'aquest formulari assigno com a tag el valor actual del text                 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //mètode que inicialitza els diferents controls i subscriu els event click dels botons del navegador
        private void inicialitzar()
        {
            this.ds = new DataSetOracle();
            this.tableAdapterManager = new DataSetOracleTableAdapters.TableAdapterManager();

            this.nav = new Navegador();
            this.nav.Dock = DockStyle.Top;
            this.Controls.Add(nav);

            this.bindSource = new BindingSource();

            creacioMenuNavegable();

            nav.Primer.Click += Primer_Click;
            nav.Seguent.Click += Seguent_Click;
            nav.Anterior.Click += Anterior_Click;
            nav.Ultim.Click += Ultim_Click;
        }

        //mètode que inicialitza tots els elements del menú de navegació, a més de subsriure'n l'event click
        private void creacioMenuNavegable()
        {
            menu = new ToolStripMenuItem();
            menu.Text = "Navegació";

            post = new ToolStripMenuItem();
            ant = new ToolStripMenuItem();
            inici = new ToolStripMenuItem();
            fi = new ToolStripMenuItem();
            filtreForm = new ToolStripMenuItem();
            filtreSel = new ToolStripMenuItem();
            filtreExcl = new ToolStripMenuItem();
            treureFiltre = new ToolStripMenuItem();
            Ordre = new ToolStripMenuItem();
            Asc = new ToolStripMenuItem();
            Desc = new ToolStripMenuItem();
            noOrdre = new ToolStripMenuItem();

            post.Text = "Posterior";
            ant.Text = "Anterior";
            inici.Text = "Inici";
            fi.Text = "Fi";
            filtreForm.Text = "Filtre de formulari";
            filtreSel.Text = "Filtre de selecció";
            filtreExcl.Text = "Filtre excloent de selecció";
            treureFiltre.Text = "Treure filtre";
            Ordre.Text = "Ordre";
            Asc.Text = "ASC";
            Desc.Text = "DESC";
            noOrdre.Text = "No";

            menu.DropDownItems.Add(post);
            menu.DropDownItems.Add(ant);
            menu.DropDownItems.Add(inici);
            menu.DropDownItems.Add(fi);
            menu.DropDownItems.Add(filtreForm);
            menu.DropDownItems.Add(filtreSel);
            menu.DropDownItems.Add(filtreExcl);
            menu.DropDownItems.Add(treureFiltre);
            menu.DropDownItems.Add(Ordre);
            Ordre.DropDownItems.Add(Asc);
            Ordre.DropDownItems.Add(Desc);
            Ordre.DropDownItems.Add(noOrdre);

            post.Click += post_Click;
            ant.Click += ant_Click;
            inici.Click += inici_Click;
            fi.Click += fi_Click;
            Asc.Click += Asc_Click;
            Desc.Click += Desc_Click;
            noOrdre.Click += noOrdre_Click;

            filtreForm.Click += filtreForm_Click;
            filtreExcl.Click += filtreExcl_Click;
            filtreSel.Click += filtreSel_Click;
            treureFiltre.Click += treureFiltre_Click;
        }

        //mètode que donat un boolea subsriu o dessubscriu cada control de l'event keydown del formulari 
        private void subscriureEventKeyDown(bool valor)
        {
            foreach (Control c in this.Controls)
            {
                if (valor)
                    c.KeyDown += FrmModel_KeyDown;
                else
                    c.KeyDown -= FrmModel_KeyDown;

                if (c.HasChildren)//el control navegador te controls fills...
                    foreach (Control c2 in c.Controls)
                    {
                        if (valor)
                            c2.KeyDown += FrmModel_KeyDown;
                        else
                            c2.KeyDown -= FrmModel_KeyDown;
                    }
            }
        }

        //mètode que donat un formulari d'aquest tipus, per a cada textbox assigna com a tag el valor del text
        /// <summary>
        /// mètode que al formulari de tipus FrmModel passat per paràmetre li assigna com a Tag el valor del text
        /// </summary>
        /// <param name="form"></param>
        public static void actualitzarTag(FrmModel form)
        {
            foreach (Control c in form.Controls)
                if (form.esTextBox(c))
                    c.Tag = c.Text;
        }

        //mètode que donat un formulari d'aquest tipus, per a cada textbox assigna com a valor del text el valor del tag, sempre i quan no sigui null...
        /// <summary>
        /// mètode estàtic de la classe que donat un formulari FrmModel assigna a cada textbox el valor del seu Tag, sempre hi quan no sigui null
        /// </summary>
        /// <param name="form"></param>
        public static void retrocedirText(FrmModel form)
        {
            foreach (Control c in form.Controls)
                if (form.esTextBox(c) && c.Tag != null)
                    ((TextBox)c).Text = ((TextBox)c).Tag.ToString();
        }


        #region events botons_menus

        //cada un dels events click dels botons del navegador fa la mateixa funció que l'element equivalent del menú de navegació(performclick...)
        protected void fi_Click(object sender, EventArgs e)
        {
            nav.Ultim.PerformClick();
        }

        protected void inici_Click(object sender, EventArgs e)
        {
            nav.Primer.PerformClick();
        }

        protected void ant_Click(object sender, EventArgs e)
        {
            nav.Anterior.PerformClick();
        }

        protected void post_Click(object sender, EventArgs e)
        {
            nav.Seguent.PerformClick();
        }


        //al clickar el butó del menú de treure el filtre aquest s'elimina
        protected virtual void treureFiltre_Click(object sender, EventArgs e)
        {
            retrocedirText(this);//recuperem el valor del tag de cada textbox
            this.bindSource.RemoveFilter();//s'elimina el filtre del binding source
            nav.RegActual = this.bindSource.Position + 1;//s'actualitzen els valors del registre actual i total...
            this.nav.RegTotals = this.bindSource.Count;

            actualitzarTag(this);//s'assigna de nou el valor del text al valor del tag

            //per a cada textbox recuperem el color de fons original, ja que els que estaven sota el filtre estaven d'un color especial
            foreach (Control c in this.Controls)
                if (esTextBox(c))
                    ((TextBox)c).BackColor = Color.White;
        }

        //al clickar al butó de filtre per selecció s'assigna com a camp del filtre el camp del textbox on hi ha el cursor actualment
        protected virtual void filtreSel_Click(object sender, EventArgs e)
        {
            if (esTextBox(this.ActiveControl))//es comprova si el control actiu es textbox
            {
                foreach (Control c in this.Controls)//si és aixís, per a cada textbox se li retorna el color de fons original, en previsió de que ja hi hagués alguna classe de filtre seleccionat anteriorment
                    if (esTextBox(c))
                        ((TextBox)c).BackColor = Color.White;

                //s'obté el valor i el camp del textbox actual fent un substring del nom del textbox...
                string valor = ((TextBox)ActiveControl).Text;
                string camp = this.ActiveControl.Name.Replace("TextBox", "");

                retrocedirText(this);//es recupera el valor original del text

                //s'assigna el filtre amb el valor del camp actual(al convertirlo a string es pot filtrar també els camps numèrics)
                this.bindSource.Filter = "CONVERT(" + camp + ",'System.String') LIKE '%" + valor + "%'";

                nav.RegActual = this.bindSource.Position + 1;//s'actualitza el número de registre
                this.nav.RegTotals = this.bindSource.Count;
                ((TextBox)ActiveControl).BackColor = Color.Aquamarine;//s'assigna un color diferent al camp seleccionat per diferenciar-lo de la resta de camps

                actualitzarTag(this);//es torna a actualitzar el tag, ja que al filtrar es probable que s'hagi cambiat d'element en el bindingsource
            }
        }

        //mètode que segueix idèntic procés que en el filtre de selecció pero en aquest cas serà d'exclusió del camp seleccionat
        protected virtual void filtreExcl_Click(object sender, EventArgs e)
        {
            if (esTextBox(this.ActiveControl))
            {
                foreach (Control c in this.Controls)
                    if (esTextBox(c))
                        ((TextBox)c).BackColor = Color.White;

                string valor = ((TextBox)ActiveControl).Text;
                string camp = this.ActiveControl.Name.Replace("TextBox", "");

                retrocedirText(this);

                this.bindSource.Filter = "CONVERT(" + camp + ",'System.String') NOT LIKE '%" + valor + "%'";

                nav.RegActual = this.bindSource.Position + 1;
                this.nav.RegTotals = this.bindSource.Count;
                ((TextBox)ActiveControl).BackColor = Color.Aquamarine;

                actualitzarTag(this);
            }
        }

        //mètode que filtra el bindingsource per els camps de formulari que tenen text
        protected virtual void filtreForm_Click(object sender, EventArgs e)
        {
            string filtre = "";
            foreach (Control c in this.Controls)//es recorre cada control  del formulari que sigui textbox i que tingiu text en el moment actual
            {
                if (esTextBox(c) && c.Text.Length > 0)
                {
                    c.BackColor = Color.Aquamarine;
                    string camp = c.Name.Replace("TextBox", "");
                    string valor = c.Text;
                    filtre += "CONVERT(" + camp + ",'System.String') LIKE '%" + valor + "%' AND ";//es va concatenant una cadena de text que s'aplicarà al filtre llavors
                }
            }
            if (filtre.Length > 0)
            {
                filtre = filtre.Substring(0, filtre.Length - 5);//extrec els 5 ultims caracters de la cadena de text
                retrocedirText(this);//torno a assignar el valor de cada camp al del seu tag...  

                this.bindSource.Filter = filtre;//aplico el filtre

                //actualitzo el valor dels registres actual i total...
                nav.RegActual = this.bindSource.Position + 1;
                this.nav.RegTotals = this.bindSource.Count;

                actualitzarTag(this);
            }
        }


        //mètode de l'event click per ordenar de forma descendent
        protected virtual void Desc_Click(object sender, EventArgs e)
        {
            if (esTextBox(this.ActiveControl))
            {
                retrocedirText(this);
                this.bindSource.Sort = this.ActiveControl.Name.Replace("TextBox", "") + " DESC";
                actualitzarTag(this);
            }
        }

        //mètode de l'event click per ordenar de forma ascendent
        protected virtual void Asc_Click(object sender, EventArgs e)
        {
            if (esTextBox(this.ActiveControl))
            {
                retrocedirText(this);
                this.bindSource.Sort = this.ActiveControl.Name.Replace("TextBox", "") + " ASC";
                actualitzarTag(this);
            }
        }

        //mètode de l'event click de treure l'ordre; no hi ha cap criteri d'ordenació
        protected virtual void noOrdre_Click(object sender, EventArgs e)
        {
            retrocedirText(this);
            this.bindSource.RemoveSort();
            this.nav.RegActual = this.bindSource.Position + 1;
            actualitzarTag(this);
        }


        //mètode de l'event click de prémer el menú de navegació a l'ultim
        protected virtual void Ultim_Click(object sender, EventArgs e)
        {
            try
            {
                retrocedirText(this);//es recupera el valor del tag en l'element actual del binding source i s'avança fins a la última posició; s'actualitza el valor del registre actual
                this.bindSource.MoveLast();
                nav.RegActual = this.bindSource.Position + 1;
                actualitzarTag(this);
            }
            catch (Exception ex)
            { }
        }

        //desplaçar el bindingsource a l'element anterior de l'actual
        protected virtual void Anterior_Click(object sender, EventArgs e)
        {
            try
            {
                retrocedirText(this);
                this.bindSource.MovePrevious();
                nav.RegActual = this.bindSource.Position + 1;
                actualitzarTag(this);
            }
            catch (Exception ex)
            { }
        }

        //desplaçar el bindingsource a l'element posterior de l'actual
        protected virtual void Seguent_Click(object sender, EventArgs e)
        {
            try
            {
                retrocedirText(this);
                this.bindSource.MoveNext();
                nav.RegActual = this.bindSource.Position + 1;
                actualitzarTag(this);
            }
            catch (Exception ex)
            { }
        }

        //desplaçar el bindingsource al primer element de tots
        protected virtual void Primer_Click(object sender, EventArgs e)
        {
            try
            {
                retrocedirText(this);
                this.bindSource.MoveFirst();
                nav.RegActual = this.bindSource.Position + 1;
                actualitzarTag(this);
            }
            catch (Exception ex)
            { }
        }
        #endregion


        //event keydown del formulari; es comprova si s'ha pulsat una de les tecles de desplaçament i en funció de cada una es fa la mateixa acció que els butons del menú equivalents
        protected virtual void FrmModel_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.PageUp)
                {
                    retrocedirText(this);
                    this.bindSource.MovePrevious();
                    nav.RegActual = this.bindSource.Position + 1;
                    actualitzarTag(this);
                }
                else if (e.KeyCode == Keys.PageDown)
                {
                    retrocedirText(this);
                    this.bindSource.MoveNext();
                    nav.RegActual = this.bindSource.Position + 1;
                    actualitzarTag(this);
                }
                else if (e.KeyCode == Keys.Home && Control.ModifierKeys == Keys.Control)
                {
                    retrocedirText(this);
                    this.bindSource.MoveFirst();
                    nav.RegActual = this.bindSource.Position + 1;
                    actualitzarTag(this);
                }
                else if (e.KeyCode == Keys.End && Control.ModifierKeys == Keys.Control)
                {
                    retrocedirText(this);
                    this.bindSource.MoveLast();
                    nav.RegActual = this.bindSource.Position + 1;
                    actualitzarTag(this);
                }
            }
            catch (Exception ex)
            { }
        }

        //al tancar el formulari s'elimina el menú de navegació del menú del formulari pare
        protected void FrmModel_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.MdiParent.MainMenuStrip != null)
            {
                this.MdiParent.MainMenuStrip.Items.Remove(menu);
            }
        }

    }
}
