# 02 - Nesne Yönelimli Programlama

C# ile nesne yönelimli tasarımın temel kavramları.

## Konular

- Sınıflar, kurucu metotlar, özellikler (`get`/`set`/`init`) ve kapsülleme
- Kalıtım, `abstract` sınıflar, `virtual`/`override`
- Arayüzler (`interface`) ve polimorfizm
- Pattern matching ile tür denetimi (`switch` ifadesi, `when`)
- `record` (değer eşitliği, `with` ifadesi) ve `struct` (değer tipi)
- Generic sınıflar ve kısıtlar (`where T : class`)
- Extension method (genişletme metotları)
- Delegate ve `event` ile olay tabanlı tasarım
- Hata fırlatma ve yakalama (`throw`, `try/catch`)

## Çalıştırma

```bash
cd 02-oop
dotnet run
```

## Dosyalar

| Dosya | Açıklama |
|-------|----------|
| `Program.cs` | Banka hesabı, çalışan hiyerarşisi, depo ve sipariş olayı örnekleri |
| `Oop.csproj` | .NET 8 konsol projesi tanımı |
