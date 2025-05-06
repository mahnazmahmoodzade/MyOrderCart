# 🛒 MyOrderCart

A Blazor Server application for browsing products, managing a shopping cart, and confirming orders via an external API.

---

## 📐 Architecture

This project follows a **Clean Architecture** approach with the following layers:

- **Domain**: Core business models (e.g., `Cart`, `CartItem`, `Product`)
- **Application**: Service interfaces and logic (`OrderService`, `IExternalOrderSender`)
- **Infrastructure**: EF Core persistence (`EfOrderRepository`), HTTP communication
- **Blazor UI**: Presentation layer using Blazor Server

---

## 🧰 Technologies Used

- **.NET 8** (Blazor Server)
- **Entity Framework Core** (In-Memory database for order storage)
- **IOptions pattern** for configuration (API endpoints)
- **xUnit + bUnit** for unit and component testing
- **Docker** for Linux-based containerization
- **Moq** for mocking interfaces in unit tests

---

## 🔌 External Integration

Orders are sent to an external system using an HTTP API. The endpoint is configurable via `appsettings.json` and injected using `IOptions<OrderOptions>`.

---

## ✅ Features Implemented

- Browse product list (loaded from `fakestoreapi.com`)
- Add/remove/increment/decrement items in cart
- Live cart count via shared `CartService`
- Confirm order → freeze cart → send to external API
- Persist confirmed orders with EF Core (in-memory)
- Configurable API endpoints (Order + Product)
- Unit tests for domain and application logic
- UI component tests using bUnit
- Dockerized build and run support

---

## 🚧 Not Yet Implemented (Planned)

- Advanced styling (e.g., using MudBlazor)
- End-to-end UI testing with Selenium
- Richer error handling and logging

---

## ▶️ Running the App

1. Clone the repo
2. Run from Visual Studio or with:

```bash
dotnet run --project src/MyOrderCart.BlazorUI
