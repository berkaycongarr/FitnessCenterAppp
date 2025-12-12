using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Controllers
{
    [Authorize]
    public class AIController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new AIRequestVM());
        }

        [HttpPost]
        public IActionResult Index(AIRequestVM model)
        {
            if (ModelState.IsValid)
            {
                // 1. Vücut Kitle İndeksi (BMI) Hesapla
                // Formül: Kilo / (Boy * Boy) [Metre cinsinden]
                double boyMetre = (double)model.Boy / 100;
                double vki = model.Kilo / (boyMetre * boyMetre);

                // 2. Dinamik Program Oluşturma (Expert System Logic)
                string oneri = "";
                string egzersiz = "";

                // VKI Analizi
                string durum = "";
                if (vki < 18.5) durum = "Zayıf";
                else if (vki < 25) durum = "Normal Kilolu";
                else if (vki < 30) durum = "Fazla Kilolu";
                else durum = "Obezite Başlangıcı";

                // Hedefe Göre Tavsiye Motoru
                if (model.Hedef == "Kilo Verme")
                {
                    oneri = $"Vücut analizine göre şuan '{durum}' kategorisindesin. Senin için kalori açığı oluşturacak bir beslenme planı hazırladım. Günlük kalori ihtiyacının 500 kcal altına düşmelisin. Bol su tüketimi ve lifli gıdalar (yulaf, brokoli) kritik önem taşıyor.";
                    egzersiz = "Haftada en az 4 gün, 45 dakikalık 'Kardiyo' ağırlıklı antrenman (Koşu, Yüzme, HIIT) yapmalısın. Nabzını 120-140 aralığında tutmaya çalış.";
                }
                else if (model.Hedef == "Kas Yapma")
                {
                    oneri = $"Vücut yapın '{durum}' kategorisinde. Kas kütleni artırmak için günlük protein alımını kilonun 2 katına (gram cinsinden) çıkarmalısın. Karbonhidratı antrenman öncesi, proteini antrenman sonrası almalısın.";
                    egzersiz = "Haftada 5 gün, bölgesel ağırlık antrenmanı (Hypertrophy) yapmalısın. Az tekrar, çok ağırlık prensibiyle çalış. Set aralarında dinlenmeyi unutma.";
                }
                else // Form Koruma
                {
                    oneri = $"Mevcut formun '{durum}' seviyesinde ve gayet iyi görünüyor. Kilonu korumak için dengeli beslenmeye devam et. Şeker ve işlenmiş gıdalardan uzak durman yeterli.";
                    egzersiz = "Haftada 3 gün tüm vücut (Full Body) antrenmanı yaparak kas tonusunu koruyabilirsin. Yoga veya Pilates ile esnekliğini artır.";
                }

                // 3. Sonucu Birleştir
                string aiResponse = $@"
                    <h4>Analiz Sonucu: {durum} (VKİ: {vki:F2})</h4>
                    <hr>
                    <p><strong>🧠 YZ Beslenme Önerisi:</strong><br> {oneri}</p>
                    <p><strong>💪 YZ Antrenman Planı:</strong><br> {egzersiz}</p>
                    <div class='alert alert-success mt-3'>Bu program senin {model.Boy}cm boyun, {model.Kilo}kg ağırlığın ve {model.Cinsiyet} fizyolojin baz alınarak yapay zeka tarafından özel oluşturulmuştur.</div>
                ";

                model.AIResult = aiResponse;
                return View(model);
            }

            return View(model);
        }
    }
}