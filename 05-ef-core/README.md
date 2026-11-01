# 05 - Entity Framework Core

Entity Framework Core ile nesne-ilişkisel eşleme (ORM). Örnek, kurulum gerektirmeyen **SQLite** veritabanı kullanır.

## Konular

- Code First yaklaşımı, entity sınıfları ve `DbContext`
- Fluent API ile model yapılandırması (uzunluk, zorunluluk, indeks, benzersizlik)
- Bire-çok ilişkiler ve gezinme özellikleri (`Ogrenci` - `Kayit` - `Kurs`)
- CRUD işlemleri: `Add`, `Remove`, `SaveChangesAsync`
- Sorgulama: `Where`, `Include/ThenInclude`, projeksiyon, `GroupBy`, `AsNoTracking`
- Toplu işlemler: `ExecuteUpdateAsync`, `ExecuteDeleteAsync`
- Transaction yönetimi
- `ToQueryString()` ile üretilen SQL'i görme

## Çalıştırma

```bash
cd 05-ef-core
dotnet run
```

İlk çalıştırmada `Microsoft.EntityFrameworkCore.Sqlite` paketi NuGet'ten indirilir. Program her çalıştırmada `egitim.db` dosyasını silip yeniden oluşturur; dosya `.gitignore` kapsamındadır.

## Dosyalar

| Dosya | Açıklama |
|-------|----------|
| `Program.cs` | Entity sınıfları, `EgitimContext` ve CRUD örnekleri |
| `EfCoreOrnek.csproj` | .NET 8 projesi ve EF Core SQLite paket referansı |
