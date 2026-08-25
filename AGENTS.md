# AGENTS.md — EduTrack CRM

Bu fayl AI agentlar (Claude Code va boshqalar) va yangi developerlar uchun loyiha qo'llanmasi.
Kod yozishdan oldin shu faylni o'qing. Har bir o'zgarishdan keyin `docs.md` ga yozib qo'ying.

---

## 1. Loyiha haqida

**EduTrack** — o'quv markazlari (education center) uchun CRM tizimi.
Filiallar, xonalar, guruhlar, o'quvchilar, o'qituvchilar, davomat va to'lovlarni boshqaradi.

- **Stack:** ASP.NET Core MVC (Razor Views), .NET **10** (`net10.0`)
- **DB:** SQL Server (LocalDB), Entity Framework Core **9.0.6** (Code First)
- **Mapping:** AutoMapper **16.0.0**
- **UI:** Bootstrap 5 + Bootstrap Icons (CDN) + jQuery (`wwwroot/lib/`)
- **Test:** yo'q (hozircha test project mavjud emas)

---

## 2. Arxitektura — 4 qavatli (N-Layer)

```
EduTrack.sln
└── src/
    ├── EduTrack.Domain/    ← Entity, Enum, Auditable base class. Hech kimga bog'liq emas.
    ├── EduTrack.Data/      ← DbContext, Migrations, Generic Repository.  → Domain
    ├── EduTrack.Service/   ← DTO, Service, Mapping, CustomException.     → Data, Domain
    └── EduTrack.MVC/       ← Controller, View, DI, Program.cs.           → Service, Data
```

**Bog'liqlik qoidasi (dependency rule):** pastdan yuqoriga. Domain hech kimni bilmaydi,
MVC hammani biladi. **Teskarisi qilmang** — masalan Domain ichida Service'ni chaqirmang.

### Qatlamlar vazifasi

| Qatlam | Nima bo'ladi | Nima BO'LMAYDI |
|---|---|---|
| Domain | Entity class'lar, `UserRole` enum, `Auditable` | Biznes logika, EF konfiguratsiya |
| Data | `EduDbContext`, `Repository<T>`, migration, seed data | DTO, biznes qoidalar |
| Service | Biznes logika, validatsiya, DTO ↔ Entity mapping | HTTP, ViewBag, `HttpContext` |
| MVC | Controller, View, DI ro'yxatga olish | Biznes logika, to'g'ridan-to'g'ri `DbContext` |

**Muhim:** Controller **hech qachon** `IRepository<T>` yoki `EduDbContext` ni to'g'ridan-to'g'ri
ishlatmasin. Faqat `IXxxService` orqali ishlaydi.

---

## 3. Domain modeli

```
Branch (filial)
 ├── Rooms (1:N)
 └── Groups (1:N)

Room (xona) ── BranchId
 └── Groups (1:N)

User (xodim) ── Role: UserRole
 └── Groups (1:N, Teacher sifatida)

Group (guruh) ── BranchId, RoomId, TeacherId (nullable)
 ├── StudentGroups (N:N o'quvchilar bilan)
 ├── Attendances (1:N)
 └── Payments (1:N)

Student (o'quvchi)
 ├── StudentGroups (N:N guruhlar bilan)
 ├── Payments (1:N)
 └── Attendances (1:N)

StudentGroup ── COMPOSITE KEY: (StudentId, GroupId)
Attendance   ── StudentId, GroupId, Date, IsPresent
Payment      ── StudentId, GroupId, Amount, ForMonth, PaymentMethod
```

### `Auditable` base class

Barcha entity'lar `Auditable` dan meros oladi:

```csharp
public class Auditable
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;   // SOFT DELETE
}
```

`IRepository<TEntity>` generic constraint'i `where TEntity : Auditable` —
yangi entity qo'shsangiz, albatta `Auditable` dan meros oling.

### `UserRole` enum (`Domain/Enums/UserRole.cs`)

| Qiymat | Rol | Izoh |
|---|---|---|
| 0 | Admin | To'liq huquq |
| 1 | Director | Filial boshqaruvi, hisobotlar |
| 2 | Manager | Guruh va o'quvchi boshqaruvi |
| 3 | Teacher | Dars berish, davomat |
| 4 | AssistantTeacher | Yordamchi o'qituvchi |
| 5 | Accountant | To'lovlar |

⚠️ Seed data'da rollar `(UserRole)1` ko'rinishida yozilgan va **enum ma'nosiga mos emas** —
"Teacher" deb belgilangan userlar aslida `Director` (1) bo'lib qolgan.
`UserService.GetAllTeachersAsync()` esa `Teacher || AssistantTeacher` bo'yicha filtrlaydi,
shuning uchun seed'dagi "teacher"lar dropdownlarda ko'rinmaydi. Batafsil: §9 Known Issues.

---

## 4. Kod konvensiyalari (mavjud kodga moslashing)

### Namespace va fayl joylashuvi
- File-scoped namespace afzal: `namespace EduTrack.Service.Services;`
  (eski fayllarda block-scoped `{ }` ham bor — mavjud faylni o'zgartirmang, yangisida file-scoped ishlating)
- Papka strukturasi namespace bilan bir xil bo'lsin.

### DTO qoidalari
Har bir entity uchun **3 ta DTO** (`Service/DTOs/<Entities>/` papkasida):

```
XxxCreationDto   — yaratish uchun (Id yo'q)
XxxUpdateDto     — tahrirlash uchun (Id bor)
XxxResultDto     — natija qaytarish uchun (Id, CreatedAt, UpdatedAt bor)
```

⚠️ Yangi DTO yozganda **navigation property (`Student`, `Group`, ...) qo'ymang** —
faqat `StudentId`, `GroupId` kabi ID'lar bo'lsin. Mavjud `Attendance*` va `Payment*`
DTO'larda entity qo'yilgan, bu xato pattern (§9 ga qarang), takrorlamang.

### Service pattern

```csharp
public class XxxService(IRepository<Xxx> repository, IMapper mapper) : IXxxService
{
    private readonly IRepository<Xxx> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<XxxResultDto> AddAsync(XxxCreationDto dto) { ... }
    public async Task<XxxResultDto> UpdateAsync(int id, XxxUpdateDto dto) { ... }
    public async Task<XxxResultDto> GetByIdAsync(int id) { ... }
    public async Task<IEnumerable<XxxResultDto>> GetAllAsync() { ... }
    public async Task<bool> RemoveAsync(int id) { ... }

    // Har bir service'da private helper — mavjudligini tekshiradi
    private async Task<Xxx> IsExistAsync(int id)
    {
        var entity = await _repository.SelectAll()
            .Where(x => x.IsDeleted == false && x.Id == id)
            .Include(...)                        // kerakli navigationlar
            .FirstOrDefaultAsync()
            ?? throw new CustomException(404, "Xxx not found");
        return entity;
    }
}
```

**Qoidalar:**
- Primary constructor ishlating (C# 12 sintaksisi) — `StudentService`/`StudentGroupService` eski uslubda,
  ularni o'zgartirish shart emas.
- **Har doim `.Where(x => x.IsDeleted == false)`** qo'shing — soft delete shunday ishlaydi,
  global query filter yo'q.
- Xatolik uchun `throw new CustomException(statusCode, message)` — oddiy `Exception` emas.
- `UpdateAsync` da `entity.UpdatedAt = DateTime.UtcNow;` ni qo'lda o'rnating —
  DbContext buni avtomatik qilmaydi.

### Repository

`IRepository<T>` (`Data/IRepositories/IRepository.cs`) — bitta generic repository, hamma uchun.

```csharp
Task<bool> DeleteAsync(int id);              // SOFT delete (IsDeleted = true)
IQueryable<TEntity> SelectAll();             // Include/Where uchun IQueryable qaytaradi
Task<TEntity> SelectByIdAsync(int id);
Task<TEntity> SelectAsync(Expression<Func<TEntity, bool>> predicate);
Task<TEntity> InsertAsync(TEntity entity);   // SaveChanges ichida
Task<TEntity> UpdateAsync(TEntity entity);   // SaveChanges ichida
```

Har bir entity uchun alohida repository yozmang — DI da `IRepository<>` generic ro'yxatga olingan.

### Controller pattern

```csharp
public class XxxController(IXxxService service) : Controller
{
    private readonly IXxxService _service = service;

    public async Task<ActionResult> Index()
    {
        try
        {
            var items = await _service.GetAllAsync();
            return View(items);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An error occurred while loading ...";
            return View(new List<XxxResultDto>());
        }
    }
    // Details(int id), Create() GET, Create(dto) POST, Edit(int id) GET,
    // Edit(int id, dto) POST, Delete(int id) GET, DeleteConfirmed(int id) POST
}
```

**Qoidalar:**
- Har bir action `try/catch` ichida. `CustomException` ni alohida ushlab, uning `Message` ini
  ko'rsating; qolgan `Exception` uchun umumiy matn.
- Foydalanuvchiga xabar: `TempData["SuccessMessage"]` / `TempData["ErrorMessage"]`.
- POST action'larda `[HttpPost]` + `[ValidateAntiForgeryToken]`.
- Delete uchun: GET `Delete(id)` tasdiqlash sahifasini ko'rsatadi,
  POST `DeleteConfirmed(id)` (`[ActionName("Delete")]` bilan) o'chiradi.
- Dropdown ma'lumotlari `ViewBag.Branches`, `ViewBag.Rooms`, `ViewBag.Teachers` orqali.
  `GroupController.PopulateDropdownsAsync()` — namuna.
- AJAX endpointlar `Json(...)` qaytaradi: `{ success = true/false, message = "..." }` formatida.

### View konvensiyalari (`Views/<Controller>/`)

Har bir CRUD controller uchun 5 ta view: `Index`, `Create`, `Edit`, `Details`, `Delete`.

- Model — **har doim `ResultDto`/`CreationDto`/`UpdateDto`**, entity emas:
  `@model IEnumerable<EduTrack.Service.DTOs.Branches.BranchResultDto>`
- `_ViewStart.cshtml` → `Layout = "_Layout"` (avtomatik).
- Bootstrap 5 klasslari, ikonkalar `<i class="bi bi-...">`.
- Har bir Index'da `TempData["SuccessMessage"]` uchun alert bloki.
- Sahifa sarlavhasi: `ViewData["Title"] = "...";`
- Bo'sh ro'yxat uchun `alert-info` bilan "No ... found" bloki.

---

## 5. Yangi funksiya qo'shish ketma-ketligi

Yangi entity yoki modul qo'shayotganda **shu tartibda** boring:

1. **Domain** → `Entities/Xxx.cs` (`: Auditable`), kerak bo'lsa `Enums/`.
2. **Data** → `EduDbContext` ga `DbSet<Xxx>`, `ConfigureRelationships()` ga munosabatlar,
   kerak bo'lsa `SeedXxx()`.
3. **Migration** yarating (§6).
4. **Service** → `DTOs/Xxxs/` (3 ta DTO) → `Interfaces/IXxxService.cs` → `Services/XxxService.cs`
   → `Mappings/MappingProfile.cs` ga 3 ta `CreateMap`.
5. **DI** → `MVC/Extentions/ServiceExtention.cs` ga `services.AddScoped<IXxxService, XxxService>();`
   (fayl nomidagi "Extention" imlo xatosi — ataylab shunday, o'zgartirmang, hamma joyda shu nom).
6. **MVC** → `Controllers/XxxController.cs` + `Views/Xxx/` (5 ta view).
7. **Layout** → `Views/Shared/_Layout.cshtml` sidebar'ga link qo'shing.
8. **docs.md** → o'zgarishni yozib qo'ying.

---

## 6. Buyruqlar

```bash
# Build
dotnet build EduTrack.sln

# Ishga tushirish (http://localhost:5114 yoki https://localhost:7258)
dotnet run --project src/EduTrack.MVC

# Migration qo'shish (Data — startup MVC)
dotnet ef migrations add <MigrationName> \
  --project src/EduTrack.Data \
  --startup-project src/EduTrack.MVC

# Migrationni qo'lda qo'llash (odatda shart emas — §7 ga qarang)
dotnet ef database update --project src/EduTrack.Data --startup-project src/EduTrack.MVC

# Oxirgi migrationni bekor qilish
dotnet ef migrations remove --project src/EduTrack.Data --startup-project src/EduTrack.MVC
```

Visual Studio Package Manager Console'da: Default project = `EduTrack.Data`,
Startup project = `EduTrack.MVC`.

---

## 7. Ma'lumotlar bazasi

- Connection string: `src/EduTrack.MVC/appsettings.json` → `ConnectionStrings:DefaultConnection`
  → `Server=(localdb)\MSSQLLocalDB;Database=EduTrackDB;Trusted_Connection=True;TrustServerCertificate=True;`
- **Migration avtomatik qo'llanadi:** `Program.cs` ning oxirida `db.Database.Migrate()` chaqiriladi
  (commit `a84d975`). Ya'ni ilovani ishga tushirsangiz DB o'zi yangilanadi.
- **Seed data** `OnModelCreating` ichida `HasData()` orqali: 3 branch, 8 room, 14 user,
  8 group, 28 student, 4 enrollment. `SeedAttendances` — kommentga olingan.
- Seed'dagi `CreatedAt` **doimo statik** `new DateTime(2025, 1, 1)` bo'lishi kerak —
  `DateTime.Now` ishlatsangiz EF har build'da yangi migration talab qiladi.
- `DeleteBehavior`: aksariyat munosabatlarda `NoAction` (cascade cycle'ni oldini olish uchun),
  `StudentGroup` da `Cascade`, `Group.Teacher` da `SetNull`.

---

## 8. Hozirgi holat (2026-08-24 sanasiga)

### ✅ Tayyor
| Modul | Service | Controller | Views |
|---|---|---|---|
| Branch | ✅ | ✅ | ✅ 5 ta |
| Room | ✅ | ✅ | ✅ 5 ta |
| Student | ✅ | ✅ | ✅ 5 ta |
| User | ✅ | ✅ | ✅ 5 ta |
| Group | ✅ | ✅ | ✅ 5 ta |
| Enrollment (StudentGroup) | ✅ | ✅ | ✅ 4 ta |

### ⚠️ Yarim / yo'q
| Modul | Holat |
|---|---|
| **Payment** | Service ✅ tayyor. `PaymentController` — **bo'sh stub**, hatto `Controller` dan meros olmagan. View'lar yo'q. |
| **Attendance** | Service ✅ tayyor. Controller **yo'q**. View'lar yo'q. |
| **Admin panel** | Butunlay yo'q (`_Layout` da link bor). |
| **Authentication / Authorization** | **Yo'q.** `Program.cs` da `UseAuthorization()` bor, lekin `UseAuthentication()` yo'q va hech qanday `[Authorize]` ishlatilmagan. Rejalashtirilgan: `origin/5-implement-permission-based-authentication`, `origin/6-create-table-and-relationships-for-permission-authentication` branchlari. |
| **Dashboard** | `Home/Index.cshtml` — default ASP.NET template. |
| **Testlar** | Yo'q. |

`_Layout.cshtml` sidebar'da `Attendance`, `Payment`, `Admin` linklari bor —
ular hozircha **404 beradi**.

---

## 9. Known Issues (bilib turing, kod yozganda takrorlamang)

1. ✅ **TUZATILDI (2026-08-24)** — `StudentGroup` o'chirish buzilgan edi.
   `StudentGroup` ning primary key'i composite — `(StudentId, GroupId)`. `Id` ustuni bor,
   lekin key emas va identity emas (hamma qatorda `0`), shuning uchun `FindAsync(id)` ishlamasdi.
   Endi `IRepository<T>` da `DeleteAsync(TEntity entity)` overload'i bor —
   **composite key'li entity uchun har doim shu overload'ni ishlating**, `DeleteAsync(int id)` ni emas.
   Batafsil: `docs.md`.

2. ✅ **TUZATILDI (2026-08-24)** — `Repository.DeleteAsync` null tekshirmasdi
   (`NullReferenceException`). Endi topilmagan `id` uchun `false` qaytaradi.

3. ✅ **TUZATILDI (2026-08-24)** — `StudentGroupService` soft delete'ni hisobga olmasdi.
   Barcha query'larda `IsDeleted == false` filtri bor. `EnrollStudentAsync` esa soft delete
   qilingan enrollmentni qayta faollashtiradi (composite key tufayli yangi qator qo'shib bo'lmaydi).

4. **Seed'dagi rollar noto'g'ri.** §3 ga qarang — "Teacher" deb belgilangan userlar `Director` (1).
   Guruh yaratishda o'qituvchi dropdown'i bo'sh chiqadi.

5. **Seed'dagi parollar hash emas.** Manager'larda `PasswordHash = "Manager@123"` — ochiq matn.
   Qolganlarida bcrypt formatidagi soxta string, lekin `UserService.HashPassword()` **SHA-256**
   ishlatadi (salt yo'q). Auth qo'shilganda bu almashtirilishi kerak (BCrypt/Argon2).

6. **`AttendanceService.RemoveAsync` / `UpdateAsync` `await` qilmaydi.**
   `var x = _repository.SelectByIdAsync(id) ?? throw ...` — `Task` hech qachon null bo'lmaydi,
   ya'ni mavjudlik tekshiruvi **ishlamaydi**.

7. **`AttendanceService.UpdateAsync` yangi obyekt yasab `UpdateAsync` ga beradi** —
   `CreatedAt` nolga tushadi va `IsDeleted` yo'qoladi.

8. **`PaymentService.AddAsync` `CustomException` emas, oddiy `Exception` tashlaydi.**

9. **DTO'larda entity ochiq.** `AttendanceCreationDto`, `Payment*Dto`, `BranchResultDto`,
   `StudentResultDto`, `GroupResultDto` ichida to'g'ridan-to'g'ri Domain entity'lar bor.
   Shu sabab `Program.cs` da `ReferenceHandler.IgnoreCycles` yoqilgan.

10. **`GroupCreationDto.TeacherId` — `int`**, lekin `Group.TeacherId` — `int?`.
    O'qituvchisiz guruh yaratib bo'lmaydi.

11. **`UserResultDto` ichida `PasswordHash` bor** — view'ga uzatiladi. Xavfsizlik muammosi.

12. **Build'da 192 ta warning** (0 error): asosan `CS8602` (view'larda `ViewBag` null dereference)
    va `CS0168` (`catch (Exception ex)` da `ex` ishlatilmagan).

13. **`UserService.GetUserRoles()`** — interface'da yo'q, `IEnumHelperService.GetRoleList()`
    bilan bir xil ishni qiladi. Dublikat.

---

## 10. Til va matn

- Kod, class/method nomlari — **inglizcha**.
- Kommentlar va foydalanuvchiga ko'rinadigan xabarlar — loyihada **aralash** (o'zbekcha + inglizcha).
  `EnrollmentController` va `StudentGroupService` — o'zbekcha, qolganlari — inglizcha.
  Mavjud faylni tahrirlaganda **shu faylning tilida** davom eting.
- Commit message'lar — inglizcha, qisqa (`added seed data and fixed some entities field`).

---

## 11. Git

- Asosiy branch: `master`. `dev` branch ham mavjud.
- Feature branch nomi: `<issue-number>-<qisqa-tavsif>` (masalan `4-implement-services`).
- PR orqali `master` ga merge qilinadi.
- `.vs/`, `bin/`, `obj/` — `.gitignore` da.

---

## 12. Agent uchun ish tartibi

1. O'zgarish kiritishdan oldin `docs.md` ni o'qing — nima qilinganini biling.
2. Mavjud pattern'ga moslashing (§4). Yangi pattern kiritmang.
3. Qatlam chegarasini buzmang (§2).
4. `dotnet build EduTrack.sln` bilan tekshiring — **0 error** bo'lishi shart.
5. Entity o'zgarsa — migration yarating (§6).
6. **`docs.md` ga yozuv qo'shing** (formatni o'sha faylning boshida ko'rasiz).
7. Sinovdan o'tmagan narsani "ishlaydi" demang.
