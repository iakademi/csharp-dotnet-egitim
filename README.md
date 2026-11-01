# C# / .NET Eğitimi

![C#](https://img.shields.io/badge/C%23-12-512BD4?logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![Seviye](https://img.shields.io/badge/seviye-ba%C5%9Flang%C4%B1%C3%A7%20%E2%86%92%20orta-blue)
![Dil](https://img.shields.io/badge/dil-T%C3%BCrk%C3%A7e-red)
![Lisans](https://img.shields.io/badge/lisans-MIT-green)

C# dili ve .NET platformunu temelden başlayarak adım adım öğrenmeniz için hazırlanmış, Türkçe açıklamalı ve doğrudan çalıştırılabilir örnek projeler koleksiyonu.

## Hakkında

Bu depo, **İ-Akademi** yazılım eğitimlerinde kullanılabilecek C#/.NET örneklerini bir araya getirir. Her klasör kendi başına çalışan küçük bir konsol projesidir ve tek bir konuya odaklanır. Kodlar Türkçe yorum satırlarıyla açıklanmıştır; böylece örnekleri okuyarak, değiştirerek ve çalıştırarak öğrenebilirsiniz.

Örnekler **.NET 8 (LTS)** ve **C# 12** ile yazılmıştır.

## İçindekiler

| Modül | Konular |
|-------|---------|
| [01-temeller](./01-temeller) | Değişkenler, veri tipleri, operatörler, koşullar, döngüler, diziler, metotlar, tuple, string işlemleri |
| [02-oop](./02-oop) | Sınıf, kapsülleme, kalıtım, soyut sınıf, arayüz, polimorfizm, record, struct, generic, extension method, event |
| [03-linq-koleksiyonlar](./03-linq-koleksiyonlar) | List, Dictionary, HashSet, Queue, Stack, LINQ operatörleri, gruplama, join, ertelenmiş çalışma |
| [04-async](./04-async) | async/await, Task, WhenAll/WhenAny, iptal, hata yönetimi, IAsyncEnumerable, Parallel.ForEachAsync |
| [05-ef-core](./05-ef-core) | Entity Framework Core ile Code First, ilişkiler, CRUD, Include, toplu işlemler, transaction (SQLite) |

## Kurulum / Başlarken

### Gereksinimler

- [.NET 8 SDK](https://dotnet.microsoft.com/download) (veya daha yeni bir sürüm)
- Bir kod düzenleyici: [Visual Studio](https://visualstudio.microsoft.com/), [Visual Studio Code](https://code.visualstudio.com/) (C# Dev Kit eklentisiyle) veya JetBrains Rider

Kurulumu doğrulamak için:

```bash
dotnet --version
```

### Depoyu klonlama ve çalıştırma

```bash
git clone https://github.com/iakademi/csharp-dotnet-egitim.git
cd csharp-dotnet-egitim

# Bir modülü çalıştırın
cd 01-temeller
dotnet run
```

Her klasörde kendi `.csproj` dosyası bulunduğundan, istediğiniz modülün klasörüne girip `dotnet run` komutunu vermeniz yeterlidir. Depo kökünden çalıştırmak isterseniz:

```bash
dotnet run --project 02-oop
```

> `05-ef-core` modülü ilk çalıştırmada NuGet üzerinden `Microsoft.EntityFrameworkCore.Sqlite` paketini indirir; internet bağlantısı gerekir.

## Klasör Yapısı

```text
csharp-dotnet-egitim/
├── 01-temeller/
│   ├── Program.cs
│   ├── Temeller.csproj
│   └── README.md
├── 02-oop/
│   ├── Program.cs
│   ├── Oop.csproj
│   └── README.md
├── 03-linq-koleksiyonlar/
│   ├── Program.cs
│   ├── LinqKoleksiyonlar.csproj
│   └── README.md
├── 04-async/
│   ├── Program.cs
│   ├── AsyncOrnekleri.csproj
│   └── README.md
├── 05-ef-core/
│   ├── Program.cs
│   ├── EfCoreOrnek.csproj
│   └── README.md
├── .gitignore
├── LICENSE
└── README.md
```

## Önerilen Çalışma Sırası

1. Modülleri numara sırasıyla ilerleyin.
2. Önce `Program.cs` dosyasını okuyun, sonra çalıştırıp çıktıyla karşılaştırın.
3. Kodda küçük değişiklikler yapıp sonucu gözlemleyin; hata mesajlarını okumak öğrenmenin parçasıdır.
4. Her modülün sonunda kendi küçük örneğinizi yazarak konuyu pekiştirin.

## Katkıda Bulunma

Katkılarınızı bekliyoruz. Bir hata bulduysanız ya da yeni bir örnek eklemek istiyorsanız:

1. Depoyu fork'layın.
2. Yeni bir dal oluşturun: `git checkout -b ozellik/yeni-ornek`
3. Değişikliklerinizi açıklayıcı bir mesajla commit'leyin.
4. Dalınızı gönderin ve bir Pull Request açın.

Kod eklerken lütfen mevcut yazım stiline uyun: Türkçe yorumlar, anlaşılır isimlendirme ve çalışan kod. Önerileriniz için [Issues](https://github.com/iakademi/csharp-dotnet-egitim/issues) bölümünü de kullanabilirsiniz.

## Lisans

Bu proje [MIT Lisansı](./LICENSE) ile lisanslanmıştır.

## İ-Akademi Hakkında

[İ-Akademi](https://iakademi.com), İstanbul merkezli bir BT eğitim merkezidir. Microsoft, Office ve Power Platform, Cisco, siber güvenlik, yazılım geliştirme, PMP/Scrum gibi alanlarda eğitimler sunar; ayrıca Pearson VUE ve Kryterion sınav merkezi olarak hizmet verir.

- Web sitesi: <https://iakademi.com>
- Yazılım eğitimleri için: [iakademi.com](https://iakademi.com) adresindeki yazılım kategorisine göz atabilirsiniz.
