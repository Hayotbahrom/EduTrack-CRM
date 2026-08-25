# docs.md — O'zgarishlar jurnali (Changelog)

Bu faylda **loyihaga kiritilgan har bir o'zgarish** yozib boriladi.
Kod o'zgartirgan har bir odam yoki AI agent shu faylga yozuv qo'shishi **majburiy**.

Loyiha arxitekturasi va kod qoidalari uchun → [`AGENTS.md`](AGENTS.md).

---

## Yozuv formati

Yangi yozuv **eng yuqoriga** (§ "O'zgarishlar tarixi" ostiga) qo'shiladi — teskari xronologik tartib.

```markdown
## [YYYY-MM-DD] Qisqa sarlavha

**Turi:** Feature | Fix | Refactor | Docs | DB | Chore
**Muallif:** <ism yoki agent nomi>
**Branch / Commit:** <branch nomi> / <commit hash yoki "—">

### Nima qilindi
- Qisqa, aniq punktlar.

### O'zgargan fayllar
- `path/to/file.cs` — nima o'zgardi
- `path/to/View.cshtml` — nima o'zgardi

### Migration
- Bor / Yo'q. Bor bo'lsa: `<MigrationName>` — qaysi jadval/ustun o'zgardi.

### Ta'sir (Breaking change / diqqat qilinadigan joylar)
- Boshqa qaysi qismga ta'sir qiladi. Yo'q bo'lsa — "Yo'q".

### Tekshirildi
- `dotnet build EduTrack.sln` → 0 error / N warning
- Qo'lda qaysi sahifa/action sinovdan o'tkazildi
```

**Qoidalar:**
- Sanani **to'liq** yozing (`2026-08-24`), "kecha"/"bugun" demang.
- Migration yaratilgan bo'lsa — nomini albatta yozing.
- Sinovdan o'tkazilmagan narsani "ishlaydi" deb yozmang; "sinovdan o'tkazilmadi" deb yozing.
- Breaking change bo'lsa — **qalin** qilib belgilang.

---

## O'zgarishlar tarixi

<!-- YANGI YOZUVLAR SHU YERGA, ENG YUQORIGA QO'SHILADI -->

## [2026-08-24] StudentGroup o'chirish bug'i tuzatildi

**Turi:** Fix
**Muallif:** Claude (Claude Code)
**Branch / Commit:** `master` / —

### Muammo
`StudentGroup` ning primary key'i composite — `(StudentId, GroupId)`. `Auditable` dan kelgan
`Id` ustuni bor, lekin u key ham, identity ham emas — barcha qatorlarda `0`.

`Repository.DeleteAsync(int id)` ichida `dbSet.FindAsync(id)` ishlatilgan edi. `FindAsync`
composite key uchun **ikkita** qiymat kutadi, bitta berilgani uchun runtime'da xato tashlardi.
`StudentGroupService.RemoveStudentAsync()` aynan shu yo'ldan o'tardi — ya'ni o'quvchini
guruhdan chiqarish umuman ishlamasdi.

Ikkita bog'liq muammo ham bor edi:
- `StudentGroupService` query'lari `IsDeleted` ni tekshirmasdi — chiqarilgan o'quvchi ro'yxatda qolaverardi.
- Composite key tufayli soft delete qilingan qator qayta enroll qilishni bloklardi
  ("allaqachon ro'yxatdan o'tgan" xatosi).

### Nima qilindi
- `IRepository<T>` ga `DeleteAsync(TEntity entity)` overload'i qo'shildi — entity'ni Id bo'yicha
  qidirmasdan to'g'ridan-to'g'ri soft delete qiladi. Composite key'li entity'lar uchun shu ishlatiladi.
- `Repository.DeleteAsync(int id)` da `FindAsync` o'rniga `Where(x => x.Id == id)` ishlatildi
  va **null tekshiruvi** qo'shildi (ilgari topilmagan `id` da `NullReferenceException` berardi —
  `AGENTS.md` §9.2 muammosi ham shu bilan yopildi). Endi `false` qaytaradi.
- Soft delete'da `UpdatedAt` ham o'rnatiladi.
- `StudentGroupService.RemoveStudentAsync()` yangi entity overload'ini ishlatadi va
  faqat `IsDeleted == false` qatorlarni qidiradi.
- `StudentGroupService.EnrollStudentAsync()` — soft delete qilingan enrollment topilsa,
  yangi qator qo'shish o'rniga mavjudini **qayta faollashtiradi** (`IsDeleted = false`).
  Faol enrollment bo'lsa, avvalgidek 400 xatosi.
- `GetStudentsByGroupAsync`, `GetGroupsByStudentAsync`, `GetStudentCountByGroupAsync` ga
  `IsDeleted == false` filtri qo'shildi (`AGENTS.md` §9.3). Bir vaqtning o'zida
  `GetStudentsByGroupAsync` va `GetStudentCountByGroupAsync` sinxron `.ToList()`/`.Count()`
  dan `.ToListAsync()`/`.CountAsync()` ga o'tkazildi.

### O'zgargan fayllar
- `src/EduTrack.Data/IRepositories/IRepository.cs` — `DeleteAsync(TEntity)` overload'i
- `src/EduTrack.Data/Repositories/Repository.cs` — `FindAsync` olib tashlandi, null guard, yangi overload
- `src/EduTrack.Service/Services/StudentGroupService.cs` — remove/enroll/query metodlari

### Migration
- Yo'q. Sxema o'zgarmadi.

### Ta'sir
- `IRepository<T>` ga yangi a'zo qo'shildi. Bu interface'ni implement qiladigan boshqa class
  loyihada yo'q (`Repository<T>` yagona), shuning uchun **breaking change emas**.
- Boshqa service'lar `DeleteAsync(int id)` ni ishlatishda davom etadi — ular oldindan
  `IsExistAsync()` bilan tekshirgani uchun xulq-atvor o'zgarmaydi. Faqat topilmagan `id` da
  endi exception o'rniga `false` qaytadi.
- Ilgari soft delete qilingan (lekin aslida hech qachon o'chirilmagan) enrollmentlar yo'q,
  chunki bu kod yo'li ishlamasdi — migratsiya/tozalash talab qilinmaydi.

### Tekshirildi
- `dotnet build EduTrack.sln --no-incremental` → **0 error, 191 warning** (baseline 192 edi)
- Ilova ishga tushirildi (LocalDB, `EduTrackDB`) va `Enrollment` endpointlari
  haqiqiy DB'da qo'lda sinovdan o'tkazildi:

  | Qadam | Natija |
  |---|---|
  | Boshlang'ich holat, guruh 1 | studentlar `{1,2,3,4}` |
  | `POST /Enrollment/Remove` (student 1) | `success: true` → `{2,3,4}` |
  | `POST /Enrollment/Enroll` (student 1) | `success: true` → `{1,2,3,4}` (qayta faollashtirish ishladi) |
  | Takroran `Enroll` (student 1) | `success: false` — "allaqachon ro'yxatdan o'tgan" |
  | `Remove` (ro'yxatda yo'q student 25) | `success: false` — "ro'yxatda yo'q" |

  Tuzatishdan oldin 2-qadam runtime xatosi bilan tugardi.
- Sinovdan keyin DB seed holatiga qaytdi (guruh 1 da yana `{1,2,3,4}`).

---

## [2026-08-24] AGENTS.md va docs.md qo'shildi

**Turi:** Docs
**Muallif:** Claude (Claude Code)
**Branch / Commit:** `master` / —

### Nima qilindi
- Loyiha to'liq tahlil qilindi (4 ta project, 8 entity, 9 service, 8 controller, 27 view).
- `AGENTS.md` yaratildi — arxitektura, kod konvensiyalari, buyruqlar, hozirgi holat, known issues.
- `docs.md` (shu fayl) yaratildi — kelajakdagi o'zgarishlar uchun jurnal.

### O'zgargan fayllar
- `AGENTS.md` — yangi
- `docs.md` — yangi

### Migration
- Yo'q.

### Ta'sir
- Yo'q. Faqat hujjat, kodga tegilmadi.

### Tekshirildi
- `dotnet build EduTrack.sln` → **0 error, 192 warning** (o'zgarishlardan oldingi holat, baseline sifatida yozildi).

---

# Ilova A — Loyihaning boshlang'ich holati (2026-08-24 baseline)

Bu bo'lim `AGENTS.md` yozilgan paytdagi holatni qayd etadi. Keyingi o'zgarishlar
yuqoridagi jurnalga yoziladi, bu bo'lim **o'zgarmaydi**.

## Modullar holati

| Modul | Entity | DTO | Service | Controller | Views | Holat |
|---|---|---|---|---|---|---|
| Branch | ✅ | ✅ | ✅ | ✅ | 5 | Tayyor |
| Room | ✅ | ✅ | ✅ | ✅ | 5 | Tayyor |
| Student | ✅ | ✅ | ✅ | ✅ | 5 | Tayyor |
| User | ✅ | ✅ | ✅ | ✅ | 5 | Tayyor |
| Group | ✅ | ✅ | ✅ | ✅ | 5 | Tayyor |
| StudentGroup (Enrollment) | ✅ | ✅ | ✅ | ✅ | 4 | Tayyor |
| Payment | ✅ | ✅ | ✅ | ❌ bo'sh stub | 0 | **Yarim** |
| Attendance | ✅ | ✅ | ✅ | ❌ yo'q | 0 | **Yarim** |
| Auth / Permission | ❌ | ❌ | ❌ | ❌ | 0 | **Yo'q** |
| Dashboard | — | — | — | ✅ default | 2 | **Bo'sh** |

## Migration holati

- Yagona migration: `20251226125426_InitialCommitWithSeedData`
- Startup'da avtomatik qo'llanadi (`Program.cs` → `db.Database.Migrate()`)

## Baseline build

```
dotnet build EduTrack.sln
→ 0 Error, 192 Warning
```

Warning turlari: `CS8602` (view'larda ViewBag null dereference),
`CS0168` (`catch (Exception ex)` da `ex` ishlatilmagan), `MVC1000` (`Html.Partial`).

---

# Ilova B — Keyingi qadamlar (Backlog)

Bajarilganda yuqoridagi jurnalga yozuv qo'shing va bu ro'yxatda ✅ belgilang.

### Prioritet 1 — buzilgan narsalar
- [x] `StudentGroup` o'chirish bug'i (composite key + `FindAsync`) — §9.1 — *2026-08-24*
- [x] `Repository.DeleteAsync` da null tekshiruvi — §9.2 — *2026-08-24*
- [x] `StudentGroupService` da soft delete filtri — §9.3 — *2026-08-24*
- [ ] Seed data'dagi noto'g'ri `UserRole` qiymatlari — `AGENTS.md` §9.4
- [ ] `AttendanceService` dagi `await` qilinmagan tekshiruvlar — §9.6, §9.7

### Prioritet 2 — tugallanmagan modullar
- [ ] `PaymentController` + 5 ta view (`Index` filter bilan, `Create`, `Edit`, `Details`, `Delete`)
- [ ] `PaymentController` uchun qo'shimcha: `GetStudentPayments`, `GetGroupRevenue`
- [ ] `AttendanceController` + view'lar (`Index`, `BulkMark` checkboxlar bilan, hisobot)
- [ ] `AttendanceService` ga bulk insert metodi (bir darsda 40 talaba)
- [ ] Dashboard — `Home/Index` ga real statistika (guruhlar soni, o'quvchilar, oylik daromad)

### Prioritet 3 — xavfsizlik
- [ ] Authentication (login/logout, cookie auth) — `origin/5-implement-permission-based-authentication`
- [ ] Permission jadvallari va munosabatlari — `origin/6-create-table-and-relationships-for-permission-authentication`
- [ ] `[Authorize]` atributlari va rolga asoslangan kirish
- [ ] `HashPassword` ni SHA-256 dan BCrypt/Argon2 ga o'tkazish — §9.5
- [ ] `UserResultDto` dan `PasswordHash` ni olib tashlash — §9.11

### Prioritet 4 — sifat
- [ ] DTO'lardan Domain entity'larni olib tashlash — §9.9
- [ ] Global exception handling middleware (har bir action'dagi `try/catch` o'rniga)
- [ ] EF global query filter: `IsDeleted == false` (har bir `Where` o'rniga —
      buni kiritsak, service'lardagi qo'lda yozilgan filtrlar keraksiz bo'ladi)
- [ ] DTO'larga `[Required]`, `[EmailAddress]`, `[Phone]` validatsiya atributlari
- [ ] Ro'yxatlarga pagination va qidiruv
- [ ] Test project (`tests/EduTrack.Tests`) — service qatlami uchun unit testlar
- [ ] Build warning'larni tozalash (192 → 0)
