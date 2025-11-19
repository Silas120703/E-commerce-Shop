// src/types/common.types.ts

// Interface chung cho mọi phản hồi phân trang
export interface PagedResult<T> {
  items: T[];           // Danh sách dữ liệu (ví dụ: Product[])
  pageIndex: number;    // Trang hiện tại
  pageSize: number;     // Kích thước trang
  totalCount: number;   // Tổng số bản ghi trong DB
  totalPages: number;   // Tổng số trang
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}