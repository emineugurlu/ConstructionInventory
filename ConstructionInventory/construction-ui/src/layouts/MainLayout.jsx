import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { LayoutDashboard, History, HardHat } from 'lucide-react';

export default function MainLayout({ children }) {
  const location = useLocation();

  const menu = [
    { name: 'Envanter', path: '/inventory', icon: <LayoutDashboard size={20}/> },
    { name: 'Stok Hareketleri', path: '/movements', icon: <History size={20}/> }
  ];

  return (
    // Arka plan rengi: #F3F4F6 (açık gri)
    <div className="flex h-screen bg-[#F3F4F6] overflow-hidden font-sans selection:bg-blue-100">
      
      {/* Sidebar: Ana renk #1E40AF (mavi) */}
      <aside className="w-64 bg-[#1E40AF] text-white flex flex-col shadow-2xl z-10">
        <div className="p-8 flex items-center gap-3 border-b border-blue-700/50">
          <div className="bg-white p-2 rounded-lg text-[#1E40AF] font-bold italic shadow-md">M</div>
          <span className="text-xl font-black tracking-tight italic">METRO<span className="text-red-500">SIS</span></span>
        </div>

        <nav className="flex-1 p-4 mt-4 space-y-2">
          {menu.map((item) => (
            <Link
              key={item.path}
              to={item.path}
              className={`flex items-center gap-4 px-6 py-4 rounded-xl font-bold transition-all ${
                location.pathname === item.path 
                ? 'bg-white/10 shadow-inner' 
                : 'opacity-70 hover:opacity-100 hover:bg-white/5'
              }`}
            >
              {item.icon} {item.name}
            </Link>
          ))}
        </nav>
      </aside>

      {/* Main Content Area */}
      <div className="flex-1 flex flex-col relative overflow-hidden">
        <header className="h-16 bg-white border-b border-gray-200 flex items-center justify-between px-10 shadow-sm">
          <span className="text-xs font-black text-gray-400 uppercase tracking-widest">Merkezi Yönetim</span>
          <div className="flex items-center gap-2">
             <div className="w-2 h-2 bg-green-500 rounded-full animate-pulse"></div>
             <span className="text-[10px] font-bold text-gray-500">SİSTEM ÇEVRİMİÇİ</span>
          </div>
        </header>

        {/* Sayfa İçeriği: Gereksiz boşlukları kaldıran tam ekran akış */}
        <main className="flex-1 overflow-y-auto p-10 custom-scrollbar">
          <div className="max-w-7xl mx-auto animate-in fade-in duration-700">
            {children}
          </div>
        </main>
      </div>
    </div>
  );
}