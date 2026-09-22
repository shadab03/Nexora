package com.nexora.inventory.application;

import com.nexora.inventory.domain.InventoryItem;
import com.nexora.inventory.dto.CreateInventoryRequest;
import com.nexora.inventory.dto.ReserveStockRequest;
import com.nexora.inventory.dto.UpdateStockRequest;
import com.nexora.inventory.exception.InsufficientStockException;
import com.nexora.inventory.exception.InventoryNotFoundException;
import com.nexora.inventory.repository.InventoryRepository;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.ArgumentCaptor;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import java.util.List;
import java.util.Optional;

import static org.assertj.core.api.Assertions.assertThat;
import static org.assertj.core.api.Assertions.assertThatThrownBy;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

@ExtendWith(MockitoExtension.class)
class InventoryServiceTest {

	@Mock
	private InventoryRepository repository;

	@InjectMocks
	private InventoryService service;

	@Test
	void createsInventoryItemWithTrimmedSkuAndInitialStock() {
		when(repository.existsBySku("SKU-1")).thenReturn(false);
		when(repository.save(any(InventoryItem.class))).thenAnswer(invocation -> invocation.getArgument(0));

		var response = service.create(new CreateInventoryRequest(" SKU-1 ", "Keyboard", 12));

		assertThat(response.sku()).isEqualTo("SKU-1");
		assertThat(response.productName()).isEqualTo("Keyboard");
		assertThat(response.quantityOnHand()).isEqualTo(12);
		assertThat(response.reservedQuantity()).isZero();
		assertThat(response.availableQuantity()).isEqualTo(12);
	}

	@Test
	void rejectsDuplicateSku() {
		when(repository.existsBySku("SKU-1")).thenReturn(true);

		assertThatThrownBy(() -> service.create(new CreateInventoryRequest("SKU-1", "Keyboard", 12)))
				.isInstanceOf(IllegalArgumentException.class)
				.hasMessage("Inventory item already exists for SKU SKU-1");
	}

	@Test
	void returnsAllItems() {
		when(repository.findAll()).thenReturn(List.of(new InventoryItem("SKU-1", "Keyboard", 12)));

		assertThat(service.getAll()).hasSize(1);
	}

	@Test
	void updatesStockForExistingSku() {
		var item = new InventoryItem("SKU-1", "Keyboard", 12);
		when(repository.findBySku("SKU-1")).thenReturn(Optional.of(item));
		when(repository.save(item)).thenReturn(item);

		var response = service.updateStock("SKU-1", new UpdateStockRequest(20));

		assertThat(response.quantityOnHand()).isEqualTo(20);
		verify(repository).save(item);
	}

	@Test
	void reservesAvailableStock() {
		var item = new InventoryItem("SKU-1", "Keyboard", 12);
		when(repository.findBySku("SKU-1")).thenReturn(Optional.of(item));
		when(repository.save(item)).thenReturn(item);

		var response = service.reserveStock("SKU-1", new ReserveStockRequest(5));

		assertThat(response.reservedQuantity()).isEqualTo(5);
		assertThat(response.availableQuantity()).isEqualTo(7);
	}

	@Test
	void rejectsReserveWhenStockIsInsufficient() {
		var item = new InventoryItem("SKU-1", "Keyboard", 4);
		when(repository.findBySku("SKU-1")).thenReturn(Optional.of(item));

		assertThatThrownBy(() -> service.reserveStock("SKU-1", new ReserveStockRequest(5)))
				.isInstanceOf(InsufficientStockException.class)
				.hasMessage("Insufficient stock for SKU SKU-1");
	}

	@Test
	void throwsNotFoundWhenSkuDoesNotExist() {
		when(repository.findBySku("missing")).thenReturn(Optional.empty());

		assertThatThrownBy(() -> service.getBySku("missing"))
				.isInstanceOf(InventoryNotFoundException.class)
				.hasMessage("Inventory item not found for SKU missing");
	}

	@Test
	void persistsReservedQuantityChange() {
		var item = new InventoryItem("SKU-1", "Keyboard", 12);
		when(repository.findBySku("SKU-1")).thenReturn(Optional.of(item));
		when(repository.save(item)).thenReturn(item);

		service.reserveStock("SKU-1", new ReserveStockRequest(3));

		var captor = ArgumentCaptor.forClass(InventoryItem.class);
		verify(repository).save(captor.capture());
		assertThat(captor.getValue().getReservedQuantity()).isEqualTo(3);
	}
}
