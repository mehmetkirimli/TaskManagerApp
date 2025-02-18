1️⃣ Identity Kullanmanın Avantajları
✅ Hazır Kullanıcı Yönetimi:
ASP.NET Core Identity, kullanıcı oturum yönetimi, şifreleme, e-posta doğrulama, şifre sıfırlama gibi özellikleri sıfırdan kod yazmadan halletmemizi sağlar.

✅ Kullanıcı Rollerini Yönetme:
Eğer ileride Admin, Kullanıcı, Editör gibi roller eklemek istersen, Identity bunu destekler. IdentityRole kullanarak Role-Based Authorization yapabilirsin.

✅ Güvenlik Standartlarına Uygun:
Identity, kullanıcı şifrelerini hashleyerek ve salting işlemi yaparak saklar. Kendimiz manuel olarak bir Users tablosu yapsak, bu şifreleme işlemlerini kendimizin yönetmesi gerekir.

✅ JWT Authentication ile Kolay Entegrasyon:
Sistemimizi JWT ile authentication yapacak şekilde kuracağız. ASP.NET Identity, SignInManager ve UserManager ile JWT Token üretmeyi ve doğrulamayı kolaylaştırır.

2️⃣ Identity Kullanmazsak Ne Olur?
Eğer Identity kullanmazsak, Users adında manuel bir tablo oluşturmamız ve şifreleme, token üretme, yetkilendirme gibi işlemleri kendimiz yazmamız gerekir.

Bunun için:

Kullanıcı tablosunu (Users) manuel olarak oluştururuz.
Şifreleri güvenli şekilde saklamak için bcrypt veya SHA256 gibi hashing algoritmaları kullanırız.
Token yönetimi için JWT ile manuel doğrulama mekanizması yazarız.
❌ Dezavantajlar:

Kimlik doğrulama ve yetkilendirme kodları fazla olur.
Şifreleme ve güvenlik bizim sorumluluğumuzda olur.
Role-based access control (RBAC) sistemini kendimiz yazmak zorunda kalırız.
3️⃣ Identity Kullanarak Nasıl Çalışıyor?
Identity ile gittiğimizde ApplicationUser : IdentityUser yaptığımızda şu avantajları elde ediyoruz:

IdentityUser zaten bir kullanıcı tablosuna sahip.
Şifreleri güvenli şekilde hashleyip saklıyor.
JWT ile entegrasyonu kolay oluyor.
Gelişmiş özellikler (Role-based access, Two-Factor Authentication) direkt geliyor.
Son Karar: Identity Kullanmalı mıyız?
✅ Eğer kullanıcı girişi, kimlik doğrulama ve yetkilendirme işlemleriyle uğraşmadan hazır bir sistem kullanmak istiyorsan Identity mantıklı.

❌ Eğer kimlik doğrulamayı kendin yönetmek istiyorsan ve kullanıcı yönetimini çok basit tutmayı planlıyorsan Identity kullanmaya gerek kalmayabilir.

Controller: Token doğrulama işlemi controller içinde yapılmaz. Token yalnızca header'dan alınır.
AuthService: Kullanıcıyı doğrulama ve JWT token oluşturma işlemleri burada yapılır.
TaskService: Token doğrulama ve kullanıcının kendisine ait verileri filtreleme işlemleri burada yapılır.
Bu yapı sayesinde token doğrulama ve kullanıcı bilgisi çıkarımı işlemleri tamamen servislerde yapılacak, controller yalnızca gerekli parametreleri alıp uygun servislere yönlendirecek.
