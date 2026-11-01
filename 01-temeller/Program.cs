// =====================================================================
// 01 - C# Temelleri
// Konular: değişkenler, veri tipleri, dönüşümler, operatörler,
//          koşullar, döngüler, diziler, metotlar, null güvenliği, tuple
// Çalıştırmak için:  dotnet run
// =====================================================================

using System.Globalization;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;
var tr = new CultureInfo("tr-TR");

// ---------------------------------------------------------------------
Baslik("1. Değişkenler ve Veri Tipleri");
// ---------------------------------------------------------------------
int yas = 25;                    // 32 bit tam sayı
long buyukSayi = 9_000_000_000L; // 64 bit tam sayı (alt çizgi okunabilirlik içindir)
double pi = 3.14159;             // ondalıklı sayı
decimal fiyat = 1499.90m;        // para hesapları için decimal kullanılır
bool aktifMi = true;
char harf = 'İ';
string ad = "Ayşe";
var otomatik = 42;               // derleyici tipi (int) kendisi belirler
const double KdvOrani = 0.20;    // sabit: değeri değiştirilemez

Console.WriteLine($"Ad: {ad}, Yaş: {yas}, Aktif mi: {aktifMi}, Harf: {harf}");
Console.WriteLine($"Büyük sayı: {buyukSayi:N0}, Pi: {pi}, var ile: {otomatik.GetType().Name}");
Console.WriteLine($"Fiyat: {fiyat.ToString("C", tr)}, KDV dahil: {(fiyat * (1 + (decimal)KdvOrani)).ToString("C", tr)}");
Console.WriteLine($"int sınırları: {int.MinValue:N0} ... {int.MaxValue:N0}");

// ---------------------------------------------------------------------
Baslik("2. Tip Dönüşümleri");
// ---------------------------------------------------------------------
double ondalik = 9.78;
int kesilmis = (int)ondalik;                 // açık (explicit) dönüşüm: ondalık kısım atılır
double genis = yas;                          // örtük (implicit) dönüşüm: veri kaybı yok
int yuvarlanmis = Convert.ToInt32(ondalik);  // yuvarlar -> 10
Console.WriteLine($"(int)9.78 = {kesilmis}, Convert.ToInt32(9.78) = {yuvarlanmis}, double: {genis}");

string[] girdiler = { "123", "12a", "", "-7" };
foreach (var girdi in girdiler)
{
    // TryParse hata fırlatmaz; başarı durumunu bool olarak döner
    if (int.TryParse(girdi, out int sayi))
        Console.WriteLine($"'{girdi}' -> {sayi} (geçerli)");
    else
        Console.WriteLine($"'{girdi}' -> sayıya çevrilemedi");
}

// ---------------------------------------------------------------------
Baslik("3. Operatörler");
// ---------------------------------------------------------------------
int a = 17, b = 5;
Console.WriteLine($"{a} + {b} = {a + b}, {a} - {b} = {a - b}, {a} * {b} = {a * b}");
Console.WriteLine($"{a} / {b} = {a / b} (tam sayı bölmesi), {a} % {b} = {a % b}, {a} / {(double)b} = {a / (double)b:F2}");
a += 3;  // a = a + 3
b++;     // b = b + 1
Console.WriteLine($"a={a}, b={b}, a>b: {a > b}, (a>10 && b<10): {a > 10 && b < 10}, (a<10 || b<10): {a < 10 || b < 10}");
string? bos = null;
Console.WriteLine($"null birleştirme (??): {bos ?? "varsayılan değer"}, bos?.Length: {bos?.Length.ToString() ?? "null"}");

// ---------------------------------------------------------------------
Baslik("4. Koşullu İfadeler");
// ---------------------------------------------------------------------
int puan = 78;
if (puan >= 90) Console.WriteLine("Pekiyi");
else if (puan >= 70) Console.WriteLine("İyi");
else if (puan >= 50) Console.WriteLine("Orta");
else Console.WriteLine("Başarısız");

// switch ifadesi (C# 8+) ve ilişkisel kalıplar (C# 9+)
string harfNotu = puan switch
{
    >= 90 => "AA",
    >= 80 => "BA",
    >= 70 => "BB",
    >= 60 => "CB",
    >= 50 => "CC",
    _ => "FF"
};
Console.WriteLine($"Puan {puan} -> Harf notu {harfNotu}");

Gun bugun = Gun.Cumartesi;
Console.WriteLine($"{bugun} hafta sonu mu? {(bugun is Gun.Cumartesi or Gun.Pazar ? "Evet" : "Hayır")}");

// ---------------------------------------------------------------------
Baslik("5. Döngüler");
// ---------------------------------------------------------------------
Console.Write("for ile 1-5: ");
for (int i = 1; i <= 5; i++) Console.Write(i + " ");
Console.WriteLine();

int toplam = 0, sayac = 1;
while (sayac <= 100) { toplam += sayac; sayac++; }
Console.WriteLine($"1'den 100'e kadar toplam (while): {toplam}");

Console.Write("Çift sayılar (continue/break): ");
for (int i = 1; ; i++)
{
    if (i > 12) break;          // döngüyü bitirir
    if (i % 2 != 0) continue;   // sonraki turla devam eder
    Console.Write(i + " ");
}
Console.WriteLine();

// ---------------------------------------------------------------------
Baslik("6. Diziler");
// ---------------------------------------------------------------------
string[] kurslar = { "C#", "Python", "SQL Server", "Scrum" };
int[] notlar = new int[5] { 85, 92, 70, 66, 100 };
Console.WriteLine($"Kurs sayısı: {kurslar.Length}, ilk kurs: {kurslar[0]}, son kurs: {kurslar[^1]}");
Console.WriteLine($"Notlar: ortalama {notlar.Average():F1}, en yüksek {notlar.Max()}, en düşük {notlar.Min()}");
Array.Sort(notlar);
Console.WriteLine("Sıralı notlar: " + string.Join(", ", notlar));
int[,] matris = { { 1, 2, 3 }, { 4, 5, 6 } };   // iki boyutlu dizi
Console.WriteLine($"Matris[1,2] = {matris[1, 2]}, satır: {matris.GetLength(0)}, sütun: {matris.GetLength(1)}");

// ---------------------------------------------------------------------
Baslik("7. Metotlar");
// ---------------------------------------------------------------------
Console.WriteLine($"Topla(3, 4) = {Topla(3, 4)}");
Console.WriteLine($"Selamla(\"Mehmet\") = {Selamla("Mehmet")}");
Console.WriteLine($"Selamla(\"Elif\", \"İyi akşamlar\") = {Selamla("Elif", "İyi akşamlar")}");
Console.WriteLine($"Selamla(selamlama: \"Merhaba\", isim: \"Can\") = {Selamla(selamlama: "Merhaba", isim: "Can")}");

if (BolmeYap(10, 4, out double sonuc)) Console.WriteLine($"10 / 4 = {sonuc}");
if (!BolmeYap(10, 0, out _)) Console.WriteLine("Sıfıra bölme yapılamaz.");

int x = 5;
Artir(ref x);                      // ref: değişkenin kendisi gönderilir
Console.WriteLine($"ref ile artırılan x: {x}");
Console.WriteLine($"params ile toplam: {Toplam(1, 2, 3, 4, 5)}");
Console.WriteLine($"Faktöriyel(10) = {Faktoriyel(10)}");

// Tuple: birden fazla değer döndürmek için
var (en, dusuk, ort) = IstatistikHesapla(new[] { 4, 8, 15, 16, 23, 42 });
Console.WriteLine($"En büyük: {en}, en küçük: {dusuk}, ortalama: {ort:F2}");

// ---------------------------------------------------------------------
Baslik("8. String İşlemleri");
// ---------------------------------------------------------------------
string metin = "  İ-Akademi C# Eğitimi  ";
Console.WriteLine($"Trim: '{metin.Trim()}'");
Console.WriteLine($"Büyük harf: {metin.Trim().ToUpper(tr)}, küçük harf: {metin.Trim().ToLower(tr)}");
Console.WriteLine($"Contains(\"C#\"): {metin.Contains("C#")}, Replace: {metin.Trim().Replace("C#", "Python")}");
Console.WriteLine($"Split: {string.Join(" | ", metin.Trim().Split(' '))}");
var sb = new StringBuilder();   // döngü içinde çok sayıda birleştirme için StringBuilder tercih edilir
for (int i = 1; i <= 5; i++) sb.Append(i).Append(i < 5 ? "-" : "");
Console.WriteLine($"StringBuilder: {sb}");

Console.WriteLine("\nÖrnek tamamlandı.");

// =====================================================================
// Yerel metotlar ve tip tanımları (üst düzey ifadelerden sonra gelmelidir)
// =====================================================================
static void Baslik(string baslik)
{
    Console.WriteLine();
    Console.WriteLine($"=== {baslik} ===");
}

static int Topla(int sayi1, int sayi2) => sayi1 + sayi2;

static string Selamla(string isim, string selamlama = "Merhaba") => $"{selamlama} {isim}!";

static bool BolmeYap(double bolunen, double bolen, out double bolum)
{
    if (bolen == 0) { bolum = 0; return false; }
    bolum = bolunen / bolen;
    return true;
}

static void Artir(ref int deger) => deger++;

static int Toplam(params int[] sayilar) => sayilar.Sum();

static long Faktoriyel(int n) => n <= 1 ? 1 : n * Faktoriyel(n - 1);   // özyineleme (recursion)

static (int enBuyuk, int enKucuk, double ortalama) IstatistikHesapla(int[] dizi)
    => (dizi.Max(), dizi.Min(), dizi.Average());

enum Gun { Pazartesi, Sali, Carsamba, Persembe, Cuma, Cumartesi, Pazar }
