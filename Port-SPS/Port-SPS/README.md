# Portál SPS Trutnov

Moderní webový portál pro Střední průmyslovou školu Trutnov s podporou autentizace přes školní emaily.

## Funkce

### 🔐 Autentizace a Autorizace
- Registrace s validací školního emailu (@spstrutnov.cz)
- Přihlášení pro studenty a učitele
- Bezpečné heslo (minimálně 8 znaků, speciální znaky)
- Ochrana proti brute-force útokům (lockout po 5 neúspěšných pokusech)
- Role-based access control (RBAC)

### 👥 Role a Přístup
1. **Veřejnost** - Přístup bez přihlášení (budoucí veřejné informace)
2. **Studenti** - Auto-schválení po registraci
3. **Učitelé** - Čekání na schválení správce
4. **Admin** - (Připraveno pro budoucí implementaci)

### 📚 Funkcionality
- Profil uživatele s informacemi
- Změna hesla
- Oddělené sekce pro studenty (Mé známky) a učitele (Mé třídy)
- Bezpečný logout

## Technologické Stack

- **.NET 10.0**
- **ASP.NET Core Razor Pages**
- **Entity Framework Core**
- **SQL Server (LocalDB)**
- **Microsoft Identity**
- **Bootstrap 5** - UI

## Instalace a Spuštění

### Předpoklady
- .NET 10 SDK
- SQL Server (nebo LocalDB)
- Visual Studio 2022 nebo jiný editor s .NET podporou

### Kroky instalace

1. **Klonování repozitáře**
   ```bash
   git clone https://github.com/Arcibaldbrat/Port-l---SPS-Trutnov
   cd Port-SPS
   ```

2. **Obnovení balíčků**
   ```bash
   dotnet restore
   ```

3. **Vytvoření databáze**
   ```bash
   dotnet ef database update
   ```
   
   Pokud Entity Framework CLI není nainstalován:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

4. **Spuštění aplikace**
   ```bash
   dotnet run
   ```
   
   Aplikace bude dostupná na: `https://localhost:7xxx`

## Konfigurace

### Connection String
Vložte do `appsettings.json` (pro produkci):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=PortSPSTrutnov;Trusted_Connection=true;"
  }
}
```

### Email Domena
Aktuálně je nastavena na `@spstrutnov.cz`. Lze upravit v `Services/SchoolEmailValidator.cs`

## Struktura Projektu

```
Port-SPS/
├── Pages/                    # Razor Pages
│   ├── Auth/                # Autentizace
│   │   ├── Register.cshtml
│   │   ├── Login.cshtml
│   │   └── ...
│   ├── Profile/             # Profil uživatele
│   ├── Student/             # Sekce pro studenty
│   ├── Teacher/             # Sekce pro učitele
│   └── Shared/              # Layouty a komponenty
├── Models/                  # Data modely
│   └── ApplicationUser.cs  # Model uživatele
├── Data/                    # Databáze
│   └── ApplicationDbContext.cs
├── Services/                # Obchodní logika
│   └── SchoolEmailValidator.cs
└── Program.cs              # Konfigurace aplikace
```

## Bezpečnost

- ✅ Validace emailů pro doménu školy
- ✅ Hashing hesел (Identity)
- ✅ CSRF Protection
- ✅ SQL Injection Prevention (EF Core)
- ✅ Account Lockout
- ✅ Authorization attributes
- ⚠️ HTTPS vyžadováno v produkci

## Budoucí Rozšíření

- [ ] Email potvrzení registrace
- [ ] Správa tříd a studentů (AdminPanel)
- [ ] Známkování
- [ ] Rozvrh hodin
- [ ] Notifikace
- [ ] Galerie/Novinky
- [ ] API pro mobilní aplikaci
- [ ] Two-Factor Authentication (2FA)

## Přispívání

Chyby hlašte přes GitHub Issues. Pull requestů vítáni!

## Licence

(Určete licenci)

## Kontakt

- School: SPS Trutnov (https://www.spstrutnov.cz)
- Repository: https://github.com/Arcibaldbrat/Port-l---SPS-Trutnov

---

**Status:** 🚀 Vývojová verze - Aktivní vývoj
