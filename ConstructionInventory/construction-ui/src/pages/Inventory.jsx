import { useState, useEffect } from 'react';
import axios from 'axios';
import { AlertCircle, Package, TrendingUp } from 'lucide-react';

export default function Inventory() {
  const [materials, setMaterials] = useState([]);

  useEffect(() => {
    axios.get('https://localhost:7106/api/Materials')
      .then(res => setMaterials(res.data))
      .catch(err => console.error(err));
  }, []);

  return (
    <div className="animate-in fade-in slide-in-from-bottom-4 duration-700">
      <div className="flex justify-between items-end mb-10">
        <div>
          <h1 className="text-4xl font-black text-white tracking-tighter mb-2">Genel <span className="text-blue-500">Envanter</span></h1>
          <p className="text-slate-500 font-medium">Metrosis projeleri genelindeki güncel stok durumları.</p>
        </div>
      </div>

      {/* ÖZET KARTLAR (StockDashboardDto Mantığı) */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-8 mb-12">
        <StatCard title="Toplam Ürün" value={materials.length} icon={<Package className="text-blue-500" />} />
        <StatCard title="Kritik Seviye" value={materials.filter(m => m.stockCount < 10).length} icon={<AlertCircle className="text-red-500" />} isAlert />
        <StatCard title="Aylık Hareket" value="+12.4%" icon={<TrendingUp className="text-emerald-500" />} />
      </div>

      {/* MODERN TABLO */}
      <div className="bg-[#151b26] rounded-[2.5rem] border border-white/5 shadow-2xl overflow-hidden">
        <table className="w-full text-left">
          <thead className="bg-white/5 text-[10px] font-black uppercase tracking-[0.2em] text-slate-500">
            <tr>
              <th className="px-10 py-6">Malzeme Tanımı</th>
              <th className="px-10 py-6 text-center">Stok Miktarı</th>
              <th className="px-10 py-6 text-center">Birim</th>
              <th className="px-10 py-6 text-right">Durum Analizi</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-white/5">
            {materials.map(m => (
              <tr key={m.id} className="hover:bg-blue-600/[0.02] transition-colors group">
                <td className="px-10 py-6 font-bold text-white text-lg group-hover:text-blue-400 transition-colors">{m.name}</td>
                <td className="px-10 py-6 text-center font-mono text-xl text-slate-300">{m.stockCount}</td>
                <td className="px-10 py-6 text-center text-slate-500 font-medium italic">{m.unit}</td>
                <td className="px-10 py-6 text-right">
                  {m.stockCount < 10 
                    ? <span className="bg-red-500/10 text-red-500 px-4 py-2 rounded-xl text-[10px] font-black border border-red-500/20 shadow-[0_0_20px_rgba(239,68,68,0.1)]">KRİTİK - SİPARİŞ VER</span>
                    : <span className="bg-blue-500/10 text-blue-500 px-4 py-2 rounded-xl text-[10px] font-black border border-blue-500/20">STOK GÜVENLİ</span>
                  }
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

function StatCard({ title, value, icon, isAlert }) {
  return (
    <div className="bg-[#151b26] p-8 rounded-[2rem] border border-white/5 relative overflow-hidden group hover:border-blue-500/30 transition-all duration-500 shadow-sm hover:shadow-2xl">
      <div className="relative z-10 flex justify-between items-start">
        <div>
          <p className="text-slate-500 text-[10px] font-black uppercase tracking-widest mb-2">{title}</p>
          <p className={`text-4xl font-black tracking-tighter ${isAlert ? 'text-red-500' : 'text-white'}`}>{value}</p>
        </div>
        <div className="p-4 bg-white/5 rounded-2xl group-hover:scale-110 transition-transform">{icon}</div>
      </div>
      <div className="absolute -right-4 -bottom-4 w-24 h-24 bg-blue-600/5 rounded-full blur-3xl group-hover:bg-blue-600/10 transition-colors" />
    </div>
  );
}