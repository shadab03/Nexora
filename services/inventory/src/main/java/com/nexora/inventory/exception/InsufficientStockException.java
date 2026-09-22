package com.nexora.inventory.exception;

public class InsufficientStockException extends RuntimeException {

	public InsufficientStockException(String sku) {
		super("Insufficient stock for SKU " + sku);
	}
}
