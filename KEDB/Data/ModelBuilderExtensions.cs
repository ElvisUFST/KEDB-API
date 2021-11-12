using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace KEDB.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            parseFejltekster(modelBuilder);
            parseRubriktyper(modelBuilder);
            parseOversendtTilToldrapport(modelBuilder);
            parseToldrapportFejlKategorier(modelBuilder);
            parseToldrapportOpdagendeAktoer(modelBuilder);
            parseToldrapportKommunikation(modelBuilder);
            parseToldrapportTransportmiddel(modelBuilder);
            parseRubrikMuligFejl(modelBuilder);
        }

        private static void parseFejltekster(ModelBuilder modelBuilder)
        {
            using (StreamReader r = new StreamReader("Static/Stamdata/Fejltekster.json"))
            {
                string json = r.ReadToEnd();
                List<Fejltekst> fejltekstList = JsonConvert.DeserializeObject<List<Fejltekst>>(json);

                for (int i = 0; i < fejltekstList.Count; i++)
                {
                    var fejltekst = fejltekstList[i];
                    modelBuilder.Entity<Fejltekst>()
                   .HasData(
                       new Fejltekst
                       {
                           Id = (i + 1), //kan ikke give auto id
                           Tekst = fejltekst.Tekst,
                           Aktiv = fejltekst.Aktiv,

                       });
                }
            }
        }

        private static void parseRubrikMuligFejl(ModelBuilder modelBuilder)
        {

            //opretter først en default profil
            modelBuilder.Entity<Profil>().HasData(
                        new Profil
                        {
                            Id = 1, //kan ikke give auto id
                            Beskrivelse = "Bruges hvis RubrikMuligFejl ikke er defineret for profil",
                            Aktiv = true,
                            ProfilNummer = "Standard"
                        });

            using (StreamReader r = new StreamReader("Static/Stamdata/RubrikMuligFejl.json"))
            {
                var json = File.ReadAllText("Static/Stamdata/RubrikMuligFejl.json");
                JsonConvert
                    .DeserializeObject<List<RubrikMuligFejl>>(json)
                    .ForEach(rmf => modelBuilder.Entity<RubrikMuligFejl>().HasData(rmf));
            }
        }

        private static void parseRubriktyper(ModelBuilder modelBuilder)
        {
            using (StreamReader r = new StreamReader("Static/Stamdata/Rubriktyper.json"))
            {
                string json = r.ReadToEnd();
                List<RubrikType> rubrikTypeList = JsonConvert.DeserializeObject<List<RubrikType>>(json);

                for (int i = 0; i < rubrikTypeList.Count; i++)
                {
                    var rubrikType = rubrikTypeList[i];
                    modelBuilder.Entity<RubrikType>().HasData(
                        new RubrikType
                        {
                            Id = (i + 1), //kan ikke give auto id
                            Nummer = rubrikType.Nummer,
                            Navn = rubrikType.Navn,
                            XmlTag = rubrikType.XmlTag
                        });
                }
            }
        }

        private static void parseOversendtTilToldrapport(ModelBuilder modelBuilder)
        {
            using (StreamReader r = new StreamReader("Static/Stamdata/ToldrapportOvertraedelsesAktoer.json"))
            {
                string json = r.ReadToEnd();
                List<ToldrapportOvertraedelsesAktoer> toldrapportOvertraedelsesAktoerList = JsonConvert.DeserializeObject<List<ToldrapportOvertraedelsesAktoer>>(json);

                for (int i = 0; i < toldrapportOvertraedelsesAktoerList.Count; i++)
                {

                    var ToldrapportOvertraedelsesAktoer = toldrapportOvertraedelsesAktoerList[i];
                    modelBuilder.Entity<ToldrapportOvertraedelsesAktoer>().HasData(
                        new ToldrapportOvertraedelsesAktoer
                        {
                            Id = (i + 1), //kan ikke give auto id
                            Tekst = ToldrapportOvertraedelsesAktoer.Tekst
                        });
                }
            }
        }

        private static void parseToldrapportFejlKategorier(ModelBuilder modelBuilder)
        {
            using (StreamReader r = new StreamReader("Static/Stamdata/ToldrapportFejlKategorier.json"))
            {
                string json = r.ReadToEnd();
                List<ToldrapportFejlKategori> toldrapportFejlKategoriList = JsonConvert.DeserializeObject<List<ToldrapportFejlKategori>>(json);

                for (int i = 0; i < toldrapportFejlKategoriList.Count; i++)
                {

                    var toldrapportFejlKategori = toldrapportFejlKategoriList[i];
                    modelBuilder.Entity<ToldrapportFejlKategori>().HasData(
                        new ToldrapportFejlKategori
                        {
                            Id = (i + 1), //kan ikke give auto id
                            Tekst = toldrapportFejlKategori.Tekst
                        });
                }
            }
        }

        private static void parseToldrapportOpdagendeAktoer(ModelBuilder modelBuilder)
        {
            using (StreamReader r = new StreamReader("Static/Stamdata/ToldrapportOpdagendeAktoer.json"))
            {
                string json = r.ReadToEnd();
                List<ToldrapportOpdagendeAktoer> toldrapportOpdagendeAktoerList = JsonConvert.DeserializeObject<List<ToldrapportOpdagendeAktoer>>(json);

                for (int i = 0; i < toldrapportOpdagendeAktoerList.Count; i++)
                {
                    var toldrapportOpdagendeAktoer = toldrapportOpdagendeAktoerList[i];
                    modelBuilder.Entity<ToldrapportOpdagendeAktoer>().HasData(
                        new ToldrapportOpdagendeAktoer
                        {
                            Id = (i + 1), //kan ikke give auto id
                            Tekst = toldrapportOpdagendeAktoer.Tekst
                        });
                }
            }
        }

        private static void parseToldrapportKommunikation(ModelBuilder modelBuilder)
        {
            using (StreamReader r = new StreamReader("Static/Stamdata/ToldrapportKommunikation.json"))
            {
                string json = r.ReadToEnd();
                List<ToldrapportKommunikation> toldrapportKommunikationList = JsonConvert.DeserializeObject<List<ToldrapportKommunikation>>(json);

                for (int i = 0; i < toldrapportKommunikationList.Count; i++)
                {
                    var toldrapportKommunikation = toldrapportKommunikationList[i];
                    modelBuilder.Entity<ToldrapportKommunikation>().HasData(
                        new ToldrapportKommunikation
                        {
                            Id = (i + 1), //kan ikke give auto id
                            Tekst = toldrapportKommunikation.Tekst
                        });
                }
            }
        }

        private static void parseToldrapportTransportmiddel(ModelBuilder modelBuilder)
        {
            using (StreamReader r = new StreamReader("Static/Stamdata/ToldrapportTransportmiddel.json"))
            {
                string json = r.ReadToEnd();
                List<ToldrapportTransportmiddel> toldrapportTransportmiddelListe = JsonConvert.DeserializeObject<List<ToldrapportTransportmiddel>>(json);

                for (int i = 0; i < toldrapportTransportmiddelListe.Count; i++)
                {
                    var toldrapportTransportmiddel = toldrapportTransportmiddelListe[i];
                    modelBuilder.Entity<ToldrapportTransportmiddel>().HasData(
                        new ToldrapportTransportmiddel
                        {
                            Id = (i + 1), //kan ikke give auto id
                            Tekst = toldrapportTransportmiddel.Tekst
                        });
                }
            }
        }
    }
}