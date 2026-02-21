# TotpServer 🔐

Production-ready TOTP (Time-based One-Time Password) authentication server built with ASP.NET Core and SQLite.

Compatible with Google Authenticator, Microsoft Authenticator, Authy, and custom TOTP clients.

---

# 🚀 Features

- TOTP authentication (RFC 6238)
- Google Authenticator compatible
- Multi-user support
- Stateless verification
- SQLite database
- Minimal API architecture
- Production-ready structure
- Secure secret generation

---

# 🧠 How it works

TotpServer uses shared secret cryptography.


Secrets are stored only on the server and client.

---

# ⚙️ Requirements

- .NET 8 SDK

Download:
https://dotnet.microsoft.com/download

---

# ▶️ Running the Server

Restore packages:
dotnet restore

Run:
dotnet run

Server runs at:
http://localhost:5030/

---

# 👤 Register User

Creates new TOTP secret.
curl -X POST "http://localhost:5030/register?username=test

Response:
{
"id": "GUID",
"username": "test",
"secret": "BASE32SECRET",
"uri": "otpauth://..."
}

---

# 🔐 Login with TOTP Code
curl -X POST"http://localhost:5030/login?username=test&code=123456"

Response:
{
"message": "Login success",
"username": "test"
}

---

# 🧪 Verify Code (alternative endpoint)
curl -X POST"http://localhost:5030/verify?userId=GUID&code=123456"

---

# 🗄 Database

SQLite database file:
totp.db

Table:
Users

Schema:

| Field | Type |
|-----|-----|
Id | Guid |
Username | string |
Secret | string |

---

# 🔐 Security Model

Uses:

- HMAC-SHA1
- 30-second rotating codes
- Base32 secrets
- RFC 6238 compliant TOTP

Secrets never transmitted after registration.

---

# 🔢 Example TOTP Code
493812

Expires in 30 seconds.

---

# 📡 API Endpoints

| Endpoint | Method | Description |
|--------|--------|-------------|
| / | GET | Health check |
| /register | POST | Register user |
| /login | POST | Login using username + code |
| /verify | POST | Verify using userId + code |

---

# 🧰 Production Recommendations

Use:

- PostgreSQL instead of SQLite
- HTTPS
- Reverse proxy (nginx)
- Secret encryption
- Rate limiting

---

# 🔐 Cryptography

Algorithm:
TOTP (RFC 6238)
HMAC-SHA1

Code length:
6 digits

Rotation interval:
30 seconds

---

# 🧑‍💻 Example Clients

Compatible with:

- Google Authenticator
- Microsoft Authenticator
- Authy
- Custom TotpClient

---

# 📈 Scalability

Supports:

- unlimited users
- multiple clients
- stateless verification

---

# 📄 License

MIT License

---

# ⭐ Summary

TotpServer provides secure, production-ready TOTP authentication compatible with industry-standard authenticator applications.



