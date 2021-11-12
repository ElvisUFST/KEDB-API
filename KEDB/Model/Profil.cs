using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KEDB.Model
{
    public class Profil
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "\"Tekst\" is required")]
        public string ProfilNummer { get; set; } //ikke database id. ID paa profil i RIS
        public string Beskrivelse { get; set; }
        public bool Aktiv { get; set; }

        //The code from here on are the relationsship to other classes/table
        [JsonIgnore]
        public virtual IList<RubrikMuligFejl> RubrikMuligeFejl { get; set; }
    }
}