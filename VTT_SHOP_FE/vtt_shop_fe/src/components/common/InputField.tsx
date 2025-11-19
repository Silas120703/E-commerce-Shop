import React from 'react';

// 1. Định nghĩa kiểu dữ liệu cho Props (Interface)
interface InputFieldProps {
  id: string;
  label: string;
  type: string;
  placeholder?: string; // Dấu ? nghĩa là không bắt buộc
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void; // Kiểu hàm xử lý sự kiện input
}

// 2. Gán kiểu vào Component
const InputField: React.FC<InputFieldProps> = ({ label, type, placeholder, value, onChange, id }) => {
  return (
    <div className="mb-4">
      <label className="mb-2 block text-sm font-medium text-gray-700" htmlFor={id}>
        {label}
      </label>
      <input
        type={type}
        id={id}
        className="w-full rounded-lg border border-gray-300 px-4 py-2 focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-200"
        placeholder={placeholder}
        value={value}
        onChange={onChange}
        required
      />
    </div>
  );
};

export default InputField;