
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

## 🧪 API Endpoints
### User
| Method | Route | Roles |
|--------|-------|-------|
| POST | /api/User/register | Public |
| POST | /api/User/login | Public |

### Tender
| Method | Route | Roles |
|--------|-------|-------|
| POST | /api/Tender | ProcurementOfficer |
| GET | /api/Tender | All |
| GET | /api/Tender/{id} | All |
| PUT | /api/Tender/{id} | ProcurementOfficer |
| PUT | /api/Tender/{id}/open | ProcurementOfficer |
| PUT | /api/Tender/{id}/close | ProcurementOfficer |

### Bid
| Method | Route | Roles |
|--------|-------|-------|
| POST | /api/Bid/submit | Bidder |
| GET | /api/Bid/tender/{id} | ProcurementOfficer |

### Evaluation
| Method | Route | Roles |
|--------|-------|-------|
| POST | /api/Evaluation/{id}/evaluate | Evaluator |

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

