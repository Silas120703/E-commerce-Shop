// src/services/auth.service.ts
import axiosClient from './axiosClient';
import type { 
  AuthResponse, 
  LoginRequest, 
  RegisterRequest, 
  VerifyTokenRequest,
  ResendEmailRequest,
  ForgotPasswordRequest,
  ResetPasswordRequest
} from '../types/auth.types';

const AuthService = {
  /**
   * Đăng nhập người dùng
   * Endpoint: POST /api/Auths/login
   */
  login: async (data: LoginRequest) => {
    // Backend trả về AuthResponseDto (AccessToken, RefreshToken)
    const response = await axiosClient.post<AuthResponse>('/Auths/login', data);
    return response.data; 
  },

  /**
   * Đăng ký tài khoản mới
   * Endpoint: POST /api/Auths/register
   */
  register: async (data: RegisterRequest) => {
    const response = await axiosClient.post('/Auths/register', data);
    return response.data;
  },

  /**
   * Xác thực email bằng token
   * Endpoint: POST /api/Auths/verify-email
   */
  verifyEmail: async (data: VerifyTokenRequest) => {
    const response = await axiosClient.post('/Auths/verify-email', data);
    return response.data;
  },

  /**
   * Gửi lại email xác thực
   * Endpoint: POST /api/Auths/resend-verify-email
   */
  resendVerifyEmail: async (data: ResendEmailRequest) => {
    return axiosClient.post('/Auths/resend-verify-email', data);
  },

  /**
   * Yêu cầu quên mật khẩu
   * Endpoint: POST /api/Auths/forgot-password
   */
  forgotPassword: async (data: ForgotPasswordRequest) => {
    return axiosClient.post('/Auths/forgot-password', data);
  },

  /**
   * Xác thực token quên mật khẩu
   * Endpoint: POST /api/Auths/verify-token-forgot-password
   */
  verifyForgotPasswordToken: async (data: VerifyTokenRequest) => {
    // API này trả về chuỗi token reset trong response data
    const response = await axiosClient.post<string>('/Auths/verify-token-forgot-password', data);
    return response.data;
  },

  /**
   * Đặt lại mật khẩu mới
   * Endpoint: POST /api/Auths/reset-password
   */
  resetPassword: async (data: ResetPasswordRequest) => {
    return axiosClient.post('/Auths/reset-password', data);
  },

  /**
   * Helper: Lưu session người dùng (dùng sau khi login/register thành công)
   */
  setSession: (authData: AuthResponse) => {
    localStorage.setItem('accessToken', authData.accessToken);
    localStorage.setItem('refreshToken', authData.refreshToken);
  },

  /**
   * Helper: Đăng xuất (chỉ xóa local, logic gọi API logout nếu backend hỗ trợ)
   */
  logout: () => {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('refreshToken');
    window.location.href = '/login';
  }
};

export default AuthService;