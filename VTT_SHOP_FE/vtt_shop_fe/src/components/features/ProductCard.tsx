// src/components/features/ProductCard.tsx
import type { Product } from '../../types/product.types';
import { ShoppingCart } from 'lucide-react';

// Hàm format tiền VND
const formatCurrency = (amount: number) => {
  return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
};

interface ProductCardProps {
  product: Product;
}

const ProductCard: React.FC<ProductCardProps> = ({ product }) => {
  return (
    <div className="group bg-white rounded-xl border border-gray-100 overflow-hidden hover:shadow-lg transition-all duration-300 hover:-translate-y-1">
      {/* Image Container */}
      <div className="relative h-64 w-full overflow-hidden bg-gray-50">
        <img 
          src={product.productPicture || 'https://via.placeholder.com/300'} 
          alt={product.name} 
          className="w-full h-full object-cover object-center group-hover:scale-105 transition-transform duration-500"
        />
        {/* Quick Action Button */}
        <button className="absolute bottom-4 right-4 bg-white p-3 rounded-full shadow-md translate-y-12 opacity-0 group-hover:translate-y-0 group-hover:opacity-100 transition-all duration-300 hover:bg-blue-600 hover:text-white">
          <ShoppingCart className="w-5 h-5" />
        </button>
      </div>

      {/* Info */}
      <div className="p-5">
        <h3 className="text-lg font-semibold text-gray-800 truncate" title={product.name}>
          {product.name}
        </h3>
        <p className="text-sm text-gray-500 mt-1 line-clamp-2 h-10">
            {product.description || "Sản phẩm chất lượng từ VTT Shop"}
        </p>
        
        <div className="mt-4 flex items-center justify-between">
          <div className="flex flex-col">
            <span className="text-sm text-gray-400 line-through">
              {/* Giả lập giá gốc cao hơn 10% */}
              {formatCurrency(product.price * 1.1)}
            </span>
            <span className="text-xl font-bold text-blue-600">
              {formatCurrency(product.price)}
            </span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductCard;