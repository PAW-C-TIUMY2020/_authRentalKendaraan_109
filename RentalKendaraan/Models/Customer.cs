using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RentalKendaraan_109.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Peminjaman = new HashSet<Peminjaman>();
        }

        public int IdCustomer { get; set; }

        [Required(ErrorMessage = "Nama Customer tidak boleh kosong")]
        public string NamaCustomer { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "NIK hanya boleh diisi oleh angka")]
        public string Nik { get; set; }

        [Required(ErrorMessage = "Alamat tidak boleh kosong")]
        public string Alamat { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Harap masukan No HP yang benar")]
        [MinLength(10, ErrorMessage = "Nomor HP minimal 10 angka")]
        [MaxLength(13, ErrorMessage = "Nomor HP maksimal 13 angka")]
        public string NoHp { get; set; }

        public int? IdGender { get; set; }

        public Gender IdGenderNavigation { get; set; }
        public ICollection<Peminjaman> Peminjaman { get; set; }
    }
}
