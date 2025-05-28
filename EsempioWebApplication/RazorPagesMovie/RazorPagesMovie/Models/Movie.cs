using System.ComponentModel.DataAnnotations;

namespace RazorPagesMovie.Models
{
    public class Movie
    {
        public int Id { get; set; } //chiave primaria della tabella
        public string? Title { get; set; }
        [DataType(DataType.Date)] 
        public DateTime ReleaseDate { get; set; } //proprietà datetime (la riga sopra specifica che l'utente non è obbligato a inserire anche l'ora)
        public string? Genre { get; set; } //il punto di domanda indica che la proprietà ammette valori null
        public decimal Price { get; set; }
    }
}
