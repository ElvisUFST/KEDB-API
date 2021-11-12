using KEDB.Data.Interface;
using KEDB.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace KEDB_DataParser
{
    public class DataParser
    {
        private readonly IRubrikTypeRepository _rubrikTypeRepository;
        private readonly IKontrolrapportRepository _kontrolrapportRepository;
        private readonly IProfilRepository _profilRepository;

        public DataParser(IRubrikTypeRepository rubrikTypeRepository, IKontrolrapportRepository kontrolrapportRepository, IProfilRepository profilRepository)
        {
            _rubrikTypeRepository = rubrikTypeRepository;
            _kontrolrapportRepository = kontrolrapportRepository;
            _profilRepository = profilRepository;
        }

        public async Task Parse(IFormFile data)
        {
            XmlDocument xDoc = new XmlDocument();
            //Denne er til test. Læser lokal xml fil
            //xDoc.Load("Static/TestData/TestData3.xml");
            using Stream fileStream = data.OpenReadStream();
            xDoc.Load(fileStream);

            XmlNodeList vareposter = xDoc.GetElementsByTagName("varepost");

            foreach (XmlElement varepost in vareposter)
            {
                Kontrolrapport newKontrolrapport = new Kontrolrapport();
                List<Rubrik> rubrikList = new List<Rubrik>();


                var children = varepost.ChildNodes;

                /*Der findes i XML-filen to xml tags - den ene med headers, og den anden med værdier. 
                De er komma-separerede og indeholder data som systemet er født med.
                De følger ikke sædvanligt XML struktur, hvorfor vi er nødt til at lave et grimt, men nødvendigt parsing af data'en */
                string[] originalHeaders = { "" };
                string[] originalValues = { "" };

                var hackHeaderNode = varepost.SelectSingleNode("hackHeaders");
                if (hackHeaderNode != null)
                {
                    originalHeaders = hackHeaderNode.InnerText.Split(",");
                }

                var hackValueNode = varepost.SelectSingleNode("hackValues");
                if (hackValueNode != null)
                {
                    originalValues = hackValueNode.InnerText.Split(",");
                }
                foreach (XmlElement child in children)
                {

                    //hvis profil ikke findes bliver den oprettet.
                    if (child.Name.Equals("profil_nummer"))
                    {
                        var profilNummer = child.InnerText;
                        var profil = await _profilRepository.GetByProfilNummer(profilNummer);
                        if (profil == null)
                        {
                            await _profilRepository.Add(new Profil
                            {
                                ProfilNummer = profilNummer,
                                Aktiv = true
                            });
                        }
                    }

                    RubrikType rubrikType = _rubrikTypeRepository.GetByXmlTag(child.Name);
                    if (rubrikType != null)
                    {

                        int index = Array.FindIndex(originalHeaders, index => index.Contains(child.Name));
                        //Sættes til samme værdi, men den ene overskrives afhængigt af situationen 
                        string originalVaerdi = child.InnerText;
                        string korrigeretVaerdi = child.InnerText;

                        //Hvis originalHeaders indeholder nuværende child.Name/xml-tag, sæt Original værdi til det fundne
                        if (index != -1)
                        {
                            originalVaerdi = originalValues[index];
                            //   korrigeretVaerdi = child.InnerText;
                        }
                        //Sæt korrigeret værdi til en tom streng, da der ikke er mismatch mellem det fødte data og det nye data
                        else
                        {
                            korrigeretVaerdi = "";
                        }

                        rubrikList.Add(new Rubrik
                        {
                            OriginalVaerdi = originalVaerdi,
                            RubrikType = rubrikType,
                            KorrigeretVaerdi = korrigeretVaerdi
                        });
                    }
                    else
                    {

                        switch (child.Name)
                        {
                            case "Ref_nr_Varepost_nr":
                                //newKontrolrapport.Referencenummer = child.InnerText.Substring(0, child.InnerText.Length - 3);
                                newKontrolrapport.Referencenummer = child.InnerText.Substring(0, 6) + GenerateNewRandom();
                                newKontrolrapport.Varepostnummer = child.InnerText.Substring(child.InnerText.Length - 3);
                                break;

                            case "Reg_told_sted":
                                newKontrolrapport.Toldsted = int.Parse(child.InnerText);
                                break;

                            case "es7601_firma_nvn_kort":
                                newKontrolrapport.VaremodtagerNavn = child.InnerText;
                                break;

                            case "Vm_Se_nr_n":
                                newKontrolrapport.VaremodtagerCVR = int.Parse(child.InnerText);
                                break;

                            case "Antag_dato_s":
                                DateTime antagetDato;
                                bool success = DateTime.TryParseExact(
                                    child.InnerText,
                                    "yyyy-MM-dd",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.AdjustToUniversal,
                                    out antagetDato);
                                newKontrolrapport.AntagetDato = antagetDato;
                                break;

                            case "es7606_hov_br_nr":
                                if (child.InnerText == null)
                                {
                                    newKontrolrapport.Branchekode = 999999; //privat virksomhed
                                }
                                else
                                {
                                    newKontrolrapport.Branchekode = int.Parse(child.InnerText);
                                }
                                break;

                            case "Kl_Se_nr_n":
                                newKontrolrapport.KlarererCVR = int.Parse(child.InnerText);
                                break;

                            case "profil_nummer":
                                newKontrolrapport.Profilnummer = child.InnerText;
                                break;
                        }
                    }
                }

                //Default værdierne er 0. 
                newKontrolrapport.ToldmaessigAendringOpkraevning = (decimal)0;
                newKontrolrapport.ToldmaessigAendringTilbagebetaling = (decimal)0;

                newKontrolrapport.Rubrikker = rubrikList;
                Kontrolrapport savedKontrolrapport = await _kontrolrapportRepository.Add(newKontrolrapport);
            }
        }

        // Bruges til at generere et random 7 cifret nummer til at erstatte de identificerende numre i referencenummer
        private string GenerateNewRandom()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D7");
            if (r.Distinct().Count() == 1)
            {
                r = GenerateNewRandom();
            }
            return r;
        }
    }
}