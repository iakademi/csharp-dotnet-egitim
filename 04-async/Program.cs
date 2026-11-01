// =====================================================================
// 04 - Asenkron Programlama (async / await)
// Konular: Task, await, Task.WhenAll, Task.WhenAny, CancellationToken,
//          zaman aşımı, hata yönetimi, IProgress, SemaphoreSlim,
//          IAsyncEnumerable, Parallel.ForEachAsync
// Ağ bağlantısı gerekmez; gecikmeler Task.Delay ile simüle edilir.
// Çalıştırmak için:  dotnet run
// =====================================================================

using System.Diagnostics;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

// ---------------------------------------------------------------------
Console.WriteLine("=== 1. Sıralı ve Paralel Çalıştırma ===");
// ---------------------------------------------------------------------
var sw = Stopwatch.StartNew();
string a1 = await VeriGetirAsync("Öğrenciler", 300);
string a2 = await VeriGetirAsync("Kurslar", 300);
string a3 = await VeriGetirAsync("Eğitmenler", 300);
Console.WriteLine($"Sıralı  : {a1}, {a2}, {a3} -> {sw.ElapsedMilliseconds} ms");

sw.Restart();
// Görevler önce başlatılır, sonra hep birlikte beklenir
string[] sonuclar = await Task.WhenAll(
    VeriGetirAsync("Öğrenciler", 300),
    VeriGetirAsync("Kurslar", 300),
    VeriGetirAsync("Eğitmenler", 300));
Console.WriteLine($"Paralel : {string.Join(", ", sonuclar)} -> {sw.ElapsedMilliseconds} ms");

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 2. Task.WhenAny: İlk biten kazanır ===");
// ---------------------------------------------------------------------
var yavas = VeriGetirAsync("Yavaş sunucu", 800);
var hizli = VeriGetirAsync("Hızlı sunucu", 150);
Task<string> ilkBiten = await Task.WhenAny(yavas, hizli);
Console.WriteLine($"İlk yanıt veren: {await ilkBiten}");

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 3. İptal (CancellationToken) ve Zaman Aşımı ===");
// ---------------------------------------------------------------------
using (var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(400)))   // 400 ms sonra otomatik iptal
{
    try
    {
        await UzunIslemAsync(cts.Token);
        Console.WriteLine("İşlem tamamlandı.");
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("İşlem zaman aşımına uğradı ve iptal edildi.");
    }
}

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 4. Hata Yönetimi ===");
// ---------------------------------------------------------------------
try
{
    await HataliIslemAsync();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"await ile yakalanan hata: {ex.Message}");
}

// WhenAll birden fazla hata verirse await yalnızca ilkini fırlatır; hepsine Task üzerinden ulaşılır
var toplu = Task.WhenAll(HataliIslemAsync(), HataliIslemAsync());
try { await toplu; }
catch
{
    var hatalar = toplu.Exception!.InnerExceptions;
    Console.WriteLine($"WhenAll toplam hata sayısı: {hatalar.Count}");
}

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 5. İlerleme Bildirimi (IProgress<T>) ===");
// ---------------------------------------------------------------------
var ilerleme = new Progress<int>(yuzde => Console.WriteLine($"  İndirme %{yuzde}"));
await DosyaIndirSimulasyonuAsync(ilerleme);

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 6. Eşzamanlılığı Sınırlama (SemaphoreSlim) ===");
// ---------------------------------------------------------------------
var kapasite = new SemaphoreSlim(2);   // aynı anda en fazla 2 iş
int aktif = 0, enFazla = 0;
var kilit = new object();
var isler = Enumerable.Range(1, 6).Select(async i =>
{
    await kapasite.WaitAsync();
    try
    {
        int simdi = Interlocked.Increment(ref aktif);
        lock (kilit) { enFazla = Math.Max(enFazla, simdi); }
        await Task.Delay(100);
    }
    finally
    {
        Interlocked.Decrement(ref aktif);
        kapasite.Release();
    }
});
await Task.WhenAll(isler);
Console.WriteLine($"6 iş çalıştırıldı, aynı anda en fazla {enFazla} iş aktifti.");

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 7. IAsyncEnumerable<T> ===");
// ---------------------------------------------------------------------
await foreach (var kayit in KayitAkisiAsync(4))
    Console.WriteLine($"  Gelen kayıt: {kayit}");

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 8. Parallel.ForEachAsync ===");
// ---------------------------------------------------------------------
var sirasiz = new System.Collections.Concurrent.ConcurrentBag<int>();
await Parallel.ForEachAsync(Enumerable.Range(1, 8), new ParallelOptions { MaxDegreeOfParallelism = 4 }, async (sayi, ct) =>
{
    await Task.Delay(50, ct);
    sirasiz.Add(sayi * sayi);
});
Console.WriteLine($"Kareler (toplam {sirasiz.Count} adet): {string.Join(", ", sirasiz.OrderBy(x => x))}");

// ---------------------------------------------------------------------
Console.WriteLine("\n=== 9. Task.Run ile CPU Yoğun İşler ===");
// ---------------------------------------------------------------------
long asalSayisi = await Task.Run(() => AsalSay(200_000));
Console.WriteLine($"200.000'e kadar {asalSayisi} asal sayı var.");

// =====================================================================
// Yardımcı metotlar
// =====================================================================
static async Task<string> VeriGetirAsync(string ad, int gecikmeMs)
{
    await Task.Delay(gecikmeMs);   // thread'i bloklamadan bekler (Thread.Sleep kullanmayın)
    return ad;
}

static async Task UzunIslemAsync(CancellationToken token)
{
    for (int adim = 1; adim <= 10; adim++)
    {
        token.ThrowIfCancellationRequested();
        await Task.Delay(200, token);
    }
}

static async Task HataliIslemAsync()
{
    await Task.Delay(50);
    throw new InvalidOperationException("Sunucuya ulaşılamadı (simülasyon).");
}

static async Task DosyaIndirSimulasyonuAsync(IProgress<int> ilerleme)
{
    for (int yuzde = 25; yuzde <= 100; yuzde += 25)
    {
        await Task.Delay(80);
        ilerleme.Report(yuzde);
    }
}

static async IAsyncEnumerable<string> KayitAkisiAsync(int adet)
{
    for (int i = 1; i <= adet; i++)
    {
        await Task.Delay(60);   // her kaydın geç geldiğini simüle eder
        yield return $"Kayıt #{i}";
    }
}

static long AsalSay(int ustSinir)
{
    long adet = 0;
    for (int n = 2; n <= ustSinir; n++)
    {
        bool asal = true;
        for (int b = 2; (long)b * b <= n; b++)
        {
            if (n % b == 0) { asal = false; break; }
        }
        if (asal) adet++;
    }
    return adet;
}
