// =====================================================================
// 05 - Entity Framework Core (SQLite ile Code First)
// Konular: DbContext, entity, ilişkiler, Fluent API, CRUD, Include,
//          LINQ sorguları, AsNoTracking, toplu güncelleme/silme, transaction
// SQLite kullanıldığı için ek bir veritabanı sunucusu kurmanız gerekmez.
// Çalıştırmak için:  dotnet run
// =====================================================================

using System.Text;
using Microsoft.EntityFrameworkCore;

Console.OutputEncoding = Encoding.UTF8;

using var db = new EgitimContext();

// Her çalıştırmada veritabanını sıfırdan oluşturuyoruz (yalnızca eğitim amaçlı)
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

// ---------------------------------------------------------------------
Console.WriteLine("=== 1. Create: Veri Ekleme ===");
// ---------------------------------------------------------------------
var csharp = new Kurs { Ad = "C# Programlama", SaatSuresi = 40, Ucret = 6500 };
var python = new Kurs { Ad = "Python Temelleri", SaatSuresi = 32, Ucret = 5200 };
var sql = new Kurs { Ad = "SQL Server", SaatSuresi = 36, Ucret = 5800 };
db.Kurslar.AddRange(csharp, python, sql);

var ogrenciler = new List<Ogrenci>
{
    new() { Ad = "Elif", Soyad = "Yılmaz", Sehir = "İstanbul" },
    new() { Ad = "Can", Soyad = "Öztürk", Sehir = "Ankara" },
    new() { Ad = "Merve", Soyad = "Şahin", Sehir = "İstanbul" },
    new() { Ad = "Kaan", Soyad = "Aksoy", Sehir = "İzmir" }
};
db.Ogrenciler.AddRange(ogrenciler);
await db.SaveChangesAsync();   // tüm değişiklikler tek seferde kaydedilir

// Gezinme özellikleri (navigation) üzerinden ilişkili kayıtlar
db.Kayitlar.AddRange(
    new Kayit { Ogrenci = ogrenciler[0], Kurs = csharp, Not = 85 },
    new Kayit { Ogrenci = ogrenciler[0], Kurs = sql, Not = 90 },
    new Kayit { Ogrenci = ogrenciler[1], Kurs = csharp, Not = 70 },
    new Kayit { Ogrenci = ogrenciler[2], Kurs = python, Not = 95 },
    new Kayit { Ogrenci = ogrenciler[3], Kurs = csharp, Not = 60 });
await db.SaveChangesAsync();
Console.WriteLine($"Eklenen: {await db.Ogrenciler.CountAsync()} öğrenci, {await db.Kurslar.CountAsync()} kurs, {await db.Kayitlar.CountAsync()} kayıt");

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 2. Read: Sorgulama ===");
// ---------------------------------------------------------------------
var istanbullular = await db.Ogrenciler
    .AsNoTracking()   // yalnızca okuma yapılacaksa izleme kapatılır, daha hızlıdır
    .Where(o => o.Sehir == "İstanbul")
    .OrderBy(o => o.Ad)
    .ToListAsync();
Console.WriteLine("İstanbul'daki öğrenciler: " + string.Join(", ", istanbullular.Select(o => o.TamAd)));

// Include: ilişkili verileri aynı sorguda yükler (eager loading)
var elif = await db.Ogrenciler
    .Include(o => o.Kayitlar).ThenInclude(k => k.Kurs)
    .FirstAsync(o => o.Ad == "Elif");
foreach (var k in elif.Kayitlar)
    Console.WriteLine($"  {elif.TamAd} -> {k.Kurs.Ad}: {k.Not}");

// Projeksiyon: yalnızca gerekli sütunlar çekilir
var kursOzeti = await db.Kurslar
    .Select(k => new
    {
        k.Ad,
        KayitSayisi = k.Kayitlar.Count,
        Ortalama = k.Kayitlar.Average(x => (double?)x.Not)
    })
    .OrderByDescending(x => x.KayitSayisi)
    .ToListAsync();
foreach (var k in kursOzeti)
    Console.WriteLine($"  {k.Ad,-18} kayıt: {k.KayitSayisi}, not ortalaması: {k.Ortalama:F1}");

// Gruplama
var sehirSayilari = await db.Ogrenciler
    .GroupBy(o => o.Sehir)
    .Select(g => new { Sehir = g.Key, Adet = g.Count() })
    .ToListAsync();
Console.WriteLine("Şehirlere göre: " + string.Join(", ", sehirSayilari.Select(s => $"{s.Sehir}={s.Adet}")));

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 3. Update: Güncelleme ===");
// ---------------------------------------------------------------------
var can = await db.Ogrenciler.FirstAsync(o => o.Ad == "Can");
can.Sehir = "Bursa";                // izlenen nesnedeki değişiklik otomatik algılanır
await db.SaveChangesAsync();
Console.WriteLine($"Can'ın yeni şehri: {(await db.Ogrenciler.FirstAsync(o => o.Id == can.Id)).Sehir}");

// ExecuteUpdate: nesneleri belleğe almadan toplu güncelleme (EF Core 7+)
int etkilenen = await db.Kurslar
    .Where(k => k.SaatSuresi >= 36)
    .ExecuteUpdateAsync(s => s.SetProperty(k => k.Ucret, k => k.Ucret * 1.10));
Console.WriteLine($"Zam yapılan kurs sayısı: {etkilenen}");

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 4. Delete: Silme ===");
// ---------------------------------------------------------------------
var silinecek = await db.Kayitlar.FirstAsync(k => k.Not == 60);
db.Kayitlar.Remove(silinecek);
await db.SaveChangesAsync();
int dusukNotlar = await db.Kayitlar.Where(k => k.Not < 75).ExecuteDeleteAsync();   // toplu silme
Console.WriteLine($"Silinen düşük notlu kayıt sayısı: {dusukNotlar}, kalan kayıt: {await db.Kayitlar.CountAsync()}");

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 5. Transaction ===");
// ---------------------------------------------------------------------
await using (var tx = await db.Database.BeginTransactionAsync())
{
    try
    {
        var yeni = new Ogrenci { Ad = "Sena", Soyad = "Koç", Sehir = "İstanbul" };
        db.Ogrenciler.Add(yeni);
        await db.SaveChangesAsync();

        db.Kayitlar.Add(new Kayit { OgrenciId = yeni.Id, KursId = sql.Id, Not = 88 });
        await db.SaveChangesAsync();

        await tx.CommitAsync();   // iki işlem birlikte onaylanır
        Console.WriteLine("Transaction onaylandı (commit).");
    }
    catch
    {
        await tx.RollbackAsync();   // hata olursa her şey geri alınır
        throw;
    }
}

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 6. Üretilen SQL'i Görmek ===");
// ---------------------------------------------------------------------
var sorgu = db.Ogrenciler.Where(o => o.Sehir == "İstanbul").OrderBy(o => o.Soyad);
Console.WriteLine(sorgu.ToQueryString());

Console.WriteLine($"\nVeritabanı dosyası: {EgitimContext.DosyaAdi}");

// =====================================================================
// Entity sınıfları
// =====================================================================
public class Ogrenci
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public string Soyad { get; set; } = string.Empty;
    public string Sehir { get; set; } = string.Empty;

    public string TamAd => $"{Ad} {Soyad}";   // veritabanına eşlenmez (Fluent API ile hariç tutulur)

    public List<Kayit> Kayitlar { get; set; } = new();
}

public class Kurs
{
    public int Id { get; set; }
    public string Ad { get; set; } = string.Empty;
    public int SaatSuresi { get; set; }
    public double Ucret { get; set; }

    public List<Kayit> Kayitlar { get; set; } = new();
}

public class Kayit
{
    public int Id { get; set; }
    public int OgrenciId { get; set; }
    public int KursId { get; set; }
    public int Not { get; set; }

    public Ogrenci Ogrenci { get; set; } = null!;
    public Kurs Kurs { get; set; } = null!;
}

// =====================================================================
// DbContext
// =====================================================================
public class EgitimContext : DbContext
{
    public const string DosyaAdi = "egitim.db";

    public DbSet<Ogrenci> Ogrenciler => Set<Ogrenci>();
    public DbSet<Kurs> Kurslar => Set<Kurs>();
    public DbSet<Kayit> Kayitlar => Set<Kayit>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DosyaAdi}");

    // Fluent API ile model ayarları
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ogrenci>(e =>
        {
            e.Property(o => o.Ad).HasMaxLength(50).IsRequired();
            e.Property(o => o.Soyad).HasMaxLength(50).IsRequired();
            e.Ignore(o => o.TamAd);
            e.HasIndex(o => o.Sehir);
        });

        modelBuilder.Entity<Kurs>(e =>
        {
            e.Property(k => k.Ad).HasMaxLength(100).IsRequired();
            e.HasIndex(k => k.Ad).IsUnique();
        });

        modelBuilder.Entity<Kayit>(e =>
        {
            e.HasOne(k => k.Ogrenci).WithMany(o => o.Kayitlar).HasForeignKey(k => k.OgrenciId);
            e.HasOne(k => k.Kurs).WithMany(c => c.Kayitlar).HasForeignKey(k => k.KursId);
            e.HasIndex(k => new { k.OgrenciId, k.KursId }).IsUnique();   // aynı kursa iki kez kayıt olunamaz
        });
    }
}
