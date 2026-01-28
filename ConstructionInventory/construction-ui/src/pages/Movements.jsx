import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { ArrowLeftRight, Calendar, HardHat } from 'lucide-react';

export default function Movements() {
  const [movements, setMovements] = useState([]);

  useEffect(() => {
    // Backend'deki StockMovementsController'dan veri çekilir
    axios.get('https://localhost:7106/api/StockMovements')
      .then(res => setMovements(res.data))
      .catch(err => console.error(err));
  }, []);

  return (
    <div className="space-y-6 animate-in fade-in duration-500">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-black text-[#111827]">Stok <span className="text-[#1E40AF]">Hareketleri</span></h1>
      </div>

      <div className="bg-white rounded-3xl shadow-xl border border-gray-100 overflow-hidden">
        <table className="w-full text-left">
          <thead className="bg-gray-50 border-b border-gray-100 font-black text-gray-400 uppercase text-xs tracking-widest">
            <tr>
              <th className="px-8 py-5">Tarih</th>
              <th className="px-8 py-5">Malzeme</th>
              <th className="px-8 py-5 text-center">Miktar</th>
              <th className="px-8 py-5">Şantiye</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-50">
            {movements.map(m => (
              <tr key={m.id} className="hover:bg-blue-50/50 transition-colors">
                <td className="px-8 py-6 text-gray-500 font-medium">
                   {new Date(m.date).toLocaleDateString()}
                </td>
                <td className="px-8 py-6 font-bold text-[#111827]">{m.materialName}</td>
                <td className={`px-8 py-6 text-center font-bold ${m.type === 0 ? 'text-green-600' : 'text-red-600'}`}>
                   {m.type === 0 ? '+' : '-'}{m.count}
                </td>
                <td className="px-8 py-6 font-semibold text-gray-600 italic">
                   {m.constructionSiteName}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}