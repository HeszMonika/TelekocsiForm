using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TelekocsiForm
{
    public partial class FrmFo : Form
    {
        private List<Auto> Autok = new List<Auto>();
        private List<Igeny> Igenyek = new List<Igeny>();

        public FrmFo()
        {
            InitializeComponent();
            lbKimenet.Items.Clear();
        }

        private void btnBeolvasas_Click(object sender, EventArgs e)
        {
            try
            {
                StreamReader sr = new StreamReader("autok.csv");
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    Autok.Add(new Auto(sr.ReadLine()));
                }
                sr.Close();

                StreamReader file = new StreamReader("igenyek.csv");
                file.ReadLine();
                while (!file.EndOfStream)
                {
                    Igenyek.Add(new Igeny(file.ReadLine()));
                }
                file.Close();


                btnSecond.Enabled = true;
                btnBeolvasas.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSecond_Click(object sender, EventArgs e)
        {
            lbKimenet.Items.Add("2. feladat: ");
            lbKimenet.Items.Add($"   {Autok.Count} autós hirdet fuvart.");
            btnSecond.Enabled = false;
            btnThird.Enabled = true;
        }

        private void btnThird_Click(object sender, EventArgs e)
        {
            lbKimenet.Items.Clear();
            lbKimenet.Items.Add("3. feladat: ");
            int db = 0;
            foreach (var a in Autok)
            {
                if (a.Utvonal == "Budapest-Miskolc")
                {
                    db = db + a.Ferohely;
                }
            }
            lbKimenet.Items.Add($"   Összesen {db} férőhelyet hirdettek az autósok Budapestről Miskolcra.");
            btnThird.Enabled = false;
            btnFourth.Enabled = true;
        }

        private void btnFourth_Click(object sender, EventArgs e)
        {
            lbKimenet.Items.Clear();
            lbKimenet.Items.Add("4. feladat: ");
            int max = 0;
            string utvonal = "";
            var utvonalak = from a in Autok orderby a.Utvonal group a by a.Utvonal into temp select temp;

            foreach (var ut in utvonalak)
            {
                //Console.WriteLine($"{ut.Key} -> {ut.Count()}");
                int fh = ut.Sum(x => x.Ferohely);
                if (max < fh)
                {
                    max = fh;
                    utvonal = ut.Key;
                }
            }
            lbKimenet.Items.Add($"   {max} - {utvonal}");
            btnFourth.Enabled = false;
            btnFifth.Enabled = true;
        }

        private void btnFifth_Click(object sender, EventArgs e)
        {
            lbKimenet.Items.Clear();
            lbKimenet.Items.Add("5. feladat: ");
            foreach (var igeny in Igenyek)
            {
                int i = igeny.VanAuto(Autok);

                if (i > -1)
                {
                    lbKimenet.Items.Add($"{igeny.Azonosito} => {Autok[i].Rendszam}");
                }
            }
            btnFifth.Enabled = false;
            btnSixth.Enabled = true;
        }

        private void btnSixth_Click(object sender, EventArgs e)
        {
            try
            {
                lbKimenet.Items.Clear();
                lbKimenet.Items.Add("6. feladat: ");
                StreamWriter sw = new StreamWriter("utasuzenetek.txt");
                foreach (var igeny in Igenyek)
                {
                    int i = igeny.VanAuto(Autok);
                    if (i > -1)
                    {
                        sw.WriteLine($"{igeny.Azonosito}: Rendszám: {Autok[i].Rendszam}, Telefonszám: {Autok[i].Telefonszam}");
                    }
                    else
                    {
                        sw.WriteLine($"{igeny.Azonosito}: Sajnos nem sikerült autót találni.");
                    }
                }
                sw.Close();
                lbKimenet.Items.Add("Az adatok fájlba írása megtörtént.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnKilepes_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
