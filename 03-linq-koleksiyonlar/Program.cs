// =====================================================================
// 03 - Koleksiyonlar ve LINQ
// Konular: List, Dictionary, HashSet, Queue, Stack, LINQ operatörleri,
//          method syntax / query syntax, gruplama, join, ertelenmiş çalışma
// Çalıştırmak için:  dotnet run
// =====================================================================

using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// ---------------------------------------------------------------------
Console.WriteLine("=== 1. Temel Koleksiyonlar ===");
// ---------------------------------------------------------------------
var sehirler = new List<string> { "İstanbul", "Ankara", "İzmir" };
sehirler.Add("Bursa");
sehirler.AddRange(new[] { "Antalya", "Adana" });
sehirler.Remove("Ankara");
sehirler.Insert(0, "Eskişehir");
Console.WriteLine($"List: {string.Join(", ", sehirler)} (adet: {sehirler.Count})");

var plakalar = new Dictionary<string, int> { ["İstanbul"] = 34, ["Ankara"] = 6, ["İzmir"] = 35 };
plakalar["Bursa"] = 16;               // yoksa ekler, varsa günceller
if (plakalar.TryGetValue("Ankara", out int ankaraPlaka)) Console.WriteLine($"Ankara plaka kodu: {ankaraPlaka}");
foreach (var (sehir, kod) in plakalar) Console.WriteLine($"  {kod,2} -> {sehir}");

var benzersiz = new HashSet<int> { 1, 2, 3, 3, 2, 1 };   // tekrar eden öğeler eklenmez
var diger = new HashSet<int> { 3, 4, 5 };
Console.WriteLine($"HashSet: {string.Join(",", benzersiz)} | kesişim: {string.Join(",", benzersiz.Intersect(diger))} | birleşim: {string.Join(",", benzersiz.Union(diger))}");

var kuyruk = new Queue<string>();     // FIFO: ilk giren ilk çıkar
kuyruk.Enqueue("Ali"); kuyruk.Enqueue("Veli"); kuyruk.Enqueue("Ayşe");
Console.WriteLine($"Kuyruktan çıkan: {kuyruk.Dequeue()}, sıradaki: {kuyruk.Peek()}");

var yigin = new Stack<string>();      // LIFO: son giren ilk çıkar
yigin.Push("Sayfa 1"); yigin.Push("Sayfa 2"); yigin.Push("Sayfa 3");
Console.WriteLine($"Yığından çıkan: {yigin.Pop()}, üstteki: {yigin.Peek()}");

// ---------------------------------------------------------------------
// Örnek veri kümesi
// ---------------------------------------------------------------------
var ogrenciler = new List<Ogrenci>
{
    new(1, "Elif Yılmaz", "İstanbul", 24),
    new(2, "Can Öztürk", "Ankara", 31),
    new(3, "Merve Şahin", "İstanbul", 27),
    new(4, "Kaan Aksoy", "İzmir", 22),
    new(5, "Deniz Polat", "Ankara", 35),
    new(6, "Sena Koç", "İstanbul", 29)
};

var kurslar = new List<Kurs>
{
    new(10, "C# Programlama", "Yazılım", 40),
    new(11, "Python Temelleri", "Yazılım", 32),
    new(12, "SQL Server", "Veritabanı", 36),
    new(13, "Scrum Temelleri", "Proje Yönetimi", 16)
};

var kayitlar = new List<Kayit>
{
    new(1, 10, 85), new(1, 12, 90), new(2, 10, 70), new(3, 11, 95),
    new(3, 12, 80), new(4, 10, 60), new(5, 13, 88), new(6, 11, 77), new(6, 10, 92)
};

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 2. Filtreleme, Dönüştürme, Sıralama ===");
// ---------------------------------------------------------------------
var istanbullular = ogrenciler.Where(o => o.Sehir == "İstanbul").Select(o => o.AdSoyad);
Console.WriteLine("İstanbul'daki öğrenciler: " + string.Join(", ", istanbullular));

var yasSirali = ogrenciler.OrderByDescending(o => o.Yas).ThenBy(o => o.AdSoyad).Take(3);
Console.WriteLine("En büyük 3 öğrenci: " + string.Join(", ", yasSirali.Select(o => $"{o.AdSoyad} ({o.Yas})")));

// Anonim tip ile projeksiyon
foreach (var k in kurslar.Where(kurs => kurs.Saat > 30).Select(kurs => new { kurs.Ad, Gun = Math.Ceiling(kurs.Saat / 8.0) }))
    Console.WriteLine($"  {k.Ad}: günde 8 saat ise {k.Gun} gün");

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 3. Toplama (Aggregate) ve Sorgulama Operatörleri ===");
// ---------------------------------------------------------------------
Console.WriteLine($"Yaş ortalaması: {ogrenciler.Average(o => o.Yas):F1}, toplam kurs saati: {kurslar.Sum(k => k.Saat)}");
Console.WriteLine($"Herkes 18 yaşından büyük mü? {ogrenciler.All(o => o.Yas > 18)}");
Console.WriteLine($"30 yaşından büyük var mı? {ogrenciler.Any(o => o.Yas > 30)}");
Console.WriteLine($"İlk İzmirli: {ogrenciler.First(o => o.Sehir == "İzmir").AdSoyad}");
Console.WriteLine($"Bursalı var mı? {ogrenciler.FirstOrDefault(o => o.Sehir == "Bursa")?.AdSoyad ?? "yok"}");
Console.WriteLine($"En yaşlı: {ogrenciler.MaxBy(o => o.Yas)!.AdSoyad}, en genç: {ogrenciler.MinBy(o => o.Yas)!.AdSoyad}");
Console.WriteLine($"Benzersiz şehirler: {string.Join(", ", ogrenciler.Select(o => o.Sehir).Distinct())}");
Console.WriteLine($"Sayfalama (Skip 2, Take 2): {string.Join(", ", ogrenciler.Skip(2).Take(2).Select(o => o.AdSoyad))}");
Console.WriteLine($"Zip: {string.Join(", ", new[] { 1, 2, 3 }.Zip(new[] { "bir", "iki", "üç" }, (s, m) => $"{s}={m}"))}");

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 4. Gruplama ===");
// ---------------------------------------------------------------------
var sehirGruplari = ogrenciler
    .GroupBy(o => o.Sehir)
    .Select(g => new { Sehir = g.Key, Adet = g.Count(), YasOrt = g.Average(o => o.Yas) })
    .OrderByDescending(x => x.Adet);

foreach (var g in sehirGruplari)
    Console.WriteLine($"  {g.Sehir,-10} {g.Adet} öğrenci, yaş ortalaması {g.YasOrt:F1}");

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 5. Join (method syntax ve query syntax) ===");
// ---------------------------------------------------------------------
// Method syntax: Öğrenci - Kayıt - Kurs birleştirmesi
var method = ogrenciler
    .Join(kayitlar, o => o.Id, k => k.OgrenciId, (o, k) => new { o.AdSoyad, k.KursId, k.Not })
    .Join(kurslar, x => x.KursId, c => c.Id, (x, c) => new { x.AdSoyad, Kurs = c.Ad, x.Not })
    .Where(x => x.Not >= 85)
    .OrderByDescending(x => x.Not);

foreach (var r in method) Console.WriteLine($"  {r.AdSoyad,-14} {r.Kurs,-18} {r.Not}");

// Query syntax: SQL'e benzer yazım; aynı sonucu üretir
var query = from o in ogrenciler
            join k in kayitlar on o.Id equals k.OgrenciId
            join c in kurslar on k.KursId equals c.Id
            where c.Kategori == "Yazılım"
            group k.Not by c.Ad into grup
            select new { Kurs = grup.Key, Ortalama = grup.Average() };

Console.WriteLine("Yazılım kurslarında not ortalamaları:");
foreach (var q in query) Console.WriteLine($"  {q.Kurs,-18} {q.Ortalama:F1}");

// GroupJoin: her kursa kayıtlı öğrenci listesi
var kursOgrenci = kurslar.GroupJoin(kayitlar, c => c.Id, k => k.KursId, (c, ks) => new { c.Ad, Sayi = ks.Count() });
Console.WriteLine("Kurs başına kayıt: " + string.Join(", ", kursOgrenci.Select(x => $"{x.Ad}={x.Sayi}")));

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 6. Ertelenmiş Çalışma (Deferred Execution) ===");
// ---------------------------------------------------------------------
var sayilar = new List<int> { 1, 2, 3 };
var ciftler = sayilar.Where(s => s % 2 == 0);   // henüz çalışmadı, yalnızca sorgu tanımlandı
sayilar.Add(4);                                  // sorgu çalışmadan önce kaynak değişti
Console.WriteLine($"Çiftler (ertelenmiş): {string.Join(",", ciftler)}");   // şimdi çalışır -> 2,4

var anlik = sayilar.Where(s => s % 2 == 0).ToList();   // ToList() sorguyu hemen çalıştırır
sayilar.Add(6);
Console.WriteLine($"ToList() sonrası eklenen 6 listede yok: {string.Join(",", anlik)}");

// Chunk, DistinctBy, ToDictionary, ToLookup
Console.WriteLine("Chunk(2): " + string.Join(" | ", Enumerable.Range(1, 7).Chunk(2).Select(c => string.Join(",", c))));
var sozluk = ogrenciler.ToDictionary(o => o.Id, o => o.AdSoyad);
Console.WriteLine($"ToDictionary -> 3 numaralı öğrenci: {sozluk[3]}");
var arama = ogrenciler.ToLookup(o => o.Sehir);
Console.WriteLine($"ToLookup -> Ankara: {string.Join(", ", arama["Ankara"].Select(o => o.AdSoyad))}");

record Ogrenci(int Id, string AdSoyad, string Sehir, int Yas);
record Kurs(int Id, string Ad, string Kategori, int Saat);
record Kayit(int OgrenciId, int KursId, int Not);
