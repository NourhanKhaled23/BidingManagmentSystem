
# 🏗️ Bidding Management System

## ✨ Features
- JWT Authentication with roles: `Bidder`, `ProcurementOfficer`, `Evaluator`
- User Registration/Login ✅
- Tender Management (CRUD + status transitions) ✅
- Bid Submission with file upload ✅
- Tender Evaluation logic (auto winner detection) ✅
- Password hashing with BCrypt ✅
- File Storage (Tender docs, Bid docs) in wwwroot ✅
- Clean architecture (DDD-inspired): Domain, Application, Infrastructure, API ✅

## 📂 Folder Structure
```
├── API
│   ├── Controllers
│   ├── DTOs
├── Application
│   ├── Services
│   ├── Interfaces
├── Domain
│   ├── Entities
│   ├── ValueObjects
├── Infrastructure
│   ├── Data
│   ├── Repositories
├── wwwroot
│   ├── uploads
│   ├── bids
```

## Bid Module

### Submit a Bid
- **Method**: POST
- **Endpoint**: `/api/Bid/submit`
- **Description**: Allows a user to submit a bid for a tender.

### Get Bids for a Tender
- **Method**: GET
- **Endpoint**: `/api/Bid/tender/{tenderId}`
- **Description**: Retrieves all bids associated with a specific tender ID.

## Evaluation Module

### Evaluate a Tender
- **Method**: POST
- **Endpoint**: `/api/Evaluation/{tenderId}/evaluate`
- **Description**: Submits an evaluation for a specific tender.

## ForgotPassword Module

### Request Password Reset
- **Method**: POST
- **Endpoint**: `/api/ForgotPassword/forgot-password`
- **Description**: Initiates a password reset request for a user.

### Reset Password
- **Method**: POST
- **Endpoint**: `/api/ForgotPassword/reset-password`
- **Description**: Resets the user's password using a reset token.

## Tender Module

### Get All Tenders
- **Method**: GET
- **Endpoint**: `/api/Tender`
- **Description**: Retrieves a list of all tenders.

### Create a Tender
- **Method**: POST
- **Endpoint**: `/api/Tender`
- **Description**: Creates a new tender.

### Get a Tender by ID
- **Method**: GET
- **Endpoint**: `/api/Tender/{id}`
- **Description**: Retrieves details of a specific tender by its ID.

### Update a Tender
- **Method**: PUT
- **Endpoint**: `/api/Tender/{id}`
- **Description**: Updates the details of a specific tender.

### Open a Tender
- **Method**: PUT
- **Endpoint**: `/api/Tender/{id}/open`
- **Description**: Opens a specific tender for bidding.

### Close a Tender
- **Method**: PUT
- **Endpoint**: `/api/Tender/{id}/close`
- **Description**: Closes a specific tender, preventing further bids.

## User Module

### Register a User
- **Method**: POST
- **Endpoint**: `/api/User/register`
- **Description**: Registers a new user in the system.

### User Login
- **Method**: POST
- **Endpoint**: `/api/User/login`
- **Description**: Authenticates a user and provides access to the system.

---
## 🧪 API Endpoints

### User
| Method | Route                    | Roles   | Description                     |
|--------|--------------------------|---------|---------------------------------|
| POST   | `/api/User/register`     | Public  | Registers a new user.           |
| POST   | `/api/User/login`        | Public  | Authenticates a user.           |

### Tender
| Method | Route                    | Roles               | Description                          |
|--------|--------------------------|---------------------|--------------------------------------|
| POST   | `/api/Tender`            | ProcurementOfficer  | Creates a new tender.                |
| GET    | `/api/Tender`            | All                 | Retrieves a list of all tenders.     |
| GET    | `/api/Tender/{id}`       | All                 | Retrieves details of a tender by ID. |
| PUT    | `/api/Tender/{id}`       | ProcurementOfficer  | Updates a tender's details.          |
| PUT    | `/api/Tender/{id}/open`  | ProcurementOfficer  | Opens a tender for bidding.          |
| PUT    | `/api/Tender/{id}/close` | ProcurementOfficer  | Closes a tender, preventing bids.    |

### Bid
| Method | Route                    | Roles               | Description                          |
|--------|--------------------------|---------------------|--------------------------------------|
| POST   | `/api/Bid/submit`        | Bidder              | Submits a bid for a tender.          |
| GET    | `/api/Bid/tender/{id}`   | ProcurementOfficer  | Retrieves all bids for a tender.     |

### Evaluation
| Method | Route                       | Roles     | Description                          |
|--------|-----------------------------|-----------|--------------------------------------|
| POST   | `/api/Evaluation/{id}/evaluate` | Evaluator | Submits an evaluation for a tender. |

### ForgotPassword
| Method | Route                           | Roles   | Description                          |
|--------|---------------------------------|---------|--------------------------------------|
| POST   | `/api/ForgotPassword/forgot-password` | Public | Initiates a password reset request. |
| POST   | `/api/ForgotPassword/reset-password`  | Public | Resets a user's password.           |

---

## Summary

- **Public**: Accessible to all users (e.g., registration, login, password reset).
- **ProcurementOfficer**: Manages tenders (create, update, open, close, view bids).
- **Bidder**: Submits bids for tenders.
- **Evaluator**: Evaluates tenders.
- **All**: Certain tender information is viewable by all authenticated users.

## 📦 Technology Stack
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger (Swashbuckle)
- BCrypt for password hashing

## 📌 Notes
- Use `Bearer <token>` in Swagger Authorize to test secured endpoints.
- Ensure DB is running and connection string is valid in `appsettings.json`
- wwwroot folder is auto-created if not present

