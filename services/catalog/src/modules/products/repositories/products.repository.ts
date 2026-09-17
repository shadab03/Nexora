import { Injectable } from '@nestjs/common';
import { InjectModel } from '@nestjs/mongoose';
import { Model } from 'mongoose';

import {
  Product,
  ProductDocument,
} from '../schemas/product.schema';

@Injectable()
export class ProductsRepository {
  constructor(
    @InjectModel(Product.name)
    private readonly productModel: Model<ProductDocument>,
  ) {}

  async findAll(): Promise<ProductDocument[]> {
    return this.productModel
      .find()
      .exec();
  }

  async findBySku(sku: string): Promise<ProductDocument | null> {
    return this.productModel
      .findOne({
        sku: sku.toUpperCase(),
      })
      .exec();
  }

  async create(product: Partial<Product>): Promise<ProductDocument> {
    const entity = new this.productModel(product);

    return entity.save();
  }
}
