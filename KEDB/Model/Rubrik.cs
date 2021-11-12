using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KEDB.Model
{
    public class Rubrik
    {
        [Key]
        public int Id { get; set; }
        public string OriginalVaerdi { get; set; }
        public string KorrigeretVaerdi { get; set; }

        public int KontrolrapportId { get; set; }

        [JsonIgnore]
        public virtual Kontrolrapport Kontrolrapport { get; set; }

        //The code from here on are the relationsship to other classes/table
        public int RubrikTypeId { get; set; }
        public virtual RubrikType RubrikType { get; set; }

        public virtual IList<RubrikValgtFejl> RubrikValgteFejl { get; set; }
    }
}
