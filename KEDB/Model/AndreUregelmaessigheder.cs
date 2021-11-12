using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KEDB.Model
{
    public class AndreUregelmaessigheder
    {
        [ForeignKey("Kontrolrapport")]
        public int Id { get; set; }
        public bool IPRvarer { get; set; }
        public bool Narkotika { get; set; }
        public bool Doping { get; set; }
        public bool Vaaben { get; set; }
        public bool Andet { get; set; }

        //The code from here on are the relationsship to other classes/table
        [JsonIgnore]
        public virtual Kontrolrapport Kontrolrapport { get; set; }
    }
}
