import {
  Body,
  Controller,
  Get,
  HttpCode,
  HttpStatus,
  Param,
  Post,
} from '@nestjs/common';

import {
  ApiCreatedResponse,
  ApiNotFoundResponse,
  ApiOkResponse,
  ApiOperation,
  ApiTags,
} from '@nestjs/swagger';

import { CreateProductDto } from '../dto/create-product.dto';
import {
  CreateProductResult,
  CreateProductService,
} from '../application/create-product.service';
import { GetAllProductsService } from '../application/get-all-products.service';
import { GetProductBySkuService } from '../application/get-product-by-sku.service';
import { ProductResult } from '../application/product-result';

@ApiTags('Products')
@Controller('api/products')
export class ProductsController {
  constructor(
    private readonly createProductService: CreateProductService,
    private readonly getAllProductsService: GetAllProductsService,
    private readonly getProductBySkuService: GetProductBySkuService,
  ) {}

  @Get()
  @ApiOperation({
    summary: 'Get all products',
  })
  @ApiOkResponse({
    description: 'Products returned successfully',
  })
  async getAll(): Promise<ProductResult[]> {
    return this.getAllProductsService.execute();
  }

  @Get(':sku')
  @ApiOperation({
    summary: 'Get product by SKU',
  })
  @ApiOkResponse({
    description: 'Product returned successfully',
  })
  @ApiNotFoundResponse({
    description: 'Product was not found',
  })
  async getBySku(
    @Param('sku') sku: string,
  ): Promise<ProductResult> {
    return this.getProductBySkuService.execute(sku);
  }

  @Post()
  @HttpCode(HttpStatus.CREATED)
  @ApiOperation({
    summary: 'Create product',
  })
  @ApiCreatedResponse({
    description: 'Product created successfully',
  })
  async create(
    @Body() request: CreateProductDto,
  ): Promise<CreateProductResult> {
    return this.createProductService.execute(request);
  }
}
