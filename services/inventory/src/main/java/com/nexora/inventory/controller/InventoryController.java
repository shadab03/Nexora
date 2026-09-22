package com.nexora.inventory.controller;

import com.nexora.inventory.application.InventoryService;
import com.nexora.inventory.dto.CreateInventoryRequest;
import com.nexora.inventory.dto.InventoryResponse;
import com.nexora.inventory.dto.ReserveStockRequest;
import com.nexora.inventory.dto.UpdateStockRequest;
import jakarta.validation.Valid;
import org.springframework.http.HttpStatus;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseStatus;
import org.springframework.web.bind.annotation.RestController;

import java.util.List;

@RestController
@RequestMapping("/api/inventory")
public class InventoryController {

	private final InventoryService service;

	public InventoryController(InventoryService service) {
		this.service = service;
	}

	@GetMapping
	public List<InventoryResponse> getAll() {
		return service.getAll();
	}

	@GetMapping("/{sku}")
	public InventoryResponse getBySku(@PathVariable String sku) {
		return service.getBySku(sku);
	}

	@PostMapping
	@ResponseStatus(HttpStatus.CREATED)
	public InventoryResponse create(@Valid @RequestBody CreateInventoryRequest request) {
		return service.create(request);
	}

	@PatchMapping("/{sku}/stock")
	public InventoryResponse updateStock(
			@PathVariable String sku,
			@Valid @RequestBody UpdateStockRequest request) {
		return service.updateStock(sku, request);
	}

	@PostMapping("/{sku}/reserve")
	public InventoryResponse reserveStock(
			@PathVariable String sku,
			@Valid @RequestBody ReserveStockRequest request) {
		return service.reserveStock(sku, request);
	}
}
