import { Injectable } from '@nestjs/common';

import { ProductsRepository } from '../repositories/products.repository';
import { ProductResult } from './product-result';

@Injectable()
export class GetAllProductsService {
  constructor(
    private readonly productsRepository: ProductsRepository,
  ) {}

  async execute(): Promise<ProductResult[]> {
    const products = await this.productsRepository.findAll();

    return products.map((product) => ({
      id: product.id,
      sku: product.sku,
      name: product.name,
      description: product.description,
      category: product.category,
      price: product.price,
      attributes: product.attributes,
      isActive: product.isActive,
    }));
  }
}
