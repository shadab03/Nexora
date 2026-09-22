# Inventory Module Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Complete `services/inventory` as a Spring Boot inventory REST service.

**Architecture:** REST controller, service business rules, JPA entity/repository, DTO records, exception advice, PostgreSQL config, Docker support.

**Tech Stack:** Java 21, Spring Boot 4, Spring Web MVC, Spring Data JPA, Bean Validation, PostgreSQL, H2 tests, JUnit 5, Mockito.

**Spec:** `docs/superpowers/specs/2026-09-20-inventory-module-design.md`

## Global Constraints

- Preserve the requested package layout under `com.nexora.inventory`.
- Keep DTOs separate from persistence entities.
- Validate input at the API boundary and enforce stock invariants in the service.
- Use PostgreSQL in runtime config and H2 for tests.

## Review Focus

- Duplicate SKUs should not silently create multiple records.
- Reserve requests larger than available stock should return 409.
- Negative stock or reserve quantities should return 400.
- Missing SKUs should return 404.
- Docker Compose should start the inventory service only after its database is healthy.

---

### Task 1: Service Domain Rules

**Files:**
- Create: `services/inventory/src/test/java/com/nexora/inventory/application/InventoryServiceTest.java`
- Create: `services/inventory/src/main/java/com/nexora/inventory/domain/InventoryItem.java`
- Create: `services/inventory/src/main/java/com/nexora/inventory/repository/InventoryRepository.java`
- Create: `services/inventory/src/main/java/com/nexora/inventory/application/InventoryService.java`
- Create DTO and exception classes under requested folders.

**Interfaces:**
- Produces: `InventoryService#create`, `getAll`, `getBySku`, `updateStock`, `reserveStock`.

- [ ] Write failing service tests.
- [ ] Run service tests and confirm they fail because implementation is missing.
- [ ] Implement domain, repository interface, DTOs, exceptions, and service.
- [ ] Run service tests and confirm they pass.

### Task 2: HTTP API And Configuration

**Files:**
- Create: `services/inventory/src/test/java/com/nexora/inventory/controller/InventoryControllerTest.java`
- Create: `services/inventory/src/main/java/com/nexora/inventory/controller/InventoryController.java`
- Create: `services/inventory/src/main/java/com/nexora/inventory/exception/GlobalExceptionHandler.java`
- Modify: `services/inventory/pom.xml`
- Replace: `services/inventory/src/main/resources/application.properties` with `application.yml`
- Create: `services/inventory/Dockerfile`
- Modify: `docker-compose.yml`

**Interfaces:**
- Consumes: `InventoryService` methods from Task 1.
- Produces: REST API described in the spec.

- [ ] Write failing controller tests.
- [ ] Run controller tests and confirm they fail because controller wiring is missing.
- [ ] Implement controller, exception handler, runtime config, Dockerfile, and Compose entries.
- [ ] Run controller tests and full Maven test suite.
