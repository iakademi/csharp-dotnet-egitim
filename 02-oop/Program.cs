// =====================================================================
// 02 - Nesne Yönelimli Programlama (OOP)
// Konular: sınıf, kapsülleme, kalıtım, soyut sınıf, arayüz, polimorfizm,
//          record, struct, generic sınıf, extension method, event
// Çalıştırmak için:  dotnet run
// =====================================================================

using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// --- Sınıf, kurucu metot ve özellikler ---------------------------------
Console.WriteLine("=== 1. Sınıf ve Kapsülleme ===");
var hesap = new BankaHesabi("TR-0001", "Zeynep Kaya", 1000m);
hesap.ParaYatir(500m);
hesap.ParaCek(300m);
try
{
    hesap.ParaCek(5000m);   // yetersiz bakiye -> özel durum fırlatılır
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Hata yakalandı: {ex.Message}");
}
Console.WriteLine(hesap);

// --- Kalıtım, soyut sınıf ve polimorfizm --------------------------------
Console.WriteLine("\n=== 2. Kalıtım ve Polimorfizm ===");
List<Calisan> calisanlar =
[
    new Egitmen("Ahmet Demir", 30000m, "C#"),
    new Yonetici("Selin Arslan", 45000m, 4),
    new Stajyer("Burak Çelik")
];

foreach (Calisan c in calisanlar)
{
    // Her nesne, kendi sınıfındaki Maas ve Tanit uygulamasını çağırır
    Console.WriteLine($"{c.Tanit()} | Aylık ücret: {c.AylikUcretHesapla():N2} TL");
}
decimal toplamMaliyet = calisanlar.Sum(c => c.AylikUcretHesapla());
Console.WriteLine($"Toplam aylık maliyet: {toplamMaliyet:N2} TL");

// --- Arayüz (interface) ------------------------------------------------
Console.WriteLine("\n=== 3. Arayüzler ===");
IRaporlanabilir[] raporlar = [new Egitmen("Deniz Yıldız", 32000m, "Python"), hesap];
foreach (var r in raporlar) Console.WriteLine(r.RaporOlustur());

// pattern matching ile tür kontrolü
foreach (var c in calisanlar)
{
    string aciklama = c switch
    {
        Egitmen e => $"{e.Ad} eğitmen, uzmanlığı {e.UzmanlikAlani}",
        Yonetici y when y.EkipBuyuklugu > 3 => $"{y.Ad} büyük bir ekibi yönetiyor",
        Yonetici y => $"{y.Ad} küçük bir ekibi yönetiyor",
        _ => $"{c.Ad} diğer"
    };
    Console.WriteLine(aciklama);
}

// --- record ve struct --------------------------------------------------
Console.WriteLine("\n=== 4. record ve struct ===");
var k1 = new Kurs("C# Temelleri", 40);
var k2 = new Kurs("C# Temelleri", 40);
var k3 = k1 with { Saat = 60 };   // with: kopyasını alıp bir özelliği değiştirir
Console.WriteLine($"k1 == k2 (değer eşitliği): {k1 == k2}");
Console.WriteLine($"k3: {k3}");

var nokta1 = new Nokta(3, 4);
var nokta2 = nokta1;              // struct değer tipidir: kopyalanır
nokta2.X = 10;
Console.WriteLine($"nokta1.X = {nokta1.X}, nokta2.X = {nokta2.X}, orijine uzaklık: {nokta1.OrijineUzaklik():F1}");

// --- Generic sınıf -----------------------------------------------------
Console.WriteLine("\n=== 5. Generic Sınıflar ===");
var kursDeposu = new Depo<Kurs>();
kursDeposu.Ekle(k1);
kursDeposu.Ekle(k3);
kursDeposu.Ekle(new Kurs("SQL Server", 32));
Console.WriteLine($"Depodaki kurs sayısı: {kursDeposu.Sayi}");
foreach (var k in kursDeposu.Bul(kurs => kurs.Saat >= 40)) Console.WriteLine($" - {k.Ad} ({k.Saat} saat)");

// --- Extension method --------------------------------------------------
Console.WriteLine("\n=== 6. Extension Method ===");
Console.WriteLine("merhaba dünya".IlkHarfleriBuyut());
Console.WriteLine($"'Ey Edip Adanada pide ye' palindrom mu? {"Ey Edip Adanada pide ye".PalindromMu()}");

// --- Delegate ve event -------------------------------------------------
Console.WriteLine("\n=== 7. Event Kullanımı ===");
var siparis = new SiparisServisi();
siparis.SiparisAlindi += (s, e) => Console.WriteLine($"[Mail] {e.Musteri} için {e.Tutar:N2} TL tutarlı sipariş alındı.");
siparis.SiparisAlindi += (s, e) => Console.WriteLine($"[Log] Sipariş no {e.SiparisNo} kaydedildi.");
siparis.SiparisVer("Mert Aydın", 249.90m);

// =====================================================================
// Tip tanımları
// =====================================================================

interface IRaporlanabilir
{
    string RaporOlustur();
}

class BankaHesabi : IRaporlanabilir
{
    private decimal _bakiye;   // alan dışarıdan doğrudan erişilemez (kapsülleme)

    public string HesapNo { get; }
    public string SahipAdi { get; set; }
    public decimal Bakiye => _bakiye;   // yalnızca okunabilir özellik

    public BankaHesabi(string hesapNo, string sahipAdi, decimal baslangicBakiyesi)
    {
        HesapNo = hesapNo;
        SahipAdi = sahipAdi;
        _bakiye = baslangicBakiyesi;
    }

    public void ParaYatir(decimal tutar)
    {
        if (tutar <= 0) throw new ArgumentOutOfRangeException(nameof(tutar), "Tutar sıfırdan büyük olmalıdır.");
        _bakiye += tutar;
    }

    public void ParaCek(decimal tutar)
    {
        if (tutar > _bakiye) throw new InvalidOperationException("Yetersiz bakiye.");
        _bakiye -= tutar;
    }

    public string RaporOlustur() => $"Hesap raporu: {HesapNo} - {SahipAdi} - Bakiye {_bakiye:N2} TL";

    public override string ToString() => $"{SahipAdi} ({HesapNo}) bakiyesi: {_bakiye:N2} TL";
}

abstract class Calisan
{
    public string Ad { get; }
    protected decimal TemelMaas { get; }

    protected Calisan(string ad, decimal temelMaas)
    {
        Ad = ad;
        TemelMaas = temelMaas;
    }

    // Alt sınıflar bu metodu mutlaka uygulamalıdır
    public abstract decimal AylikUcretHesapla();

    // Alt sınıflar isterse ezebilir (override)
    public virtual string Tanit() => $"{GetType().Name}: {Ad}";
}

class Egitmen : Calisan, IRaporlanabilir
{
    public string UzmanlikAlani { get; }

    public Egitmen(string ad, decimal maas, string uzmanlik) : base(ad, maas) => UzmanlikAlani = uzmanlik;

    public override decimal AylikUcretHesapla() => TemelMaas + 2500m;   // sabit ek ödeme

    public override string Tanit() => $"{base.Tanit()} ({UzmanlikAlani})";

    public string RaporOlustur() => $"Eğitmen raporu: {Ad} - {UzmanlikAlani}";
}

class Yonetici : Calisan
{
    public int EkipBuyuklugu { get; }

    public Yonetici(string ad, decimal maas, int ekip) : base(ad, maas) => EkipBuyuklugu = ekip;

    public override decimal AylikUcretHesapla() => TemelMaas + EkipBuyuklugu * 1000m;
}

class Stajyer : Calisan
{
    public Stajyer(string ad) : base(ad, 12000m) { }

    public override decimal AylikUcretHesapla() => TemelMaas;
}

// record: değişmez (immutable) veri taşıyıcılar için ideal
record Kurs(string Ad, int Saat);

// struct: küçük, değer tipi veri yapıları için
struct Nokta
{
    public double X { get; set; }
    public double Y { get; set; }

    public Nokta(double x, double y) { X = x; Y = y; }

    public readonly double OrijineUzaklik() => Math.Sqrt(X * X + Y * Y);
}

class Depo<T> where T : class
{
    private readonly List<T> _ogeler = new();

    public int Sayi => _ogeler.Count;

    public void Ekle(T oge) => _ogeler.Add(oge);

    public IEnumerable<T> Bul(Func<T, bool> sart) => _ogeler.Where(sart);
}

static class StringUzantilari
{
    public static string IlkHarfleriBuyut(this string metin)
    {
        var tr = new System.Globalization.CultureInfo("tr-TR");
        return tr.TextInfo.ToTitleCase(metin);
    }

    public static bool PalindromMu(this string metin)
    {
        var temiz = new string(metin.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
        return temiz.SequenceEqual(temiz.Reverse());
    }
}

class SiparisEventArgs : EventArgs
{
    public int SiparisNo { get; init; }
    public string Musteri { get; init; } = string.Empty;
    public decimal Tutar { get; init; }
}

class SiparisServisi
{
    private int _sonNo = 1000;

    public event EventHandler<SiparisEventArgs>? SiparisAlindi;

    public void SiparisVer(string musteri, decimal tutar)
    {
        var args = new SiparisEventArgs { SiparisNo = ++_sonNo, Musteri = musteri, Tutar = tutar };
        SiparisAlindi?.Invoke(this, args);   // abone yoksa null olur, güvenle çağrılır
    }
}
