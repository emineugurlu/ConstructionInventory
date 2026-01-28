import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Search, AlertCircle } from 'lucide-react';

export default function Inventory() {
  const [materials, setMaterials] = useState([]);
  const [search, setSearch] = useState("");

  useEffect(() => {
    // Backend verisini çekiyoruz
    axios.get('https://localhost:7106/api/Materials')
      .then(res => setMaterials(res.data))
      .catch(err => console.error(err));
  }, []);

  const filtered = materials.filter(m => m.name.toLowerCase().includes(search.toLowerCase()));

  return (
    <div className="space-y-8">
      <div className="flex justify-between items-end">
        <div>
          <h1 className="text-3xl font-black text-[#111827]">Malzeme <span className="text-[#1E40AF]">Listesi</span></h1>
          <p className="text-gray-500 text-sm mt-1">Metrosis güncel stok verileri.</p>
        </div>
        
        {/* Arama Input: Hover ve Fokus efektli */}
        <div className="relative">
          <Search className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400" size={18} />
          <input 
            type="text"
            placeholder="Malzeme ara..."
            className="pl-12 pr-6 py-3 bg-white border border-gray-200 rounded-2xl w-80 shadow-sm focus:ring-2 focus:ring-[#1E40AF] outline-none transition-all"
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>
      </div>

      {/* Tablo Tasarımı: Rounded corners ve Shadow */}
      <div className="bg-white rounded-3xl shadow-xl border border-gray-100 overflow-hidden">
        <table className="w-full text-left">
          <thead className="bg-gray-50 border-b border-gray-100">
            <tr>
              <th className="px-8 py-5 text-xs font-black text-gray-400 uppercase tracking-widest">Malzeme Adı</th>
              <th className="px-8 py-5 text-xs font-black text-gray-400 uppercase tracking-widest">Mevcut Stok</th>
              <th className="px-8 py-5 text-xs font-black text-gray-400 uppercase tracking-widest">Durum</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-50 text-[#111827]">
            {filtered.map(m => (
              <tr key={m.id} className="hover:bg-blue-50/30 transition-colors group">
                <td className="px-8 py-6 font-bold">{m.name}</td>
                <td className="px-8 py-6 font-mono font-bold text-[#1E40AF]">{m.stockCount} {m.unit}</td>
                <td className="px-8 py-6">
                  {/* Kritik stok: #DC2626 (kırmızı) */}
                  {m.stockCount < 10 ? (
                    <span className="bg-red-50 text-red-600 px-4 py-2 rounded-xl text-[10px] font-black border border-red-100 flex items-center gap-2 w-fit">
                      <AlertCircle size={14}/> KRİTİK SEVİYE
                    </span>
                  ) : (
                    <span className="bg-emerald-50 text-emerald-600 px-4 py-2 rounded-xl text-[10px] font-black border border-emerald-100 w-fit block">
                      STOK GÜVENLİ
                    </span>
                  )}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}