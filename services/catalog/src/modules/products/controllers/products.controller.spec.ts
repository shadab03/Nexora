import { Test, TestingModule } from '@nestjs/testing';
import { CreateProductService } from '../application/create-product.service';
import { GetAllProductsService } from '../application/get-all-products.service';
import { GetProductBySkuService } from '../application/get-product-by-sku.service';
import { ProductsController } from './products.controller';

describe('ProductsController', () => {
  let controller: ProductsController;
  let createProductService: jest.Mocked<CreateProductService>;
  let getAllProductsService: jest.Mocked<GetAllProductsService>;
  let getProductBySkuService: jest.Mocked<GetProductBySkuService>;

  beforeEach(async () => {
    createProductService = {
      execute: jest.fn(),
    } as unknown as jest.Mocked<CreateProductService>;
    getAllProductsService = {
      execute: jest.fn(),
    } as unknown as jest.Mocked<GetAllProductsService>;
    getProductBySkuService = {
      execute: jest.fn(),
    } as unknown as jest.Mocked<GetProductBySkuService>;

    const module: TestingModule = await Test.createTestingModule({
      controllers: [ProductsController],
      providers: [
        {
          provide: CreateProductService,
          useValue: createProductService,
        },
        {
          provide: GetAllProductsService,
          useValue: getAllProductsService,
        },
        {
          provide: GetProductBySkuService,
          useValue: getProductBySkuService,
        },
      ],
    }).compile();

    controller = module.get<ProductsController>(ProductsController);
  });

  it('should be defined', () => {
    expect(controller).toBeDefined();
  });

  it('returns all products from the read service', async () => {
    const products = [
      {
        id: 'product-1',
        sku: 'SKU-1',
        name: 'Keyboard',
        category: 'Accessories',
        price: 120,
        attributes: {},
        isActive: true,
      },
    ];
    getAllProductsService.execute.mockResolvedValue(products);

    await expect(controller.getAll()).resolves.toBe(products);
  });

  it('returns a product by sku from the read service', async () => {
    const product = {
      id: 'product-1',
      sku: 'SKU-1',
      name: 'Keyboard',
      category: 'Accessories',
      price: 120,
      attributes: {},
      isActive: true,
    };
    getProductBySkuService.execute.mockResolvedValue(product);

    await expect(controller.getBySku('sku-1')).resolves.toBe(product);
  });
});
