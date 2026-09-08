# Khetsophon_CSI402_8456

โปรเจกต์ ASP.NET Core MVC (.NET 10.0) สำหรับรายวิชา CSI402

## ความต้องการของระบบ (Prerequisites)

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) ขึ้นไป
- [Node.js](https://nodejs.org/) (แนะนำ LTS) พร้อม npm — ใช้สำหรับ build Tailwind CSS
- Git

ตรวจสอบเวอร์ชันที่ติดตั้งด้วยคำสั่ง:

```bash
dotnet --version
node --version
npm --version
```

## วิธี Clone โปรเจกต์

```bash
git clone https://github.com/<username>/CSI402.git
cd CSI402
```

## วิธีรันโปรเจกต์

1. เข้าไปยังโฟลเดอร์โปรเจกต์หลัก (ที่มีไฟล์ `.csproj`):

   ```bash
   cd Khetsophon_CSI402_8456
   ```

2. ติดตั้ง .NET dependencies (restore packages):

   ```bash
   dotnet restore
   ```

3. ติดตั้ง Tailwind CSS (npm packages) — ดูรายละเอียดในหัวข้อ [ติดตั้งและใช้งาน Tailwind CSS](#ติดตั้งและใช้งาน-tailwind-css-กับ-aspnet-core):

   ```bash
   npm install
   ```

4. รันโปรเจกต์ — ต้องรันทั้ง .NET และตัว build Tailwind CSS พร้อมกัน (แนะนำเปิด 2 terminal):

   **Terminal 1** — build/watch Tailwind CSS:

   ```bash
   npm run tailwind:watch
   ```

   **Terminal 2** — รัน ASP.NET Core:

   ```bash
   dotnet run
   ```

   หรือรันแบบ watch mode เพื่อให้ auto-reload เมื่อมีการแก้ไขโค้ด C#/Razor:

   ```bash
   dotnet watch run
   ```

5. เปิดเบราว์เซอร์ไปที่ URL ที่แสดงใน terminal (โดยปกติจะเป็น `https://localhost:xxxx` หรือ `http://localhost:xxxx`)

## คำสั่งที่ใช้บ่อย

| คำสั่ง | คำอธิบาย |
| --- | --- |
| `dotnet restore` | ติดตั้ง/คืนค่า NuGet packages ที่โปรเจกต์ต้องใช้ |
| `dotnet build` | คอมไพล์โปรเจกต์ |
| `dotnet run` | รันโปรเจกต์ |
| `dotnet watch run` | รันโปรเจกต์แบบ auto-reload เมื่อมีการแก้ไขไฟล์ |
| `dotnet clean` | ลบไฟล์ที่ build ไว้ (โฟลเดอร์ `bin`, `obj`) |
| `dotnet build --configuration Release` | build แบบ Release |
| `npm install` | ติดตั้ง Tailwind CSS และ dependencies ของ npm |
| `npm run tailwind:watch` | build Tailwind CSS แบบ watch (auto-rebuild เมื่อแก้ view/css) |
| `npm run tailwind:build` | build Tailwind CSS แบบ minify ครั้งเดียว (ใช้ก่อน deploy) |

## ติดตั้งและใช้งาน Tailwind CSS กับ ASP.NET Core

โปรเจกต์นี้ใช้ [Tailwind CSS v4](https://tailwindcss.com/) แทน Bootstrap โดยใช้ **Tailwind CLI** (ไม่ต้องพึ่ง Node.js ตอนรัน production เพราะ output เป็นไฟล์ CSS ธรรมดา) ขั้นตอนติดตั้งมีดังนี้ (ทำไปแล้วในโปรเจกต์นี้ — ใช้เป็นแนวทางหากต้องการทำใหม่ที่โปรเจกต์อื่น):

1. เข้าไปยังโฟลเดอร์โปรเจกต์ (โฟลเดอร์ที่มีไฟล์ `.csproj`) แล้วสร้างไฟล์ `package.json`:

   ```bash
   cd Khetsophon_CSI402_8456
   npm init -y
   ```

2. ติดตั้ง Tailwind CSS และ CLI:

   ```bash
   npm install -D tailwindcss @tailwindcss/cli
   ```

3. สร้างไฟล์ CSS ต้นทาง (input) ที่ `wwwroot/css/app.css`:

   ```css
   @import "tailwindcss";
   ```

   Tailwind v4 จะสแกนหาไฟล์ `.cshtml` และ class ที่ใช้โดยอัตโนมัติ (ไม่ต้องสร้าง `tailwind.config.js` เพิ่มก็ได้)

4. เพิ่มคำสั่ง build ไว้ใน `package.json` (`scripts`):

   ```json
   {
     "scripts": {
       "tailwind:build": "tailwindcss -i ./wwwroot/css/app.css -o ./wwwroot/css/site.css --minify",
       "tailwind:watch": "tailwindcss -i ./wwwroot/css/app.css -o ./wwwroot/css/site.css --watch"
     }
   }
   ```

   คำสั่งนี้จะอ่านไฟล์ `app.css` แล้ว compile ออกมาเป็น `wwwroot/css/site.css` ซึ่งเป็นไฟล์ที่ `_Layout.cshtml` อ้างอิงอยู่แล้ว (`<link rel="stylesheet" href="~/css/site.css" />`) — จึงไม่ต้องแก้ layout เพิ่ม

5. รัน build ครั้งแรก หรือรันแบบ watch ระหว่างพัฒนา:

   ```bash
   npm run tailwind:build   # build ครั้งเดียว
   npm run tailwind:watch   # build อัตโนมัติทุกครั้งที่แก้ไฟล์
   ```

6. ระหว่างพัฒนา ให้รันคู่กันเสมอ: `npm run tailwind:watch` (หรือ `dotnet watch run` ฝั่ง .NET) เพื่อให้ class ของ Tailwind ที่เพิ่มใน `.cshtml` ถูก compile เข้า `site.css` ทันที

> **หมายเหตุ:** ไฟล์ `wwwroot/css/site.css` เป็นไฟล์ที่ build ขึ้นมาจาก Tailwind ห้ามแก้ไขตรง ๆ เพราะจะถูกเขียนทับทุกครั้งที่ build ใหม่ ให้แก้ที่ `wwwroot/css/app.css` หรือใส่ utility class ลงไปใน `.cshtml` โดยตรงแทน

## โครงสร้างโปรเจกต์

```
CSI402/
├── Khetsophon_CSI402_8456.sln          # Solution file
└── Khetsophon_CSI402_8456/             # โปรเจกต์หลัก (ASP.NET Core MVC)
    ├── Controllers/                    # Controllers
    ├── Models/                         # Models
    ├── Views/                          # Views (Razor)
    ├── wwwroot/
    │   └── css/
    │       ├── app.css                 # ไฟล์ต้นทาง Tailwind (@import "tailwindcss")
    │       └── site.css                # ไฟล์ CSS ที่ build แล้ว (ห้ามแก้ตรง ๆ)
    ├── Program.cs                      # จุดเริ่มต้นของแอปพลิเคชัน
    ├── appsettings.json                # การตั้งค่าแอปพลิเคชัน
    ├── package.json                    # dependencies ของ Tailwind CSS
    └── Khetsophon_CSI402_8456.csproj   # ไฟล์โปรเจกต์
```

## หมายเหตุ

- โปรเจกต์นี้ใช้ ASP.NET Core MVC pattern
- หากต้องการเปิดด้วย Visual Studio หรือ VS Code ให้เปิดไฟล์ `Khetsophon_CSI402_8456.sln` หรือโฟลเดอร์ root ของ repo


#2F39A9 primary
#92EEFF second