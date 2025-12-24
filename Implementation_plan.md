# EduTrack Controller Implementation Ketma-ketlik

## Phase 1: Asosiy Reference Data (Dropdown uchun kerakli)

### 1️⃣ **BranchController** ✅ BIRINCHI
**Sabab:** Group, Room, User barchasi Branch-ga bog'liq

**CRUD:**
- Index - barcha branchlar
- Create - yangi branch
- Edit - tahrirlash
- Delete - o'chirish
- Details - ko'rish

**Logic:** Oddiy CRUD, special logic yo'q

**Views:**
- Index.cshtml (table)
- Create.cshtml (form)
- Edit.cshtml (form)
- Details.cshtml (info)
- Delete.cshtml (confirmation)

---

### 2️⃣ **RoomController** ✅ IKKINCHI
**Sabab:** Group-larni Room-ga assignment qilish kerak

**CRUD:**
- Index - barcha roomlar (Branch filter bilan)
- Create - yangi room (Branch dropdown)
- Edit - tahrirlash
- Delete - o'chirish
- Details - ko'rish

**Special:**
- Branch dropdown orqali room tanlash
- Capacity validation

**Views:** Oddiy CRUD + Branch dropdown

---

### 3️⃣ **UserController** ✅ UCHINCHI
**Sabab:** Group-larni User(Teacher)-ga assignment qilish kerak

**CRUD:**
- Index - barcha teachers
- Create - yangi teacher (Role, Password)
- Edit - tahrirlash
- Delete - o'chirish
- Details - ko'rish

**Special:**
- Password hash qilish
- Role enum (Admin, Teacher, Manager)
- Email validation

**Views:** Create/Edit form bilan password field

---

## Phase 2: Main Business Logic

### 4️⃣ **GroupController** ✅ TO'RTINCHI
**Sabab:** Student, Attendance, Payment barcha Group-ga bog'liq

**CRUD:**
- Index - barcha grouplar (Branch, Room filter)
- Create - yangi group (Branch, Room, User dropdown)
- Edit - tahrirlash
- Delete - o'chirish
- Details - ko'rish
- **GetByBranch** - Branch bo'yicha grouplar

**Special Logic:**
- Branch -> Room dropdown (dependent dropdowns)
- Room capacity validation
- User (Teacher) tanlash
- Monthly fee setting

**Views:**
- Index.cshtml (filter bilan)
- Create.cshtml (3 ta dropdown)
- Edit.cshtml (form)
- Details.cshtml (students ro'yxati)
- Delete.cshtml

---

### 5️⃣ **StudentGroupController** (Enrollment)
**Sabab:** Student-ni Group-ga yoki Group-dan chiqarish

**Operations:**
- EnrollStudent - Student group-ga qo'shish
- RemoveStudent - Group-dan chiqarish
- GetGroupStudents - Group-da qaysi studentlar bor

**No Traditional CRUD** - faqat enrollment operations

**Views:**
- EnrollmentModal (popup orqali)
- StudentsList per group

---

## Phase 3: Financial & Attendance

### 6️⃣ **PaymentController** ⚠️ BESHINCHI
**Sabab:** Complex filtering va reporting

**Operations:**
- Index - barcha to'lovlar (Student, Group, Month filter)
- Create - yangi to'lov qo'shish
- Edit - tahrirlash
- Delete - o'chirish
- GetStudentPayments - Student bo'yicha to'lovlar
- GetGroupRevenue - Group bo'yicha daromad

**Special Logic:**
- Student dropdown (Group orqali filter)
- Month enum
- Payment status calculation (Paid/Pending/Overdue)
- Invoice generation

**Views:**
- Index.cshtml (advanced filter)
- Create.cshtml (Student + Group + Amount)
- Edit.cshtml
- PaymentHistory (Student payment list)

---

### 7️⃣ **AttendanceController** ⚠️ OLTINCHI
**Sabab:** Teacher qo'lda attendance qiladi, complex

**Operations:**
- Index - barcha attendance
- CreateBulk - bir darsda barcha studentlar (Form with checkboxes)
- Edit - einzeln o'zgaritirish
- GetGroupAttendance - Group bo'yicha
- GetStudentAttendance - Student bo'yicha
- AttendanceReport - haftalik/oylik report

**Special Logic:**
- Bulk insert (40 talaba bir vaqtda)
- Attendance percentage calculation
- Date range filtering

**Views:**
- Index.cshtml (Group + Date filter)
- BulkMark.cshtml (Checkboxes bilan)
- StudentAttendanceReport.cshtml (chart)

---

## Implementation Order Summary

```
┌─────────────────────────────────────────┐
│ 1️⃣ BranchController (Asosiy reference)  │
│ 2️⃣ RoomController (Branch -> Room)      │
│ 3️⃣ UserController (Teachers)            │
│ 4️⃣ GroupController (Main entity)        │
│ 5️⃣ StudentGroupController (Enrollment)  │
│ 6️⃣ PaymentController (Financial)        │
│ 7️⃣ AttendanceController (Complex)       │
└─────────────────────────────────────────┘
```

---

## Har bir Controller uchun Views

### Simple CRUD (Branch, Room, User)
```
- Index.cshtml (table)
- Create.cshtml (form)
- Edit.cshtml (form)
- Details.cshtml (info)
- Delete.cshtml (confirmation)
```

### Complex CRUD (Group, Payment, Attendance)
```
- Index.cshtml (filter, table)
- Create.cshtml (multiple dropdowns)
- Edit.cshtml (form)
- Details.cshtml (related data)
- Delete.cshtml (confirmation)
- [Special views] (reports, bulk operations)
```

---

## Key Points

1. **Dropdown Hierarchy:**
   - Branch -> Room (RoomController-da)
   - Branch -> Group (GroupController-da)
   - Group -> StudentGroup (StudentGroupController-da)

2. **Special Views:**
   - Dependent dropdowns (JavaScript orqali)
   - Bulk operations (checkboxes)
   - Date range filters
   - Status badges

3. **Service Layer:**
   - Har bir controller uchun IXxxService interface
   - Business logic service-da, controller-da yo'q

4. **DTOs:**
   - CreationDto
   - UpdateDto
   - ResultDto
   - Qo'shimcha: FilterDto, ReportDto

---

## Shuning uchun avval:

**Boshlaylik:** **BranchController** ✅

Ozod bo'lsangiz, yozib beraman:
- BranchController (CRUD)
- Branch views (5 ta)
- Branch DTOs

OK? 🚀