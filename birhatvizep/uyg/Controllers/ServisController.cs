using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using uyg.Models;
using uyg.ViewModel;

namespace uyg.Controllers
{
    public class ServisController : ApiController
    {
        DB03Entities db = new DB03Entities();
        SonucModel sonuc = new SonucModel();

        #region Odev
        [HttpGet]
        [Route("api/odevliste")]
        public List<OdevModel> OdevListe()
        {

            List<OdevModel> liste = db.Odev.Select(x => new OdevModel()
            {
                odevAdi = x.odevAdi,
                odevId = x.odevId,
                odevKredi = x.odevKredi
            }).ToList();

            return liste;



        }
        [HttpGet]
        [Route("api/odevbyid/{odevId}")]
        public OdevModel OdevById(string odevId)
        {
            OdevModel kayit = db.Odev.Where(s => s.odevId == odevId).Select(x => new OdevModel()
            {
                odevAdi = x.odevAdi,
                odevId = x.odevId,
                odevKredi = x.odevKredi


            }).FirstOrDefault();
            return kayit;

        }

        [HttpPost]
        [Route("api/odevekle")]

        public SonucModel OdevEkle(OdevModel model)
        {
            if (db.Odev.Count(s => s.odevId == model.odevId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Odev Eklenemedi";
                return sonuc;
            }

            Odev yeni = new Odev();
            yeni.odevId = Guid.NewGuid().ToString();
            yeni.odevAdi = model.odevAdi;
            yeni.odevKredi = model.odevKredi;
            db.Odev.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Odev Eklendi";
            return sonuc;

        }
        [HttpPut]
        [Route("api/odevduzenle")]
        public SonucModel OdevDuzenle(OdevModel model)
        {
            Odev kayit = db.Odev.Where(s => s.odevId == model.odevId).FirstOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ödev Bulunamadı";
                return sonuc;
            }


            kayit.odevAdi = model.odevAdi;
            kayit.odevKredi = model.odevKredi;
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ödev Düzenlendi";
            return sonuc;

        }
        [HttpDelete]
        [Route("api/odevsil/{odevId}")]
        public SonucModel OdevSil(string odevId)
        {
            Odev kayit = db.Odev.Where(s => s.odevId == odevId).FirstOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı";
                return sonuc;
            }

            db.Odev.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Silindi";
            return sonuc;
        }
        #endregion




        #region Ogrenci

        [HttpGet]
        [Route("api/ogrenciliste")]
        public List<OgrenciModel> OgrenciListe()
        {
            List<OgrenciModel> liste = db.Ogrenci.Select(selector: NewMethod()).ToList();
            return liste;
        }

        private static Expression<Func<Ogrenci, OgrenciModel>> NewMethod()
        {
            return s =>
            {
                OgrenciModel ogrenciModel1 = new OgrenciModel()
                {
                    ogrAdsoyad = s.ogrAdsoyad,
                    ogrDogTarih = s.ogrDogTarih,
                    ogrId = s.ogrId,
                    ogrNo = s.ogrNo
                };
                OgrenciModel ogrenciModel = ogrenciModel1;
                return ogrenciModel;
            };
        }

        [HttpGet]
        [Route("api/ogrencibyid/{ogrId}")]

        public OgrenciModel OgrenciById(string ogrId)
        {
            OgrenciModel kayit = db.Ogrenci.Where(s => s.ogrId == ogrId).Select(s=> new OgrenciModel()
            {
                ogrId=s.ogrId,
                ogrAdsoyad=s.ogrAdsoyad,
                ogrDogTarih=s.ogrDogTarih,
                ogrNo=s.ogrNo
            }).SingleOrDefault();
            return kayit;

        }
        [HttpPost]
        [Route("api/ogrenciekle")]
        public SonucModel OgrenciEkle(OgrenciModel model)
        {
            if (db.Ogrenci.Count(s=>s.ogrId == model.ogrId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Bu öğrenci zaten var.";

                return sonuc;
            }
            Ogrenci yeniOgrenci = new Ogrenci();
            yeniOgrenci.ogrId = Guid.NewGuid().ToString();
            yeniOgrenci.ogrDogTarih = model.ogrDogTarih;
            yeniOgrenci.ogrAdsoyad = model.ogrAdsoyad;
            yeniOgrenci.ogrNo = model.ogrNo;
            db.Ogrenci.Add(yeniOgrenci);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Öğrenci Başarıyla Eklendi";
            return sonuc;


        }
        [HttpDelete]
        [Route("api/ogrencisil")]

        public SonucModel ogrenciSil(string ogrId)
        {
            
            Ogrenci kayit = db.Ogrenci.Where(s => s.ogrId == ogrId).SingleOrDefault();
            if (kayit == null) {
                sonuc.islem = false;
                sonuc.mesaj = "Böyle bir öğrenci bulunamadı.";
                return sonuc; 
                    }
            db.Ogrenci.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Öğrenci Başarıyla Silindi";
            return sonuc;


        }
        
       
        #endregion

    }
}








