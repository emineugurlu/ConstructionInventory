import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import MainLayout from './layouts/MainLayout';
import Inventory from './pages/Inventory';

// Henüz oluşturmadığımız sayfalar için geçici bileşenler
const ConstructionSites = () => <div className="text-white p-10 font-bold">Şantiye Yönetimi - Yakında</div>;
const StockMovements = () => <div className="text-white p-10 font-bold">Stok Hareket Geçmişi - Yakında</div>;

function App() {
  return (
    <Router>
      {/* MainLayout sayesinde sidebar ve header tüm sayfalarda sabit kalır */}
      <MainLayout>
        <Routes>
          {/* Ana sayfa açıldığında direkt envantere yönlendirir */}
          <Route path="/" element={<Navigate to="/inventory" />} />
          
          {/* Backend'deki Materials verilerini işleyen sayfa */}
          <Route path="/inventory" element={<Inventory />} />
          
          {/* Backend'deki ConstructionSites verilerini işleyen sayfa */}
          <Route path="/sites" element={<ConstructionSites />} />
          
          {/* Backend'deki StockMovements verilerini işleyen sayfa */}
          <Route path="/movements" element={<StockMovements />} />
        </Routes>
      </MainLayout>
    </Router>
  );
}

export default App;