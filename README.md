## Süreç Açýklamasý

### Sipariþ Süreci

1. **Sipariþ Talebi**  
   Kullanýcý, `OrderService` üzerinden bir sipariþ isteðinde bulunur.

2. **Sipariþ Bilgisi Yayýnlama**  
   Sipariþ bilgileri, `OrderCreatedEvent` olarak MassTransit üzerinden yayýnlanýr.

3. **Stok Kontrolü**  
   - `StockService`, sipariþ bilgilerini alýr ve stok bilgilerini MongoDB üzerinde kontrol eder.
   - **Yeterli Stok Durumu**  
     Eðer stok yeterli ise, `StockReserveEvent` yayýnlanýr.
   - **Yetersiz Stok Durumu**  
     Eðer stok yetersiz ise, `StockNotReservedEvent` yayýnlanýr.

4. **Ödeme Ýþlemleri**  
   - `StockReserveEvent` yayýnlandýðýnda, `PaymentService` ödeme iþlemlerini simüle eder.
   - **Baþarýlý Ödeme Durumu**  
     Ödeme baþarýlý olursa, `PaymentCompletedEvent` yayýnlanýr.
   - **Baþarýsýz Ödeme Durumu**  
     Ödeme baþarýsýz olursa, `PaymentFailedEvent` yayýnlanýr.

5. **Sipariþ Durum Güncelleme**  
   - **Baþarýlý Ödeme Durumu**  
     `OrderService`, `PaymentCompletedEvent`'ini alarak sipariþin durumunu "Tamamlandý" olarak günceller.
   - **Baþarýsýz Ödeme Durumu**  
     `OrderService`, `PaymentFailedEvent`'ini alarak sipariþin durumunu "Baþarýsýz" olarak günceller.

## Kullanýlan Teknolojiler

- **C#**  
  Proje geliþtirme dili olarak kullanýlmýþtýr.

- **Entity Framework**  
  Veritabaný iþlemleri için kullanýlan ORM (Object-Relational Mapping) aracýdýr.

- **MassTransit (CloudAMQP ? RabbitMQ)**  
  Mikro hizmetler arasýnda mesajlaþma ve olay yönetimi için kullanýlýr. CloudAMQP ve RabbitMQ, mesajlaþma altyapýsý olarak kullanýlmýþtýr.

- **PostgreSQL (Docker)**  
  Ýliþkisel veritabaný yönetim sistemi olarak kullanýlýr ve Docker üzerinde çalýþtýrýlýr. Order Kayýtlarý tutulmaktadýr.

- **MongoDB Atlas (Docker)**  
  NoSQL veritabaný olarak kullanýlýr ve Docker üzerinde çalýþtýrýlýr. Stock bilgileri takip edilir.

