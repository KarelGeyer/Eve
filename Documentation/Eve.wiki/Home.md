# 🏠 Family App - Dokumentace systému

Vítejte v hlavní technické dokumentaci projektu. Tento portál slouží jako centrální zdroj pravdy pro vývojáře, architekty a testery. Systém je navržen jako modulární monolit podporující hybridní úložiště a real-time synchronizaci.

---

## 📖 Jak číst tuto dokumentaci

Veškerá dokumentace je psána v **Markdownu**. Všeobecně je doporučeno si naklonovat wiki repozitář a dokumentaci studovat ve VSCode. Pro prohlížení a editaci diagramů jsou pužity otevřené formáty, které lze otevřít dvěma způsoby:

### 1. Visual Studio Code (Doporučeno)

Pro plnou integraci přímo v editoru si nainstalujte tato rozšíření:

- **[Draw.io Integration](https://marketplace.visualstudio.com/items?itemName=hediet.vscode-drawio)** – Pro nativní otevírání a editaci souborů `.drawio` a `.dio`.
- **[dbdiagram.io](https://marketplace.visualstudio.com/items?itemName=dbdiagram.dbdiagram-vscode)** – Pro podporu syntaxe databázových schémat.

### 2. Webové aplikace

Pokud nechcete instalovat rozšíření, stačí soubory z dokumentace nahrát do webových verzí nástrojů:

- **[Draw.io (diagrams.net)](https://app.diagrams.net/)** – Stačí přetáhnout `.drawio` soubor do prohlížeče.
- **[dbdiagram.io](https://dbdiagram.io/)** – Pro vizualizaci DB schématu vložením `.dbml` kódu nebo nahráním souboru.

---

## 🗺️ Rozcestník dokumentace

Pro rychlou navigaci použijte níže uvedené sekce:

### 🏗️ Základy a Architektura

- **[Architektura systému](./Architecture)** – High-level pohled na NX monorepo, .NET API Host a datové toky.
- **[Schéma Architektury systému](./Application-architecture.drawio)** – Schéma architektury systému.
- **[Schéma Databáze](./db-schema.dbml)** – ER diagram a popis klíčových tabulek (Groups, Users, Events).
- **[Schéma Flow Aplikace](./Application-Flows.drawio)** – Flow aplikace.

### 🔐 Bezpečnost a Identity

- **[Autentizační modul](./Authentication.md)** – Logika přihlašování, Refresh tokeny a ochrana proti útokům.
- **[Správa uživatelského účtu](./User-Management.md)** – Profil, změna údajů a bezpečné smazání účtu (Delete Flow).
- **[Správa skupinového účtu](./Administrace-a-řízení-skupiny.md)** – Profil, změna údajů a bezpečné smazání účtu (Delete Flow).
- **[Role a Oprávnění](./Roles-Permissions.md)** – Definice rolí (Admin, Member, Guest) a hierarchie práv.

### 📅 Funkční moduly (Features)

- **[Modul Událostí (Events)](./Events-Module.md)** – Správa kalendáře, účastníků a lokací.
- **[Externí služby a Media](./External-Services.md)** (v přípravě) – Integrace na Google Drive, NAS a S3 úložiště.
- **[Finance & Rozpočet](./Finance-Module.md)** (v přípravě)– Sledování společných výdajů.

---

## 🚀 Technický Stack

| Vrstva            | Technologie                           |
| :---------------- | :------------------------------------ |
| **Backend**       | .NET 8 (C#), Entity Framework Core    |
| **Frontend**      | React Native (Expo) + Next.js (Admin) |
| **Monorepo**      | NX Build System                       |
| **Databáze**      | MS SQL Server                         |
| **Komunikace**    | REST API + SignalR (Real-time)        |
| **Zprávy/Fronty** | MassTransit (In-memory / RabbitMQ)    |

---

## 🛠️ Rychlý start pro vývojáře

1. **Lokální prostředí**: Ujistěte se, že máte nainstalované Docker Desktop a .NET SDK.
2. **Databáze**: Spusťte `docker-compose up` pro inicializaci SQL Serveru.
3. **Frontend**: V rootu NX monorepa spusťte `npx nx serve mobile-app`.
4. **API**: Otevřete solution v IDE a spusťte profil `FamilyApp.Api`.

> [!IMPORTANT]
> Každý nový endpoint musí být dokumentován v příslušné sekci a musí dodržovat standardní Response kódy (200 OK, 401 Unauthorized, 403 Forbidden, 409 Conflict).

---

_Poslední aktualizace: 2026-02-02_
