using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Online_Doktor_Randevu_Sistemi
{
  
    public class Hasta
    {
        [DisplayName("Hasta ID")]
        public int hasta_id { get; set; }

        [DisplayName("Ad Soyad")]
        public string ad { get; set; }

        [DisplayName("TC Kimlik Numarası")]
        public string tc { get; set; }

        [DisplayName("Telefon")]
        public string telefon { get; set; }

        public Randevu randevu_al(Doktor doktor, string saat, int islemId)
        {
            if (doktor.uygunluk_kontrol(saat))
            {
                doktor.uygun_saatler.Remove(saat);

                return new Randevu
                {
                    randevu_id = islemId,
                    hasta_ad = this.ad,
                    doktor_ad = doktor.ad,
                    uzmanlik = doktor.uzmanlik,
                    saat = saat
                };
            }
            return null;
        }

        public override string ToString() => $"{ad} ({tc})";
    }

   
    public class Doktor
    {
        [DisplayName("Doktor ID")]
        public int doktor_id { get; set; }

        [DisplayName("Ad Soyad")]
        public string ad { get; set; }

        [DisplayName("Uzmanlık Alanı")]
        public string uzmanlik { get; set; }

        [Browsable(false)]
        public List<string> uygun_saatler { get; set; } = new List<string>();

        public bool uygunluk_kontrol(string saat)
        {
            return uygun_saatler.Contains(saat);
        }

        public override string ToString() => $"{ad} - {uzmanlik}";
    }

    
    public class Randevu
    {
        [DisplayName("Kayıt Numarası")]
        public int randevu_id { get; set; }

        [DisplayName("Hasta Adı")]
        public string hasta_ad { get; set; }

        [DisplayName("Doktor Adı")]
        public string doktor_ad { get; set; }

        [DisplayName("Bölüm")]
        public string uzmanlik { get; set; }

        [DisplayName("Randevu Saati")]
        public string saat { get; set; }
    }

    public partial class Form1 : Form
    {
        List<Hasta> hastalar = new List<Hasta>();
        List<Doktor> doktorlar = new List<Doktor>();
        List<Randevu> randevular = new List<Randevu>();

        int hastaSayac = 1001;
        int randevuSayac = 50001;

        TabControl sekmeler;
        TabPage sekmeDoktorlar, sekmeHastalar, sekmeRandevu;
        DataGridView dgvDoktorlar, dgvHastalar, dgvRandevular;
        ComboBox cmbHastaSec, cmbDoktorSec, cmbSaatSec;
        TextBox txtHastaAd, txtHastaTc, txtHastaTel;

        public Form1()
        {
            this.Text = "Merkezi Hekim Randevu Sistemi - 2300005412 Fırat Diricanlı";
            this.Size = new Size(1150, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            SistemVerileriniHazirla();
            ArayuzuInsaEt();
        }

        private void SistemVerileriniHazirla()
        {
            
            doktorlar.Add(new Doktor { doktor_id = 1, ad = "Dr. Erdem Yücesan", uzmanlik = "Nöroloji", uygun_saatler = new List<string> { "09:00", "10:15", "13:30", "15:45" } });
            doktorlar.Add(new Doktor { doktor_id = 2, ad = "Dr. Ahmet Yılmaz", uzmanlik = "Kardiyoloji", uygun_saatler = new List<string> { "09:30", "11:00", "14:00" } });
            doktorlar.Add(new Doktor { doktor_id = 3, ad = "Dr. Ayşe Kaya", uzmanlik = "Dahiliye", uygun_saatler = new List<string> { "08:30", "10:00", "13:00", "16:00" } });
            doktorlar.Add(new Doktor { doktor_id = 4, ad = "Dr. Mehmet Demir", uzmanlik = "Ortopedi", uygun_saatler = new List<string> { "10:30", "14:30", "16:30" } });
            doktorlar.Add(new Doktor { doktor_id = 5, ad = "Dr. Elif Şahin", uzmanlik = "Göz Hastalıkları", uygun_saatler = new List<string> { "09:15", "11:45", "15:15" } });
            doktorlar.Add(new Doktor { doktor_id = 6, ad = "Dr. Zeynep Çelik", uzmanlik = "Cildiye", uygun_saatler = new List<string> { "10:45", "13:15", "15:00" } });
            doktorlar.Add(new Doktor { doktor_id = 7, ad = "Dr. Caner Yıldız", uzmanlik = "Genel Cerrahi", uygun_saatler = new List<string> { "08:00", "09:45", "14:15" } });
            doktorlar.Add(new Doktor { doktor_id = 8, ad = "Dr. Burcu Aslan", uzmanlik = "Psikiyatri", uygun_saatler = new List<string> { "11:30", "14:45", "16:15" } });

            hastalar.Add(new Hasta { hasta_id = 1000, ad = "Fırat Diricanlı", tc = "12345678901", telefon = "0555 123 45 67" });
        }

        private void ArayuzuInsaEt()
        {
            sekmeler = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

            sekmeDoktorlar = new TabPage("Poliklinik ve Doktorlar");
            sekmeHastalar = new TabPage("Hasta Kayıt Birimi");
            sekmeRandevu = new TabPage("Randevu İşlemleri");

            dgvDoktorlar = TabloOlustur();
            sekmeDoktorlar.Controls.Add(dgvDoktorlar);

            Panel pnlHasta = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Color.WhiteSmoke };
            txtHastaAd = new TextBox { Location = new Point(20, 30), Width = 180, PlaceholderText = "Hasta Ad Soyad" };
            txtHastaTc = new TextBox { Location = new Point(210, 30), Width = 150, PlaceholderText = "TC Kimlik No", MaxLength = 11 };
            txtHastaTel = new TextBox { Location = new Point(370, 30), Width = 150, PlaceholderText = "Telefon Numarası" };
            Button btnHastaEkle = new Button { Text = "SİSTEME KAYDET", Location = new Point(530, 28), Size = new Size(180, 32), BackColor = Color.SteelBlue, ForeColor = Color.White };
            btnHastaEkle.Click += (s, e) => SistemeHastaKaydet();

            dgvHastalar = TabloOlustur();
            pnlHasta.Controls.AddRange(new Control[] { txtHastaAd, txtHastaTc, txtHastaTel, btnHastaEkle });
            sekmeHastalar.Controls.Add(dgvHastalar);
            sekmeHastalar.Controls.Add(pnlHasta);

            Panel pnlRandevu = new Panel { Dock = DockStyle.Top, Height = 120, BackColor = Color.WhiteSmoke };

            Label l1 = new Label { Text = "Hasta Seçimi:", Location = new Point(20, 20), AutoSize = true };
            cmbHastaSec = new ComboBox { Location = new Point(140, 18), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            Label l2 = new Label { Text = "Doktor Seçimi:", Location = new Point(20, 60), AutoSize = true };
            cmbDoktorSec = new ComboBox { Location = new Point(140, 58), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbDoktorSec.SelectedIndexChanged += DoktorDegisinceSaatleriGetir;

            Label l3 = new Label { Text = "Uygun Saatler:", Location = new Point(410, 60), AutoSize = true };
            cmbSaatSec = new ComboBox { Location = new Point(530, 58), Width = 120, DropDownStyle = ComboBoxStyle.DropDownList };

            Button btnRandevuAl = new Button { Text = "RANDEVU OLUŞTUR", Location = new Point(680, 55), Size = new Size(200, 35), BackColor = Color.SeaGreen, ForeColor = Color.White };
            btnRandevuAl.Click += (s, e) => RandevuIsleminiGerceklestir();

            dgvRandevular = TabloOlustur();

            pnlRandevu.Controls.AddRange(new Control[] { l1, cmbHastaSec, l2, cmbDoktorSec, l3, cmbSaatSec, btnRandevuAl });
            sekmeRandevu.Controls.Add(dgvRandevular);
            sekmeRandevu.Controls.Add(pnlRandevu);

            sekmeler.TabPages.AddRange(new TabPage[] { sekmeDoktorlar, sekmeHastalar, sekmeRandevu });
            this.Controls.Add(sekmeler);

            TablolariGuncelle();
        }

        private DataGridView TabloOlustur()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
        }

        private void TablolariGuncelle()
        {
            dgvDoktorlar.DataSource = null; dgvDoktorlar.DataSource = doktorlar.ToList();
            dgvHastalar.DataSource = null; dgvHastalar.DataSource = hastalar.ToList();
            dgvRandevular.DataSource = null; dgvRandevular.DataSource = randevular.ToList();

            cmbHastaSec.Items.Clear();
            cmbDoktorSec.Items.Clear();
            foreach (var h in hastalar) cmbHastaSec.Items.Add(h);
            foreach (var d in doktorlar) cmbDoktorSec.Items.Add(d);

            if (cmbDoktorSec.Items.Count > 0) cmbDoktorSec.SelectedIndex = 0;
        }

        private void DoktorDegisinceSaatleriGetir(object sender, EventArgs e)
        {
            cmbSaatSec.Items.Clear();
            if (cmbDoktorSec.SelectedItem is Doktor seciliDoktor)
            {
                foreach (var saat in seciliDoktor.uygun_saatler)
                {
                    cmbSaatSec.Items.Add(saat);
                }
                if (cmbSaatSec.Items.Count > 0) cmbSaatSec.SelectedIndex = 0;
                else cmbSaatSec.Items.Add("Dolu");
            }
        }

        private void SistemeHastaKaydet()
        {
            if (!string.IsNullOrWhiteSpace(txtHastaAd.Text) && !string.IsNullOrWhiteSpace(txtHastaTc.Text))
            {
                hastalar.Add(new Hasta
                {
                    hasta_id = hastaSayac++,
                    ad = txtHastaAd.Text,
                    tc = txtHastaTc.Text,
                    telefon = txtHastaTel.Text
                });
                txtHastaAd.Clear(); txtHastaTc.Clear(); txtHastaTel.Clear();
                TablolariGuncelle();
            }
            else
            {
                MessageBox.Show("Lütfen hasta adı ve TC kimlik numarası alanlarını doldurunuz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RandevuIsleminiGerceklestir()
        {
            if (cmbHastaSec.SelectedItem is Hasta seciliHasta && cmbDoktorSec.SelectedItem is Doktor seciliDoktor && cmbSaatSec.SelectedItem != null)
            {
                string seciliSaat = cmbSaatSec.SelectedItem.ToString();

                if (seciliSaat == "Dolu")
                {
                    MessageBox.Show("Seçili hekimin uygun randevu saati bulunmamaktadır.", "İşlem Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Randevu yeniRandevu = seciliHasta.randevu_al(seciliDoktor, seciliSaat, randevuSayac++);

                if (yeniRandevu != null)
                {
                    randevular.Add(yeniRandevu);
                    TablolariGuncelle();
                    MessageBox.Show("Randevu işleminiz başarıyla oluşturulmuştur.", "Sistem Mesajı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Lütfen randevu işlemi için hasta, doktor ve saat seçimini eksiksiz yapınız.", "Hatalı İşlem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}