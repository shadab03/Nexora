package com.nexora.inventory.dto;

import jakarta.validation.constraints.Min;

public record UpdateStockRequest(@Min(0) int quantityOnHand) {
}
