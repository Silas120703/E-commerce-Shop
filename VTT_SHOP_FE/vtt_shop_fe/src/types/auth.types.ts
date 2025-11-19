// src/types/auth.types.ts

// Tương ứng với AuthResponseDto trong Backend
export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
}

// Tương ứng với LoginDTO
export interface LoginRequest {
  credential: string; // Backend đặt tên là Credential (Phone hoặc Email)
  password: string;
}

// Tương ứng với UserCreateDTO
export interface RegisterRequest {
  name: string;
  gender: string;
  birthday?: string; // DateOnly gửi dưới dạng string "YYYY-MM-DD"
  email: string;
  phone: string;
  password: string;
}

// Tương ứng với RefreshTokenDTO
export interface RefreshTokenRequest {
  refreshToken: string;
}

// Tương ứng với VerifyTokenDTO
export interface VerifyTokenRequest {
  token: string;
}

// Tương ứng với ForgotPassword
export interface ForgotPasswordRequest {
  infor: string;
}

// Tương ứng với ResetPasswordDto
export interface ResetPasswordRequest {
  resetToken: string;
  newPassword: string;
}

// Tương ứng với ResendEmail
export interface ResendEmailRequest {
  email: string;
}