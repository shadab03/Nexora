package com.nexora.inventory.dto;

import jakarta.validation.constraints.Min;
import jakarta.validation.constraints.NotBlank;

public record CreateInventoryRequest(
		@NotBlank String sku,
		@NotBlank String productName,
		@Min(0) int quantityOnHand
) {
}
