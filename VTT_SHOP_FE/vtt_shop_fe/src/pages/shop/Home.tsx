// src/pages/shop/Home.tsx
import { useEffect, useState } from 'react';
import MainLayout from '../../layouts/MainLayout';
import ProductService from '../../services/product.service';
import ProductCard from '../../components/features/ProductCard';
import type { Product } from '../../types/product.types';
import { Loader2 } from 'lucide-react';

const Home = () => {
  const [products, setProducts] = useState<Product[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  // Fetch sản phẩm khi component mount
  useEffect(() => {
    const fetchProducts = async () => {
      try {
        setLoading(true);
        // Gọi API search rỗng để lấy tất cả
        const data = await ProductService.getProducts('');
        setProducts(data);
      } catch (err) {
        console.error("Failed to fetch products", err);
        setError("Không thể tải danh sách sản phẩm. Vui lòng thử lại sau.");
      } finally {
        setLoading(false);
      }
    };

    fetchProducts();
  }, []);

  return (
    <MainLayout>
      {/* 1. Hero Banner Section */}
      <section className="bg-linear-to-r from-blue-600 to-indigo-700 text-white py-20">
        <div className="container mx-auto px-4 flex flex-col md:flex-row items-center justify-between">
          <div className="md:w-1/2 mb-10 md:mb-0">
            <h1 className="text-4xl md:text-6xl font-bold mb-6 leading-tight">
              Mua sắm thông minh <br/> tại <span className="text-yellow-300">VTT Shop</span>
            </h1>
            <p className="text-lg text-blue-100 mb-8 max-w-lg">
              Khám phá hàng ngàn sản phẩm chất lượng cao với mức giá ưu đãi nhất thị trường. Giao hàng nhanh chóng, thanh toán tiện lợi.
            </p>
            <button className="bg-white text-blue-700 font-bold py-3 px-8 rounded-full shadow-lg hover:bg-gray-100 hover:scale-105 transition-all">
              Khám phá ngay
            </button>
          </div>
          <div className="md:w-1/2 flex justify-center">
             {/* Placeholder cho ảnh banner */}
             <div className="w-full max-w-md h-64 bg-white/10 backdrop-blur-md rounded-2xl border border-white/20 shadow-2xl flex items-center justify-center">
                <span className="text-2xl font-bold opacity-80">Banner Image Area</span>
             </div>
          </div>
        </div>
      </section>

      {/* 2. Product List Section */}
      <section className="container mx-auto px-4 py-16">
        <div className="flex items-center justify-between mb-10">
          <h2 className="text-3xl font-bold text-gray-800 relative inline-block">
            Sản phẩm nổi bật
            <span className="absolute bottom-0 left-0 w-1/2 h-1 bg-blue-600 rounded-full"></span>
          </h2>
          <a href="#" className="text-blue-600 font-semibold hover:text-blue-800 hover:underline">
            Xem tất cả &rarr;
          </a>
        </div>

        {/* Logic hiển thị: Loading / Error / Grid */}
        {loading ? (
          <div className="flex justify-center items-center h-64">
            <Loader2 className="w-10 h-10 text-blue-600 animate-spin" />
          </div>
        ) : error ? (
          <div className="text-center text-red-500 py-10 bg-red-50 rounded-lg">
            {error}
          </div>
        ) : (
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-8">
            {products.map((product) => (
              <ProductCard key={product.id} product={product} />
            ))}
          </div>
        )}

        {!loading && products.length === 0 && !error && (
           <div className="text-center text-gray-500 py-10">
             Chưa có sản phẩm nào trong hệ thống.
           </div>
        )}
      </section>
    </MainLayout>
  );
};

export default Home;