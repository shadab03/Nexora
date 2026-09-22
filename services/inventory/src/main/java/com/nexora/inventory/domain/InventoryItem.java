package com.nexora.inventory.domain;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.PrePersist;
import jakarta.persistence.PreUpdate;
import jakarta.persistence.Table;

import java.time.Instant;

@Entity
@Table(name = "inventory_items")
public class InventoryItem {

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	private Long id;

	@Column(nullable = false, unique = true, length = 80)
	private String sku;

	@Column(nullable = false, length = 200)
	private String productName;

	@Column(nullable = false)
	private int quantityOnHand;

	@Column(nullable = false)
	private int reservedQuantity;

	@Column(nullable = false, updatable = false)
	private Instant createdAt;

	@Column(nullable = false)
	private Instant updatedAt;

	protected InventoryItem() {
	}

	public InventoryItem(String sku, String productName, int quantityOnHand) {
		if (quantityOnHand < 0) {
			throw new IllegalArgumentException("Quantity on hand cannot be negative");
		}
		this.sku = normalizeSku(sku);
		this.productName = productName;
		this.quantityOnHand = quantityOnHand;
		this.reservedQuantity = 0;
	}

	public Long getId() {
		return id;
	}

	public String getSku() {
		return sku;
	}

	public String getProductName() {
		return productName;
	}

	public int getQuantityOnHand() {
		return quantityOnHand;
	}

	public int getReservedQuantity() {
		return reservedQuantity;
	}

	public int getAvailableQuantity() {
		return quantityOnHand - reservedQuantity;
	}

	public Instant getCreatedAt() {
		return createdAt;
	}

	public Instant getUpdatedAt() {
		return updatedAt;
	}

	public void updateStock(int quantityOnHand) {
		if (quantityOnHand < 0) {
			throw new IllegalArgumentException("Quantity on hand cannot be negative");
		}
		if (quantityOnHand < reservedQuantity) {
			throw new IllegalArgumentException("Quantity on hand cannot be less than reserved quantity");
		}
		this.quantityOnHand = quantityOnHand;
	}

	public void reserve(int quantity) {
		if (quantity <= 0) {
			throw new IllegalArgumentException("Reserve quantity must be greater than zero");
		}
		if (quantity > getAvailableQuantity()) {
			throw new IllegalStateException("Insufficient stock");
		}
		this.reservedQuantity += quantity;
	}

	@PrePersist
	void onCreate() {
		var now = Instant.now();
		createdAt = now;
		updatedAt = now;
	}

	@PreUpdate
	void onUpdate() {
		updatedAt = Instant.now();
	}

	public static String normalizeSku(String sku) {
		if (sku == null) {
			throw new IllegalArgumentException("SKU is required");
		}
		var normalized = sku.trim();
		if (normalized.isEmpty()) {
			throw new IllegalArgumentException("SKU is required");
		}
		return normalized;
	}
}
