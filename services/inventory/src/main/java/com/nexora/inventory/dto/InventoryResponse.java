package com.nexora.inventory.dto;

import com.nexora.inventory.domain.InventoryItem;

import java.time.Instant;

public record InventoryResponse(
		String sku,
		String productName,
		int quantityOnHand,
		int reservedQuantity,
		int availableQuantity,
		Instant createdAt,
		Instant updatedAt
) {
	public static InventoryResponse from(InventoryItem item) {
		return new InventoryResponse(
				item.getSku(),
				item.getProductName(),
				item.getQuantityOnHand(),
				item.getReservedQuantity(),
				item.getAvailableQuantity(),
				item.getCreatedAt(),
				item.getUpdatedAt()
		);
	}
}
