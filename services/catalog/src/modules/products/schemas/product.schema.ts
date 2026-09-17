import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { HydratedDocument } from 'mongoose';

export type ProductDocument = HydratedDocument<Product>;

@Schema({
  timestamps: true,
  collection: 'products',
})
export class Product {
  @Prop({
    required: true,
    unique: true,
    trim: true,
    uppercase: true,
  })
  sku: string;

  @Prop({
    required: true,
    trim: true,
  })
  name: string;

  @Prop({
    trim: true,
  })
  description?: string;

  @Prop({
    required: true,
    trim: true,
  })
  category: string;

  @Prop({
    required: true,
    min: 0,
  })
  price: number;

  @Prop({
    type: Object,
    default: {},
  })
  attributes: Record<string, string>;

  @Prop({
    default: true,
  })
  isActive: boolean;
}

export const ProductSchema =
  SchemaFactory.createForClass(Product);