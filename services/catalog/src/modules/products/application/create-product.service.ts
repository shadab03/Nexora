import {
  ConflictException,
  Injectable,
} from '@nestjs/common';

import { CreateProductDto } from '../dto/create-product.dto';
import { ProductsRepository } from '../repositories/products.repository';

export interface CreateProductResult {
  productId: string;
}

@Injectable()
export class CreateProductService {
  constructor(
    private readonly productsRepository: ProductsRepository,
  ) {}

  async execute(
    request: CreateProductDto,
  ): Promise<CreateProductResult> {
    const normalizedSku = request.sku
      .trim()
      .toUpperCase();

    const existingProduct =
      await this.productsRepository.findBySku(normalizedSku);

    if (existingProduct) {
      throw new ConflictException(
        `Product with SKU ${normalizedSku} already exists`,
      );
    }

    const product =
      await this.productsRepository.create({
        sku: normalizedSku,
        name: request.name.trim(),
        description: request.description?.trim(),
        category: request.category.trim(),
        price: request.price,
        attributes: request.attributes ?? {},
        isActive: request.isActive ?? true,
      });

    return {
      productId: product.id,
    };
  }
}