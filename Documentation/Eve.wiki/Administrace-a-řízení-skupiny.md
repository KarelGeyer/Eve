# 👥 Správa skupiny: Administrace a řízení skupiny

## Admin část

<img width="931" height="550" alt="image" src="https://github.com/user-attachments/assets/4fb4ad55-b93c-4f8b-805f-2260ee7312e4" />

## Členská část

<img width="364" height="452" alt="image" src="https://github.com/user-attachments/assets/655ec73e-8f3e-4776-bfe7-36810973268a" />

## 1. Přehled

Tento modul řeší správu členů konkrétní skupiny. Je v podstatě extrémně závislý na Adminech konkrétní skupiny, jelikož běžní členové nemají oprávnění se skupinou nějakým způsobem nakládat. To z bezpečnostních důvodů, aby nedošlo k nechtěným změnám v rolích, členech či modulech a podobně. Výjimka je pouze v žádosti o roli, kterou může zaslat jakýkoliv člen sklupiny, nicméně schválení je opět na Adminovi skupiny.

## 2. Přehled členů skupiny

Všichni členové mají možnost vidět všechny členy, kteří jsou aktuálně napojeni na skupinu a jejich detail. V případě, že jeden z členských účtů není aktivní nebo je blokovaný, jeho detail o tom zobrazí informaci. Manipulace s oprávněním, rolí a členstvím ve skupině je stále možná.

## 3. Žádost o změnu role/oprávnění

[ADMIN]
Admin má v administraci možnost vidět žádosti o role či oprávnění a akceptovat je, případně je zamítat. Více o tomto tématu níže ve [Správě Rolí a Oprávnění](#5-správa-rolí-a-oprávnění-admin-a-owner) Po vytvoření role by Admin měl být notifikován o nové žádosti. Při přijetí žádosti se flow řídí stejným mechanismem, jako když roli vybírá admin sám. Při zamítnutí se požadavek s databáze smaže.

[MEMBER]
Člen skupiny má právo si zažádat o novou roli v UI. Žádost může mít pouze jednu.

### Kroky k vytvoření žádosti o přidělení role:

1. **Požadavek (Request)**: Člen si vybere jednu z rolí nebo oprávnění z možností v UI a odešle `RequestRoleRequest` případně `RequestPermissionRequest`.
2. **Validace**:
   - **Kontrola aktivní role/oprávnění**: Kontrola, zda uživatel již danou roli/oprávnění nemá.
   - **Kontrola členů**: Kontrola, zda uživatel je opravdu součástí skupiny a zda je aktivní.
   - **Kontrola existující žádosti**: Systém ověří, zda v databázi již neexistuje jiná nevyřízená žádost od stejného uživatele (prevence spamování).
3. **Reakce (Response)**:
   - **202 Accepted**: Žádost byla validní a byla úspěšně uložena. **Systém vygeneruje notifikaci pro administrátory skupiny.**
   - **400 Bad Request**: Neplatné ID role/oprávnění nebo chybný formát dat.
   - **403 Forbidden**: Uživatel není členem skupiny.
   - **409 Conflict**: Uživatel již tuto roli má nebo už jedna identická žádost čeká na vyřízení.

## 4. Přidávání nových členů, Generování `Invitation Code` a Správa členů (ADMIN)

Přidávání nových členů a generování `Invitation Code` je popsané na stráce [Registrace skupiny](Registrace-a-Login-Skupiny#žádost-o-připojení-do-skupiny).

## 5. Správa Rolí a Oprávnění [Admin]

### 5.1 Běžná změna role

Tento postup se používá pro změny rolí, které nevyžadují speciální dohled (všechny role kromě Admin a Owner).

#### Kroky ke změně role (Běžná role):

1. **Požadavek (Request)**: Admin v UI na kartě uživatele vybere roli a odešle `ChangeMemberRoleRequest`.
2. **Validace**:
   - **Kontrola oprávnění**: Odesílatel musí mít roli Admin nebo Owner.
   - **Kontrola aktivní role**: Cílový uživatel nesmí danou roli již mít.
   - **Kontrola členů**: Cílový uživatel musí být aktivní součástí skupiny.
   - **Owner rights**: Kontrola, zda uživatel není Owner, tomu není možné odebrat ani udělit Roli.
3. **Reakce (Response)**:
   - **200 OK**: Role byla změněna.
   - **400 Bad Request**: Neexistující Role / ID.
   - **403 Forbidden**: Odesílatel nemá dostatečná oprávnění ke správě členů.
   - **409 Conflict**: Uživatel již danou roli má.

---

### 5.2 Povýšení na Admina a odebrání Admina

Speciální flow. Povýšit člena na Admina může pouze **Owner** skupiny nebo Člen, který k tomu má explicitně přidělené oprávnění (`ManageAdmins`). To samé platí pro odebrání role Admin

#### Kroky ke změně role (Admin role):

1. **Požadavek (Request)**: Oprávněný uživatel v UI zvolí "Povýšit na administrátora" a odešle `AssignNewGroupAdminRequest`. Případně zvolí admina a odešle `RemoveGroupAdminRequest`
2. **Validace**:
   - **Kontrola oprávnění**: Odesílatel musí být buď **Owner**, nebo mít oprávnění pro správu adminů.
   - **Kontrola cíle**: Cílový uživatel nesmí být již Adminem / tento bod není platný v případě odebírání Admin role.
   - **Kontrola členů**: Cílový uživatel musí být aktivní člen skupiny.
   - **Owner rights**: Kontrola, zda uživatel není Owner, tomu není možné odebrat ani udělit Roli.
3. **Reakce (Response)**:
   - **200 OK**: Role byla změněna na Admin / Admin role byla odebrána.
   - **403 Forbidden**: Odesílatel není Owner nebo nemá speciální oprávnění k povyšování na Adminy čo odebírání Admin role.
   - **409 Conflict**: Cílový uživatel již roli Admin má / tento bod není platný v případě odebírání Admin role..

---

### 5.3 Přiřazení Běžných Oprávnění

Používá se pro přidání extra oprávnění nad rámec role (např. `CreateEvent` pro Hosta).

#### Kroky k přidání oprávnění (Běžné oprávnění):

1. **Požadavek (Request)**: Admin/Owner odešle `AddPermissionRequest`.
2. **Validace**:
   - **Kontrola oprávnění**: Odesílatel musí mít roli Admin nebo Owner.
   - **Kontrola existence**: Oprávnění musí existovat v tabulce `Permission`.
   - **Owner rights**: Kontrola, zda uživatel není Owner, tomu není možné odebrat ani udělit žádná oprávnění.
3. **Reakce (Response)**:
   - **200 OK**: Oprávnění bylo přidáno do `UserExtraPermissions`.
   - **204 OK**: Uživatel toto Oprávnění již má`.
   - **404 Not Found**: Neexistující Oprávnění.
   - **409 Conflict**: Uživatel již dané oprávnění má explicitně přiřazené.

---

### 5.4 Odebrání Běžných Oprávnění

1. **Požadavek (Request)**: Admin/Owner odešle `RemovePermissionRequest`.
2. **Validace**:
   - **Kontrola oprávnění**: Admin nebo Owner.
   - **Owner rights**: Kontrola, zda uživatel není Owner, tomu není možné odebrat ani udělit žádná oprávnění.
3. **Reakce (Response)**:
   - **200 OK**: Oprávnění bylo odebráno z `UserExtraPermissions`.
   - **204 NO Content**: Uživatel toto oprávnění již nemá.

---

### 5.5 Přiřazení a Odebrání Speciálních Oprávnění

Obdoba 5.3 a 5.4 s dodatečnou kontrolou:

- **Logika**: Pokud `Permission.Code` patří mezi kritické (např. `ManageAdmins`, `ManageSpecialPermissions`), systém vyžaduje, aby odesílatel byl **Owner** nebo měl právě speciální oprávnění `ManageSpecialPermissions`. Admin bez specifického `IsOwner = true` tato oprávnění přidělovat nemůže.

## 6. Mazání členů skupiny a Opuštění skupiny

### 6.1. Odebrání uživatelů ze skupiny

**Kroky k odebrání:**

1. **Požadavek (Request)**: Admin/Owner v UI vybere uživatele, smaže jej a potvrdí smazání a tím odešle `RemoveUserFromGroupRequest`.
2. **Validace**:
   - **Kontrola oprávnění**: Odesílatel musí mít **Admin** nebo speciální oprávnění. Adminy může odebrat jedině Owner.
   - **Owner protection**: Cílový uživatel nesmí být **Owner**. Ownera nelze ze skupiny odebrat.
   - **Self-kick check**: Uživatel nemůže odebrat sám sebe (na to slouží sekce 6.2).
3. **Reakce (Response)**:
   - **200 OK**: Uživatel byl odstraněn. Systém kaskádovitě smazal jeho vazby v `UserGroups`, `UserRoles` a `UserExtraPermissions`.
   - **403 Forbidden**: Pokus o odebrání Ownera nebo nedostatečná práva odesílatele.
   - **404 Not Found**: Cílový uživatel není členem této skupiny.
   - **409 Conflict**: Pokus o odebrání sebe sama skrze tento endpoint.

### 6.2. Opuštění skupiny

**Kroky k opuštění:**

1. **Požadavek (Request)**: Uživatel v nastavení skupiny odešle `LeaveGroupRequest`.
2. **Validace**:
   - **Owner Lock**: Pokud je uživatel **Owner**, systém mu nedovolí odejít, dokud nepřevede vlastnictví na jiného člena nebo skupinu nesmaže.
3. **Reakce (Response)**:
   - **200 OK**: Uživatel úspěšně opustil skupinu.
   - **400 Bad Request**: Uživatel je Owner a musí nejprve předat práva nebo smazat skupinu.
   - **404 Not Found**: Uživatel není členem skupiny.

## 7. Předání práv Ownera

Owner má právo předat vlastnictví skupiny a tím i veškerá práva a plnou moc na správou celé skupiny a všech jejích členů jakémukoliv členovi skupiny, kterému je více než 18 let. Pokud ve skupině owner není sám a tudíž je zde více členů, systém mu neumožní skupinu opustit, dokud svá práva takto nepředá. Může ovšem skupinu smazat.

**Kroky k předání vlastnických práv:**

1. **Požadavek (Request)**: Owner v nastavení skupiny vybere uživatele, zvolí "Předat vlastnictví skupiny", potvrdí volbu a odešle `ChangeOwnershipRequest`.
2. **Validace**:
   - **Kontrola Role**: Odesílatel musí mít roli **Owner**. Nikdo jiný nemůže vlastnictví převést.
   - **Kontrola cíle**: Cílový uživatel musí být aktivním členem skupiny.
   - **Kontrola identity**: Cílový uživatel nesmí být ten stejný uživatel, který požadavek odesílá.
3. **Reakce (Response)**:
   - **200 OK**: Vlastnictví bylo úspěšně převedeno. Původnímu Ownerovi je nastavena role Admin, novému uživateli je nastaven příznak `IsOwner = true` a Role Admin, pokud ji ještě nemá.
   - **403 Forbidden**: Odesílatel není Owner skupiny.
   - **404 Not Found**: Cílový uživatel ve skupině neexistuje.
   - **422 Unprocessable Entity**: Cílový uživatel má neaktivní nebo blokovaný účet (nelze na něj převést odpovědnost).

## 8. Mazání skupiny

Owner má právo skupinu celou kompletně smazat, po smazání bude náplánované odstranění vazeb v databázi za 1 měsíc a uživatelé budou notifikováni. Po dobu jednoho měsíce mohou uživatelé aplikace nadále plně využívat. Owner skupiny má právo toto rozhodnutí v průběhu měsíce zvrátit, poté bude skupina nenávratně smazána.

**Kroky ke smazání skupiny**

1. **Požadavek (Request)**: Owner v nastavení skupiny vybere "Smazat skupinu", potvrdí volbu (zadáním názvu skupiny) a odešle `DeleteGroupRequest`.
2. **Validace**:
   - **Kontrola Role**: Odesílatel musí být **Owner**. Admin ani nikdo jiný tuto akci nesmí provést.
   - **Kontrola předplatného**: Pokud jsou aktivní placené moduly, systém je automaticky ukončí k datu smazání.
3. **Reakce (Response)**:
   - **200 OK**: Skupina byla označena ke smazání `IsMarkedToBeDeleted` a je náplánována ke smazání za 30 dní od odeslání požadavku na smazání.
   - **403 Forbidden**: Odesílatel není Owner.
   - **404 Not Found**: Skupina neexistuje.
   - **409 Conflict**: Skupinu nelze smazat, dokud nejsou vypořádány kritické závazky (např. běžící export dat nebo nevyřízené platby).

\*\* Kroky ke stornu smazání skupiny:

1. **Požadavek (Request)**: Owner v nastavení skupiny vybere "Stornovat smazání skupiny", potvrdí volbu a odešle `CancelDeleteGroupRequest`.
2. **Validace**:
   _ **Kontrola Role**: Odesílatel musí mít roli **Owner**.
   _ **Kontrola stavu**: Skupina musí být označena ke smazání. Pokud již proběhlo samotné smazání, akci nelze pr
   vést.
3. **Reakce (Response)**:
   - **200 OK**: Plánované smazání bylo zrušeno. Skupina a všechny přidružené moduly jsou opět plně aktivní.
   - **403 Forbidden**: Odesílatel není Owner.
   - **404 Not Found**: Skupina neexistuje.
   - **409 Conflict**: Skupina není ve stavu určeném ke smazání, nebo proces mazání již nelze zastavit.

## 9. Externí služby a úložiště

Tato sekce bude doplněna později

## 10. Moduly

### 9.1 Free Moduly

Tato sekce bude doplněna později

### 9.2 Placené Moduly

Tato sekce bude doplněna později

## 10 🛠️ Technická dokumentace: Správa skupiny (API & Business Logic)

### 10.1. API Endpoints

| Metoda     | Endpoint                                                    | Popis                                       |
| :--------- | :---------------------------------------------------------- | :------------------------------------------ |
| **GET**    | `/api/groups/{groupId}/members`                             | Získává všechny členy skupiny               |
| **GET**    | `/api/groups/{groupId}/members/{userId}`                    | Získává konkrétního člena skupiny           |
| **POST**   | `/api/groups/{groupId}/requests`                            | Člen zašle žádost o roli nebo oprávnění.    |
| **POST**   | `/api/groups/{groupId}/requests/{id}/approve`               | Schválení žádosti (Admin/Owner).            |
| **DELETE** | `/api/groups/{groupId}/requests/{id}`                       | Zamítnutí/Smazání žádosti o roli.           |
| **PATCH**  | `/api/groups/{groupId}/members/{userId}/role`               | Přímá změna role uživatele.                 |
| **POST**   | `/api/groups/{groupId}/members/{userId}/permissions`        | Přidělení extra oprávnění uživateli.        |
| **DELETE** | `/api/groups/{groupId}/members/{userId}/permissions/{code}` | Odebrání extra oprávnění uživateli.         |
| **DELETE** | `/api/groups/{groupId}/members/{userId}`                    | Odebrání uživatele ze skupiny (Kick).       |
| **POST**   | `/api/groups/{groupId}/leave`                               | Dobrovolné opuštění skupiny uživatelem.     |
| **PATCH**  | `/api/groups/{groupId}/transfer-ownership`                  | Předání vlastnických práv (Owner Transfer). |
| **DELETE** | `/api/groups/{groupId}`                                     | Zahájení 30denní lhůty pro smazání skupiny. |
| **POST**   | `/api/groups/{groupId}/restore`                             | Storno plánovaného smazání skupiny.         |

### 10.2. Request Models

- **RequestRoleRequest / RequestPermissionRequest**:

```json
{
  "roleId": "int",
  "permissionCode": "string",
  "reason": "string"
}
```

- **ApproveRoleRequest**:

```json
{
  "requestId": "int",
  "note": "string"
}
```

- **ChangeMemberRoleRequest**:

```json
{
  "newRoleId": "int"
}
```

- **AssignNewGroupAdminRequest / RemoveGroupAdminRequest**:

```json
{
  "targetUserId": "int"
}
```

- **AddPermissionRequest / RemovePermissionRequest**:

```json
{
  "permissionCode": "string"
}
```

- **RemoveUserFromGroupRequest**:

```json
{
  "targetUserId": "int",
  "reason": "string"
}
```

- **LeaveGroupRequest**:

```json
{
  "groupId": "int"
}
```

- **ChangeOwnershipRequest**:

```json
{
  "targetUserId": "int"
}
```

- **DeleteGroupRequest**:

```json
{
  "confirmName": "string"
}
```

- **CancelDeleteGroupRequest**:

```json
{
  "groupId": "int"
}
```

### 10.3. Business Rules (Kritická logika)

- **Ochrana Ownera**: Jakákoliv operace směřující k modifikaci nebo odebrání uživatele s příznakem `IsOwner = true` je systémem blokována (vyjma vlastního předání práv).
- **Hierarchie**: Admin nemůže povýšit člena na Admina ani odebrat jiného Admina bez specifického oprávnění `ManageAdmins` nebo bez příznaku `IsOwner = true`.
- **Věk pro vlastnictví**: Předání vlastnictví je validováno proti věku cílového uživatele (minimálně 18 let na základě dat identity).
- **Atomický převod**: Při změně Ownera dojde v rámci jedné databázové transakce k povýšení cíle na Ownera a degradaci původního majitele na roli Admin.

---

## 10.4. Lifecycle & Deletion Policy

Správa životního cyklu skupiny zajišťuje ochranu dat před nechtěným nebo impulzivním smazáním.

### 10.4.1 Ochranná lhůta (Soft Delete)

- Po odeslání požadavku na smazání je nastavena lhůta **30 dní**.
- Během této doby je skupina v databázi označena příznakem `IsMarkedToBeDeleted`.
- Všechny moduly (Finance, Události) přecházejí do režimu omezeného zápisu s varovnou notifikací.

### 10.4.2 Notifikační schéma

- **T = 0**: Okamžitá notifikace všem členům o naplánování smazání.
- **T - 7 dní**: Druhé varování o blížícím se odstranění dat.
- **T - 24 hodin**: Finální upozornění před nevratným smazáním.

### 10.4.3 Obnova a Hard Delete

- **Restore**: Owner může kdykoliv během 30denní lhůty akci stornovat přes `/restore` endpoint, čímž obnoví plnou funkcionalitu.
- **Hard Delete**: Po vypršení 30 dní dojde k trvalému odstranění všech záznamů ze všech modulů a odpojení externích úložišť bez možnosti obnovy.
