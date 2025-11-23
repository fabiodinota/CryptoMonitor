# 📈 CryptoMonitor

![.NET](https://img.shields.io/badge/.NET-9.0-512bd4?style=flat&logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-MVC-512bd4?style=flat&logo=dotnet)
![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-blue?style=flat)
![SQLite](https://img.shields.io/badge/Database-SQLite-003B57?style=flat&logo=sqlite)
![License](https://img.shields.io/badge/License-MIT-green.svg)

**CryptoMonitor** is a robust tracking platform for cryptocurrencies and exchanges. Built with **.NET 9.0**, it demonstrates a clean **N-Layer Architecture** and advanced **Entity Framework Core** patterns, managing complex Many-to-Many relationships between digital assets and trading platforms.

---

## 📷 Demo

> *Home Page (Index): Overview of all Cryptocurrencies.*
<img width="3141" height="1398" alt="localhost_7181_Crypto" src="https://github.com/user-attachments/assets/500c3889-0840-4641-a078-88e3c538e0c4" />

> *Exchanges Index: Overview of all tracked exchanges.*
<img width="3141" height="1398" alt="localhost_7181_Exchange" src="https://github.com/user-attachments/assets/399a495d-d10a-454a-8381-cbdcd521a321" />

> *Exchange Listings Dashboard: Feed of recently added market pairs (Crypto to Exchange).*
<img width="3141" height="1398" alt="localhost_7181_ExchangeListing" src="https://github.com/user-attachments/assets/2c8e4133-6ca4-4b62-acee-e8ec26d2c3ab" />

> *Cryptocurrency Details: Showing scalar properties and related Exchanges via the Listings table.*
<img width="3141" height="1398" alt="localhost_7181_Crypto_Details_1" src="https://github.com/user-attachments/assets/df0fddcb-2e1b-4841-9cc6-8d7525a2547b" />

> *Exchange Details: Showing scalar properties, trust score, and related User Reviews.*
<img width="3141" height="1398" alt="localhost_7181_Exchange_Details_1" src="https://github.com/user-attachments/assets/8169812b-51e1-4a7d-b851-1d6c36b20313" />

> *Add Cryptocurrency Form: Multi-step form allowing selection of Max Supply and Exchanges via a multi-select list.*
<img width="3141" height="1398" alt="localhost_7181_Crypto_Add" src="https://github.com/user-attachments/assets/f565cadb-b8f4-4cb8-82cc-4d1b2f677516" />

---

## ✨ Key Features

* **Asset Management**: Create and track Cryptocurrencies (Coins, Tokens, Stablecoins, MemeCoins).
* **Exchange Tracking**: Manage Exchange details including Trust Scores and Website links.
* **Market Listings (Many-to-Many)**:
    * Link Cryptocurrencies to Exchanges.
    * Track metadata for listings (e.g., *Listing Date*).
    * View recent market additions via the "New Listings" dashboard.
* **User Reviews**: One-to-Many relationship allowing users to rate and review Exchanges.
* **Advanced Filtering**: Filter Exchanges by Trust Score and Name simultaneously using LINQ dynamic queries.
* **Dual Interface**: Includes both a web-based **ASP.NET MVC** interface and a **Console Application** for administrative tasks.

---

## 🏗️ Architecture

The solution follows a strict **N-Layer Architecture** to ensure separation of concerns (SoC) and maintainability:

| Layer | Project | Responsibility |
| :--- | :--- | :--- |
| **Presentation** | `UI.MVC` | ASP.NET Core MVC web interface using Bootstrap. |
| **Presentation** | `UI.CA` | Console Application for quick data testing and admin tasks. |
| **Business Logic** | `BL` | Contains `CryptoManager`. Handles validation logic, filtering rules, and acts as the gatekeeper. |
| **Data Access** | `DAL` | Contains `Repository` and `DbContext`. Manages EF Core queries, Eager Loading (`.Include`), and database transactions. |
| **Domain** | `Domain` | Contains POCO entities (`Cryptocurrency`, `Exchange`) and Enums. |

### Data Model (UML)

The application implements a **Many-to-Many relationship with an intermediate entity** (Scenario 2) to track listing dates.

```mermaid
classDiagram
    class Cryptocurrency {
        +int Id
        +string Name
        +string Symbol
        +double CurrentPrice
        +CryptoType Type
        +List~ExchangeListing~ Listings
    }

    class Exchange {
        +int Id
        +string Name
        +int TrustScore
        +List~ExchangeListing~ Listings
        +List~UserReview~ Reviews
    }

    class ExchangeListing {
        +int CryptocurrencyId
        +int ExchangeId
        +DateTime ListingDate
    }

    class UserReview {
        +int Id
        +string UserName
        +double Rating
        +int ExchangeId
    }

    Cryptocurrency "1" -- "0..*" ExchangeListing : Is Listed Via
    Exchange "1" -- "0..*" ExchangeListing : Offers
    Exchange "1" -- "0..*" UserReview : Receives
```

## 🚀 Getting Started

### Prerequisites
* [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* IDE: JetBrains Rider, Visual Studio 2022, or VS Code.

### Installation

1.  **Clone the repository**
    ```bash
    git clone [https://github.com/YourUsername/CryptoMonitor.git](https://github.com/YourUsername/CryptoMonitor.git)
    cd CryptoMonitor
    ```

2.  **Restore dependencies**
    ```bash
    dotnet restore
    ```

### Run the Application
The application uses **SQLite**, so no external database server setup is required. The database will be automatically created and seeded with sample data upon first run.

Navigate to the MVC project and run:
```bash
cd CryptoMonitor.UI.MVC
dotnet run
```

### Access the App
Open your browser and navigate to `http://localhost:5000` (or the port shown in your terminal).

---

## 💻 Tech Stack Details

* **Framework**: .NET 9.0
* **ORM**: Entity Framework Core (SQLite Provider)
* **Loading Strategy**: Eager Loading (`.Include()`, `.ThenInclude()`) & Lazy Loading Proxies.
* **Frontend**: Razor Views (.cshtml), Bootstrap 5, jQuery Validation.
* **Patterns**: Repository Pattern, Dependency Injection (DI).

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.
