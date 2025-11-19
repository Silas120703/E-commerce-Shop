// src/layouts/MainLayout.tsx
import Header from '../components/layout/Header';

interface MainLayoutProps {
  children: React.ReactNode;
}

const MainLayout: React.FC<MainLayoutProps> = ({ children }) => {
  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <Header />
      <main className="grow">
        {children}
      </main>
      
      {/* Footer đơn giản */}
      <footer className="bg-white border-t border-gray-200 py-8 mt-12">
        <div className="container mx-auto px-4 text-center text-gray-500">
          <p>&copy; 2024 VTT Shop. All rights reserved.</p>
        </div>
      </footer>
    </div>
  );
};

export default MainLayout;