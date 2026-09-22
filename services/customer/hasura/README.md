# Customer Hasura

Hasura runs on top of the customer PostgreSQL database and exposes GraphQL for the existing customer tables.

The container uses Hasura's `cli-migrations-v3` image, so metadata in this folder is applied automatically at startup.

## Run

From the repository root:

```powershell
docker compose up customer-db customer-hasura
```

Console:

```text
http://localhost:8081
```

Admin secret:

```text
nexora_customer_admin
```

## First Setup

The metadata tracks:

- `public.customers`
- `public.customer_addresses`
- `customers.addresses`
- `customer_addresses.customer`

EF migrations still own the database schema. Start `customer-service` once when you need the .NET application to apply migrations before Hasura tracks/query tables.

The Hasura container connects to Postgres with:

```text
postgres://postgres:postgres@customer-db:5432/customer_db
```

## Sample Query

```graphql
query CustomersWithAddresses {
  customers {
    id
    first_name
    last_name
    email
    phone_number
    created_at
    addresses {
      id
      line1
      city
      country
      postal_code
    }
  }
}
```

## Apply Metadata Manually

If you change files under `metadata/` while Hasura is already running:

```powershell
docker compose exec customer-hasura hasura metadata apply --endpoint http://localhost:8080 --admin-secret nexora_customer_admin
```
