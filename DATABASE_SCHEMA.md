# ข้อกำหนดและโครงสร้างฐานข้อมูล (Database Schema Specification)
## ระบบร้านฟิกเกอร์และโมเดล FriendHouse (CSI402)
### โครงสร้างฐานข้อมูลเชิงสัมพันธ์ฉบับกระชับ 14 ตาราง (MySQL 8.0+ / MySQL Workbench)

เอกสารฉบับนี้จัดทำขึ้นเพื่อระบุโครงสร้างฐานข้อมูลเชิงสัมพันธ์ (Relational Database Schema) สำหรับระบบ **FriendHouse** โดยออกแบบให้สอดคล้องกับมาตรฐาน **MySQL 8.0+** และ **MySQL Workbench** มีการกำหนด Collation เป็น `utf8mb4_unicode_ci` รองรับภาษาไทย ภาษาญี่ปุ่น และภาษาอังกฤษอย่างสมบูรณ์ ประกอบด้วย **14 ตารางหลัก** ดังนี้:

---

## 1. ไดอะแกรมความสัมพันธ์ของข้อมูล (Entity-Relationship Diagram)

```mermaid
erDiagram
    %% Identity & Users
    USERS ||--o{ CART_ITEMS : maintains
    USERS ||--o{ PREORDERS : places
    USERS ||--o{ REWARD_REDEMPTIONS : redeems
    USERS ||--o{ FINANCIAL_LEDGER : records
    USERS ||--o{ RECEIPTS : cashier
    USERS ||--o{ RECEIPTS : customer
    USERS ||--o{ SYSTEM_LOGS : triggers
    
    %% Catalog & Figures & Inventory
    CATEGORIES ||--o{ PRODUCTS : categorizes
    PRODUCTS ||--o| INVENTORIES : tracked_in
    PRODUCTS ||--o{ CART_ITEMS : in_cart
    PRODUCTS ||--o{ PREORDER_ITEMS : booked
    PRODUCTS ||--o{ RECEIPT_ITEMS : billed
    
    %% Orders, Preorders & Receipts
    PREORDERS ||--o{ PREORDER_ITEMS : includes
    PREORDERS ||--o{ RECEIPTS : settled_in
    RECEIPTS ||--o{ RECEIPT_ITEMS : contains
    
    %% Loyalty Rewards
    REWARDS ||--o{ REWARD_REDEMPTIONS : rewards
```

---

## 2. รายละเอียดตารางข้อมูล (Table Specifications - MySQL Standard)

### 2.1 กลุ่มบัญชีผู้ใช้และสิทธิ์ (Identity, Access & Points)

#### ตาราง: `Users` (บัญชีผู้ใช้งานระบบ)
เก็บบัญชีผู้ใช้งานระบบทุกกลุ่ม กำหนดบทบาทชัดเจน 1 คนต่อ 1 หน้าที่ (Role) และเก็บแต้มสะสมของสมาชิกในตัว
| คอลัมน์ | ชนิดข้อมูล | Constraints | ค่าเริ่มต้น | คำอธิบาย |
| :--- | :--- | :--- | :--- | :--- |
| `UserId` | INT | PK, AUTO_INCREMENT | - | รหัสผู้ใช้งาน |
| `UserCode` | VARCHAR(30) | UNIQUE, NOT NULL | - | รหัสประจำตัว (เช่น `USR-CUST-0081`, `USR-STAF-0012`) |
| `Email` | VARCHAR(150) | UNIQUE, NOT NULL | - | อีเมลเข้าสู่ระบบ (ใช้เป็น Username) |
| `PasswordHash` | VARCHAR(255) | NOT NULL | - | รหัสผ่านที่เข้ารหัสแล้ว (BCrypt Work Factor 12) |
| `Phone` | VARCHAR(20) | NOT NULL, INDEX | - | เบอร์โทรศัพท์ (ใช้ค้นหาเคาน์เตอร์รับพรีออเดอร์) |
| `FullName` | VARCHAR(150) | NULL | - | ชื่อ-นามสกุล (สำหรับ Internal Roles / ลูกค้าเว้นว่างได้) |
| `AvatarUrl` | VARCHAR(255) | NULL | - | ที่อยู่ไฟล์รูปโปรไฟล์ (`/media/images/...`) |
| `Role` | VARCHAR(30) | NOT NULL | `'Customer'` | บทบาทผู้ใช้ (`Customer`, `Staff`, `Owner`, `ITSupport`, `Accountant`) |
| `Points` | INT | NOT NULL, CHECK (Points >= 0) | 0 | ยอดแต้มสะสมคงเหลือสำหรับแลกของรางวัล |
| `IsActive` | TINYINT(1) | NOT NULL | 1 | สถานะบัญชี (1: ปกติ, 0: ระงับการใช้งาน) |
| `CreatedAt` | DATETIME | NOT NULL | CURRENT_TIMESTAMP | วันที่ลงทะเบียน |

---

### 2.2 กลุ่มสินค้าและสต็อก (Catalog, Products & Inventory)

#### ตาราง: `Categories` (หมวดหมู่สินค้า)
| คอลัมน์ | ชนิดข้อมูล | Constraints | ค่าเริ่มต้น | คำอธิบาย |
| :--- | :--- | :--- | :--- | :--- |
| `CategoryId` | INT | PK, AUTO_INCREMENT | - | รหัสหมวดหมู่ |
| `CategoryName` | VARCHAR(100) | NOT NULL | - | ชื่อหมวดหมู่ (เช่น Scale Figures, Nendoroid, กล่องสุ่ม) |
| `Slug` | VARCHAR(100) | UNIQUE, NOT NULL | - | URL Slug สำหรับค้นหาและจัดหมวด |
| `IsActive` | TINYINT(1) | NOT NULL | 1 | สถานะเปิดแสดงผล |

#### ตาราง: `Products` (ข้อมูลฟิกเกอร์และโมเดล)
| คอลัมน์ | ชนิดข้อมูล | Constraints | ค่าเริ่มต้น | คำอธิบาย |
| :--- | :--- | :--- | :--- | :--- |
| `ProductId` | INT | PK, AUTO_INCREMENT | - | รหัสสินค้า |
| `Sku` | VARCHAR(50) | UNIQUE, NOT NULL, INDEX | - | รหัส SKU (เช่น `SKU-FG-9901`) |
| `Barcode` | VARCHAR(50) | UNIQUE, NULL, INDEX | - | บาร์โค้ด EAN-13 / QR Code สำหรับสแกน |
| `Name` | VARCHAR(200) | NOT NULL | - | ชื่อสินค้าภาษาไทย/อังกฤษ |
| `CategoryId` | INT | FK -> `Categories(CategoryId)` | - | หมวดหมู่สินค้า |
| `SeriesName` | VARCHAR(150) | NULL | - | ชื่อซีรีส์ / อนิเมะ / เกม (เช่น Naruto, Vocaloid) |
| `BrandName` | VARCHAR(100) | NULL | - | ค่ายผู้ผลิตลิขสิทธิ์แท้ (เช่น Good Smile Company) |
| `Scale` | VARCHAR(30) | NULL | - | สัดส่วนสเกล (เช่น 1/7, Non-Scale) |
| `HeightCm` | DECIMAL(6,2) | NULL, CHECK (HeightCm > 0) | - | ขนาดความสูง (ซม.) |
| `Material` | VARCHAR(100) | NULL | - | วัสดุหลัก (เช่น PVC, ABS) |
| `RetailPrice` | DECIMAL(10,2) | NOT NULL, CHECK (RetailPrice >= 0) | - | ราคาจำหน่ายหน้าร้านปกติ |
| `PreorderPrice` | DECIMAL(10,2) | NULL, CHECK (PreorderPrice >= 0) | - | ราคาพิเศษช่วงพรีออเดอร์ |
| `ExpectedArrival`| VARCHAR(100) | NULL | - | กำหนดสินค้าเข้าร้านโดยประมาณ |
| `IsPreorder` | TINYINT(1) | NOT NULL | 0 | สินค้าเปิดรับพรีออเดอร์หรือไม่ (1: ใช่, 0: พร้อมส่ง) |
| `PreorderDeadline`| DATETIME | NULL | - | วันหมดเขตรับสั่งจองพรีออเดอร์ |
| `ImageUrl` | VARCHAR(255) | NOT NULL | - | เส้นทางไฟล์รูปภาพหน้าปก (`/media/images/...`) |
| `Model3DPath` | VARCHAR(255) | NULL | - | เส้นทางไฟล์โมเดล 3D (`/media/models/...glb`) |
| `IsFeaturedHero` | TINYINT(1) | NOT NULL | 0 | แสดงที่ส่วนบนสุดของหน้าแรกแบบ 3D หรือไม่ |
| `IsActive` | TINYINT(1) | NOT NULL | 1 | เปิดแสดงบนหน้าร้าน |
| `CreatedAt` | DATETIME | NOT NULL | CURRENT_TIMESTAMP | วันที่สร้างรายการ |

#### ตาราง: `Inventories` (คลังสต็อกสินค้าหน้าร้าน)
| คอลัมน์ | ชนิดข้อมูล | Constraints | ค่าเริ่มต้น | คำอธิบาย |
| :--- | :--- | :--- | :--- | :--- |
| `InventoryId` | INT | PK, AUTO_INCREMENT | - | รหัสสต็อก |
| `ProductId` | INT | UNIQUE, FK -> `Products(ProductId)` | - | รหัสสินค้า (1:1) |
| `QuantityOnHand` | INT | NOT NULL, CHECK (QuantityOnHand >= 0) | 0 | จำนวนคงเหลือจริงในร้าน |
| `ReservedQuantity` | INT | NOT NULL, CHECK (ReservedQuantity >= 0) | 0 | จำนวนที่ถูกลูกค้าจองพรีออเดอร์รอรับ |
| `AvailableQuantity`| INT | GENERATED ALWAYS AS (QuantityOnHand - ReservedQuantity) STORED | - | จำนวนที่พร้อมขายทันทีหน้าร้าน |
| `SafetyStockLevel` | INT | NOT NULL | 3 | ระดับเตือนภัยใกล้หมด |
| `StorageLocation` | VARCHAR(100) | NULL | - | ตำแหน่งจัดเก็บ (เช่น ชั้นวาง A-01, ตู้โชว์ G-01) |
| `LastRestockedAt` | DATETIME | NULL | - | วันเวลาที่สแกนรับเข้าสต็อกล่าสุด |
| `UpdatedAt` | DATETIME | NOT NULL | CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP | วันที่อัปเดตสต็อกล่าสุด |

---

### 2.3 กลุ่มตะกร้าสินค้าและพรีออเดอร์ (Cart & Preorders)

#### ตาราง: `CartItems` (สินค้าในตะกร้าชั่วคราว)
| คอลัมน์ | ชนิดข้อมูล | Constraints | ค่าเริ่มต้น | คำอธิบาย |
| :--- | :--- | :--- | :--- | :--- |
| `CartItemId` | INT | PK, AUTO_INCREMENT | - | รหัสแถวในตะกร้า |
| `UserId` | INT | FK -> `Users(UserId)` | - | รหัสลูกค้าเจ้าของตะกร้า |
| `ProductId` | INT | FK -> `Products(ProductId)` | - | รหัสสินค้าที่เลือก |
| `Quantity` | INT | NOT NULL, CHECK (Quantity > 0) | 1 | จำนวนชิ้นที่ระบุ |
| `CreatedAt` | DATETIME | NOT NULL | CURRENT_TIMESTAMP | วันเวลาที่เพิ่มลงตะกร้า |

#### ตาราง: `Preorders` (คำสั่งจองพรีออเดอร์)
| คอลัมน์ | ชนิดข้อมูล | Constraints | ค่าเริ่มต้น | คำอธิบาย |
| :--- | :--- | :--- | :--- | :--- |
| `PreorderId` | INT | PK, AUTO_INCREMENT | - | รหัสคำสั่งจอง |
| `ReservationNumber` | VARCHAR(30) | UNIQUE, NOT NULL, INDEX | - | หมายเลขอ้างอิงใบจอง (เช่น `PO-000482`) |
| `UserId` | INT | FK -> `Users(UserId)` | - | ลูกค้าผู้สั่งจอง |
| `ContactPhone` | VARCHAR(20) | NOT NULL, INDEX | - | เบอร์โทรศัพท์สำหรับรับ SMS แจ้งเตือน |
| `EstimatedPickupDate`| DATE | NOT NULL | - | วันที่ลูกค้านัดหมายเข้ารับสินค้าที่ร้าน |
| `CurrentStatus` | VARCHAR(50) | NOT NULL | `'PendingConfirm'` | สถานะ (`PendingConfirm`, `WaitingStock`, `ReadyForPickup`, `Completed`, `Cancelled`) |
| `TotalFinalPrice` | DECIMAL(10,2) | NOT NULL | 0.00 | ยอดราคารวมทั้งหมดของคำสั่งจอง |
| `PickupBranch` | VARCHAR(100) | NOT NULL | `'FriendHouse HQ'` | สาขาที่นัดรับสินค้า |
| `CreatedAt` | DATETIME | NOT NULL | CURRENT_TIMESTAMP | วันที่ทำรายการจอง |
| `CompletedAt` | DATETIME | NULL | - | วันที่ลูกค้ารับสินค้าและปิดบิล |

#### ตาราง: `PreorderItems` (รายการฟิกเกอร์ในใบจอง)
| คอลัมน์ | ชนิดข้อมูล | Constraints | ค่าเริ่มต้น | คำอธิบาย |
| :--- | :--- | :--- | :--- | :--- |
| `ItemId` | INT | PK, AUTO_INCREMENT | - | รหัสรายการในใบจอง |
| `PreorderId` | INT | FK -> `Preorders(PreorderId)` ON DELETE CASCADE | - | รหัสใบจอง |
| `ProductId` | INT | FK -> `Products(ProductId)` | - | รหัสสินค้าฟิกเกอร์ |
| `UnitPrice` | DECIMAL(10,2) | NOT NULL | - | ราคาพรีออเดอร์ต่อหน่วย ณ วันสั่งจอง |
| `Quantity` | INT | NOT NULL, CHECK (Quantity > 0) | 1 | จำนวนที่จอง |
| `LineTotal` | DECIMAL(10,2) | GENERATED ALWAYS AS (UnitPrice * Quantity) STORED | - | ยอดรวมของแถว |

---

### 2.4 กลุ่มของรางวัลสะสมแต้ม (Loyalty Rewards)

#### ตาราง: `Rewards` (ของรางวัลแต้มสะสม)
| คอลัมน์ | ชนิดข้อมูล | Constraints | ค่าเริ่มต้น | คำอธิบาย |
| :--- | :--- | :--- | :--- | :--- |
| `RewardId` | INT | PK, AUTO_INCREMENT | - | รหัสของรางวัล |
| `RewardCode` | VARCHAR(30) | UNIQUE, NOT NULL | - | รหัสอ้างอิงของรางวัล (เช่น `RWD-KEY-DEMON`) |
| `Title` | VARCHAR(150) | NOT NULL | - | ชื่อของรางวัล (เช่น พวงกุญแจ, แก้วมัค) |
| `Brand` | VARCHAR(100) | NULL | - | แบรนด์หรือลิขสิทธิ์ของพรีเมียม |
| `CategoryTag` | VARCHAR(50) | NOT NULL | `'Merchandise'` | หมวดของรางวัล (Figures, Merchandise) |
| `PointsRequired` | INT | NOT NULL, CHECK (PointsRequired > 0) | - | จำนวนแต้มสะสมที่ต้องใช้แลก |
| `StockQuantity` | INT | NOT NULL, CHECK (StockQuantity >= 0) | 0 | จำนวนชิ้นคงเหลือในสต็อก |
| `ImageUrl` | VARCHAR(255) | NOT NULL | - | รูปภาพประกอบของรางวัล (`/media/images/...`) |
| `IsAvailable` | TINYINT(1) | NOT NULL | 1 | สถานะเปิดให้แลกรางวัล |
| `CreatedAt` | DATETIME | NOT NULL | CURRENT_TIMESTAMP | วันที่เพิ่มของรางวัลเข้าระบบ |

#### ตาราง: `RewardRedemptions` (ประวัติการแลกของรางวัล)
| คอลัมน์ | ชนิดข้อมูล | Constraints | ค่าเริ่มต้น | คำอธิบาย |
| :--- | :--- | :--- | :--- | :--- |
| `RedemptionId` | BIGINT | PK, AUTO_INCREMENT | - | รหัสรายการแลกรางวัล |
| `RedemptionCode` | VARCHAR(40) | UNIQUE, NOT NULL, INDEX | - | รหัสคูปองแสดงรับของหน้าร้าน (เช่น `RDM-20260909-001`) |
| `UserId` | INT | FK -> `Users(UserId)` | - | ลูกค้าผู้กดแลกของรางวัล |
| `RewardId` | INT | FK -> `Rewards(RewardId)` | - | ของรางวัลที่ได้รับ |
| `PointsSpent` | INT | NOT NULL, CHECK (PointsSpent > 0) | - | แต้มสะสมที่ถูกหักออก |
| `Status` | VARCHAR(30) | NOT NULL | `'PendingPickup'` | สถานะ (`PendingPickup`, `Completed`, `Cancelled`) |
| `PickupBranch` | VARCHAR(100) | NOT NULL | `'FriendHouse HQ'` | สาขาที่นัดรับของรางวัล |
| `RedeemedAt` | DATETIME | NOT NULL | CURRENT_TIMESTAMP | วันเวลาที่กดยืนยันการแลก |
| `CompletedAt` | DATETIME | NULL | - | วันเวลาที่พนักงานส่งมอบของและปิดบิล |

---

### 2.5 กลุ่มโปรโมชั่นและส่วนลด (Promotions)

#### ตาราง: `Promotions` (แคมเปญโปรโมชั่น 5 รูปแบบ)
| คอลัมน์ | ชนิดข้อมูล | Constraints | ค่าเริ่มต้น | คำอธิบาย |
| :--- | :--- | :--- | :--- | :--- |
| `PromotionId` | INT | PK, AUTO_INCREMENT | - | รหัสโปรโมชั่น |
| `Name` | VARCHAR(150) | NOT NULL | - | ชื่อโปรโมชั่น |
| `VoucherCode` | VARCHAR(50) | UNIQUE, NOT NULL, INDEX | - | รหัสคูปองส่วนลด (เช่น `CLEARANCE50`) |
| `PromoType` | VARCHAR(50) | NOT NULL | - | รูปแบบ (`percent`, `fixed`, `bogo`, `bundle`, `double_points`) |
| `DiscountValue` | DECIMAL(10,2) | NULL, CHECK (DiscountValue >= 0) | - | มูลค่าส่วนลด (% หรือ บาท) |
| `MinimumSpend` | DECIMAL(10,2) | NULL, CHECK (MinimumSpend >= 0) | 0.00 | ยอดสั่งซื้อขั้นต่ำที่ได้รับสิทธิ์ |
| `StartDate` | DATE | NOT NULL | - | วันเริ่มต้นโปรโมชั่น |
| `EndDate` | DATE | NOT NULL | - | วันหมดเขตโปรโมชั่น |
| `ApplicableScope`| VARCHAR(50) | NOT NULL | `'all'` | ขอบเขตสินค้า (`all`, `clearance`, `figures`) |
| `UsageLimit` | INT | NULL, CHECK (UsageLimit > 0) | - | จำนวนครั้งการใช้งานสูงสุด |
| `CurrentUsageCount`| INT | NOT NULL, CHECK (CurrentUsageCount >= 0) | 0 | จำนวนครั้งที่มีการใช้สิทธิ์ไปแล้ว |
| `IsActive` | TINYINT(1) | NOT NULL | 1 | สวิตช์เปิด/ปิดการใช้งานทันที |

---

### 2.6 กลุ่มการเงิน บัญชี และใบเสร็จ (Finance, Ledger & Receipts)

#### ตาราง: `FinancialLedger` (บันทึกบัญชีรายรับ-รายจ่าย)
| คอลัมน์ | ชนิดข้อมูล | Constraints | ค่าเริ่มต้น | คำอธิบาย |
| :--- | :--- | :--- | :--- | :--- |
| `EntryId` | INT | PK, AUTO_INCREMENT | - | รหัสบันทึกบัญชี |
| `EntryType` | VARCHAR(20) | NOT NULL | `'expense'` | ประเภท (`expense` หรือ `income`) |
| `CategoryName` | VARCHAR(100) | NOT NULL | - | ชื่อหมวดหมู่ (เช่น ค่าเช่าพื้นที่, ค่าน้ำไฟ, ค่าขนส่ง) |
| `Description` | VARCHAR(255) | NOT NULL | - | รายละเอียดคำอธิบายรายการ |
| `Amount` | DECIMAL(12,2) | NOT NULL, CHECK (Amount > 0) | - | จำนวนเงิน (บาท) |
| `EntryDate` | DATE | NOT NULL, INDEX | - | วันที่เกิดรายการ |
| `RecordedByUserId`| INT | FK -> `Users(UserId)` | - | นักบัญชีผู้บันทึกรายการ |
| `CreatedAt` | DATETIME | NOT NULL | CURRENT_TIMESTAMP | วันเวลาที่บันทึกลงระบบ |

#### ตาราง: `Receipts` (ใบเสร็จรับเงินอย่างเป็นทางการ)
| คอลัมน์ | ชนิดข้อมูล | Constraints | ค่าเริ่มต้น | คำอธิบาย |
| :--- | :--- | :--- | :--- | :--- |
| `ReceiptId` | INT | PK, AUTO_INCREMENT | - | รหัสใบเสร็จในระบบ |
| `ReceiptNo` | VARCHAR(40) | UNIQUE, NOT NULL, INDEX | - | เลขที่ใบเสร็จ (เช่น `RC-25690909-0024`) |
| `PreorderId` | INT | NULL, FK -> `Preorders(PreorderId)` | - | เชื่อมโยงกับคำสั่งพรีออเดอร์ (ถ้ามี) |
| `CustomerId` | INT | NULL, FK -> `Users(UserId)` | - | สมาชิกลูกค้าที่รับแต้ม (ข้ามการกรอกได้) |
| `CustomerPhone` | VARCHAR(20) | NULL | - | เบอร์โทรศัพท์ลูกค้าสำหรับค้นหาบิล (ถ้ามี) |
| `CashierUserId` | INT | FK -> `Users(UserId)` | - | พนักงานแคชเชียร์ผู้ออกใบเสร็จ |
| `SubtotalAmount`| DECIMAL(10,2) | NOT NULL, CHECK (SubtotalAmount >= 0) | - | ยอดรวมก่อนภาษี/ส่วนลด |
| `DiscountAmount`| DECIMAL(10,2) | NOT NULL, CHECK (DiscountAmount >= 0) | 0.00 | ยอดส่วนลดจากโปรโมชั่น |
| `VatAmount` | DECIMAL(10,2) | NOT NULL, CHECK (VatAmount >= 0) | - | ภาษีมูลค่าเพิ่ม 7% (รวมในราคา) |
| `NetTotalAmount`| DECIMAL(10,2) | NOT NULL, CHECK (NetTotalAmount >= 0) | - | ยอดเงินสุทธิที่รับชำระจริงหน้าร้าน |
| `BarcodeData` | VARCHAR(100) | NOT NULL | - | ข้อมูลบาร์โค้ดสำหรับสแกนตรวจสอบบิล |
| `IssuedAt` | DATETIME | NOT NULL, INDEX | CURRENT_TIMESTAMP | วันเวลาที่ออกใบเสร็จ |

#### ตาราง: `ReceiptItems` (รายการสินค้าในใบเสร็จรับเงิน)
| คอลัมน์ | ชนิดข้อมูล | Constraints | คำอธิบาย |
| :--- | :--- | :--- | :--- |
| `ReceiptItemId` | INT | PK, AUTO_INCREMENT | รหัสรายการ |
| `ReceiptId` | INT | FK -> `Receipts(ReceiptId)` ON DELETE CASCADE | รหัสใบเสร็จแม่ |
| `ProductId` | INT | NULL, FK -> `Products(ProductId)` | รหัสสินค้าฟิกเกอร์ |
| `ItemDescription`| VARCHAR(200) | NOT NULL | ชื่อรายการที่พิมพ์บนบิล |
| `Quantity` | INT | NOT NULL, CHECK (Quantity > 0) | จำนวนชิ้น |
| `UnitPrice` | DECIMAL(10,2) | NOT NULL, CHECK (UnitPrice >= 0) | ราคาต่อหน่วย |
| `LineTotal` | DECIMAL(10,2) | GENERATED ALWAYS AS (UnitPrice * Quantity) STORED | ยอดรวมของบรรทัดนี้ |

---

### 2.7 กลุ่มบันทึกระบบและการตรวจสอบ (System Logs & Audit)

#### ตาราง: `SystemLogs` (บันทึกเหตุการณ์และข้อผิดพลาดระบบ)
| คอลัมน์ | ชนิดข้อมูล | Constraints | ค่าเริ่มต้น | คำอธิบาย |
| :--- | :--- | :--- | :--- | :--- |
| `LogId` | BIGINT | PK, AUTO_INCREMENT | - | รหัส Log |
| `LogCode` | VARCHAR(30) | UNIQUE, NOT NULL | - | รหัสเหตุการณ์แสดงผล (เช่น `LOG-9021`) |
| `LogLevel` | VARCHAR(20) | NOT NULL, INDEX | - | ระดับความสำคัญ (`Info`, `Warning`, `Error`, `Critical`) |
| `Module` | VARCHAR(100) | NOT NULL, INDEX | - | โมดูล/บริการ (เช่น `StockSyncModule`, `AuthModule`) |
| `Message` | VARCHAR(500) | NOT NULL | - | ข้อความเหตุการณ์สรุปย่อ |
| `DetailPayload`| LONGTEXT | NULL | - | รายละเอียดเชิงเทคนิค / ข้อมูล JSON / Stack Trace |
| `IpAddress` | VARCHAR(50) | NULL | - | IP Address ของเครื่องผู้ส่งคำขอ |
| `UserId` | INT | NULL, FK -> `Users(UserId)` | - | ผู้ใช้งานที่ทำให้เกิดเหตุการณ์ (ถ้ามี) |
| `Timestamp` | DATETIME | NOT NULL, INDEX | CURRENT_TIMESTAMP | วันและเวลาที่เกิดเหตุการณ์ |

---

## 3. ดัชนีและการเพิ่มประสิทธิภาพ (Indexes - MySQL Standard)

1. **การค้นหาและเข้าสู่ระบบ**:
   - `CREATE INDEX idx_users_email_role ON Users(Email, Role, IsActive);`
   - `CREATE INDEX idx_users_phone ON Users(Phone);`
2. **การค้นหาสินค้าและสต็อกหน้าร้าน**:
   - `CREATE INDEX idx_products_sku ON Products(Sku);`
   - `CREATE INDEX idx_products_barcode ON Products(Barcode);`
   - `CREATE INDEX idx_products_category ON Products(CategoryId, IsActive);`
3. **การค้นหาคำสั่งพรีออเดอร์ที่เคาน์เตอร์**:
   - `CREATE INDEX idx_preorders_phone_res ON Preorders(ContactPhone, ReservationNumber, CurrentStatus);`
4. **การตรวจสอบใบเสร็จและรายรับ-รายจ่าย**:
   - `CREATE INDEX idx_receipts_receiptno ON Receipts(ReceiptNo);`
   - `CREATE INDEX idx_receipts_issuedat ON Receipts(IssuedAt);`
   - `CREATE INDEX idx_ledger_date_type ON FinancialLedger(EntryDate, EntryType);`
5. **การกรอง System Logs**:
   - `CREATE INDEX idx_logs_level_time ON SystemLogs(LogLevel, Timestamp DESC);`

---

## 4. สรุปการแมปชนิดข้อมูล SQL Server → MySQL

| SQL Server (เดิม) | MySQL 8.0+ (ใหม่) | หมายเหตุ |
| :--- | :--- | :--- |
| `INT IDENTITY(1,1)` | `INT AUTO_INCREMENT` | เหมือนกัน แต่เขียนแตกต่าง |
| `BIGINT IDENTITY(1,1)` | `BIGINT AUTO_INCREMENT` | เหมือนกัน |
| `NVARCHAR(n)` | `VARCHAR(n)` | UTF8mb4 รองรับภาษาไทยในตัว |
| `DATETIME2` | `DATETIME` | รองรับ Microsecond เช่นกัน |
| `BIT` | `TINYINT(1)` | 0 = False, 1 = True |
| `NVARCHAR(MAX)` | `LONGTEXT` | รองรับข้อมูลขนาดใหญ่ |
| `AS (expr) PERSISTED` | `GENERATED ALWAYS AS (expr) STORED` | Computed Column |
| `SYSUTCDATETIME()` | `CURRENT_TIMESTAMP` | เวลา UTC ปัจจุบัน |

---

## 5. วิธีสร้างฐานข้อมูลบน MySQL Workbench

> ไฟล์ SQL Script พร้อมรัน: `friendhouse_db_mysql.sql` (โฟลเดอร์รากของโปรเจกต์ `d:/code/CSI402/`)

### วิธีรัน Script (Import):
1. เปิด **MySQL Workbench** เชื่อมต่อกับ MySQL Server
2. **File → Open SQL Script...** (`Ctrl+Shift+O`) → เลือกไฟล์ `friendhouse_db_mysql.sql`
3. กดปุ่ม **Execute All** (`Ctrl+Shift+Enter`)
4. กด **Refresh** ที่แผง Schemas → ฐานข้อมูล `friendhouse_db` และ 14 ตาราง + Seed Data จะปรากฏทันที

### วิธี Export นำไปใช้เครื่องอื่น:
1. **Server → Data Export**
2. เลือก `friendhouse_db` → **Export to Self-Contained File**
3. ติ๊ก **Include Create Schema**
4. กด **Start Export** → นำไฟล์ `.sql` ไปรันบนเครื่องปลายทางได้เลย
