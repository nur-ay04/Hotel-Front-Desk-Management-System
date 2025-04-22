using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.Entity;
using Projem_Otel.Entity;
using System.Data.Common;
using System.Windows.Forms.VisualStyles;

namespace Projem_Otel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Server=LAPTOP-QL9SNOH8\\SQLEXPRESS;Database=oteldb;Trusted_Connection=True;");
        oteldbEntities2 db = new oteldbEntities2();
        private void Form1_Load(object sender, EventArgs e)
        {
            GuncelleBosOdaSayisi();
            ListeleMusteriler();
            dataGridView1.Columns["SehirID"].Visible = false;
            dataGridView1.Columns["OdaTuruID"].Visible = false;
            dataGridView1.Columns["DurumID"].Visible = false;

            SqlCommand cmd = new SqlCommand("SELECT DISTINCT OdaTuru FROM Odalar", baglanti);
            baglanti.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string odaTuru = reader["OdaTuru"].ToString();
                cbOdaTuru.Text = odaTuru;
            }
            reader.Close();
            baglanti.Close();

            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("select * from Durum", baglanti);
            SqlDataAdapter da3 = new SqlDataAdapter(komut3);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            cbMusteriDurumu.DisplayMember = "Durum1";
            cbMusteriDurumu.ValueMember = "DurumID";
            cbMusteriDurumu.DataSource = dt3;
            baglanti.Close();

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from Sehirler", baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbGelinenSehir.DisplayMember = "SehirAd";
            cbGelinenSehir.ValueMember = "SehirID";
            cbGelinenSehir.DataSource = dt;
            baglanti.Close();

            baglanti.Open();
            SqlCommand komut2 = new SqlCommand("select * from OdaTurleri", baglanti);
            SqlDataAdapter da2 = new SqlDataAdapter(komut2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            cbOdaTuru.DisplayMember = "OdaTuru";
            cbOdaTuru.ValueMember = "OdaTuruID";
            cbOdaTuru.DataSource = dt2;
            baglanti.Close();
        }
        private void ListeleMusteriler()
        {
            var degerler = from x in db.Musteriler
                           select new
                           {
                               x.TC,
                               x.Ad,
                               x.Soyad,
                               x.Sehirler.SehirAd,
                               x.GelisTarihi,
                               x.CikisTarihi,
                               x.DogumTarihi,
                               x.Telefon,
                               x.OdaTurleri.OdaTuru,
                               x.Sehirler.SehirID,
                               x.OdaTurleri.OdaTuruID,
                               x.DurumID,
                               x.Durum.Durum1
                           };
            if (cbGelinenSehir.SelectedIndex != -1)
            {
                int d = int.Parse(cbGelinenSehir.SelectedValue.ToString());
                degerler = degerler.Where(y => y.SehirID == d);
            }

            if (cbOdaTuru.SelectedIndex != -1)
            {
                int e = int.Parse(cbOdaTuru.SelectedValue.ToString());
                degerler = degerler.Where(y => y.OdaTuruID == e);
            }
            dataGridView1.DataSource = degerler.ToList();
        }
        private void GuncelleBosOdaSayisi()
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            SqlCommand oda2KisilikKontrol = new SqlCommand("SELECT COUNT(OdaID) FROM Odalar WHERE OdaTuruID = 1 AND Dolu = '0'", baglanti);
            int oda2KisilikSayisi = (int)oda2KisilikKontrol.ExecuteScalar();
            lblOdaSayisi.Text = $"2 Kişilik Oda: {oda2KisilikSayisi}";
            baglanti.Close();

            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            SqlCommand oda4KisilikKontrol = new SqlCommand("SELECT COUNT(OdaID) FROM Odalar WHERE OdaTuruID = 2 AND Dolu = '0'", baglanti);
            int oda4KisilikSayisi = (int)oda4KisilikKontrol.ExecuteScalar();
            lblOdaSayisi2.Text = $"4 Kişilik Oda: {oda4KisilikSayisi}";
            baglanti.Close();

            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
            SqlCommand odaVIPKontrol = new SqlCommand("SELECT COUNT(OdaID) FROM Odalar WHERE OdaTuruID = 3 AND Dolu = '0'", baglanti);
            int odaVIPSayisi = (int)odaVIPKontrol.ExecuteScalar();
            lblOdaSayisi3.Text = $"VIP Oda: {odaVIPSayisi}";
            baglanti.Close();
        }
        private void OdaDurumuGuncelle(int odaTuruID, int durumID)
        {
            try
            {
                baglanti.Open();

                string query = @"
                UPDATE Odalar 
                SET Dolu = CASE 
                    WHEN @DurumID = 2 THEN 0 
                    ELSE 1 
                END
                WHERE OdaID = (
                    SELECT TOP 1 OdaID 
                    FROM Odalar 
                    WHERE OdaTuruID = @OdaTuruID AND Dolu = 1
                )";

                using (SqlCommand odasayisiguncelle = new SqlCommand(query, baglanti))
                {
                    //odasayisiguncelle.Parameters.AddWithValue("@DurumID", cbMusteriDurumu.SelectedValue);
                    //odasayisiguncelle.Parameters.AddWithValue("@OdaTuruID", cbOdaTuru.SelectedValue);
                    odasayisiguncelle.Parameters.AddWithValue("@DurumID", durumID);
                    odasayisiguncelle.Parameters.AddWithValue("@OdaTuruID", odaTuruID);

                    int affectedRows = odasayisiguncelle.ExecuteNonQuery();

                    if (affectedRows > 0)
                    {
                        MessageBox.Show("Oda durumu başarıyla güncellendi.");
                        GuncelleBosOdaSayisi();
                    }
                    else
                    {
                        MessageBox.Show("Güncellenecek oda bulunamadı.");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message);
            }
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(mtTC.Text) ||
                string.IsNullOrWhiteSpace(TxtAd.Text) ||
                string.IsNullOrWhiteSpace(TxtSoyad.Text) ||
                string.IsNullOrWhiteSpace(mtGelisTarihi.Text) ||
                string.IsNullOrWhiteSpace(mtCikisTarihi.Text) ||
                string.IsNullOrWhiteSpace(mtDogumTarihi.Text) ||
                string.IsNullOrWhiteSpace(mtTelefonNumarası.Text) ||
                cbOdaTuru.SelectedIndex == -1 || cbGelinenSehir.SelectedItem == null)
            {
                MessageBox.Show("Lütfen tüm alanları doldurduğunuzdan emin olun.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            baglanti.Open();

            SqlCommand odaKontrol = new SqlCommand("SELECT COUNT(OdaID) FROM Odalar WHERE OdaTuruID = @OdaTuruID AND Dolu = '0'", baglanti);
            odaKontrol.Parameters.AddWithValue("@OdaTuruID", cbOdaTuru.SelectedValue.ToString());

            int bosOdaSayisi = (int)odaKontrol.ExecuteScalar();

            if (bosOdaSayisi == 0)
            {
                MessageBox.Show("Seçilen oda türünde boş oda yok!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            SqlCommand odaGuncelle = new SqlCommand("UPDATE Odalar SET Dolu = 1 WHERE OdaID = (SELECT TOP 1 OdaID FROM Odalar WHERE OdaTuruID = @OdaTuruID AND Dolu = 0)", baglanti);
            odaGuncelle.Parameters.AddWithValue("@OdaTuruID", cbOdaTuru.SelectedValue.ToString());
            odaGuncelle.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("Oda başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);


            baglanti.Open();
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Musteriler (TC, Ad, Soyad, SehirID, GelisTarihi, CikisTarihi, DogumTarihi, Telefon, OdaTuruID, DurumID) VALUES (@TC, @Ad, @Soyad, @GelinenSehir, @GelisTarihi, @CikisTarihi, @DogumTarihi, @Telefon, @OdaTuruID, @durum)", baglanti);
            cmd.Parameters.AddWithValue("@TC", mtTC.Text);
            cmd.Parameters.AddWithValue("@Ad", TxtAd.Text);
            cmd.Parameters.AddWithValue("@Soyad", TxtSoyad.Text);
            cmd.Parameters.AddWithValue("@GelinenSehir", cbGelinenSehir.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@GelisTarihi", DateTime.Parse(mtGelisTarihi.Text));
            cmd.Parameters.AddWithValue("@CikisTarihi", DateTime.Parse(mtCikisTarihi.Text));
            cmd.Parameters.AddWithValue("@DogumTarihi", DateTime.Parse(mtDogumTarihi.Text));
            cmd.Parameters.AddWithValue("@Telefon", mtTelefonNumarası.Text);
            cmd.Parameters.AddWithValue("@OdaTuruID", cbOdaTuru.SelectedValue.ToString());
            cmd.Parameters.AddWithValue("@durum", cbMusteriDurumu.SelectedValue.ToString());
            cmd.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("Müşteri başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Temizle();
            ListeleMusteriler();
            GuncelleBosOdaSayisi();

        }
        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update Musteriler set Ad = @ad, Soyad = @soyad, SehirID= @sehirid, GelisTarihi =@gelistarihi, CikisTarihi = @cikistarihi, DogumTarihi = @dogumtarihi, Telefon = @telefon, OdaTuruID = @odaturu, DurumID= @durum where TC= @tc", baglanti);
            komut.Parameters.AddWithValue("@tc", mtTC.Text);
            komut.Parameters.AddWithValue("@ad", TxtAd.Text);
            komut.Parameters.AddWithValue("@soyad", TxtSoyad.Text);
            komut.Parameters.AddWithValue("@sehirid", cbGelinenSehir.SelectedValue.ToString());
            komut.Parameters.AddWithValue("@gelistarihi", DateTime.Parse(mtGelisTarihi.Text));
            komut.Parameters.AddWithValue("@cikistarihi", DateTime.Parse(mtCikisTarihi.Text));
            komut.Parameters.AddWithValue("@dogumtarihi", DateTime.Parse(mtDogumTarihi.Text));
            komut.Parameters.AddWithValue("@telefon", mtTelefonNumarası.Text);
            komut.Parameters.AddWithValue("@odaturu", cbOdaTuru.SelectedValue);
            komut.Parameters.AddWithValue("@durum", cbMusteriDurumu.SelectedValue.ToString());
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("müşteri başarılı bir şekilde güncellendi.");
            int secilenOdaTuruID = Convert.ToInt32(cbOdaTuru.SelectedValue);
            int secilenDurumID = Convert.ToInt32(cbMusteriDurumu.SelectedValue);
            OdaDurumuGuncelle(secilenOdaTuruID, secilenDurumID);
            GuncelleBosOdaSayisi();
            ListeleMusteriler();
            Temizle();
        }
        private void Temizle()
        {
            mtTC.Clear();
            TxtAd.Clear();
            TxtSoyad.Clear();
            cbGelinenSehir.SelectedIndex = -1;
            cbOdaTuru.SelectedIndex = -1;
            cbMusteriDurumu.SelectedIndex = -1;
            mtGelisTarihi.Clear();
            mtCikisTarihi.Clear();
            mtDogumTarihi.Clear();
            mtTelefonNumarası.Clear();
        }
        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                mtTC.Text = selectedRow.Cells["TC"].Value.ToString();
                TxtAd.Text = selectedRow.Cells["Ad"].Value.ToString();
                TxtSoyad.Text = selectedRow.Cells["Soyad"].Value.ToString();
                mtGelisTarihi.Text = selectedRow.Cells["GelisTarihi"].Value.ToString();
                mtCikisTarihi.Text = selectedRow.Cells["CikisTarihi"].Value.ToString();
                mtDogumTarihi.Text = selectedRow.Cells["DogumTarihi"].Value.ToString();
                mtTelefonNumarası.Text = selectedRow.Cells["Telefon"].Value.ToString();

                cbGelinenSehir.SelectedValue = selectedRow.Cells["SehirID"].Value.ToString();
                cbOdaTuru.SelectedValue = selectedRow.Cells["OdaTuruID"].Value.ToString();
                cbMusteriDurumu.SelectedValue = selectedRow.Cells["DurumID"].Value.ToString();

            }
        }
        private void BtnAra_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter(@"SELECT 
                    m.TC, 
                    m.Ad, 
                    m.Soyad, 
                    m.SehirID, 
                    m.GelisTarihi, 
                    m.CikisTarihi, 
                    m.DogumTarihi, 
                    m.Telefon, 
                    o.OdaTuruID, 
                    d.DurumID,
                    o.OdaTuru,
                    d.Durum1
                FROM Musteriler m
                LEFT JOIN OdaTurleri o ON m.OdaTuruID = o.OdaTuruID
                LEFT JOIN Durum d ON m.DurumID = d.DurumID
                WHERE m.TC = @TC", baglanti);
            dataGridView1.Columns["DurumID"].Visible = false;
            dataGridView1.Columns["OdaTuruID"].Visible = false;

            da.SelectCommand.Parameters.AddWithValue("@TC", mtTC.Text);

            DataTable dt = new DataTable();
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Bu TC numarasına ait veri bulunamadı.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView1.DataSource = null;
            }
        }

        private void BtnListele_Click(object sender, EventArgs e)
        {
            ListeleMusteriler();
        }
    }
}



