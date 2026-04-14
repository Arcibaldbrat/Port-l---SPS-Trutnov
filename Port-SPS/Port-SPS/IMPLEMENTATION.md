# 🎉 Portál SPS Trutnov - Implementace hotova!

Vítejte! Vytvořil jsem kompletní školní portál s moderním designem a plnou funkčností. Tady je co bylo implementováno:

## ✨ Co je nového

### 🎨 Nový Design
- **Moderní UI** se studeným modrým designem
- **Responzivní layout** - funguje na mobilu, tabletu i počítači
- **Intuitivní navigace** s rychlými ikonami
- **Hezké karty a komponenty** s animacemi

### 📄 Nové Stránky (všechny s autentizací)
1. **📊 Dashboard** - Přehledová stránka s hlavními statistikami
2. **🗓️ Rozvrh** - Tabulka s rozvrhem hodin
3. **📈 Známky** - Přehled známek podle předmětů
4. **💬 Zprávy** - Komunikace mezi uživateli
5. **👥 Třídy** - Seznam tříd a skupin
6. **📢 Oznámení** - Školní oznámení a novinky
7. **👨‍🏫 Učitelé** - Katalog pedagogů se kontakty
8. **📚 Materiály** - Studijní materiály ke stažení

### 🎯 Funkcionality

✅ **Autentizace**
- Login/Registrace
- Validace školního emailu (@spstrutnov.cz)
- Bezpečné heslo (minimálně 8 znaků)
- Ochrana proti brute-force

✅ **Role a Přístup**
- Veřejnost - bez přihlášení vidí úvodní stránku
- Studenti - plný přístup po registraci
- Učitelé - čekají na schválení admina
- Admin - správa všeho

✅ **Profil**
- Zobrazení osobních údajů
- Změna hesla
- Bezpečnostní nastavení

✅ **CSS Styling**
- Úplně nové `site.css` s moderním designem
- Dark header s gradientem
- Karty s hover efekty
- Responzivní grid layouty
- Scrollbar styling
- Animace

✅ **Navigace**
- Sticky header s logem školy
- Navigační menu s ikonami
- Footer se školními informacemi
- Dynamické zobrazení menu pouze pro přihlášené uživatele

## 📁 Struktura Projektu

```
Port-SPS/
├── Pages/
│   ├── Dashboard.cshtml(.cs)           ← NOVÁ - Přehled
│   ├── Schedule.cshtml(.cs)            ← NOVÁ - Rozvrh
│   ├── Grades.cshtml(.cs)              ← NOVÁ - Známky (přesunuto)
│   ├── Messages.cshtml(.cs)            ← NOVÁ - Zprávy
│   ├── Classes.cshtml(.cs)             ← NOVÁ - Třídy (přesunuto)
│   ├── Announcements.cshtml(.cs)       ← NOVÁ - Oznámení
│   ├── Teachers.cshtml(.cs)            ← NOVÁ - Učitelé
│   ├── Materials.cshtml(.cs)           ← NOVÁ - Materiály
│   ├── Auth/                           - Autentizace
│   ├── Profile/                        - Uživatelský profil
│   ├── Student/
│   │   └── Grades.cshtml(.cs)          ← UPRAVENO
│   ├── Teacher/
│   │   └── Classes.cshtml(.cs)         ← UPRAVENO
│   ├── Shared/
│   │   └── _Layout.cshtml              ← UPRAVENO - Nový header/footer
│   └── _Imports.cshtml
├── wwwroot/css/
│   └── site.css                        ← KOMPLETNĚ PŘEPSÁNO!
└── ... (ostatní soubory)
```

## 🚀 Jak Spustit

### 1. Inicializace Databáze
```bash
# Windows
init-db.bat

# Nebo manuálně
dotnet ef database update
```

### 2. Spuštění aplikace
```bash
dotnet run
```

Aplikace bude dostupná na: `https://localhost:7xxx`

## 🎮 Testování

### Zaregistrovat si účet:
1. Přejděte na "Registrace"
2. Vložte email končící na `@spstrutnov.cz` (např. `student@spstrutnov.cz`)
3. Vyberte roli (Student = auto-schválení, Učitel = čekání na schválení)
4. Vytvořte heslo (min. 8 znaků)

### Přihlášení:
1. Klikněte na "Přihlášení"
2. Vložte email a heslo

## 🎨 CSS Vlastnosti

Všechny barvy jsou definovány v CSS proměnných:
```css
--primary-color: #2563eb;          /* Modrá */
--secondary-color: #10b981;        /* Zelená */
--accent-color: #f59e0b;           /* Oranžová */
--danger-color: #ef4444;           /* Červená */
```

## 📱 Responzivní Design

Breakpointy:
- **Desktop**: 1400px max-width
- **Tablet**: max-width: 768px
- **Mobile**: max-width: 480px

## 🔐 Bezpečnost

✅ HTTPS (vyžadováno v produkci)
✅ Password hashing (ASP.NET Identity)
✅ CSRF Protection
✅ SQL Injection Prevention (EF Core)
✅ Account Lockout (5 pokusů)
✅ Authorization attributes

## 📝 Poznámky

1. **Heslo je povinné**: Obsahovat minimalně 1 speciální znak a být dlouhé alespoň 8 znaků
2. **Školní email**: Musí končit na `@spstrutnov.cz`
3. **Učitelé**: Musí být schváleni adminem dříve, než se mohou přihlásit
4. **Responsive**: Všechny stránky se automaticky přizpůsobují mobilu

## 🎯 Budoucí Rozšíření

- [ ] Admin panel pro správu uživatelů
- [ ] Email notifikace
- [ ] Real-time messaging
- [ ] Galerie fotografií
- [ ] Kalendář akcí
- [ ] Integrační API

## 🛠️ Tech Stack

- .NET 10.0
- ASP.NET Core Razor Pages
- Entity Framework Core
- SQL Server (LocalDB)
- Bootstrap 5 (utilities)
- Custom CSS (moderní design)

---

**Status**: ✅ Hotovo a připraveno k nasazení!
**Počet nových stránek**: 8 (+ upravené Grades, Classes, Layout, CSS)
**Linek CSS**: ~800 lines moderního stylu

Pokud byste potřeboval jakékoliv úpravy nebo další funkcionality, jste vítán! 🚀
