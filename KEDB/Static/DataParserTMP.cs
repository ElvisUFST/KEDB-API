using KEDB.Data.Interface;
using KEDB.Dto;
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
    public class DataParserTMP
    {
        private readonly IRubrikTypeRepository _rubrikTypeRepository;
        private readonly IKontrolrapportRepository _kontrolrapportRepository;
        private readonly IProfilRepository _profilRepository;
        private readonly IRubrikRepository _rubrikRepository;

        public DataParserTMP(IRubrikTypeRepository rubrikTypeRepository, IKontrolrapportRepository kontrolrapportRepository, IProfilRepository profilRepository, IRubrikRepository rubrikRepository)
        {
            _rubrikTypeRepository = rubrikTypeRepository;
            _kontrolrapportRepository = kontrolrapportRepository;
            _profilRepository = profilRepository;
            _rubrikRepository = rubrikRepository;
        }

        public async Task Parse(IFormFile data)
        {

            XmlDocument xDoc = new XmlDocument();
            using Stream fileStream = data.OpenReadStream();
            xDoc.Load(fileStream);

            XmlNodeList vareposter = xDoc.GetElementsByTagName("varepost");

            List<String> sessionRefs = new List<string>();  //liste med refnr. der bliver læst ind nu
            foreach (XmlElement varepost in vareposter)
            {

                string refnr = varepost.SelectSingleNode("Ref_nr_Varepost_nr").InnerText.Substring(0, 13);

                if (refnr.Substring(0, 4).Equals(DateTime.Now.Year.ToString())) //Der er data med for 2018-2021 i filen. Det er ikke nødvendigt at tjekke på vareposter fra tidligere år
                {
                    var kontrolrapporter = await _kontrolrapportRepository.GetAll(new KontrolrapportSearchFilterDto
                    {
                        Referencenummer = refnr
                    });

                    if (kontrolrapporter.Count() < 1)
                    {
                        if (!sessionRefs.Contains(refnr))
                        {
                            sessionRefs.Add(refnr);
                            await readVarepost(varepost);
                        }
                        else //hvis vareposten er med flere gange i samme fil er det fordi der er flere certifikater på samme varepost
                        {
                            var kontrolrapport = kontrolrapporter.First();
                            RubrikType rubrikType = _rubrikTypeRepository.GetByXmlTag("Cert_kod");
                            Rubrik certRubrik = new Rubrik
                            {
                                OriginalVaerdi = varepost.SelectSingleNode("Cert_kod").InnerText + " " + varepost.SelectSingleNode("Cert_nr").InnerText, //Certifikat består af to dele. Certifikat kode og nr. Disse står som to seperate værdier
                                RubrikTypeId = rubrikType.Id,
                                KontrolrapportId = kontrolrapport.Id,
                            };

                            Rubrik gemtRubrik = await _rubrikRepository.Add(certRubrik);
                        }
                    }
                }
            }
        }

        private async Task readVarepost(XmlElement varepost)
        {

            Kontrolrapport newKontrolrapport = new Kontrolrapport();

            newKontrolrapport.Profilnummer = "1725";
            newKontrolrapport.Toldsted = int.Parse(varepost.SelectSingleNode("Reg_told_sted").InnerText);

            List<Rubrik> rubrikList = new List<Rubrik>();

            var children = varepost.ChildNodes;

            foreach (XmlElement child in children)
            {
                RubrikType rubrikType = _rubrikTypeRepository.GetByXmlTag(child.Name);

                if (rubrikType != null)
                {
                    if (rubrikType.XmlTag.Equals("Cert_kod"))
                    {
                        rubrikList.Add(new Rubrik
                        {
                            OriginalVaerdi = child.InnerText + " " + varepost.SelectSingleNode("Cert_nr").InnerText,
                            RubrikType = rubrikType,
                        });
                    }
                    else
                    {
                        rubrikList.Add(new Rubrik
                        {
                            OriginalVaerdi = child.InnerText,
                            RubrikType = rubrikType,
                        });
                    }
                }
                else
                {

                    switch (child.Name)
                    {
                        case "Ref_nr_Varepost_nr":
                            newKontrolrapport.Referencenummer = child.InnerText.Substring(0, 13);
                            newKontrolrapport.Varepostnummer = child.InnerText.Substring(child.InnerText.Length - 3);
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
}