package com.nexora.inventory.repository;

import com.nexora.inventory.domain.InventoryItem;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface InventoryRepository extends JpaRepository<InventoryItem, Long> {

	Optional<InventoryItem> findBySku(String sku);

	boolean existsBySku(String sku);
}
