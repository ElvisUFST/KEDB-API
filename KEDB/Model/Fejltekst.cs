using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KEDB.Model
{
    public class Fejltekst
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "\"Tekst\" is required")]
        public string Tekst { get; set; }
        public bool Aktiv { get; set; }

        //The code from here on are the relationsship to other classes/table
        [JsonIgnore]
        public virtual IList<RubrikMuligFejl> RubrikMuligeFejl { get; set; }

        [JsonIgnore]
        public virtual IList<RubrikValgtFejl> RubrikValgteFejl { get; set; }

    }
}
