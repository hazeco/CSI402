# Antigravity Project Instructions & Rules

## 1. Project Overview
- **Project Type**: ASP.NET Core MVC (.NET 10.0)
- **Project Directory**: `Khetsophon_CSI402_8456`
- **Solution File**: `Khetsophon_CSI402_8456.sln`

---

## 2. Backend & Controller Rules (Strict)
- **Do not touch backend**: ห้ามแตะต้องหรือแก้ไขโค้ดส่วน Backend, Database, Business Logic, หรือ Models
- **Focus on Frontend only**: รับผิดชอบเฉพาะส่วน Frontend (Views `.cshtml`, CSS, JS, Assets)
- **Controller Exception**:
  - อนุญาตให้สร้างไฟล์ Controller ใหม่ หรือเพิ่ม Action method ได้เฉพาะเพื่อเชื่อมโยงเส้นทางเปิดหน้า View เท่านั้น
  - รูปแบบที่อนุญาต:
    ```csharp
    public IActionResult ActionName()
    {
        return View();
    }
    ```
  - ห้ามใส่ Database query, Service injection, หรือ Business logic ใดๆ ใน Controller เด็ดขาด
- **Routing & Forms**:
  - ในไฟล์ View (`.cshtml`) ต้องกำหนด `asp-controller` และ `asp-action` ให้ถูกต้องสำหรับการนำทางเสมอ
  - ในแท็ก `<form>` หรือองค์ประกอบที่มี `asp-controller` และ `asp-action` **ห้ามใส่แอตทริบิวต์ `method`** (เช่น `method="post"` หรือ `method="get"`) เด็ดขาด ปล่อยให้ User นำไปจัดการในส่วน Backend เองภายหลัง
- **Media & Content Configuration (Top of File)**:
  - ทรัพยากรที่เป็น Media ทั้งหมด เช่น รูปภาพ (Images), วิดีโอ (Videos), โมเดล 3D, ไอคอน (Icons/SVG Paths) และข้อความเนื้อหาสำคัญ (Content Texts) **ต้องประกาศรวมเป็น Object/Data structure ไว้ที่ส่วนบนสุดของไฟล์ (บรรทัดต้นๆ เสมอ)** เช่น ในบล็อก C# `@{ ... }` ด้านบนของไฟล์ View ห้าม Hardcode ลิงก์รูปภาพหรือคอนฟิกไว้ลึกๆ ในแท็ก HTML ด้านล่าง
  - **แหล่งเก็บไฟล์ Media (Central Media Storage)**:
    - รูปภาพทั้งหมด: `wwwroot/media/images/`
    - โมเดล 3D: `wwwroot/media/models/`
    - วิดีโอ: `wwwroot/media/videos/`
  - **การตั้งชื่อและ Format**: ชื่อไฟล์ Media ต้องสื่อความหมายตรงกับรูป/มีเดียที่ใช้งานจริง (เช่น `naruto_kyubi_sage.glb`, `banner_preorder.png`) และใช้นามสกุลไฟล์ให้ถูกต้องตามประเภท

---

## 3. Design & UI Guidelines
- **No Emojis**: ห้ามใช้อีโมจิ (Emoji) ในโค้ดและ UI โดยเด็ดขาด
- **Language**: ใช้เฉพาะภาษาไทยสำหรับข้อความและเนื้อหาบน UI ทั้งหมด
- **Icons**: ใช้เฉพาะไฟล์หรือแท็ก SVG เท่านั้น (`<svg>...</svg>`) ห้ามใช้อีโมจิแทนไอคอน
- **Color Palette**:
  - Primary: `#2F39A9`
  - Secondary: `#92EEFF`
  - Accent: `#3027FF`
  - Text: `#050315`
  - Background: `#FBFBFE`
- **Tailwind CSS & CSS Limitation**:
  - ใช้ Tailwind CSS (Utility Classes) เป็นหลักในการจัดสไตล์ (อย่างน้อย 90% ขึ้นไป)
  - ห้ามเขียน Custom CSS หรือ Inline style โดยไม่จำเป็น หากจำเป็นต้องใช้จริงๆ ต้องใช้ไม่เกิน 10% ของการจัดสไตล์ทั้งหมด
- **Responsive Design**:
  - ทุกหน้าและทุกองค์ประกอบต้องรองรับการแสดงผลแบบ Responsive ทุกขนาดหน้าจอ (Mobile, Tablet, Desktop) อย่างสมบูรณ์

---

## 4. Commands
- Run project (inside `Khetsophon_CSI402_8456`):
  ```bash
  dotnet watch run
  ```
- Build project:
  ```bash
  dotnet build
  ```

  run `npm run tailwind:watch` in terminal
  run `dotnet watch run` in terminal


