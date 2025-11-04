using System;
using System.Collections.Generic;

namespace Gudang
{
    public abstract class Barang
    {
        private int kode;
        private string nama;
        private int stok;

        public int Kode { get { return kode; } }
        public string Nama { get { return nama; } set { nama = value; } }
        public int Stok { get { return stok; } }

        public Barang(int kode, string nama, int stok)
        {
            this.kode = kode;
            this.nama = nama;
            this.stok = stok;
        }
        public void TambahStok(int jumlah)
        {
            if (jumlah > 0)
            {
                stok += jumlah;
            }
        }
        public virtual bool KurangiStok(int jumlah)
        {
            if (jumlah > 0 && jumlah <= stok)
            {
                stok -= jumlah;
                return true;
            }
            return false;
        }
        public abstract void TampilkanInfo();
    }
    public class BarangUmum : Barang
    {
        public BarangUmum(int kode, string nama, int stok) : base(kode, nama, stok) { }

        public override void TampilkanInfo()
        {
            Console.WriteLine($"Kode: {Kode}, Nama: {Nama}, Stok: {Stok}, Jenis: Umum");
        }
    }
    public class BarangMudahPecah : Barang
    {
        public BarangMudahPecah(int kode, string nama, int stok) : base(kode, nama, stok) { }

        public override bool KurangiStok(int jumlah)
        {
            if (jumlah > 10)
            {
                Console.WriteLine("Error: Barang Mudah Pecah maksimal keluar 10 unit per transaksi.");
                return false;
            }
            return base.KurangiStok(jumlah);
        }

        public override void TampilkanInfo()
        {
            Console.WriteLine($"Kode: {Kode}, Nama: {Nama}, Stok: {Stok}, Jenis: Mudah Pecah");
        }
    }
    public class Transaksi
    {
        public int KodeBarang { get; set; }
        public string Jenis { get; set; }
        public int Jumlah { get; set; }
        public DateTime Waktu { get; set; }

        public Transaksi(int kodeBarang, string jenis, int jumlah)
        {
            KodeBarang = kodeBarang;
            Jenis = jenis;
            Jumlah = jumlah;
            Waktu = DateTime.Now;
        }

        public void TampilkanInfo()
        {
            Console.WriteLine($"Waktu: {Waktu}, Kode Barang: {KodeBarang}, Jenis: {Jenis}, Jumlah: {Jumlah}");
        }
    }
    public class gudang
    {
        private List<Barang> daftarBarang = new List<Barang>();
        private List<Transaksi> riwayatTransaksi = new List<Transaksi>();
        public void TambahBarang()
        {
            int kode = 0;
            bool validKode = false;
            while (!validKode)
            {
                Console.Write("Masukkan kode barang (Harus Berupa Angka): ");
                try
                {
                    kode = int.Parse(Console.ReadLine());
                    if (kode > 0)
                    {
                        if (daftarBarang.Any(b => b.Kode == kode))
                        {
                            Console.WriteLine("Kode barang sudah ada. Coba lagi.");
                        }
                        else
                        {
                            validKode = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Kode barang harus angka positif. Coba lagi.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Input kode harus berupa angka. Coba lagi.");
                }
            }
            Console.Write("Masukkan nama barang: ");
            string nama = Console.ReadLine();
            int stok = 0;
            bool validStok = false;
            while (!validStok)
            {
                Console.Write("Masukkan stok awal (minimal 1): ");
                try
                {
                    stok = int.Parse(Console.ReadLine());
                    if (stok >= 1)
                    {
                        validStok = true;
                    }
                    else
                    {
                        Console.WriteLine("Stok harus minimal 1. Coba lagi.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Input stok harus berupa angka. Coba lagi.");
                }
            }
            int jenis = 0;
            bool validJenis = false;
            while (!validJenis)
            {
                Console.Write("Jenis barang (1: Umum, 2: Mudah Pecah): ");
                try
                {
                    jenis = int.Parse(Console.ReadLine());
                    if (jenis == 1 || jenis == 2)
                    {
                        validJenis = true;
                    }
                    else
                    {
                        Console.WriteLine("Jenis barang harus 1 atau 2. Coba lagi.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Input jenis harus berupa angka. Coba lagi.");
                }
            }

            Barang barang;
            if (jenis == 1)
            {
                barang = new BarangUmum(kode, nama, stok);
            }
            else
            {
                barang = new BarangMudahPecah(kode, nama, stok);
            }
            daftarBarang.Add(barang);
            Console.WriteLine("Barang berhasil ditambahkan.");
        }
        public void UbahBarang()
        {
            if (daftarBarang.Count == 0)
            {
                Console.WriteLine("Tidak ada barang untuk diubah.");
                return;
            }
            Console.WriteLine("Daftar Barang:");
            foreach (var b in daftarBarang)
            {
                Console.WriteLine($"Kode: {b.Kode}, Nama: {b.Nama}");
            }
            int kode = 0;
            bool validKode = false;
            while (!validKode)
            {
                Console.Write("Masukkan kode barang yang ingin diubah: ");
                try
                {
                    kode = int.Parse(Console.ReadLine());
                    if (daftarBarang.Any(b => b.Kode == kode))
                    {
                        validKode = true;
                    }
                    else
                    {
                        Console.WriteLine("Kode barang tidak ditemukan. Coba lagi.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Input kode harus berupa angka. Coba lagi.");
                }
            }
            Barang barang = daftarBarang.First(b => b.Kode == kode);
            Console.Write("Masukkan nama baru: ");
            string namaBaru = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(namaBaru))
            {
                barang.Nama = namaBaru;
                Console.WriteLine("Barang berhasil diubah.");
            }
            else
            {
                Console.WriteLine("Nama baru tidak boleh kosong.");
            }
        }
        public void LihatBarang()
        {
            if (daftarBarang.Count == 0)
            {
                Console.WriteLine("Tidak ada barang.");
                return;
            }
            Console.WriteLine("Daftar Barang:");
            foreach (var b in daftarBarang)
            {
                Console.WriteLine($"Kode: {b.Kode}, Nama: {b.Nama}");
            }
            int kode = 0;
            bool validKode = false;
            while (!validKode)
            {
                Console.Write("Masukkan kode barang untuk melihat detail: ");
                try
                {
                    kode = int.Parse(Console.ReadLine());
                    if (daftarBarang.Any(b => b.Kode == kode))
                    {
                        validKode = true;
                    }
                    else
                    {
                        Console.WriteLine("Kode barang tidak ditemukan. Coba lagi.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Input kode harus berupa angka. Coba lagi.");
                }
            }
            Barang barang = daftarBarang.First(b => b.Kode == kode);
            barang.TampilkanInfo();
        }
        public void LakukanTransaksi()
        {
            if (daftarBarang.Count == 0)
            {
                Console.WriteLine("Tidak ada barang untuk transaksi.");
                return;
            }
            Console.WriteLine("Daftar Barang:");
            foreach (var b in daftarBarang)
            {
                Console.WriteLine($"Kode: {b.Kode}, Nama: {b.Nama}");
            }
            int kode = 0;
            bool validKode = false;
            while (!validKode)
            {
                Console.Write("Masukkan kode barang untuk transaksi: ");
                try
                {
                    kode = int.Parse(Console.ReadLine());
                    if (daftarBarang.Any(b => b.Kode == kode))
                    {
                        validKode = true;
                    }
                    else
                    {
                        Console.WriteLine("Kode barang tidak ditemukan. Coba lagi.");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Input kode harus berupa angka. Coba lagi.");
                }
            }
            Barang barang = daftarBarang.First(b => b.Kode == kode);
            Console.Write("Jenis transaksi (IN/OUT): ");
            string jenis = Console.ReadLine().ToUpper();
            int jumlah = 0;
            bool validJumlah = false;
            while (!validJumlah)
            {
                Console.Write("Masukkan jumlah: ");
                try
                {
                    jumlah = int.Parse(Console.ReadLine());
                    validJumlah = true;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Input jumlah harus berupa angka. Coba lagi.");
                }
            }

            bool berhasil = false;
            if (jenis == "IN")
            {
                barang.TambahStok(jumlah);
                berhasil = true;
            }
            else if (jenis == "OUT")
            {
                berhasil = barang.KurangiStok(jumlah);
            }

            if (berhasil)
            {
                riwayatTransaksi.Add(new Transaksi(barang.Kode, jenis, jumlah));
                Console.WriteLine("Transaksi berhasil.");
            }
            else
            {
                Console.WriteLine("Transaksi gagal.");
            }
        }
        public void LihatRiwayat()
        {
            if (riwayatTransaksi.Count == 0)
            {
                Console.WriteLine("Tidak ada riwayat transaksi.");
                return;
            }
            foreach (var t in riwayatTransaksi)
            {
                t.TampilkanInfo();
            }
        }
        public void LihatStok()
        {
            if (daftarBarang.Count == 0)
            {
                Console.WriteLine("Tidak ada barang.");
                return;
            }
            Console.WriteLine("Daftar Barang:");
            foreach (var b in daftarBarang)
            {
                b.TampilkanInfo();
            }
            Console.WriteLine($"Total barang: {daftarBarang.Count}, Total stok: {daftarBarang.Sum(b => b.Stok)}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Warehouse warehouse = new Warehouse();

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Tambah Barang");
                Console.WriteLine("2. Ubah Barang");
                Console.WriteLine("3. Lihat Barang");
                Console.WriteLine("4. Lakukan Transaksi");
                Console.WriteLine("5. Lihat Riwayat Transaksi");
                Console.WriteLine("6. Lihat Stok");
                Console.WriteLine("7. Keluar");
                int pilihan = 0;
                bool validMenu = false;
                while (!validMenu)
                {
                    Console.Write("Pilih: ");
                    try
                    {
                        pilihan = int.Parse(Console.ReadLine());
                        validMenu = true;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Input pilihan menu harus berupa angka. Coba lagi.");
                    }
                }

                switch (pilihan)
                {
                    case 1: warehouse.TambahBarang(); break;
                    case 2: warehouse.UbahBarang(); break;
                    case 3: warehouse.LihatBarang(); break;
                    case 4: warehouse.LakukanTransaksi(); break;
                    case 5: warehouse.LihatRiwayat(); break;
                    case 6: warehouse.LihatStok(); break;
                    case 7: return;
                    default: Console.WriteLine("Pilihan tidak valid."); break;
                }
            }
        }
    }
}
