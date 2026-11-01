# 04 - Asenkron Programlama

`async`/`await` ile engellemeyen (non-blocking) ve verimli kod yazmak.

## Konular

- `Task` ve `Task<T>`, `async`/`await` mantığı
- Sıralı çalıştırma ile `Task.WhenAll` karşılaştırması ve süre ölçümü
- `Task.WhenAny` ile ilk biten görevi yakalama
- `CancellationToken` ve zaman aşımı ile iptal
- Asenkron kodda hata yönetimi, `WhenAll` ile birden çok hata
- `IProgress<T>` ile ilerleme bildirimi
- `SemaphoreSlim` ile eşzamanlılığı sınırlama
- `IAsyncEnumerable<T>` ve `await foreach`
- `Parallel.ForEachAsync` ve `Task.Run` (CPU yoğun işler)

Ağ bağlantısı gerekmez; gecikmeler `Task.Delay` ile simüle edilir.

## Çalıştırma

```bash
cd 04-async
dotnet run
```

## Dosyalar

| Dosya | Açıklama |
|-------|----------|
| `Program.cs` | Dokuz bölümlük asenkron programlama örneği |
| `AsyncOrnekleri.csproj` | .NET 8 konsol projesi tanımı |
