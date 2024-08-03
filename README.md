## S�re� A��klamas�

### Sipari� S�reci

1. **Sipari� Talebi**  
   Kullan�c�, `OrderService` �zerinden bir sipari� iste�inde bulunur.

2. **Sipari� Bilgisi Yay�nlama**  
   Sipari� bilgileri, `OrderCreatedEvent` olarak MassTransit �zerinden yay�nlan�r.

3. **Stok Kontrol�**  
   - `StockService`, sipari� bilgilerini al�r ve stok bilgilerini MongoDB �zerinde kontrol eder.
   - **Yeterli Stok Durumu**  
     E�er stok yeterli ise, `StockReserveEvent` yay�nlan�r.
   - **Yetersiz Stok Durumu**  
     E�er stok yetersiz ise, `StockNotReservedEvent` yay�nlan�r.

4. **�deme ��lemleri**  
   - `StockReserveEvent` yay�nland���nda, `PaymentService` �deme i�lemlerini sim�le eder.
   - **Ba�ar�l� �deme Durumu**  
     �deme ba�ar�l� olursa, `PaymentCompletedEvent` yay�nlan�r.
   - **Ba�ar�s�z �deme Durumu**  
     �deme ba�ar�s�z olursa, `PaymentFailedEvent` yay�nlan�r.

5. **Sipari� Durum G�ncelleme**  
   - **Ba�ar�l� �deme Durumu**  
     `OrderService`, `PaymentCompletedEvent`'ini alarak sipari�in durumunu "Tamamland�" olarak g�nceller.
   - **Ba�ar�s�z �deme Durumu**  
     `OrderService`, `PaymentFailedEvent`'ini alarak sipari�in durumunu "Ba�ar�s�z" olarak g�nceller.

## Kullan�lan Teknolojiler

- **C#**  
  Proje geli�tirme dili olarak kullan�lm��t�r.

- **Entity Framework**  
  Veritaban� i�lemleri i�in kullan�lan ORM (Object-Relational Mapping) arac�d�r.

- **MassTransit (CloudAMQP ? RabbitMQ)**  
  Mikro hizmetler aras�nda mesajla�ma ve olay y�netimi i�in kullan�l�r. CloudAMQP ve RabbitMQ, mesajla�ma altyap�s� olarak kullan�lm��t�r.

- **PostgreSQL (Docker)**  
  �li�kisel veritaban� y�netim sistemi olarak kullan�l�r ve Docker �zerinde �al��t�r�l�r. Order Kay�tlar� tutulmaktad�r.

- **MongoDB Atlas (Docker)**  
  NoSQL veritaban� olarak kullan�l�r ve Docker �zerinde �al��t�r�l�r. Stock bilgileri takip edilir.

