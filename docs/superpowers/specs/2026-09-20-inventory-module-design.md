# Inventory Module Design

## Goal

Build `services/inventory` into a working Spring Boot inventory service that can create inventory records, read them, update stock, and reserve stock for order flows.

## Architecture

The service uses a conventional Spring Boot layered structure. `InventoryController` exposes REST endpoints, `InventoryService` owns business rules, `InventoryRepository` persists `InventoryItem` entities with Spring Data JPA, and DTO records define the API contract.

PostgreSQL is the production datastore. Tests use H2 so business behavior can be verified without external services.

## API

- `GET /api/inventory` returns all inventory items.
- `GET /api/inventory/{sku}` returns one item by SKU.
- `POST /api/inventory` creates an item with SKU, product name, and initial quantity.
- `PATCH /api/inventory/{sku}/stock` replaces the on-hand quantity.
- `POST /api/inventory/{sku}/reserve` reserves a requested quantity.

## Rules

- SKU is unique and normalized by trimming whitespace.
- Product name must not be blank.
- Quantities must be zero or greater.
- Reserving stock decreases available stock by increasing `reservedQuantity`; it never makes available stock negative.
- Missing SKUs return HTTP 404.
- Insufficient stock returns HTTP 409.
- Validation errors return HTTP 400.

## Testing

Unit tests cover service business rules. Controller tests cover HTTP status mapping and JSON shape. The full Maven test suite is the completion check.
