import { Module } from '@nestjs/common';
import { MongooseModule } from '@nestjs/mongoose';

import {
  Product,
  ProductSchema,
} from './schemas/product.schema';

import { ProductsController } from './controllers/products.controller';
import { ProductsRepository } from './repositories/products.repository';
import { CreateProductService } from './application/create-product.service';
import { GetAllProductsService } from './application/get-all-products.service';
import { GetProductBySkuService } from './application/get-product-by-sku.service';

@Module({
  imports: [
    MongooseModule.forFeature([
      {
        name: Product.name,
        schema: ProductSchema,
      },
    ]),
  ],

  controllers: [
    ProductsController,
  ],

  providers: [
    ProductsRepository,
    CreateProductService,
    GetAllProductsService,
    GetProductBySkuService,
  ],
})
export class ProductsModule {}
