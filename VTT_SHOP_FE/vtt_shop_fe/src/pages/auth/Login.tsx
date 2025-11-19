// src/pages/auth/Login.tsx
import React, { useState } from 'react';
import AuthService from '../../services/auth.service';
import type { LoginRequest } from '../../types/auth.types';
import InputField from '../../components/common/InputField';
// import { useNavigate } from 'react-router-dom'; // Uncomment khi cài react-router

const Login = () => {
  // const navigate = useNavigate();
  const [credential, setCredential] = useState('');
  const [password, setPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setIsLoading(true);

    try {
      const loginData: LoginRequest = {
        credential: credential, // Backend yêu cầu 'credential', không phải 'email'
        password: password
      };

      const data = await AuthService.login(loginData);
      
      // Lưu token bằng helper function trong service
      AuthService.setSession(data);

      alert("Đăng nhập thành công!");
      // navigate('/'); // Chuyển hướng về trang chủ

    } catch (err: any) {
      // Xử lý lỗi từ backend (thường nằm trong err.response.data.Message)
      const errorMessage = err.response?.data?.Message || "Đăng nhập thất bại. Vui lòng thử lại.";
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-100">
      <div className="w-full max-w-md rounded-lg bg-white p-8 shadow-lg">
        <div className="mb-6 text-center">
          <h1 className="text-3xl font-bold text-blue-600">VTT Shop</h1>
          <p className="mt-2 text-sm text-gray-600">Đăng nhập để tiếp tục</p>
        </div>

        {error && (
          <div className="mb-4 rounded bg-red-100 p-3 text-sm text-red-700 border border-red-400">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit}>
          <InputField
            id="credential"
            label="Email hoặc Số điện thoại"
            type="text"
            placeholder="name@example.com"
            value={credential}
            onChange={(e) => setCredential(e.target.value)}
          />
          
          <InputField
            id="password"
            label="Mật khẩu"
            type="password"
            placeholder="••••••••"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />

          <button
            type="submit"
            disabled={isLoading}
            className={`w-full rounded-lg px-4 py-2 font-bold text-white transition duration-200 focus:outline-none focus:ring-4 focus:ring-blue-300 ${
              isLoading ? 'bg-blue-400 cursor-wait' : 'bg-blue-600 hover:bg-blue-700'
            }`}
          >
            {isLoading ? 'Đang xử lý...' : 'Đăng nhập'}
          </button>
        </form>
        
        {/* Footer Links */}
        <div className="mt-6 flex items-center justify-between text-sm">
            <button onClick={() => alert("Tính năng đang phát triển")} className="text-gray-500 hover:text-blue-600">
                Quên mật khẩu?
            </button>
             <button onClick={() => alert("Chuyển sang trang đăng ký")} className="font-semibold text-blue-600 hover:text-blue-800">
                Tạo tài khoản mới
            </button>
        </div>
      </div>
    </div>
  );
};

export default Login;