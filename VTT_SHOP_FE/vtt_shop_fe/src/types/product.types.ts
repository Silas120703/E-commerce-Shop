// src/types/product.types.ts

export interface Product {
  id: number;
  name: string;
  slugName: string;
  description: string;
  quantity: number;
  price: number;
  productPictureId: number;
  productPicture: string; 
}