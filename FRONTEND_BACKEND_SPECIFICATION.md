# เอกสารข้อกำหนดการเชื่อมโยงระบบ Frontend และ Backend (FriendHouse)
## FRONTEND TO BACKEND SPECIFICATION & VIEWMODEL CONTRACTS

เอกสารฉบับนี้จัดทำขึ้นเพื่อเป็นคู่มือมาตรฐานสำหรับนักพัฒนาฝั่ง **Backend / Full-Stack** ในการเชื่อมโยง Business Logic, Database Access, API Services และ Data Binding เข้ากับส่วนติดต่อผู้ใช้งาน (Frontend Razor Views `.cshtml`) ของระบบร้านฟิกเกอร์และโมเดล **FriendHouse**

---

## สารบัญระบบตามบทบาทผู้ใช้งาน (User Roles)
1. [กลุ่มที่ 1: บัญชีและสมาชิกลูกค้า (Account & Customer)](#1-กลุ่มบัญชีและสมาชิกลูกค้า-account--customer)
2. [กลุ่มที่ 2: หน้าร้านและการสั่งจองพรีออเดอร์ (Storefront & Pre-order)](#2-กลุ่มหน้าร้านและการสั่งจองพรีออเดอร์-storefront--pre-order)
3. [กลุ่มที่ 3: พนักงานหน้าร้าน (Staff Console)](#3-กลุ่มพนักงานหน้าร้าน-staff-console)
4. [กลุ่มที่ 4: เจ้าของร้าน (Owner Portal)](#4-กลุ่มเจ้าของร้าน-owner-portal)
5. [กลุ่มที่ 5: ผู้ดูแลระบบ (IT Support Portal)](#5-กลุ่มผู้ดูแลระบบ-it-support-portal)
6. [กลุ่มที่ 6: คนทำบัญชีและการเงิน (Accountant Portal)](#6-กลุ่มคนทำบัญชีและการเงิน-accountant-portal)

---

## 1. กลุ่มบัญชีและสมาชิกลูกค้า (Account & Customer)

### 1.1 หน้าเข้าสู่ระบบ (Login)
- **View File**: `Views/Account/Login.cshtml`
- **Controller**: `AccountController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public IActionResult Login() => View();
  ```
- **Action (POST)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
  ```
- **ViewModel Definition**:
  ```csharp
  namespace Khetsophon_CSI402_8456.ViewModels;

  public class LoginViewModel
  {
      [Required(ErrorMessage = "กรุณากรอกอีเมล")]
      [EmailAddress(ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง")]
      public string Email { get; set; } = string.Empty;

      [Required(ErrorMessage = "กรุณากรอกรหัสผ่าน")]
      [DataType(DataType.Password)]
      public string Password { get; set; } = string.Empty;

      public bool RememberMe { get; set; } = false;
  }
  ```
- **พารามิเตอร์ที่ส่งจากฟอร์ม**:
  - `[Email: string, Password: string, RememberMe: bool]`
- **สิ่งที่ Controller ต้องจัดการ**:
  - ตรวจสอบความถูกต้องของบัญชีผู้ใช้ในระบบ Identity / Database
  - สร้าง ClaimsPrincipal และลงทะเบียน Authentication Cookie
  - นำทาง (Redirect) ไปยังหน้าร้าน `Home/Index` หรือ Dashboard ตามสิทธิ์ (Role)

---

### 1.2 หน้าสมัครสมาชิกใหม่ (Register)
- **View File**: `Views/Account/Register.cshtml`
- **Controller**: `AccountController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public IActionResult Register() => View();
  ```
- **Action (POST)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> Register(RegisterViewModel model)
  ```
- **ViewModel Definition**:
  ```csharp
  namespace Khetsophon_CSI402_8456.ViewModels;

  public class RegisterViewModel
  {
      [Required(ErrorMessage = "กรุณากรอกอีเมล")]
      [EmailAddress(ErrorMessage = "รูปแบบอีเมลไม่ถูกต้อง")]
      public string Email { get; set; } = string.Empty;

      [Required(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์")]
      [Phone(ErrorMessage = "เบอร์โทรศัพท์ไม่ถูกต้อง")]
      public string Phone { get; set; } = string.Empty;

      [Required(ErrorMessage = "กรุณากรอกรหัสผ่าน")]
      [StringLength(100, MinimumLength = 8, ErrorMessage = "รหัสผ่านต้องมีความยาวอย่างน้อย 8 ตัวอักษร")]
      [DataType(DataType.Password)]
      public string Password { get; set; } = string.Empty;

      [DataType(DataType.Password)]
      [Compare("Password", ErrorMessage = "รหัสผ่านยืนยันไม่ตรงกัน")]
      public string ConfirmPassword { get; set; } = string.Empty;
  }
  ```
- **พารามิเตอร์ที่ส่งจากฟอร์ม**:
  - `[Email: string, Phone: string, Password: string, ConfirmPassword: string]`
- **สิ่งที่ Controller ต้องจัดการ**:
  - ตรวจสอบความซ้ำซ้อนของอีเมลและเบอร์โทรศัพท์
  - แฮชรหัสผ่าน (Password Hashing: BCrypt หรือ PBKDF2)
  - สร้างบัญชี Role `Customer` และตั้งต้นแต้มสะสมเริ่มต้น 0 แต้ม

---

### 1.3 หน้าเปลี่ยน / รีเซ็ตรหัสผ่าน (Reset Password)
- **View File**: `Views/Account/ResetPassword.cshtml`
- **Controller**: `AccountController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public IActionResult ResetPassword() => View();
  ```
- **Action (POST)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
  ```
- **ViewModel Definition**:
  ```csharp
  public class ResetPasswordViewModel
  {
      [Required]
      [DataType(DataType.Password)]
      public string CurrentPassword { get; set; } = string.Empty;

      [Required]
      [StringLength(100, MinimumLength = 8)]
      [DataType(DataType.Password)]
      public string NewPassword { get; set; } = string.Empty;

      [Compare("NewPassword")]
      public string ConfirmNewPassword { get; set; } = string.Empty;
  }
  ```
- **พารามิเตอร์ที่ส่งจากฟอร์ม**:
  - `[CurrentPassword: string, NewPassword: string, ConfirmNewPassword: string]`

---

### 1.4 หน้าโปรไฟล์ผู้ใช้ (Profile)
- **View File**: `Views/Account/Profile.cshtml`
- **Controller**: `AccountController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Profile()
  ```
- **Action (POST - Edit Profile)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> EditProfile(UserProfileViewModel model)
  ```
- **ViewModel Definition**:
  ```csharp
  public class UserProfileViewModel
  {
      public int Id { get; set; }
      public string Name { get; set; } = string.Empty;
      public string Email { get; set; } = string.Empty;
      public string Phone { get; set; } = string.Empty;
      public string Role { get; set; } = "Customer";
      public int RewardPoints { get; set; }
      public int PreorderActiveCount { get; set; }
      public int CompletedOrdersCount { get; set; }
      public string? AvatarUrl { get; set; }
      public IFormFile? AvatarFile { get; set; }
  }
  ```
- **พารามิเตอร์ที่ส่งจากฟอร์ม**:
  - `[FullName: string, Email: string, Phone: string, AvatarFile: IFormFile (optional)]`

---

## 2. กลุ่มหน้าร้านและการสั่งจองพรีออเดอร์ (Storefront & Pre-order)

### 2.1 หน้าแรกของร้าน (Home Storefront)
- **View File**: `Views/Home/Index.cshtml`
- **Controller**: `HomeController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Index()
  ```
- **ViewModel Definition**:
  ```csharp
  public class StorefrontViewModel
  {
      public ProductItemDto FeaturedHeroProduct { get; set; } = new();
      public List<PromotionDto> ActivePromotions { get; set; } = new();
      public List<ProductItemDto> RecommendedFigures { get; set; } = new();
      public List<ProductItemDto> PreorderHighlights { get; set; } = new();
  }
  ```
- **สิ่งที่ Controller ต้องจัดการ**:
  - ดึงข้อมูลสินค้าพรีออเดอร์เด่น (ที่มีไฟล์ 3D `.glb` หรือภาพ 360)
  - ดึงโปรโมชั่นที่เปิดใช้งานอยู่ 5 รูปแบบ

---

### 2.2 หน้าคอลเลกชันสินค้าทั้งหมด (Catalog Collection)
- **View File**: `Views/Home/Collection.cshtml`
- **Controller**: `HomeController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Collection(string? series, string? category, string? status, string? search, int page = 1)
  ```
- **พารามิเตอร์ Query Strings**:
  - `[series: string?, category: string?, status: string?, search: string?, page: int]`

---

### 2.3 หน้ารายละเอียดสินค้าและโมดอลยืนยันการจอง (Product Detail & Preorder Modal)
- **View File**: `Views/Home/ProductDetail.cshtml`
- **Controller**: `HomeController`
- **Action (GET - รายละเอียดสินค้า)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> ProductDetail(int id)
  ```
- **Action (POST - ยืนยันการสั่งจองพรีออเดอร์จากโมดอล)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> ConfirmPreorder(PreorderBookingDto booking)
  ```
- **ViewModel Definition**:
  ```csharp
  public class PreorderBookingDto
  {
      [Required]
      public int ProductId { get; set; }

      [Required]
      [Range(1, 10)]
      public int Quantity { get; set; } = 1;

      [Required(ErrorMessage = "กรุณาระบุวันที่สะดวกเข้ารับ")]
      public DateTime PickupDate { get; set; }

      [Required(ErrorMessage = "กรุณาระบุเบอร์โทรศัพท์ติดต่อ")]
      public string ContactPhone { get; set; } = string.Empty;

      public string? Note { get; set; }
  }
  ```
- **พารามิเตอร์ที่ส่งจากฟอร์มโมดอล**:
  - `[ProductId: int, Quantity: int, PickupDate: DateTime, ContactPhone: string]`
- **สิ่งที่ Controller ต้องจัดการ**:
  - บันทึกคำสั่งจองเข้าตาราง `Preorders`
  - สร้างรหัสอ้างอิงใบจอง เช่น `#PO-000482`
  - Redirect ไปยัง `Home/PreorderStatus` พร้อมส่งเลขที่ใบจอง

---

### 2.4 หน้าตะกร้าสินค้า (Cart)
- **View File**: `Views/Home/Cart.cshtml`
- **Controller**: `HomeController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Cart()
  ```
- **Action (POST - ยืนยันตะกร้าเข้าสู่คิวพรีออเดอร์)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> CheckoutCart(List<int> selectedCartItemIds)
  ```

---

### 2.5 หน้าตรวจสอบสถานะพรีออเดอร์ของฉัน (Preorder Status)
- **View File**: `Views/Home/PreorderStatus.cshtml`
- **Controller**: `HomeController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> PreorderStatus(string? orderNo)
  ```
- **พารามิเตอร์ Query String**:
  - `[orderNo: string (เช่น PO-000482)]`

---

### 2.6 หน้าแคตตาล็อกแลกแต้มรางวัล (Rewards Catalog)
- **View File**: `Views/Home/Rewards.cshtml`
- **Controller**: `HomeController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Rewards()
  ```
- **Action (POST - ยืนยันการแลกรางวัล)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> RedeemReward(int rewardId)
  ```

---

### 2.7 หน้าโปรโมชั่นทั้งหมด (Promotions Storefront)
- **View File**: `Views/Home/Promotion.cshtml`
- **Controller**: `HomeController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Promotion()
  ```

---

## 3. กลุ่มพนักงานหน้าร้าน (Staff Console)

### 3.1 หน้ารายการสต็อกสินค้าคงเหลือ (Stock Inventory Index)
- **View File**: `Views/Staff/Index.cshtml`
- **Controller**: `StaffController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Index(string? searchQuery, string? category, string? stockStatus)
  ```
- **พารามิเตอร์ Query Strings / Filter Form**:
  - `[searchQuery: string?, category: string?, stockStatus: string?]`
- **ViewModel Definition**:
  ```csharp
  public class StockInventoryViewModel
  {
      public List<StockItemDto> Items { get; set; } = new();
      public int TotalCount { get; set; }
      public int LowStockCount { get; set; }
      public int OutOfStockCount { get; set; }
  }

  public class StockItemDto
  {
      public int Id { get; set; }
      public string Name { get; set; } = string.Empty;
      public string Sku { get; set; } = string.Empty;
      public string Category { get; set; } = string.Empty;
      public int CurrentQuantity { get; set; }
      public int SafetyThreshold { get; set; } = 5;
      public string Status { get; set; } = "ปกติ"; // ปกติ, ใกล้หมด, หมด
      public string StorageLocation { get; set; } = string.Empty;
      public decimal RetailPrice { get; set; }
      public DateTime LastReceivedDate { get; set; }
  }
  ```

---

### 3.2 หน้าสแกนรับสินค้าเข้าสต็อก (Scan Receive)
- **View File**: `Views/Staff/ScanReceive.cshtml`
- **Controller**: `StaffController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public IActionResult ScanReceive() => View();
  ```
- **Action (POST - รับเข้าผ่าน Manual หรือ Scanner)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> ScanReceive(ScanReceiveInputDto input)
  ```
- **ViewModel Definition**:
  ```csharp
  public class ScanReceiveInputDto
  {
      [Required(ErrorMessage = "กรุณาระบุรหัสบาร์โค้ดหรือ SKU")]
      public string Sku { get; set; } = string.Empty;

      [Required]
      [Range(1, 1000, ErrorMessage = "จำนวนรับเข้าต้องมากกว่า 0")]
      public int Quantity { get; set; } = 1;

      public string? BatchNumber { get; set; }
      public string? Note { get; set; }
  }
  ```
- **พารามิเตอร์ที่ส่งจากฟอร์ม**:
  - `[Sku: string, Quantity: int]`

---

### 3.3 หน้าเคาน์เตอร์พรีออเดอร์หน้าร้าน (Pre-order Pickup Counter)
- **View File**: `Views/Staff/CounterPreorder.cshtml`
- **Controller**: `StaffController`
- **Action (GET / POST)**:
  ```csharp
  [HttpGet]
  public IActionResult CounterPreorder() => View();

  [HttpPost]
  public async Task<IActionResult> CounterPreorder(CounterPreorderViewModel model)
  ```
- **ViewModel Definition**:
  ```csharp
  namespace Khetsophon_CSI402_8456.ViewModels;

  public class CounterPreorderViewModel
  {
      [Required(ErrorMessage = "กรุณาระบุเบอร์โทรหรือเลขที่ใบจอง")]
      public string ReservationNumber { get; set; } = string.Empty;

      public string? Phone { get; set; }
      public string? RedemptionNumber { get; set; }
  }
  ```
- **พารามิเตอร์ที่ส่งจากฟอร์ม**:
  - `[ReservationNumber: string (เบอร์โทรศัพท์ของลูกค้า หรือรหัสใบจอง เช่น 081-234-5000 หรือ PO-000482)]`
- **สิ่งที่ Controller ต้องจัดการ**:
  - ค้นหาคำสั่งพรีออเดอร์ที่สถานะ `พร้อมรับสินค้าที่ร้าน`
  - คำนวณยอดเงินมัดจำที่ชำระแล้ว และยอดส่วนที่เหลือที่ต้องชำระหน้าร้าน
  - บันทึกการส่งมอบสินค้าและพิมพ์ใบเสร็จรับเงิน

---

## 4. กลุ่มเจ้าของร้าน (Owner Portal)

### 4.1 หน้าจัดการโปรโมชั่น (Promotions Index)
- **View File**: `Views/Owner/Promotions.cshtml`
- **Controller**: `OwnerController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Promotions()
  ```
- **Action (POST - Toggle Status)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> TogglePromotionStatus(int id, bool isActive)
  ```
- **พารามิเตอร์**:
  - `[id: int, isActive: bool]`

---

### 4.2 หน้าสร้างโปรโมชั่นใหม่ (Add Promotion)
- **View File**: `Views/Owner/AddPromotion.cshtml`
- **Controller**: `OwnerController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public IActionResult AddPromotion() => View();
  ```
- **Action (POST)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> AddPromotion(CreatePromotionViewModel model)
  ```
- **ViewModel Definition**:
  ```csharp
  public class CreatePromotionViewModel
  {
      [Required(ErrorMessage = "กรุณาระบุชื่อโปรโมชั่น")]
      public string Name { get; set; } = string.Empty;

      [Required(ErrorMessage = "กรุณาระบุรหัสโค้ดโปรโมชั่น")]
      public string Code { get; set; } = string.Empty;

      [Required]
      public string PromoType { get; set; } = "percent"; // percent, fixed, bogo, bundle, double_points

      public decimal? DiscountValue { get; set; }
      public decimal? MinSpend { get; set; }

      [Required]
      public DateTime StartDate { get; set; }

      [Required]
      public DateTime EndDate { get; set; }

      public string ApplicableScope { get; set; } = "all"; // all, clearance, scale_figures, preorder_only
      public int? UsageLimit { get; set; }
      public bool IsActive { get; set; } = true;
  }
  ```
- **พารามิเตอร์ที่ส่งจากฟอร์ม**:
  - `[Name: string, Code: string, PromoType: string, DiscountValue: decimal?, MinSpend: decimal?, StartDate: DateTime, EndDate: DateTime, ApplicableScope: string, UsageLimit: int?, IsActive: bool]`

---

### 4.3 หน้าแก้ไขโปรโมชั่น (Edit Promotion)
- **View File**: `Views/Owner/EditPromotion.cshtml`
- **Controller**: `OwnerController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> EditPromotion(int id)
  ```
- **Action (POST)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> EditPromotion(EditPromotionViewModel model)
  ```
- **ViewModel Definition**:
  ```csharp
  public class EditPromotionViewModel : CreatePromotionViewModel
  {
      [Required]
      public int Id { get; set; }
  }
  ```
- **พารามิเตอร์ที่ส่งจากฟอร์ม**:
  - `[Id: int, Name: string, Code: string, PromoType: string, DiscountValue: decimal?, MinSpend: decimal?, StartDate: DateTime, EndDate: DateTime, ApplicableScope: string, UsageLimit: int?, IsActive: bool]`

---

### 4.4 หน้ารายงานสรุปยอดประจำเดือน (Monthly Report)
- **View File**: `Views/Owner/MonthlyReport.cshtml`
- **Controller**: `OwnerController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> MonthlyReport(string? month, string? quarter)
  ```
- **Action (POST / FileResult - Export Report)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> ExportReport(string exportScope, string exportFormat, string period)
  ```
- **พารามิเตอร์**:
  - `[exportScope: string (month/quarter), exportFormat: string (excel/pdf), period: string]`

---

## 5. กลุ่มผู้ดูแลระบบ (IT Support Portal)

### 5.1 หน้าภาพรวมสถานะระบบ (Dashboard)
- **View File**: `Views/ITSupport/Dashboard.cshtml`
- **Controller**: `ITSupportController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Dashboard()
  ```
- **ViewModel Definition**:
  ```csharp
  public class SystemOverviewViewModel
  {
      public string ServerStatus { get; set; } = "Online";
      public int LatencyMs { get; set; } = 118;
      public double UptimePercentage { get; set; } = 99.98;
      public int ActiveDbConnections { get; set; } = 24;
      public double StorageUsedGb { get; set; } = 4.2;
      public double StorageTotalGb { get; set; } = 50.0;
      public List<SystemLogDto> RecentCriticalLogs { get; set; } = new();
  }
  ```

---

### 5.2 หน้าจัดการบัญชีผู้ใช้งาน (Users Management)
- **View File**: `Views/ITSupport/Users.cshtml`
- **Controller**: `ITSupportController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Users(string? searchQuery, string? role, string? status, int page = 1)
  ```
- **Action (POST - Create User)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> CreateUser(CreateUserDto dto)
  ```
- **Action (POST - Edit User)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> EditUser(EditUserDto dto)
  ```
- **Action (POST - Reset Password)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> HelpResetPassword(int userId)
  ```
- **Action (POST - Delete User)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> DeleteUser(int userId)
  ```
- **Action (POST - Export CSV)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> ExportUsersCsv(string timeRange)
  ```
- **ViewModel Definition**:
  ```csharp
  public class CreateUserDto
  {
      [Required]
      public string Role { get; set; } = "customer"; // customer, staff, owner, itsupport, accountant

      public string? FullName { get; set; } // สำหรับ role ที่ไม่ใช่ customer

      [Required]
      [EmailAddress]
      public string Email { get; set; } = string.Empty;

      [Required]
      [Phone]
      public string Phone { get; set; } = string.Empty;

      [Required]
      [MinLength(8)]
      public string Password { get; set; } = string.Empty;

      public IFormFile? AvatarFile { get; set; }
  }
  ```
- **พารามิเตอร์ที่ส่งจากฟอร์ม**:
  - Create: `[Role: string, FullName: string?, Email: string, Phone: string, Password: string, AvatarFile: IFormFile?]`
  - Edit: `[Id: int, FullName: string?, Email: string, Phone: string, Role: string]`
  - Delete: `[Id: int]`

---

### 5.3 หน้าบันทึกกิจกรรมและข้อผิดพลาด (System Logs)
- **View File**: `Views/ITSupport/Logs.cshtml`
- **Controller**: `ITSupportController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Logs(string? level, string? timeRange, string? search, int page = 1)
  ```
- **Action (POST - Export CSV)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> ExportLogsCsv(string timeRange, List<string> levels)
  ```
- **พารามิเตอร์**:
  - `[level: string (all, Error, Warning, Info), timeRange: string, search: string, page: int]`

---

## 6. กลุ่มคนทำบัญชีและการเงิน (Accountant Portal)

### 6.1 หน้าแดชบอร์ดการเงิน (Financial Dashboard)
- **View File**: `Views/Accountant/Index.cshtml`
- **Controller**: `AccountantController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Index(string? month)
  ```
- **ViewModel Definition**:
  ```csharp
  public class FinancialDashboardViewModel
  {
      public decimal TotalMonthlyRevenue { get; set; }
      public decimal StorefrontRevenue { get; set; }
      public decimal PreorderDepositPending { get; set; }
      public decimal RecordedExpenses { get; set; }
      public decimal EstimatedGrossMargin => TotalMonthlyRevenue - RecordedExpenses;
      public double PromptPaySharePercent { get; set; } = 65.0;
      public double CashSharePercent { get; set; } = 35.0;
      public List<RecentTransactionDto> RecentSales { get; set; } = new();
  }
  ```

---

### 6.2 หน้าบันทึกรายรับ-รายจ่าย (Ledger)
- **View File**: `Views/Accountant/Ledger.cshtml`
- **Controller**: `AccountantController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Ledger(string? month, string? type, string? search)
  ```
- **Action (POST - บันทึกรายการใหม่ / แก้ไข)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> SaveLedgerEntry(LedgerEntryInputDto model)
  ```
- **Action (POST - ลบรายการ)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> DeleteLedgerEntry(int id)
  ```
- **ViewModel Definition**:
  ```csharp
  public class LedgerEntryInputDto
  {
      public int? Id { get; set; }

      [Required]
      public string Type { get; set; } = "expense"; // expense หรือ income

      [Required(ErrorMessage = "กรุณาเลือกหมวดหมู่")]
      public string Category { get; set; } = string.Empty;

      [Required(ErrorMessage = "กรุณากรอกรายละเอียด")]
      public string Description { get; set; } = string.Empty;

      [Required(ErrorMessage = "กรุณาระบุจำนวนเงิน")]
      [Range(0.01, 10000000, ErrorMessage = "จำนวนเงินต้องมากกว่า 0")]
      public decimal Amount { get; set; }

      [Required]
      public DateTime Date { get; set; } = DateTime.Today;
  }
  ```
- **พารามิเตอร์ที่ส่งจากฟอร์ม**:
  - `[Id: int?, Type: string, Category: string, Description: string, Amount: decimal, Date: DateTime]`

---

### 6.3 หน้าประวัติใบเสร็จและธุรกรรม (Receipts History & Preview)
- **View File**: `Views/Accountant/Receipts.cshtml`
- **Controller**: `AccountantController`
- **Action (GET)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> Receipts(string? search, DateTime? fromDate, DateTime? toDate, int page = 1)
  ```
- **Action (POST - Export Excel)**:
  ```csharp
  [HttpPost]
  public async Task<IActionResult> ExportReceiptsExcel(string timeRange)
  ```
- **Action (GET - ข้อมูลใบเสร็จสำหรับพิมพ์ / พรีวิว)**:
  ```csharp
  [HttpGet]
  public async Task<IActionResult> GetReceiptDetail(string receiptNo)
  ```
- **ViewModel Definition**:
  ```csharp
  public class ReceiptDetailDto
  {
      public string ReceiptNo { get; set; } = string.Empty;
      public DateTime DateTime { get; set; }
      public string CustomerName { get; set; } = string.Empty;
      public string CustomerPhone { get; set; } = string.Empty;
      public string StaffName { get; set; } = string.Empty;
      public string PaymentMethod { get; set; } = string.Empty;
      public List<ReceiptItemLineDto> Items { get; set; } = new();
      public decimal Subtotal { get; set; }
      public decimal VatAmount { get; set; }
      public decimal TotalAmount { get; set; }
  }
  ```

---

## สรุปหลักการความปลอดภัยและการจัดการข้อผิดพลาด (Security & Exception Rules)
1. **การป้องกัน CSRF (Cross-Site Request Forgery)**: ทุกการส่งฟอร์มแบบ `POST` แนะนำให้ประดับแอตทริบิวต์ `[ValidateAntiForgeryToken]` ที่ระดับ Action method ฝั่ง Controller
2. **การผูกสิทธิ์ตามบทบาท (Role-based Authorization)**:
   - สมาชิกทั่วไป: `[Authorize(Roles = "Customer")]`
   - พนักงาน: `[Authorize(Roles = "Staff,Owner")]`
   - เจ้าของร้าน: `[Authorize(Roles = "Owner")]`
   - ไอทีซัพพอร์ต: `[Authorize(Roles = "ITSupport")]`
   - คนทำบัญชี: `[Authorize(Roles = "Accountant,Owner")]`
3. **การจัดการ Session / Token หมดอายุ**: หากผู้ใช้ไม่มีสิทธิ์หรือ Session หลุด ระบบจะนำทางกลับไปยัง `Account/Login` พร้อมพารามิเตอร์ `returnUrl`
