// src/components/layout/Header.tsx
import { ShoppingCart, Search, User, LogOut } from 'lucide-react';
import { useState } from 'react';
import AuthService from '../../services/auth.service';

const Header = () => {
  const [isUserMenuOpen, setIsUserMenuOpen] = useState(false);
  // Giả lập số lượng trong giỏ hàng
  const cartItemCount = 2; 

  return (
    <header className="sticky top-0 z-50 w-full bg-white shadow-sm border-b border-gray-200">
      <div className="container mx-auto px-4 h-16 flex items-center justify-between">
        
        {/* 1. Logo */}
        <div className="flex items-center gap-2 cursor-pointer">
          <div className="w-8 h-8 bg-blue-600 rounded-lg flex items-center justify-center">
            <span className="text-white font-bold text-xl">V</span>
          </div>
          <span className="text-xl font-bold text-gray-800 tracking-tight">VTT Shop</span>
        </div>

        {/* 2. Search Bar (Ẩn trên mobile nhỏ) */}
        <div className="hidden md:flex flex-1 max-w-md mx-8 relative">
          <input 
            type="text" 
            placeholder="Tìm kiếm sản phẩm..." 
            className="w-full pl-10 pr-4 py-2 rounded-full border border-gray-300 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition-all"
          />
          <Search className="absolute left-3 top-2.5 text-gray-400 w-5 h-5" />
        </div>

        {/* 3. Right Actions */}
        <div className="flex items-center gap-6">
          {/* Cart */}
          <div className="relative cursor-pointer hover:text-blue-600 transition-colors">
            <ShoppingCart className="w-6 h-6 text-gray-700" />
            {cartItemCount > 0 && (
              <span className="absolute -top-2 -right-2 bg-red-500 text-white text-xs font-bold w-5 h-5 flex items-center justify-center rounded-full">
                {cartItemCount}
              </span>
            )}
          </div>

          {/* User Menu */}
          <div className="relative">
            <button 
              onClick={() => setIsUserMenuOpen(!isUserMenuOpen)}
              className="flex items-center gap-2 hover:bg-gray-100 p-1.5 rounded-full transition-all"
            >
              <div className="w-8 h-8 bg-gray-200 rounded-full flex items-center justify-center overflow-hidden">
                 <User className="w-5 h-5 text-gray-600" />
                 {/* Nếu có avatar thì dùng thẻ img ở đây */}
              </div>
            </button>

            {/* Dropdown */}
            {isUserMenuOpen && (
              <div className="absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-lg border border-gray-100 py-1 animate-in fade-in zoom-in duration-200">
                <div className="px-4 py-2 border-b border-gray-100">
                  <p className="text-sm font-semibold text-gray-800">Xin chào, User</p>
                </div>
                <button className="w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-50 flex items-center gap-2">
                  <User className="w-4 h-4" /> Hồ sơ
                </button>
                <button 
                  onClick={AuthService.logout}
                  className="w-full text-left px-4 py-2 text-sm text-red-600 hover:bg-red-50 flex items-center gap-2"
                >
                  <LogOut className="w-4 h-4" /> Đăng xuất
                </button>
              </div>
            )}
          </div>
        </div>
      </div>
    </header>
  );
};

export default Header;