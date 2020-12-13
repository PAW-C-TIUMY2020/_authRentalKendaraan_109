using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentalKendaraan_109.Models
{
    public partial class Kendaraan
    {
        public Kendaraan()
        {
            Peminjaman = new HashSet<Peminjaman>();
        }

        public int IdKendaraan { get; set; }

        [Required(ErrorMessage = "Nama Kendaraan tidak boleh kosong")]
        public string NamaKendaraan { get; set; }

        [Required(ErrorMessage = "Nomor Polisi tidak boleh kosong")]
        public string NoPolisi { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Nomor STNK hanya boleh diisi oleh angka")]
        [MinLength(5, ErrorMessage = "Nomor HP minimal 5 angka")]
        [MaxLength(10, ErrorMessage = "Nomor HP maksimal 10 angka")]
        public string NoStnk { get; set; }
        public int? IdJenisKendaraan { get; set; }

        [Required(ErrorMessage = "Ketersediaan tidak boleh kosong")]
        public string Ketersediaan { get; set; }

        public JenisKendaraan IdJenisKendaraanNavigation { get; set; }
        public ICollection<Peminjaman> Peminjaman { get; set; }
    }
}
