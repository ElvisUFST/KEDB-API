using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KEDB.Model
{
    public class Toldrapport : IValidatableObject
    {
        [ForeignKey("Kontrolrapport")]
        public int Id { get; set; }

        [Display(Name = "Andre Deltagere")]
        public string AndreDeltagere { get; set; }

        [Required(ErrorMessage = "Date is required"), Column(TypeName = "date")]
        public DateTime DagsDato { get; set; }

        [Required(ErrorMessage = "\"Talt med\" is required")]
        [Display(Name = "Talt med")]
        public string TaltMed { get; set; }

        [Display(Name = "Godkendt ordning")]
        public bool GodkendtOrdning { get; set; }

        [Display(Name = "Hvilken godkendt ordning")]
        public string GodkendtOrdningTekst { get; set; }

        [Required(ErrorMessage = "\"Sagsbeskrivelse\" is required")]
        public string Sagsbeskrivelse { get; set; }

        public bool Toldkontrol { get; set; }

        //The code from here on are the relationsship to other classes/table
        public int ToldrapportTransportmiddelId { get; set; }
        public virtual ToldrapportTransportmiddel ToldrapportTransportmiddel { get; set; }

        public int ToldrapportOpdagendeAktoerId { get; set; }
        public virtual ToldrapportOpdagendeAktoer ToldrapportOpdagendeAktoer { get; set; }

        public int ToldrapportFejlKategoriId { get; set; }
        public virtual ToldrapportFejlKategori ToldrapportFejlKategori { get; set; }

        public int ToldrapportKommunikationId { get; set; }
        public virtual ToldrapportKommunikation ToldrapportKommunikation { get; set; }

        public int ToldrapportOvertraedelsesAktoerId { get; set; }
        public virtual ToldrapportOvertraedelsesAktoer ToldrapportOvertraedelsesAktoer { get; set; }

        [JsonIgnore]
        public virtual Kontrolrapport Kontrolrapport { get; set; }

        //Hvis GodkendtOrdning == true skal GodkendtOrdningTekst være requiered
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (GodkendtOrdning && (String.IsNullOrEmpty(GodkendtOrdningTekst) || String.IsNullOrWhiteSpace(GodkendtOrdningTekst)))
            {
                yield return new ValidationResult("\"Hvilken godkendt ordning\" is requiered");
            }
        }
    }
}
