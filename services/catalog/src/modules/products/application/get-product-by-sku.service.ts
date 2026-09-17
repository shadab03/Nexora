import {
  Injectable,
  NotFoundException,
} from '@nestjs/common';

import { ProductsRepository } from '../repositories/products.repository';
import { ProductResult } from './product-result';

@Injectable()
export class GetProductBySkuService {
  constructor(
    private readonly productsRepository: ProductsRepository,
  ) {}

  async execute(sku: string): Promise<ProductResult> {
    const normalizedSku = sku
      .trim()
      .toUpperCase();

    const product =
      await this.productsRepository.findBySku(normalizedSku);

    if (!product) {
      throw new NotFoundException(
        `Product with SKU ${normalizedSku} was not found`,
      );
    }

    return {
      id: product.id,
      sku: product.sku,
      name: product.name,
      description: product.description,
      category: product.category,
      price: product.price,
      attributes: product.attributes,
      isActive: product.isActive,
    };
  }
}
