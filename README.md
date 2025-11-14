
# 🧩 Case Project – TLSLogistics Customer Order Management System

Bu proje, **.NET 8 Web API** ve **ASP.NET MVC UI** katmanlarından oluşan tam kapsamlı bir **Customer–Order–Stock yönetim sistemi**dir.  
Proje, bir **mülakat (case study)** kapsamında geliştirilmiş olup; kurumsal mimari standartları, temiz kod prensipleri ve katmanlı yapı anlayışını yansıtır.

----------

## 🚀 Özellikler

-   **Tam entegre mimari**: API, Business, DataAccess, Entities, UI
    
-   **Repository + Service Pattern** ile soyutlanmış iş katmanı
    
-   **JWT tabanlı authentication** (cookie üzerinden taşınır)
    
-   **Role-based authorization** (ör. `AdminOnly` policy)
    
-   **MVC UI** tarafında:
    
    -   DevExtreme grid ve modal formlar
        
    -   Dinamik filtreleme, satır içi düzenleme (row double click edit)
        
    -   Global AJAX error handling
        
    -   Modern loading spinner & alert mekanizması
        
-   **Dashboard modülü**:
    
    -   Son 12 ay ciro grafiği
        
    -   Son 7 gün sipariş grafiği
        
    -   Şehre göre sipariş dağılımı
        
    -   En çok satan stoklar ve müşteriler
        
-   **Tam entegre Entity yapısı**:
    
    -   `Customer` ↔ `CustomerAddress`
        
    -   `Order` ↔ `OrderDetail` ↔ `Stock`
        
-   **BaseResponse<T> yapısı** ile tutarlı API yanıt modeli
    

----------

## 🧱 Mimari Yapı

Solution
│
├── Web.API → JWT Authentication + Controller Endpoints
├── Web.UI → MVC Client (Bootstrap + jQuery + DevExtreme)
├── Business (Application)  → Servis katmanı (DashboardService, AuthService vb.)
├── DataAccess              → EF Core Repository + DbContext
├── Entities                → Entity, DTO, Enum, Response modelleri
└── Contracts               → API DTO & interface tanımları` 

**Veri akışı:**

UI 
├─ HttpClient (JwtCookieHandler)
├─ API (Controller)
├─Service
├─Repository
 ├─EF Core (DbContext)` 

----------

## ⚙️ Kurulum

### Gereksinimler

-   .NET 8 SDK
    
-   SQL Server (LocalDB yeterli)
    
-   Visual Studio 2022 veya Rider
    

### 1. Repoyu Klonla

`git clone https://github.com/<kullanici>/<repo-adi>.git cd <repo-adi>` 

### 2. appsettings Dosyaları
**Web.UI/appsettings.json**
**Web.API/appsettings.json**
 dosyalarını size iletmiş olduğum dosyalar ile değiştirin.
**Not:** İletmiş olduğum dosyaların uzantıları *"appsettings.json.webui"* ve *"appsettings.json.webapi"* şeklinde olup, dosyaları aktarmadan önce uzantıları değiştirmeniz gerekmektedir.

### 3. Veritabanını Oluştur

`cd DataAccess
dotnet ef database update` 

**Not:** Güvenli senaryoda admin kullanıcı atama işlemleri sadece api tarafına erişim sağlanarak yapılmaktadır. İlk kullanıcı ataması sonrasında isteğe bağlı ui tarafına kullanıcı ve rol işlemleri için kapı açılabilir ancak tavsiye edilmemektedir. Bu yüzden ilk kullanıcı ataması yapabilmek için proje içerisindeki `Web.Api AuthController` kısmında bulunan  `[Authorize]` attributelarını kaldırıp localden ilk kullanıcı eklemesi, rol ekleme ve rol atama işlemlerini gerçekleştirebilirsiniz. Bu işlemler sonrasında mevcut yapıda `Administrator` rolü ile tam erişim yetkisine rahip olabilirsiniz. 

### 4. Projeyi Çalıştır

Visual Studio’da:

-   Solution’a sağ tık → “Set Startup Projects”
    
-   `Web.API` ve `Web.UI` için **Multiple Startup Projects** ayarla
    
-   F5 (Run)
    

----------

## 🔐 Kimlik Doğrulama Akışı

1.  **Login (AccountController → Login)**  
    Kullanıcı email & şifre girer → API’den JWT alınır.
    
2.  JWT, `AuthToken` adlı **HttpOnly cookie** içinde saklanır.
    
3.  Tüm HttpClient istekleri `JwtCookieHandler` ile bu cookie’den token çeker.
    
4.  API tarafında `[Authorize]` & `[Authorize(Policy = "AdminOnly")]` ile güvenlik sağlanır.
    

----------

## 📊 Dashboard Özeti

**DashboardService**, `Repository` katmanı kullanarak aşağıdaki istatistikleri üretir:

-   **Summary**: Haftalık yeni sipariş, aylık ciro, ort. sepet tutarı
    
-   **SalesLast12**: Son 12 ay ciro trendi (Chart.js)
    
-   **OrdersLast7**: Son 7 gün sipariş adedi (Chart.js)
    
-   **OrdersByStatus**: İptal / Teslim / İşlemde / Teslimatta / İade oranları
    
-   **TopCustomers**: Son 90 günde en çok ciro yapan müşteriler
    
-   **TopStocks**: Son 90 günde en çok satılan ürünler
    
-   **ByCity**: Şehre göre sipariş dağılımı
    

----------

## 🧩 Teknolojiler

**Backend:** ASP.NET Core 8 Web API

**UI:** ASP.NET Core MVC + Bootstrap 5 + jQuery + DevExtreme

**Auth:** JWT + Cookie Authentication

**ORM:** Entity Framework Core 8

**DB:** SQL Server (LocalDB veya MSSQL)

**Chart:** Chart.js

**Patterns:** Repository, Service, DTO, Response Wrapper...

----------

## 💡 Kritik Notlar

-   **Migration** işlemleri `DataAccess` projesinde tutulur
    
-   **Dashboard sorguları** performans için projection bazlıdır (EF çevirilebilir LINQ)
    
-   **Login** sayfası Layout kullanmaz, özel spinner ve alert scriptine sahiptir
    
-   **Global AJAX error handler** tüm 400–401–403 hatalarını yakalar ve yönlendirir
    
-   **Tüm controller sonuçları** `BaseResponse<T>` formatında döner
    

-----
**Yakup İçer**  
Software Developer  | Engineer
📧 yakupicer@gmail.com  
🌐 [linkedin.com/in/yakupicer](https://www.linkedin.com/in/yakup-i%C3%A7er-yakicer/)
