import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { 
  LayoutDashboard, Box, ArrowLeftRight, 
  MapPin, AlertCircle, Plus, RefreshCw, TrendingUp 
} from 'lucide-react';

const API_BASE = 'https://localhost:7106/api';

export default function App() {
  const [activeTab, setActiveTab] = useState('inventory');
  const [materials, setMaterials] = useState([]);
  const [sites, setSites] = useState([]);
  const [loading, setLoading] = useState(true);

  // Verileri paralel çekme (Tüm backend yapılarını kapsar)
  const loadData = async () => {
    setLoading(true);
    try {
      const [matRes, siteRes] = await Promise.all([
        axios.get(`${API_BASE}/Materials`),
        axios.get(`${API_BASE}/ConstructionSites`)
      ]);
      setMaterials(matRes.data);
      setSites(siteRes.data);
    } catch (err) { console.error("Veri hatası:", err); }
    setLoading(false);
  };

  useEffect(() => { loadData(); }, []);

  return (
    <div className="flex min-h-screen bg-[#0b0e14] text-slate-300 font-sans">
      {/* SOL SIDEBAR - METROSIS KURUMSAL */}
      <aside className="w-72 bg-[#151b26] border-r border-white/5 flex flex-col">
        <div className="p-8 border-b border-white/5 flex items-center gap-3 bg-gradient-to-r from-blue-900/20 to-transparent">
          <div className="bg-blue-600 p-2 rounded-lg shadow-lg shadow-blue-500/20 text-white font-black italic">M</div>
          <span className="text-2xl font-black tracking-tighter text-white italic uppercase">METRO<span className="text-red-600 font-normal">SIS</span></span>
        </div>
        
        <nav className="flex-1 p-6 space-y-3">
          <p className="text-[10px] font-bold text-slate-500 uppercase tracking-widest mb-4 ml-4">Yönetim Paneli</p>
          {[
            { id: 'inventory', icon: <Box size={20} />, label: 'Envanter Takibi' },
            { id: 'movements', icon: <ArrowLeftRight size={20} />, label: 'Stok Hareketleri' },
            { id: 'sites', icon: <MapPin size={20} />, label: 'Şantiye Listesi' }
          ].map(item => (
            <button 
              key={item.id}
              onClick={() => setActiveTab(item.id)}
              className={`w-full flex items-center gap-4 px-5 py-3 rounded-2xl transition-all duration-300 font-semibold ${activeTab === item.id ? 'bg-blue-600 text-white shadow-xl shadow-blue-600/20' : 'text-slate-400 hover:bg-slate-800 hover:text-white'}`}
            >
              {item.icon} {item.label}
            </button>
          ))}
        </nav>
      </aside>

      {/* ANA İÇERİK */}
      <main className="flex-1 flex flex-col overflow-hidden">
        {/* TOPBAR */}
        <header className="h-24 border-b border-white/5 px-10 flex justify-between items-center bg-[#151b26]/50 backdrop-blur-xl">
          <div>
            <h2 className="text-sm font-bold text-blue-500 uppercase tracking-widest">Envanter Sistemi</h2>
            <p className="text-2xl font-bold text-white tracking-tight">
              {activeTab === 'inventory' ? 'Genel Stok Durumu' : activeTab === 'movements' ? 'Son Hareketler' : 'Proje Lokasyonları'}
            </p>
          </div>
          <div className="flex gap-4">
            <button onClick={loadData} className="p-3 bg-slate-800 rounded-xl hover:bg-slate-700 transition text-slate-300">
              <RefreshCw size={20} className={loading ? 'animate-spin' : ''} />
            </button>
            <button className="bg-red-600 hover:bg-red-700 text-white px-6 py-3 rounded-xl flex items-center gap-3 font-bold text-sm shadow-lg shadow-red-600/20 transition-all active:scale-95">
              <Plus size={20} /> YENİ KAYIT EKLE
            </button>
          </div>
        </header>

        {/* DASHBOARD İÇERİĞİ */}
        <div className="p-10 overflow-y-auto custom-scrollbar">
          {/* ÖZET KARTLAR (StockDashboardDto) */}
          <div className="grid grid-cols-1 md:grid-cols-4 gap-8 mb-10">
            <SummaryCard title="Toplam Malzeme" value={materials.length} icon={<Box className="text-blue-500" />} />
            <SummaryCard title="Kritik Stok" value={materials.filter(m => m.stockCount < 10).length} icon={<AlertCircle className="text-red-500" />} isAlert />
            <SummaryCard title="Aktif Şantiyeler" value={sites.length} icon={<MapPin className="text-emerald-500" />} />
            <SummaryCard title="Aylık Hareket" value="1.240" icon={<TrendingUp className="text-orange-500" />} />
          </div>

          {/* TABLO - MODERN LİSTELEME */}
          <div className="bg-[#151b26] rounded-3xl border border-white/5 shadow-2xl overflow-hidden">
            <div className="p-8 border-b border-white/5 flex justify-between items-center bg-gradient-to-r from-blue-900/5 to-transparent">
              <h3 className="font-bold text-white text-lg">Envanter Listesi</h3>
              <div className="flex gap-2">
                <span className="w-3 h-3 rounded-full bg-red-500 animate-pulse"></span>
                <span className="text-[10px] font-black text-slate-500 uppercase tracking-tighter">Canlı Veri Akışı</span>
              </div>
            </div>
            
            <table className="w-full text-left">
              <thead className="text-slate-500 text-[10px] font-black uppercase tracking-[0.2em] bg-slate-800/20">
                <tr>
                  <th className="px-10 py-6">Malzeme & Tanım</th>
                  <th className="px-10 py-6">Miktar</th>
                  <th className="px-10 py-6">Birim</th>
                  <th className="px-10 py-6">Stok Durumu</th>
                  <th className="px-10 py-6 text-right">Detay</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-white/5">
                {materials.map(m => (
                  <tr key={m.id} className="group hover:bg-blue-600/[0.03] transition-colors">
                    <td className="px-10 py-5 font-bold text-white group-hover:text-blue-400 transition-colors">{m.name}</td>
                    <td className="px-10 py-5 font-mono text-lg text-slate-300">{m.stockCount}</td>
                    <td className="px-10 py-5 text-slate-500 text-sm font-medium italic">{m.unit}</td>
                    <td className="px-10 py-5">
                      {m.stockCount < 10 
                        ? <span className="px-4 py-1.5 bg-red-500/10 text-red-500 rounded-lg text-[10px] font-black border border-red-500/20">ACİL SİPARİŞ</span>
                        : <span className="px-4 py-1.5 bg-blue-500/10 text-blue-500 rounded-lg text-[10px] font-black border border-blue-500/20">STOK GÜVENLİ</span>
                      }
                    </td>
                    <td className="px-10 py-5 text-right">
                      <button className="p-2 hover:bg-slate-700 rounded-lg text-slate-500 hover:text-white transition">
                        <ArrowLeftRight size={18} />
                      </button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      </main>
    </div>
  );
}

function SummaryCard({ title, value, icon, isAlert }) {
  return (
    <div className={`p-8 rounded-[2rem] bg-[#151b26] border border-white/5 shadow-sm hover:shadow-2xl hover:border-blue-500/20 transition-all duration-500 group relative overflow-hidden`}>
      <div className="flex justify-between items-start relative z-10">
        <div>
          <p className="text-slate-500 text-xs font-black uppercase tracking-widest mb-2 group-hover:text-slate-300">{title}</p>
          <p className={`text-4xl font-black tracking-tighter ${isAlert ? 'text-red-500' : 'text-white'}`}>{value}</p>
        </div>
        <div className="p-4 bg-slate-800/50 rounded-2xl group-hover:scale-110 transition-transform">{icon}</div>
      </div>
      <div className="absolute -right-4 -bottom-4 w-24 h-24 bg-blue-600/5 rounded-full blur-3xl" />
    </div>
  );
}