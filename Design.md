## Do not use emoji in the code
use only Thai
use icon svg only

# Color
#2F39A9 primary
#92EEFF second
#3027ff Accent
#050315 text
#fbfbfe background

# Rule of code
- Don't do anything with backend only do frontend and set asp-controller, asp-action in frontend
- ในแท็ก <form> หรือที่มี asp-controller, asp-action ห้ามใส่แอตทริบิวต์ method (เช่น method="post" หรือ method="get")
- ทรัพยากรที่เป็น Media ทั้งหมด เช่น รูปภาพ วิดีโอ 3D ไอคอน และข้อความคงที่ ให้ประกาศรวมเป็น Object/Data structure ไว้บนหัวสุดของไฟล์ (บรรทัดต้นๆ เสมอ)
- แหล่งเก็บไฟล์ Media: รูปภาพเก็บใน `wwwroot/media/images/`, โมเดล 3D เก็บใน `wwwroot/media/models/`, วิดีโอเก็บใน `wwwroot/media/videos/`
- ชื่อไฟล์ Media ต้องตั้งให้สื่อความหมายตรงกับเนื้อหาจริงและใช้นามสกุลไฟล์ที่ถูกต้องตามฟอร์แมต (.png, .jpg, .webp, .glb, .mp4)
- ห้ามเขียน Custom CSS เองเป็นหลัก ให้ใช้ Tailwind CSS (Utility classes) ไม่น้อยกว่า 90% หากจำเป็นต้องใช้ CSS จริงๆ ต้องใช้ไม่เกิน 10% ของสไตล์ทั้งหมด
- ทุกหน้าต้องทำเป็น Responsive รองรับทุกขนาดหน้าจอ (Mobile, Tablet, Desktop)