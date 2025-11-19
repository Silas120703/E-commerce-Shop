// src/services/product.service.ts
import axiosClient from './axiosClient';
import type { Product } from '../types/product.types';

const ProductService = {
  /**
   * Lấy danh sách sản phẩm (có thể search)
   */
  getProducts: async (keyword: string = '') => {
    // Backend: GET /api/Products/search-product?name=...
    const response = await axiosClient.get<Product[]>('/Products/search-product', {
      params: { name: keyword }
    });
    return response.data;
  },

  /**
   * Lấy chi tiết 1 sản phẩm
   */
  getProductById: async (id: number) => {
    const response = await axiosClient.get<Product>(`/Products/${id}`);
    return response.data;
  }
};

export default ProductService;