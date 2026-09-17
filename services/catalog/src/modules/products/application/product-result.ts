export interface ProductResult {
  id: string;
  sku: string;
  name: string;
  description?: string;
  category: string;
  price: number;
  attributes: Record<string, string>;
  isActive: boolean;
}
