# พจนานุกรมข้อมูล (Data Dictionary)
## ระบบร้านฟิกเกอร์และโมเดล FriendHouse (CSI402)

เอกสารฉบับนี้เป็น **พจนานุกรมข้อมูล (Data Dictionary)** สำหรับฐานข้อมูลเชิงสัมพันธ์ของระบบ **FriendHouse** จัดทำขึ้นเพื่อระบุโครงสร้าง คุณสมบัติของเขตข้อมูล (Fields/Attributes) ชนิดข้อมูล ข้อจำกัด (Constraints) ค่าเริ่มต้น ตลอดจนตัวอย่างข้อมูลจริง เพื่อให้นักพัฒนาระบบ (Database Administrator, Backend Developer, QA) ใช้อ้างอิงในการออกแบบฐานข้อมูล การเขียนคำสั่ง SQL / ORM (Entity Framework Core) และการเชื่อมต่อ API อย่างถูกต้อง แม่นยำ 100%

---

## สารบัญตารางข้อมูล (Table Index)

| ลำดับ | ชื่อตาราง (Physical Name) | ชื่อภาษาไทย (Logical Name) | กลุ่มงาน (Module) | จำนวนคอลัมน์ | คีย์หลัก (PK) |
| :---: | :--- | :--- | :--- | :---: | :--- |
| 1 | `Roles` | ข้อมูลบทบาทผู้ใช้งาน | Identity & Access | 5 | `RoleId` |
| 2 | `Users` | บัญชีผู้ใช้งานระบบ | Identity & Access | 11 | `UserId` |
| 3 | `UserRoles` | สิทธิ์บทบาทของผู้ใช้งาน | Identity & Access | 3 | `UserId, RoleId` |
| 4 | `MemberPoints` | ยอดแต้มสะสมสมาชิก | Loyalty & Points | 5 | `PointId` |
| 5 | `PointTransactions` | ประวัติการได้/ใช้แต้มสะสม | Loyalty & Points | 8 | `TransactionId` |
| 6 | `Categories` | หมวดหมู่สินค้า | Catalog & Figures | 4 | `CategoryId` |
| 7 | `Series` | ซีรีส์ / อนิเมะ / เกม | Catalog & Figures | 3 | `SeriesId` |
| 8 | `Brands` | แบรนด์ผู้ผลิต / ลิขสิทธิ์ | Catalog & Figures | 3 | `BrandId` |
| 9 | `Products` | ข้อมูลฟิกเกอร์และโมเดล | Catalog & Figures | 20 | `ProductId` |
| 10 | `ProductImages` | รูปภาพประกอบสินค้า | Catalog & Figures | 5 | `ImageId` |
| 11 | `Inventories` | คลังสต็อกสินค้าคงเหลือ | Warehouse & Stock | 9 | `InventoryId` |
| 12 | `StockMovements` | ประวัติการเคลื่อนไหวสต็อก | Warehouse & Stock | 9 | `MovementId` |
| 13 | `Preorders` | คำสั่งจองพรีออเดอร์ | Orders & Preorder | 13 | `PreorderId` |
| 14 | `PreorderItems` | รายการสินค้าในใบจอง | Orders & Preorder | 7 | `ItemId` |
| 15 | `PreorderStatusLogs` | ประวัติการเปลี่ยนสถานะการจอง | Orders & Preorder | 7 | `LogId` |
| 16 | `Promotions` | แคมเปญโปรโมชั่น | Marketing & Promos | 13 | `PromotionId` |
| 17 | `PromotionProducts` | สินค้าที่เข้าร่วมโปรโมชั่น | Marketing & Promos | 2 | `PromotionId, ProductId` |
| 18 | `PromotionUsages` | ประวัติการใช้สิทธิ์โปรโมชั่น | Marketing & Promos | 6 | `UsageId` |
| 19 | `ExpenseCategories` | หมวดหมู่รายรับ-รายจ่าย | Accounting & Ledger | 3 | `CategoryId` |
| 20 | `FinancialLedger` | บันทึกบัญชีรายรับ-รายจ่าย | Accounting & Ledger | 9 | `EntryId` |
| 21 | `Receipts` | ใบเสร็จรับเงินอย่างเป็นทางการ | Billing & Receipts | 15 | `ReceiptId` |
| 22 | `ReceiptItems` | รายการสินค้าในใบเสร็จ | Billing & Receipts | 7 | `ReceiptItemId` |
| 23 | `PaymentTransactions` | ธุรกรรมการชำระเงิน | Billing & Receipts | 7 | `PaymentId` |
| 24 | `SystemLogs` | บันทึกเหตุการณ์และข้อผิดพลาด | System & Audit | 9 | `LogId` |

---

## 1. หมวดบัญชีผู้ใช้และสิทธิ์ (Identity & Access Management)

### 1.1 ตาราง: `Roles`
- **คำอธิบาย**: จัดเก็บข้อมูลบทบาทหน้าที่ของบุคคลในระบบ เพื่อนโยบาย Role-Based Access Control (RBAC)
- **Primary Key**: `RoleId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `RoleId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสระบุบทบาท | `1` |
| `RoleCode` | VARCHAR(50) | - | No | - | UNIQUE | รหัสบทบาทภาษาอังกฤษอ้างอิงในโค้ด | `'Customer'` |
| `RoleName` | NVARCHAR(100) | - | No | - | - | ชื่อบทบาทภาษาไทยสำหรับแสดงผล | `'ลูกค้าทั่วไป'` |
| `Description` | NVARCHAR(255) | - | Yes | NULL | - | คำอธิบายขอบเขตการทำงานของบทบาท | `'สั่งพรีออเดอร์และสะสมแต้ม'` |
| `CreatedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่สร้างระเบียน | `'2026-01-01 00:00:00'` |

---

### 1.2 ตาราง: `Users`
- **คำอธิบาย**: บัญชีผู้ใช้งานระบบทั้งสมาชิกภายนอกและบุคลากรภายในร้าน (Staff, Owner, IT, Accountant)
- **Primary Key**: `UserId`
- **Index**: `Email` (Unique), `Phone` (Non-clustered)

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `UserId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสผู้ใช้งานภายในระบบ | `1` |
| `UserCode` | VARCHAR(30) | - | No | - | UNIQUE | รหัสอ้างอิงผู้ใช้ในเอกสารและ UI | `'USR-CUST-0081'` |
| `Email` | VARCHAR(150) | - | No | - | UNIQUE | อีเมลสำหรับใช้เข้าสู่ระบบ | `'sarah.fig@email.com'` |
| `PasswordHash` | VARCHAR(255) | - | No | - | - | รหัสผ่านที่ผ่านการเข้ารหัสแฮช (BCrypt) | `'$2a$12$e8...'` |
| `Phone` | VARCHAR(20) | - | No | - | INDEX | เบอร์โทรศัพท์ (ใช้ค้นหาที่เคาน์เตอร์) | `'081-234-5678'` |
| `FullName` | NVARCHAR(150) | - | Yes | NULL | - | ชื่อ-นามสกุล (สำหรับ Internal Roles) | `'ณัฐชา ใจดี'` |
| `AvatarUrl` | VARCHAR(255) | - | Yes | NULL | - | ที่อยู่ไฟล์รูปประจำตัวในโฟลเดอร์มีเดีย | `'/media/images/avatar1.png'` |
| `IsActive` | BIT | - | No | 1 | 0 = ระงับ, 1 = ปกติ | สถานะความพร้อมใช้งานของบัญชี | `1` |
| `LastLoginAt` | DATETIME2 | - | Yes | NULL | - | วันเวลาที่เข้าใช้งานระบบล่าสุด | `'2026-09-28 14:10:00'` |
| `CreatedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่ลงทะเบียนเข้าสู่ระบบ | `'2026-08-12 10:30:00'` |
| `UpdatedAt` | DATETIME2 | - | Yes | NULL | - | วันเวลาที่แก้ไขข้อมูลล่าสุด | `'2026-09-01 09:15:00'` |

---

### 1.3 ตาราง: `UserRoles`
- **คำอธิบาย**: กำหนดความสัมพันธ์ระหว่างบัญชีผู้ใช้งานกับบทบาท (Many-to-Many Mapping)
- **Primary Key**: `(UserId, RoleId)` (Composite Primary Key)

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `UserId` | INT | **PK, FK** | No | - | FK -> `Users(UserId)` | รหัสผู้ใช้งาน (CASCADE DELETE) | `2` |
| `RoleId` | INT | **PK, FK** | No | - | FK -> `Roles(RoleId)` | รหัสบทบาทที่ได้รับมอบหมาย | `2` |
| `AssignedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่กำหนดสิทธิ์ | `'2026-01-01 09:00:00'` |

---

### 1.4 ตาราง: `MemberPoints`
- **คำอธิบาย**: ข้อมูลยอดคงเหลือแต้มสะสม Loyalty Points ของสมาชิกลูกค้าแต่ละราย
- **Primary Key**: `PointId`
- **Foreign Key**: `UserId` -> `Users(UserId)`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `PointId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสระเบียนแต้มสะสม | `1` |
| `UserId` | INT | **FK** | No | - | UNIQUE | รหัสสมาชิกลูกค้าเจ้าของแต้ม | `1` |
| `CurrentPoints` | INT | - | No | 0 | CHECK (>= 0) | ยอดแต้มสะสมคงเหลือที่ใช้แลกได้ | `2450` |
| `LifetimePoints`| INT | - | No | 0 | CHECK (>= 0) | แต้มสะสมรวมตลอดชีพทั้งหมดที่เคยได้รับ | `5200` |
| `UpdatedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่อัปเดตยอดแต้มล่าสุด | `'2026-09-09 14:15:00'` |

---

### 1.5 ตาราง: `PointTransactions`
- **คำอธิบาย**: บันทึกประวัติการได้แต้มและการใช้แต้มสะสม เพื่อการตรวจสอบย้อนหลัง (Ledger of Points)
- **Primary Key**: `TransactionId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `TransactionId`| BIGINT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสธุรกรรมแต้มสะสม | `1001` |
| `UserId` | INT | **FK** | No | - | FK -> `Users(UserId)` | รหัสสมาชิกผู้ทำรายการ | `1` |
| `PointsDelta` | INT | - | No | - | ค่าบวก (+) หรือ ลบ (-) | จำนวนแต้มที่เพิ่มขึ้นหรือลดลง | `+42` หรือ `-500` |
| `BalanceAfter` | INT | - | No | - | CHECK (>= 0) | ยอดแต้มคงเหลือหลังการทำรายการ | `2450` |
| `SourceType` | VARCHAR(50) | - | No | - | รหัสประเภทแหล่งที่มา | ประเภทรายการ (`Receipt`, `Redeem`) | `'PurchaseReceipt'` |
| `ReferenceId` | VARCHAR(50) | - | Yes | NULL | เลขที่ใบเสร็จหรือรหัสของรางวัล | รหัสอ้างอิงที่ทำให้เกิดธุรกรรม | `'RC-25690909-0024'` |
| `Description` | NVARCHAR(255) | - | No | - | - | คำอธิบายเหตุการณ์ | `'รับแต้มจากยอดซื้อ ฿4,200'` |
| `CreatedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่บันทึกรายการ | `'2026-09-09 14:15:00'` |

---

## 2. หมวดสินค้าและแคตตาล็อก (Catalog, Figures & Media)

### 2.1 ตาราง: `Categories`
- **คำอธิบาย**: หมวดหมู่สินค้าฟิกเกอร์และของสะสม
- **Primary Key**: `CategoryId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `CategoryId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสหมวดหมู่สินค้า | `1` |
| `CategoryName` | NVARCHAR(100) | - | No | - | - | ชื่อหมวดหมู่ภาษาไทย | `'Scale Figures'` |
| `Slug` | VARCHAR(100) | - | No | - | UNIQUE | URL Slug สำหรับค้นหาและจัดหมวด | `'scale-figures'` |
| `IsActive` | BIT | - | No | 1 | 1 = แสดง, 0 = ซ่อน | สถานะเปิดแสดงผล | `1` |

---

### 2.2 ตาราง: `Series`
- **คำอธิบาย**: ซีรีส์อนิเมะ มังงะ หรือเกมที่สินค้าสังกัด
- **Primary Key**: `SeriesId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `SeriesId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสซีรีส์ | `1` |
| `SeriesName` | NVARCHAR(150) | - | No | - | - | ชื่อซีรีส์ / เรื่อง | `'Naruto Shippuden'` |
| `Slug` | VARCHAR(150) | - | No | - | UNIQUE | URL Slug สำหรับตัวกรองซีรีส์ | `'naruto'` |

---

### 2.3 ตาราง: `Brands`
- **คำอธิบาย**: ค่ายผู้ผลิตหรือผู้ถือลิขสิทธิ์ฟิกเกอร์แท้
- **Primary Key**: `BrandId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `BrandId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสแบรนด์ผู้ผลิต | `1` |
| `BrandName` | NVARCHAR(100) | - | No | - | UNIQUE | ชื่อแบรนด์ / ค่ายผลิต | `'Good Smile Company'` |

---

### 2.4 ตาราง: `Products`
- **คำอธิบาย**: ข้อมูลรายละเอียด สเปก ราคา และสถานะพรีออเดอร์ของฟิกเกอร์ทุกรายการ
- **Primary Key**: `ProductId`
- **Index**: `Sku` (Unique), `Barcode` (Unique, Nullable), `CategoryId` (Non-clustered)

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `ProductId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสสินค้าประจำระบบ | `101` |
| `Sku` | VARCHAR(50) | - | No | - | UNIQUE, INDEX | รหัสสต็อก SKU ประจำชิ้น | `'SKU-FG-9901'` |
| `Barcode` | VARCHAR(50) | - | Yes | NULL | UNIQUE, INDEX | รหัสบาร์โค้ด EAN-13 หรือ QR Code | `'8851234567890'` |
| `Name` | NVARCHAR(200) | - | No | - | - | ชื่อสินค้าภาษาไทย / อังกฤษ | `'Hatsune Miku - Magical Mirai 2024'` |
| `CategoryId` | INT | **FK** | No | - | FK -> `Categories` | หมวดหมู่สินค้า | `1` |
| `SeriesId` | INT | **FK** | Yes | NULL | FK -> `Series` | ซีรีส์เรื่องที่สังกัด | `2` |
| `BrandId` | INT | **FK** | Yes | NULL | FK -> `Brands` | แบรนด์ผู้ผลิต | `1` |
| `Scale` | VARCHAR(30) | - | Yes | NULL | - | ขนาดสเกลของฟิกเกอร์ | `'1/7'` |
| `HeightCm` | DECIMAL(6,2) | - | Yes | NULL | CHECK (> 0) | ความสูงโดยประมาณ (เซนติเมตร) | `24.50` |
| `Material` | NVARCHAR(100) | - | Yes | NULL | - | วัสดุหลักที่ใช้ผลิต | `'PVC, ABS'` |
| `RetailPrice` | DECIMAL(10,2) | - | No | - | CHECK (>= 0) | ราคาจำหน่ายหน้าร้านปกติ (บาท) | `4200.00` |
| `PreorderPrice` | DECIMAL(10,2) | - | Yes | NULL | CHECK (>= 0) | ราคาพิเศษช่วงสั่งจองพรีออเดอร์ | `3990.00` |
| `DepositRequired`| DECIMAL(10,2)| - | No | 0.00 | CHECK (>= 0) | ยอดมัดจำขั้นต่ำที่ต้องชำระหน้าร้าน | `500.00` |
| `ExpectedArrival`| NVARCHAR(100)| - | Yes | NULL | - | กำหนดเข้าร้านโดยประมาณ | `'ไตรมาส 4 ปี 2569'` |
| `IsPreorder` | BIT | - | No | 0 | 1 = สินค้าพรีออเดอร์ | เป็นสินค้าเปิดพรีออเดอร์หรือไม่ | `1` |
| `PreorderDeadline`| DATETIME2 | - | Yes | NULL | - | วันหมดเขตเปิดรับสั่งจอง | `'2026-10-31 23:59:59'` |
| `Model3DPath` | VARCHAR(255) | - | Yes | NULL | - | เส้นทางไฟล์ 3D (`wwwroot/media/...`) | `'/media/models/naruto_kyubi_sage.glb'` |
| `IsFeaturedHero`| BIT | - | No | 0 | 1 = เด่นบน Hero หน้าร้าน | แสดง 3D Model บนหน้าแรกหรือไม่ | `1` |
| `IsActive` | BIT | - | No | 1 | 1 = วางจำหน่ายปกติ | สถานะเปิดใช้งานสินค้า | `1` |
| `CreatedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่เพิ่มสินค้าเข้าสู่ระบบ | `'2026-08-01 10:00:00'` |

---

### 2.5 ตาราง: `ProductImages`
- **คำอธิบาย**: ไฟล์รูปภาพประกอบฟิกเกอร์แต่ละมุมมองสำหรับแสดงบน Catalog และหน้ารายละเอียด
- **Primary Key**: `ImageId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `ImageId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสระเบียนรูปภาพ | `1` |
| `ProductId` | INT | **FK** | No | - | FK -> `Products(ProductId)` | รหัสสินค้า (CASCADE DELETE) | `101` |
| `ImagePath` | VARCHAR(255) | - | No | - | โฟลเดอร์ `media/images` | เส้นทาง URL สื่อรูปภาพ | `'/media/images/hatsune_miku_magical.png'` |
| `IsPrimary` | BIT | - | No | 0 | 1 = รูปปกหลัก | รูปภาพหน้าปกหลักของสินค้า | `1` |
| `DisplayOrder` | INT | - | No | 0 | ลำดับการจัดวาง | ลำดับการแสดงภาพในแกลเลอรี | `0` |

---

## 3. หมวดคลังสินค้าและสต็อก (Warehouse & Inventory Tracking)

### 3.1 ตาราง: `Inventories`
- **คำอธิบาย**: จัดเก็บสต็อกสินค้าคงเหลือจริง ตำแหน่งจัดเก็บ และระดับการแจ้งเตือนความปลอดภัย
- **Primary Key**: `InventoryId`
- **Unique**: `ProductId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `InventoryId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสสต็อกสินค้า | `1` |
| `ProductId` | INT | **FK** | No | - | UNIQUE, FK -> `Products` | รหัสสินค้าที่นับสต็อก | `101` |
| `QuantityOnHand`| INT | - | No | 0 | CHECK (>= 0) | จำนวนสินค้าที่มีอยู่จริงในคลัง | `12` |
| `ReservedQuantity`| INT | - | No | 0 | CHECK (>= 0) | จำนวนที่ถูกลูกค้าจองพรีออเดอร์รอรับ | `4` |
| `AvailableQuantity`| INT | - | No | Computed | AS (QOH - Reserved) | จำนวนคงเหลือที่ขายได้ทันที | `8` |
| `SafetyStockLevel`| INT | - | No | 3 | เกณฑ์เตือนสินค้าใกล้หมด | ระดับแจ้งเตือนเมื่อสต็อกใกล้หมด | `3` |
| `StorageLocation`| NVARCHAR(100)| - | Yes | NULL | ตำแหน่งตู้หรือชั้นวาง | ตำแหน่งจัดเก็บในร้าน | `'ตู้โชว์กระจก G-01'` |
| `LastRestockedAt`| DATETIME2 | - | Yes | NULL | - | วันเวลาที่สแกนรับเข้าล่าสุด | `'2026-09-04 14:28:00'` |
| `UpdatedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่ข้อมูลสต็อกอัปเดตล่าสุด | `'2026-09-09 14:15:00'` |

---

### 3.2 ตาราง: `StockMovements`
- **คำอธิบาย**: บันทึก Log การเปลี่ยนแปลงของสต็อกทุกชิ้นอย่างละเอียด (Audit Trail ของสินค้า)
- **Primary Key**: `MovementId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `MovementId` | BIGINT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสรายการเคลื่อนไหวสต็อก | `5001` |
| `ProductId` | INT | **FK** | No | - | FK -> `Products(ProductId)` | รหัสสินค้าที่เคลื่อนไหว | `101` |
| `MovementType` | VARCHAR(50) | - | No | - | ดู Lookup Code 6.3 | ประเภทรายการเคลื่อนไหว | `'ScanReceive'` |
| `QuantityDelta` | INT | - | No | - | ไม่เท่ากับ 0 | ยอดที่เพิ่มขึ้นหรือลดลง | `+10` หรือ `-1` |
| `BalanceAfter` | INT | - | No | - | CHECK (>= 0) | จำนวนคงเหลือหลังการเคลื่อนไหว | `12` |
| `ReferenceCode` | VARCHAR(50) | - | Yes | NULL | เลขที่ใบจองหรือเลขบาร์โค้ด | เอกสารหรือรหัสอ้างอิง | `'RC-25690909-0024'` |
| `RecordedByUserId`| INT | **FK** | No | - | FK -> `Users(UserId)` | พนักงานผู้บันทึกรายการสต็อก | `2` |
| `Remark` | NVARCHAR(255) | - | Yes | NULL | - | หมายเหตุเพิ่มเติม | `'รับเข้าผ่านเครื่องสแกนกล้อง'` |
| `CreatedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่เกิดการเคลื่อนไหว | `'2026-09-09 14:28:10'` |

---

## 4. หมวดการสั่งจองพรีออเดอร์ (Orders & Preorder Management)

### 4.1 ตาราง: `Preorders`
- **คำอธิบาย**: ข้อมูลคำสั่งจองพรีออเดอร์ของลูกค้า ยอดมัดจำ ยอดคงเหลือ และสถานะการนัดรับสินค้า
- **Primary Key**: `PreorderId`
- **Index**: `ReservationNumber` (Unique), `ContactPhone` (Non-clustered)

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `PreorderId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสคำสั่งจองภายในระบบ | `482` |
| `ReservationNumber`| VARCHAR(30)| - | No | - | UNIQUE, INDEX | เลขที่อ้างอิงใบจองสำหรับลูกค้า | `'PO-000482'` |
| `UserId` | INT | **FK** | No | - | FK -> `Users(UserId)` | รหัสลูกค้าผู้สั่งจอง | `1` |
| `ContactPhone` | VARCHAR(20) | - | No | - | INDEX | เบอร์โทรศัพท์สำหรับรับ SMS นัดรับ | `'081-234-5000'` |
| `EstimatedPickupDate`| DATE | - | No | - | - | วันที่ลูกค้านัดหมายเข้ามารับสินค้า | `'2026-11-15'` |
| `CurrentStatus` | VARCHAR(50) | - | No | `'PendingConfirm'` | ดู Lookup Code 6.2 | สถานะปัจจุบันของคำสั่งจอง | `'ReadyForPickup'` |
| `TotalDepositRequired`| DECIMAL(10,2)| -| No | 0.00 | CHECK (>= 0) | ยอดเงินมัดจำขั้นต่ำที่ต้องชำระ | `500.00` |
| `TotalDepositPaid` | DECIMAL(10,2)| -| No | 0.00 | CHECK (>= 0) | ยอดมัดจำที่ลูกค้ารับชำระแล้ว | `500.00` |
| `TotalRemainingBalance`| DECIMAL(10,2)| -| No | 0.00 | CHECK (>= 0) | ยอดเงินคงเหลือที่ต้องจ่ายเมื่อรับของ | `3700.00` |
| `TotalFinalPrice` | DECIMAL(10,2)| -| No | - | CHECK (>= 0) | ราคารวมสุทธิของคำสั่งจอง | `4200.00` |
| `PickupBranch` | NVARCHAR(100)| - | No | `'FriendHouse HQ'` | - | สาขาของร้านที่ลูกค้านัดรับของ | `'FriendHouse HQ'` |
| `CreatedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่ยืนยันการจอง | `'2026-08-28 16:45:00'` |
| `CompletedAt` | DATETIME2 | - | Yes | NULL | - | วันเวลาที่ลูกค้ารับของและปิดบิล | `'2026-09-09 14:15:00'` |

---

### 4.2 ตาราง: `PreorderItems`
- **คำอธิบาย**: รายการฟิกเกอร์ที่สั่งจองในใบจองพรีออเดอร์แต่ละใบ
- **Primary Key**: `ItemId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `ItemId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสรายการสินค้าในใบจอง | `1` |
| `PreorderId` | INT | **FK** | No | - | FK -> `Preorders` (CASCADE) | รหัสใบจอง | `482` |
| `ProductId` | INT | **FK** | No | - | FK -> `Products(ProductId)` | รหัสฟิกเกอร์ที่จอง | `101` |
| `UnitPrice` | DECIMAL(10,2) | - | No | - | CHECK (>= 0) | ราคาต่อหน่วย ณ วันสั่งจอง | `4200.00` |
| `DepositPerUnit` | DECIMAL(10,2) | - | No | - | CHECK (>= 0) | อัตรามัดจำต่อชิ้น | `500.00` |
| `Quantity` | INT | - | No | 1 | CHECK (> 0) | จำนวนชิ้นที่จอง | `1` |
| `LineTotal` | DECIMAL(10,2) | - | No | Computed | AS (UnitPrice * Quantity) | ราคารวมของรายการ | `4200.00` |

---

### 4.3 ตาราง: `PreorderStatusLogs`
- **คำอธิบาย**: ประวัติการปรับเปลี่ยนสถานะของคำสั่งพรีออเดอร์ทั้ง 4 ขั้นตอน
- **Primary Key**: `LogId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `LogId` | BIGINT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสประวัติสถานะคำสั่งจอง | `301` |
| `PreorderId` | INT | **FK** | No | - | FK -> `Preorders(PreorderId)`| รหัสใบจอง | `482` |
| `OldStatus` | VARCHAR(50) | - | Yes | NULL | - | สถานะเดิมก่อนหน้า | `'WaitingStock'` |
| `NewStatus` | VARCHAR(50) | - | No | - | - | สถานะใหม่ที่อัปเดต | `'ReadyForPickup'` |
| `ChangedByUserId`| INT | **FK** | Yes | NULL | FK -> `Users(UserId)` | ผู้แก้ไขสถานะ (พนักงาน/ระบบ) | `2` |
| `Notes` | NVARCHAR(255) | - | Yes | NULL | - | หมายเหตุประกอบการเปลี่ยนสถานะ | `'สินค้ามาถึงหน้าร้านแล้ว ส่ง SMS'` |
| `CreatedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่สถานะเปลี่ยน | `'2026-09-08 11:20:00'` |

---

## 5. หมวดโปรโมชั่นและส่วนลด (Marketing & Promotions)

### 5.1 ตาราง: `Promotions`
- **คำอธิบาย**: แคมเปญส่วนลด 5 รูปแบบของร้าน FriendHouse ที่เจ้าของร้านจัดการ
- **Primary Key**: `PromotionId`
- **Index**: `VoucherCode` (Unique)

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `PromotionId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสโปรโมชั่น | `1` |
| `Name` | NVARCHAR(150) | - | No | - | - | ชื่อโปรโมชั่น | `'ล้างสต๊อกส่งท้ายปี ลด 20–50%'` |
| `VoucherCode` | VARCHAR(50) | - | No | - | UNIQUE, INDEX | รหัสคูปอง / โค้ดส่วนลด | `'CLEARANCE50'` |
| `PromoType` | VARCHAR(50) | - | No | - | ดู Lookup Code 6.4 | ประเภทโปรโมชั่น | `'percent'` |
| `DiscountValue` | DECIMAL(10,2) | - | Yes | NULL | CHECK (>= 0) | มูลค่าส่วนลด (% หรือ บาท) | `50.00` |
| `MinimumSpend` | DECIMAL(10,2) | - | Yes | 0.00 | CHECK (>= 0) | ยอดสั่งซื้อขั้นต่ำที่ได้รับสิทธิ์ | `500.00` |
| `StartDate` | DATE | - | No | - | - | วันเริ่มต้นโปรโมชั่น | `'2026-09-01'` |
| `EndDate` | DATE | - | No | - | - | วันสิ้นสุดโปรโมชั่น | `'2026-12-31'` |
| `ApplicableScope`| VARCHAR(50) | - | No | `'all'` | all, clearance, figures | ขอบเขตสินค้าที่เข้าร่วม | `'clearance'` |
| `UsageLimit` | INT | - | Yes | NULL | CHECK (> 0) | จำนวนครั้งการใช้งานสูงสุด | `100` |
| `CurrentUsageCount`| INT | - | No | 0 | CHECK (>= 0) | ยอดจำนวนครั้งที่มีการใช้ไปแล้ว | `42` |
| `IsActive` | BIT | - | No | 1 | 1 = เปิดใช้งาน, 0 = ปิด | สวิตช์เปิด/ปิดการใช้งานทันที | `1` |
| `CreatedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่สร้างโปรโมชั่น | `'2026-08-25 15:00:00'` |

---

### 5.2 ตาราง: `PromotionProducts`
- **คำอธิบาย**: สินค้าเฉพาะรายการที่เลือกเข้าร่วมโปรโมชั่น (Mapping Table)
- **Primary Key**: `(PromotionId, ProductId)`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `PromotionId` | INT | **PK, FK** | No | - | FK -> `Promotions` (CASCADE) | รหัสโปรโมชั่น | `1` |
| `ProductId` | INT | **PK, FK** | No | - | FK -> `Products` (CASCADE) | รหัสสินค้าที่ได้รับส่วนลด | `101` |

---

### 5.3 ตาราง: `PromotionUsages`
- **คำอธิบาย**: ประวัติการนำโค้ดโปรโมชั่นไปใช้ในการเปิดบิลใบเสร็จ
- **Primary Key**: `UsageId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `UsageId` | BIGINT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสประวัติการใช้สิทธิ์ | `1` |
| `PromotionId` | INT | **FK** | No | - | FK -> `Promotions` | รหัสโปรโมชั่นที่ใช้ | `1` |
| `UserId` | INT | **FK** | No | - | FK -> `Users(UserId)` | ลูกค้าผู้ใช้สิทธิ์ | `1` |
| `ReferenceReceiptNo`| VARCHAR(50)| - | Yes | NULL | เลขที่ใบเสร็จรับเงิน | ใบเสร็จที่ได้ส่วนลด | `'RC-25690909-0024'` |
| `DiscountApplied`| DECIMAL(10,2)| -| No | - | CHECK (>= 0) | มูลค่าส่วนลดที่หักออกจริง (บาท) | `200.00` |
| `UsedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่ใช้สิทธิ์ | `'2026-09-09 14:15:00'` |

---

## 6. หมวดการเงิน บัญชี และใบเสร็จ (Finance, Ledger & Billing)

### 6.1 ตาราง: `ExpenseCategories`
- **คำอธิบาย**: หมวดหมู่สำหรับลงบันทึกรายรับ-รายจ่ายที่กรอกเองในระบบบัญชี
- **Primary Key**: `CategoryId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `CategoryId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสหมวดหมู่บัญชี | `1` |
| `CategoryName` | NVARCHAR(100) | - | No | - | - | ชื่อหมวดหมู่ | `'ค่าเช่าพื้นที่ร้าน'` |
| `DefaultType` | VARCHAR(20) | - | No | `'expense'` | `'expense'` หรือ `'income'` | ประเภทรายการปกติ | `'expense'` |

---

### 6.2 ตาราง: `FinancialLedger`
- **คำอธิบาย**: สมุดรายวันบันทึกรายรับ-รายจ่ายของร้านที่ต้องกรอกเอง (เช่น ค่าเช่า ค่าน้ำไฟ อุปกรณ์แพ็คของ)
- **Primary Key**: `EntryId`
- **Index**: `EntryDate` (Non-clustered)

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `EntryId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสบันทึกบัญชี | `1` |
| `EntryType` | VARCHAR(20) | - | No | `'expense'` | `'expense'` หรือ `'income'` | ประเภทรายการ | `'expense'` |
| `CategoryId` | INT | **FK** | No | - | FK -> `ExpenseCategories` | หมวดหมู่บัญชี | `1` |
| `Description` | NVARCHAR(255) | - | No | - | - | รายละเอียดคำอธิบายรายการ | `'ค่าเช่าห้องพาณิชย์สาขา HQ ก.ย.'` |
| `Amount` | DECIMAL(12,2) | - | No | - | CHECK (> 0) | จำนวนเงิน (บาท) | `35000.00` |
| `EntryDate` | DATE | - | No | - | INDEX | วันที่เกิดรายการ | `'2026-09-01'` |
| `RecordedByUserId`| INT | **FK** | No | - | FK -> `Users(UserId)` | พนักงาน/นักบัญชีผู้บันทึก | `4` |
| `CreatedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่บันทึกลงระบบ | `'2026-09-01 10:00:00'` |
| `UpdatedAt` | DATETIME2 | - | Yes | NULL | - | วันเวลาที่แก้ไขข้อมูลล่าสุด | `'2026-09-01 10:05:00'` |

---

### 6.3 ตาราง: `Receipts`
- **คำอธิบาย**: ใบเสร็จรับเงินอย่างเป็นทางการ (Official Receipt) พิมพ์หน้าร้าน
- **Primary Key**: `ReceiptId`
- **Index**: `ReceiptNo` (Unique), `IssuedAt` (Non-clustered)

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `ReceiptId` | INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสใบเสร็จในระบบ | `24` |
| `ReceiptNo` | VARCHAR(40) | - | No | - | UNIQUE, INDEX | เลขที่ใบเสร็จที่พิมพ์บนหัวบิล | `'RC-25690909-0024'` |
| `PreorderId` | INT | **FK** | Yes | NULL | FK -> `Preorders` | เชื่อมคำสั่งพรีออเดอร์ (ถ้ามี) | `482` |
| `CustomerId` | INT | **FK** | Yes | NULL | FK -> `Users(UserId)` | รหัสสมาชิกลูกค้า (ถ้าล็อกอิน) | `1` |
| `CustomerName` | NVARCHAR(150) | - | No | - | - | ชื่อลูกค้าที่ปรากฏบนบิล | `'คุณกิตติศักดิ์ พูลสวัสดิ์'` |
| `CustomerPhone`| VARCHAR(20) | - | No | - | - | เบอร์โทรศัพท์ลูกค้า | `'081-234-5000'` |
| `CashierUserId`| INT | **FK** | No | - | FK -> `Users(UserId)` | พนักงานแคชเชียร์ผู้ออกใบเสร็จ | `2` |
| `SubtotalAmount`| DECIMAL(10,2)| - | No | - | CHECK (>= 0) | ยอดรวมสินค้าก่อนหักส่วนลด | `4200.00` |
| `DiscountAmount`| DECIMAL(10,2)| - | No | 0.00 | CHECK (>= 0) | ส่วนลดที่หักออก | `0.00` |
| `VatAmount` | DECIMAL(10,2) | - | No | - | CHECK (>= 0) | ภาษีมูลค่าเพิ่ม 7% (รวมในราคา) | `274.77` |
| `NetTotalAmount`| DECIMAL(10,2) | - | No | - | CHECK (>= 0) | ยอดสุทธิที่ลูกค้าชำระจริง | `4200.00` |
| `PaymentMethod` | VARCHAR(50) | - | No | - | ดู Lookup Code 6.5 | ช่องทางการชำระเงิน | `'PromptPayQR'` |
| `TransactionType`| VARCHAR(50)| - | No | - | ดู Lookup Code 6.6 | ประเภทธุรกรรม | `'PreorderFinalPickup'` |
| `BarcodeData` | VARCHAR(100) | - | No | - | - | รหัสบาร์โค้ดสำหรับสแกนตรวจสอบ | `'*RC-25690909-0024*'` |
| `IssuedAt` | DATETIME2 | - | No | SYSUTCDATETIME() | INDEX | วันเวลาที่ออกใบเสร็จ | `'2026-09-09 14:15:00'` |

---

### 6.4 ตาราง: `ReceiptItems`
- **คำอธิบาย**: รายการสินค้าที่พิมพ์ในตารางของใบเสร็จรับเงิน
- **Primary Key**: `ReceiptItemId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `ReceiptItemId`| INT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสรายการในใบเสร็จ | `1` |
| `ReceiptId` | INT | **FK** | No | - | FK -> `Receipts` (CASCADE) | รหัสใบเสร็จ | `24` |
| `ProductId` | INT | **FK** | Yes | NULL | FK -> `Products(ProductId)` | รหัสสินค้า | `101` |
| `ItemDescription`| NVARCHAR(200)| - | No | - | - | ชื่อรายการสินค้า | `'Hatsune Miku Magical Mirai 2024'` |
| `Quantity` | INT | - | No | 1 | CHECK (> 0) | จำนวนชิ้น | `1` |
| `UnitPrice` | DECIMAL(10,2) | - | No | - | CHECK (>= 0) | ราคาต่อหน่วย | `4200.00` |
| `LineTotal` | DECIMAL(10,2) | - | No | - | CHECK (>= 0) | ยอดรวมของบรรทัดนี้ | `4200.00` |

---

### 6.5 ตาราง: `PaymentTransactions`
- **คำอธิบาย**: ธุรกรรมการชำระเงิน บันทึกการตอบรับจากช่องทางธนาคาร/เงินสด
- **Primary Key**: `PaymentId`

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `PaymentId` | BIGINT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัสธุรกรรมการจ่ายเงิน | `801` |
| `ReceiptId` | INT | **FK** | No | - | FK -> `Receipts(ReceiptId)` | รหัสใบเสร็จรับเงิน | `24` |
| `ProviderTxnId` | VARCHAR(100) | - | Yes | NULL | รหัสอ้างอิงของธนาคาร | เลขอ้างอิงจากระบบ PromptPay | `'TXN-0909113012'` |
| `PaymentChannel`| VARCHAR(50) | - | No | - | ดู Lookup Code 6.5 | ช่องทางที่เงินเข้า | `'PromptPayQR'` |
| `PaidAmount` | DECIMAL(10,2) | - | No | - | CHECK (> 0) | จำนวนเงินที่รับชำระจริง | `4200.00` |
| `PaymentStatus` | VARCHAR(30) | - | No | `'Completed'` | `'Completed'`, `'Failed'` | สถานะการชำระ | `'Completed'` |
| `PaidAt` | DATETIME2 | - | No | SYSUTCDATETIME() | - | วันเวลาที่เงินเข้าระบบ | `'2026-09-09 14:15:00'` |

---

## 7. หมวดบันทึกเหตุการณ์ระบบ (System & Audit Logs)

### 7.1 ตาราง: `SystemLogs`
- **คำอธิบาย**: บันทึกเหตุการณ์ การเข้าสู่ระบบ ข้อผิดพลาด และการตรวจสอบความปลอดภัยของระบบ
- **Primary Key**: `LogId`
- **Index**: `LogLevel` (Non-clustered), `Timestamp` (Non-clustered)

| ชื่อฟิลด์ (Field) | ชนิดข้อมูล (Data Type) | PK/FK | Null? | ค่าเริ่มต้น (Default) | ข้อจำกัด (Constraints) | คำอธิบาย (Description) | ตัวอย่างข้อมูล (Example) |
| :--- | :--- | :---: | :---: | :--- | :--- | :--- | :--- |
| `LogId` | BIGINT | **PK** | No | IDENTITY(1,1) | Auto Increment | รหัส Log ภายในระบบ | `9021` |
| `LogCode` | VARCHAR(30) | - | No | - | UNIQUE | รหัสอ้างอิงแสดงผลบน UI | `'LOG-9021'` |
| `LogLevel` | VARCHAR(20) | - | No | - | ดู Lookup Code 6.7 | ระดับความสำคัญของเหตุการณ์ | `'Info'` |
| `Module` | VARCHAR(100) | - | No | - | INDEX | โมดูลหรือบริการที่บันทึก Log | `'StockSyncModule'` |
| `Message` | NVARCHAR(500) | - | No | - | - | ข้อความเหตุการณ์สรุปย่อ | `'ซิงก์สต็อกเข้าสำเร็จ 2 รายการ'` |
| `DetailPayload` | NVARCHAR(MAX) | - | Yes | NULL | JSON หรือ StackTrace | ข้อมูลเทคนิคและตัวแปรละเอียด | `'Barcodes: [SKU-BB-1042]'` |
| `IpAddress` | VARCHAR(50) | - | Yes | NULL | - | IP Address ของผู้ส่งคำขอ | `'192.168.1.45'` |
| `UserId` | INT | **FK** | Yes | NULL | FK -> `Users(UserId)` | บัญชีผู้ใช้งานที่เกี่ยวข้อง (ถ้ามี) | `2` |
| `Timestamp` | DATETIME2 | - | No | SYSUTCDATETIME() | INDEX | วันเวลาที่เกิดเหตุการณ์ | `'2026-09-09 14:28:10'` |

---

## 8. ตารางรหัสอ้างอิงและค่าคงที่ (Lookup Codes & Enumerations)

### 8.1 บทบาทผู้ใช้งาน (`Roles.RoleCode`)
| รหัส (Code) | ชื่อภาษาไทย | ขอบเขตสิทธิ์และการทำงานในระบบ FriendHouse |
| :--- | :--- | :--- |
| `Customer` | ลูกค้าทั่วไป | เข้าชมหน้าร้าน สั่งจองพรีออเดอร์ ดูสถานะใบจอง และแลกแต้มสะสมรางวัล |
| `Staff` | พนักงานหน้าร้าน | ตรวจสอบสต็อก สแกนรับสินค้าเข้า และประจำเคาน์เตอร์ส่งมอบพรีออเดอร์ |
| `Owner` | เจ้าของร้าน | จัดการโปรโมชั่นทั้ง 5 รูปแบบ ตรวจสอบรายงานยอดขายและรายรับประจำเดือน |
| `ITSupport` | ไอทีซัพพอร์ต | ตรวจสอบสถานะ Server/Database จัดการบัญชีผู้ใช้ รีเซ็ตรหัสผ่าน และดู Logs |
| `Accountant` | คนทำบัญชี | ตรวจสอบแดชบอร์ดการเงิน บันทึกรายรับ-รายจ่าย (Ledger) และพิมพ์ใบเสร็จ |

---

### 8.2 สถานะคำสั่งจองพรีออเดอร์ (`Preorders.CurrentStatus`)
| รหัส (Code) | ชื่อภาษาไทย | คำอธิบายและเงื่อนไขการเปลี่ยนสถานะ |
| :--- | :--- | :--- |
| `PendingConfirm` | แจ้งพรีออเดอร์ | ลูกค้าทำการส่งคำสั่งจองจากหน้าร้าน รอยืนยันการรับจอง |
| `WaitingStock` | รอสินค้าเข้า | ร้านค้าสั่งผลิต/นำเข้าฟิกเกอร์จากต่างประเทศ กำลังเดินทางมายังสาขา |
| `ReadyForPickup` | สินค้าถึงร้าน / รอรับ | ฟิกเกอร์เข้าร้านเรียบร้อยแล้ว ส่ง SMS แจ้งเตือนลูกค้าให้มารับที่ร้าน HQ |
| `Completed` | รับสินค้าแล้ว | ลูกค้ามารับสินค้าที่เคาน์เตอร์ ชำระยอดส่วนที่เหลือ และออกใบเสร็จสมบูรณ์ |
| `Cancelled` | ยกเลิกการจอง | ลูกค้าหรือทางร้านทำการยกเลิกคำสั่งจองพรีออเดอร์ |

---

### 8.3 ประเภทการเคลื่อนไหวสต็อก (`StockMovements.MovementType`)
| รหัส (Code) | คำอธิบายภาษาไทย | ทิศทางสต็อก (Delta) |
| :--- | :--- | :---: |
| `ScanReceive` | สแกนรับสินค้าเข้าสต็อกหน้าร้าน | **+ (บวก)** |
| `StoreSale` | ขายสินค้าพร้อมส่งหน้าร้านโดยตรง | **- (ลบ)** |
| `PreorderPickup` | ส่งมอบสินค้าพรีออเดอร์ให้ลูกค้าที่เคาน์เตอร์ | **- (ลบ)** |
| `Adjustment` | ปรับยอดสต็อกเนื่องจากตรวจนับหรือสินค้าชำรุด | **+/- (บวกหรือลบ)** |
| `Return` | ลูกค้าคืนสินค้าหรือส่งเคลมค่ายผลิต | **+ (บวก)** |

---

### 8.4 ประเภทโปรโมชั่น (`Promotions.PromoType`)
| รหัส (Code) | ชื่อโปรโมชั่นภาษาไทย | ตรรกะการคำนวณส่วนลด |
| :--- | :--- | :--- |
| `percent` | ส่วนลดเป็นเปอร์เซ็นต์ (%) | หักเปอร์เซ็นต์จากราคาสินค้า เช่น ลด 10%, 20%, 50% |
| `fixed` | ส่วนลดจำนวนเงินคงที่ (บาท) | หักยอดเงินตรงตัว เช่น ลด 100 บาท, ลด 200 บาท |
| `bogo` | ซื้อ 1 แถม 1 (Buy 1 Get 1) | เมื่อซื้อสินค้าหลัก 1 ชิ้น รับฟรีสินค้าของแถมที่กำหนด |
| `bundle` | เซ็ตคู่ดูโอ้ (Bundle Pack) | ลดราคาพิเศษเมื่อซื้อสินค้าคู่กันในซีรีส์เดียวกัน |
| `double_points` | แต้มสะสมคูณสอง (Double Points)| สมาชิกได้รับคะแนน Loyalty Points x2 เท่าของยอดใช้จ่าย |

---

### 8.5 ช่องทางการชำระเงิน (`PaymentMethod` / `PaymentChannel`)
| รหัส (Code) | ชื่อภาษาไทย | สัดส่วนในร้านโดยประมาณ |
| :--- | :--- | :---: |
| `PromptPayQR` | สแกน QR PromptPay ผ่าน Mobile Banking | **65%** |
| `Cash` | เงินสดที่เคาน์เตอร์หน้าร้าน | **35%** |
| `CreditCard` | บัตรเครดิต / บัตรเดบิต (เตรียมรองรับ) | - |

---

### 8.6 ประเภทธุรกรรมใบเสร็จ (`Receipts.TransactionType`)
| รหัส (Code) | ชื่อภาษาไทย | คำอธิบาย |
| :--- | :--- | :--- |
| `PreorderFinalPickup` | รับสินค้าพรีออเดอร์ (ชำระยอดที่เหลือ) | บิลส่งมอบฟิกเกอร์พรีออเดอร์ หักยอดมัดจำเดิมแล้ว |
| `PreorderDeposit` | ชำระเงินมัดจำพรีออเดอร์ | บิลรับเงินมัดจำล่วงหน้าเพื่อการันตีสิทธิ์สินค้า |
| `DirectStoreSale` | ซื้อสินค้าพร้อมส่งหน้าร้าน | บิลการซื้อขายสินค้าทั่วไปที่มีของพร้อมส่งในร้าน |

---

### 8.7 ระดับความสำคัญของระบบ Log (`SystemLogs.LogLevel`)
| รหัส (Code) | สี Badge บน UI | คำอธิบาย |
| :--- | :--- | :--- |
| `Info` | สีน้ำเงิน (`bg-primary/10 text-primary`) | การทำงานปกติของระบบ เช่น เข้าสู่ระบบสำเร็จ, สำรองข้อมูลสำเร็จ |
| `Warning` | สีส้ม/เหลือง (`bg-amber-100 text-amber-800`) | คำเตือนที่ควรเฝ้าระวัง เช่น สต็อกลดเหลือ 0, แจ้งเตือนใบจองใกล้หมดเขต |
| `Error` | สีแดง (`bg-rose-100 text-rose-700`) | ข้อผิดพลาดของระบบ เช่น ล็อกอินผิดเกิน 5 ครั้ง, ต่อฐานข้อมูลไม่ติด |
| `Critical` | สีแดงเข้ม | ระบบไม่สามารถให้บริการได้ จำเป็นต้องมีผู้ดูแลแก้ไขทันที |
