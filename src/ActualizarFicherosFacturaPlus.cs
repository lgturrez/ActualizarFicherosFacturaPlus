using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Configuration;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        List<string> lsFileList = new List<string>();
        private string driver;
        private Boolean blnSimular = false;
        private Boolean blnPreguntar = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void ejecutar()
        {
            if (!blnPreguntar || MessageBox.Show("Se van a sobreescribir los ficheros de FacturaPlus. " + Environment.NewLine + "Asegúrate de haber hecho una copia de seguridad. " + Environment.NewLine + "¿Continuar?", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                foreach (string fichero in lsFileList)
                {
                    /*if (driver.Equals("MSOLEDB")){
                        UpdateData(txtRuta.Text, fichero, ConfigurationSettings.AppSettings[fichero + "_PK"]);
                    }else
                    {
                        UpdateData1(txtRuta.Text, fichero, ConfigurationSettings.AppSettings[fichero + "_PK"]);
                    }*/
                    UpdateData1(txtRuta.Text, fichero, ConfigurationSettings.AppSettings[fichero + "_PK"]);
                }
                toolStripStatusLabel1.Text = "Finalizado";
            }else
            {
                toolStripStatusLabel1.Text = "Proceso cancelado";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            lblResultado.Text = "";
            blnPreguntar = true;
            blnSimular = false;
            ejecutar();
            
        }
        
        public Boolean UpdateData(String dbLocation, String tablename, String PK)
        {
            OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbLocation + ";Extended Properties=dBASE IV;User ID=;Password=;"); // give your path directly 
            try
            {
                int conta = 0;
                if (PK == null) PK = ConfigurationSettings.AppSettings["default_PK"];
                con.Open();
                toolStripStatusLabel1.Text = "Actualizando tabla " + tablename;

                OleDbDataAdapter da = new OleDbDataAdapter("select * from " + tablename + " order by " + PK , con); // update this query with your table name 

                int contador = 1;

                DataSet ds = new DataSet();
                da.Fill(ds);

                DataRow fila0 = ds.Tables[0].Rows[0];

                int totalRows = ds.Tables[0].Rows.Count;

                List<String> arrColumnasFecha = new List<String>();
                foreach (DataColumn column in ds.Tables[0].Columns)
                {
                    if (fila0[column.ColumnName.ToString()].GetType() == typeof(DateTime))
                    {
                        arrColumnasFecha.Add(column.ColumnName.ToString());
                    }
                }

                foreach (DataRow  item in ds.Tables[0].Rows)
                {
                    toolStripStatusLabel1.Text = "Actualizando tabla " + tablename + " - Registro " + contador + " de " + totalRows;
                    statusStrip1.Refresh();
                    Boolean blnCambiado = false;
                    foreach (String columna in arrColumnasFecha)
                    {
                        DateTime temp = (DateTime)item[columna];
                        if (temp.Year == 1920)
                        {
                            item[columna] = temp.AddYears(100);
                            OleDbCommand cmd = new OleDbCommand("UPDATE " + tablename + " SET " + columna + " = '" + ((DateTime)item[columna]).ToString("MM/dd/yyyy") + "' WHERE " + PK + " = " + item[PK] + ";", con);
                            cmd.ExecuteNonQuery();
                            blnCambiado = true;
                            
                        }
                    }
                    if (blnCambiado) conta++;
                    contador++;
                        
                }
                
                con.Close();
                lblResultado.Text = lblResultado.Text + Environment.NewLine + tablename + ": " + conta + " registros actualizados de " + totalRows;
                lblResultado.Refresh();
                conta = 0;
                toolStripStatusLabel1.Text = "Finalizado";

                return true;
            }
            catch (Exception e)
            {
                var error = e.ToString();
                // check error details 
                return false;
            }
        }

        public Boolean UpdateData1(String dbLocation, String tablename, String PK)
        {
            OleDbConnection con;
            try
            {

                if (driver.Equals("MSOLEDB"))
                {
                    con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + dbLocation + ";Extended Properties=dBASE IV;User ID=;Password=;"); // give your path directly 
                    con.Open();
                }
                else
                {
                    con = new OleDbConnection(@"Provider=VFPOLEDB;Data Source=" + dbLocation + ";Extended Properties=dBASE IV;User ID=;Password=;"); // give your path directly 
                    con.Open();
                    OleDbCommand cmd = new OleDbCommand("SET DELETED OFF", con);
                    cmd.ExecuteNonQuery();
                }

                int conta = 0;
                if (PK == null) PK = ConfigurationSettings.AppSettings["default_PK"];
                
                toolStripStatusLabel1.Text = "Actualizando tabla " + tablename;

                OleDbDataAdapter da = new OleDbDataAdapter("select * from " + tablename  + " order by " + PK, con); // update this query with your table name 
                //OleDbDataAdapter da = new OleDbDataAdapter("select * from " + tablename +" where " + PK + "= 60 order by " + PK, con); // update this query with your table name 

                int contador = 1;

                DataSet ds = new DataSet();
                da.Fill(ds);

                DataRow fila0 = ds.Tables[0].Rows[0];

                int totalRows = ds.Tables[0].Rows.Count;

                List<String> arrColumnasFecha = new List<String>();
                foreach (DataColumn column in ds.Tables[0].Columns)
                {
                    if (fila0[column.ColumnName.ToString()].GetType() == typeof(DateTime))
                    {
                        arrColumnasFecha.Add(column.ColumnName.ToString());
                    }
                }

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    toolStripStatusLabel1.Text = "Actualizando tabla " + tablename + " - Registro " + contador + " de " + totalRows;
                    statusStrip1.Refresh();
                    Boolean blnCambiado = false;
                    foreach (String columna in arrColumnasFecha)
                    {
                        DateTime temp = (DateTime)item[columna];
                        if (temp.Year >= 1920 && temp.Year <= 1930)
                        {
                            try
                            {
                                item[columna] = temp.AddYears(100);
                                string sentenciaSQL;
                                if (driver.Equals("MSOLEDB"))
                                {
                                    sentenciaSQL = "UPDATE " + tablename + " SET " + columna + " = '" + ((DateTime)item[columna]).ToString("dd/MM/yyyy") + "' WHERE " + PK + " = " + item[PK] + ";"; 
                                }else
                                {
                                    sentenciaSQL = "UPDATE " + tablename + " SET " + columna + " = {" + ((DateTime)item[columna]).ToString("MM/dd/yyyy") + "} WHERE " + PK + " = " + item[PK] + ";";
                                }
                                OleDbCommand cmd = new OleDbCommand(sentenciaSQL, con);
                                if (!blnSimular)
                                {
                                    int rows = cmd.ExecuteNonQuery();
                                }
                                //Console.WriteLine(sentenciaSQL + " - Registros: " + rows);
                                blnCambiado = true;
                            }
                            catch (Exception e)
                            {
                                var error = e.ToString();
                                Console.WriteLine (error);
                                
                            }

                        }else
                        {
                            //Console.WriteLine("No es 1920");
                        }
                    }
                    if (blnCambiado) conta++;
                    contador++;

                }
                
                con.Close();
                lblResultado.Text = lblResultado.Text + Environment.NewLine + tablename + ": " + conta + " registros actualizados de " + totalRows;
                lblResultado.Refresh();
                conta = 0;
                toolStripStatusLabel1.Text = "Finalizado";

                return true;
            }
            catch (Exception e)
            {
                var error = e.ToString();
                // check error details 
                return false;
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog();
        }

        private void OpenFileDialog()
        {
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            folderBrowserDialog1.SelectedPath = txtRuta.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtRuta.Text = folderBrowserDialog1.SelectedPath;
                CargarFicheros();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            Boolean blnAutorun = false;

            foreach (string arg in args)
            {
                if (arg.Equals ("autorun")){
                    blnAutorun = true;        
                }
            }

            txtRuta.Text = ConfigurationSettings.AppSettings["carpetaInicial"];  //System.Configuration.ConfigurationSettings.AppSettings["carpetaInicial"];
            CargarFicheros();
            driver = ConfigurationSettings.AppSettings["driver"];
            if (blnAutorun)
            {
                blnPreguntar = false;
                ejecutar();
                Application.Exit();
            }
        }

        private static string[] GetFileNames(string path, string filter)
        {
            string[] files = Directory.GetFiles(path, filter,SearchOption.TopDirectoryOnly);
            for (int i = 0; i < files.Length; i++)
                files[i] = Path.GetFileName(files[i]);
            return files;
        }

        private void CargarFicheros()
        {
            if (Directory.Exists(txtRuta.Text)){
                string ficheros = ConfigurationSettings.AppSettings["Ficheros"];
                if (ficheros.Equals("*"))
                {
                    lsFileList = new List<string>(GetFileNames(txtRuta.Text, "*.dbf"));
                }
                else
                {
                    lsFileList = new List<string>(ficheros.Split('#'));
                }
                if (lsFileList.Count == 0)
                {
                    MessageBox.Show("No hay ficheros .dbf en el directorio", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnUpdate.Enabled = false;
                }
                else
                {
                    btnUpdate.Enabled = true;
                }
                                
            }
            else
            {
                MessageBox.Show("El directorio no existe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OpenFileDialog();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            blnSimular = true;
            blnPreguntar = false;
            lblResultado.Text = "ESTO ES UNA SIMULACION, LOS FICHEROS NO SE MODIFICAN";
            ejecutar();
            blnSimular = false;
        }
    }
}
