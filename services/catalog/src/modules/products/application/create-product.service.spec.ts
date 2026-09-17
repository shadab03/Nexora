import { Test, TestingModule } from '@nestjs/testing';
import { CreateProductService } from './create-product.service';
import { ProductsRepository } from '../repositories/products.repository';

describe('CreateProductService', () => {
  let service: CreateProductService;
  let productsRepository: jest.Mocked<ProductsRepository>;

  beforeEach(async () => {
    productsRepository = {
      findBySku: jest.fn(),
      create: jest.fn(),
    } as unknown as jest.Mocked<ProductsRepository>;

    const module: TestingModule = await Test.createTestingModule({
      providers: [
        CreateProductService,
        {
          provide: ProductsRepository,
          useValue: productsRepository,
        },
      ],
    }).compile();

    service = module.get<CreateProductService>(CreateProductService);
  });

  it('normalizes input and returns the created product id', async () => {
    productsRepository.findBySku.mockResolvedValue(null);
    productsRepository.create.mockResolvedValue({
      id: 'product-1',
    } as never);

    await expect(
      service.execute({
        sku: ' sku-1 ',
        name: ' Keyboard ',
        description: ' Mechanical ',
        category: ' Accessories ',
        price: 120,
      }),
    ).resolves.toEqual({
      productId: 'product-1',
    });

    expect(productsRepository.findBySku.mock.calls).toEqual([
      ['SKU-1'],
    ]);
    expect(productsRepository.create.mock.calls).toEqual([
      [
        {
          sku: 'SKU-1',
          name: 'Keyboard',
          description: 'Mechanical',
          category: 'Accessories',
          price: 120,
          attributes: {},
          isActive: true,
        },
      ],
    ]);
  });
});
