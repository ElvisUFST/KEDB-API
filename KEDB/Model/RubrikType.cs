using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KEDB.Model
{
    [Index(nameof(Nummer), IsUnique = true)]
    public class RubrikType
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "\"Navn\" is required")]
        public string Navn { get; set; }

        [Required(ErrorMessage = "\"Nummer\" is required")]
        public double Nummer { get; set; }
        public string XmlTag { get; set; }

        //The code from here on are the relationsship to other classes/table
        [JsonIgnore]
        public virtual IList<RubrikMuligFejl> RubrikMuligeFejl { get; set; }
    }
}
