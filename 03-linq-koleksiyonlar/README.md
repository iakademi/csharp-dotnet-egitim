# 03 - Koleksiyonlar ve LINQ

Veri kümeleriyle çalışmak için koleksiyon türleri ve LINQ sorguları.

## Konular

- `List<T>`, `Dictionary<TKey,TValue>`, `HashSet<T>`, `Queue<T>`, `Stack<T>`
- Filtreleme (`Where`), dönüştürme (`Select`), sıralama (`OrderBy`, `ThenBy`)
- `Sum`, `Average`, `Min/Max`, `MinBy/MaxBy`, `Any`, `All`, `First`, `FirstOrDefault`
- `Take`, `Skip`, `Distinct`, `Zip`, `Chunk`
- `GroupBy` ile gruplama ve gruplar üzerinde toplama
- `Join` ve `GroupJoin` (method syntax) ile query syntax karşılaştırması
- Ertelenmiş çalışma (deferred execution) ve `ToList()` ile anlık çalıştırma
- `ToDictionary` ve `ToLookup`

## Çalıştırma

```bash
cd 03-linq-koleksiyonlar
dotnet run
```

## Dosyalar

| Dosya | Açıklama |
|-------|----------|
| `Program.cs` | Öğrenci, kurs ve kayıt listeleri üzerinde yapılan LINQ örnekleri |
| `LinqKoleksiyonlar.csproj` | .NET 8 konsol projesi tanımı |
