package com.nexora.inventory.controller;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.nexora.inventory.application.InventoryService;
import com.nexora.inventory.dto.CreateInventoryRequest;
import com.nexora.inventory.dto.InventoryResponse;
import com.nexora.inventory.dto.ReserveStockRequest;
import com.nexora.inventory.dto.UpdateStockRequest;
import com.nexora.inventory.exception.InsufficientStockException;
import com.nexora.inventory.exception.InventoryNotFoundException;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.WebMvcTest;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;

import java.util.List;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.patch;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@WebMvcTest(InventoryController.class)
class InventoryControllerTest {

	@Autowired
	private MockMvc mockMvc;

	@Autowired
	private ObjectMapper objectMapper;

	@MockitoBean
	private InventoryService service;

	@Test
	void getsAllInventoryItems() throws Exception {
		when(service.getAll()).thenReturn(List.of(response()));

		mockMvc.perform(get("/api/inventory"))
				.andExpect(status().isOk())
				.andExpect(jsonPath("$[0].sku").value("SKU-1"))
				.andExpect(jsonPath("$[0].availableQuantity").value(7));
	}

	@Test
	void createsInventoryItem() throws Exception {
		when(service.create(any(CreateInventoryRequest.class))).thenReturn(response());

		mockMvc.perform(post("/api/inventory")
						.contentType(MediaType.APPLICATION_JSON)
						.content(objectMapper.writeValueAsString(new CreateInventoryRequest("SKU-1", "Keyboard", 12))))
				.andExpect(status().isCreated())
				.andExpect(jsonPath("$.sku").value("SKU-1"));
	}

	@Test
	void returnsBadRequestForInvalidCreateRequest() throws Exception {
		mockMvc.perform(post("/api/inventory")
						.contentType(MediaType.APPLICATION_JSON)
						.content(objectMapper.writeValueAsString(new CreateInventoryRequest("", "Keyboard", -1))))
				.andExpect(status().isBadRequest())
				.andExpect(jsonPath("$.status").value(400));
	}

	@Test
	void mapsMissingSkuToNotFound() throws Exception {
		when(service.getBySku("missing")).thenThrow(new InventoryNotFoundException("missing"));

		mockMvc.perform(get("/api/inventory/missing"))
				.andExpect(status().isNotFound())
				.andExpect(jsonPath("$.message").value("Inventory item not found for SKU missing"));
	}

	@Test
	void updatesStock() throws Exception {
		when(service.updateStock(any(String.class), any(UpdateStockRequest.class))).thenReturn(response());

		mockMvc.perform(patch("/api/inventory/SKU-1/stock")
						.contentType(MediaType.APPLICATION_JSON)
						.content(objectMapper.writeValueAsString(new UpdateStockRequest(12))))
				.andExpect(status().isOk())
				.andExpect(jsonPath("$.quantityOnHand").value(12));
	}

	@Test
	void mapsInsufficientStockToConflict() throws Exception {
		when(service.reserveStock(any(String.class), any(ReserveStockRequest.class)))
				.thenThrow(new InsufficientStockException("SKU-1"));

		mockMvc.perform(post("/api/inventory/SKU-1/reserve")
						.contentType(MediaType.APPLICATION_JSON)
						.content(objectMapper.writeValueAsString(new ReserveStockRequest(99))))
				.andExpect(status().isConflict())
				.andExpect(jsonPath("$.message").value("Insufficient stock for SKU SKU-1"));
	}

	private InventoryResponse response() {
		return new InventoryResponse("SKU-1", "Keyboard", 12, 5, 7, null, null);
	}
}
