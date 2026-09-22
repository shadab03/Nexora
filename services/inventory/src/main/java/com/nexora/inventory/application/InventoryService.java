package com.nexora.inventory.application;

import com.nexora.inventory.domain.InventoryItem;
import com.nexora.inventory.dto.CreateInventoryRequest;
import com.nexora.inventory.dto.InventoryResponse;
import com.nexora.inventory.dto.ReserveStockRequest;
import com.nexora.inventory.dto.UpdateStockRequest;
import com.nexora.inventory.exception.InsufficientStockException;
import com.nexora.inventory.exception.InventoryNotFoundException;
import com.nexora.inventory.repository.InventoryRepository;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

@Service
public class InventoryService {

	private final InventoryRepository repository;

	public InventoryService(InventoryRepository repository) {
		this.repository = repository;
	}

	@Transactional
	public InventoryResponse create(CreateInventoryRequest request) {
		var sku = InventoryItem.normalizeSku(request.sku());
		if (repository.existsBySku(sku)) {
			throw new IllegalArgumentException("Inventory item already exists for SKU " + sku);
		}
		var item = new InventoryItem(sku, request.productName().trim(), request.quantityOnHand());
		return InventoryResponse.from(repository.save(item));
	}

	@Transactional(readOnly = true)
	public List<InventoryResponse> getAll() {
		return repository.findAll().stream()
				.map(InventoryResponse::from)
				.toList();
	}

	@Transactional(readOnly = true)
	public InventoryResponse getBySku(String sku) {
		return InventoryResponse.from(findBySku(sku));
	}

	@Transactional
	public InventoryResponse updateStock(String sku, UpdateStockRequest request) {
		var item = findBySku(sku);
		item.updateStock(request.quantityOnHand());
		return InventoryResponse.from(repository.save(item));
	}

	@Transactional
	public InventoryResponse reserveStock(String sku, ReserveStockRequest request) {
		var item = findBySku(sku);
		try {
			item.reserve(request.quantity());
		} catch (IllegalStateException exception) {
			throw new InsufficientStockException(item.getSku());
		}
		return InventoryResponse.from(repository.save(item));
	}

	private InventoryItem findBySku(String sku) {
		var normalizedSku = InventoryItem.normalizeSku(sku);
		return repository.findBySku(normalizedSku)
				.orElseThrow(() -> new InventoryNotFoundException(normalizedSku));
	}
}
