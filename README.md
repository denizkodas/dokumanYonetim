# Doküman Yönetim Sistemi

Bu proje, .NET 8 ve Razor Pages kullanılarak geliştirilmiş bir Doküman Yönetim Sistemi'dir. Kullanıcılar, doküman ekleyebilir, güncelleyebilir, silebilir ve kategorilere ayırabilir. Ayrıca kullanıcı yönetimi ve rol tabanlı erişim kontrolü de sunar.

## Özellikler

- **Kullanıcı Girişi ve Yetkilendirme:** Kullanıcılar sisteme giriş yapabilir, rollerine göre farklı yetkilere sahip olabilir.
- **Doküman Yönetimi:** Doküman ekleme, güncelleme, silme ve listeleme işlemleri.
- **Kategori Yönetimi:** Dokümanlar kategorilere atanabilir ve kategoriler yönetilebilir.
- **Kullanıcı Yönetimi:** Yeni kullanıcı ekleme, güncelleme ve silme işlemleri.
- **Bekleyen Dokümanlar:** Onay bekleyen dokümanların listelenmesi.
- **Modern Arayüz:** Razor Pages ve özelleştirilmiş _Layout ile kullanıcı dostu arayüz.

## Kurulum

1. **Depoyu Klonlayın:**
2. **Visual Studio 2022 ile açın.**
3. **Gerekli NuGet paketlerini yükleyin.**
4. **Veritabanı bağlantı ayarlarını `Baglanti.bgl` üzerinden yapılandırın.**
5. **Projeyi derleyin ve çalıştırın.**

## Kullanım

- Kargo hareketi ekleyin, güncelleyin veya silin.
- Kargo durumlarını ve hareket geçmişini görüntüleyin.
- Raporlama ve bildirim özelliklerini kullanın.

## Temel Sınıflar ve İşlevler

### EntityLayer

- `EntityKargoHareket`: Kargo hareketi bilgilerini tutar.
- `EntityKargoDurum`: Kargonun mevcut durumu ve lokasyonunu tutar.

### DataAccessLayer

- `DalKargoHareket`: Kargo hareketleri için CRUD işlemleri sağlar.
- `KargoHareketEkle`
- `KargoHareketSil`
- `KargoHareketGuncelle`
- `KargoHareketListele`

### BussinesLayer

- `BLKargoHareket`: İş kuralları ve doğrulama içerir.
- `BLKargoHareketEkle`
- `BLKargoHareketGuncelle`
- `BLHareketSil`
- `KargoHareketListele`

### KargoTakipProjesi

- `FormMainMenu`: Ana menü ve kullanıcı arayüzü.
- Farklı formlar üzerinden ürün, sipariş, müşteri, raporlama ve bildirim işlemleri yapılabilir.

## Ekran Görüntüleri

> Ekran görüntüleri ekleyebilirsiniz.

## Katkı Sağlama

Katkı sağlamak için lütfen bir pull request gönderin veya issue açın.

## Lisans

Bu proje MIT lisansı ile lisanslanmıştır.
