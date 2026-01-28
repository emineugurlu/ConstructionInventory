import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import MainLayout from './layouts/MainLayout';
import Inventory from './pages/Inventory';
import Movements from './pages/Movements'; // Bu dosyayı aşağıda oluşturuyoruz

function App() {
  return (
    <Router>
      <MainLayout>
        <Routes>
          <Route path="/" element={<Navigate to="/inventory" />} />
          <Route path="/inventory" element={<Inventory />} />
          <Route path="/movements" element={<Movements />} />
        </Routes>
      </MainLayout>
    </Router>
  );
}

export default App;