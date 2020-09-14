namespace ETicaret.Models
{
    public class KategoriUrun
    {
        // public int Id { get; set; }//PK  ???????

        //  KategoriId+UrunId: PK (BirleşikId)
        // [ForeignKey]
        // public int KID { get; set; } //FK
        public int KategoriId { get; set; } //Varsayılan FK, scaler
        public int  UrunId{ get; set; }//FK,   scaler



        
        public Kategori Kategori { get; set; } //referans
        public Urun Urun { get; set; } //referans
    }
}