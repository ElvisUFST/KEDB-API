using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KEDB.Model
{
    public class Kontrolrapport
    {
        [Key]
        public int Id { get; set; }

        [StringLength(13)]
        [Required, ReadOnly(true)]
        public string Referencenummer { get; set; }

        [StringLength(3)]
        [Required, ReadOnly(true)]
        public string Varepostnummer { get; set; }

        [HiddenInput(DisplayValue = false)]
        public bool Slettet { get; set; }

        [StringLength(10)]
        [Display(Name = "Workzone journalnummer")]
        public string WorkzoneJournalnummer { get; set; }

        [StringLength(4)]
        [Required, Display(Name = "Profil Id")]
        public string Profilnummer { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Branchekode")]
        public long Branchekode { get; set; }

        [Display(Name = "Toldmæssig ændring opkrævet")]
        public decimal ToldmaessigAendringOpkraevning { get; set; }

        [Display(Name = "Toldmæssig ændring tilbagebetaling")]
        public decimal ToldmaessigAendringTilbagebetaling { get; set; }

        [Display(Name = "Oversendt til analyse")]
        public bool OversendtTilAnalyse { get; set; }

        [Display(Name = "Oversendt til toldrapport")]
        public bool OversendtTilToldrapport { get; set; }

        public string Sagsbehandler { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Varemodtager navn")]
        public string VaremodtagerNavn { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Varemodtager CVR/SE")]
        public int VaremodtagerCVR { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Klarerer CVR/SE")]
        public int KlarererCVR { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Klarerer Navn")]
        public string KlarererNavn { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Toldsted")]
        public int Toldsted { get; set; }

        [ReadOnly(true), Column(TypeName = "date")]
        [Display(Name = "Indlæst dato")]
        public DateTime IndlaestDate { get; set; }

        [ReadOnly(true), Column(TypeName = "date")]
        [Display(Name = "Antaget dato")]
        public DateTime AntagetDato { get; set; }

        [Display(Name = "Afrapporteret dato"), Column(TypeName = "date")]
        public DateTime? AfrapporteretDato { get; set; }

        [Display(Name = "Redigeret dato"), Column(TypeName = "date")]
        public DateTime? RedigeretDato { get; set; }

        //The code from here on are the relationsship to other classes/table
        public virtual Toldrapport Toldrapport { get; set; }

        public virtual AndreUregelmaessigheder AndreUregelmaessigheder { get; set; }

        public virtual ICollection<Rubrik> Rubrikker { get; set; }
    }
}
