namespace WebEngineering.Models
{
    public class Produkt
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Bestellung> Bestellungen { get; set; }
        public virtual ICollection<Lieferung> Lieferungen { get; set; }
    }
}
