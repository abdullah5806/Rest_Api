# PMS REST API

A beginner-friendly ASP.NET Core API for managing products. It uses an in-memory store, so data resets to the sample products whenever the app restarts.

## Run it

From this folder, run:

```powershell
dotnet run
```

The HTTP profile listens at `http://localhost:5145`. Open `PMS.http` in Visual Studio Code and use **Send Request** to try each endpoint.

## Routes

| Method | Route | Success response |
| --- | --- | --- |
| `GET` | `/api/products` | `200 OK` with the product list |
| `GET` | `/api/products/{id}` | `200 OK` with one product |
| `POST` | `/api/products` | `201 Created` with the product and its `Location` |
| `PUT` | `/api/products/{id}` | `204 No Content` |
| `DELETE` | `/api/products/{id}` | `204 No Content` |

Create and update requests use the same JSON shape:

```json
{
  "name": "Wireless Mouse",
  "description": "A simple wireless mouse",
  "price": 19.99,
  "stockQuantity": 25
}
```

For a successful create, the API responds with `201 Created`, a `Location` header pointing to the new resource, and a product body:

```json
{
  "id": 4,
  "name": "Wireless Mouse",
  "description": "A simple wireless mouse",
  "price": 19.99,
  "stockQuantity": 25
}
```

Names must be 2–100 characters, descriptions are limited to 500 characters, prices must be between 0.01 and 1,000,000, and stock quantities must be between 0 and 1,000,000. Invalid request bodies return `400` validation details; unknown product IDs return `404` problem details.

An unknown ID returns a problem response similar to:

```json
{
  "title": "Product not found",
  "status": 404,
  "detail": "No product with ID 999 exists."
}
```
